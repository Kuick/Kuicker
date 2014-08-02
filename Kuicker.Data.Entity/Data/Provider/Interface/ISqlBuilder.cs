// ISqlBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public interface ISqlBuilder
	{
		EntityApi Api { get; set; }
		ISqlFormater Formator { get; set; }

		string ProviderName { get; }

		string SelectDistinctYear(SqlSelect select);
		string SelectDistinctMonth(SqlSelect select);
		string SelectDistinctDay(SqlSelect select);
		IQueryable<TableSchema> TableSchemas { get; }
		IQueryable<ViewSchema> ViewSchemas { get; }
		IQueryable<PackageSchema> PackageSchemas { get; }

		string BuildCreateTableCommand(TableSchema schema);
		string BuildDropIndexCommand(IndexSchema schema);
		string BuildCreateIndexCommand(IndexSchema schema);
		string BuildAddColumnCommand(ColumnSchema schema);
		string BuildAlterColumnCommand(ColumnSchema schema);
	}
}
