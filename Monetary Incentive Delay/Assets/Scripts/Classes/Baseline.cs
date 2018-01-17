using System;
using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using Handlers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
	public class Baseline : MonoBehaviour
	{
		private  TimeSettings _timeSettings;
		private DisplayStatus _currentDisplayStatus;
		private float _passedTime;
		private const int ShowSpriteIndex = 4;

		private readonly List<string> _info = new List<string>
		{
			"3",
			"2",
			"1",
			"Begin",
			"",
			"And now onto the real thing."
		};

		private int _numberOfTasks;
		private double[] _reactionTimes;
		private double _threshold = -1;

		private GameObject _sprite;
		private GameObject _panelSrt;

		private int _shownInfoText;
		private int _iter;
		private List<double> _spamCounter;

		[UsedImplicitly]
		private void Start()
		{
			_panelSrt = gameObject;
			_timeSettings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.BaselineSettings.Time
				: new TimeSettings(new Interval(1000, 2000), 160, 1000);
			_numberOfTasks = GlobalSettings.Gs != null ? GlobalSettings.Gs.BaselineSettings.NumberOfTasks : 10;
			InitValues();
		}

		private void InitValues()
		{
			_timeSettings.SetInfoDisplay(1000);
			_reactionTimes = new double[_numberOfTasks];
			for (var i = 0; i < _numberOfTasks; i++)
				_reactionTimes[i] = -1;
			_spamCounter = new List<double> {DateTime.Now.Millisecond};
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_passedTime = 0;
			_shownInfoText = 0;
			_panelSrt.GetComponentInChildren<Text>().text = _info[_shownInfoText];
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
					if (_passedTime > _timeSettings.InfoDisplayTimeMilliseconds)
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
					if (_passedTime > _timeSettings.SpriteDelayTimeMilliseconds)
					{
						ShowSprite();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.DisplayingSprite:
					if (_passedTime > _timeSettings.SpriteDisplayTimeMilliseconds)
						RemoveSprite();
					HandleUserInput();
					break;
				case DisplayStatus.WaitingUserInput:
					HandleUserInput();
					break;
				case DisplayStatus.Nothing:
					break;
				case DisplayStatus.GoToMainMenu:
					if (_passedTime > _timeSettings.InfoDisplayTimeMilliseconds)
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
				GuiHandler.NextScene();
			}
			else
			{
				if (++_shownInfoText == ShowSpriteIndex)
				{
					_currentDisplayStatus = DisplayStatus.WaitToDisplaySprite;
				}
				_panelSrt.GetComponentInChildren<Text>().text = _info[_shownInfoText];
			}
		}

		private void DisplayInfo()
		{
			bool mainMenu = false;
			_timeSettings.SetInfoDisplay(3000);
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

			_panelSrt.GetComponentInChildren<Text>().text = performance;
			GlobalSettings.Gs?.UpdateThreshold(_threshold);
			_passedTime = 0;
			_currentDisplayStatus = mainMenu ? DisplayStatus.GoToMainMenu : DisplayStatus.DisplayingInfo;
		}

		private void HandleUserInput()
		{
			if (!Input.GetKeyDown("space")) return;
			if (_reactionTimes[_iter] < 0)
				_reactionTimes[_iter] = _passedTime;
		}

		private void ShowSprite()
		{
			_iter++;
			_sprite = SpriteHandler.Sh.CreateSprite(SpriteTypes.Baseline, _panelSrt);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
#if UNITY_EDITOR
			Debug.Log(_numberOfTasks);
#endif
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus =
				_iter < _numberOfTasks - 1 ? DisplayStatus.WaitToDisplaySprite : DisplayStatus.DisplayResults;
		}

		private void CheckForSpamming()
		{
			if (!Input.GetKeyDown("space")) return;
			_spamCounter.Add(TimeHandler.GetMilliseconds());
		}

		private bool DidHeSpam()
		{
			int littleTime = 0;
			for (int i = 1; i < _spamCounter.Count; i++)
			{			
				if (_spamCounter[i] - _spamCounter[i - 1] < 200)
					littleTime++;
			}
			double percentage = (double) littleTime / _numberOfTasks;
			return _spamCounter.Count > 2 * _numberOfTasks || percentage > 0.3;
		}
	}
}
