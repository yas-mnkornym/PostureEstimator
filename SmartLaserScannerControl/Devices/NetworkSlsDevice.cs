using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventuz.OSC;

namespace SmartLaserScannerControl.Devices
{
	public class NetworkSlsDevice : ISlsDevice
	{
		NetWriter writer_ = null;

		public NetworkSlsDevice(NetWriter writer)
		{
			if (writer == null) { new ArgumentNullException("writer"); }
			writer_ = writer;
		}

		public void SendCommand(ICommand command)
		{
			var data = command.GetData();
			writer_.Send(new OscElement(data.Address, data.Arguments));
		}
	}
}
