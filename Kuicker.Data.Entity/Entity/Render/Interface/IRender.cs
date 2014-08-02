// IRender.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Web;

namespace Kuicker.Data
{
	public interface IRender
	{
		string ToString(object value);
		HtmlString ToHtml(object value);
	}
}
