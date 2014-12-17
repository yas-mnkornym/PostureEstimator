using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class AdjustMinusAngleCommand : CommandBase
	{
		public AdjustMinusAngleCommand(int value)
			: base("adjustMinusAngle", value)
		{ }
	}
}
