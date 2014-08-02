// PackageSchema.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Linq;

namespace Kuicker.Data
{
	public class PackageSchema
	{
		public string PackageName { get; set; }
		public IQueryable<ProcedureSchema> Procedures { get; set; }


	}
}
