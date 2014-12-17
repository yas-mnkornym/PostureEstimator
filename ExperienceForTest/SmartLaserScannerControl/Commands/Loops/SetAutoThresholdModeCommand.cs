using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class SetAutoThresholdModeCommand : CommandBase
	{
		public SetAutoThresholdModeCommand(bool enableAutoThreshold)
			: base("autoThreshold", enableAutoThreshold ? 1 : 0)
		{ }
	}
}
