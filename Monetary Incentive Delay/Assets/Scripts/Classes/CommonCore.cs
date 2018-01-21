using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Handlers;
using Assets.Scripts.Handlers.GUI;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
	public class CommonCore
	{
		private readonly TaskType _myType;

		private ITaskSettings _taskSettings;
		private ISpriteSettings _spriteSettings;
		private readonly IInformationHolder _informationHolder;
		private DisplayStatus _currentDisplayStatus;
		private InformationNugget _currentInfo;

		private float _passedTime;
		private double[] _reactionTimes;
		private int[] _taskType;
		private double _threshold;

		private GameObject _currentSprite;
		private SpriteTypes _currentSpriteType;
		private SpriteTypes _upcomingSpriteType;
		private GameObject _panel;
		private GameObject _rewardPanel;
		private GameObject _punishmentPanel;
		private GameObject _spammingText; 

		private int _iSprite;
		private bool _spacebarPressed;
		private bool _allowedSkipping;
		private int _numberOfSpamming;
		private static double _timeOfSpamming;

		public CommonCore(TaskType myType)
		{
			_myType = myType;
			switch (myType)
			{
				case TaskType.Control:
					_informationHolder = new ControlInformation();
					break;
				case TaskType.Reward:
					_informationHolder = new RewardInformation();
					break;
				case TaskType.Punishment:
					_informationHolder = new PunishmentInformation();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(myType), myType, null);
			}
		}

		public void Start(GameObject panel, GameObject rewardPanel, GameObject punishmentPanel, GameObject spammingText)
		{
			if (GlobalSettings.Gs == null)
				GuiHandler.GoToMainMenu();
			else
			{
				switch (_myType)
				{
					case TaskType.Control:
						_taskSettings = GlobalSettings.Gs.ControlSettings;
						break;
					case TaskType.Reward:
						_taskSettings = GlobalSettings.Gs.RewardSettings;
						break;
					case TaskType.Punishment:
						_taskSettings = GlobalSettings.Gs.PunishmentSettings;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				_spriteSettings = GlobalSettings.Gs.SpriteSettings;
				_threshold = GlobalSettings.Gs.Threshold;
			}
			SetPanels(panel, rewardPanel, punishmentPanel, spammingText);
			GetInformation();
			InitValues();
		}

		private void SetPanels(GameObject panel, GameObject rewardPanel, GameObject punishmentPanel, GameObject spammingText)
		{
			_panel = panel;
			_rewardPanel = rewardPanel;
			_punishmentPanel = punishmentPanel;
			_spammingText = spammingText;
		}

		private void GetInformation()
		{
			_informationHolder.Reset();
			_currentInfo = _informationHolder.GetNextInformation();
			_currentDisplayStatus = _currentInfo.NextDisplayStatus;
			_panel.GetComponentInChildren<Text>().text = _currentInfo.InfoText;
			_allowedSkipping = _currentInfo.Skippable;
		}

		private void InitValues()
		{
			_taskType = Randomness.RandomizeField(_taskSettings.NumberOfTasks, _taskSettings.NonIncentivePercentage);
			_upcomingSpriteType = _taskType[0] == 0 ? _taskSettings.NonIncentiveOrder[0] : _taskSettings.IncentiveOrder[0];
			_reactionTimes = new double[_taskSettings.NumberOfTasks];
			for (var i = 0; i < _taskSettings.NumberOfTasks; i++)
				_reactionTimes[i] = -1;
			_iSprite = -1;
			_passedTime = 0;
			_numberOfSpamming = 0;
		}

		public void Update()
		{
			_spacebarPressed = AntiSpamming.CheckForSpamming(_currentDisplayStatus, _spacebarPressed);
			CheckSkipping();
			_passedTime += Time.deltaTime * 1000;

			double threshold = UnityClient.Communicator.HandleServerParams();
			if (Math.Abs(threshold) > float.Epsilon) {
				_threshold = threshold;
			}

			switch (_currentDisplayStatus)
			{
				case DisplayStatus.DisplayingInfo:
					var limit = _currentInfo.DisplayTime != -1 ? _currentInfo.DisplayTime : _taskSettings.InfoTime;
					if (_passedTime > limit)
					{
						if(_currentSprite != null)
							SpriteHandler.Sh.DestroySprite(_currentSprite);
						_currentInfo = _informationHolder.GetNextInformation();
						_currentDisplayStatus = _currentInfo.NextDisplayStatus;
						_panel.GetComponentInChildren<Text>().text = _currentInfo.InfoText;
						_allowedSkipping = _currentInfo.Skippable;
						_passedTime = 0;
					}
					break;
				case DisplayStatus.DisplayResults:
					HandleUserInput();
					if (_passedTime > _spriteSettings.GetTimeSettings(_currentSpriteType).SpriteDelayTime)
					{
						DisplayInfo();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.WaitToDisplaySprite:
					HandleUserInput();
					if (_passedTime > _spriteSettings.GetTimeSettings(_upcomingSpriteType).SpriteDelayTime)
					{
						_panel.GetComponentInChildren<Text>().text = "";
						ShowSprite();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.DisplayingSprite:
					if (_passedTime > _spriteSettings.GetTimeSettings(_currentSpriteType).SpriteDisplayTime)
						RemoveSprite();
					HandleUserInput();
					break;
				case DisplayStatus.WaitingUserInput:
					if (_spacebarPressed)
						_currentDisplayStatus = DisplayStatus.DisplayingInfo;
					break;
				case DisplayStatus.DisplayBoth:
					limit = _currentInfo.DisplayTime != -1 ? _currentInfo.DisplayTime : _taskSettings.InfoTime;
					if (_passedTime > limit)
					{
						if (_currentSprite != null)
							SpriteHandler.Sh.DestroySprite(_currentSprite);
						_currentInfo = _informationHolder.GetNextInformation();
						_currentSprite = SpriteHandler.Sh.CreateSprite(_currentInfo.Type, _panel, Position.Above);
						_currentDisplayStatus = _currentInfo.NextDisplayStatus;
						_panel.GetComponentInChildren<Text>().text = _currentInfo.InfoText;
						_allowedSkipping = _currentInfo.Skippable;
						_passedTime = 0;
					}
					break;
				case DisplayStatus.GoToMainMenu:
					if (_passedTime > _taskSettings.InfoTime){
						UnityClient.Communicator.Disconnect();			
						GuiHandler.GoToMainMenu();
					}
					break;
				case DisplayStatus.GoToNextScene:
					limit = _currentInfo.DisplayTime != -1 ? _currentInfo.DisplayTime : _taskSettings.InfoTime;
					if (_passedTime > limit)
					{
						if (Randomness.Rand.NextDouble() < 0.5)
							_punishmentPanel.SetActive(true);
						else
							_rewardPanel.SetActive(true);
						_panel.SetActive(false);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void DisplayInfo()
		{
			string performance = OutputTextHandler.HowManyValid(_reactionTimes, _taskType, _threshold);
			_panel.GetComponentInChildren<Text>().text = performance;
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;			
		}

		private void HandleUserInput()
		{
			RemoveNotification();
			bool isIncentive = _taskType[(_iSprite - 1) / 3] != 0;
			if (!_spacebarPressed || _iSprite < 1) return;
			CheckSpamming();
			if (_iSprite < 1 || !(_reactionTimes[(_iSprite - 1) / 3] < 0)) return;
			
			SpriteTypes type = SpriteTypes.Correct;
			_reactionTimes[(_iSprite - 1) / 3] = _passedTime;
			
			if (_passedTime > _threshold) type = SpriteTypes.Incorrect;
			
			if (!isIncentive) {
				_taskSettings.NonIncentiveOrder[2] = type;
				SendFeedback(_passedTime, false);
			} else {
				_taskSettings.IncentiveOrder[2] = type;
				SendFeedback(_passedTime, true);
			}
		}

		private void ShowSprite()
		{
			_iSprite++;
			if (_iSprite % 3 == 2 && _reactionTimes[_iSprite / 3] < 0)
				if (_taskType[_iSprite / 3] == 0)
					_taskSettings.NonIncentiveOrder[_iSprite % 3] = SpriteTypes.Incorrect;
				else
					_taskSettings.IncentiveOrder[_iSprite % 3] = SpriteTypes.Incorrect;

			_currentSpriteType = _taskType[_iSprite / 3] == 0 ? _taskSettings.NonIncentiveOrder[_iSprite % 3] : _taskSettings.IncentiveOrder[_iSprite % 3];
			if (_iSprite < _taskSettings.NumberOfTasks * 3 - 1)
				_upcomingSpriteType = _taskType[(_iSprite + 1) / 3] == 0 ? _taskSettings.NonIncentiveOrder[(_iSprite + 1) % 3] : _taskSettings.IncentiveOrder[(_iSprite + 1) % 3];

			_currentSprite = SpriteHandler.Sh.CreateSprite(_currentSpriteType, _panel);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_currentSprite);
			_currentDisplayStatus = _iSprite < _taskSettings.NumberOfTasks * 3 - 1 ? DisplayStatus.WaitToDisplaySprite : DisplayStatus.DisplayResults;
		}

		private void CheckSkipping()
		{
			if (!_spacebarPressed || !_allowedSkipping) return;
			_passedTime = 100000;
		}

        private void SendFeedback(double reactionTime, bool incentive)
        {
            UnityClient.Communicator.SendFeedback(_myType, incentive, reactionTime, _threshold);
        }

		private void CheckSpamming()
		{
			bool spamming = AntiSpamming.SoftCheck((_iSprite - 1) / 3);
			if (spamming && _spammingText.gameObject.activeInHierarchy == false)
				ShowNotification();
		}

		private void ShowNotification()
		{
			if (++_numberOfSpamming >= 3)
				_currentDisplayStatus = DisplayStatus.GoToMainMenu;
			else
			{
				_timeOfSpamming = TimeHandler.GetMilliseconds();
				_spammingText.gameObject.SetActive(true);
			}
		}

		private void RemoveNotification()
		{
			if(_spammingText.gameObject.activeInHierarchy && TimeHandler.GetMilliseconds() - _timeOfSpamming >= _taskSettings.InfoTime)
				_spammingText.gameObject.SetActive(false);
		}
	}
}