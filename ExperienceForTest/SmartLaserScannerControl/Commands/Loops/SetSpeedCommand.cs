using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class SetSpeedCommand : CommandBase
	{
		/// <summary>
		/// スピードを設定する
		/// </summary>
		/// <param name="speed">1 -> 0.1, 10 -> 1</param>
		public SetSpeedCommand(int speed)
			: base("setSpeed", speed)
		{ }
	}
}
