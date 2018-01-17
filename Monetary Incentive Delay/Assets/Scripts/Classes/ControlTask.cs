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

		private GameObject _sprite;
		private GameObject _panel;
		public GameObject RewardPanel;
		public GameObject PunishmentPanel;

		private int _shownInfoText;
		private int _iter;
		private List<double> _spamCounter;
		private bool _spacebarPressed;

		[UsedImplicitly]
		private void Start()
		{
			_panel = gameObject;
			_settings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.ControlSettings
				: new TaskSettings(2000, TaskType.Control, 20);
			_threshold = GlobalSettings.Gs != null ? GlobalSettings.Gs.Threshold : 300;
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
					if (_passedTime > _settings.InfoTime)
					{
						ShowSprite();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.DisplayingSprite:
					if (_passedTime > _settings.InfoTime)
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
			if (_shownInfoText + 1 == _info.Count)
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
			bool mainMenu = false;
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
				mainMenu = true;
				performance = "You did nothing!";
			}

			if (DidHeSpam())
			{
				performance += " \nBut you spammed the keyboard";
				mainMenu = true;
			}

			_panel.GetComponentInChildren<Text>().text = performance;
			GlobalSettings.Gs?.UpdateThreshold(_threshold);
			_passedTime = 0;
			_currentDisplayStatus = mainMenu ? DisplayStatus.GoToMainMenu : DisplayStatus.DisplayingInfo;
		}

		private void HandleUserInput()
		{
			if (!_spacebarPressed) return;
			if (_reactionTimes[_iter] < 0)
				_reactionTimes[_iter] = _passedTime;
		}

		private void ShowSprite()
		{
			_iter++;
			var spriteType = _taskType[_iter] == 0
				? _settings.NonIncentiveOrder[_iter / 3]
				: _settings.IncentiveOrder[_iter / 3];
			_sprite = SpriteHandler.Sh.CreateSprite(spriteType, _panel);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus =
				_iter < _settings.NumberOfTasks - 1 ? DisplayStatus.WaitToDisplaySprite : DisplayStatus.DisplayResults;
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
