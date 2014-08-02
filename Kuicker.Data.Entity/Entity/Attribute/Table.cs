// Table.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;

namespace Kuicker.Data
{
	[AttributeUsage(
		AttributeTargets.Class,
		AllowMultiple = false,
		Inherited = true)]
	public sealed class Table : Attribute
	{
		public Table()
		{
		}

		public string EntityName { get; internal set; }
		public string TableName { get; internal set; }

		public string Category { get; internal set; }
		public string Description { get; internal set; }

		public List<Index> Indexes { get; internal set; }

		public bool IsView { get; internal set; }
		public DiffFollow Follow { get; internal set; }

		private IEntity _Schema;
		public IEntity Schema
		{
			get
			{
				if(null == _Schema) {
					_Schema = EntityCache.Get(EntityName);
				}
				return _Schema;
			}
		}

		public List<Column> Columns
		{
			get
			{
				return Schema.__Columns;
			}
		}


		private int _Index;
		public int Index
		{
			get
			{
				if(_Index < 0) {
					_Index = EntityCache.GetTableIndex(EntityName);
				}
				return _Index;
			}
		}

		public string Alias
		{
			get
			{
				return Schema.__Alias;
			}
		}


		internal int GetColumnIndex(string columnName)
		{
			int index = 0;
			foreach(var column in Columns) {
				index++;
				if(column.ColumnName.Equals(
					columnName, StringComparison.OrdinalIgnoreCase)) {
					break;
				}
			}
			if(index == 0) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"This table '",
							TableName,
							"' have no column '",
							columnName,
							"'."
						)
						.Error()
						.Message
				);
			}

			return index;
		}

		internal string GetColumnAlias(string columnName)
		{
			int index = GetColumnIndex(columnName);
			return string.Concat(
				Alias,
				"_",
				index.Order(Columns.Count)
			);
		}
	}
}
