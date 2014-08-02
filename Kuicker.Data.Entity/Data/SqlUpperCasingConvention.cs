// SqlUpperCasingConvention.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Kuicker.Data
{
	public class SqlUpperCasingConvention : ISqlConvention
	{
		private static object _TableLock = new object();
		private static object _ColumnLock = new object();
		private static Dictionary<string, string> _Tables =
			new Dictionary<string, string>();
		private static Dictionary<string, string> _Columns =
			new Dictionary<string, string>();

		public string ToTableName(Type type)
		{
			string key = type.Name;
			string name;

			if(_Tables.TryGetValue(key, out name)) {
				return name;
			}

			lock(_TableLock) {
				if(_Tables.TryGetValue(key, out name)) {
					return name;
				}

				name = string.Format(
					"T__{0}__",
					type.Name.TrimEnd("Entity").ToUpperCasing()
				);
				_Tables.Add(key, name);

				return name;
			}
		}

		public string ToColumnName(Type type, PropertyInfo info)
		{
			string key = string.Concat(type.Name, ".", info.Name);
			string columnName;

			if(_Columns.TryGetValue(key, out columnName)) {
				return columnName;
			}

			lock(_ColumnLock) {
				if(_Columns.TryGetValue(key, out columnName)) {
					return columnName;
				}

				columnName = info.Name.ToUpperCasing();
				_Columns.Add(key, columnName);
				return columnName;
			}
		}
	}
}
