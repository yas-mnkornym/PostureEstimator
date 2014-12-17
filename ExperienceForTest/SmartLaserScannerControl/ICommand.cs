
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl
{
	public interface ICommand
	{
		CommandData GetData();
	}

}
