// Result.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Kuicker
{
	public class Result
	{
		private object _Lock = new object();

		public Result()
		{
			this.Datas = new List<Any>();
			this.InnerResults = new List<Result>();
			this.Message = string.Empty;
			this.Success = true;
		}

		#region property
		public List<Any> Datas { get; private set; }
		public List<Result> InnerResults { get; private set; }
		public string Message { get; set; }
		public Exception Exception { get; set; }

		private bool _Success;
		public bool Success
		{
			get
			{
				lock(_Lock) {
					if(!_Success) { return false; }
					foreach(Result result in InnerResults) {
						if(null == result) { continue; }
						if(!result.Success) { return false; }
					}
					return _Success;
				}
			}
			set
			{
				_Success = value;
			}
		}
		#endregion

		#region override
		#endregion
		public override string ToString()
		{
			return Success ? "Success" : "Failure";
		}

		#region static
		public static Result BuildSuccess()
		{
			return new Result() {
				Message = string.Empty,
				Success = true
			};
		}

		public static Result BuildFailure()
		{
			return new Result() {
				Message = string.Empty,
				Success = false
			};
		}
		#endregion
	}


	public class Result<T>
		: Result
		where T : Result<T>, new ()
	{
		public Result()
			: base()
		{
			this.InnerResults = new List<T>();
		}

		public new List<T> InnerResults { get; private set; }

		public new static T BuildSuccess()
		{
			return new T() {
				Message = string.Empty,
				Success = true
			};
		}

		public new static T BuildFailure()
		{
			return new T() {
				Message = string.Empty,
				Success = false
			};
		}
	}
}
