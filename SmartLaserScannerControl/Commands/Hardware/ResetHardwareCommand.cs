using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Hardware
{
	public class ResetHardwareCommand : CommandBase
	{
		public ResetHardwareCommand()
			: base("mbedReset")
		{ }
	}
}
