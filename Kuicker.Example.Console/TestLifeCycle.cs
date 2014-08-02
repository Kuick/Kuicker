using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kuicker;

namespace ExampleConsole
{
	public class TestLifeCycle : LifeCycle
	{
		public override void DoBeforeBuiltinStart()
		{
			LogRecord.Intercepter = x => {
				x.Title = "Default Title";
			};

			TestLogBlock.Intercepter = x => {
				x.Department = "1234";
			};
		}
	}
}
