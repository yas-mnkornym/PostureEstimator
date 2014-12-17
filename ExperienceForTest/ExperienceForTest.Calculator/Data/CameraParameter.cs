using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperienceForTest.Calculator.Data
{
	public class CameraParameter : Livet.NotificationObject
	{
		double focalLengthX_, focalLengthY_;
		double centerX_, centerY_;

		public double FocalLengthX {
			get{
				return focalLengthX_;
			}
			set
			{
				if (focalLengthX_ != value) {
					focalLengthX_ = value;
					RaisePropertyChanged();
				}
			}
		}


		public double FocalLengthY
		{
			get
			{
				return focalLengthY_;
			}
			set
			{
				if (focalLengthY_ != value) {
					focalLengthY_ = value;
					RaisePropertyChanged();
				}
			}
		}

		public double CenterX
		{
			get
			{
				return centerX_;
			}
			set
			{
				if (centerX_ != value) {
					centerX_ = value;
					RaisePropertyChanged();
				}
			}
		}


		public double CenterY
		{
			get
			{
				return centerY_;
			}
			set
			{
				if (centerY_ != value) {
					centerY_ = value;
					RaisePropertyChanged();
				}
			}
		}
	}
}
