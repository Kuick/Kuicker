// ViewSchema.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public class ViewSchema
	{
		public string ViewName { get; set; }
		public string Comments { get; set; }
		public IQueryable<ColumnSchema> Columns { get; set; }
	}
}
