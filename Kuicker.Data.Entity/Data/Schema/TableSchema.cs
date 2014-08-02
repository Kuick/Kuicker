// TableSchema.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public class TableSchema
	{
		public string TableName { get; set; }
		public string Comments { get; set; }
		public IQueryable<ColumnSchema> Columns { get; set; }
		public IQueryable<ColumnSchema> PrimaryKeys { get; set; }
		public IQueryable<IndexSchema> Indexs { get; set; }

		public bool IsPrimaryKey(string columnName)
		{
			if(null == PrimaryKeys){return false;}
			return PrimaryKeys.Any(x => x.ColumnName == columnName);
		}
	}
}
