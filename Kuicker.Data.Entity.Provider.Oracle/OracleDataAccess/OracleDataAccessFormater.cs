// OracleDataAccessFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class OracleDataAccessFormater : OracleFormater
	{
		public override string ProviderName
		{
			get
			{
				return "Oracle.DataAccess.Client";
			}
		}
	}
}
