// MySqlClientBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public class MySqlClientBuilder : SqlBuilder
	{
		public override string ProviderName
		{
			get
			{
				return "MySql.Data.MySqlClient";
			}
		}

		public override string SelectDistinctYear(SqlSelect select)
		{
			throw new NotImplementedException();
		}

		public override string SelectDistinctMonth(SqlSelect select)
		{
			throw new NotImplementedException();
		}

		public override string SelectDistinctDay(SqlSelect select)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public override string BuildDropIndexCommand(IndexSchema schema)
		{
			throw new NotImplementedException();
		}

		public override string BuildCreateIndexCommand(IndexSchema schema)
		{
			throw new NotImplementedException();
		}

		public override string BuildAddColumnCommand(ColumnSchema schema)
		{
			throw new NotImplementedException();
		}

		public override string BuildAlterColumnCommand(ColumnSchema schema)
		{
			throw new NotImplementedException();
		}
	}
}
