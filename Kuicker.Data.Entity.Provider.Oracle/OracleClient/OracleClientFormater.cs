﻿// OracleClientFormater.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class OracleClientFormater : OracleFormater
	{
		public override string ProviderName
		{
			get
			{
				return "System.Data.OracleClient";
			}
		}
	}
}
