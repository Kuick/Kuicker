// DiffFollow.cs
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
	public sealed class DiffFollow : Attribute
	{
		public DiffFollow()
			: this(DiffMethod.Add | DiffMethod.Modify | DiffMethod.Remove)
		{
		}
		public DiffFollow(DiffMethod method)
		{
			this.Follow = method;
			this.Add = Flag.Check((int)method, (int)DiffMethod.Add);
			this.Modify = Flag.Check((int)method, (int)DiffMethod.Modify);
			this.Remove = Flag.Check((int)method, (int)DiffMethod.Remove);
		}

		public DiffMethod Follow { get; set; }
		public bool Add { get; set; }
		public bool Modify { get; set; }
		public bool Remove { get; set; }
	}
}
