// BuiltinBase.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Threading.Tasks;

namespace Kuicker
{
	public abstract class BuiltinBase : IBuiltin
	{
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
	}
}
