// DataFormat.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.ComponentModel;

namespace Kuicker
{
	[DefaultValue(DataFormat.Unknown)]
	public enum DataFormat
	{
		Unknown,

		[Description("string")]
		String,

		[Description("DateTime")]
		DateTime,


		[Description("bool")]
		Boolean,

		[Description("char")]
		Char,

		[Description("byte")]
		Byte,

		[Description("short")]
		Short,

		[Description("int")]
		Integer,

		[Description("long")]
		Long,

		[Description("float")]
		Float,

		[Description("double")]
		Double,

		[Description("decimal")]
		Decimal,


		[Description("Enum")]
		Enum,


		[Description("object")]
		Object,

		[Description("object[]")]
		Objects,

		[Description("byte[]")]
		ByteArray,

		[Description("Color")]
		Color,

		[Description("Guid")]
		Guid,

		[Description("TimeSpan")]
		TimeSpan,
	}
}
