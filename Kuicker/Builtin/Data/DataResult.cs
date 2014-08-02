// DataResult.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Kuicker
{
	public class DataResult : Result<DataResult>
	{
		private object _DataLock = new object();

		public DataResult()
			: base()
		{
		}

		public string TransactionID { get; set; }
		public string ExecuteMethod { get; set; }
		public CommandType CommandType { get; set; }
		public object ScalarValue { get; set; }
		public string Command { get; set; }
		public List<DbParameter> Parameters { get; set; }

		private int _AffectedCount;
		public int AffectedCount
		{
			get
			{
				lock(_DataLock) {
					int count = _AffectedCount;
					foreach(var result in InnerResults) {
						if(null == result) { continue; }
						count += result.AffectedCount;
					}
					return count;
				}
			}
			set
			{
				_AffectedCount = value;
			}
		}
	}
}
