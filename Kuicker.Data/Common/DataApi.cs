// DataApi.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Kuicker.Data
{
	public class DataApi : IDataApi
	{
		#region constructor
		public DataApi()
			: this(Constants.Default)
		{
		}

		public DataApi(string name)
		{
			this.OriginalName = name;
			this.TransactionID = Guid.NewGuid().ToString();

			try {
				this.Settings = DataSettings.GetByName(name);
				this.Factory = DbProviderFactories.GetFactory(
					this.Settings.ProviderName
				);
				this.ConnectionStringBuilder =
					this.Factory.CreateConnectionStringBuilder();
				this.ConnectionStringBuilder.ConnectionString = 
					this.Settings.ConnectionString;
				this.CommandBuilder =
					this.Factory.CreateCommandBuilder();
				var connection = this.Factory.CreateConnection();
				connection.ConnectionString = 
					this.Settings.ConnectionString;
				this.Connection = connection;

				// USER ID
				object userID;
				if(ConnectionStringBuilder
					.TryGetValue("USER ID", out userID)) {
					this.UserID = userID.ToString();
				} else {
					throw new Exception(
						new[]{
							"Can't find 'USER ID' value ",
							"in connectionString (",
							connection.ConnectionString,
							")",
						}.Join()
					);
				}

				// PASSWORD
				object password;
				if(ConnectionStringBuilder
					.TryGetValue("PASSWORD", out password)) {
					this.Password = password.ToString();
				} else {
					throw new Exception(
						new[]{
							"Can't find 'PASSWORD' value ",
							"in connectionString (",
							connection.ConnectionString,
							")",
						}.Join()
					);
				}

				// Information
				this.Information = CurrentInformation;

				// ExecuteCommandBehavior
				this.ExecuteCommandBehavior = 
					CommandBehavior.CloseConnection;

			} catch(Exception ex) {
				var log = LogRecord
					.Create()
					.SetTransactionID(this.TransactionID)
					.Add(ex)
					.Fatal();
				throw;
			}
		}
		#endregion

		#region property
		public IDataProviderSetting Settings { get; private set; }
		public DbProviderFactory Factory { get; private set; }
		public DbCommandBuilder CommandBuilder { get; private set; }
		public DbConnectionStringBuilder ConnectionStringBuilder
		{ get; private set; }
		public IDataInformation Information { get; private set; }
		public CommandBehavior ExecuteCommandBehavior { get; set; }
		public Exception LastException { get; private set; }
		public DataResult LastDataResult { get; private set; }
		public string TransactionID { get; private set; }

		public string UserID { get; private set; }
		public string Password { get; private set; }

		#endregion

		#region Utility
		public string QuoteIdentifier(string identifier)
		{
			return identifier
				.SplitAndTrim(".")
				.Join(
					x => x
						.AppendPrefix(CommandBuilder.QuotePrefix)
						.AppendSuffix(CommandBuilder.QuoteSuffix),
					"."
				);
		}

		public string UnquoteIdentifier(string identifier)
		{
			return identifier
				.SplitAndTrim(".")
				.Join(
					x => x
						.TrimStart(CommandBuilder.QuotePrefix)
						.TrimEnd(CommandBuilder.QuoteSuffix),
					"."
				);
		}
		#endregion

		#region CUD
		public DataResult Insert(string tableName, List<Any> values)
		{
			#region check
			if(tableName.IsNullOrEmpty()) {
				throw new ArgumentNullException(
					"tableName",
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.SetMessage(
							"Can't invoke Insert() method ",
							"without settings table name."
						)
						.Error()
						.Message
				);
			}

			if(values.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.SetMessage(
							"You can't invoke Insert() method ",
							"without settings any value."
						)
						.Error()
						.Message
				);
			}
			#endregion

			// Sql
			var sql = string.Format(
@"INSERT INTO {0} (
{1}
) VALUES (
{2}
)",
				CommandBuilder.QuoteIdentifier(tableName),
				values
					.GetNames()
					.Join(
						"," + Environment.NewLine,
						"	" + CommandBuilder.QuotePrefix,
						CommandBuilder.QuoteSuffix
					),
				values
					.GetNames()
					.Join(
						x => string.Format(
							"	{1}x_{2}",
							CommandBuilder.QuoteIdentifier(x),
							Information.ParameterMarker,
							x
						),
						"," + Environment.NewLine
					)
			);

			// Parameters
			var parameters = new List<DbParameter>();

			// where
			foreach(var value in values) {
				var parameter = Factory.CreateParameter();
				parameter.ParameterName = string.Concat(
					//Information.ParameterMarker,
					"x_",
					value.Name
				);
				parameter.Value = value.ToParameterValue();
				parameters.Add(parameter);
			}

			// Execute
			var result = ExecuteSql(
				sql, parameters.ToArray()
			);

			return result;
		}

		public DataResult Update(
			string tableName, List<Any> values, List<Any> wheres)
		{
			#region check
			if(tableName.IsNullOrEmpty()) {
				throw new ArgumentNullException(
					"tableName",
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.SetMessage(
							"Can't invoke Update() method ",
							"without settings table name."
						)
						.Error()
						.Message
				);
			}

			if(values.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.SetMessage(
							"Can't invoke Update() method ",
							"without settings any value."
						)
						.Error()
						.Message
				);
			}

			if(wheres.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.SetMessage(
							"Can't invoke Update() method ",
							"without settings any condition."
						)
						.Error()
						.Message
				);
			}
			#endregion

			// Sql
			var sql = string.Format(
@"UPDATE {0} SET
{1}
WHERE
{2}",
				CommandBuilder.QuoteIdentifier(tableName),
				values
					.GetNames()
					.Join(
						x => string.Format(
							"	{0} = {1}{2}",
							CommandBuilder.QuoteIdentifier(x),
							Information.ParameterMarker,
							x
						),
						"," + Environment.NewLine
					),
				wheres
					.GetNames()
					.Join(
						x => string.Format(
							"	{0} = {1}x_{2}",
							CommandBuilder.QuoteIdentifier(x),
							Information.ParameterMarker,
							x
						),
						" AND" + Environment.NewLine
					)
			);

			// Parameters
			var parameters = new List<DbParameter>();

			// set
			foreach(var value in values) {
				var parameter = Factory.CreateParameter();
				parameter.ParameterName = string.Concat(
					Information.ParameterMarker, value.Name
				);
				parameter.Value = value.ToParameterValue();
				parameters.Add(parameter);
			}

			// where
			foreach(var where in wheres) {
				var parameter = Factory.CreateParameter();
				parameter.ParameterName = string.Concat(
					Information.ParameterMarker, "x_", where
				);
				parameter.Value = where.ToParameterValue();
				parameters.Add(parameter);
			}

			// Execute
			var result = ExecuteSql(
				sql, parameters.ToArray()
			);

			return result;
		}

		public DataResult Delete(string tableName, List<Any> wheres)
		{
			#region check
			if(tableName.IsNullOrEmpty()) {
				throw new ArgumentNullException(
					"tableName",
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.SetMessage(
							"Can't invoke Delete() method ",
							"without settings table name."
						)
						.Error()
						.Message
				);
			}

			if(wheres.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.SetMessage(
							"You can't invoke Delete() method ",
							"without settings any condition."
						)
						.Error()
						.Message
				);
			}
			#endregion

			// Sql
			var sql = string.Format(
@"DELETE FROM {0}
WHERE
{1}",
				CommandBuilder.QuoteIdentifier(tableName),
				wheres
					.GetNames()
					.Join(
						x => string.Format(
							"	{0} = {1}x_{2}",
							CommandBuilder.QuoteIdentifier(x),
							Information.ParameterMarker,
							x
						),
						" AND" + Environment.NewLine
					)
			);

			// Parameters
			var parameters = new List<DbParameter>();

			// where
			foreach(var where in wheres) {
				var parameter = Factory.CreateParameter();
				parameter.ParameterName = string.Concat(
					Information.ParameterMarker, "x_", where.Name
				);
				parameter.Value = where.ToParameterValue();
				parameters.Add(parameter);
			}

			// Execute
			var result = ExecuteSql(
				sql, parameters.ToArray()
			);

			return result;
		}
		#endregion

		#region Execute Sql / Stored Procedure
		public DataResult ExecuteSql(
			string sql, params DbParameter[] parameters)
		{
			return ExecuteMain(
				CommandType.Text,
				sql,
				parameters
			);
		}
		public DataResult ExecuteSql(
			string sql)
		{
			return ExecuteMain(
				CommandType.Text,
				sql
			);
		}
		public DataResult ExecuteSql(
			string sql, params Any[] parameters)
		{
			return ExecuteMain(
				CommandType.Text,
				sql,
				ToParameters(parameters).ToArray()
			);
		}

		public object ExecuteSqlByScalar(
			string sql)
		{
			return ExecuteByScalarMain(
				CommandType.Text,
				sql
			);
		}
		public object ExecuteSqlByScalar(
			string sql, params DbParameter[] parameters)
		{
			return ExecuteByScalarMain(
				CommandType.Text,
				sql,
				parameters
			);
		}
		public object ExecuteSqlByScalar(
			string sql, params Any[] parameters)
		{
			return ExecuteByScalarMain(
				CommandType.Text,
				sql,
				ToParameters(parameters).ToArray()
			);
		}

		public DbDataReader ExecuteSqlByReader(
			string sql)
		{
			return ExecuteByReaderMain(
				CommandType.Text,
				sql
			);
		}
		public DbDataReader ExecuteSqlByReader(
			string sql, params DbParameter[] parameters)
		{
			return ExecuteByReaderMain(
				CommandType.Text,
				sql,
				parameters
			);
		}
		public DbDataReader ExecuteSqlByReader(
			string sql, params Any[] parameters)
		{
			return ExecuteByReaderMain(
				CommandType.Text,
				sql,
				ToParameters(parameters).ToArray()
			);
		}

		public DataResult ExecuteSp(
			string spName)
		{
			return ExecuteMain(
				CommandType.StoredProcedure,
				spName
			);
		}
		public DataResult ExecuteSp(
			string spName, params DbParameter[] parameters)
		{
			return ExecuteMain(
				CommandType.StoredProcedure,
				spName,
				parameters
			);
		}
		public DataResult ExecuteSp(
			string spName, params Any[] parameters)
		{
			return ExecuteMain(
				CommandType.StoredProcedure,
				spName,
				ToParameters(parameters).ToArray()
			);
		}

		public object ExecuteSpByScalar(
			string spName)
		{
			return ExecuteByScalarMain(
				CommandType.StoredProcedure,
				spName
			);
		}
		public object ExecuteSpByScalar(
			string spName, params DbParameter[] parameters)
		{
			return ExecuteByScalarMain(
				CommandType.StoredProcedure,
				spName,
				parameters
			);
		}
		public object ExecuteSpByScalar(
			string spName, params Any[] parameters)
		{
			return ExecuteByScalarMain(
				CommandType.StoredProcedure,
				spName,
				ToParameters(parameters).ToArray()
			);
		}

		public DbDataReader ExecuteSpByReader(
			string spName)
		{
			return ExecuteByReaderMain(
				CommandType.StoredProcedure,
				spName
			);
		}
		public DbDataReader ExecuteSpByReader(
			string spName, params DbParameter[] parameters)
		{
			return ExecuteByReaderMain(
				CommandType.StoredProcedure,
				spName,
				parameters
			);
		}
		public DbDataReader ExecuteSpByReader(
			string spName, params Any[] parameters)
		{
			return ExecuteByReaderMain(
				CommandType.StoredProcedure,
				spName,
				ToParameters(parameters).ToArray()
			);
		}
		#endregion

		#region Dispose
		public void Dispose()
		{
			if(null != Connection) {
				CloseConnection();
				Connection.Dispose();
			}
		}
		#endregion

		#region private
		private string OriginalName { get; set; }
		private DbConnection Connection { get; set; }

		private static object _Lock = new object();
		private static ConcurrentDictionary<string, DataInformation>
			_Informations =
			new ConcurrentDictionary<string, DataInformation>();
		private DataInformation CurrentInformation
		{
			get
			{
				DataInformation information;

				if(_Informations.TryGetValue(
					Connection.ConnectionString,
					out information)) {
					return information;
				}

				lock(_Lock) {
					if(_Informations.TryGetValue(
						Connection.ConnectionString,
						out information)) {
						return information;
					}

					OpenConnection();
					var table = Connection.GetSchema(
						DbMetaDataCollectionNames
							.DataSourceInformation
					);
					CloseConnection();

					information = new DataInformation(table);
					_Informations.AddOrUpdate(
						Connection.ConnectionString,
						information,
						(xKey, xValue) => information
					);

					return information;
				}
			}
		}

		public DbParameter CreateParameter(string name, object value)
		{
			var p = Factory.CreateParameter();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		public List<DbParameter> ToParameters(params Any[] anys)
		{
			var list = new List<DbParameter>();
			if(anys.IsNullOrEmpty()) { return list; }

			foreach(var any in anys) {
				var p = CreateParameter(any.Name, any.Value);
				list.Add(p);
			}

			return list;
		}

		public List<Any> ToAnys(params DbParameter[] parameters)
		{
			var list = new List<Any>();
			if(parameters.IsNullOrEmpty()) { return list; }

			foreach(var parameter in parameters) {
				list.Add(parameter.ParameterName, parameter.Value);
			}

			return list;
		}

		private DataResult ExecuteMain(
			CommandType type,
			string sqlOrSpName,
			params DbParameter[] parameters)
		{
			LastException = null;
			LastDataResult = null;
			var result = new DataResult() {
				TransactionID = TransactionID,
				ExecuteMethod = "DataApi.ExecuteMain",
				CommandType = type,
				Command = sqlOrSpName,
				Parameters = new List<DbParameter>(parameters),
			};
			var log = LogRecord
				.Create()
				.SetTransactionID(this.TransactionID);

			try {
				using(var command = Connection.CreateCommand()) {
					command.CommandType = type;
					command.CommandText = sqlOrSpName;

					AddParameters(command, parameters);
					OpenConnection();

					result.AffectedCount = command.ExecuteNonQuery();
					return result;
				}

			} catch(Exception ex) {
				LastException = ex;
				log.Add(ex);
				result.Success = false;
				result.Exception = ex;
				throw;

			} finally {
				log
					.Add("CommandType", type.ToString())
					.Add("sqlOrSpName", sqlOrSpName)
					.AddRange(ToAnys(parameters))
					.Add("AffectedCount", result.AffectedCount);
				if(null == LastException) {
					log.Debug();
				} else {
					log.Error();
				}
				LastDataResult = result;
			}
		}

		private object ExecuteByScalarMain(
			CommandType type,
			string sqlOrSpName,
			params DbParameter[] parameters)
		{
			LastException = null;
			LastDataResult = new DataResult() {
				TransactionID = TransactionID,
				ExecuteMethod = "DataApi.ExecuteByScalarMain",
				CommandType = type,
				Command = sqlOrSpName,
				Parameters = new List<DbParameter>(parameters),
			};
			var log = LogRecord
				.Create()
				.SetTransactionID(this.TransactionID);

			try {
				using(var command = Connection.CreateCommand()) {
					command.CommandType = type;
					command.CommandText = sqlOrSpName;

					if(null != parameters) {
						foreach(var parameter in parameters) {
							if(null == parameter) { continue; }
							command.Parameters.Add(parameter);
						}
					}
					OpenConnection();
					var value = command.ExecuteScalar();
					LastDataResult.ScalarValue = value;
					return value;
				}

			} catch(Exception ex) {
				LastDataResult.Success = false;
				LastDataResult.Exception = ex;
				LastException = ex;
				log.Add(ex);
				throw;

			} finally {
				log
					.Add("CommandType", type.ToString())
					.Add("sqlOrSpName", sqlOrSpName)
					.AddRange(ToAnys(parameters));
				if(null == LastException) {
					log.Debug();
				} else {
					log.Error();
				}
			}
		}

		private DbDataReader ExecuteByReaderMain(
			CommandType type,
			string sqlOrSpName,
			params DbParameter[] parameters)
		{
			LastException = null;
			LastDataResult = new DataResult() {
				TransactionID = TransactionID,
				ExecuteMethod = "DataApi.ExecuteByReaderMain",
				CommandType = type,
				Command = sqlOrSpName,
				Parameters = new List<DbParameter>(parameters),
			};
			var log = LogRecord
				.Create()
				.SetTransactionID(this.TransactionID);

			try {
				using(var command = Connection.CreateCommand()) {
					command.CommandType = type;
					command.CommandText = sqlOrSpName;

					AddParameters(command, parameters);
					OpenConnection();

					DbDataReader reader = command.ExecuteReader(
						ExecuteCommandBehavior
					);

					return reader;
				}
			} catch(Exception ex) {
				LastDataResult.Success = false;
				LastDataResult.Exception = ex;
				LastException = ex;
				log.Add(ex);
				throw;

			} finally {
				log
					.Add("CommandType", type.ToString())
					.Add("sqlOrSpName", sqlOrSpName)
					.AddRange(ToAnys(parameters));
				if(null == LastException) {
					log.Debug();
				} else {
					log.Error();
				}
			}
		}

		private void OpenConnection()
		{
			if(null == Connection) {
				throw new NullReferenceException(
					LogRecord
					.Create()
					.SetTransactionID(this.TransactionID)
					.SetMessage("Connection is null reference")
					.Fatal()
					.Message
				);
			}
			if(ConnectionState.Open != Connection.State) {
				try {
					Connection.Open();
					var c = DbConnectionCounter.Increase(OriginalName);
					if(c.Peak == c.Current) {
						LogRecord
							.Create()
							.SetTransactionID(this.TransactionID)
							.SetMessage(
								OriginalName,
								": By far the largest number of ",
								"concurrent connections ",
								"to the database is ",
								c.Peak
							)
							.Debug();
					}
				} catch(Exception ex) {
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.Add(ex)
						.Error();
					throw;
				}
			}
		}
		private void CloseConnection()
		{
			if(null == Connection) {
				var c = DbConnectionCounter.Decrease(OriginalName);
			} else {
				try {
					Connection.Close();
					var c = DbConnectionCounter.Decrease(OriginalName);
				} catch(Exception ex) {
					LogRecord
						.Create()
						.SetTransactionID(this.TransactionID)
						.Add(ex)
						.Error();
					throw;
				}
			}
		}

		private void AddParameters(
			DbCommand command,
			params DbParameter[] parameters)
		{
			if(null == command) {
				throw new ArgumentNullException("command");
			}

			if(null != parameters) {
				foreach(var parameter in parameters) {
					// skip null
					if(null == parameter) { continue; }
					if(parameter.ParameterName.IsNullOrEmpty()) {
						continue;
					}
					command.Parameters.Add(parameter);
				}
			}
		}
		#endregion
	}
}
