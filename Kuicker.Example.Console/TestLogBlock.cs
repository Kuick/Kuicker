using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kuicker;

namespace ExampleConsole
{
	public class TestLogBlock : LogRecord<TestLogBlock>
	{
		public string Department { get; set; }
	}
}
