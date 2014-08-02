// DynamicRow.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Dynamic;
using System.Linq;

namespace Kuicker.Data
{
	public class DynamicRow : DynamicObject
	{
		public DynamicRow()
			: this(string.Empty)
		{
		}
		public DynamicRow(string tableName)
		{
			__ReturnAny = true;
			__DatabaseFirst = false;
			__TableName = tableName;
			__Values = new List<Any>();
			__HashCodes = new List<Any>();
			__DbNulls = new List<string>();
		}

		public bool __ReturnAny { get; set; }

		public bool __DatabaseFirst { get; internal set; }
		public string __TableName { get; internal set; }
		public List<Any> __Values { get; internal set; }
		public List<Any> __HashCodes { get; internal set; }
		public List<string> __DbNulls { get; internal set; }
		public int __OriginalHashCode { get; internal set; }

		public Result Add()
		{
			if(__DatabaseFirst) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							new string[] {
								"DynamicRow instance ",
								"in 'DatabaseFirst' mode ",
								"can't invoke Add() method.",
							}.Join()
						)
						.Error()
						.Message
				);
			}

			if(__TableName.IsNullOrEmpty()) {
				throw new Exception(
					LogRecord
						.Create()
						.SetMessage(
							new string[] {
								"DynamicRow instance ",
								"can't invoke Add() method ",
								"without settings __TableName.",
							}.Join()
						)
						.Error()
						.Message
				);
			}

			using(var api = Kernel.Data.CreateApi(__TableName)) {
				// Sql
				var names = __Values.GetNames();
				var sql = string.Format(
	@"INSERT INTO {0} (
{1}
) VALUES (
{2}
)",
					string.Concat(
						api.CommandBuilder.QuotePrefix,
						__TableName,
						api.CommandBuilder.QuoteSuffix
					),
					names
						.Join(
							"," + Environment.NewLine,
							"	" + api.CommandBuilder.QuotePrefix,
							api.CommandBuilder.QuoteSuffix
						),
					names
						.Join(
							"," + Environment.NewLine,
							"	" + api.ParameterMarker,
							string.Empty
						)
				);

				// Parameters
				var parameters = new List<DbParameter>();

				foreach(var value in __Values) {
					var parameter = api.Factory.CreateParameter();
					parameter.ParameterName = string.Concat(
						api.ParameterMarker, value.Name
					);
					parameter.Value = value.ToParameterValue();
					parameters.Add(parameter);
				}

				// Execute
				var result = api.ExecuteSql(
					sql, parameters.ToArray()
				);
				return result;
			}
		}


		public bool Dirty
		{
			get
			{
				if(!__DatabaseFirst) { return false; }

				foreach(Any any in __Values) {
					int hashCode = null == any.Value
						? 0 : any.Value.GetHashCode();
					int originalHashCode =
						__HashCodes.ToInteger(any.Name);
					if(originalHashCode != hashCode) {
						return true;
					}
				}

				return false;
			}
		}

		public List<Any> DirtyValues
		{
			get
			{
				if(!__DatabaseFirst) { return __Values; }

				var list = new List<Any>();
				if(__DatabaseFirst) {
					foreach(Any any in __Values) {
						int hashCode = null == any.Value
							? 0 : any.Value.GetHashCode();
						int originalHashCode =
							__HashCodes.ToInteger(any.Name);
						if(originalHashCode != hashCode) {
							list.Add(any);
						}
					}
				}
				return list;
			}
		}

		public override bool TryGetMember(
			GetMemberBinder binder, out object result)
		{
			return TryGetValue(binder.Name, out result);
		}

		public bool TryGetValue(string name, out object result)
		{
			result = null;
			if(name.IsNullOrEmpty()) { return false; }

			name = name.ToUpper();
			result = __Values.FirstOrDefault(x =>
				x.Name.ToUpper() == name
			);
			if(null == result) {
				name = name.Replace("_", "");
				result = __Values.FirstOrDefault(x =>
					x.Name.Replace("_", "").ToUpper() == name
				);
			}

			if(null == result) {
				if(__ReturnAny) {
					result = new Any(name);
				}
				return false;
			} else {
				Any any = result as Any;
				if(__ReturnAny) {
					result = null == any ? new Any(name) : any;
				} else {
					result = null == any ? null : any.Value;
				}
				return true;
			}
		}

		public override bool TrySetMember(
			SetMemberBinder binder, object value)
		{
			return TrySetMember(binder.Name, value);
		}

		public bool TrySetMember(string name, object value)
		{
			if(__DatabaseFirst) {
				if(__Values.Contains(name)) {
					__Values.SafeAdd(name, value);
					return true;
				} else {
					return false;
				}
			} else {
				if(null == __Values) {
					__Values = new List<Any>();
				}
				__Values.SafeAdd(name, value);
				return true;
			}
		}
	}
}
