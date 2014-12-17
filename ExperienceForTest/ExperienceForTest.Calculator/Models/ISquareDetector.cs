using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperienceForTest.Calculator.Models
{
	interface ISquareDetector
	{
		Data.Square Detect(Data.FrameValue frame);
	}
}
