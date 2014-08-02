// PrimaryKey.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class PrimaryKey : Attribute
	{
	}
}
