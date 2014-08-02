// Platform.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public sealed class Platform
	{
		private static Kernel _Kernel = null;

		public static void Start()
		{
			if(null == _Kernel) {
				_Kernel = Kernel.Current;
			}
		}

		public static void Stop()
		{
			if(null != _Kernel) {
				_Kernel.Dispose();
				_Kernel = null;
			}
		}
	}
}
