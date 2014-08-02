// SqlBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public abstract class SqlBuilder : ISqlBuilder
	{
		public SqlBuilder()
		{
		}


		public EntityApi Api { get; set; }
		public ISqlFormater Formator { get; set; }

		public abstract string ProviderName { get; }


		public abstract string SelectDistinctYear(SqlSelect select);
		public abstract string SelectDistinctMonth(SqlSelect select);
		public abstract string SelectDistinctDay(SqlSelect select);

		public abstract IQueryable<TableSchema> TableSchemas { get; }
		public abstract IQueryable<ViewSchema> ViewSchemas { get; }
		public abstract IQueryable<PackageSchema> PackageSchemas { get; }

		public abstract string BuildCreateTableCommand(TableSchema schema);
		public abstract string BuildDropIndexCommand(IndexSchema schema);
		public abstract string BuildCreateIndexCommand(IndexSchema schema);
		public abstract string BuildAddColumnCommand(ColumnSchema schema);
		public abstract string BuildAlterColumnCommand(ColumnSchema schema);
	}
}
