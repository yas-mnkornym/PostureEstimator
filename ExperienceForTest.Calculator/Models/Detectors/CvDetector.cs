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
			var lines = GetLines(frame).ToArray();

			// 交点取得 (強度の強い(交点の角度が深い)最初の四つ)
			var points = DetectPoints(lines)
				.OrderByDescending(x => x.Strength)
				.Take(4).ToArray();

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

		PointD[] SortPoints(IEnumerable<PointD> points)
		{
			var firstPoint = points.First();
			var restPoints = points.Skip(1)
				.Select(x => new PointD { X = x.X - firstPoint.X, Y = x.Y - firstPoint.Y })
				.Select(x => new { Point = x, Polar = RectanglarToPolar(x) })
				.OrderBy(x => x.Polar.Theta)
				.Select(x => x.Point);
			return (new PointD[] { firstPoint }).Concat(restPoints).ToArray();
		}

		PolarPoint RectanglarToPolar(PointD point)
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

		PointD[] DetectPoints(PolarPoint[] lines)
		{
			List<PointD> list = new List<PointD>();
			for (int i = 0; i < lines.Length; i++) {
				for (int j = 0; j < lines.Length; j++) {
					if (i == j) { continue; }
					var line1 = lines[i];
					var line2 = lines[j];

					// 傾き計算
					var a1 = -(Math.Cos(line1.Theta) / Math.Sin(line1.Theta));
					var a2 = -(Math.Cos(line2.Theta) / Math.Sin(line2.Theta));

					// 角度計算
					var strength = Math.Abs(a1 - a2);
					if (strength == 0) { continue; }

					// 切片計算
					var b1 = line1.Rho - Math.Sin(line1.Theta);
					var b2 = line2.Rho - Math.Sin(line2.Theta);

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
				}
			}
			return list.ToArray();
		}

		PolarPoint[] GetLines(Data.FrameValue frame)
		{
			List<PolarPoint> list = new List<PolarPoint>();
			using(var img = converter_.Convert(frame))
			using (var storage = new CvMemStorage()) {
				var lines = img.HoughLines2(storage, HoughLinesMethod.Standard, 1, Math.PI / 180.0, 8);
				for (int i = 0; i < lines.Total && i < 4; ++i) {
					var elem = lines.GetSeqElem<CvLineSegmentPolar>(i).Value;
					var line = new PolarPoint {
						Theta = elem.Theta,
						Rho = elem.Rho
					};
					list.Add(line);
				}
			}
			return list.ToArray();
		}

		struct PolarPoint
		{
			public double Theta;
			public double Rho;
		}

		struct PointD
		{
			public double X;
			public double Y;
			public double Strength;

			public Data.Point2D ToPoint()
			{
				return new Data.Point2D{
					X = X,
					Y = Y
				};
			}
		}
	}
}
