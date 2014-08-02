// IntervalLogger.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kuicker
{
	public class IntervalLogger
		: IDisposable
	{
		private Stopwatch Stopwatch;

		public IntervalLogger()
		{
			this.Record = new LogRecord();
			this.Record.Title = RunTime.CalleeFullName();
			this.Stopwatch = new Stopwatch();
			this.Stopwatch.Start();
		}

		public LogRecord Record { get; private set; }

		public void Dispose()
		{
			this.Stopwatch.Stop();
			this.Record.ExecutionTime =
				Stopwatch.Elapsed.TotalMilliseconds / 1000f;
			this.Record.Write();
			GC.SuppressFinalize(this);
		}
	}

	public class IntervalLogger<T>
		: IDisposable
		where T : LogRecord<T>, new()
	{
		private Stopwatch Stopwatch;

		public IntervalLogger()
		{
			this.Record = new T();
			this.Record.Title = RunTime.CalleeFullName();
			this.Stopwatch = new Stopwatch();
			this.Stopwatch.Start();
		}

		public T Record { get; private set; }

		public void Dispose()
		{
			this.Stopwatch.Stop();
			this.Record.ExecutionTime =
				Stopwatch.Elapsed.TotalMilliseconds / 1000f;
			this.Record.Write();
			GC.SuppressFinalize(this);
		}
	}

	public class ILogger : IntervalLogger
	{
		public ILogger()
			: base()
		{
			base.Record.Title = RunTime.CalleeFullName();
		}
	}

	public class ILogger<T> 
		: IntervalLogger<T>
		where T : LogRecord<T>, new()
	{
		public ILogger()
			: base()
		{
			base.Record.Title = RunTime.CalleeFullName();
		}
	}
}
