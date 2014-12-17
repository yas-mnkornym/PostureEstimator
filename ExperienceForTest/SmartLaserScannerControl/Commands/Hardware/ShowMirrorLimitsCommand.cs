using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SmartLaserScannerControl.Commands.Hardware
{
	public class ShowMirrorLimitsCommand : CommandBase
	{
		public ShowMirrorLimitsCommand(int num)
			: base("showMirrorLimits", num)
		{ }
	}
}
