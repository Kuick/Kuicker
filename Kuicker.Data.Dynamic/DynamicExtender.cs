// DynamicExtender.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

namespace Kuicker.Data
{
	public static class DynamicExtender
	{
		public static List<DynamicColumn> GetColumns(
			this DbDataReader reader)
		{
			var list = new List<DynamicColumn>();
			if(null == reader) { return list; }

			var schema = reader.GetSchemaTable();

			foreach(DataRow row in schema.Rows) {
				DynamicColumn column = new DynamicColumn();
				foreach(DataColumn col in schema.Columns) {
					//Console.WriteLine(string.Format(
					//	"{0}={1}", col.ColumnName, row[col]
					//));
					switch(col.ColumnName) {
						case "AllowDBNull":
							column.AllowDBNull = row[col].ToBoolean();
							break;
						case "AutoIncrement":
						case "IsAutoIncrement":
							column.AutoIncrement = row[col].ToBoolean();
							break;
						case "AutoIncrementSeed":
							column.AutoIncrementSeed = row[col].ToLong();
							break;
						case "AutoIncrementStep":
							column.AutoIncrementStep = row[col].ToLong();
							break;
						case "Caption":
							column.Caption = row[col].ToString();
							break;
						case "ColumnName":
							column.ColumnName = row[col].ToString();
							break;
						case "BaseColumnName":
							column.BaseColumnName = row[col].ToString();
							break;
						case "IsAliased":
							column.IsAliased = row[col].ToBoolean();
							break;
						case "DataType":
							column.DataType = Reflector.GetPrimitiveType(
								row[col].ToString()
							);
							break;
						case "DefaultValue":
							column.DefaultValue = row[col];
							break;
						case "Expression":
							column.Expression = row[col].ToString();
							break;
						case "MaxLength":
							column.MaxLength = row[col].ToInteger();
							break;
						case "Namespace":
							column.Namespace = row[col].ToString();
							break;
						case "Prefix":
							column.Prefix = row[col].ToString();
							break;
						case "ReadOnly":
							column.ReadOnly = row[col].ToBoolean();
							break;
						case "Unique":
							column.Unique = row[col].ToBoolean();
							break;
						case "BaseTableName":
							column.BaseTableName = row[col].ToString();
							break;
						case "IsKey":
							column.IsKey = row[col].ToBoolean();
							break;
						case "IsRowID":
						case "IsRowVersion":
							column.IsRowID = row[col].ToBoolean();
							break;
						case "ProviderType":
							column.ProviderType = row[col].ToInteger();
							break;
						case "IsHidden":
							column.IsHidden = row[col].ToBoolean();
							break;
						case "IsLong":
							column.IsLong = row[col].ToBoolean();
							break;
						default:
							break;
					}
				}
				list.Add(column);
			}
			return list;
		}

		public static List<dynamic> ToDynamics(
			this DbDataReader reader)
		{
			return ToDynamics(reader, -1, -1);
		}
		public static List<dynamic> ToDynamics(
			this DbDataReader reader, int pageSize, int pageIndex)
		{
			var list = new List<dynamic>();
			if(null == reader) { return list; }
			bool paging = pageSize > 0 && pageIndex > 0;
			int index = 0;

			var schema = reader.GetSchemaTable();
			var tableName = schema.TableName;

			var columns = new List<string>();
			for(int i = 0; i < reader.FieldCount; i++) {
				string s = reader.GetName(i);
				columns.Add(s);
			}

			var dbValues = new object[reader.FieldCount];
			while(reader.Read()) {
				index++;
				if(paging) {
					if((pageIndex - 1) * pageSize > index) {
						continue;
					}
					if(pageIndex * pageSize > index) {
						break;
					}
				}

				reader.GetValues(dbValues);

				//var row = schema.NewRow();
				var values = new List<Any>();
				var hashCodes = new List<Any>();
				var dbNulls = new List<string>();

				var columnIndex = -1;
				foreach(var column in columns) {
					columnIndex++;
					var value = dbValues[columnIndex];
					if(DBNull.Value == value) {
						dbNulls.Add(column);
						value = null;
					}
					values.Add(column, value);

					if(null != value) {
						int hashCode = value.GetHashCode();
						hashCodes.Add(column, hashCode);
					}
				}

				var data = new DynamicRow() {
					__ReturnAny = true,
					__DatabaseFirst = true,
					__TableName = tableName,
					__Values = values,
					__HashCodes = hashCodes,
					__DbNulls = dbNulls,
				};
				data.__OriginalHashCode = data.GetHashCode();

				if(dbNulls.Any()) {
					foreach(var dbNull in dbNulls) {
						hashCodes.Add(
							dbNull, 
							data.__OriginalHashCode
						);
					}
				}
				data.__HashCodes = hashCodes;

				list.Add(data as dynamic);
			}
			return list;
		}
	}
}
