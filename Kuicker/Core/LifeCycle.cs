// LifeCycle.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public abstract class LifeCycle : ILifeCycle
	{
		public virtual void DoBeforeStart() { }
		public virtual void DoBeforeBuiltinStart() { }
		public virtual void DoAfterBuiltinStart() { }
		public virtual void DoBeforePluginStart() { }
		public virtual void DoAfterPluginStart() { }
		public virtual void DoAfterStart() { }

		public virtual void DoBeforePluginSuspend() { }
		public virtual void DoAfterPluginSuspend() { }

		public virtual void DoBeforePluginResume() { }
		public virtual void DoAfterPluginResume() { }

		public virtual void DoBeforeStop() { }
		public virtual void DoAfterStop() { }
	}
}
