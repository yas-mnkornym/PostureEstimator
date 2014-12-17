using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperienceForTest.Calculator.Data;

namespace ExperienceForTest.Calculator.Models
{
	interface ITransformCalculator
	{
		Transform Transform(Point2D []points);
	}
}
