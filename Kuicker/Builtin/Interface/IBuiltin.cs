// IBuiltin.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Threading.Tasks;

namespace Kuicker
{
	public interface IBuiltin
	{
		string Name { get; }

		Task Start();
		Task Stop();
	}
}
