// EntityLifeCycle.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class EntityLifeCycle : LifeCycle
	{
		public override void DoBeforeStart()
		{
			SqlConvention.Default = 
				new SqlUpperCasingConvention();

			EntityCache.Collect();
		}

		public override void DoAfterStop()
		{
			EntityCache.Clear();
		}
	}
}
