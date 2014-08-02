// DefaultBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kuicker.Data
{
	public class DefaultBuilder : SqlBuilder
	{
		public override string ProviderName
		{
			get
			{
				return Constants.Default;
			}
		}

		public override string SelectDistinctYear(SqlSelect select)
		{
			throw new NotSupportedException();
		}

		public override string SelectDistinctMonth(SqlSelect select)
		{
			throw new NotSupportedException();
		}

		public override string SelectDistinctDay(SqlSelect select)
		{
			throw new NotSupportedException();
		}

		public override IQueryable<TableSchema> TableSchemas
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override IQueryable<ViewSchema> ViewSchemas
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override IQueryable<PackageSchema> PackageSchemas
		{
			get
			{
				throw new NotSupportedException();
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
