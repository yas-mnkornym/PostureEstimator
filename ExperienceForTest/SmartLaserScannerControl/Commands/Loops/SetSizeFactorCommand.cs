using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class SetSizeFactorCommand : CommandBase
	{
		public SetSizeFactorCommand(int factor)
			: base("sizeFactor", factor)
		{ }
	}
}
