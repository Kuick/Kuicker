// ISqlFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public interface ISqlFormater
	{
		string ProviderName { get; }
		string Wildcard { get; }
		string AssignNullMaxVarChar { get; }

		string SelectTop(string originalSql, int top);
		string SelectPaging(
			string originalSql, int pageSize, int pageIndex
		);

		bool SupportSelectTop { get; }
		bool SupportPaging { get; }

		DataFormat ToDataFormat(string dataType);
	}
}
