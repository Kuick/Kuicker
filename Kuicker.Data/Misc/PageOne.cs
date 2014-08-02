// DynamicRow.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class PageOne
	{
		public PageOne()
		{
		}

		public PageOne(int pageIndex, bool active)
		{
			this.PageIndex = pageIndex;
			this.Active = active;
		}

		public int PageIndex { get; set; }
		public bool Active { get; set; }
	}
}
