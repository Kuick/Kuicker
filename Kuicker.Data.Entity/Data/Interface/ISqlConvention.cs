// ISqlConvention.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Reflection;

namespace Kuicker.Data
{
	public interface ISqlConvention
	{
		string ToTableName(Type type);
		string ToColumnName(Type type, PropertyInfo info);
	}
}
