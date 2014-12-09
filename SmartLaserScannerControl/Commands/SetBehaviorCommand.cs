using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLaserScannerControl.Commands
{
	public class SetBehaviorCommand : ICommand
	{
		EBehaviorMode mode_ = EBehaviorMode.None;
		int value_ = 0;

		public SetBehaviorCommand(EBehaviorMode mode, int value)
		{
			mode_ = mode;
			value_ = value;
		}

		public string BehaviorToString(EBehaviorMode mode)
		{
			switch (mode) {
				case EBehaviorMode.ElasticFollowing:
					return "elastic_following";

				case EBehaviorMode.ElasticMouth:
					return "elastic_mouth";

				case EBehaviorMode.ElasticMouthSmall:
					return "elastick_mouth_small";

				case EBehaviorMode.SpotBouncing:
					return "spot_bouncing";

				case EBehaviorMode.SpotLorentz:
					return "spot_lorentz";

				case EBehaviorMode.AiHockey:
					return "air_hockey";

				case EBehaviorMode.SpotFollowing:
					return "spot_following";

				case EBehaviorMode.CircularPong:
					return "circular_pong";

				case EBehaviorMode.FishNet:
					return "fish_nat";

				case EBehaviorMode.VerticalPinball:
					return "vertical_pinball";

				case EBehaviorMode.PacMan:
					return "pac_man";

				case EBehaviorMode.SpotTracking:
					return "spot_tracking";

				case EBehaviorMode.SpotTest:
					return "spot_test";

				case EBehaviorMode.Standby:
					return "standby";

				case EBehaviorMode.Resume:
					return "resume";

				default:
					return "";
			}
		}

		public CommandData GetData()
		{
			return new CommandData {
				Address = BehaviorToString(mode_),
				Arguments = new object[]{
					value_
				}
			};
		}
	}

	public enum EBehaviorMode
	{
		None,
		ElasticFollowing,
		ElasticMouth,
		ElasticMouthSmall,
		SpotBouncing,
		SpotLorentz,
		AiHockey,
		RainMode,
		SpotFollowing,
		CircularPong,
		FishNet,
		VerticalPinball,
		PacMan,
		SpotTracking,
		SpotTest,
		Standby,
		Resume
	}
}
