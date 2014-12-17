using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperienceForTest.Calculator.Data;
using Livet;
using SharpGL;

namespace ExperienceForTest.Calculator.ViewModels
{
	public class Monitor3dControlViewModel : ViewModel
	{
		#region Bindings
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

		#region MarkerParameter
		RectangleMarkerParameter markerParmaeter_ = null;
		public RectangleMarkerParameter MarkerParmaeter
		{
			get
			{
				return markerParmaeter_;
			}
			set
			{
				if (markerParmaeter_ != value) {
					markerParmaeter_ = value;
					RaisePropertyChanged();
				}
			}
		}
		#endregion
		#endregion // Bindings

		#region OpenGL
		void Initialize(OpenGL gl, double windowWidth, double windowHeight)
		{
			gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			gl.Enable(OpenGL.GL_DEPTH_TEST);
			gl.MatrixMode(OpenGL.GL_PROJECTION);
			gl.LoadIdentity();
			gl.Perspective(
				45.0,
				(double)640 / 480,
				0.01,
				10.0); // 単位はm この場合10m
			gl.LookAt(
				0.0, 0.0, 0.0,
				0.0, 0.0, 1.0,
				0.0, 1.0, 0.0);
		}

		#region Draw
		#region fields
		float[][] vertices_ = new float[][]{
			new float[]{-0.5f,-0.5f,0.5f},
			new float[] {0.5f,-0.5f,0.5f},
			new float[] {0.5f,0.5f,0.5f},
			new float[] {-0.5f,0.5f,0.5f},
			new float[] {0.5f,-0.5f,-0.5f},
			new float[] {-0.5f,-0.5f,-0.5f},
			new float[] {-0.5f,0.5f,-0.5f},
			new float[] {0.5f,0.5f,-0.5f}
		};
		#endregion // fields

		void Draw(OpenGL gl, double windowWidth, double windowHeight)
		{
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
			gl.Viewport(0, 0, (int)windowWidth, (int)windowHeight);
			gl.LoadIdentity();

			// 回転とかうんことか
			if (transform_ != null || true) {
				gl.PushMatrix();

				gl.Translate(-Transform.Translation.X, -Transform.Translation.Y, -Transform.Translation.Z);
				gl.Rotate((float)(Transform.Rotation.X * 18.00 / Math.PI),
					(float)(Transform.Rotation.Y * 180.0 / Math.PI),
					(float)(Transform.Rotation.Z * 180.0 / Math.PI));
				gl.Scale(0.22, 0.14, 1); // 実際の大きさ(cm)に変換
				

				gl.Color(1.0, 0.0, 1.0, 1.0);

				// 左回り
				gl.Begin(OpenGL.GL_POLYGON);
				gl.Vertex(-0.5f, -0.5f, 0.0f);
				gl.Vertex(0.5f, -0.5f, 0.0f);
				gl.Vertex(0.5f, 0.5f, 0.0f);
				gl.Vertex(-0.5f, 0.5f, 0.0f);
				gl.End();

				gl.PopMatrix();
			}
		}
		#endregion // Draw
		#endregion // OpenGL

		#region Commands
		#region DrawCommand
		DelegateCommand drawCommand_ = null;
		public DelegateCommand DrawCommand
		{
			get
			{
				if (drawCommand_ == null) {
					drawCommand_ = new DelegateCommand {
						ExecuteHandler = param => {
							var glParam = param as Views.Behaviors.OpenGLCommandParameter;
							if (glParam != null) {
								Draw(glParam.OpenGL, glParam.ControlWidth, glParam.ControlHeight);
							}
						}
					};
				}
				return drawCommand_;
			}
		}
		#endregion

		#region InitializeCommand
		DelegateCommand initializeCommand_ = null;
		public DelegateCommand InitializeCommand
		{
			get
			{
				if (initializeCommand_ == null) {
					initializeCommand_ = new DelegateCommand {
						ExecuteHandler = param => {
							var glParam = param as Views.Behaviors.OpenGLCommandParameter;
							if (glParam != null) {
								Initialize(glParam.OpenGL, glParam.ControlWidth, glParam.ControlHeight);
							}
						}
					};
				}
				return initializeCommand_;
			}
		}
		#endregion
		#endregion // Commands

	}
}
