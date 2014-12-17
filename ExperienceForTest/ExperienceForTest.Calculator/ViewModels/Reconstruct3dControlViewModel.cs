using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperienceForTest.Calculator.Data;
using ExperienceForTest.Calculator.Models.TransformCalculators;
using Livet;
using OpenCvSharp.CPlusPlus;

namespace ExperienceForTest.Calculator.ViewModels
{
	public class Reconstruct3dControlViewModel : ViewModel
	{
		#region Bindings
		#region CameraParameter
		CameraParameter camerParameter_ = null;
		public CameraParameter CameraParameter
		{
			get
			{
				return camerParameter_;
			}
			set
			{
				if (camerParameter_ != value) {
					camerParameter_ = value;
					RaisePropertyChanged();

					ReconstructCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion 
		#region MarkerParameter
		RectangleMarkerParameter markerParameter_ = null;
		public RectangleMarkerParameter MarkerParameter
		{
			get
			{
				return markerParameter_;
			}
			set
			{
				if (markerParameter_ != value) {
					markerParameter_ = value;
					RaisePropertyChanged();

					ReconstructCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion

		#region Square
		Square square_;
		public Square Square
		{
			get
			{
				return square_;
			}
			set
			{
				square_ = value;
				RaisePropertyChanged();

				ReconstructCommand.RaiseCanExecuteChanged();
			}
		}
		#endregion

		#region Transform
		Transform transform_ = null;
		public Transform Transform
		{
			get
			{
				return transform_;
			}
			set
			{
				if (transform_ != value) {
					transform_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region IsProcessing
		bool isProcessing_ = false;
		public bool IsProcessing
		{
			get
			{
				return isProcessing_;
			}
			set
			{
				if (isProcessing_ != value) {
					isProcessing_ = value;
					RaisePropertyChanged();

					ReconstructCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion
		#endregion // Bindings

		#region Commands
		DelegateCommand reconstructCommand_ = null;
		public DelegateCommand ReconstructCommand
		{
			get
			{
				return reconstructCommand_ ?? (reconstructCommand_ = new DelegateCommand {
					ExecuteHandler = async param => {
						Transform = await Task.Run(() => {
							var tc = new CvTransformCaclulator(
								new Point3f[]{
								new Point3f(0, 0, 0),
								new Point3f((float)MarkerParameter.Width, 0, 0),
								new Point3f((float)MarkerParameter.Width, (float)MarkerParameter.Height, 0),
								new Point3f(0, (float)MarkerParameter.Height, 0)
							},
								CameraParameter);
							return tc.Transform(Square.PointArray);
						});
						if (Transformed != null) {
							Transformed(this, new EventArgs());
						}
					},
					CanExecuteHandler = param => {
						return (
							!IsProcessing &&
							CameraParameter != null &&
							MarkerParameter != null);
					}
				});
			}
		}
		#endregion // Commands

		#region events
		public event EventHandler Transformed;
		#endregion // events
	}
}
