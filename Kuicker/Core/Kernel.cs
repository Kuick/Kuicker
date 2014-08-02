// Kernel.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using Kuicker.Core;

namespace Kuicker
{
	public sealed class Kernel : IDisposable
	{
		#region field
		private DateTime StartUpTime;
		private string InstanceID;
		private static object _BlockLock = new object();
		private static object _PieceLock = new object();
		private static Kernel _Current;
		internal static ReadOnlyDictionary<string, ILifeCycle> _LifeCycles;
		internal static ReadOnlyDictionary<string, IBuiltin> _Builtins;
		internal static ReadOnlyDictionary<string, IPlugin> _Plugins;

		private IKLifeCycle _KernelLifeCycle;
		private IEnumerable<ILifeCycle> _OtherLifeCycles;
		#endregion

		#region constructor
		private Kernel()
		{
			this.StartUpTime = DateTime.Now;
			this.InstanceID = Guid.NewGuid().ToString();
			this.Register();
		}
		#endregion

		#region IDisposable
		public void Dispose()
		{
			this.Stop();
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Singleton
		public static Kernel Current
		{
			get
			{
				if(null == _Current) {
					lock(_BlockLock) {
						if(null == _Current) {
							_Current = new Kernel();
							if(_Current.Start()) {
								// Success
							} else {
								// Failure
								if(_Current.Stop()) {
									// Success
									_Current = null;
								} else {
									// Failure
								}
							}
						}
					}
				}
				return _Current;
			}
		}
		#endregion

		#region property
		public KernelStatus Status { get; private set; }

		public bool InConstancy
		{
			get
			{
				return Status.In(
					KernelStatus.Stopped,
					KernelStatus.Running,
					KernelStatus.Suspended,
					KernelStatus.Sink
				);
			}
		}

		public bool InTransition
		{
			get
			{
				return !InConstancy;
			}
		}
		#endregion

		#region Action
		private bool Start()
		{
			if(KernelStatus.Stopped != Status) { return false; }

			lock(_BlockLock) {
				if(KernelStatus.Stopped != Status) { return false; }
				using(var il = new ILogger()) {
					try {
						InvokeStart(il);
						Status = KernelStatus.Running;
						return true;
					} catch(Exception ex) {
						il.Record.Level = LogLevel.Fatal;
						il.Record.Add("KernelStatus", Status);
						il.Record.Add(ex);
						Status = KernelStatus.Sink;
						return false;
					}
				}
			}
		}

		private bool Stop()
		{
			if(KernelStatus.Running != Status) { return false; }

			lock(_BlockLock) {
				if(KernelStatus.Running != Status) { return false; }
				using(var il = new ILogger()) {
					try {
						InvokeStop(il);
						Status = KernelStatus.Stopped;
						return true;
					} catch(Exception ex) {
						il.Record.Level = LogLevel.Fatal;
						il.Record.Add("KernelStatus", Status);
						il.Record.Add(ex);
						Status = KernelStatus.Sink;
						return false;
					}
				}
			}
		}

		public bool Suspend()
		{
			if(KernelStatus.Running != Status) { return false; }

			lock(_BlockLock) {
				if(KernelStatus.Running != Status) { return false; }
				using(var il = new ILogger()) {
					try {
						InvokeSuspend(il);
						Status = KernelStatus.Suspended;
						return true;
					} catch(Exception ex) {
						il.Record.Level = LogLevel.Fatal;
						il.Record.Add("KernelStatus", Status);
						il.Record.Add(ex);
						Status = KernelStatus.Sink;
						return false;
					}
				}
			}
		}

		public bool Resume()
		{
			if(KernelStatus.Suspended != Status) { return false; }

			lock(_BlockLock) {
				if(KernelStatus.Suspended != Status) { return false; }
				using(var il = new ILogger()) {
					try {
						InvokeResume(il);
						Status = KernelStatus.Running;
						return true;
					} catch(Exception ex) {
						il.Record.Level = LogLevel.Fatal;
						il.Record.Add("KernelStatus", Status);
						il.Record.Add(ex);
						Status = KernelStatus.Sink;
						return false;
					}
				}
			}
		}

		public bool RestartBuiltin()
		{
			foreach(var builtin in _Builtins) {
				if(!RestartBuiltin(builtin.Value, -1)){
					return false;
				}
			}
			return true;
		}

		public bool RestartBuiltin<T>()
			where T : IBuiltin
		{
			return RestartBuiltin<T>(-1);
		}

		public bool RestartBuiltin<T>(int millisecondsTimeout)
			where T : IBuiltin
		{
			IBuiltin b = GetBuiltin<T>();
			return RestartBuiltin(b, millisecondsTimeout);
		}

		private bool RestartBuiltin(IBuiltin builtin)
		{
			return RestartBuiltin(builtin , - 1);
		}

		private bool RestartBuiltin(
			IBuiltin builtin, int millisecondsTimeout)
		{
			if(null == builtin) { return false; }

			var stop = builtin.Stop();
			if(stop.Wait(millisecondsTimeout)) {
				var start = builtin.Start();
				if(start.Wait(millisecondsTimeout)) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}



		public bool RestartPlugin()
		{
			foreach(var plugin in _Plugins) {
				if(!RestartPlugin(plugin.Value, -1)) {
					return false;
				}
			}
			return true;
		}

		public bool RestartPlugin<T>()
			where T : IPlugin
		{
			return RestartPlugin<T>(-1);
		}

		public bool RestartPlugin<T>(int millisecondsTimeout)
			where T : IPlugin
		{
			IPlugin p = GetPlugin<T>();
			return RestartPlugin(p, millisecondsTimeout);
		}

		private bool RestartPlugin(IPlugin plugin)
		{
			return RestartPlugin(plugin, -1);
		}

		private bool RestartPlugin(
			IPlugin plugin, int millisecondsTimeout)
		{
			if(null == plugin) { return false; }

			var stop = plugin.Stop();
			if(stop.Wait(millisecondsTimeout)) {
				var start = plugin.Start();
				if(start.Wait(millisecondsTimeout)) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}
		#endregion

		#region Invoke
		private void InvokeStart(IntervalLogger il)
		{
			// BeforeStart
			Status = KernelStatus.BeforeStart;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoBeforeStart();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoBeforeStart()
				);
			}

			// BeforeBuiltinStart
			Status = KernelStatus.BeforeBuiltinStart;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoBeforeBuiltinStart();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoBeforeBuiltinStart()
				);
			}

			// BuiltinStart
			Status = KernelStatus.BuiltinStart;
			using(var iil = new IILogger(il, Status)) {
				// Only Kernel
				_KernelLifeCycle.DoBuiltinStart();
			}

			// AfterBuiltinStart
			Status = KernelStatus.AfterBuiltinStart;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoAfterBuiltinStart();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoAfterBuiltinStart()
				);
			}

			// BeforePluginStart
			Status = KernelStatus.BeforePluginStart;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoBeforePluginStart();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoBeforePluginStart()
				);
			}

			// PluginStart
			Status = KernelStatus.PluginStart;
			using(var iil = new IILogger(il, Status)) {
				// Only Kernel
				_KernelLifeCycle.DoPluginStart();
			}

			// AfterPluginStart
			Status = KernelStatus.AfterPluginStart;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoAfterPluginStart();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoAfterPluginStart()
				);
			}

			// AfterStart
			Status = KernelStatus.AfterStart;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoAfterStart();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoAfterStart()
				);
			}
		}

		private void InvokeStop(IntervalLogger il)
		{
			// BeforeStop
			Status = KernelStatus.BeforeStop;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoBeforeStop();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoBeforeStop()
				);
			}

			// PluginStop
			Status = KernelStatus.PluginStop;
			using(var iil = new IILogger(il, Status)) {
				// Only Kernel
				_KernelLifeCycle.DoPluginStop();
			}

			// BuiltinStop
			Status = KernelStatus.BuiltinStop;
			using(var iil = new IILogger(il, Status)) {
				// Only Kernel
				_KernelLifeCycle.DoBuiltinStop();
			}

			// AfterStop
			Status = KernelStatus.AfterStop;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoAfterStop();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoAfterStop()
				);
			}
		}

		private void InvokeSuspend(IntervalLogger il)
		{
			// BeforePluginSuspend
			Status = KernelStatus.BeforePluginSuspend;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoBeforePluginSuspend();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoBeforePluginSuspend()
				);
			}

			// PluginSuspend
			Status = KernelStatus.PluginSuspend;
			using(var iil = new IILogger(il, Status)) {
				// Only Kernel
				_KernelLifeCycle.DoPluginSuspend();
			}

			// AfterPluginSuspend
			Status = KernelStatus.AfterPluginSuspend;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoAfterPluginSuspend();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoAfterPluginSuspend()
				);
			}
		}

		private void InvokeResume(IntervalLogger il)
		{
			// BeforePluginSuspend
			Status = KernelStatus.BeforePluginResume;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoBeforePluginResume();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoBeforePluginResume()
				);
			}

			// PluginResume
			Status = KernelStatus.PluginResume;
			using(var iil = new IILogger(il, Status)) {
				// Only Kernel
				_KernelLifeCycle.DoPluginResume();
			}

			// AfterPluginSuspend
			Status = KernelStatus.AfterPluginResume;
			using(var iil = new IILogger(il, Status)) {
				// Kernel First
				_KernelLifeCycle.DoAfterPluginResume();
				Parallel.ForEach(
					_OtherLifeCycles, x => x.DoAfterPluginResume()
				);
			}
		}
		#endregion

		#region Builtin
		public static T GetBuiltin<T>() where T : IBuiltin
		{
			T builtin = default(T);
			if(null != _Builtins) {
				lock(_PieceLock) {
					if(null != _Builtins) {
						var one = _Builtins
							.Values
							.FirstOrDefault(x =>
								x.IsDerived<T>()
							);
						if(null != one) { builtin = (T)one; }
					}
				}
			}
			return builtin;
		}

		private static ILog _Log;
		public static ILog Log
		{
			get
			{
				if(null == _Log) {
					_Log = Kernel.GetBuiltin<ILog>();
				}
				return _Log;
			}
		}

		private static IData _Data;
		public static IData Data
		{
			get
			{
				if(null == _Data) {
					_Data = Kernel.GetBuiltin<IData>();
				}
				return _Data;
			}
		}
		#endregion

		#region Plugin
		public static T GetPlugin<T>() where T : IPlugin
		{
			T plugin = default(T);
			if(null != _Plugins) {
				lock(_PieceLock) {
					if(null != _Plugins) {
						var one = _Plugins
							.Values
							.FirstOrDefault(x =>
								x.IsDerived<T>()
							);
						if(null != one) { plugin = (T)one; }
					}
				}
			}
			return plugin;
		}
		#endregion

		#region private
		private void Register()
		{
			_LifeCycles = new ReadOnlyDictionary<string, ILifeCycle>(
				Reflector.CollectImplementedObject<ILifeCycle>()
			);
			_Builtins = new ReadOnlyDictionary<string, IBuiltin>(
				Reflector.CollectImplementedObject<IBuiltin>()
			);
			_Plugins = new ReadOnlyDictionary<string, IPlugin>(
				Reflector.CollectImplementedObject<IPlugin>()
			);

			// KernelLifeCycle
			var kvp = _LifeCycles.FirstOrDefault(x =>
				x.Value.IsDerived<IKLifeCycle>()
			);
			_KernelLifeCycle = kvp.Value as IKLifeCycle;

			// ILifeCycle
			var kvps = _LifeCycles.Where(x =>
				!x.Value.IsDerived<IKLifeCycle>()
			);
			if(kvps.Any()) {
				_OtherLifeCycles = kvps
					.Select<
						KeyValuePair<string, ILifeCycle>,
						ILifeCycle
						>((x, i) =>
							x.Value
					);
			}
			if(null == _OtherLifeCycles) {
				_OtherLifeCycles = Enumerable.Empty<ILifeCycle>();
			}

		}
		#endregion
	}
}
