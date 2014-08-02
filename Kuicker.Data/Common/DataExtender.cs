// DataExtender.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;

namespace Kuicker.Data
{
	public static class DataExtender
	{
		public static object ToParameterValue(this object value)
		{
			switch(value.GetFormat()) {
				case DataFormat.Unknown:
					return value;
				case DataFormat.String:
					return value;
				case DataFormat.DateTime:
					return value;
				case DataFormat.Boolean:
					return (bool)value ? 1 : 0;
				case DataFormat.Char:
					return value;
				case DataFormat.Byte:
					return value;
				case DataFormat.Short:
					return value;
				case DataFormat.Integer:
					return value;
				case DataFormat.Long:
					return value;
				case DataFormat.Float:
					return value;
				case DataFormat.Double:
					return value;
				case DataFormat.Decimal:
					return value;
				case DataFormat.Enum:
					return value.ToString();
				case DataFormat.ByteArray:
					return value;
				case DataFormat.Color:
					return ((Color)value).ToArgb();
				case DataFormat.Guid:
					return value.ToString();
				case DataFormat.TimeSpan:
					return value.ToTimeSpan();
				default:
					return value;
			}
		}

		public static object ToParameterValue(this Any any)
		{
			if(null == any) {
				throw new ArgumentNullException("any");
			}
			return ToParameterValue(any.Value);
		}

		public static object ToParameterValue(
			this List<Any> anys, string name)
		{
			if(anys.IsNullOrEmpty()) {
				throw new ArgumentNullException("anys");
			}
			if(name.IsNullOrEmpty()) {
				throw new ArgumentNullException("name");
			}

			var any = anys.Get(name);
			if(null == any) {
				throw new Exception(
					"Can't find the value called '" + name + "'"
				);
			}

			return any.ToParameterValue();
		}
	}
}
