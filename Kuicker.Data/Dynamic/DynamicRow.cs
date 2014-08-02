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
	public class DynamicRow : DynamicObject, IDynamicRow
	{
		public DynamicRow()
			: this(string.Empty)
		{
		}
		private DynamicRow(string tableName)
		{
			__ReturnAny = true;
			__DatabaseFirst = false;
			__DynamicColumns = new List<DynamicColumn>();
			__FlatValues = new List<Any>();
			__DynamicValues = new List<Any>();
			__DynamicOriginalValues = new List<Any>();
			__OriginalHashCodes = new List<Any>();
			__DynamicDbNulls = new List<string>();

			__DynamicTableNames.Add(tableName);
		}

		public static dynamic CreateDynamic(string tableName)
		{
			return new DynamicRow(tableName) as dynamic;
		}

		public bool __ReturnAny { get; set; }

		public bool __DatabaseFirst { get; internal set; }
		public List<DynamicColumn> __DynamicColumns { get; internal set; }
		public List<Any> __FlatValues { get; internal set; }
		public List<Any> __DynamicValues { get; internal set; }
		public List<Any> __DynamicOriginalValues { get; internal set; }
		public List<Any> __OriginalHashCodes { get; internal set; }
		public List<string> __DynamicDbNulls { get; internal set; }
		public int __OriginalHashCode { get; internal set; }

		private List<string> _xTableNames;
		public List<string> __DynamicTableNames
		{
			get
			{
				if(null == _xTableNames) {
					_xTableNames = __DynamicColumns.TableNames();
				}
				return _xTableNames;
			}
		}

		private List<string> _xPrimaryKeys;
		public List<string> __DynamicPrimaryKeys
		{
			get
			{
				if(null == _xPrimaryKeys) {
					_xPrimaryKeys = __DynamicColumns.PrimaryKeys();
				}
				return _xPrimaryKeys;
			}
		}

		public bool __Dirty
		{
			get
			{
				if(!__DatabaseFirst) { return false; }

				foreach(Any any in __DynamicValues) {
					int hashCode = null == any.Value
						? 0 : any.Value.GetHashCode();
					int originalHashCode =
						__OriginalHashCodes.ToInteger(any.Name);
					if(originalHashCode != hashCode) {
						return true;
					}
				}

				return false;
			}
		}

		public List<Any> __DynamicDirtyValues
		{
			get
			{
				if(!__DatabaseFirst) { return __DynamicValues; }

				var list = new List<Any>();
				foreach(Any any in __DynamicValues) {
					int hashCode = null == any.Value
						? 0 : any.Value.GetHashCode();
					int originalHashCode =
						__OriginalHashCodes.ToInteger(any.Name);
					if(originalHashCode != hashCode) {
						list.Add(any);
					}
				}
				return list;
			}
		}

		public List<Any> __DynamicBasePrimaryKeys
		{
			get
			{
				List<string> keys = __DynamicPrimaryKeys;

				if(keys.IsNullOrEmpty()) {
					foreach(var column in __DynamicColumns) {
						// exclude
						if(column.DataType.IsBytes()) { continue; }

						// include
						keys.Add(column.BaseColumnName);
					}

					if(keys.IsNullOrEmpty()) {
						throw new Exception(
							"__AllKeys can't null or empty"
						);
					}
				}

				List<Any> list = new List<Any>();
				foreach(var value in keys) {
					var column = __DynamicColumns.FirstOrDefault(x =>
						x.BaseColumnName == value
					);
					if(null == column) {
						throw new Exception(
							new[]{
								"DynamicRow.BasePrimaryKeys's value ",
								"doesn't exists in column collection."
							}.Join()
						);
					}

					list.Add(
						value,
						__DynamicOriginalValues.ToParameterValue(
							column.ColumnName
						)
					);
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
			result = __FlatValues.FirstOrDefault(x =>
				x.Name.ToUpper() == name
			);
			if(null == result) {
				name = name.Replace("_", "");
				result = __FlatValues.FirstOrDefault(x =>
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
				if(__DynamicValues.Contains(name)) {
					__DynamicValues.SafeAdd(name, value);
					__FlatValues.SafeAdd(name, value);
					return true;
				} else {
					return false;
				}
			} else {
				if(null == __DynamicValues) {
					__DynamicValues = new List<Any>();
				}
				if(null == __FlatValues) {
					__DynamicValues = new List<Any>();
				}
				__DynamicValues.SafeAdd(name, value);
				__FlatValues.SafeAdd(name, value);
				return true;
			}
		}
	}
}
