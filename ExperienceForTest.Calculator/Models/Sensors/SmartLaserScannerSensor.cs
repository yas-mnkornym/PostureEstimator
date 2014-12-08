using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperienceForTest.Calculator.Models.Sensors
{
	public class SmartLaserScannerSensor : ISensor
	{

		public event EventHandler<FrameEventArgs> Frame;
	}
}
