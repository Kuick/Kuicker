// EnumItem.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace Kuicker
{
	public class EnumItem
	{
		public Type Type { get; set; }
		public string Name { get; set; }
		public int Value { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }

		public Any ToAny()
		{
			return new Any(Name, Value);
		}

		public T ToEnum<T>()
		{
			return Name.ToEnum<T>();
		}
	}
}
