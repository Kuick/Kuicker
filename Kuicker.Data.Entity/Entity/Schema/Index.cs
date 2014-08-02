// Index.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kuicker.Data
{
	public class Index
	{
		public Index()
		{
			this.ColumnNames = new List<string>();
		}

		public string TableName { get; set; }
		public string EntityName { get; set; }
		public bool Unique { get; set; }

		private string _IndexName;
		public string IndexName
		{
			get
			{
				if(null == _IndexName) {
					return string.Format(
						"I__{0}__{1}__",
						EntityName.TrimEnd("Entity").ToUpperCasing(),
						ColumnNames.GetHashCode()
					);
				}
				return _IndexName;
			}
			set
			{
				_IndexName = value;
			}
		}


		public List<string> ColumnNames { get; set; }

		internal Table _Table;
		public Table Table
		{
			get
			{
				if(null == _Table) {
					_Table = EntityCache.GetTable(EntityName);
				}
				return _Table;
			}
		}
	}

	public class Index<T> : Index
		where T : class, IEntity<T>, new()
	{
		public Index()
			: base()
		{
			this.Schema = EntityCache.Get<T>();
			this._Table = this.Schema.__Table;
			this.EntityName = this.Schema.__EntityName;
			this.TableName = this.Schema.__TableName;
		}

		public IEntity Schema { get; private set; }

		public Index<T> SetUnique()
		{
			Unique = true;
			return this;
		}

		public Index<T> AddColumn(
			Expression<Func<T, object>> expression)
		{
			Column column = Column.Evaluate<T>(expression);
			ColumnNames.Add(column.ColumnName);
			return this;
		}
	}
}
