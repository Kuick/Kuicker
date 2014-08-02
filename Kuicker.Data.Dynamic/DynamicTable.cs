// DynamicTable.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Dynamic;
using System.Linq;

namespace Kuicker.Data
{
	public class DynamicTable
	{
		public DynamicTable(string tableName)
		{
			this.TableName = tableName;
		}

		public string TableName { get; set; }
		public List<DynamicColumn> Columns { get; set; }
	}
}
