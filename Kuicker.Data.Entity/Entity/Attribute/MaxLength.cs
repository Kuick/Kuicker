// MaxLength.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class MaxLength : Attribute
	{
		public MaxLength(int size)
		{
			this.Size = size;
		}

		public int Size { get; set; }
	}
}
