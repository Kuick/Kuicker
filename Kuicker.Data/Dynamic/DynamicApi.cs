// DynamicApi.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Kuicker.Data
{
	public class DynamicApi : DataApi
	{
		public DynamicApi()
			: this(string.Empty)
		{
		}

		public DynamicApi(string name)
			: base(name)
		{
			base.ExecuteCommandBehavior =
				CommandBehavior.CloseConnection
				|
				CommandBehavior.KeyInfo;
		}

		public DataResult Insert(DynamicRow row)
		{
			#region check
			if(row.__DatabaseFirst) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"DynamicRow instance ",
							"in 'DatabaseFirst' mode ",
							"can't invoke Insert() method."
						)
						.Error()
						.Message
				);
			}

			if(row.__DynamicTableNames.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"DynamicRow instance ",
							"can't invoke Insert() method ",
							"without settings table name."
						)
						.Error()
						.Message
				);
			}

			if(row.__DynamicTableNames.Count() > 1) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"You can't invoke Insert() method ",
							"for multiple tables ",
							"in DynamicRow instance."
						)
						.Error()
						.Message
				);
			}
			#endregion

			return base.Insert(
				row.__DynamicTableNames[0], 
				row.__DynamicValues
			);
		}

		public DataResult Update(DynamicRow row)
		{
			#region check
			if(!row.__DatabaseFirst) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"DynamicRow instance ",
							"in 'CodeFirst' mode ",
							"can't invoke Update() method."
						)
						.Error()
						.Message
				);
			}

			if(row.__DynamicTableNames.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"DynamicRow instance ",
							"can't invoke Update() method ",
							"without settings table name."
						)
						.Error()
						.Message
				);
			}

			if(row.__DynamicTableNames.Count() > 1) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"You can't invoke Update() method ",
							"for multiple tables ",
							"in DynamicRow instance."
						)
						.Error()
						.Message
				);
			}
			#endregion

			return base.Update(
				row.__DynamicTableNames[0], 
				row.__DynamicDirtyValues,
				row.__DynamicBasePrimaryKeys
			);
		}

		public DataResult Delete(DynamicRow row)
		{
			#region check
			if(!row.__DatabaseFirst) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"DynamicRow instance ",
							"in 'CodeFirst' mode ",
							"can't invoke Delete() method."
						)
						.Error()
						.Message
				);
			}

			if(row.__DynamicTableNames.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"DynamicRow instance ",
							"can't invoke Delete() method ",
							"without settings table name."
						)
						.Error()
						.Message
				);
			}

			if(row.__DynamicTableNames.Count() > 1) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							"You can't invoke Delete() method ",
							"for multiple tables ",
							"in DynamicRow instance."
						)
						.Error()
						.Message
				);
			}
			#endregion

			return base.Delete(
				row.__DynamicTableNames[0],
				row.__DynamicBasePrimaryKeys
			);
		}
	}
}
