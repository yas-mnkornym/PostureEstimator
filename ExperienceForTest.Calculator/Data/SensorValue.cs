using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperienceForTest.Calculator.Data
{
	public class SensorValue
	{
		public SensorValue(int x, int y, double value)
		{
			X = x;
			Y = y;
			Value = value;
		}

		public SensorValue() { }

		public int X { get; set; }
		public int Y { get; set; }
		public double Value { get; set; }
	}
}
