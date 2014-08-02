// IEntity.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Kuicker.Data
{
	public interface IEntity : IDynamicRow
	{
		// EventHandler
		void OnInit();
		event EntityEventHandler BeforeAdd;
		event EntityEventHandler BeforeModify;
		event EntityEventHandler BeforeRemove;

		event InstanceEventHandler AfterSelect;
		event EntityEventHandler AfterAdd;
		event EntityEventHandler AfterModify;
		event EntityEventHandler AfterRemove;

		// schema
		string __EntityName { get; }
		string __EntityFullName { get; }
		string __TableName { get; }
		string __Alias { get; }
		Table __Table { get; }
		List<Index> __Indexes { get; }
		List<Column> __Columns { get; }
		List<Column> __KeyColumns { get; }
		Column GetColumn(string propertyOrColumnName);
		Column SaftGetColumn(string propertyOrColumnName);

		// Instance
		string __TitleValue { get; }
		string __KeyValue { get; }
		List<Difference> __History { get; }
		bool __IsDbNull(string propertyOrColumnName);

		// DDL
		bool __Alterable { get; }

		// DML
		Sql CreateSql();
		void Interceptor(Sql sql);
		bool __AllowExecuteWithoutPrimaryKeys { get; }
		bool __Addable { get; }
		bool __Modifyable { get; }
		bool __Removable { get; }
		DataResult Add();
		DataResult Add(EntityApi api);
		DataResult Modify();
		DataResult Modify(EntityApi api);
		DataResult Remove();
		DataResult Remove(EntityApi api);
		J GetJoin<J>() where J : class, IEntity, new();
		Int64 SizeOf();
		bool __EnableCache { get; }
		List<Any> __ColumnNameValues { get; }
		List<Any> __KeyColumnNameValues { get; }
		List<Any> __DirtyValues { get; }
		List<Any> __PropertyNameValues { get; }
		bool __MultiPrimaryKey { get; }

		// dynamic
		bool __EnableDynamic { get; }
	}

	public interface IEntity<T>
		: IEntity
		where T : class, IEntity<T>, new()
	{
		void Interceptor(Sql<T> sql);
	}
}
