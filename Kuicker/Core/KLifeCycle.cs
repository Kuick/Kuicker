// KLifeCycle.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kuicker
{
	internal sealed class KLifeCycle : LifeCycle, IKLifeCycle
	{
		public void DoBuiltinStart()
		{
			using(var il = new ILogger()) {
				if(null != Kernel._Builtins) {
					Parallel.ForEach(
						Kernel._Builtins, 
						x => x.Value.Start().Wait()
					);
				}
			}
		}

		public void DoPluginStart()
		{
			using(var il = new ILogger()) {
				if(null != Kernel._Plugins) {
					Parallel.ForEach(
						Kernel._Plugins,
						x => x.Value.Start().Wait()
					);
				}
			}
		}

		public void DoPluginStop()
		{
			using(var il = new ILogger()) {
				if(null != Kernel._Plugins) {
					Parallel.ForEach(
						Kernel._Plugins,
						x => x.Value.Stop().Wait()
					);
				}
			}
		}

		public void DoBuiltinStop()
		{
			using(var il = new ILogger()) {
				if(null != Kernel._Builtins) {
					Parallel.ForEach(
						Kernel._Builtins,
						x => x.Value.Stop().Wait()
					);
				}
			}
		}

		public void DoPluginSuspend()
		{
			using(var il = new ILogger()) {
				if(null != Kernel._Plugins) {
					Parallel.ForEach(
						Kernel._Plugins,
						x => x.Value.Suspend().Wait()
					);
				}
			}
		}

		public void DoPluginResume()
		{
			using(var il = new ILogger()) {
				if(null != Kernel._Plugins) {
					Parallel.ForEach(
						Kernel._Plugins,
						x => x.Value.Resume().Wait()
					);
				}
			}
		}

		public override void DoAfterBuiltinStart()
		{
			LogRecord
				.Create()
				.SetTitle("RunTime")
				.Add("UserDomainName", RunTime.UserDomainName)
				.Add("ComputerName", RunTime.ComputerName)
				.Add("UserName", RunTime.UserName)
				.Add("OperatingSystemBits", RunTime.OperatingSystemBits)
				.Add("ProcessorBits", RunTime.ProcessorBits)
				.Add("OSVersion", Environment.OSVersion.VersionString)
				.Add("IsWebApp", RunTime.IsWebApp)
				.Add("BinFolder", RunTime.BinFolder)
				.Info();

			LogRecord
				.Create()
				.SetTitle("Kernel")
				.Add("AppID", Config.Kernel.AppID)
				.Add("Debug", Config.Kernel.Debug)
				.Add(
					"SkipAssemblyPrefixes", 
					Config.Kernel.SkipAssemblyPrefixes.Join(", ")
				)
				.Add(
					"OnlyAssemblyPrefixes",
					Config.Kernel.OnlyAssemblyPrefixes.Join(", ")
				)
				.Info();

			LogRecord
				.Create()
				.SetTitle("Builtin")
				.AddRange(Config.Current.BuiltinSection)
				.Info();

			LogRecord
				.Create()
				.SetTitle("Plugin")
				.AddRange(Config.Current.PluginSection)
				.Info();

			LogRecord
				.Create()
				.SetTitle("Application")
				.AddRange(Config.Current.ApplicationSection)
				.Info();

			base.DoAfterBuiltinStart();
		}
	}
}
