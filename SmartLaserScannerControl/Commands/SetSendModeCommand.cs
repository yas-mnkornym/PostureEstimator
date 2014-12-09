using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands
{
	public class SetSendModeCommand : ICommand
	{
		ESendMode sendMode_ = ESendMode.None;
		bool value_ = false;

		public SetSendModeCommand(ESendMode sendMode, bool value)
		{
			sendMode_ = sendMode;
			value_ = value;
		}

		public CommandData GetData()
		{
			return new CommandData {
				Address = SendModeToString(sendMode_),
				Arguments = new object[]{
					value_ ? 1 : 0
				}
			};
		}

		string SendModeToString(ESendMode sendMode)
		{
			switch (sendMode) {
				case ESendMode.Osc:
					return "sendOSC";

				case ESendMode.Area:
					return "sendArea";

				case ESendMode.Pos:
					return "sendPos";

				case ESendMode.AnchorPos:
					return "sendAnchorPos";

				case ESendMode.Regions:
					return "sendRegions";

				case ESendMode.Touched:
					return "sendTouched";

				default:
					return "";
			}
		}
	}

	public enum ESendMode
	{
		None,
		Osc,
		Area,
		Pos,
		AnchorPos,
		Regions,
		Touched,
	}

}
