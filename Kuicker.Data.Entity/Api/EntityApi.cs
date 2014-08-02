// EntityApi.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Kuicker.Data
{
	public sealed class EntityApi : DynamicApi
	{
		public EntityApi()
			: this(string.Empty)
		{
		}

		public EntityApi(string name)
			: base(name)
		{
		}

		private static IDictionary<string, ISqlBuilder> _Builders;
		public static IDictionary<string, ISqlBuilder> Builders
		{
			get
			{
				if(_Builders.IsNullOrEmpty()) {
					_Builders = Reflector
						.CollectImplementedObject<ISqlBuilder>();
				}
				return _Builders;
			}
		}

		private static IDictionary<string, ISqlFormater> _Formaters;
		public static IDictionary<string, ISqlFormater> Formaters
		{
			get
			{
				if(_Formaters.IsNullOrEmpty()) {
					_Formaters = Reflector
						.CollectImplementedObject<ISqlFormater>();
				}
				return _Formaters;
			}
		}

		private ISqlBuilder _Builder;
		public ISqlBuilder Builder
		{
			get
			{
				if(null == _Builder) {
					var one = Builders.FirstOrDefault(x =>
						x.Value.ProviderName
						==
						base.Settings.ProviderName
					);
					if(one.Key.IsNullOrEmpty()) {
						throw new NullReferenceException(
							LogRecord
								.Create()
								.SetMessage("No such Entity provider")
								.Add(
									"ProviderName",
									 base.Settings.ProviderName
								)
								.Fatal()
								.Message
						);
					}

					_Builder = one.Value;
					_Builder.Api = this;
					_Builder.Formator = Formater;
				}
				return _Builder;
			}
		}

		private ISqlFormater _Formater;
		public ISqlFormater Formater
		{
			get
			{
				if(null == _Formater) {
					var one = Formaters.FirstOrDefault(x =>
						x.Value.ProviderName
						==
						base.Settings.ProviderName
					);
					if(one.Key.IsNullOrEmpty()) {
						throw new NullReferenceException(
							LogRecord
								.Create()
								.SetMessage("No such Entity provider")
								.Add(
									"ProviderName",
									 base.Settings.ProviderName
								)
								.Fatal()
								.Message
						);
					}

					_Formater = one.Value;
				}
				return _Formater;
			}
		}

		public IQueryable<T> QueryBySql<T>(
			string sql,
			params DbParameter[] parameters)
			where T : class, IEntity<T>, new()
		{
			return QueryBySql<T>(sql, -1, -1, parameters);
		}
		public IQueryable<IEntity> QueryBySql(
			Type type,
			string sql,
			params DbParameter[] parameters)
		{
			return QueryBySql(type, sql, -1, -1, parameters);
		}

		public IQueryable<T> QueryBySql<T>(
			string sql,
			int pageSize,
			int pageIndex,
			params DbParameter[] parameters)
			where T : class, IEntity<T>, new()
		{
			return Query<T>(
					base.ExecuteSqlByReader(sql, parameters),
					pageSize,
					pageIndex
				);
		}

		public IQueryable<IEntity> QueryBySql(
			Type type,
			string sql,
			int pageSize,
			int pageIndex,
			params DbParameter[] parameters)
		{
			return Query(
				type,
				base.ExecuteSqlByReader(sql, parameters),
				pageSize,
				pageIndex
			);
		}

		public IQueryable<T> QueryBySp<T>(
			string spName,
			params DbParameter[] parameters)
			where T : class, IEntity<T>, new()
		{
			return QueryBySp<T>(spName, -1, -1, parameters);
		}

		public IQueryable<IEntity> QueryBySp(
			Type type,
			string spName,
			params DbParameter[] parameters)
		{
			return QueryBySp(type, spName, -1, -1, parameters);
		}

		public IQueryable<T> Query<T>(SqlParseResult result)
			where T : class, IEntity<T>, new()
		{
			return Query(result)
				.ToList()
				.ConvertAll(x => x as T)
				.AsQueryable();
		}

		public IQueryable<IEntity> Query(SqlParseResult result)
		{
			if(result.Paging) {
				return QueryBySql(
					result.Sql.Schema.GetType(),
					result.Command,
					result.Sql.PageSize,
					result.Sql.PageIndex,
					result.AllParameters.ToArray()
				);
			} else {
				return QueryBySql(
					result.Sql.Schema.GetType(),
					result.Command,
					result.AllParameters.ToArray()
				);
			}
		}

		public IQueryable<T> QueryBySp<T>(
			string spName,
			int pageSize,
			int pageIndex,
			params DbParameter[] parameters)
			where T : class, IEntity<T>, new()
		{
			return Query<T>(
				base.ExecuteSpByReader(spName, parameters),
				pageSize,
				pageIndex
			);
		}

		public IQueryable<IEntity> QueryBySp(
			Type type,
			string spName,
			int pageSize,
			int pageIndex,
			params DbParameter[] parameters)
		{
			return Query(
				type,
				base.ExecuteSpByReader(spName, parameters),
				pageSize,
				pageIndex
			);
		}

		private IQueryable<T> Query<T>(
			DbDataReader r, int pageSize, int pageIndex)
			where T : class, IEntity<T>, new()
		{
			return Query(typeof(T), r, pageSize, pageIndex)
				.ToList()
				.ConvertAll(x => x as T)
				.AsQueryable();
		}

		private IQueryable<IEntity> Query(
			Type type, DbDataReader r, int pageSize, int pageIndex)
		{
			using(var il = new ILogger()) {
				var list = new List<IEntity>();

				using(var reader = new SqlReader(r)) {

					IEntity schema = EntityCache.Get(type.Name);
					// dynamic
					var dc = schema.__EnableDynamic
						? r.GetColumns() : null;

					while(reader.Read()) {
						IEntity one = Reflector
							.CreateInstance(type) as IEntity;
						reader.Bind(il, one);
						list.Add(one);

						// dynamic
						if(null != dc) {
							var row = one as DynamicRow;
							r.CurrentBindDynamicRow(dc, ref row);
						}
					}
				}

				// AfterSelect
				foreach(var instance in list) {
					var one = instance as Entity;
					if(null != one) {
						one.InvokeAfterSelect();
					}
				}

				return list.AsQueryable();
			}
		}

		public DataResult Insert(params IEntity[] instances)
		{
			var result = DataResult.BuildSuccess();
			foreach(var instance in instances) {
				var one = instance as Entity;

				// Addable
				if(!instance.__Addable) {
					result.InnerResults.Add(new DataResult() {
						Success = false,
						Message =
							new[]{
								"Not allow to insert: ",
								instance.__TitleValue,
							}.Join(),
					});
					continue;
				}

				// BeforeAdd
				if(null != one) {
					var r = new DataResult();
					one.InvokeBeforeAdd(r);
					if(!r.Success) {
						r.Message = new[]{
							"BeforeAdd return failure: ",
							instance.__TitleValue,
						}.Join();
						result.InnerResults.Add(r);
						continue;
					}
				}

				// Execute
				var insertR = base.Insert(
					instance.__TableName, 
					instance.__ColumnNameValues
				);
				result.InnerResults.Add(insertR);


				// AfterAdd
				if(null != one) {
					var r = new DataResult();
					one.InvokeAfterAdd(r);
					if(!r.Success) {
						r.Message = new[]{
							"AfterAdd return failure: ",
							instance.__TitleValue,
						}.Join();
						result.InnerResults.Add(r);
						continue;
					}
				}
			}
			return result;
		}

		public DataResult Update(params IEntity[] instances)
		{
			var result = DataResult.BuildSuccess();
			foreach(var instance in instances) {
				var one = instance as Entity;

				// AllowExecuteWithoutPrimaryKeys
				if(!one.__KeyColumns.Any() && !one.__AllowExecuteWithoutPrimaryKeys) {
					result.InnerResults.Add(new DataResult() {
						Success = false,
						Message =
							new[]{
								instance.__EntityFullName,
								" (",
								instance.__TitleValue,
								") ",
								" not allow to update ",
								"without primary keys.",
							}.Join(),
					});
					continue;
				}

				// Modifyable
				if(!instance.__Modifyable) {
					result.InnerResults.Add(new DataResult() {
						Success = false,
						Message =
							new[]{
								"Not allow to update: ",
								instance.__TitleValue,
							}.Join(),
					});
					continue;
				}

				// BeforeModify
				if(null != one) {
					var r = new DataResult();
					one.InvokeBeforeModify(r);
					if(!r.Success) {
						r.Message = new[]{
							"BeforeModify return failure: ",
							instance.__TitleValue,
						}.Join();
						result.InnerResults.Add(r);
						continue;
					}
				}

				// Execute
				var exeResult = base.Update(
					instance.__TableName,
					instance.__DirtyValues,
					instance.__KeyColumnNameValues
				);
				result.InnerResults.Add(exeResult);

				// AfterModify
				if(null != one) {
					var r = new DataResult();
					one.InvokeAfterModify(r);
					if(!r.Success) {
						r.Message = new[]{
							"AfterModify return failure: ",
							instance.__TitleValue,
						}.Join();
						result.InnerResults.Add(r);
						continue;
					}
				}
			}
			return result;
		}

		public DataResult Delete(params IEntity[] instances)
		{
			var result = DataResult.BuildSuccess();
			foreach(var instance in instances) {
				var one = instance as Entity;

				// AllowExecuteWithoutPrimaryKeys
				if(!one.__AllowExecuteWithoutPrimaryKeys) {
					result.InnerResults.Add(new DataResult() {
						Success = false,
						Message =
							new[]{
								instance.__EntityFullName,
								" (",
								instance.__TitleValue,
								") ",
								" not allow to delete ",
								"without primary keys.",
							}.Join(),
					});
					continue;
				}

				// Removable
				if(!instance.__Removable) {
					result.InnerResults.Add(new DataResult() {
						Success = false,
						Message =
							new[]{
								"Not allow to delete: ",
								instance.__TitleValue,
							}.Join(),
					});
					continue;
				}

				// BeforeRemove
				if(null != one) {
					var r = new DataResult();
					one.InvokeBeforeRemove(r);
					if(!r.Success) {
						r.Message = new[]{
							"BeforeRemove return failure: ",
							instance.__TitleValue,
						}.Join();
						result.InnerResults.Add(r);
						continue;
					}
				}

				// Execute
				var exeResult = base.Delete(
					instance.__TableName,
					instance.__KeyColumnNameValues
				);
				result.InnerResults.Add(exeResult);

				// AfterRemove
				if(null != one) {
					var r = new DataResult();
					one.InvokeAfterRemove(r);
					if(!r.Success) {
						r.Message = new[]{
							"AfterRemove return failure: ",
							instance.__TitleValue,
						}.Join();
						result.InnerResults.Add(r);
						continue;
					}
				}
			}
			return result;
		}


		public DataResult Execute(SqlParseResult result)
		{
			if(!result.Success) {
				var r = new DataResult() {
					Message = result.Message,
					Exception = result.Exception,
					Success = result.Success,
					TransactionID = base.TransactionID,
					ExecuteMethod = "EntityApi.Execute",
					CommandType = CommandType.Text,
					AffectedCount = 0,
					ScalarValue = null,
					Command = result.Command,
				};
				r.Datas.AddRange(result.Datas);
				r.Parameters.AddRange(result.AllParameters);
			}
			return ExecuteSql(
				result.Command, 
				result.AllParameters.ToArray()
			);
		}

	}
}
