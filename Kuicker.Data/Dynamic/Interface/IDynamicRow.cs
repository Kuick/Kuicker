// IDynamicRow.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Kuicker.Data
{
	public interface IDynamicRow : IDynamicMetaObjectProvider
	{
		bool __ReturnAny { get; set; }
		bool __DatabaseFirst { get; }

		List<DynamicColumn> __DynamicColumns { get; }
		List<Any> __DynamicValues { get; }
		List<Any> __DynamicOriginalValues { get; }
		List<Any> __OriginalHashCodes { get; }
		List<string> __DynamicDbNulls { get; }
		int __OriginalHashCode { get; }
		List<string> __DynamicTableNames { get; }
		List<string> __DynamicPrimaryKeys { get; }

		bool __Dirty { get; }
		List<Any> __DynamicDirtyValues { get; }
		List<Any> __DynamicBasePrimaryKeys { get; }
	}
}
