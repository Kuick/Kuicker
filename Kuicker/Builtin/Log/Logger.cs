// Logger.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public class Logger
	{
		public static void Fatal(string message)
		{
			LogRecord
				.Create()
				.SetMessage(message)
				.Fatal();
		}

		public static void Error(string message)
		{
			LogRecord
				.Create()
				.SetMessage(message)
				.Error();
		}

		public static void Warn(string message)
		{
			LogRecord
				.Create()
				.SetMessage(message)
				.Warn();
		}

		public static void Info(string message)
		{
			LogRecord
				.Create()
				.SetMessage(message)
				.Info();
		}

		public static void Debug(string message)
		{
			LogRecord
				.Create()
				.SetMessage(message)
				.Debug();
		}
	}
}
