using System.Collections.Generic;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.DataTypes
{
	public class UserInfoInformation : IInformationHolder
	{
		private int _currentIndex = -1;
		public InformationNugget CurrentNugget { get; private set; }

		private readonly List<InformationNugget> _info = new List<InformationNugget>
		{
			new InformationNugget("First we need to determine your speed to make this a bit more challenging for you.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("When the orange box appears, try to hit space as fast as humanly possible.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Let's give it a try.", DisplayStatus.WaitToDisplaySprite, 2000, true),
			new InformationNugget("By the way, there won't be any rewards right now. This is only baseline round.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Prepare yourself for the test round.", DisplayStatus.GoToNextScene, -1, true)
		};

		public void Reset()
		{
			_currentIndex = -1;
		}

		public InformationNugget GetNextInformation()
		{
			_currentIndex = (_currentIndex + 1) % _info.Count;
			CurrentNugget = _info[_currentIndex];
			return CurrentNugget;
		}
	}

	public class BaselineInformation : IInformationHolder
	{
		private int _currentIndex = -1;
		public InformationNugget CurrentNugget { get; private set; }

		private readonly List<InformationNugget> _info = new List<InformationNugget>
		{
			new InformationNugget("3", DisplayStatus.DisplayingInfo, 1000, false),
			new InformationNugget("2", DisplayStatus.DisplayingInfo, 1000, false),
			new InformationNugget("1", DisplayStatus.DisplayingInfo, 1000, false),
			new InformationNugget("Begin", DisplayStatus.WaitToDisplaySprite, 1000, false),
			new InformationNugget("And now onto the real thing.", DisplayStatus.GoToNextScene, -1, false)
		};

		public void Reset()
		{
			_currentIndex = -1;
		}

		public InformationNugget GetNextInformation()
		{
			_currentIndex = (_currentIndex + 1) % _info.Count;
			CurrentNugget = _info[_currentIndex];
			return CurrentNugget;
		}
	}

	public class ControlInformation : IInformationHolder
	{
		private int _currentIndex = -1;
		public InformationNugget CurrentNugget { get; private set; }

		private List<InformationNugget> _info = new List<InformationNugget>
		{
			new InformationNugget("You have arrived at control task.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Here you won't gain nor lose any money.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("There are two different cues representing two different trials.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("This box represents incentive cue, i.e. important", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("This box represents nonincentive cue, i.e. not important", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("After these cues you may expect a target box to apprear.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Your reaction time is measured after the TARGET box appears.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Press space to begin", DisplayStatus.WaitingUserInput, -1, true),
			new InformationNugget("", DisplayStatus.WaitToDisplaySprite, -1, false),
			new InformationNugget("Great. Now onto the next task.", DisplayStatus.GoToNextScene, -1, false)
		};

		public void Reset()
		{
			_currentIndex = -1;
		}

		public InformationNugget GetNextInformation()
		{
			_currentIndex = (_currentIndex + 1) % _info.Count;
			CurrentNugget = _info[_currentIndex];
			return CurrentNugget;
		}
	}

	public class RewardInformation : IInformationHolder
	{
		private int _currentIndex = -1;
		public InformationNugget CurrentNugget { get; private set; }

		private List<InformationNugget> _info = new List<InformationNugget>
		{
			new InformationNugget("You have arrived at reward task.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Here you will GAIN money at incentive tasks!", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("There are two different cues representing two different trials.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("This box represents incentive cue, i.e. important", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("This box represents nonincentive cue, i.e. not important", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("After these cues you may expect a target box to apprear.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Your reaction time is measured after the TARGET box appears.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Press space to begin", DisplayStatus.WaitingUserInput, -1, true),
			new InformationNugget("", DisplayStatus.WaitToDisplaySprite, -1, false),
			new InformationNugget("That's it. You finished the tasks successfully. Congratulations", DisplayStatus.GoToMainMenu, -1, false)
		};

		public void Reset()
		{
			_currentIndex = -1;
		}

		public InformationNugget GetNextInformation()
		{
			_currentIndex = (_currentIndex + 1) % _info.Count;
			CurrentNugget = _info[_currentIndex];
			return CurrentNugget;
		}
	}

	public class PunishmentInformation : IInformationHolder
	{
		private int _currentIndex = -1;
		public InformationNugget CurrentNugget { get; private set; }

		private List<InformationNugget> _info = new List<InformationNugget>
		{
			new InformationNugget("You have arrived at punishment task.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Here you will LOSE money at incentive tasks!", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("There are two different cues representing two different trials.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("This box represents incentive cue, i.e. important", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("This box represents nonincentive cue, i.e. not important", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("After these cues you may expect a target box to apprear.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Your reaction time is measured after the TARGET box appears.", DisplayStatus.DisplayingInfo, -1, true),
			new InformationNugget("Press space to begin", DisplayStatus.WaitingUserInput, -1, true),
			new InformationNugget("", DisplayStatus.WaitToDisplaySprite, -1, false),
			new InformationNugget("That's it. You finished the tasks successfully. Congratulations", DisplayStatus.GoToMainMenu, -1, false)
		};

		public void Reset()
		{
			_currentIndex = -1;
		}

		public InformationNugget GetNextInformation()
		{
			_currentIndex = (_currentIndex + 1) % _info.Count;
			CurrentNugget = _info[_currentIndex];
			return CurrentNugget;
		}
	}
}