using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ExperienceForTest.Calculator.Models.BitmapConverters
{
	public class SetPixelConverter : IBitmapConverter
	{
		public IplImage Convert(Data.FrameValue frame)
		{
			var img = new IplImage(new CvSize(frame.Width, frame.Height), BitDepth.U8, 1);
			img.Set(new CvColor(0x00, 0x00, 0x00));

			foreach (var value in frame.Values) {
				img.Set2D(value.X, value.Y, CvColor.White);
			}
			for (int i = 0; i < 4; i++) {
				img.Dilate(img);
			}
			return img;
		}
	}
}
