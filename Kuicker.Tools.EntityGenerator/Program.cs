using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Kuicker;

namespace Kuicker.Tools.EntityGenerator
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			using(var kernel = Kernel.Current) {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new frmMain());
			}
		}
	}
}
