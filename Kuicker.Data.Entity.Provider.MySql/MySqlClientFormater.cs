// MySqlClientFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public class MySqlClientFormater : SqlFormater
	{
		public override string ProviderName
		{
			get
			{
				return "MySql.Data.MySqlClient";
			}
		}

		public override string Wildcard
		{
			get
			{
				return Symbol.Asterisk;
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

			throw new NotImplementedException();
		}

		public override string SelectPaging(
			string originalSql, int pageSize, int pageIndex)
		{
			base.MustBeSelectCommand(originalSql);

			throw new NotImplementedException();
		}

		public override bool SupportSelectTop { get { return false; } }
		public override bool SupportPaging { get { return false; } }

		public override DataFormat ToDataFormat(string dataType)
		{
			// todo
			throw new NotImplementedException();
		}
	}
}
