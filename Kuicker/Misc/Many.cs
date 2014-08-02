// Many.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public class Many : Any
	{
		public Many()
			: this(string.Empty)
		{
		}
		public Many(string group)
			: this(group, string.Empty)
		{
		}
		public Many(string group, string name)
			: this(group, name, string.Empty)
		{
		}
		public Many(string group, string name, params object[] values)
			: base(name, values)
		{
			if(group.IsNullOrEmpty()) { group = "Default"; }
			this.Group = group;
		}

		public string Group { get; set; }

		public string Key
		{
			get
			{
				return string.Concat(Group, ".", Name);
			}
		}

		public Any ToAny()
		{
			Any any = new Any(Name, Value);
			return any;
		}
		public Any ToAnyWithFullName()
		{
			return Group.IsNullOrEmpty() || Name.IsNullOrEmpty()
				? new Any(
					string.Concat(Group.AirBag(), Name.AirBag()),
					Value
				)
				: new Any(
					string.Concat(Group, ".", Name),
					Value
				);
		}
	}
}
