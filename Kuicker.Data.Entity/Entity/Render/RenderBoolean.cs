// RenderBoolean.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Web;

namespace Kuicker.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class RenderBoolean : Attribute, IRender
	{
		public string ToString(object value)
		{
			return value.ToBoolean().ToStringX();
		}

		public HtmlString ToHtml(object value)
		{
			return ToString(value).ToHtml();
		}
	}
}
