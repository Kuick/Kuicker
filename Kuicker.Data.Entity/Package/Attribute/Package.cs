// PackageAttribute.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;

namespace Kuicker.Data
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class PackageAttribute : Attribute
	{
		public PackageAttribute()
		{
		}

		//public string PackageName { get; internal set; }
	}
}
