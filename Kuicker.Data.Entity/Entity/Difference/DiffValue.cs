// DiffValue.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker.Data
{
	public class DiffValue
	{
		public string ColumnName { get; set; }

		public string OriginalValue { get; set; }
		public bool OriginalIsNull { get; set; }

		public string CurrentValue { get; set; }
		public bool CurrentIsNull { get; set; }
	}
}
