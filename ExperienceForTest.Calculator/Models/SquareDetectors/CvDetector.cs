using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ExperienceForTest.Calculator.Models.Detectors
{
	internal class CvDetector : ISquareDetector
	{
		IBitmapConverter converter_ = null;

		public CvDetector(IBitmapConverter converter)
		{
			if (converter == null) { throw new ArgumentNullException("converter"); }
			converter_ = converter;
		}

		public Data.Square Detect(Data.FrameValue frame)
		{
			// 直線取得
			var lines = GetLines(frame);
			if(LineDetected != null){
				LineDetected(this, new LinesDetectedEventArgs(lines));
			}
			lines = lines.Take(4).ToArray();
			// 交点取得 (強度の強い(交点の角度が深い)最初の四つ)
			var points = DetectPoints(lines);

			// 交点をソート
			var sortedPpoints = SortPoints(points).ToArray();
			if (sortedPpoints.Length != 4) { throw new Exception("交点座標のソート結果が4点ありません。"); }

			return new Data.Square{
				P1 = sortedPpoints[0].ToPoint(),
				P2 = sortedPpoints[1].ToPoint(),
				P3 = sortedPpoints[2].ToPoint(),
				P4 = sortedPpoints[3].ToPoint()
			};
		}

		CvPoint[] SortPoints(IEnumerable<CvPoint> points)
		{
			var firstPoint = points.First();
			var restPoints = points.Skip(1)
				.Select(x => new { Original = x, Diff = new CvPoint { X = x.X - firstPoint.X, Y = x.Y - firstPoint.Y } })
				.Select(x => new { Original = x.Original, Diff = x.Diff, Polar = RectanglarToPolar(x.Diff) })
				.OrderBy(x => x.Polar.Theta)
				.Select(x => x.Original);
			return (new CvPoint[] { firstPoint }).Concat(restPoints).ToArray();
		}

		PolarPoint RectanglarToPolar(CvPoint point)
		{
			if (point.X != 0) {
				return new PolarPoint {
					Rho = Math.Sqrt(point.X * point.X + point.Y * point.Y),
					Theta = Math.Atan(point.Y / point.X)
				};
			}
			else {
				var rho = Math.Sqrt(point.X * point.X + point.Y * point.Y);
				return new PolarPoint {
					Rho = rho,
					Theta = Math.Acos(0)
				};
			}
		}

		CvPoint[] DetectPoints(CvLineSegmentPolar[] lines)
		{
			List<Tuple<CvPoint, double>> list = new List<Tuple<CvPoint, double>>();
			for (int i = 0; i < lines.Length; i++) {
				for (int j = i+1; j < lines.Length; j++) {
					if (i == j) { continue; }
					var line1 = lines[i];
					var line2 = lines[j];

					var point = line1.LineIntersection(line2);
					if (point == null) { continue; }

					// 傾き計算
					var a1 = -(Math.Cos(line1.Theta) / Math.Sin(line1.Theta));
					var a2 = -(Math.Cos(line2.Theta) / Math.Sin(line2.Theta));

					// 角度計算
					var strength = Math.Abs(a1 - a2);
					list.Add(new Tuple<CvPoint, double>(point.Value, strength));

					/*

					// 切片計算
					var b1 = line1.Rho / Math.Sin(line1.Theta);
					var b2 = line2.Rho / Math.Sin(line2.Theta);

					// 交点x座標計算
					var x = (b2 - b1) / (a1 - a2);

					// 交点y座標計算
					var y = a1 * x + b1;

					// 返す
					var p = new PointD {
						X = x,
						Y = y,
						Strength = strength
					};
					list.Add(p);
					 * */
				}
			}
			return list.OrderByDescending(x => x.Item2).Take(4).Select(x => x.Item1).ToArray();
		}

		CvLineSegmentPolar[] GetLines(Data.FrameValue frame)
		{
			double thetaThres = 30.0 * Math.PI / 180.0;
			//var rad = Math.PI / 2;
			double rhoThres = 100.0;

			List<CvLineSegmentPolar> list = new List<CvLineSegmentPolar>();
			using(var img = converter_.Convert(frame))
			using (var storage = new CvMemStorage()) {
				var lines = img.HoughLines2(storage, HoughLinesMethod.Standard, 1, Math.PI / 180.0, 10);
				for (int i = 0, count = 0; i < lines.Total && count < 4; ++i) {
					var elem = lines.GetSeqElem<CvLineSegmentPolar>(i).Value;
					var theta = elem.Theta;
					if (theta > Math.PI / 2) {
						theta -= (float)Math.PI;
					}

					var thetaCount = list.Count(x => Math.Abs(x.Theta - elem.Theta) < thetaThres);
					if (thetaCount == 1) {
						if (list.Any(x => Math.Abs(x.Rho - elem.Rho) < rhoThres)) {
							continue;
						}
					}
					else if(thetaCount >= 2){
						continue;
					}

					//	continue;
					//	//if (list.Any(x => Math.Abs(x.Theta - elem.Theta) < thetaThres)) { continue; }
					//}
					list.Add(elem);
					count++;
				}
			}
			return list.ToArray();
		}

		struct PolarPoint
		{
			public double Theta;
			public double Rho;
		}

		#region events
		public event EventHandler<LinesDetectedEventArgs> LineDetected;
		#endregion
	}

	public class LinesDetectedEventArgs : EventArgs
	{
		public LinesDetectedEventArgs(CvLineSegmentPolar []lines)
		{
			Lines = lines;
		}

		public CvLineSegmentPolar[] Lines { get; private set; }
	}

	static class CvPointExt
	{
		public static Data.Point2D ToPoint(this CvPoint point)
		{
			return new Data.Point2D {
				X = point.X,
				Y = point.Y
			};
		}
	}
}
