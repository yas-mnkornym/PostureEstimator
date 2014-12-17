using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using SharpGL;
using SharpGL.WPF;

namespace ExperienceForTest.Calculator.Views.Behaviors
{
	internal class OpenGLCommandsBehavior : Behavior<OpenGLControl>
	{
		#region Dependency Properties
		#region InitializedCommand
		public static DependencyProperty InitializedCommandProperty = DependencyProperty.Register(
			"InitializedCommand",
			typeof(ICommand),
			typeof(OpenGLCommandsBehavior),
			new PropertyMetadata(null)
			);

		public ICommand InitializedCommand
		{
			get
			{
				return (ICommand)GetValue(InitializedCommandProperty);
			}
			set
			{
				SetValue(InitializedCommandProperty, value);
			}
		}
		#endregion

		#region DrawCommand
		public static DependencyProperty DrawCommandProperty = DependencyProperty.Register(
			"DrawCommand",
			typeof(ICommand),
			typeof(OpenGLCommandsBehavior),
			new PropertyMetadata(null)
			);

		public ICommand DrawCommand
		{
			get
			{
				return (ICommand)GetValue(DrawCommandProperty);
			}
			set
			{
				SetValue(DrawCommandProperty, value);
			}
		}
		#endregion
		#endregion // Dependency Properties

		#region overrides
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.OpenGLDraw += AssociatedObject_OpenGLDraw;
			AssociatedObject.OpenGLInitialized += AssociatedObject_OpenGLInitialized;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.OpenGLDraw -= AssociatedObject_OpenGLDraw;
			AssociatedObject.OpenGLInitialized -= AssociatedObject_OpenGLInitialized;

			base.OnDetaching();
		}
		#endregion

		#region event handlers
		void AssociatedObject_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			var param = new OpenGLCommandParameter(
				args.OpenGL,
				AssociatedObject.ActualWidth,
				AssociatedObject.ActualHeight);

			if (InitializedCommand != null && InitializedCommand.CanExecute(param)) {
				InitializedCommand.Execute(param);
			}
		}

		void AssociatedObject_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
		{
			var param = new OpenGLCommandParameter(
				args.OpenGL,
				AssociatedObject.ActualWidth,
				AssociatedObject.ActualHeight);

			if (DrawCommand != null && DrawCommand.CanExecute(param)) {
				DrawCommand.Execute(param);
			}
		}
		#endregion
	}

	internal class OpenGLCommandParameter
	{
		public OpenGLCommandParameter(
			OpenGL openGl,
			double controlWidth,
			double controlHeight)
		{
			OpenGL = openGl;
			ControlWidth = controlWidth;
			ControlHeight = controlHeight;
		}

		public OpenGL OpenGL { get; private set; }
		public double ControlWidth { get; private set; }
		public double ControlHeight { get; private set; }
	}
}
