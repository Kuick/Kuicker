// KernelStatus.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.ComponentModel;

namespace Kuicker
{
	[DefaultValue(KernelStatus.Stopped)]
	public enum KernelStatus
	{
		Stopped,

		BeforeStart,         // >────────────┐
		BeforeBuiltinStart,  // >─┐          │
		BuiltinStart,        //   │ Builtin  │
		AfterBuiltinStart,   // <─┘          │ Start
		BeforePluginStart,   // >─┐          │
		PluginStart,         //   │ Plugin   │
		AfterPluginStart,    // <─┘          │
		AfterStart,          // <────────────┘

		Running,

		BeforeStop,          // >────────────┐
		PluginStop,          // Plugin       │ Stop
		BuiltinStop,         // Builtin      │
		AfterStop,           // <────────────┘

		BeforePluginSuspend, // >────────────┐
		PluginSuspend,       // Plugin       │ Suspend
		AfterPluginSuspend,  // <────────────┘

		Suspended,

		BeforePluginResume,  // >────────────┐
		PluginResume,        // Plugin       │ Resume
		AfterPluginResume,   // <────────────┘

		Sink,
	}
}
