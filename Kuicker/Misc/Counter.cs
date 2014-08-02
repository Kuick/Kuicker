// Counter.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public class Counter
	{
		public Counter()
		{
			this.Current = 1;
			this.Peak = 1;
			this.PeakTime = DateTime.Now;
		}

		public int Current { get; set; }
		public int Peak { get; set; }
		public DateTime PeakTime { get; set; }
	}
}
