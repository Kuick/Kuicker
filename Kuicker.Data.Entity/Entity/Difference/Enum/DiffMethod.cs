// DiffMethod.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.ComponentModel;

namespace Kuicker.Data
{
	[Flags]
	[DefaultValue(DiffMethod.Add | DiffMethod.Modify | DiffMethod.Remove)]
	public enum DiffMethod
	{
		[Description("Non")]
		Non = 0,

		[Description("Add")]
		Add = 1,

		[Description("Modify")]
		Modify = 2,

		[Description("Remove")]
		Remove = 4
	}
}
