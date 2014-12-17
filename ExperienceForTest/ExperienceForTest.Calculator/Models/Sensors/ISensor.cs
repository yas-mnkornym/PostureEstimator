using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperienceForTest.Calculator.Data;

namespace ExperienceForTest.Calculator.Models.Sensors
{
	public interface ISensor
	{
		event EventHandler<FrameEventArgs> Frame;
	}

	public class FrameEventArgs : EventArgs
	{
		public FrameEventArgs(Point2D[] points)
		{
			Points = points;
		}

		public Point2D[] Points { get; private set; }
	}
}
