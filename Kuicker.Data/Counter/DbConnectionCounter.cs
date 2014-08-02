// DbConnectionCounter.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Concurrent;

namespace Kuicker.Data
{
	public class DbConnectionCounter
	{
		private static ConcurrentDictionary<string, Counter> 
			_Counters = new ConcurrentDictionary<string, Counter>();

		public static Counter Increase(string name)
		{
			return _Counters.AddOrUpdate(
				name, 
				new Counter(), 
				(key, existing) => {
					existing.Current = Math.Max(
						1, existing.Current + 1
					);
					if(existing.Peak < existing.Current) {
						existing.Peak = existing.Current;
						existing.PeakTime = DateTime.Now;
					}
					return existing;
				}
			);
		}

		public static Counter Decrease(string name)
		{
			return _Counters.AddOrUpdate(
				name,
				new Counter() {
					Current = 0
				},
				(key, existing) => {
					existing.Current = Math.Max(
						0, existing.Current - 1
					);
					return existing;
				}
			);
		}
	}
}
