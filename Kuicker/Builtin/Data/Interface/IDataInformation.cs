// IDataInformation.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Kuicker
{
	public interface IDataInformation
	{
		string CompositeIdentifierSeparatorPattern { get; }
		string DataSourceProductName { get; }
		string DataSourceProductVersion { get; }
		string DataSourceProductVersionNormalized { get; }
		GroupByBehavior GroupByBehavior { get; }
		string IdentifierPattern { get; }
		IdentifierCase IdentifierCase { get; }
		bool OrderByColumnsInSelect { get; }
		string ParameterMarkerFormat { get; }
		string ParameterMarkerPattern { get; }
		int ParameterNameMaxLength { get; }
		string ParameterNamePattern { get; }
		string QuotedIdentifierPattern { get; }
		Regex QuotedIdentifierCase { get; }
		string StatementSeparatorPattern { get; }
		Regex StringLiteralPattern { get; }
		SupportedJoinOperators SupportedJoinOperators { get; }

		Regex ParameterNamePatternRegex { get; }
		string ParameterMarker { get; }
	}
}
