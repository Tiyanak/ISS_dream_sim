using System;
using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
	public class UserInfo : MonoBehaviour
	{
		private readonly int _infoTimeSetting;
		private ISpriteTime _timeSettings;
		private DisplayStatus _currentDisplayStatus;
		private float _passedTime;
		private const int ShowSpriteIndex = 3;
		private readonly List<string> _info = new List<string>
		{
			"First we need to determine your speed to make this a bit more challenging for you.",
			"When the orange box appears, try to hit space as fast as humanly possible.",
			"Let's give it a try.",
			"",
			"By the way, there won't be any rewards right now. This is only baseline round.",
			"Prepare yourself for the test round."
		};

		private GameObject _panelInformation;
		private GameObject _sprite;
		public GameObject PanelSrt;
	
		private int _shownInfoText;
		private bool _waitForUserInput;
		private double _reactionTime;

		public UserInfo()
		{
			_infoTimeSetting = 4000;
		}

		[UsedImplicitly]
		private void Start()
		{
			_panelInformation = gameObject;
			_timeSettings = GlobalSettings.Gs != null
				? GlobalSettings.Gs.SpriteSettings.GetTimeSettings(SpriteTypes.Baseline)
				: new SpriteTime(new Interval(600, 1000), new Interval(200, 210), SpriteTypes.Baseline);
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_passedTime = 0;
			_shownInfoText = 0;
			_waitForUserInput = false;
			_panelInformation.GetComponentInChildren<Text>().text = _info[_shownInfoText];
		}

		[UsedImplicitly]
		private void Update() 
		{
			CheckKeyboard();
			_passedTime += Time.deltaTime * 1000;
		
			switch (_currentDisplayStatus)
			{
				case DisplayStatus.DisplayingInfo:
					if (_passedTime > _infoTimeSetting)
					{
						ChangeText();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.WaitToDisplaySprite:
					if (_passedTime > _timeSettings.SpriteDisplayTime)
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
				case DisplayStatus.DisplayResults:
					DisplayResults();
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
				_panelInformation.SetActive(false);
				PanelSrt.SetActive(true);
			}
			else
			{
				if (++_shownInfoText == ShowSpriteIndex)
				{
					_waitForUserInput = true;
					_currentDisplayStatus = DisplayStatus.WaitToDisplaySprite;
				}
				_panelInformation.GetComponentInChildren<Text>().text = _info[_shownInfoText];
			}	
		}

		private void HandleUserInput()
		{
			if (!Input.GetKeyDown("space")) return;
			_reactionTime = _passedTime;
			_currentDisplayStatus = DisplayStatus.DisplayResults;
			_waitForUserInput = false;
			_passedTime = 0;
		}

		private void DisplayResults()
		{
			_panelInformation.GetComponentInChildren<Text>().text = "Your reaction took " + _reactionTime.ToString("#.0") + " miliseconds.";
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
		}

		private void ShowSprite()
		{
			_sprite = SpriteHandler.Sh.CreateSprite(SpriteTypes.Baseline, _panelInformation);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus = _waitForUserInput ? DisplayStatus.WaitingUserInput : DisplayStatus.DisplayingInfo;
		}

		private void CheckKeyboard()
		{
			if (!Input.GetKeyDown("space") || _waitForUserInput) return;
			_passedTime = 100000;
		}
	}
}
