using System;
using System.Collections.Generic;
using System.ComponentModel;
using Kuicker.Data;

namespace Kuicker.Example
{
	[Package]
	[Category("Test")]
	[Description("first package")]
	public class YPackage : IPackage
	{
		public static string __PackageName
		{
			get
			{
				return "X";
			}
		}

		#region Xxx1
		public static DataResult Xxx1(
			string a1, 
			DateTime a2)
		{
			return Xxx1(
				new EntityApi(__PackageName), 
				a1, 
				a2
			);
		}
		
		public static DataResult Xxx1(
			EntityApi api, 
			string a1, 
			DateTime a2)
		{
			return api.ExecuteSp(
				"SPXxx",
				new Any("a1", a1),
				new Any("a2", a2)
			);
		}
		#endregion

		#region Xxx2
		public static int Xxx2(
			string a1, 
			DateTime a2)
		{
			return Xxx2(
				new EntityApi(__PackageName), 
				a1, 
				a2
			);
		}

		public static int Xxx2(
			EntityApi api, 
			string a1, 
			DateTime a2)
		{
			return api.ExecuteSpByScalar(
				"SPXxx",
				new Any("a1", a1),
				new Any("a2", a2)
			).ToInteger();
		}
		#endregion

		//#region Xxx3
		//public static Tuple<string, int> Xxx3(
		//	string a1, 
		//	DateTime a2)
		//{
		//	return Xxx3(
		//		new EntityApi(__PackageName), 
		//		a1, 
		//		a2
		//	);
		//}

		//public static Tuple<string, int> Xxx3(
		//	EntityApi api, 
		//	string a1, 
		//	DateTime a2)
		//{
		//	var rows = api.ExecuteSpByReader(
		//		"SPXxx",
		//		new Any("a1", a1),
		//		new Any("a2", a2)
		//	).ToDynamics();

		//	if(rows.Count == 0) {
		//		return new Tuple<string, int>(
		//			default(string),
		//			default(int)
		//		);
		//	}

		//	return new Tuple<string, int>(
		//		rows[0].A1.ToString(),
		//		rows[0].A2.ToInteger()
		//	);
		//}
		//#endregion

		#region Xxx4
		public static List<dynamic> Xxx4(
			string a1, 
			DateTime a2)
		{
			return Xxx4(
				new EntityApi(__PackageName), 
				a1, 
				a2
			);
		}

		public static List<dynamic> Xxx4(
			EntityApi api, 
			string a1, 
			DateTime a2)
		{
			return api.ExecuteSpByReader(
				"SPXxx",
				new Any("a1", a1),
				new Any("a2", a2)
			).ToDynamics()[0];
		}
		#endregion
	}
}
