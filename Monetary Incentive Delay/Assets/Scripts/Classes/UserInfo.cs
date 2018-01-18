using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using Assets.Scripts.Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
	public class UserInfo : MonoBehaviour
	{
		private ISpriteTime _timeSettings;
		private int _infoDisplayTime;
		private readonly IInformationHolder _informationHolder;
		private DisplayStatus _currentDisplayStatus;
		private InformationNugget _currentInfo;

		private GameObject _panel;
		private GameObject _sprite;
		public GameObject PanelSrt;

		private float _passedTime;
		private double _reactionTime;
		private bool _spacebarPressed;
		private bool _allowedSkipping;
		private bool _waitingForUser;

		public UserInfo()
		{
			_informationHolder = new UserInfoInformation();
		}

		[UsedImplicitly]
		private void Start()
		{
			if (GlobalSettings.Gs == null)
				GuiHandler.GoToMainMenu();
			else
			{
				_timeSettings = GlobalSettings.Gs.SpriteSettings.BaselineSpriteSettings;
				_infoDisplayTime = GlobalSettings.Gs.BaselineSettings.InfoDisplayTime;
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
			_passedTime = 0;
			_waitingForUser = false;
		}

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
					DisplayResults();
					_passedTime = 0;
					break;
				case DisplayStatus.WaitToDisplaySprite:
					limit = _currentInfo.DisplayTime != -1 ? _currentInfo.DisplayTime : _infoDisplayTime;
					if (_passedTime > limit)
					{
						_panel.GetComponentInChildren<Text>().text = "";
						ShowSprite();
						_waitingForUser = true;
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
						HandleUserInput();
					break;
				case DisplayStatus.Nothing:
					break;
				case DisplayStatus.GoToMainMenu:
					if (_passedTime > _infoDisplayTime)
						GuiHandler.GoToMainMenu();
					break;
				case DisplayStatus.GoToNextScene:
					if (_passedTime > _infoDisplayTime)
					{
						_panel.SetActive(false);
						PanelSrt.SetActive(true);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}	
		}

		private void HandleUserInput()
		{
			if (!_spacebarPressed) return;
			_reactionTime = _passedTime;
			if(_sprite != null)
				RemoveSprite();
			_waitingForUser = false;
			_currentDisplayStatus = DisplayStatus.DisplayResults;
		}

		private void DisplayResults()
		{
			_panel.GetComponentInChildren<Text>().text = "Your reaction took " + _reactionTime.ToString("#.0") + " miliseconds.";
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
		}

		private void ShowSprite()
		{
			_sprite = SpriteHandler.Sh.CreateSprite(SpriteTypes.Baseline, _panel);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus = _waitingForUser ? DisplayStatus.WaitingUserInput : DisplayStatus.DisplayingInfo;
		}

		private void CheckSkipping()
		{
			if (!_spacebarPressed || !_allowedSkipping || _waitingForUser) return;
			_passedTime = 100000;
		}
	}
}
