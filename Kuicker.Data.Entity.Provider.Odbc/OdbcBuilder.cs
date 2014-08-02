// OdbcBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public class OdbcBuilder : SqlBuilder
	{
		public override string ProviderName
		{
			get
			{
				return "System.Data.Odbc";
			}
		}

		public override string SelectDistinctYear(SqlSelect select)
		{
			return string.Format(
				"CONVERT(VARCHAR(4), {0}, 21) AS {1}",
				select.Sql.Api.QuoteIdentifier(select.SqlFullName),
				select.As
			);
		}

		public override string SelectDistinctMonth(SqlSelect select)
		{
			return string.Format(
				"CONVERT(VARCHAR(7), {0}, 21) AS {1}",
				select.Sql.Api.QuoteIdentifier(select.SqlFullName),
				select.As
			);
		}

		public override string SelectDistinctDay(SqlSelect select)
		{
			return string.Format(
				"CONVERT(VARCHAR(10), {0}, 21) AS {1}",
				select.Sql.Api.QuoteIdentifier(select.SqlFullName),
				select.As
			);
		}

		public override IQueryable<TableSchema> TableSchemas
		{
			get
			{
				// todo
				throw new NotImplementedException();
			}
		}

		public override IQueryable<ViewSchema> ViewSchemas
		{
			get
			{
				// todo
				throw new NotImplementedException();
			}
		}

		public override IQueryable<PackageSchema> PackageSchemas
		{
			get
			{
				// todo
				throw new NotImplementedException();
			}
		}

		public override string BuildCreateTableCommand(TableSchema schema)
		{
			throw new NotSupportedException();
		}

		public override string BuildDropIndexCommand(IndexSchema schema)
		{
			throw new NotSupportedException();
		}

		public override string BuildCreateIndexCommand(IndexSchema schema)
		{
			throw new NotSupportedException();
		}

		public override string BuildAddColumnCommand(ColumnSchema schema)
		{
			throw new NotSupportedException();
		}

		public override string BuildAlterColumnCommand(ColumnSchema schema)
		{
			throw new NotSupportedException();
		}
	}
}
