// EntityExtender.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Kuicker.Data
{
	public static class EntityExtender
	{
		#region String
		public static string Replace(
			this string template, params Entity[] instances)
		{
			StringBuilder sb = new StringBuilder(template);
			if(null != instances) {
				foreach(Entity instance in instances) {
					foreach(Column column in instance.__Columns) {
						string value = instance
							.GetValue(column.ColumnName).ToString();
						sb.Replace( // {T_USER.NAME}
							string.Concat(
								Symbol.OpenBrace, 
								column.DbFullName, 
								Symbol.CloseBrace
							), 
							value
						);
						sb.Replace( // {UserEntity.Name}
							string.Concat(
								Symbol.OpenBrace,
								column.EntityFullName,
								Symbol.CloseBrace
							),
							value
						);
					}
				}
			}
			return sb.ToString();
		}

		public static string Replace(
			this string template, params Any[] anys)
		{
			StringBuilder sb = new StringBuilder(template);
			if(!anys.IsNullOrEmpty()) {
				foreach(Any any in anys) {
					sb.Replace( // {Name}
						string.Concat(
							Symbol.OpenBrace,
							any.Name, 
							Symbol.CloseBrace
						), 
						any.ToString()
					);
				}
			}
			return sb.ToString();
		}
		#endregion

		#region IEntity[]
		public static string ToJson(this List<IEntity> objs)
		{
			if(objs == null || objs.Count == 0) { return "[]"; }
			StringBuilder sb = new StringBuilder();
			sb.Append("[");
			foreach(IEntity obj in objs) {
				if(sb.Length > 1) { sb.Append(","); }
				if(obj == null) { sb.Append("{}"); }
				sb.Append(obj.ToJson());
			}
			sb.Append("]");
			return sb.ToString();
		}
		#endregion

		#region SqlDataType
		//public static bool IsDBCS(this SqlDataType dbType)
		//{
		//	return dbType.EnumIn(
		//		SqlDataType.MaxVarWChar,
		//		SqlDataType.VarWChar,
		//		SqlDataType.WChar
		//	);
		//}
		#endregion

		#region DataSet, DataTable, DataRow
		//public static List<IEntity> ToDynamic(this DataSet ds)
		//{
		//	if(null == ds || ds.Tables.Count == 0) {
		//		return new List<IEntity>();
		//	}
		//	return ds.Tables[0].ToDynamic();
		//}
		//public static List<IEntity> ToDynamic(this DataTable table)
		//{
		//	if(null == table || table.Rows.Count == 0) {
		//		return new List<IEntity>();
		//	}

		//	List<IEntity> list = new List<IEntity>();
		//	foreach(DataRow row in table.Rows) {
		//		list.Add(row.ToDynamic());
		//	}

		//	return list;
		//}

		//public static IEntity ToDynamic(this DataRow row)
		//{
		//	IEntity data = new DynamicData();
		//	data.Row = row;
		//	return data;
		//}

		public static List<T> ToEntity<T>(this DataSet ds)
			where T : class, IEntity, new()
		{
			if(null == ds || ds.Tables.Count < 1) { return null; }
			DataTable dt = ds.Tables[0];
			return dt.ToEntity<T>();
		}

		public static List<T> ToEntity<T>(this DataTable dt)
			where T : class, IEntity, new()
		{
			try {
				if(null == dt || dt.Rows.Count == 0) {
					return new List<T>();
				}

				List<T> list = new List<T>();
				foreach(DataRow row in dt.Rows) {
					T one = row.ToEntity<T>();
					list.Add(one);
				}
				return list;
			} catch {
				return new List<T>();
			}
		}

		public static T ToEntity<T>(this DataRow row)
			where T : class, IEntity, new()
		{
			#region current
			if(null == row) { return default(T); }
			T one = new T();
			//one.Row = row;
			foreach(Column column in one.__Columns) {
				try {
					one.SetValue(
						column.ColumnName, 
						row[column.ColumnName]
					);
				} catch {
					// swallow
				}
			}
			return one;
			#endregion
		}
		#endregion

		#region ToDataTable
		//public static DataTable ToDataTable<T>(this List<T> instances)
		//	where T : IEntity
		//{
		//	if(null == instances) { return null; }
		//	if(instances.Count == 0) { return new DataTable(); }
		//	return instances[0].DataTable;
		//}
		#endregion
		
		#region In
		public static bool In(this string value, Sql sql)
		{
			// for expression only
			return true;
		}
		public static bool In(this int value, Sql sql)
		{
			// for expression only
			return true;
		}
		#endregion
	}
}
