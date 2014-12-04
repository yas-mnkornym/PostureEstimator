using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperienceForTest.Calculator.Data;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace ExperienceForTest.Calculator.Models.TransformCalculators
{
	public class CvTransformCaclulator : ITransformCalculator
	{
		Point3f []objectPoints_ = null;
		CameraParameter cameraParameter_ = null;

		public CvTransformCaclulator(
			IEnumerable<Point3f> objectPoints,
			CameraParameter cameraParameter)
		{
			if (objectPoints == null) { throw new ArgumentNullException("objectPoints"); }
			if (cameraParameter == null) { throw new ArgumentNullException("cameraParameter"); }
			objectPoints_ = objectPoints.ToArray();
			cameraParameter_ = cameraParameter;
		}

		public Data.Transform Transform(Data.Point2D[] points)
		{
			if (points == null) { throw new ArgumentNullException("points"); }
			if (points.Length != objectPoints_.Length) {
				throw new ArgumentNullException("points", "引数に与えられた点数がオブジェクト上の点数と異なります。");
			}

			var imagePoints = points.Select(x => new Point2f((float)x.X, (float)x.Y)).ToArray();

			OutputArray rvec = new Mat(), tvec = new Mat();
			Cv2.SolvePnP(
				InputArray.Create(objectPoints_),
				InputArray.Create(imagePoints),
				InputArray.Create(
				new double[,]{
					{cameraParameter_.FocalLengthX, 0, cameraParameter_.CenterX},
					{0, cameraParameter_.FocalLengthY, cameraParameter_.CenterY},
					{0, 0, 1}
				}),
				InputArray.Create(new double[]{0, 0, 0, 0}),
				rvec, tvec);
			var rmat = rvec.GetMat();
				var tmat = tvec.GetMat();
			return new Transform {
				Rotation = new Point3D {
					X = rmat.Get<double>(0),
					Y = rmat.Get<double>(1),
					Z = rmat.Get<double>(2)
				},
				Translation = new Point3D {
					X = tmat.Get<double>(0),
					Y = tmat.Get<double>(1),
					Z = tmat.Get<double>(2)
				}
			};
		}

		public Data.Transform Transform1(Data.Point2D []points)
		{
			if (points == null) { throw new ArgumentNullException("points"); }
			if (points.Length != objectPoints_.Length) {
				throw new ArgumentNullException("points", "引数に与えられた点数がオブジェクト上の点数と異なります。");
			}
			
			var imagePoints = points.Select(x => new Point2f((float)x.X, (float)x.Y)).ToArray();

			double[] rvec, tvec;
			Cv2.SolvePnP(
				objectPoints_,
				imagePoints,
				new double[,]{
					{cameraParameter_.FocalLengthX, 0, cameraParameter_.CenterX},
					{0, cameraParameter_.FocalLengthY, cameraParameter_.CenterY},
					{0, 0, 1}
				},
				null,
				out rvec, out tvec,
				false, SolvePnPFlag.P3P);
			return new Transform {
				Rotation = new Point3D {
					X = rvec[0],
					Y = rvec[1],
					Z = rvec[2]
				},
				Translation = new Point3D {
					X = tvec[0],
					Y = tvec[1],
					Z = tvec[2]
				}
			};
		}
	}
}
