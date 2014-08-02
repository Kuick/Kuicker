// EntityConfig.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class EntityConfig
	{
		public static bool Difference
		{
			get
			{
				return Config.Builtin.Data.ToBoolean(
					"Difference", false
				);
			}
		}
	}
}
