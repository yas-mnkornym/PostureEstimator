using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace ExperienceForTest.Calculator.Data
{
	public class RectangleMarkerParameter : NotificationObject
	{
		double width_, height_;
		double offsetX_, offsetY_;


		public double Width
		{
			get
			{
				return width_;
			}
			set
			{
				if (width_ != value) {
					width_ = value;
					RaisePropertyChanged();
				}
			}
		}

		public double Height
		{
			get
			{
				return height_;
			}
			set
			{
				if (height_ != value) {
					height_ = value;
					RaisePropertyChanged();
				}
			}
		}

		public double OffsetX
		{
			get
			{
				return offsetX_;
			}
			set
			{
				if (offsetX_ != value) {
					offsetX_ = value;
					RaisePropertyChanged();
				}
			}
		}
		public double OffsetY
		{
			get
			{
				return offsetY_;
			}
			set
			{
				if (offsetY_ != value) {
					offsetY_ = value;
					RaisePropertyChanged();
				}
			}
		}
	}
}
