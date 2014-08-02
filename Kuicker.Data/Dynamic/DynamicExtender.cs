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

		public static List<string> TableNames(
			this List<DynamicColumn> columns)
		{
			if(columns.IsNullOrEmpty()) { return new List<string>();}
			var tableNames = columns
				.Where(x => !x.BaseTableName.IsNullOrEmpty())
				.Select(x => x.BaseTableName)
				.Distinct()
				.ToArray();
			return new List<string>(tableNames);
		}

		public static List<string> PrimaryKeys(
			this List<DynamicColumn> columns)
		{
			if(columns.IsNullOrEmpty()) { return new List<string>(); }
			var primaryKeys = columns
				.Where(x => x.IsKey && x.BaseColumnName.IsNullOrEmpty())
				.Select(x => x.BaseColumnName)
				.Distinct()
				.ToArray();
			return new List<string>(primaryKeys);
		}

		public static List<dynamic>[] ToDynamics(
			this DbDataReader reader)
		{
			return ToDynamics(reader, -1, -1);
		}
		public static List<dynamic>[] ToDynamics(
			this DbDataReader reader, int pageSize, int pageIndex)
		{
			var all = new List<List<dynamic>>();
			if(null == reader || !reader.HasRows) {
				return all.ToArray();
			}
			bool paging = pageSize > 0 && pageIndex > 0;

			do {
				var list = new List<dynamic>();
				int index = 0;
				var columns = reader.GetColumns();
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

					DynamicRow row = new DynamicRow();
					reader.CurrentBindDynamicRow(columns, ref row);
					list.Add(row as dynamic);
				}
				all.Add(list);
			}
			while(reader.NextResult());

			return all.ToArray();
		}

		public static void CurrentBindDynamicRow(
			this DbDataReader reader, 
			List<DynamicColumn> columns,
			ref DynamicRow row)
		{
			if(null == reader) { return; }
			if(columns.IsNullOrEmpty()) { return; }

			var dbValues = new object[columns.Count];
			reader.GetValues(dbValues);

			var flatValues = new List<Any>();
			var values = new List<Any>();
			var hashCodes = new List<Any>();
			var dbNulls = new List<string>();
			var columnIndex = -1;

			foreach(var column in columns) {
				columnIndex++;
				var value = dbValues[columnIndex];
				if(DBNull.Value == value) {
					dbNulls.Add(column.BaseColumnName);
					value = null;
				}
				flatValues.SafeAdd(column.ColumnName, value);
				flatValues.SafeAdd(column.BaseColumnName, value);
				values.Add(column.BaseColumnName, value);


				if(null != value) {
					int hashCode = value.GetHashCode();
					hashCodes.Add(
						column.BaseColumnName, hashCode
					);
				}
			}

			row.__ReturnAny = true;
			row.__DatabaseFirst = true;
			row.__DynamicColumns = columns;
			row.__FlatValues = flatValues;
			row.__DynamicValues = values;
			row.__DynamicOriginalValues = values;
			row.__OriginalHashCodes = hashCodes;
			row.__DynamicDbNulls = dbNulls;
			row.__OriginalHashCode = row.GetHashCode();

			if(dbNulls.Any()) {
				foreach(var dbNull in dbNulls) {
					hashCodes.Add(
						dbNull,
						row.__OriginalHashCode
					);
				}
			}
			row.__OriginalHashCodes = hashCodes;
		}
	}
}
