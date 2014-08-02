// XXXXXXX.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public class DbConfig
	{
		public string Name { get; set; }
		public string Provider { get; set; }
		public string Schema { get; set; }
		public string Pooling { get; set; }
		public string MaxPoolSize { get; set; }
		public string ConnectionString { get; set; }
	}
}
