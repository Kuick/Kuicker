using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Kuicker;
using Kuicker.Data;
using Kuicker.Example;

namespace ExampleConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			using(Kernel kernel = Kernel.Current) {

				#region Trim
//				string lines = 
//@"  
//
//	
//  
//
//";
//				bool isNull = lines.IsNullOrEmpty();
//				string lines2 = lines.Trim();
				#endregion



				#region Log
				//TestLogBlock block = new TestLogBlock();
				////block.AccountID = "123";
				//// ...
				//block.Datas.Add("Asd", "asdsad");

				//block.Info();


				//new LogRecord() {
				//	Message = "Kernel is running.",
				//}.Info();

				//new TestLogBlock() {
				//	Message = "Press any key to exit.",
				//}.Info();



				//using(var il = new IntervalLogger<TestLogBlock>()) {
				//	il.Record.Symbol = "2885";
				//}
				#endregion


				#region Linq
				//List<Any> anys = new List<Any>();
				//anys.Add("A", 1);
				//anys.Add("B", 2);
				//anys.Add("C", 3);
				//anys.Add("D", 4);
				//anys.Add("A", 5);

				//var values = anys
				//	.Where(x => x.ToInt() > 0)
				//	.Select(x => x.Name)
				//	.Distinct()
				//	.ToArray();
				#endregion

				#region CRUD
				//using(var api = new DynamicApi("Client")) {

				//	Result result;

				//	// C
				//	var row = DynamicRow.Create("X");
				//	row.XSTRING = "0";
				//	row.XDATETIME = DateTime.Now;
				//	row.XBOOLEAN = true;
				//	row.XCHAR = 'A';
				//	row.XBYTE = (byte)8;
				//	row.XSHORT = (short)2048;
				//	row.XINTEGER = 12345;
				//	row.XLONG = 123234324L;
				//	row.XFLOAT = 1241.24564563F;
				//	row.XDOUBLE = 1332.363466D;
				//	row.XDECIMAL = 1124325523.453463466M;
				//	row.XENUM = DataFormat.Color;
				//	row.XBYTEARRAY = new byte[] {
				//		1, 2, 1, 1, 3, 0, 8, 9, 9
				//	};
				//	row.XCOLOR = Color.Black;
				//	row.XGUID = Guid.NewGuid();
				//	result = api.Insert(row);


				//	//var reader = api.ExecuteSqlByReader(
				//	//	"Select * From X"
				//	//);
				//	//var ds = reader.ToDynamics();
				//	//foreach(var d in ds) {
				//	//	d.XENUM = DataFormat.Boolean;
				//	//	api.Update(d);

				//	//	api.Delete(d);
				//	//}

				//}


				//using(var api = new DynamicApi("Client")) {
				//	var ds = api
				//		.ExecuteSqlByReader("Select * From X")
				//		.ToDynamics();

				//	foreach(var d in ds) {
				//		//d.XINTEGER = d.XINTEGER.ToInteger() + 1;
				//		//d.XENUM = DataFormat.Double;
				//		//var result = api.Update(d);

				//		var result = api.Delete(d);
				//	}
				//}
				#endregion


				#region Query into dynamic
				//using(var api = Kernel.Data.CreateApi("Client")) {
				//	var reader = api.ExecuteSqlByReader(
				//		//"SELECT y.XString as yString FROM X y"
				//		"SELECT * FROM X"
				//	);

				//	//var columns = reader.GetColumns();
				//	var ds = reader.ToDynamics();
				//	foreach(var d in ds) {
				//		string XSTRING = d.XSTRING.ToString();
				//		DateTime XDATETIME = d.XDATETIME.ToDateTime();
				//		bool XBOOLEAN = d.XBOOLEAN.ToBoolean();
				//		char XCHAR = d.XCHAR.ToChar();
				//		byte XBYTE = d.XBYTE.ToByte();
				//		short XSHORT = d.XSHORT.ToShort();
				//		int XINTEGER = d.XINTEGER.ToInteger();
				//		long XLONG = d.XLONG.ToLong();
				//		float XFLOAT = d.XFLOAT.ToFloat();
				//		double XDOUBLE = d.XDOUBLE.ToDouble();
				//		decimal XDECIMAL = d.XDECIMAL.ToDecimal();
				//		DataFormat XENUM = d.XENUM.ToEnum<DataFormat>();
				//		byte[] XBYTEARRAY = d.XBYTEARRAY.ToByteArray();
				//		Color XCOLOR = d.XCOLOR.ToColor();
				//		Guid XGUID = d.XGUID.ToGuid();

				//		Console.WriteLine("XSTRING=" + XSTRING);
				//		Console.WriteLine("XDATETIME=" + XDATETIME);
				//		Console.WriteLine("XBOOLEAN=" + XBOOLEAN);
				//		Console.WriteLine("XCHAR=" + XCHAR);
				//		Console.WriteLine("XBYTE=" + XBYTE);
				//		Console.WriteLine("XSHORT=" + XSHORT);
				//		Console.WriteLine("XINTEGER=" + XINTEGER);
				//		Console.WriteLine("XLONG=" + XLONG);
				//		Console.WriteLine("XFLOAT=" + XFLOAT);
				//		Console.WriteLine("XDOUBLE=" + XDOUBLE);
				//		Console.WriteLine("XDECIMAL=" + XDECIMAL);
				//		Console.WriteLine("XENUM=" + XENUM);
				//		Console.WriteLine("XBYTEARRAY=" + XBYTEARRAY.ToStringX());
				//		Console.WriteLine("XCOLOR=" + XCOLOR);
				//		Console.WriteLine("XGUID=" + XGUID);
				//	}
				//}
				#endregion

				#region dynamic query update
				//using(var api = new DynamicApi("Client")) {
				//	var ds = api
				//		.DataApi
				//		.ExecuteSqlByReader("Select * From X")
				//		.ToDynamics();

				//	foreach(var d in ds) {
				//		//d.XINTEGER = d.XINTEGER.ToInteger() + 1;
				//		//d.XENUM = DataFormat.Double;
				//		//var result = api.Update(d);

				//		var result = api.Delete(d);
				//	}
				//}
				#endregion

				#region dynamic Add
				//dynamic row = new DynamicRow("X");
				//// DataFormat.Unknown:

				//// DataFormat.String:
				//row.XSTRING = "ck";

				//// DataFormat.DateTime:
				//row.XDATETIME = DateTime.Now;

				//// DataFormat.Boolean:
				//row.XBOOLEAN = true;

				//// DataFormat.Char:
				//row.XCHAR = 'A';

				//// DataFormat.Byte:
				//row.XBYTE = (byte)8;

				//// DataFormat.Short:
				//row.XSHORT = (short)2048;

				//// DataFormat.Integer:
				//row.XINTEGER = 12345;

				//// DataFormat.Long:
				//row.XLONG = 123234324L;

				//// DataFormat.Float:
				//row.XFLOAT = 1241.24564563F;

				//// DataFormat.Double:
				//row.XDOUBLE = 1332.363466D;

				//// DataFormat.Decimal:
				//row.XDECIMAL = 1124325523.453463466M;

				//// DataFormat.Enum:
				//row.XENUM = DataFormat.Color;

				////// DataFormat.Object:
				////data.A13 = new TestLogBlock();

				////// DataFormat.Objects:
				////data.A14 = new object[]{"12", 213};

				//// DataFormat.ByteArray:
				//row.XBYTEARRAY = new byte[] {
				//	1, 2, 1, 1, 3, 0, 8, 9, 9
				//};

				//// DataFormat.Color:
				//row.XCOLOR = Color.Black;

				//// DataFormat.Guid:
				//row.XGUID = Guid.NewGuid();

				//using(DynamicApi api = new DynamicApi("Client")) {
				//	var result = api.Insert(row);
				//}
				#endregion


				#region DbCommandBuilder
				//var q1 = dbCommandBuilder.UnquoteIdentifier("sss.zz");
				//var q2 = dbCommandBuilder.UnquoteIdentifier("[sss].zz");
				//var q3 = dbCommandBuilder.UnquoteIdentifier("'sss'.[zz]");
				//var q4 = dbCommandBuilder.UnquoteIdentifier("[sss].[zz]");
				//var q5 = dbCommandBuilder.UnquoteIdentifier("[sss].[z]z");
				#endregion


				#region Loop Log
				//while(true) {
				//	new TestLogBlock() {
				//		Message = "from - Console",
				//	}.Error();
				//	Thread.Sleep(600000);
				//}
				#endregion





				//Execute();


				//Console.WriteLine("Kernel is running.");
				//Console.WriteLine("Press any key to exit.");



				#region Entity
				//using(var api = new EntityApi("Default")) {
				//	var sql = Sql<XEntity>
				//		.CreateSelect()
				//		.Where(x =>
				//			(
				//			#region =, !=, >, >=, <, <=
				//				//1 == 1
				//			#endregion

				//			#region Like, StartsWith, StartsX, EndsWith, EndsX
				//				//&
				//				(
				//				x.XString.Like("2")
				//				||
				//				x.XString.StartsX("2", "asd", "fds", "qweqw", ":")
				//				)
				//				&
				//				x.XString.StartsWith("2")
				//				&
				//				x.XString.EndsX("2", "asd")
				//				&
				//				x.XString.EndsWith("2")
				//			#endregion

				//			#region In
				//				//&
				//				//x.XString.In("2", "d", "qwe")
				//				//&
				//				//x.XInteger.In(3, 4, 1, 67)
				//				//&
				//				//x.XEnum.In(DataFormat.Color, DataFormat.Double)
				//				//&
				//				//x.XString.In(Sql<XEntity>.CreateSelect())
				//			#endregion

				//			#region Between
				//				//&
				//				//x.XInteger.Between(3, 8)
				//				//&
				//				//x.XDateTime.Between(
				//				//	new DateTime(2010, 12, 3),
				//				//	new DateTime(2012, 12, 3)
				//				//)
				//			#endregion
				//			)
				//			|
				//			x.XBoolean == true
				//		);

				//	SqlParseResult pr = sql.Parse(api);

				//	var all = api.QueryBySql<XEntity>(
				//		pr.Command, pr.Parameters.ToArray()
				//	);
				//}


				#endregion

				Console.ReadKey();

				// code here
			}
		}

		static void Execute()
		{

			LogRecord.Create().SetMessage("1111").Warn();
		}
	}
}
