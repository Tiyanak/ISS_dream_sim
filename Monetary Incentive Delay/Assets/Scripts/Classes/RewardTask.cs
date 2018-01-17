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
	public class RewardTask : MonoBehaviour
	{
		private ITaskSettings _settings;
		private DisplayStatus _currentDisplayStatus;
		private ISpriteSettings _spriteSettings;
		private float _passedTime;
		private const int ShowSpriteIndex = 8;
		private const int WaitUserSpacebar = 7;

		private readonly List<string> _info = new List<string>
		{
			"You have arrived at reward task.",
			"Here you will GAIN money at incentive tasks.",
			"Your reaction time shall be recorded.",
			"There are two different cues representing two different trials.",
			"This box represents incentive cue, i.e. reward",
			"This box represents nonincentive cue, i.e. no reward",
			"After these cues you may expect a target box to apprear.",
			"Press space to begin",
			"",
			"That's it. You finished the tasks successfully. Congratulations"
		};

		private double[] _reactionTimes;
		private int[] _taskType;
		private double _threshold;

		private GameObject _currentSprite;
		private SpriteTypes _currentSpriteType;
		private GameObject _panel;

		private int _shownInfoText;
		private int _iter;
		private List<double> _spamCounter;
		private bool _spacebarPressed;

		[UsedImplicitly]
		private void Start()
		{
			_panel = gameObject;
			_settings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.RewardSettings
				: new TaskSettings(2000, TaskType.Reward, 20);
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
			_spamCounter = new List<double> {DateTime.Now.Millisecond};
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_passedTime = 0;
			_shownInfoText = 0;
			_panel.GetComponentInChildren<Text>().text = _info[_shownInfoText];
			_iter = -1;
		}

		// Update is called once per frame
		[UsedImplicitly]
		private void Update()
		{
			CheckForSpamming();
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
					if (_passedTime > _spriteSettings.GetTimeSettings(_currentSpriteType).SpriteDelayTime)
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
					GuiHandler.GoToMainMenu();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ChangeText()
		{
			if (_shownInfoText + 1 >= _info.Count)
			{
				_currentDisplayStatus = DisplayStatus.GoToMainMenu;
			}
			else
			{
				_shownInfoText++;
				switch (_shownInfoText)
				{
					case ShowSpriteIndex:
						_currentSpriteType = _taskType[0] == 0 ? _settings.NonIncentiveOrder[0] : _settings.IncentiveOrder[0];
						_currentDisplayStatus = DisplayStatus.WaitToDisplaySprite;
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
#if UNITY_EDITOR
			Debug.Log("mean time: " + mean);
#endif
			_threshold = SrtHandler.GetAcceptableReationTime(_reactionTimes);
			string performance;

			if (mean < 300)
				performance = "You did great!";
			else if (mean < 600)
				performance = "You did O.K.";
			else if (mean < double.MaxValue)
				performance = "You did rather poorly.";
			else
			{
				performance = "You did nothing!";
			}

			if (DidHeSpam())
			{
				performance += " \nBut you spammed the keyboard";
			}

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
			
				
			_currentSprite = SpriteHandler.Sh.CreateSprite(_currentSpriteType, _panel);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_currentSprite);
			_currentDisplayStatus =
				_iter < _settings.NumberOfTasks * 3 - 1 ? DisplayStatus.WaitToDisplaySprite : DisplayStatus.DisplayResults;
		}

		private void CheckIfSpacebarPressed()
		{
			_spacebarPressed = Input.GetKeyDown("space");
		}

		private void CheckForSpamming()
		{
			CheckIfSpacebarPressed();
			if (_spacebarPressed && (_currentDisplayStatus == DisplayStatus.DisplayingSprite ||
			                                  _currentDisplayStatus == DisplayStatus.WaitToDisplaySprite))
			{
				_spamCounter.Add(TimeHandler.GetMilliseconds());
			}
			else if(_spacebarPressed)
				_passedTime = 100000;
		}

		private bool DidHeSpam()
		{
			int littleTime = 0;
			for (int i = 1; i < _spamCounter.Count; i++)
			{			
				if (_spamCounter[i] - _spamCounter[i - 1] < 200)
					littleTime++;
			}
			double percentage = (double) littleTime / _settings.NumberOfTasks;
			return _spamCounter.Count > 2 * _settings.NumberOfTasks || percentage > 0.3;
		}
	}
}
