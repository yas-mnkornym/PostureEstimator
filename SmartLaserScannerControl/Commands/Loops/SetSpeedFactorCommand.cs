using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class SetSpeedFactorCommand : CommandBase
	{
		public SetSpeedFactorCommand(int factor)
			: base("speedFactor", factor)
		{ }
	}
}
