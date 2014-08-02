// PluginBase.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Threading.Tasks;

namespace Kuicker
{
	public abstract class PluginBase : IPlugin
	{
		private Action<IPlugin, string> _EmptyAction;

		public abstract string Name { get; }

		public virtual Task Start()
		{
			var task = new Task(() => { });
			return task;
		}

		public virtual Task Stop()
		{
			var task = new Task(() => { });
			return task;
		}

		public virtual Task Suspend()
		{
			var task = new Task(() => { });
			return task;
		}

		public virtual Task Resume()
		{
			var task = new Task(() => { });
			return task;
		}

		public virtual void SendMessage(string msg)
		{
			return;
		}

		public Action<IPlugin, string> ReceiveMessage
		{
			get
			{
				if(null == _EmptyAction) {
					_EmptyAction = (i, x) => { };
				}
				return _EmptyAction;
			}
			set
			{
				_EmptyAction = value;
			}
		}
	}
}
