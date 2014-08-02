// OracleFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kuicker.Data
{
	public abstract class OracleFormater : SqlFormater
	{
		public override string Wildcard
		{
			get
			{
				return Symbol.Percent;
			}
		}

		public override string AssignNullMaxVarChar
		{
			get
			{
				return "empty_clob()";
			}
		}

		public override string SelectTop(
			string originalSql, int top)
		{
			base.MustBeSelectCommand(originalSql);

			return string.Format(
@"SELECT * FROM (
--// original sql
{0}
--\\ original sql
) WHERE ROWNUM <= {1}",
				originalSql,
				top
			);
		}

		public override string SelectPaging(
			string originalSql, int pageSize, int pageIndex)
		{
			base.MustBeSelectCommand(originalSql);

			return string.Format(
@"SELECT * FROM (
	SELECT ROWNUM as KN, K.* FROM (
--// original sql
{0}
--\\ original sql
	) K
	WHERE ROWNUM <= {1} * {2}
)
WHERE KN >= {1} * ({2} - 1) + 1",
				originalSql,
				pageSize,
				pageIndex
			);
		}

		public override bool SupportSelectTop { get { return false; } }
		public override bool SupportPaging { get { return false; } }

		// http://msdn.microsoft.com/en-us/library/yk72thhd(v=vs.110).aspx
		public override DataFormat ToDataFormat(string dataType)
		{
			dataType = dataType.ToUpper();

			switch(dataType) {
				case "BFILE":
					return DataFormat.ByteArray;
				case "BLOB":
					return DataFormat.ByteArray;
				case "CHAR":
					return DataFormat.String;
				case "CLOB":
					return DataFormat.String;
				case "DATE":
					return DataFormat.DateTime;
				case "FLOAT":
					return DataFormat.Decimal;
				case "INTEGER":
					return DataFormat.Decimal;
				case "INTERVAL YEAR TO MONTH":
					return DataFormat.Integer;
				case "INTERVAL DAY TO SECOND":
					return DataFormat.TimeSpan;
				case "LONG":
					return DataFormat.String;
				case "LONG RAW":
					return DataFormat.ByteArray;
				case "NCHAR":
					return DataFormat.String;
				case "NCLOB":
					return DataFormat.String;
				case "NUMBER":
					return DataFormat.Decimal;
				case "NVARCHAR2":
					return DataFormat.String;
				case "RAW":
					return DataFormat.ByteArray;
				case "REF CURSOR":
					throw new NotImplementedException();
				case "ROWID":
					return DataFormat.String;
				case "TIMESTAMP":
					return DataFormat.DateTime;
				case "TIMESTAMP WITH LOCAL TIME ZONE":
					return DataFormat.DateTime;
				case "TIMESTAMP WITH TIME ZONE":
					return DataFormat.DateTime;
				case "UNSIGNED INTEGER":
					return DataFormat.Decimal;
				case "VARCHAR2":
					return DataFormat.String;

				case "PL/SQL BOOLEAN":
					return DataFormat.Boolean;
				case "PL/SQL RECORD":
					return DataFormat.String;
				case "PLS_INTEGER":
					return DataFormat.Integer;
				case "BINARY_INTEGER":
					return DataFormat.Integer;
					
				default:
					LogRecord
						.Create()
						.SetMessage(
							"Not implemented dataType of ",
							dataType
						)
						.Error();
					return DataFormat.String;
			}
		}
	}
}
