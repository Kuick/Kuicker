// Entity.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Kuicker.Data
{
	public delegate void EntityEventHandler(
		IEntity sender, DataResult result
	);
	public delegate void InstanceEventHandler(
		IEntity sender
	);

	public class Entity : DynamicRow, IEntity
	{
		#region constants
		public const string __PrimaryKeySeparator = "__x__";
		#endregion

		#region constructor
		public Entity()
		{
			OnInit();

			if(EntityConfig.Difference) {
				if(null != Difference.Handler) {
					var follow = EntityCache.__Collected
						? __Table.Follow
						: this.GetType().GetAttribute<DiffFollow>();

					if(null != follow) {
						BeforeModify += new EntityEventHandler(DiffBeforeModify);
						BeforeRemove += new EntityEventHandler(DiffBeforeRemove);
						AfterAdd += new EntityEventHandler(DiffAfterAdd);
						AfterModify += new EntityEventHandler(DiffAfterModify);
						AfterRemove += new EntityEventHandler(DiffAfterRemove);
					}
				}
			}
		}
		#endregion

		#region Event
		public event EntityEventHandler BeforeAdd;
		public event EntityEventHandler BeforeModify;
		public event EntityEventHandler BeforeRemove;

		public event InstanceEventHandler AfterSelect;
		public event EntityEventHandler AfterAdd;
		public event EntityEventHandler AfterModify;
		public event EntityEventHandler AfterRemove;

		public virtual void OnInit() { }
		#endregion

		#region schema
		private string _EntityName;
		[XmlIgnore]
		public string __EntityName
		{
			get
			{
				if(_EntityName.IsNullOrEmpty()) {
					_EntityName = GetType().Name;
				}
				return _EntityName;
			}
		}

		private string _EntityFullName;
		[XmlIgnore]
		public string __EntityFullName
		{
			get
			{
				if(_EntityFullName.IsNullOrEmpty()) {
					_EntityFullName = GetType().FullName;
				}
				return _EntityFullName;
			}
		}

		[XmlIgnore]
		public virtual string __TableName
		{
			get
			{
				return SqlConvention.ToTableName(GetType());
			}
		}

		private string _Alias;
		[XmlIgnore]
		public virtual string __Alias
		{
			get
			{
				if(_Alias.IsNullOrEmpty()) {
					_Alias = EntityCache.GetAlias(__EntityName);
				}
				return _Alias;
			}
		}

		private Table _Table;
		[XmlIgnore]
		public Table __Table
		{
			get
			{
				if(null == _Table) {
					_Table = EntityCache.GetTable(__EntityName);
				}
				return _Table;
			}
		}

		[XmlIgnore]
		public virtual List<Index> __Indexes
		{
			get
			{
				return new List<Index>();
			}
		}

		private List<Column> _Columns;
		[XmlIgnore]
		public List<Column> __Columns
		{
			get
			{
				if(null == _Columns) {
					_Columns = EntityCache.GetColumns(__EntityName);
				}
				return _Columns;
			}
		}

		private List<Column> _KeyColumns;
		[XmlIgnore]
		public List<Column> __KeyColumns
		{
			get
			{
				if(null == _KeyColumns) {
					_KeyColumns = _Columns
						.Where(x => x.PrimaryKey)
						.ToList();
				}
				return _KeyColumns;
			}
		}

		public Column GetColumn(string propertyOrColumnName)
		{
			var column = SaftGetColumn(propertyOrColumnName);
			if(column == null) {
				throw new NullReferenceException(
					LogRecord
						.Create()
						.SetMessage(
							__TableName,
							" have no column or property ",
							"or alias named ",
							propertyOrColumnName
						)
						.Error()
						.Message
				);
			}
			return column;
		}

		public Column SaftGetColumn(string propertyOrColumnName)
		{
			if(propertyOrColumnName.IsNullOrEmpty()) {
				return null;
			}

			var pos = propertyOrColumnName.IndexOf(".");
			if(pos > 0) {
				propertyOrColumnName = propertyOrColumnName.Left(pos);
			}

			var column = __Columns.FirstOrDefault(x =>
				x.ColumnName.Equals(
					propertyOrColumnName,
					StringComparison.OrdinalIgnoreCase
				)
			);
			if(null != column) { return column; }

			column = __Columns.FirstOrDefault(x =>
				x.Property.Name.Equals(
					propertyOrColumnName,
					StringComparison.OrdinalIgnoreCase
				)
			);
			if(null != column) { return column; }

			column = __Columns.FirstOrDefault(x =>
				x.Alias.Equals(
					propertyOrColumnName,
					StringComparison.OrdinalIgnoreCase
				)
			);
			if(null != column) { return column; }

			return null;
		}
		#endregion

		#region Instance
		[XmlIgnore]
		public virtual string __TitleValue
		{
			get
			{
				var sb = new StringBuilder();
				if(__KeyColumns.IsNullOrEmpty()) {
					sb.Append(__EntityFullName);
					sb.Append("::");
					sb.Append(GetHashCode());
				} else {
					foreach(var keyColumn in __KeyColumns) {
						if(sb.Length > 0) { sb.Append("::"); }
						sb.Append(
							this.GetValue(keyColumn.PropertyName)
						);
					}
				}
				return sb.ToString();
			}
		}

		[XmlIgnore]
		public string __KeyValue
		{
			get
			{
				return __KeyColumnNameValues.Join(
					x => x.ToString(),
					__PrimaryKeySeparator
				);
			}
		}

		[XmlIgnore]
		public List<Difference> __History
		{
			get
			{
				if(EntityConfig.Difference) {
					if(null != Difference.History) {
						return Difference.History(
							__EntityName, __KeyValue
						);
					}
				}
				return new List<Difference>();
			}
		}

		public bool __IsDbNull(string propertyOrColumnName)
		{
			var column = GetColumn(propertyOrColumnName);
			return __DynamicDbNulls.Contains(column.ColumnName);
		}
		#endregion

		#region DDL
		public virtual bool __Alterable { get { return false; } }
		#endregion

		#region DML
		public Sql CreateSql() { return Sql.Create(__TableName); }
		public virtual void Interceptor(Sql sql) { }
		public virtual bool __AllowExecuteWithoutPrimaryKeys
		{ get { return false; } }
		public virtual bool __Addable { get { return true; } }
		public virtual bool __Modifyable { get { return true; } }
		public virtual bool __Removable { get { return true; } }

		public DataResult Add()
		{
			return Add(new EntityApi(__EntityName));
		}
		public DataResult Add(EntityApi api)
		{
			return api.Insert(this);
		}
		public DataResult Modify()
		{
			return Modify(new EntityApi(__EntityName));
		}
		public DataResult Modify(EntityApi api)
		{
			return api.Update(this);
		}
		public DataResult Remove()
		{
			return Remove(new EntityApi(__EntityName));
		}
		public DataResult Remove(EntityApi api)
		{
			return api.Delete(this);
		}

		public J GetJoin<J>() where J : class, IEntity, new()
		{
			J j = new J();
			foreach(Column column in j.__Columns) {
				object value;
				if(TryGetValue(column.Alias, out value)) {
					j.SetValue(column.PropertyName, value);
				}
			}
			return j;
		}
		public long SizeOf()
		{
			int size = 0;
			foreach(var column in __Columns) {
				switch(column.Format) {
					case DataFormat.Color:
					case DataFormat.Integer:
						size += sizeof(int);
						break;
					case DataFormat.Decimal:
						size += sizeof(decimal);
						break;
					case DataFormat.Long:
						size += sizeof(long);
						break;
					case DataFormat.Short:
						size += sizeof(short);
						break;
					case DataFormat.Double:
						size += sizeof(double);
						break;
					case DataFormat.Float:
						size += sizeof(float);
						break;
					case DataFormat.Boolean:
						size += sizeof(bool);
						break;
					case DataFormat.Char:
						size += sizeof(char);
						break;
					case DataFormat.Byte:
						size += sizeof(byte);
						break;
					case DataFormat.ByteArray:
						byte[] bs = this
							.GetValue(column.PropertyName) as byte[];
						if(null != bs) {
							size += sizeof(byte) * bs.Length;
						}
						break;
					case DataFormat.DateTime:
						size += 8;
						break;
					case DataFormat.Guid:
						size += 8; // ?
						break;
					case DataFormat.Unknown:
					case DataFormat.Object:
					case DataFormat.Objects:
					case DataFormat.String:
					case DataFormat.Enum:
						object v = this.GetValue(column.PropertyName);
						if(null == v) { continue; }
						string value = v.ToString();
						size += ASCIIEncoding.Unicode.GetByteCount(
							value
						);
						break;
					case DataFormat.TimeSpan:
						size += Marshal.SizeOf(typeof(TimeSpan));
						break;

					default:
						break;
				}
			}
			return size;
		}
		[XmlIgnore]
		public bool __EnableCache
		{
			get { throw new NotImplementedException(); }
		}

		[XmlIgnore]
		public List<Any> __ColumnNameValues
		{
			get
			{
				var list = new List<Any>();
				foreach(var column in __Columns) {
					var columnName = column.ColumnName;
					var columnValue = this.GetValue(
						column.PropertyName
					);
					list.Add(columnName, columnValue);
				}
				return list;
			}
		}

		[XmlIgnore]
		public List<Any> __KeyColumnNameValues
		{
			get
			{
				var columns = __KeyColumns.IsNullOrEmpty()
					? __Columns
					: __KeyColumns;
				var list = new List<Any>();
				foreach(var column in columns) {
					var columnName = column.ColumnName;
					var columnValue = this.GetValue(
						column.PropertyName
					);
					list.Add(columnName, columnValue);
				}
				return list.OrderBy(x => x.Name).ToList();
			}
		}

		[XmlIgnore]
		public List<Any> __DirtyValues
		{
			get
			{
				if(!__DatabaseFirst) { return __ColumnNameValues; }

				var list = new List<Any>();
				foreach(Any any in __ColumnNameValues) {
					int hashCode = null == any.Value
						? 0 : any.Value.GetHashCode();
					int originalHashCode =
						__OriginalHashCodes.ToInteger(any.Name);
					if(originalHashCode != hashCode) {
						list.Add(any);
					}
				}
				return list;
			}
		}

		[XmlIgnore]
		public List<Any> __PropertyNameValues
		{
			get
			{
				var list = new List<Any>();
				foreach(var column in __Columns) {
					var propertyName = column.PropertyName;
					var propertyValue = this.GetValue(
						column.PropertyName
					);
					list.Add(propertyName, propertyValue);
				}
				return list;
			}
		}

		public bool __MultiPrimaryKey
		{
			get
			{
				return
					!__KeyColumns.IsNullOrEmpty()
					&&
					__KeyColumns.Count() > 1;
			}
		}
		#endregion

		#region dynamic
		[XmlIgnore]
		public virtual bool __EnableDynamic
		{
			get
			{
				return false;
			}
		}
		#endregion

		#region static
		public static List<Difference> GetHistory(
			string tableOrEntityName, string primaryKey)
		{
			if(EntityConfig.Difference) {
				if(null != Difference.History) {
					var schema = EntityCache.Get(tableOrEntityName);
					return Difference.History(
						schema.__EntityName, primaryKey
					);
				}
			}
			return new List<Difference>();
		}

		public static Difference CompareDiff(IEntity from, IEntity to)
		{
			if(null == from && null == to) { return null; }
			Type typeFrom = null == from ? null : from.GetType();
			Type typeTo = null == to ? null : to.GetType();

			if(null != from && null != to) {
				if(typeFrom != typeTo) {
					throw new Exception(
						LogRecord
							.Create()
							.SetMessage(
								typeFrom.Name,
								" vs. ",
								typeTo.Name,
								", different Entity types ",
								"can not be compared."
							)
							.Error()
							.Message
					);
				}
			}

			bool needCompare = null != from && null != to;
			var schema = null == from
				? to.__Table
				: from.__Table;
			var primaryValue = null == from
				? to.__KeyValue
				: from.__KeyValue;

			Difference diff = new Difference() {
				EntityName = schema.EntityName,
				PrimaryValue = primaryValue,
			};
			foreach(Column column in schema.Columns) {
				if(column.IgnoreDifference) { continue; }

				string val1 = null == from
					? null
					: from.GetValue(column.PropertyName).ToStringX();
				string val2 = null == to
					? null
					: to.GetValue(column.PropertyName).ToStringX();
				if(needCompare ? val1 == val2 : false) { continue; }

				DiffValue diffValue = new DiffValue() {
					ColumnName = column.ColumnName,
					OriginalValue = val1,
					OriginalIsNull =
						null == from 
						|| 
						from.__IsDbNull(column.ColumnName),
					CurrentValue = val2,
					CurrentIsNull =
						null == to 
						|| 
						to.__IsDbNull(column.ColumnName),
				};
				diff.Values.Add(diffValue);
			}

			return diff;
		}

		public static List<Any> BuildKeyColumnNameValues(
			string tableOrEntityName,
			string keyValue)
		{
			var schema = EntityCache.Get(tableOrEntityName);
			var keys = keyValue.SplitAndTrim(__PrimaryKeySeparator);
			if(schema.__KeyColumns.Count != keys.Length) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"Primary keys number does not match."
						)
						.Error()
						.Message
				);
			}

			var list = new List<Any>();
			int index = -1;
			foreach(var column in schema.__KeyColumns) {
				list.Add(column.ColumnName, keys[++index]);
			}
			return list;
		}
		#endregion

		#region event handler
		internal void InvokeBeforeAdd(DataResult result)
		{
			if(null != BeforeAdd) { BeforeAdd(this, result); }
		}
		internal void InvokeBeforeModify(DataResult result)
		{
			if(null != BeforeModify) { BeforeModify(this, result); }
		}
		internal void InvokeBeforeRemove(DataResult result)
		{
			if(null != BeforeRemove) { BeforeRemove(this, result); }
		}
		internal void InvokeAfterSelect()
		{
			if(null != AfterSelect) { AfterSelect(this); }
		}
		internal void InvokeAfterAdd(DataResult result)
		{
			if(null != AfterAdd) { AfterAdd(this, result); }
		}
		internal void InvokeAfterModify(DataResult result)
		{
			if(null != AfterModify) { AfterModify(this, result); }
		}
		internal void InvokeAfterRemove(DataResult result)
		{
			if(null != AfterRemove) { AfterRemove(this, result); }
		}

		private IEntity _BeforeModify;
		private void DiffBeforeModify(IEntity sender, DataResult result)
		{
			if(!result.Success) { return; }
			if(null == sender.__Table.Follow) { return; }
			if(sender.__Table.Follow.Modify) {
				_BeforeModify = Get(
					sender.__Table.TableName,
					sender.__KeyColumnNameValues.ToArray()
				);
			}
		}
		private IEntity _BeforeRemove;
		private void DiffBeforeRemove(IEntity sender, DataResult result)
		{
			if(!result.Success) { return; }
			if(null == sender.__Table.Follow) { return; }
			if(sender.__Table.Follow.Remove) {
				_BeforeRemove = Get(
					sender.__Table.TableName,
					sender.__KeyColumnNameValues.ToArray()
				);
			}
		}
		private void DiffAfterAdd(IEntity sender, DataResult result)
		{
			if(!result.Success) { return; }
			if(null == sender.__Table.Follow) { return; }
			if(sender.__Table.Follow.Add) {
				Difference.Handler(
					DiffMethod.Add, null, this
				);
			}
		}
		private void DiffAfterModify(IEntity sender, DataResult result)
		{
			if(!result.Success) { return; }
			if(null == sender.__Table.Follow) { return; }
			if(sender.__Table.Follow.Modify) {
				Difference.Handler(
					DiffMethod.Modify, _BeforeModify, this
				);
			}
		}
		private void DiffAfterRemove(IEntity sender, DataResult result)
		{
			if(!result.Success) { return; }
			if(null == sender.__Table.Follow) { return; }
			if(sender.__Table.Follow.Remove) {
				Difference.Handler(
					DiffMethod.Remove, _BeforeRemove, null
				);
			}
		}
		#endregion

		#region Execute
		public static IQueryable<IEntity> All(
			string tableName)
		{
			return Sql
				.CreateSelect(tableName)
				.Query();
		}
		public static IQueryable<IEntity> All(
			EntityApi api, string tableName)
		{
			return Sql
				.CreateSelect(tableName)
				.Query(api);
		}

		public static int Count(
			string tableName)
		{
			return Sql
				.CreateSelect(tableName)
				.Count();
		}
		public static int Count(
			EntityApi api, string tableName)
		{
			return Sql
				.CreateSelect(tableName)
				.Count(api);
		}

		public static bool Exists(
			string tableName, params Any[] conditions)
		{
			return Sql
				.CreateSelect(tableName)
				.Where(conditions)
				.Exists();
		}
		public static bool Exists(
			EntityApi api, string tableName, params Any[] conditions)
		{
			return Sql
				.CreateSelect(tableName)
				.Where(conditions)
				.Exists(api);
		}

		public static IEntity Get(
			string tableName, params Any[] conditions)
		{
			return Sql
				.CreateSelect(tableName)
				.Where(conditions)
				.QueryFirst();
		}
		public static IEntity Get(
			EntityApi api, string tableName, params Any[] conditions)
		{
			return Sql
				.CreateSelect(tableName)
				.Where(conditions)
				.QueryFirst(api);
		}
		#endregion
	}


	public class Entity<T>
		: Entity, IEntity<T>
		where T : class, IEntity<T>, new()
	{
		public virtual void Interceptor(Sql<T> sql)
		{
			base.Interceptor(sql);
		}

		public new static Sql<T> CreateSql()
		{
			return Sql<T>.Create();
		}

		public static Result Add(T first, params T[] others)
		{
			if(null == first) { return Result.BuildFailure(); }
			T schema = EntityCache.Get<T>();

			using(var api = new EntityApi(schema.__EntityName)) {
				Result result = api.Insert(first);
				if(others.IsNullOrEmpty()) { return result; }
				foreach(var one in others) {
					Result innerResult = api.Insert(one);
					result.InnerResults.Add(innerResult);
				}
				return result;
			}
		}
		public static Result Remove(T first, params T[] others)
		{
			if(null == first) { return Result.BuildFailure(); }
			T schema = EntityCache.Get<T>();

			using(var api = new EntityApi(schema.__EntityName)) {
				Result result = api.Delete(first);
				if(others.IsNullOrEmpty()) { return result; }
				foreach(var one in others) {
					Result innerResult = api.Delete(one);
					result.InnerResults.Add(innerResult);
				}
				return result;
			}
		}
		public static Result Modify(T first, params T[] others)
		{
			if(null == first) { return Result.BuildFailure(); }
			T schema = EntityCache.Get<T>();

			using(var api = new EntityApi(schema.__EntityName)) {
				Result result = api.Update(first);
				if(others.IsNullOrEmpty()) { return result; }
				foreach(var one in others) {
					Result innerResult = api.Update(one);
					result.InnerResults.Add(innerResult);
				}
				return result;
			}
		}

		public static IQueryable<T> All()
		{
			return Sql<T>
				.CreateSelect()
				.Query();
		}
		public static IQueryable<T> All(EntityApi api)
		{
			return Sql<T>
				.CreateSelect()
				.Query(api);
		}

		public static int Count()
		{
			return Sql<T>
				.CreateSelect()
				.Count();
		}
		public static int Count(
			EntityApi api)
		{
			return Sql<T>
				.CreateSelect()
				.Count(api);
		}

		public static bool Exists(
			params Any[] conditions)
		{
			return Sql<T>
				.CreateSelect()
				.Where(conditions)
				.Exists();
		}
		public static bool Exists(
			EntityApi api, params Any[] conditions)
		{
			return Sql<T>
				.CreateSelect()
				.Where(conditions)
				.Exists(api);
		}

		public static T Get(
			params Any[] conditions)
		{
			return Sql<T>
				.CreateSelect()
				.Where(conditions)
				.QueryFirst();
		}
		public static T Get(
			EntityApi api, params Any[] conditions)
		{
			return Sql<T>
				.CreateSelect()
				.Where(conditions)
				.QueryFirst(api);
		}

		public static Sql<T> Where(
			Expression<Func<T, object>> expression)
		{
			return CreateSql().Where(expression);
		}

		public static IQueryable<T> EmptyQueryable
		{
			get
			{
				return Enumerable.Empty<T>().AsQueryable();
			}
		}
	}
}
