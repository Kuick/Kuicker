// InternalIntervalLogger.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kuicker
{
	public class InternalIntervalLogger : IDisposable
	{
		private Stopwatch Stopwatch;

		public InternalIntervalLogger(
			IntervalLogger logger, object title)
		{
			this.Title = title.ToString();
			this.Block = logger.Record;
			this.InterDatas = new List<Any>();

			Stopwatch = new Stopwatch();
			Stopwatch.Start();
		}

		public LogRecord Block { get; private set; }
		public List<Any> InterDatas { get; private set; }

		public string Title { get; set; }
		public double ExecutionTime { get; set; }

		public void Dispose()
		{
			this.Stopwatch.Stop();
			this.InterDatas.Insert(
				0, 
				new Any(
					"ExecutionTime",
					this.Stopwatch.Elapsed.TotalMilliseconds / 1000f
				)
			);
			Block.Datas.AddRange(
				this.InterDatas.ToManys(this.Title).ToAnys()
			);
			GC.SuppressFinalize(this);
		}
	}

	public class InternalIntervalLogger<T>
		: IDisposable
		where T : LogRecord<T>, new()
	{
		private Stopwatch Stopwatch;

		public InternalIntervalLogger(
			IntervalLogger<T> logger, object title)
		{
			this.Title = title.ToString();
			this.Block = logger.Record;
			this.InterDatas = new List<Any>();

			Stopwatch = new Stopwatch();
			Stopwatch.Start();
		}

		public T Block { get; private set; }
		public List<Any> InterDatas { get; private set; }

		public string Title { get; set; }
		public double ExecutionTime { get; set; }

		public void Dispose()
		{
			this.Stopwatch.Stop();
			this.InterDatas.Insert(
				0, 
				new Any(
					"ExecutionTime",
					this.Stopwatch.Elapsed.TotalMilliseconds / 1000f
				)
			);
			Block.Datas.AddRange(
				this.InterDatas.ToManys(this.Title).ToAnys()
			);
			GC.SuppressFinalize(this);
		}
	}

	public class IILogger : InternalIntervalLogger
	{
		public IILogger(IntervalLogger logger, object title)
			: base(logger, title)
		{
		}
	}

	public class IILogger<T>
		: InternalIntervalLogger<T>
		where T : LogRecord<T>, new()
	{
		public IILogger(IntervalLogger<T> logger, object title)
			: base(logger, title)
		{
		}
	}
}
