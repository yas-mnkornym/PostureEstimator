using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class AdjustContrastCommand : CommandBase
	{
		/// <summary>
		/// value is in PERCENT (100=1, 50 = 0.5...)
		/// </summary>
		/// <param name="value"></param>
		public AdjustContrastCommand(int value)
			: base("adjustContrast", value)
		{ }
	}
}
