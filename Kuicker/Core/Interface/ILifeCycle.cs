// ILifeCycle.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public interface ILifeCycle
	{
		// Start
		void DoBeforeStart();
		void DoBeforeBuiltinStart();
		void DoAfterBuiltinStart();
		void DoBeforePluginStart();
		void DoAfterPluginStart();
		void DoAfterStart();

		// Stop
		void DoBeforeStop();
		void DoAfterStop();

		// Suspend
		void DoBeforePluginSuspend();
		void DoAfterPluginSuspend();

		// Resume
		void DoBeforePluginResume();
		void DoAfterPluginResume();
	}
}
