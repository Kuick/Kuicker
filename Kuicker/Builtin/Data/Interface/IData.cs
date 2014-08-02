// IData.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Data.Common;

namespace Kuicker
{
	public interface IData : IBuiltin
	{
		IDataApi CreateApi();
		IDataApi CreateApi(string name);
	}
}
