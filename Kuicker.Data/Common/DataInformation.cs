// DataInformation.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Kuicker.Data
{
	public class DataInformation : IDataInformation
	{
		public DataInformation(DataTable table)
		{
			if(table.Rows.Count != 1) {
				throw new Exception(
					new[]{
						"DataSourceInformation ",
						"should have only one row data"
					}.Join()
				);
			}

			DataRow row = table.Rows[0];
			foreach(DataColumn c in table.Columns) {
				string name = c.ColumnName;
				if(name.IsNullOrEmpty()) { continue; }
				object value = row[name];
				if(value == DBNull.Value) { value = null; }
				if(value == null) { continue; }

				switch(name) {
					case "QuotedIdentifierCase":
						QuotedIdentifierCase = new Regex(
							value.ToString()
						);
						break;
					case "StringLiteralPattern":
						StringLiteralPattern = new Regex(
							value.ToString()
						);
						break;
					case "GroupByBehavior":
						value = Convert.ChangeType(
							value,
							EnumCache
								.GetUnderlyingType<GroupByBehavior>()
						);
						GroupByBehavior = (GroupByBehavior)value;
						break;
					case "IdentifierCase":
						value = Convert.ChangeType(
							value,
							EnumCache
								.GetUnderlyingType<IdentifierCase>()
						);
						IdentifierCase = (IdentifierCase)value;
						break;
					case "SupportedJoinOperators":
						value = Convert.ChangeType(
							value,
							EnumCache
								.GetUnderlyingType<SupportedJoinOperators>()
						);
						SupportedJoinOperators =
							(SupportedJoinOperators)value;
						break;
					default:
						if(!this.SetValue(name, value)) {
							LogRecord
								.Create()
								.SetMessage("Missing parameter.")
								.Add("Name", name)
								.Add("value", value)
								.Debug();
						}
						break;
				}
			}

			ParameterNamePatternRegex = new Regex(
				ParameterNamePattern
			);
			ParameterMarker = ParameterNameMaxLength != 0
				? ParameterMarkerPattern.Substring(0, 1)
				: ParameterMarkerFormat;

		}

		public string CompositeIdentifierSeparatorPattern { get; private set; }
		public string DataSourceProductName { get; private set; }
		public string DataSourceProductVersion { get; private set; }
		public string DataSourceProductVersionNormalized { get; private set; }
		public GroupByBehavior GroupByBehavior { get; private set; }
		public string IdentifierPattern { get; private set; }
		public IdentifierCase IdentifierCase { get; private set; }
		public bool OrderByColumnsInSelect { get; private set; }
		public string ParameterMarkerFormat { get; private set; }
		public string ParameterMarkerPattern { get; private set; }
		public int ParameterNameMaxLength { get; private set; }
		public string ParameterNamePattern { get; private set; }
		public string QuotedIdentifierPattern { get; private set; }
		public Regex QuotedIdentifierCase { get; private set; }
		public string StatementSeparatorPattern { get; private set; }
		public Regex StringLiteralPattern { get; private set; }
		public SupportedJoinOperators SupportedJoinOperators { get; private set; }

		public Regex ParameterNamePatternRegex { get; private set; }
		public string ParameterMarker { get; private set; }
	}
}
