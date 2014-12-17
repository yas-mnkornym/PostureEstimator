using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ExperienceForTest.Calculator.ViewModels
{
	public class DelegateCommand : ICommand
	{
		public Action<object> ExecuteHandler { get; set; }
		public Func<object, bool> CanExecuteHandler { get; set; }

		public bool CanExecute(object parameter)
		{
			if (CanExecuteHandler != null) {
				return CanExecuteHandler(parameter);
			}
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			if (ExecuteHandler != null) {
				ExecuteHandler(parameter);
			}
		}

		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null) {
				CanExecuteChanged(this, new EventArgs());
			}
		}
	}

}
