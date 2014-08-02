// DefaultFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kuicker.Data
{
	public class DefaultFormater : SqlFormater
	{
		public override string ProviderName
		{
			get
			{
				return Constants.Default;
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
			throw new NotSupportedException();
		}

		public override string SelectPaging(
			string originalSql, int pageSize, int pageIndex)
		{
			throw new NotSupportedException();
		}

		public override bool SupportSelectTop
		{
			get
			{
				return false;
			}
		}

		public override bool SupportPaging
		{
			get
			{
				return false;
			}
		}

		public override DataFormat ToDataFormat(string dataType)
		{
			throw new NotSupportedException();
		}
	}
}
