using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperienceForTest.Calculator.Data;
using Livet;

namespace ExperienceForTest.Calculator.ViewModels
{
	public class Monitor2DControlViewModel : ViewModel
	{
		#region Bindings
		#region FramgeValue
		FrameValue frameValue_ = null;
		public FrameValue FrameValue
		{
			get
			{
				return frameValue_;
			}
			set
			{
				if (frameValue_ != value) {
					frameValue_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion 


		#region Lines
		ObservableCollection<LineInfo> lines_ = new ObservableCollection<LineInfo>();
		public ObservableCollection<LineInfo> Lines
		{
			get
			{
				return lines_;
			}
			set
			{
				if (lines_ != value) {
					lines_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Vertices
		ObservableCollection<CircleInfo> vertices_ = new ObservableCollection<CircleInfo>();
		public ObservableCollection<CircleInfo> Vertices
		{
			get
			{
				return vertices_;
			}
			set
			{
				if (vertices_ != value) {
					vertices_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion
		#endregion // Bindings
	}

	public class LineInfo
	{
		public double X1 { get; set; }
		public double X2 { get; set; }
		public double Y1 { get; set; }
		public double Y2 { get; set; }
	}

	public class CircleInfo
	{
		public double CenterX { get; set; }
		public double CenterY { get; set; }
		public double Radius { get; set; }

		public double Left
		{
			get
			{
				return CenterX - Radius;
			}
		}

		public double Top
		{
			get
			{
				return CenterY - Radius;
			}
		}

		public double Width
		{
			get
			{
				return Radius * 2;
			}
		}

		public double Height
		{
			get
			{
				return Radius * 2;
			}
		}
	}
}
