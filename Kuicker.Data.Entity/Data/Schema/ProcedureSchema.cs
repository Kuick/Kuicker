// ProcedureSchema.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;
using System.Text;

namespace Kuicker.Data
{
	public class ProcedureSchema
	{
		public string PackageName { get; set; }
		public string ProcedureName { get; set; }
		public int Overload { get; set; }
		public IQueryable<ArgumentSchema> Ins { get; set; }
		public IQueryable<ArgumentSchema> Outs { get; set; }

		public string DbFullName
		{
			get
			{
				return string.Concat(
					PackageName,
					".",
					ProcedureName
				);
			}
		}

		public string ToOutType(ISqlFormater formater)
		{
			var count = Outs.IsNullOrEmpty()
				? 0
				: Outs.Count();

			switch(count) {
				case 0:
					return "DataResult";
				case 1:
					return Outs.FirstOrDefault().ToTypeName(formater);
				default:
					return "List<dynamic>[]";
			}
		}

		public string ToApiMethod()
		{
			var count = Outs.IsNullOrEmpty()
				? 0
				: Outs.Count();

			switch(count) {
				case 0:
					return "ExecuteSp";
				case 1:
					return "ExecuteSpByScalar";
				default:
					return "ExecuteSpByReader";
			}
		}

		public string ToOutFormatMethod(ISqlFormater formater)
		{
			var count = Outs.IsNullOrEmpty()
				? 0
				: Outs.Count();

			switch(count) {
				case 0:
					return string.Empty;
				case 1:
					return string.Concat(
						".To", 
						Outs
							.FirstOrDefault()
							.ToTypeName(formater)
							.ToPascalCasing(),
						"()"
					);
				default:
					return ".ToDynamics()";
			}
		}
	}
}
