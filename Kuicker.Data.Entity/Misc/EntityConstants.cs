// EntityConstants.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Kuicker;

namespace Kuicker.Data
{
	public class EntityConstants : Constants
	{
		public class Sql
		{
			public const string Select = "SELECT";
			public const string Insert = "INSERT";
			public const string Update = "UPDATE";
			public const string Delete = "DELETE";
			public const string Distinct = "DISTINCT";
			public const string Null = "NULL";

			public const string Standalone = "Standalone";
		}
	}
}
