using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExperienceForTest.Calculator.Data;
using ExperienceForTest.Calculator.Models;
using ExperienceForTest.Calculator.Models.BitmapConverters;
using ExperienceForTest.Calculator.Models.Detectors;
using Livet;
using Microsoft.Win32;
using OpenCvSharp;

namespace ExperienceForTest.Calculator.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		public MainWindowViewModel()
		{}

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

					if (frameValue_ != null) {
						Monitor2dVm = new Monitor2DControlViewModel {
							FrameValue = frameValue_
						};
					}

					CalcCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion 

		#region monitor2DVm
		Monitor2DControlViewModel monitor2dVm_ = null;
		public Monitor2DControlViewModel Monitor2dVm
		{
			get
			{
				return monitor2dVm_;
			}
			set
			{
				if (monitor2dVm_ != value) {
					monitor2dVm_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region monitor3dVm
		Monitor3dControlViewModel monitor3dVm_ = null;
		public Monitor3dControlViewModel Monitor3dVm
		{
			get
			{
				return monitor3dVm_;
			}
			set
			{
				if (monitor3dVm_ != value) {
					monitor3dVm_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Status
		string status_ = null;
		public string Status
		{
			get
			{
				return status_;
			}
			set
			{
				if (status_ != value) {
					status_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion // Status

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

					OpenCommand.RaiseCanExecuteChanged();
					CalcCommand.RaiseCanExecuteChanged();
				}
			}
		}
		#endregion

		#region Reconstruct3dVm
		Reconstruct3dControlViewModel reconstruct3dVm_ = null;
		public Reconstruct3dControlViewModel Reconstruct3dVm
		{
			get
			{
				return reconstruct3dVm_;
			}
			set
			{
				if (Reconstruct3dVm != value) {
					reconstruct3dVm_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion
		#endregion // Bindings

		void SetStatus(string status, params object[] args)
		{
			Status = string.Format(status, args);
		}

		#region Commands
		#region OpenCommand
		DelegateCommand openCommand_ = null;
		public DelegateCommand OpenCommand{
			get{
				return openCommand_ ?? (openCommand_ = new DelegateCommand {
					ExecuteHandler = async param => {
						IsProcessing = true;
						try {
							SetStatus("Selecting a file to open...");
							var ofd = new OpenFileDialog();
							ofd.Filter = "All Files|*.*";
							ofd.FilterIndex = 0;
							ofd.RestoreDirectory = false;
							ofd.CheckPathExists = true;
							ofd.CheckFileExists = true;
							if (ofd.ShowDialog() == true) {
								var file = ofd.FileName;
								using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
								using (var reader = new StreamReader(fs, Encoding.UTF8, true, 2014, true)) {

									SetStatus("Parsing file: {0}", file);
									// サイズ取得
									string str = reader.ReadLine();
									str = reader.ReadLine();
									var tokens = str.Split('\t');
									//DataWidth = Convert.ToInt32(tokens[0]);
									//DataHeight = Convert.ToInt32(tokens[1]);
									double actualSize = 4096.0;
									double virtualSize = 600.0;
									var scale = (actualSize / virtualSize);

									// データ取得
									List<SensorValue> values = new List<SensorValue>();
									while ((str = await reader.ReadLineAsync()) != null) {
										if (string.IsNullOrWhiteSpace(str)) { continue; }
										tokens = str.Split('\t');
										values.Add(new SensorValue {
											X = (int)Math.Round(Convert.ToDouble(tokens[0]) * scale),
											Y = (int)Math.Round(Convert.ToDouble(tokens[1]) * scale),
											Value = 0,
										});
									}
									FrameValue = new FrameValue {
										Height = (int)actualSize,
										Width = (int)actualSize,
										Values = values.ToArray()
									};
									SetStatus("Done opening file");
								}
							}
						}
						catch (Exception ex) {
							SetStatus("Error on opening file");
							ErrMsg("Failed to open file", ex);
						}
						finally {
							IsProcessing = false;
						}
					},
					CanExecuteHandler = param => {
						return !IsProcessing;
					}
				});
			}
		}
		#endregion // OpenComman

		#region CalcCommand
		DelegateCommand calcCommand_ = null;
		public DelegateCommand CalcCommand
		{
			get
			{
				return calcCommand_ ?? (calcCommand_ = new DelegateCommand {
					ExecuteHandler = async param => {
						IsProcessing = true;
						try {
							SetStatus("calculating vertices");

							var frame = frameValue_;

							LineInfo[] lines = null;
							var square = await Task.Run(() => {
								var detector = new CvDetector(new SetPixelConverter());
								detector.LineDetected += (_, e) => {
									List<LineInfo> list = new List<LineInfo>();
									foreach (var line in e.Lines) {
										var x1 = line.XPosOfLine(0);
										var x2 = line.XPosOfLine(frame.Height);
										if (x1 == null || x2 == null) { continue; }
										list.Add(new LineInfo {
											X1 = x1.Value,
											X2 = x2.Value,
											Y1 = 0,
											Y2 = frame.Height
										});
									}
									lines = list.ToArray();
								};
								return detector.Detect(frame);
							});
							if (Monitor2dVm != null) {
								Monitor2dVm.Lines = new ObservableCollection<LineInfo>(lines);
								Monitor2dVm.Vertices = new ObservableCollection<CircleInfo>(square.PointArray.Select(x => new CircleInfo { CenterX = x.X, CenterY = x.Y, Radius = 15 }));
							}

							Reconstruct3dVm = new Reconstruct3dControlViewModel {
								CameraParameter = new CameraParameter() {
									FocalLengthX = 5.9957148864529627e+03,
									FocalLengthY = 5.9647451141521778e+03,
									CenterX = 2.8869455045867635e+03,
									CenterY = 2.9005238051336592e+03
								},
								MarkerParameter = new RectangleMarkerParameter {
									Width = 0.22,
									Height = 0.14
								},
								Square = square
							};
							Reconstruct3dVm.Transformed += (_, __) => {
								Monitor3dVm = new Monitor3dControlViewModel {
									Transform = Reconstruct3dVm.Transform
								};
							};

							SetStatus("Done calculating vertices");
							InfoMsg("Done calculating vertices!\n\n" + square.ToString());
						}
						catch (Exception ex) {
							SetStatus("Error on calculating vertices");
							ErrMsg("Failed to calculate vertices", ex);
						}
						finally {
							IsProcessing = false;
						}
					},
					CanExecuteHandler = param => {
						return (!IsProcessing && frameValue_ != null);
					}
				});
			}
		}
		#endregion // CalcCommand
		#endregion // Commands


		void ErrMsg(string msg)
		{
			MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		void ErrMsg(string msg, Exception ex)
		{
			ErrMsg(string.Format("{0}\n\n【Exception】\n{1}", msg, ex.ToString()));
		}

		void InfoMsg(string msg)
		{
			MessageBox.Show(msg, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		void InfoMsg(string msg, Exception ex)
		{
			InfoMsg(string.Format("{0}\n\n【Exception】\n{1}", msg, ex.ToString()));
		}
	}

}
