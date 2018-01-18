using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using Assets.Scripts.Interfaces;
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
		private readonly IInformationHolder _informationHolder;
		private DisplayStatus _currentDisplayStatus;
		private InformationNugget _currentInfo;

		private float _passedTime;
		private double[] _reactionTimes;
		private double _threshold = -1;
		
		private GameObject _sprite;
		private GameObject _panel;

		private int _iSprite;
		private bool _spacebarPressed;
		private bool _allowedSkipping;

		public Baseline()
		{
			_informationHolder = new BaselineInformation();
		}

		[UsedImplicitly]
		private void Start()
		{
			if (GlobalSettings.Gs == null)
				GuiHandler.GoToMainMenu();
			else
			{
				_baselineSettings = GlobalSettings.Gs.BaselineSettings;
				_threshold = GlobalSettings.Gs.Threshold;
				_timeSettings = GlobalSettings.Gs.SpriteSettings.BaselineSpriteSettings;
				_infoDisplayTime = _baselineSettings.InfoDisplayTime;
			}
			_panel = gameObject;
			GetInformation();
			InitValues();
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
			_reactionTimes = new double[_baselineSettings.NumberOfTasks];
			for (var i = 0; i < _baselineSettings.NumberOfTasks; i++)
				_reactionTimes[i] = -1;
			_iSprite = -1;
			_passedTime = 0;
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
					var limit = _currentInfo.DisplayTime != -1 ? _currentInfo.DisplayTime : _infoDisplayTime;
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
					limit = _currentInfo.DisplayTime != -1 ? _currentInfo.DisplayTime : _infoDisplayTime;
					if (_passedTime > limit)
					{
						DisplayInfo();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.WaitToDisplaySprite:
					HandleUserInput();
					if (_passedTime > _timeSettings.SpriteDelayTime)
					{
						_panel.GetComponentInChildren<Text>().text = "";
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
					if (_spacebarPressed)
						_currentDisplayStatus = DisplayStatus.DisplayingInfo;
					break;
				case DisplayStatus.Nothing:
					break;
				case DisplayStatus.GoToMainMenu:
					if (_passedTime > _infoDisplayTime)
						GuiHandler.GoToMainMenu();
					break;
				case DisplayStatus.GoToNextScene:
					if (_passedTime > _infoDisplayTime)
						GuiHandler.GoToTests();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void DisplayInfo()
		{
			double mean = SrtHandler.GetMean(_reactionTimes);
			_threshold = SrtHandler.GetAcceptableReationTime(_reactionTimes);
			string performance = OutputTextHandler.Performance(mean, _baselineSettings.NumberOfTasks);
			_panel.GetComponentInChildren<Text>().text = performance;
			GlobalSettings.Gs?.UpdateThreshold(_threshold);
			_currentDisplayStatus = AntiSpamming.DidHeSpam(_baselineSettings.NumberOfTasks) || !(mean < double.MaxValue) 
				? DisplayStatus.GoToMainMenu : DisplayStatus.DisplayingInfo;
		}

		private void HandleUserInput()
		{
			if (!_spacebarPressed) return;
			if (_iSprite < 0 || !(_reactionTimes[_iSprite] < 0)) return;
			_reactionTimes[_iSprite] = _passedTime;
		}

		private void ShowSprite()
		{
			_iSprite++;
			_sprite = SpriteHandler.Sh.CreateSprite(SpriteTypes.Baseline, _panel);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus = _iSprite < _baselineSettings.NumberOfTasks - 1 ? DisplayStatus.WaitToDisplaySprite : DisplayStatus.DisplayResults;
		}

		private void CheckSkipping()
		{
			if (!_spacebarPressed || !_allowedSkipping) return;
			_passedTime = 100000;
		}
	}
}
