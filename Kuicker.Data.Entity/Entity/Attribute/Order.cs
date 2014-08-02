// Order.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class Order : Attribute
	{
		public Order(int index)
		{
			this.Index = index;
		}

		public int Index { get; set; }
	}
}
