using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands.Loops
{
	public class SetSizeCommand : CommandBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">direct size of the scafold</param>
		public SetSizeCommand(int value)
			: base("setSize", value)
		{ }
	}
}
