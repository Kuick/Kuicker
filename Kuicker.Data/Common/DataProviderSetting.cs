// DataProviderSetting.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class DataProviderSetting : IDataProviderSetting
	{
		public string Name { get; set; }
		public string ProviderName { get; set; }
		public string ConnectionString { get; set; }
	}
}
