// ILogRecord.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Web;

namespace Kuicker
{
	public interface ILogRecord
	{
		string Title { get; set; }
		string LogID { get; set; }
		string TransactionID { get; set; }
		LogLevel Level { get; set; }
		DateTime Timestamp { get; set; }
		double ExecutionTime { get; set; }
		string Message { get; set; }
		string AppID { get; set; }
		string ServerIP { get; set; }
		string ServerName { get; set; }
		string ClientIP { get; set; }
		List<Any> Browser { get; set; }
		List<Any> Datas { get; set; }

		LogRecord SetTitle(string title);
		LogRecord SetTransactionID(string transactionID);
		LogRecord SetLevel(LogLevel level);
		LogRecord SetMessage(string message);
		LogRecord Add(Exception ex);
		LogRecord Add(string name, object value);
		LogRecord AddRange(params Any[] anys);
		LogRecord AddRange(List<Any> anys);

		void DoBeforeWrite();
	}
}
