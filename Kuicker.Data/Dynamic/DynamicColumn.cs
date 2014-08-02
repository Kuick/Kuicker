// DynamicColumn.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Data;

namespace Kuicker.Data
{
	public class DynamicColumn
	{
		public bool AllowDBNull { get; set; }
		public bool AutoIncrement { get; set; }
		public long AutoIncrementSeed { get; set; }
		public long AutoIncrementStep { get; set; }
		public string Caption { get; set; }
		public string ColumnName { get; set; }
		public Type DataType { get; set; }
		public object DefaultValue { get; set; }
		public string Expression { get; set; }
		public int MaxLength { get; set; }
		public string Namespace { get; set; }
		public string Prefix { get; set; }
		public bool ReadOnly { get; set; }
		public bool Unique { get; set; }

		public bool IsAliased { get; set; }
		public bool IsKey { get; set; }
		public bool IsRowID { get; set; }
		public bool IsHidden { get; set; }
		public bool IsLong { get; set; }
		public int ProviderType { get; set; }

		private string _BaseColumnName;
		public string BaseColumnName
		{
			get
			{
				if(_BaseColumnName.IsNullOrEmpty()) {
					return ColumnName;
				}
				return _BaseColumnName;
			}
			set
			{
				_BaseColumnName = value;
			}
		}

		public string BaseTableName { get; set; }
	}
}
