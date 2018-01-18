using System;
using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using Handlers;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts.Classes
{
	public class ControlTask : MonoBehaviour
	{
		private ITaskSettings _settings;
		private DisplayStatus _currentDisplayStatus;
		private ISpriteSettings _spriteSettings;
		private float _passedTime;
		private const int ShowSpriteIndex = 8;
		private const int WaitUserSpacebar = 7;

		private readonly List<string> _info = new List<string>
		{
			"You have arrived at control task.",
			"Here you won't gain nor lose any money.",
			"Your reaction time shall be recorded.",
			"There are two different cues representing two different trials.",
			"This box represents incentive cue, i.e. important",
			"This box represents nonincentive cue, i.e. not important",
			"After these cues you may expect a target box to apprear.",
			"Press space to begin",
			"",
			"Great. Now onto the next task."
		};

		private double[] _reactionTimes;
		private int[] _taskType;
		private double _threshold;

		private GameObject _currentSprite;
		private SpriteTypes _currentSpriteType;
		private SpriteTypes _upcomingSpriteType;
		private GameObject _panel;
		public GameObject RewardPanel;
		public GameObject PunishmentPanel;

		private int _shownInfoText;
		private int _iter;
		private bool _spacebarPressed;
		private bool _allowedSkipping;

		[UsedImplicitly]
		private void Start()
		{
			_panel = gameObject;
			_settings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.ControlSettings
				: new TaskSettings(2000, TaskType.Control, 20);
			_spriteSettings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.SpriteSettings
				: new SpriteSettings();
			_threshold = GlobalSettings.Gs != null ? GlobalSettings.Gs.Threshold : 400;
			InitValues();
		}

		private void InitValues()
		{
			_taskType = Randomness.RandomizeField(_settings.NumberOfTasks, _settings.NonIncentivePercentage);
			_reactionTimes = new double[_settings.NumberOfTasks];
			for (var i = 0; i < _settings.NumberOfTasks; i++)
				_reactionTimes[i] = -1;
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_passedTime = 0;
			_shownInfoText = 0;
			_panel.GetComponentInChildren<Text>().text = _info[_shownInfoText];
			_allowedSkipping = true;
			_iter = -1;
		}

		// Update is called once per frame
		[UsedImplicitly]
		private void Update()
		{
			_spacebarPressed = AntiSpamming.CheckForSpamming(_currentDisplayStatus);
			CheckSkipping();
			_passedTime += Time.deltaTime * 1000;

			switch (_currentDisplayStatus)
			{
				case DisplayStatus.DisplayingInfo:
					if (_passedTime > _settings.InfoTime)
					{
						ChangeText();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.DisplayResults:
					DisplayInfo();
					break;
				case DisplayStatus.WaitToDisplaySprite:
					HandleUserInput();
					if (_passedTime > _spriteSettings.GetTimeSettings(_upcomingSpriteType).SpriteDelayTime)
					{
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
					{
						_currentDisplayStatus = DisplayStatus.DisplayingInfo;
						_passedTime = 0;
					}
					break;
				case DisplayStatus.Nothing:
					break;
				case DisplayStatus.GoToMainMenu:
					if (_passedTime > _settings.InfoTime)
					{
						GuiHandler.GoToMainMenu();
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ChangeText()
		{
			if (_shownInfoText + 1 >= _info.Count)
			{
				_panel.SetActive(false);
				var rand = new Random();
				if(rand.NextDouble() < 0.5)
					PunishmentPanel.SetActive(true);
				else
					RewardPanel.SetActive(true);
			}
			else
			{
				_shownInfoText++;
				switch (_shownInfoText)
				{
					case ShowSpriteIndex:
						_upcomingSpriteType = _taskType[0] == 0 ? _settings.NonIncentiveOrder[0] : _settings.IncentiveOrder[0];
						_currentDisplayStatus = DisplayStatus.WaitToDisplaySprite;
						_allowedSkipping = false;
						break;
					case WaitUserSpacebar:
						_currentDisplayStatus = DisplayStatus.WaitingUserInput;
						break;
				}
				_panel.GetComponentInChildren<Text>().text = _info[_shownInfoText];
			}
		}

		private void DisplayInfo()
		{
			_settings.SetInfoTime(3000);
			double mean = SrtHandler.GetMean(_reactionTimes);
			_threshold = SrtHandler.GetAcceptableReationTime(_reactionTimes);
			string performance = OutputTextHandler.Performance(mean, _settings.NumberOfTasks);
			_panel.GetComponentInChildren<Text>().text = performance;
			_passedTime = 0;
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
		}

		private void HandleUserInput()
		{
			if (!_spacebarPressed) return;
			if (_iter < 1 || !(_reactionTimes[(_iter - 1) / 3] < 0)) return;
			SpriteTypes type = SpriteTypes.Correct;
			_reactionTimes[(_iter - 1) / 3] = _passedTime;
			if (_passedTime > _threshold)
				type = SpriteTypes.Incorrect;
			if (_taskType[(_iter - 1) / 3] == 0)
				_settings.NonIncentiveOrder[2] = type;
			else
				_settings.IncentiveOrder[2] = type;
		}

		private void ShowSprite()
		{
			_iter++;
			if(_iter % 3 == 2 && _reactionTimes[_iter/3] < 0)
				if (_taskType[_iter / 3] == 0)
					_settings.NonIncentiveOrder[_iter % 3] = SpriteTypes.Incorrect;
				else
					_settings.IncentiveOrder[_iter % 3] = SpriteTypes.Incorrect;
			
			_currentSpriteType = _taskType[_iter/3] == 0 ? _settings.NonIncentiveOrder[_iter % 3] : _settings.IncentiveOrder[_iter % 3];
			_upcomingSpriteType = _taskType[(_iter + 1)/3] == 0 ? _settings.NonIncentiveOrder[(_iter + 1) % 3] : _settings.IncentiveOrder[(_iter + 1) % 3];

				
			_currentSprite = SpriteHandler.Sh.CreateSprite(_currentSpriteType, _panel);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_currentSprite);
			_currentDisplayStatus =
				_iter < _settings.NumberOfTasks * 3 - 1 ? DisplayStatus.WaitToDisplaySprite : DisplayStatus.DisplayResults;
		}
		
		private void CheckSkipping()
		{
			if (!_spacebarPressed || !_allowedSkipping) return;
			_passedTime = 100000;
		}
	}
}