using System;
using System.Collections.Generic;
using DataTypes;
using UnityEngine;
using UnityEngine.UI;

namespace Classes
{
	public class UserInfo : MonoBehaviour
	{
		private readonly TimeSettings _timeSettings;
		private DisplayStatus _currentDisplayStatus;
		private float _passedTime;
		private readonly int _showSpriteIndex = 3;
		private readonly List<string> _info = new List<string>
		{
			"First we need to determine your speed to make this a bit more challenging for you.",
			"When the orange box appears, try to hit space as fast as humanly possible.",
			"Let's give it a try.",
			"",
			"By the way, there won't be any rewards right now. This is only baseline round."
		};

		public GameObject PanelInformation;
		private GameObject _sprite;
		public GameObject PanelSrt;
	
		private int _shownInfoText;
		private bool _waitForUserInput;

		public UserInfo()
		{
			_timeSettings = new TimeSettings(500, 1000, 2000);
		}
	
		private void Start()
		{
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_passedTime = 0;
			_shownInfoText = 0;
			_waitForUserInput = false;
			PanelInformation.GetComponentInChildren<Text>().text = _info[_shownInfoText];
		}

		// Update is called once per frame
		private void Update() 
		{
			_passedTime += Time.deltaTime * 1000;
		
			switch (_currentDisplayStatus)
			{
				case DisplayStatus.DisplayingInfo:
					if (_passedTime > _timeSettings.InfoDisplayTimeMiliseconds)
					{
						ChangeText();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.WaitToDisplaySprite:
					if (_passedTime > _timeSettings.SpriteDelayTimeMiliseconds)
					{
						ShowSprite();
						_passedTime = 0;
					}
					break;
				case DisplayStatus.DisplayingSprite:
					if (_passedTime > _timeSettings.SpriteDisplayTimeMiliseconds)
						RemoveSprite();
					HandleUserInput();
					break;
				case DisplayStatus.WaitingUserInput:
					HandleUserInput();
					break;
				case DisplayStatus.Nothing:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}	
		}

		private void ChangeText()
		{
			if (_shownInfoText + 1 == _info.Count)
			{
				_shownInfoText++;
				PanelInformation.SetActive(false);
				PanelSrt.SetActive(true);
			}
			else
			{
				if (++_shownInfoText == _showSpriteIndex)
				{
					_waitForUserInput = true;
					_currentDisplayStatus = DisplayStatus.WaitingUserInput;
				}
				PanelInformation.GetComponentInChildren<Text>().text = _info[_shownInfoText];
			}	
		}

		private void HandleUserInput()
		{
			if (!Input.GetKeyDown("space")) return;
			PanelInformation.GetComponentInChildren<Text>().text = "Your reaction took " + _passedTime + " miliseconds.";
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_waitForUserInput = false;
			_passedTime = 0;
		}

		private void ShowSprite()
		{
			_sprite = SpriteHandler.Sh.CreateSprite(SpriteTypes.Baseline, PanelInformation);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus = _waitForUserInput ? DisplayStatus.DisplayingInfo : DisplayStatus.WaitingUserInput;
		}
	}
}
