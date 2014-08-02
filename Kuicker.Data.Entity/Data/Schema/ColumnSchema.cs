// TableSchema.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class ColumnSchema
	{
		public string TableName { get; set; }
		public string ColumnName { get; set; }
		public int ColumnID { get; set; }
		public string DataType { get; set; }
		public int DataLength { get; set; }
		public bool Nullable { get; set; }
		public string Comments { get; set; }
		public TableSchema Table { get; set; }

		public string ToTypeName(ISqlFormater formater)
		{
			var ef = EnumCache.Get<DataFormat>();
			var format = formater.ToDataFormat(DataType);
			var ei = ef.Get(format.ToStringX());
			var type = ei.Description;
			return type;
		}

		public bool PrimaryKey
		{
			get
			{
				return Table.IsPrimaryKey(ColumnName);
			}
		}
	}
}
