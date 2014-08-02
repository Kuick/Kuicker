// ArgumentSchema.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;
using System.Text;

namespace Kuicker.Data
{
	public class ArgumentSchema
	{
		public string PackageName { get; set; }
		public string ProcedureName { get; set; }
		public string ArgumentName { get; set; }
		public string Comments { get; set; }
		public int Overload { get; set; }
		public int Sequence { get; set; }
		public string DataType { get; set; }
		public string InOut { get; set; }
		public int DataLength { get; set; }

		public string ToTypeName(ISqlFormater formater)
		{
			var ef = EnumCache.Get<DataFormat>();
			var format = formater.ToDataFormat(DataType);
			var ei = ef.Get(format.ToStringX());
			var type = ei.Description;
			return type;
		}

		public string ToMethodArgument(ISqlFormater formater)
		{
			return string.Concat(
				ToTypeName(formater),
				" ",
				ArgumentName
			);
		}

		//public string ToParameterArgument(ISqlFormater formater)
		//{
		//	return string.Concat(
		//		ToTypeName(formater),
		//		" ",
		//		ArgumentName
		//	);
		//}
	}
}
