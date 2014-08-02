// Config.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Kuicker
{
	public class Config : ConfigurationSection
	{
		#region inner class
		public class Xml
		{
			public const string Kuicker = "Kuicker";
			public const string Kernel = "Kernel";
			public const string Builtin = "Builtin";
			public const string Plugin = "Plugin";
			public const string Application = "Application";
		}
		#endregion

		#region field
		private static object _Lock = new object();
		private static Config _Current;
		#endregion

		#region constructor
		internal Config()
		{
			this.KernelSection = new List<Any>();
			this.BuiltinSection = new List<Many>();
			this.PluginSection = new List<Many>();
			this.ApplicationSection = new List<Many>();
		}
		#endregion

		#region static
		internal static Config Current
		{
			get
			{
				if(null == _Current) {
					lock(_Lock) {
						if(null == _Current) {
							_Current = ConfigurationManager.GetSection(
								Xml.Kuicker
							) as Config;
							if(null == _Current) {
								_Current = new Config();
							}
						}
					}
				}
				return _Current;
			}
		}
		#endregion

		#region property
		public List<Any> KernelSection { get; internal set; }
		public List<Many> BuiltinSection { get; internal set; }
		public List<Many> PluginSection { get; internal set; }
		public List<Many> ApplicationSection { get; internal set; }
		#endregion

		#region Config
		public class Kernel
		{
			private static string _AppID;
			public static string AppID
			{
				get
				{
					if(_AppID.IsNullOrEmpty()) {
						_AppID = Current.KernelSection.ToString("AppID");
					}
					return _AppID;
				}
			}

			public static bool Debug
			{
				get
				{
					return Current.KernelSection.ToBoolean("Debug");
				}
			}

			private static string[] _SkipAssemblyPrefixes;
			public static string[] SkipAssemblyPrefixes
			{
				get
				{
					if(null == _SkipAssemblyPrefixes) {
						_SkipAssemblyPrefixes = Current
							.KernelSection
							.ToStrings("SkipAssemblyPrefixes");
					}
					return _SkipAssemblyPrefixes;
				}
			}

			private static string[] _OnlyAssemblyPrefixes;
			public static string[] OnlyAssemblyPrefixes
			{
				get
				{
					if(null == _OnlyAssemblyPrefixes) {
						_OnlyAssemblyPrefixes = Current
							.KernelSection
							.ToStrings("OnlyAssemblyPrefixes");
					}
					return _OnlyAssemblyPrefixes;
				}
			}
		}
		#endregion

		#region Builtin
		public class Builtin
		{
			public static Many Get(string group, string name)
			{
				var many = Current.BuiltinSection.GetMany(group, name);
				return null == many ? new Many() : many;
			}

			public static List<Many> InGroup(string group)
			{
				return Current.BuiltinSection.GetManys(group);
			}

			public static List<Any> Log
			{
				get
				{
					var manys = Current
						.BuiltinSection
						.GetManys(Constants.Builtin.Log);

					return null == manys
						? new List<Any>()
						: manys.Select(x => x.ToAnyWithFullName()).ToList();
				}
			}

			public static List<Any> Data
			{
				get
				{
					var manys = Current
						.BuiltinSection
						.GetManys(Constants.Builtin.Data);

					return null == manys
						? new List<Any>()
						: manys.Select(x => x.ToAnyWithFullName()).ToList();
				}
			}
		}
		#endregion

		#region Plugin
		public class Plugin
		{
			public static Many Get(string group, string name)
			{
				var many = Current.PluginSection.GetMany(group, name);
				return null == many ? new Many() : many;
			}

			public static List<Many> InGroup(string group)
			{
				return Current
					.PluginSection
					.GetManys(group);
			}
		}
		#endregion

		#region Application
		public class Application
		{
			public static Many Get(string group, string name)
			{
				var many = Current.ApplicationSection.GetMany(group, name);
				return null == many ? new Many() : many;
			}

			public static List<Many> InGroup(string group)
			{
				return Current
					.ApplicationSection
					.GetManys(group);
			}
		}
		#endregion
	}
}
