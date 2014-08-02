// IKLifeCycle.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	internal interface IKLifeCycle : ILifeCycle
	{
		// Start
		void DoBuiltinStart();
		void DoPluginStart();

		// Stop
		void DoPluginStop();
		void DoBuiltinStop();

		// Suspend
		void DoPluginSuspend();

		// Resume
		void DoPluginResume();
	}
}
