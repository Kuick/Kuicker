// IDataApi.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Kuicker
{
	// Unit of Work
	// ADO.NET
	public interface IDataApi : IDisposable
	{
		// Trasaction: TrasactionScope
		// prefix    : Execute
		// in        : sql|sp + parameters
		// out       : affectedRow(Result), Scalar, IDataReader
		// Naming    : Execute{In}[By{Out}]

		IDataProviderSetting Settings { get; }
		DbProviderFactory Factory { get; }
		DbCommandBuilder CommandBuilder { get; }
		DbConnectionStringBuilder ConnectionStringBuilder { get; }
		IDataInformation Information { get; }
		CommandBehavior ExecuteCommandBehavior { get; set; }
		Exception LastException { get; }
		DataResult LastDataResult { get; }
		string TransactionID { get; }

		// Utility
		string QuoteIdentifier(string identifier);
		string UnquoteIdentifier(string identifier);

		// CRUD
		DataResult Insert(
			string tableName, List<Any> values
		);
		DataResult Update(
			string tableName, List<Any> values, List<Any> wheres
		);
		DataResult Delete(
			string tableName, List<Any> wheres
		);

		// Sql Command
		DataResult ExecuteSql(
			string sql
		);
		DataResult ExecuteSql(
			string sql, params DbParameter[] parameters
		);
		DataResult ExecuteSql(
			string sql, params Any[] parameters
		);
		object ExecuteSqlByScalar(
			string sql
		);
		object ExecuteSqlByScalar(
			string sql, params DbParameter[] parameters
		);
		object ExecuteSqlByScalar(
			string sql, params Any[] parameters
		);
		DbDataReader ExecuteSqlByReader(
			string sql
		);
		DbDataReader ExecuteSqlByReader(
			string sql, params DbParameter[] parameters
		);
		DbDataReader ExecuteSqlByReader(
			string sql, params Any[] parameters
		);


		// Stored Procedure
		DataResult ExecuteSp(
			string spName
		);
		DataResult ExecuteSp(
			string spName, params DbParameter[] parameters
		);
		DataResult ExecuteSp(
			string spName, params Any[] parameters
		);
		object ExecuteSpByScalar(
			string spName
		);
		object ExecuteSpByScalar(
			string spName, params DbParameter[] parameters
		);
		object ExecuteSpByScalar(
			string spName, params Any[] parameters
		);
		DbDataReader ExecuteSpByReader(
			string spName
		);
		DbDataReader ExecuteSpByReader(
			string spName, params DbParameter[] parameters
		);
		DbDataReader ExecuteSpByReader(
			string spName, params Any[] parameters
		);
	}
}
