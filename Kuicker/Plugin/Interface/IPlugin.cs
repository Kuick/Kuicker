// IPlugin.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Threading.Tasks;

namespace Kuicker
{
	public interface IPlugin
	{
		string Name { get; }

		Task Start();
		Task Stop();
		Task Suspend();
		Task Resume();

		void SendMessage(string msg);
		Action<IPlugin, string> ReceiveMessage { get; set; }
	}
}
