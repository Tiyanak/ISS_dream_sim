using System;
using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using Handlers;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
	public class Baseline : MonoBehaviour
	{
		private IBaselineSettings _baselineSettings;
		private ISpriteTime _timeSettings;
		private int _infoDisplayTime;
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

		private double[] _reactionTimes;
		private double _threshold = -1;
		private bool _spacebarPressed;

		private GameObject _sprite;
		private GameObject _panelSrt;

		private int _shownInfoText;
		private int _iter;

		[UsedImplicitly]
		private void Start()
		{
			const int numberOfTasks = 10;
			const int infoDisplayTime = 3000;
			
			_panelSrt = gameObject;
			// Try to load global settings, if not then define your own
			_baselineSettings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.BaselineSettings
				: new BaselineSettings(infoDisplayTime, numberOfTasks);
			_timeSettings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.SpriteSettings.GetTimeSettings(SpriteTypes.Baseline)
				: new SpriteTime(new Interval(1000, 2000),new Interval(180, 200),  SpriteTypes.Baseline);
			InitValues();
		}

		private void InitValues()
		{
			_infoDisplayTime = 1000;
			_reactionTimes = new double[_baselineSettings.NumberOfTasks];
			for (var i = 0; i < _baselineSettings.NumberOfTasks; i++)
				_reactionTimes[i] = -1;
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
			_spacebarPressed = AntiSpamming.CheckForSpamming(_currentDisplayStatus);
			_passedTime += Time.deltaTime * 1000;

			switch (_currentDisplayStatus)
			{
				case DisplayStatus.DisplayingInfo:
					if (_passedTime > _infoDisplayTime)
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
					if (_passedTime > _timeSettings.SpriteDelayTime)
					{
						ShowSprite();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.DisplayingSprite:
					if (_passedTime > _timeSettings.SpriteDisplayTime)
						RemoveSprite();
					HandleUserInput();
					break;
				case DisplayStatus.WaitingUserInput:
					HandleUserInput();
					break;
				case DisplayStatus.Nothing:
					break;
				case DisplayStatus.GoToMainMenu:
					if (_passedTime > _infoDisplayTime)
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
				GuiHandler.GoToTests();
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
			_infoDisplayTime = _baselineSettings.InfoDisplayTime;
			double mean = SrtHandler.GetMean(_reactionTimes);
			_threshold = SrtHandler.GetAcceptableReationTime(_reactionTimes);
			string performance = OutputTextHandler.Performance(mean, _baselineSettings.NumberOfTasks);

			_panelSrt.GetComponentInChildren<Text>().text = performance;
			GlobalSettings.Gs?.UpdateThreshold(_threshold);
			_passedTime = 0;
			_currentDisplayStatus = AntiSpamming.DidHeSpam(_baselineSettings.NumberOfTasks) || !(mean < double.MaxValue) 
				? DisplayStatus.GoToMainMenu : DisplayStatus.DisplayingInfo;
		}

		private void HandleUserInput()
		{
			if (_iter < 0 || !_spacebarPressed) return;
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
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus = _iter < _baselineSettings.NumberOfTasks - 1 ? DisplayStatus.WaitToDisplaySprite : DisplayStatus.DisplayResults;
		}
	}
}
