using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;

namespace ExperienceForTest.Calculator.Data
{
	public struct Point2D
	{
		public double X { get; set; }
		public double Y { get; set; }

		public override string ToString()
		{
			return DynamicJson.Serialize(this);
		}
	}

	public struct Square
	{
		public Point2D P1 { get; set; }
		public Point2D P2 { get; set; }
		public Point2D P3 { get; set; }
		public Point2D P4 { get; set; }

		public Point2D[] PointArray
		{
			get
			{
				return new Point2D[] { P1, P2, P3, P4 };
			}
		}

		public override string ToString()
		{
			return DynamicJson.Serialize(this);
		}
	}
}
