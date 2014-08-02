// TableSchema.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public class IndexSchema
	{
		public string TableName { get; set; }
		public string IndexName { get; set; }
		public IQueryable<ColumnSchema> Columns { get; set; }
		public bool Unique { get; set; }
	}
}
