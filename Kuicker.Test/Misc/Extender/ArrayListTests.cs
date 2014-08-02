using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kuicker.Test
{
	public class ArrayListTests
	{
		[Fact]
		public void IsNullOrEmpty()
		{
			ArrayList al1 = null;
			Assert.True(al1.IsNullOrEmpty());

			ArrayList al2 = new ArrayList();
			Assert.True(al2.IsNullOrEmpty());

			ArrayList al3 = new ArrayList();
			al3.Add(1);
			al3.Add("A");
			al3.Add(DateTime.Now);
			Assert.False(al3.IsNullOrEmpty());
		}
	}
}
