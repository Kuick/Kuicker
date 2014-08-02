// SqlClientBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;
using System.Text;

namespace Kuicker.Data
{
	public class SqlClientBuilder : SqlBuilder
	{
		public override string ProviderName
		{
			get
			{
				return "System.Data.SqlClient";
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
			var sb = new StringBuilder();
			sb.AppendFormat(
@"CREATE TABLE {0}",
				Api.CommandBuilder.QuoteIdentifier(schema.TableName)
			);

			bool firstTime = true;
			sb.Append("(" + Environment.NewLine);
			foreach(var column in schema.Columns) {
				sb.AppendFormat(
@"	{0}{1}{2}",
					(firstTime ? " " : ","),
					ColumnSqlCommand(column),
					Environment.NewLine
				);
				if(firstTime) { firstTime = false; }
			}

			// primary key
			sb.AppendFormat(
@"	{0}CONSTRAINT [{1}_PK] PRIMARY KEY CLUSTERED({2}",
				(firstTime ? " " : ","),
				schema.TableName,
				Environment.NewLine
			);
			bool pkFirstTime = true;
			foreach(var column in schema.PrimaryKeys) {
				sb.AppendFormat(
@"		{0}{1} ASC{2}",
					(pkFirstTime ? " " : ","),
					Api.CommandBuilder.QuoteIdentifier(column.ColumnName),
					Environment.NewLine
				);
				if(pkFirstTime) { pkFirstTime = false; }
			}
			sb.AppendFormat("\t){0}", Environment.NewLine);

			sb.Append(")");

			return sb.ToString();
		}

		public override string BuildDropIndexCommand(IndexSchema schema)
		{
			var sql = String.Format(
@"DROP INDEX {0}.{1}",
				Api.CommandBuilder.QuoteIdentifier(schema.TableName),
				Api.CommandBuilder.QuoteIdentifier(schema.IndexName)
			);
			return sql;
		}

		public override string BuildCreateIndexCommand(IndexSchema schema)
		{
			var sb = new StringBuilder();
			sb.AppendFormat(
@"CREATE {0}INDEX {1} ON {2}",
				(schema.Unique ? "UNIQUE " : string.Empty),
				Api.CommandBuilder.QuoteIdentifier(schema.IndexName),
				Api.CommandBuilder.QuoteIdentifier(schema.TableName)
			);

			bool firstTime = true;
			sb.Append("(");
			foreach(var column in schema.Columns) {
				sb.AppendLine();
				sb.AppendFormat(
@"	{0}{1}",
					(firstTime ? " " : ","),
					Api.CommandBuilder.QuoteIdentifier(column.ColumnName)
				);
				if(firstTime) { firstTime = false; }
			}
			sb.AppendLine();
			sb.Append(")");

			return sb.ToString();
		}

		public override string BuildAddColumnCommand(ColumnSchema schema)
		{
			string sql = String.Format(
@"ALTER TABLE {0} ADD {1}",
				Api.CommandBuilder.QuoteIdentifier(schema.TableName),
				ColumnSqlCommand(schema)
			);
			return sql;
		}

		public override string BuildAlterColumnCommand(ColumnSchema schema)
		{
			string sql = String.Format(
@"ALTER TABLE {0} ALTER COLUMN {1}",
				Api.CommandBuilder.QuoteIdentifier(schema.TableName),
				ColumnSqlCommand(schema)
			);
			return sql;
		}

		#region private
		private string ColumnSqlCommand(ColumnSchema schema)
		{
			throw new NotImplementedException();

			//DataFormat format = Formator.ToDataFormat(schema.DataType);

			//var sb = new StringBuilder();
			//sb.Append(
			//	Api.CommandBuilder.QuoteIdentifier(schema.ColumnName));
			//sb.Append(" ");

			//int len = schema.DataLength;
			//switch(format) {
			//	case DataFormat.Unknown:
			//		break;
			//	case DataFormat.Boolean:
			//		sb.AppendFormat("VARCHAR({0})", 5);
			//		break;
			//	case DataFormat.Char:
			//		sb.AppendFormat("CHAR({0})", len);
			//		break;
			//	case DataFormat.Bit:
			//		sb.Append("BIT");
			//		break;
			//	case DataFormat.Long:
			//		sb.Append("BIGINT");
			//		break;
			//	case DataFormat.Decimal:
			//		sb.AppendFormat("DECIMAL({0},{1})", len + 6, 6);
			//		break;
			//	case DataFormat.Double:
			//		sb.Append("FLOAT");
			//		break;
			//	case DataFormat.Integer:
			//		sb.Append("INT");
			//		break;
			//	case DataFormat.Enum:
			//		sb.AppendFormat("VARCHAR({0})", 128);
			//		break;
			//	case DataFormat.MaxVarBinary:
			//		sb.Append(isSQL2000 ? "IMAGE" : " VARBINARY(MAX)");
			//		break;
			//	case DataFormat.MaxVarChar:
			//		sb.Append(isSQL2000 ? "TEXT" : " VARCHAR  (MAX)");
			//		break;
			//	case DataFormat.MaxVarWChar:
			//		sb.Append(isSQL2000 ? "NTEXT" : " NVARCHAR (MAX)");
			//		break;
			//	case DataFormat.TimeStamp:
			//		sb.Append("DATETIME");
			//		break;
			//	case DataFormat.VarChar:
			//		sb.AppendFormat("VARCHAR({0})", len);
			//		break;
			//	case DataFormat.VarWChar:
			//		sb.AppendFormat("NVARCHAR({0})", len);
			//		break;
			//	case DataFormat.WChar:
			//		sb.AppendFormat("NCHAR({0})", len);
			//		break;
			//	case DataFormat.Uuid:
			//		sb.AppendFormat("VARCHAR({0})", 32);
			//		break;
			//	case DataFormat.Identity:
			//		sb.Append("BIGINT");
			//		break;
			//	default:
			//		throw new NotImplementedException();
			//}

			//// null
			//sb.Append(column.Spec.NotAllowNull ? " NOT NULL" : " NULL");

			//// identity
			//if(column.Spec.Identity) { sb.Append(" IDENTITY(1, 1)"); }

			//return sb.ToString();
		}
		#endregion
	}
}
