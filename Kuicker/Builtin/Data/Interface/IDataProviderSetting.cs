// IDataProviderSetting.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public interface IDataProviderSetting
	{
		string Name { get; set; }
		string ProviderName { get; set; }
		string ConnectionString { get; set; }
	}
}
