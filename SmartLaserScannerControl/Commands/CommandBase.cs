using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands
{
	public abstract class CommandBase : ICommand
	{
		string address_;
		object[] arguments_;

		protected CommandBase()
		{}

		protected CommandBase(
			string address,
			params object[] arguments)
		{
			if (address == null) { throw new ArgumentNullException("address"); }
			address_ = address;
			arguments_ = arguments;
		}

		public CommandData GetData()
		{
			if(address_ == null){ throw new InvalidOperationException("address is null");}
			return new CommandData{
				Address = address_,
				Arguments = arguments_ ?? new object[]{}
			};
		}
	}
}
