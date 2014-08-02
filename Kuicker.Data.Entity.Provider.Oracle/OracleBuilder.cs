// OracleBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kuicker.Data
{
	public abstract class OracleBuilder : SqlBuilder
	{
		public override string SelectDistinctYear(SqlSelect select)
		{
			return string.Format(
				"TO_CHAR({0}, 'YYYY') AS {1}",
				select.Sql.Api.QuoteIdentifier(select.SqlFullName),
				select.As
			);
		}

		public override string SelectDistinctMonth(SqlSelect select)
		{
			return string.Format(
				"TO_CHAR({0}, 'YYYY-MM') AS {1}",
				select.Sql.Api.QuoteIdentifier(select.SqlFullName),
				select.As
			);
		}

		public override string SelectDistinctDay(SqlSelect select)
		{
			return string.Format(
				"TO_CHAR({0}, 'YYYY-MM-DD') AS {1}",
				select.Sql.Api.QuoteIdentifier(select.SqlFullName),
				select.As
			);
		}

		private IQueryable<TableSchema> _TableSchemas;
		public override IQueryable<TableSchema> TableSchemas
		{
			get
			{
				if(null == _TableSchemas) {
					var sql = string.Format(
@"SELECT
	T.TABLE_NAME as TABLE_NAME,
	C.COMMENTS   as COMMENTS
FROM
	{0}_TABLES T
	LEFT OUTER JOIN {0}_TAB_COMMENTS C
		ON T.TABLE_NAME = C.TABLE_NAME
ORDER BY
	T.TABLE_NAME",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<TableSchema>();
					foreach(var row in rows) {
						string tableName = row.TableName.ToString();
						string comments = row.Comments.ToString();
						var columns = AllColumns
							.Where(x => x.TableName == tableName);

						var pKeys = AllPrimaryKeys.Where(x => 
							x.TableName == tableName
						);

						var primaryKeys = Enumerable
								.Empty<ColumnSchema>()
								.AsQueryable();
						if(pKeys.Any()){
							var pColumnNames = pKeys
								.Select(x => x.ColumnName)
								.ToArray();
							primaryKeys = AllColumns.Where(x =>
								x.ColumnName.In(pColumnNames)
							);
						}
						var indexs = AllIndexes.Where(x =>
							x.TableName == tableName
						);
						var one = new TableSchema() {
							TableName = tableName,
							Comments = comments,
							Columns = columns,
							PrimaryKeys = primaryKeys,
							Indexs = indexs,
						};
						list.Add(one);
					}
					_TableSchemas = list.AsQueryable();
				}
				return _TableSchemas;
			}
		}

		private IQueryable<ViewSchema> _ViewSchemas;
		public override IQueryable<ViewSchema> ViewSchemas
		{
			get
			{
				if(null == _ViewSchemas) {
					var sql = string.Format(
@"SELECT
	T.VIEW_NAME as VIEW_NAME,
	C.COMMENTS  as COMMENTS
FROM
	{0}_VIEWS T
	LEFT OUTER JOIN {0}_TAB_COMMENTS C
		ON T.VIEW_NAME = C.TABLE_NAME
ORDER BY
	T.VIEW_NAME",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<ViewSchema>();
					foreach(var row in rows) {
						string viewName = row.ViewName.ToString();
						string comments = row.Comments.ToString();
						var columns = AllColumns
							.Where(x => x.TableName == viewName);
						var one = new ViewSchema() {
							ViewName = viewName,
							Comments = comments,
							Columns = columns,
						};
						list.Add(one);
					}
					_ViewSchemas = list.AsQueryable();
				}
				return _ViewSchemas;
			}
		}

		private IQueryable<PackageSchema> _PackageSchemas;
		public override IQueryable<PackageSchema> PackageSchemas
		{
			get
			{
				if(null == _PackageSchemas) {
					var sql = string.Format(
@"SELECT
	DISTINCT OBJECT_NAME AS PACKAGE_NAME
FROM
	{0}_PROCEDURES
WHERE
	OBJECT_TYPE = 'PACKAGE'",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<PackageSchema>();
					foreach(var row in rows) {
						string packageName = row.PackageName.ToString();
						var procedures = AllProcedures.Where(x =>
							x.PackageName == packageName
						);
						var one = new PackageSchema() {
							PackageName = packageName,
							Procedures = procedures,
						};
						list.Add(one);
					}

					// Standalone
					var standalones = AllProcedures.Where(x =>
						x.PackageName == EntityConstants.Sql.Standalone
					);
					if(standalones.Any()) {
						var one = new PackageSchema() {
							PackageName = EntityConstants.Sql.Standalone,
							Procedures = standalones,
						};
						list.Add(one);
					}

					_PackageSchemas = list.AsQueryable();
				}
				return _PackageSchemas;
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
			sb.Append(")");

			return sb.ToString();
		}

		public override string BuildDropIndexCommand(IndexSchema schema)
		{
			string sql = String.Format(
@"DROP INDEX {0};",
				Api.CommandBuilder.QuoteIdentifier(schema.IndexName)
			);
			return sql;
		}

		public override string BuildCreateIndexCommand(IndexSchema schema)
		{
			var sb = new StringBuilder();
			sb.AppendFormat(
@"CREATE {0}INDEX {1} on {2}",
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
			// skip pk
			if(schema.PrimaryKey) { return string.Empty; }

			string sql = String.Format(
@"ALTER TABLE {0} ADD {1}",
				Api.CommandBuilder.QuoteIdentifier(schema.TableName),
				ColumnSqlCommand(schema)
			);

			return sql;
		}

		public override string BuildAlterColumnCommand(ColumnSchema schema)
		{
			// skip pk
			if(schema.PrimaryKey) { return string.Empty; }

			var sb = new StringBuilder();
			if(schema.GetType().IsBoolean()) {
				// To ensure that the data format conversion is correct, 
				// processing data before correction.
				sb.AppendFormat(
@"UPDATE {0} SET {1} = '1' WHERE {1} = 'True';
UPDATE {0} SET {1} = '0' WHERE {1} = 'False';",
					Api.CommandBuilder.QuoteIdentifier(schema.TableName),
					Api.CommandBuilder.QuoteIdentifier(schema.ColumnName)
				);
				sb.AppendLine();
			}
			sb.AppendFormat(
@"ALTER TABLE {0} MODIFY {1};",
				Api.CommandBuilder.QuoteIdentifier(schema.TableName),
				ColumnSqlCommand(schema)
			);

			string sql = sb.ToString();
			return sql;
		}

		#region private
		private string ColumnSqlCommand(ColumnSchema schema)
		{
			throw new NotImplementedException();

			//string str = Tag(column.ColumnName);
			//int len = column.Length;
			//switch(column.DbType) {
			//	case SqlDataType.Bit:
			//		str += " NUMBER(1)";
			//		break;
			//	case SqlDataType.Long:
			//		str += " NUMBER(38)";
			//		break;
			//	case SqlDataType.Integer:
			//		str += " NUMBER(38)";
			//		break;
			//	case SqlDataType.Boolean:
			//		str += " VARCHAR2(" + 5 + ")";
			//		break;
			//	case SqlDataType.Enum:
			//		str += " VARCHAR2(" + 128 + ")";
			//		break;
			//	case SqlDataType.Char:
			//		str += " CHAR(" + len + ")";
			//		break;
			//	case SqlDataType.WChar:
			//		str += " NCHAR(" + len + ")";
			//		break;
			//	case SqlDataType.VarChar:
			//		str += " VARCHAR2(" + len + ")";
			//		break;
			//	case SqlDataType.VarWChar:
			//		str += " NVARCHAR2(" + len + ")";
			//		break;
			//	case SqlDataType.MaxVarChar:
			//	case SqlDataType.MaxVarWChar:
			//		str += " LONG VARCHAR";
			//		break;
			//	case SqlDataType.Decimal:
			//		str += " DECIMAL  (" + len + ", 3)";
			//		break;
			//	case SqlDataType.TimeStamp:
			//		str += " DATE";
			//		break;
			//	case SqlDataType.MaxVarBinary:
			//		str += " LONG RAW";
			//		break;
			//	default:
			//		throw new NotImplementedException(string.Format(
			//			"OracleBuilder.ColumnSqlCommand: {0}",
			//			column.DbType.ToString()
			//		));
			//}
			//str += !column.NotAllowNull ? " NULL " : " NOT NULL ";
			//str += column.PrimaryKey ? " primary key " : string.Empty;
			//return str;
		}




		private string _Scope;
		// <add group="Data" name="Scope" value="USER|ALL" />
		public string Scope
		{
			get
			{
				if(null == _Scope) {
					_Scope = Config.Builtin.Data.ToString(
						"Scope", 
						"USER"
					);
				}
				return _Scope;
			}
		}

		private string _Owner;
		// <add group="Data" name="Owner" value="SCOTT" />
		public string Owner
		{
			get
			{
				if(null == _Owner) {
					_Owner = Config.Builtin.Data.ToString(
						"Owner",
						Api.UserID
					);
				}
				return _Scope;
			}
		}

		private IQueryable<ColumnSchema> _AllColumns;
		private IQueryable<ColumnSchema> AllColumns
		{
			get
			{
				if(null == _AllColumns) {
					var sql = string.Format(
@"SELECT
	TC.TABLE_NAME  AS TABLE_NAME,
	TC.COLUMN_NAME AS COLUMN_NAME,
	TC.COLUMN_ID   AS COLUMN_ID,
	TC.DATA_TYPE   AS DATA_TYPE,
	TC.DATA_LENGTH AS DATA_LENGTH,
	TC.NULLABLE    AS NULLABLE,
	CC.COMMENTS    AS COMMENTS
FROM
	{0}_TAB_COLUMNS TC
	LEFT OUTER JOIN {0}_COL_COMMENTS CC
		ON  TC.TABLE_NAME  = CC.TABLE_NAME
		AND TC.COLUMN_NAME = CC.COLUMN_NAME
ORDER BY
	TC.TABLE_NAME,
	TC.COLUMN_ID",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<ColumnSchema>();
					foreach(var row in rows) {
						var one = new ColumnSchema() {
							TableName = row.TableName.ToString(),
							ColumnName = row.ColumnName.ToString(),
							ColumnID = row.ColumnID.ToInteger(),
							DataType = row.DataType.ToString(),
							DataLength = row.DataLength.ToInteger(),
							Nullable = row.Nullable.ToString().ToUpper() == "Y",
							Comments = row.Comments.ToString(),
						};
						list.Add(one);
					}
					_AllColumns = list.AsQueryable();
				}
				return _AllColumns;
			}
		}





		private IQueryable<XPrimaryKey> _AllPrimaryKeys;
		private IQueryable<XPrimaryKey> AllPrimaryKeys
		{
			get
			{
				if(null == _AllPrimaryKeys) {
					var sql = string.Format(
@"SELECT
	COLS.TABLE_NAME  as TABLE_NAME,
	COLS.COLUMN_NAME as COLUMN_NAME
FROM
	{0}_CONSTRAINTS  CONS,
	{0}_CONS_COLUMNS COLS
WHERE 
	CONS.CONSTRAINT_TYPE = 'P' AND
	CONS.CONSTRAINT_NAME = COLS.CONSTRAINT_NAME AND
	CONS.OWNER           = COLS.OWNER
ORDER BY
	COLS.TABLE_NAME,
	COLS.POSITION",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<XPrimaryKey>();
					foreach(var row in rows) {
						var one = new XPrimaryKey() {
							TableName = row.TableName.ToString(),
							ColumnName = row.ColumnName.ToString(),
						};
						list.Add(one);
					}
					_AllPrimaryKeys = list.AsQueryable();
				}
				return _AllPrimaryKeys;
			}
		}



		private IQueryable<IndexSchema> _AllIndexes;
		private IQueryable<IndexSchema> AllIndexes
		{
			get
			{
				if(null == _AllIndexes) {
					var sql = string.Format(
@"SELECT
    A.TABLE_NAME  AS TABLE_NAME,
    A.INDEX_NAME  AS INDEX_NAME,
    B.COLUMN_NAME AS COLUMN_NAME,
    A.UNIQUENESS  AS UNIQUENESS
FROM
    {0}_INDEXES A
    LEFT OUTER JOIN {0}_IND_COLUMNS B
        ON A.INDEX_NAME = B.INDEX_NAME
WHERE
	A.GENERATED = 'N'
ORDER BY
	A.TABLE_NAME,
	A.INDEX_NAME,
	B.COLUMN_POSITION",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<IndexSchema>();
					var pTableName = string.Empty;
					var pIndexName = string.Empty;
					var pUnique = false;
					var pColumnNames = new List<ColumnSchema>();
					foreach(var row in rows) {
						string tableName = row.TableName.ToString();
						string indexName = row.IndexName.ToString();
						bool unique = row.Uniqueness.ToString().ToUpper() == "UNIQUE";
						string columnName = row.ColumnName.ToString();

						if(pIndexName != indexName && !pIndexName.IsNullOrEmpty()) {
							var index = new IndexSchema() {
								TableName = pTableName,
								IndexName = pIndexName,
								Unique = pUnique,
								Columns = pColumnNames.AsQueryable(),
							};
							list.Add(index);
							pColumnNames = new List<ColumnSchema>();
						}

						var column = AllColumns.FirstOrDefault(x =>
							x.TableName == tableName
							&&
							x.ColumnName == columnName
						);
						pTableName = tableName;
						pIndexName = indexName;
						pUnique = unique;
						pColumnNames.Add(column);
					}
					if(!pIndexName.IsNullOrEmpty()) {
						var index = new IndexSchema() {
							TableName = pTableName,
							IndexName = pIndexName,
							Unique = pUnique,
							Columns = pColumnNames.AsQueryable(),
						};
						list.Add(index);
					}
					_AllIndexes = list.AsQueryable();
				}
				return _AllIndexes;
			}
		}



		private IQueryable<ProcedureSchema> _AllProcedures;
		private IQueryable<ProcedureSchema> AllProcedures
		{
			get
			{
				if(null == _AllProcedures) {
					var sql = string.Format(
@"SELECT 
	'Standalone' AS PACKAGE_NAME,
	OBJECT_NAME  AS PROCEDURE_NAME,
	OVERLOAD
FROM
	{0}_PROCEDURES
WHERE
	OBJECT_TYPE = 'PROCEDURE'

Union

SELECT
	OBJECT_NAME    AS PACKAGE_NAME,
	PROCEDURE_NAME AS PROCEDURE_NAME,
	OVERLOAD
FROM
	{0}_PROCEDURES
WHERE
	PROCEDURE_NAME IS NOT NULL
	AND
	OBJECT_TYPE = 'PACKAGE'
ORDER BY 
	PACKAGE_NAME,
	PROCEDURE_NAME,
	OVERLOAD",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<ProcedureSchema>();
					foreach(var row in rows) {
						string packageName = row.PackageName.ToString();
						string procedureName = row.ProcedureName.ToString();
						int overload = row.Overload.ToInteger(0);

						var ins = AllArguments.Where(x =>
							x.PackageName == packageName &&
							x.ProcedureName == procedureName &&
							x.Overload == overload &&
							x.InOut.In("IN", "IN/OUT")
						);
						var outs = AllArguments.Where(x =>
							x.PackageName == packageName &&
							x.ProcedureName == procedureName &&
							x.Overload == overload &&
							x.InOut.In("OUT", "IN/OUT")
						);

						var one = new ProcedureSchema() {
							PackageName = packageName,
							ProcedureName = procedureName,
							Overload = overload,
							Ins = ins,
							Outs = outs,
						};
						list.Add(one);
					}
					_AllProcedures = list.AsQueryable();
				}
				return _AllProcedures;
			}
		}


		private IQueryable<ArgumentSchema> _AllArguments;
		private IQueryable<ArgumentSchema> AllArguments
		{
			get
			{
				if(null == _AllArguments) {
					var sql = string.Format(
@"SELECT
	PACKAGE_NAME  AS PACKAGE_NAME,
	OBJECT_NAME   AS PROCEDURE_NAME,
	ARGUMENT_NAME AS ARGUMENT_NAME,
	OVERLOAD      AS OVERLOAD,
	SEQUENCE      AS SEQUENCE,
	DATA_TYPE     AS DATA_TYPE,
	IN_OUT        AS IN_OUT,
	DATA_LENGTH   AS DATA_LENGTH
FROM
	{0}_ARGUMENTS 
ORDER BY 
	PACKAGE_NAME,
	OBJECT_NAME,
	OVERLOAD,
	SEQUENCE",
						Scope
					);
					var rows = Api.ExecuteSqlByReader(sql).ToDynamics()[0];
					var list = new List<ArgumentSchema>();
					foreach(var row in rows) {
						var one = new ArgumentSchema() {
							PackageName = row.PackageName.ToString(),
							ProcedureName = row.ProcedureName.ToString(),
							ArgumentName = row.ArgumentName.ToString(),
							Overload = row.Overload.ToInteger(0),
							Sequence = row.Sequence.ToInteger(),
							DataType = row.DataType.ToString(),
							InOut = row.InOut.ToString(),
							DataLength = row.DataLength.ToInteger(),
						};
						if(one.ArgumentName.IsNullOrEmpty()) {
							continue;
						}

						list.Add(one);
					}
					_AllArguments = list.AsQueryable();
				}
				return _AllArguments;
			}
		}


		private class XPrimaryKey
		{
			public string TableName { get; set; }
			public string ColumnName { get; set; }
		}
		#endregion
	}
}
