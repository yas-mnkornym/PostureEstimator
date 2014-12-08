using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class AdjustRenderFractionCommand : CommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">value 100 means no change of speed. 200 is twice as fast, 50 is half as fast.</param>
		public AdjustRenderFractionCommand(int value)
			: base("adjustRenderFraction", value)
		{ }
	}
}
