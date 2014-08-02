// ILog.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Threading.Tasks;

namespace Kuicker
{
	public interface ILog : IBuiltin
	{
		void Fatal(string json);
		void Error(string json);
		void Warn(string json);
		void Info(string json);
		void Debug(string json);
	}
}
