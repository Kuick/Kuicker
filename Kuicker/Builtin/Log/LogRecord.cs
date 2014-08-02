// LogRecord.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Kuicker
{
	public class LogRecord : ILogRecord
	{
		public LogRecord()
		{
			this.Title = RunTime.CalleeFullName();
			this.LogID = Guid.NewGuid().ToString();
			this.TransactionID = string.Empty;
			this.Level = LogLevel.Info;
			this.Timestamp = DateTime.Now;
			this.Message = string.Empty;
			this.AppID = Config.Kernel.AppID;
			this.ServerIP = RunTime.ServerIP;
			this.ServerName = Environment.MachineName;
			this.ClientIP = RunTime.GetClientIP();
			this.Browser = RunTime.ReadyForRequest
				? HttpContext.Current.Request.Browser.ToAnys()
				: new List<Any>();
			this.Datas = new List<Any>();
		}

		public string Title { get; set; }
		public string LogID { get; set; }
		public string TransactionID { get; set; }
		public LogLevel Level { get; set; }
		public DateTime Timestamp { get; set; }
		public double ExecutionTime { get; set; }
		public string Message { get; set; }
		public string AppID { get; set; }
		public string ServerIP { get; set; }
		public string ServerName { get; set; }
		public string ClientIP { get; set; }
		public List<Any> Browser { get; set; }
		public List<Any> Datas { get; set; }

		public LogRecord SetTitle(string title)
		{
			Title = title;
			return this;
		}
		public LogRecord SetTransactionID(string transactionID)
		{
			TransactionID = transactionID;
			return this;
		}
		public LogRecord SetLevel(LogLevel level)
		{
			Level = level;
			return this;
		}
		public LogRecord SetMessage(string message)
		{
			Message = message;
			return this;
		}
		public LogRecord SetMessage(
			params object[] pieces)
		{
			if(pieces.IsNullOrEmpty()) { return this; }
			StringBuilder sb = new StringBuilder();
			foreach(var piece in pieces) {
				if(null == piece) { continue; }
				sb.Append(piece.ToString());
			}
			Message = sb.ToString();
			return this;
		}

		public LogRecord SetMessageFormat(
			string format, params string[] parameters)
		{
			Message = string.Format(format, parameters);
			return this;
		}

		public LogRecord Add(Exception ex)
		{
			if(null == ex) { return this; }
			Datas.AddRange(
				ex.ToAnys().ToManys("Exception").ToAnys()
			);
			if(Message.IsNullOrEmpty()) {
				Message = ex.Message;
			}

			return this;
		}

		public LogRecord Add(bool ifTrue, string name, object value)
		{
			if(ifTrue) {
				Datas.Add(name, value);
			}
			return this;
		}

		public LogRecord Add(string name, object value)
		{
			Datas.Add(name, value);
			return this;
		}

		public LogRecord AddRange(bool ifTrue, params Any[] anys)
		{
			if(ifTrue) {
				Datas.AddRange(anys);
			}
			return this;
		}

		public LogRecord AddRange(params Any[] anys)
		{
			Datas.AddRange(anys);
			return this;
		}

		public LogRecord AddRange(bool ifTrue, List<Any> anys)
		{
			if(ifTrue) {
				if(null != anys) {
					Datas.AddRange(anys.ToArray());
				}
			}
			return this;
		}

		public LogRecord AddRange(List<Any> anys)
		{
			if(null != anys) {
				Datas.AddRange(anys.ToArray());
			}
			return this;
		}

		public LogRecord AddRange(bool ifTrue, List<Many> manys)
		{
			if(ifTrue) {
				if(null != manys) {
					foreach(var many in manys) {
						Datas.Add(many.ToAnyWithFullName());
					}
				}
			}
			return this;
		}

		public LogRecord AddRange(List<Many> manys)
		{
			if(null != manys) {
				foreach(var many in manys) {
					Datas.Add(many.ToAnyWithFullName());
				}
			}
			return this;
		}

		public virtual void DoBeforeWrite()
		{
			if(null == Intercepter) { return; }
			Intercepter(this);
		}

		public static LogRecord Create()
		{
			return new LogRecord()
				.SetTitle(RunTime.CalleeFullName());
		}

		public static LogRecord Create(LogLevel level)
		{
			return new LogRecord()
				{ 
					Level = level 
				}
				.SetTitle(RunTime.CalleeFullName());
		}

		public static Action<LogRecord> Intercepter { get; set; }
	}

	public abstract class LogRecord<T>
		: LogRecord
		where T : LogRecord<T>, new()
	{
		public LogRecord()
			: base()
		{
			base.Title = RunTime.CalleeFullName(2);
		}

		public new T SetTitle(string title)
		{
			Title = title;
			return (T)this;
		}
		public new T SetTransactionID(string transactionID)
		{
			TransactionID = transactionID;
			return (T)this;
		}
		public new T SetLevel(LogLevel level)
		{
			Level = level;
			return (T)this;
		}
		public new T SetMessage(string message)
		{
			Message = message;
			return (T)this;
		}

		public new T Add(Exception ex)
		{
			base.Add(ex);
			return (T)this;
		}

		public new T Add(string name, object value)
		{
			base.Add(name, value);
			return (T)this;
		}

		public new T AddRange(params Any[] anys)
		{
			base.AddRange(anys);
			return (T)this;
		}

		public new T AddRange(List<Any> anys)
		{
			base.AddRange(anys);
			return (T)this;
		}

		public override void DoBeforeWrite()
		{
			if(null == Intercepter) { return; }
			Intercepter((T)this);
		}

		public new static T Create()
		{
			return new T()
				.SetTitle(RunTime.CalleeFullName(2));
		}

		public new static T Create(LogLevel level)
		{
			return new T()
				{ 
					Level = level
				}
				.SetTitle(RunTime.CalleeFullName(2));
		}

		public new static Action<T> Intercepter { get; set; }
	}
}
