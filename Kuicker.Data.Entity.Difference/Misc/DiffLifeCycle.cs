// SqlClientBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using Newtonsoft.Json;

namespace Kuicker.Data
{
	public class DiffLifeCycle : LifeCycle
	{
		public override void DoAfterBuiltinStart()
		{
			// Handler
			Difference.Handler = (method, original, current) => {
				try {
					Difference difference = DiffEntity.CompareDiff(
						original, current
					);
					if(null == difference) { return; }

					difference.Who = RunTime.IsWebApp
						? HttpContext.Current.User.Identity.Name
						: Environment.UserName;
					difference.Method = method;

					var diff = new DiffEntity();
					diff.Title = string.Format(
						"{0}({1}) {2}",
						difference.EntityName,
						difference.PrimaryValue,
						method
					);
					diff.Who = difference.Who;
					diff.EntityName = difference.EntityName;
					diff.KeyValue = difference.PrimaryValue;
					diff.Method = difference.Method;
					diff.Json = JsonConvert.SerializeObject(difference);
					diff.Add();

				} catch(Exception ex) {
					LogRecord
						.Create()
						.Add(
							"Original Entity Name",
							null == original
								? "Unknown"
								: original.__EntityName
						)
						.Add(
							"Original Datas",
							null == original
								? "NULL"
								: original.ToAnys().ToStringX()
						)
						.Add(
							"Current Entity Name",
							null == current
								? "Unknown"
								: current.__EntityName
						)
						.Add(
							"Current Datas",
							null == current
								? "NULL"
								: current.ToAnys().ToStringX()
						)
						.Add(ex)
						.Error();
				}
			};

			// History
			Difference.History = (entityName, keyValue) => {
				try {
					var keys = Entity.BuildKeyColumnNameValues(
						entityName, keyValue
					);

					var all = DiffEntity
						.Where(x =>
							x.EntityName == entityName
							&&
							x.KeyValue == keyValue
						)
						.Ascend(x => x.CreatedDate)
						.Query();

					var diffs = all
						.Select(x =>
							JsonConvert.DeserializeObject<Difference>(
								x.Json
							)
						)
						.ToList();

					return diffs;

				} catch(Exception ex) {
					LogRecord
						.Create()
						.Add("Entity Name", entityName)
						.Add("Key Value", keyValue)
						.Add(ex)
						.Error();
					throw;
				}
			};
		}
	}
}
