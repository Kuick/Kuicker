// View.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	[AttributeUsage(
		AttributeTargets.Class,
		AllowMultiple = false,
		Inherited = true)]
	public sealed class View : Attribute
	{
	}
}
