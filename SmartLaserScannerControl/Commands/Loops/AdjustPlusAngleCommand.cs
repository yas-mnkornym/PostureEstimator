using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class AdjustPlusAngleCommand : CommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">this is not a factor, but an additive quantity to the current delay</param>
		public AdjustPlusAngleCommand(int value)
			: base("adjustPlusAngle", value)
		{ }
	}
}
