// Data.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Kuicker.Data
{
	public class Data : BuiltinBase, IData
	{
		public IDataApi CreateApi()
		{
			return new DataApi();
		}

		public IDataApi CreateApi(string name)
		{
			return new DataApi(name);
		}

		public override string Name
		{
			get { return Constants.Builtin.Data; }
		}

		public override Task Start()
		{
			Task task = Task.Run(() => {
				DataSettings.Configure();
				// schema sync (db first | code first)
			});
			return task;
		}

		public override Task Stop()
		{
			Task task = Task.Run(() => {
				DataSettings.Clear();
			});
			return task;
		}
	}
}
