// DataSettings.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;

namespace Kuicker.Data
{
	public sealed class DataSettings
	{
		private static object _Lock = new object();

		public static ConcurrentDictionary<string, IDataProviderSetting> All
		{ get; private set; }

		public static IDataProviderSetting Default
		{ get; private set; }

		public static void Add(
			string name,
			string connectionString,
			string providerName)
		{
			Configure();
			if(name.IsNullOrEmpty()) {
				throw new ArgumentException(
					"connectionString's name can't be null",
					"name"
				);
			}
			if(connectionString.IsNullOrEmpty()) {
				throw new ArgumentException(
					"connectionString's connectionString can't be null",
					"connectionString"
				);
			}
			if(providerName.IsNullOrEmpty()) {
				throw new ArgumentException(
					"connectionString's providerName can't be null",
					"providerName"
				);
			}

			name = name.ToUpper();

			lock(_Lock) {
				var settings = new DataProviderSetting() {
					Name = name,
					ConnectionString = connectionString,
					ProviderName = providerName,
				};
				Add(settings);
			}
		}

		public static IDataProviderSetting GetByName(string name)
		{
			Configure();
			if(All.Count == 0) {
				throw new SettingsPropertyNotFoundException(
					new[]{
						"Can't find any connectionString settings ",
						"in web.config (app.config) file"
					}.Join()
				);
			}

			IDataProviderSetting settings;
			if(name.IsNullOrEmpty()) {
				settings = Default;

			} else {
				name = name.ToUpper();

				// by original name
				if(All.TryGetValue(name, out settings)) {
					return settings;
				}

				// by default name
				settings = Default;
			}

			// can't find
			if(null == settings) {
				throw new Exception(
					new[]{
						"Can't find any connectionString settings ",
						"called ",
						name,
						" or ",
						Constants.Default,
					}.Join()
				);
			}

			return settings;
		}

		public static IDataProviderSetting GetByProviderName(
			string providerName)
		{
			Configure();
			if(All.Count == 0) { return null; }
			if(providerName.IsNullOrEmpty()) { return null; }

			var list = All.Where(x => 
				x.Value.ProviderName == providerName
			);

			if(!list.Any()) {
				throw new Exception(
					new[]{
						"Can't find any ConnectionStringSettings's ",
						"ProviderName called '",
						providerName,
						"'",
					}.Join()
				);
			}
			if(list.Count() > 1) {
				throw new Exception(
					new[]{
						"More than one ConnectionStringSettings's ",
						"ProviderName called '",
						providerName,
						"'",
					}.Join()
				);
			}

			return list.FirstOrDefault().Value;
		}

		internal static void Configure()
		{
			if(null != All) { return; }

			lock(_Lock){
				if(null != All) { return; }

				// all
				All = new ConcurrentDictionary
					<string, IDataProviderSetting>();
				IEnumerator all = ConfigurationManager
					.ConnectionStrings
					.GetEnumerator();
				while(all.MoveNext()){
					var settings = all.Current 
						as ConnectionStringSettings;
					if(null == settings) { continue; }

					string[] names = settings.Name.SplitAndTrim(",");
					foreach (var name in names)
					{
						Add(new DataProviderSetting() {
							Name = name.ToUpper(),
							ConnectionString = settings.ConnectionString,
							ProviderName = settings.ProviderName,
						});
					}
				}

				if(All.Any() && null == Default) {
					Default = All.FirstOrDefault().Value;
				}
			}
		}

		public static void Remove(string name)
		{
			IDataProviderSetting setting;
			All.TryRemove(name, out setting);
		}

		public static void Clear()
		{
			All.Clear();
			Default = null;
		}

		public static void Add(IDataProviderSetting settings)
		{
			// All
			All.AddOrUpdate(
				settings.Name,
				settings,
				(xKey, xValue) => settings
			);

			// Default
			if(settings.Name.EqualsX(Constants.Default)) {
				Default = settings;
			}
		}
	}
}
