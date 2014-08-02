// LogExtender.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public static class LogExtender
	{
		public static void Write<T>(this T record)
			where T : class, ILogRecord, new()
		{
			if(null == Kernel.Log) { return; }
			if(null == record) { return; }

			record.DoBeforeWrite();

			var json = record.ToJson();

			switch(record.Level) {
				case LogLevel.Fatal:
					Kernel.Log.Fatal(json);
					break;
				case LogLevel.Error:
					Kernel.Log.Error(json);
					break;
				case LogLevel.Warn:
					Kernel.Log.Warn(json);
					break;
				case LogLevel.Info:
					Kernel.Log.Info(json);
					break;
				case LogLevel.Debug:
					Kernel.Log.Debug(json);
					break;
				default:
					throw new NotImplementedException(
						"LogLevel = " + record.Level
					);
			}
		}

		public static T Fatal<T>(this T record)
			where T : class, ILogRecord, new()
		{
			record.Level = LogLevel.Fatal;
			record.Write();
			return record;
		}

		public static T Error<T>(this T record)
			where T : class, ILogRecord, new()
		{
			record.Level = LogLevel.Error;
			record.Write();
			return record;
		}

		public static T Warn<T>(this T record)
			where T : class, ILogRecord, new()
		{
			record.Level = LogLevel.Warn;
			record.Write();
			return record;
		}

		public static T Info<T>(this T record)
			where T : class, ILogRecord, new()
		{
			record.Level = LogLevel.Info;
			record.Write();
			return record;
		}

		public static T Debug<T>(this T record)
			where T : class, ILogRecord, new()
		{
			record.Level = LogLevel.Debug;
			record.Write();
			return record;
		}
	}
}
