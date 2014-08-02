// EntityCache.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace Kuicker.Data
{
	public class EntityCache
	{
		private static object __Lock = new object();
		private static int __Index = 0;
		internal static bool __Collected = false;
		private static IDictionary<string, IEntity> __Schemas;
		private static IDictionary<string, int> __TableIndex;
		private static IDictionary<string, Table> __Tables;
		private static IDictionary<string, List<Column>> __Columns;
		private static IDictionary<string, Column> __FlatColumn;
		private static IDictionary<string, IEntity> __FlatSchema;
		private static IDictionary<string, List<Column>> __FlatColumns;
		private static IDictionary<string, Table> __FlatTable;

		internal static void Collect()
		{
			lock(__Lock) {
				// Schemas
				__Schemas = Reflector
					.CollectAttributedObject<Table, IEntity>();

				// TableIndex
				__TableIndex = new Dictionary<string, int>();
				foreach(var schema in __Schemas) {
					__Index++;
					__TableIndex.Add(
						schema.Key, 
						__Index
					);
				}

				// Tables
				__Tables = new Dictionary<string, Table>();
				foreach(var schema in __Schemas) {
					var type = schema.Value.GetType();
					var table = new Table() {
						EntityName = schema.Value.__EntityName,
						TableName = schema.Value.__TableName,
						Category = type.GetCategory(),
						Description = type.GetDescription(),
						Indexes = schema.Value.__Indexes,
						IsView = type.HasAttribute<View>(),
						Follow = type.GetAttribute<DiffFollow>(),
					};
					__Tables.Add(schema.Key, table);
				}

				// Columns
				__Columns = new Dictionary<string, List<Column>>();
				foreach(var schema in __Schemas) {
					var columns = new List<Column>();
					var type = schema.Value.GetType();
					var infos = type.GetProperties();
					foreach(var info in infos) {
						var column = info.GetAttribute<Column>();
						if(null == column) { continue; }

						//
						if(column.ColumnName.IsNullOrEmpty()) {
							column.ColumnName = SqlConvention
								.ToColumnName(type, info);
						}

						column.Property = info;
						column.TableName = schema.Value.__TableName;
						column.EntityName = schema.Value.__EntityName;
						column.AllowDBNull = info.HasAttribute<AllowDBNull>();
						column.Identity = info.HasAttribute<Identity>();
						column.PrimaryKey = info.HasAttribute<PrimaryKey>();
						column.ReadOnly = info.HasAttribute<ReadOnly>();
						column.Unique = info.HasAttribute<Unique>();
						column.IgnoreDifference = info.HasAttribute<DiffIgnore>();

						var order = info.GetAttribute<Order>();
						column.Order = null == order
							? int.MaxValue
							: order.Index;

						var maxLength = info.GetAttribute<MaxLength>();
						if(null != maxLength) {
							column.MaxLength = maxLength.Size;
						}

						column.Category = info.GetCategory();
						column.Description = info.GetDescription();
						column.DefaultValue = info.GetDefaultValue();

						columns.Add(column);
					}
					columns.OrderBy(x => x.Order);
					__Columns.Add(schema.Key, columns);
				}

				__Collected = true;
			}

			// __FlatSchema
			__FlatSchema = new Dictionary<string, IEntity>();
			__FlatColumns = new Dictionary<string, List<Column>>();
			foreach(var schema in __Schemas) {
				var value = schema.Value;

				//
				__FlatSchema.SafeAdd(
					value.GetType().FullName.ToUpper(), value
				);
				__FlatSchema.SafeAdd(
					value.__EntityName.ToUpper(), value
				);
				__FlatSchema.SafeAdd(
					value.__TableName.ToUpper(), value
				);
				__FlatSchema.SafeAdd(
					value.__Alias.ToUpper(), value
				);

				//
				List<Column> columns = __Columns[schema.Key];
				__FlatColumns.SafeAdd(
					value.GetType().FullName.ToUpper(), columns
				);
				__FlatColumns.SafeAdd(
					value.__EntityName.ToUpper(), columns
				);
				__FlatColumns.SafeAdd(
					value.__TableName.ToUpper(), columns
				);
				__FlatColumns.SafeAdd(
					value.__Alias.ToUpper(), columns
				);
			}

			// __FlatTable
			__FlatTable = new Dictionary<string, Table>();
			foreach(var tableKV in __Tables) {
				var schema = __FlatSchema[tableKV.Key.ToUpper()];

				__FlatTable.SafeAdd(
					schema.GetType().FullName.ToUpper(), tableKV.Value
				);
				__FlatTable.SafeAdd(
					schema.__EntityName.ToUpper(), tableKV.Value
				);
				__FlatTable.SafeAdd(
					schema.__TableName.ToUpper(), tableKV.Value
				);
				__FlatTable.SafeAdd(
					schema.__Alias.ToUpper(), tableKV.Value
				);
			}


			// __FlatColumn
			__FlatColumn = new Dictionary<string, Column>();
			foreach(var columnsKV in __Columns) {
				foreach(var one in columnsKV.Value) {
					__FlatColumn.Add(one.Alias, one);
				}
			}
		}
		internal static void Clear()
		{
			lock(__Lock) {
				__Schemas = null;
				__TableIndex = null;
				__Tables = null;
				__Columns = null;
				__FlatColumn = null;
				__FlatSchema = null;
				__FlatColumns = null;
				__FlatTable = null;

				__Collected = false;
			}
		}

		public static IEntity Get(string name)
		{
			IEntity one;
			if(TryGet(name, out one)) { return one; }

			// Exception
			throw new Exception(
				LogRecord
					.Create()
					.SetMessage(
						"This Entity '",
						name,
						"' doesn't exists in cache."
					)
					.Error()
					.Message
			);
		}

		public static T Get<T>()
			where T : class, IEntity, new()
		{
			IEntity one = Get(typeof(T).FullName);
			return (T)one;
		}

		public static bool TryGet(string name, out IEntity one)
		{
			one = default(IEntity);
			if(!__Collected) { return false; }
			if(name.IsNullOrEmpty()) { return false; }

			return __FlatSchema.TryGetValue(
				name.ToUpper(), out one
			);
		}
		public static bool TryGet<T>(out IEntity one)
			where T : class, IEntity, new()
		{
			return TryGet(typeof(T).FullName, out one);
		}


		public static int GetTableIndex(string entityName)
		{
			IEntity one = Get(entityName);
			int index;
			if(__TableIndex.TryGetValue(
				one.__EntityFullName, out index)) {
				return index;
			}
			throw new Exception();
		}

		public static int GetTableIndex<T>()
		{
			return GetTableIndex(typeof(T).FullName);
		}

		public static string GetAlias(string entityName)
		{
			int index = GetTableIndex(entityName);
			return "as_" + index.Order(__Schemas.Count);

		}

		public static string GetAlias<T>()
		{
			return GetAlias(typeof(T).FullName);
		}

		public static Table GetTable(string entityName)
		{
			IEntity one = Get(entityName);
			Table value;
			if(__FlatTable.TryGetValue(
				one.__EntityFullName.ToUpper(), out value)) {
				return value;
			}
			throw new Exception();
		}

		public static Table GetTable<T>()
		{
			return GetTable(typeof(T).FullName);
		}

		public static List<Column> GetColumns(string entityName)
		{
			IEntity one = Get(entityName);
			List<Column> value;
			if(__FlatColumns.TryGetValue(
				one.__EntityFullName.ToUpper(), out value)) {
				return value;
			}
			throw new Exception();
		}

		public static List<Column> GetColumns<T>()
		{
			return GetColumns(typeof(T).FullName);
		}

		public static bool TryParseColumnAlias(
			string alias, out Column column)
		{
			return __FlatColumn.TryGetValue(
				alias.ToUpper(), out column
			);
		}
	}
}
