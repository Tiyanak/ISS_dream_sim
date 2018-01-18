using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using Assets.Scripts.Interfaces;
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

		private int _iSprite;
		private bool _spacebarPressed;
		private bool _allowedSkipping;

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

		public void Start(GameObject panel, GameObject rewardPanel, GameObject punishmentPanel)
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
			SetPanels(panel, rewardPanel, punishmentPanel);
			GetInformation();
			InitValues();
		}

		private void SetPanels(GameObject panel, GameObject rewardPanel, GameObject punishmentPanel)
		{
			_panel = panel;
			_rewardPanel = rewardPanel;
			_punishmentPanel = punishmentPanel;
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
		}

		public void Update()
		{
			_spacebarPressed = AntiSpamming.CheckForSpamming(_currentDisplayStatus);
			CheckSkipping();
			_passedTime += Time.deltaTime * 1000;

			switch (_currentDisplayStatus)
			{
				case DisplayStatus.DisplayingInfo:
					var limit = _currentInfo.DisplayTime != -1 ? _currentInfo.DisplayTime : _taskSettings.InfoTime;
					if (_passedTime > limit)
					{
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
				case DisplayStatus.Nothing:
					break;
				case DisplayStatus.GoToMainMenu:
					if (_passedTime > _taskSettings.InfoTime)
						GuiHandler.GoToMainMenu();
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
			double mean = SrtHandler.GetMean(_reactionTimes);
			string performance = OutputTextHandler.Performance(mean, _taskSettings.NumberOfTasks);
			_panel.GetComponentInChildren<Text>().text = performance;
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
		}

		private void HandleUserInput()
		{
			if (!_spacebarPressed) return;
			if (_iSprite < 1 || !(_reactionTimes[(_iSprite - 1) / 3] < 0)) return;
			SpriteTypes type = SpriteTypes.Correct;
			_reactionTimes[(_iSprite - 1) / 3] = _passedTime;
			if (_passedTime > _threshold)
				type = SpriteTypes.Incorrect;
			if (_taskType[(_iSprite - 1) / 3] == 0)
				_taskSettings.NonIncentiveOrder[2] = type;
			else
				_taskSettings.IncentiveOrder[2] = type;
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
	}
}