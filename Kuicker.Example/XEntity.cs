using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kuicker;
using Kuicker.Data;

namespace Kuicker.Example
{
	[Table]
	[Category("Test")]
	[Description("first table")]
	public class XEntity : Entity<XEntity>
	{
		public XEntity()
			: base()
		{
		}

		/// <summary>
		/// XSTRING
		/// </summary>
		[Description("XSTRING")]
		[DataMember, Column, PrimaryKey]
		public string XString { get; set; }

		/// <summary>
		/// XDATETIME
		/// </summary>
		[Description("XDATETIME")]
		[DataMember, Column, AllowDBNull]
		public DateTime XDateTime { get; set; }

		/// <summary>
		/// XBOOLEAN
		/// </summary>
		[Description("XBOOLEAN")]
		[DataMember, Column, AllowDBNull]
		public decimal XBoolean { get; set; }

		/// <summary>
		/// XCHAR
		/// </summary>
		[Description("XCHAR")]
		[DataMember, Column, AllowDBNull]
		public string XChar { get; set; }

		/// <summary>
		/// XBYTE
		/// </summary>
		[Description("XBYTE")]
		[DataMember, Column, AllowDBNull]
		public decimal XByte { get; set; }

		/// <summary>
		/// XSHORT
		/// </summary>
		[Description("XSHORT")]
		[DataMember, Column, PrimaryKey]
		public decimal XShort { get; set; }

		/// <summary>
		/// XINTEGER
		/// </summary>
		[Description("XINTEGER")]
		[DataMember, Column, AllowDBNull]
		public decimal XInteger { get; set; }

		/// <summary>
		/// XLONG
		/// </summary>
		[Description("XLONG")]
		[DataMember, Column, AllowDBNull]
		public decimal XLong { get; set; }

		/// <summary>
		/// XFLOAT
		/// </summary>
		[Description("XFLOAT")]
		[DataMember, Column, AllowDBNull]
		public decimal XFloat { get; set; }

		/// <summary>
		/// XDOUBLE
		/// </summary>
		[Description("XDOUBLE")]
		[DataMember, Column, AllowDBNull]
		public decimal XDouble { get; set; }

		/// <summary>
		/// XDECIMAL
		/// </summary>
		[Description("XDECIMAL")]
		[DataMember, Column, AllowDBNull]
		public decimal XDecimal { get; set; }

		/// <summary>
		/// XGUID
		/// </summary>
		[Description("XGUID")]
		[DataMember, Column, AllowDBNull]
		public string XGuid { get; set; }

		public override string __TableName
		{
			get
			{
				return "X";
			}
		}
	}
}
