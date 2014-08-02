// SqlFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public abstract class SqlFormater : ISqlFormater
	{
		public SqlFormater() { }

		public abstract string ProviderName { get; }
		public abstract string Wildcard { get; }
		public abstract string AssignNullMaxVarChar { get; }


		public abstract string SelectTop(string originalSql, int top);
		public abstract string SelectPaging(
			string originalSql, int pageSize, int pageIndex
		);
		public virtual bool SupportSelectTop { get { return false; } }
		public virtual bool SupportPaging { get { return false; } }

		public abstract DataFormat ToDataFormat(string dataType);

		#region helper
		protected bool IsSelectCommand(string sql)
		{
			if(sql.IsNullOrEmpty()) { return false; }
			return sql.Trim().ToUpper().StartsX(
				EntityConstants.Sql.Select
			);
		}
		protected void MustBeSelectCommand(string sql)
		{
			if(IsSelectCommand(sql)) { return; }

			throw new Exception(
				LogRecord
					.Create()
					.SetMessage(
						"This is not select sql command:",
						Environment.NewLine,
						sql
					)
					.Error()
					.Message
			);
		}
		#endregion
	}
}
