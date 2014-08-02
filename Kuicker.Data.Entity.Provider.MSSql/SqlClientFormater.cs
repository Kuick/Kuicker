// SqlClientFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;
using System.Text;

namespace Kuicker.Data
{
	public class SqlClientFormater : SqlFormater
	{
		public override string ProviderName
		{
			get
			{
				return "System.Data.SqlClient";
			}
		}

		public override string Wildcard
		{
			get
			{
				return Symbol.Percent;
			}
		}

		public override string AssignNullMaxVarChar
		{
			get
			{
				return EntityConstants.Sql.Null;
			}
		}

		public override string SelectTop(
			string originalSql, int top)
		{
			base.MustBeSelectCommand(originalSql);

			int i = originalSql.IndexOf(EntityConstants.Sql.Select);
			int j = originalSql.IndexOf(EntityConstants.Sql.Distinct);
			StringBuilder sb = new StringBuilder(originalSql);
			int k = j < 0
				? i + EntityConstants.Sql.Select.Length
				: j + EntityConstants.Sql.Distinct.Length;
			sb = sb.Insert(k, String.Format(" TOP {0} ", top));
			return sb.ToString();
		}

		public override string SelectPaging(
			string originalSql, int pageSize, int pageIndex)
		{
			base.MustBeSelectCommand(originalSql);

			// todo
			throw new NotImplementedException();
		}

		public override bool SupportSelectTop { get { return true; } }
		public override bool SupportPaging { get { return false; } }

		public override DataFormat ToDataFormat(string dataType)
		{
			// todo
			throw new NotImplementedException();
		}
	}
}
