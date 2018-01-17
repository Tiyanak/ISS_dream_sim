using System;
using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Handlers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
	public class Baseline : MonoBehaviour
	{
		private readonly TimeSettings _timeSettings;
		private DisplayStatus _currentDisplayStatus;
		private float _passedTime;
		private readonly int _showSpriteIndex = 4;
		private readonly List<string> _info = new List<string>
		{
			"3",
			"2",
			"1",
			"Begin",
			""
		};
		
		private GameObject _sprite;
		private GameObject _panelSrt;

		private int _shownInfoText;
		private bool _waitForUserInput;

		public Baseline()
		{
			_timeSettings = new TimeSettings(new Interval(500,750), 150, 1000);
		}

		[UsedImplicitly]
		private void Start()
		{
			_panelSrt = gameObject;
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_passedTime = 0;
			_shownInfoText = 0;
			_waitForUserInput = false;
			
			_panelSrt.GetComponentInChildren<Text>().text = _info[_shownInfoText];
		}

		// Update is called once per frame
		[UsedImplicitly]
		private void Update()
		{
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
				case DisplayStatus.WaitToDisplaySprite:
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
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ChangeText()
		{
			if (_shownInfoText + 1 == _info.Count)
			{
				_shownInfoText++;
				GuiHandler.NextScene();
			}
			else
			{
				if (++_shownInfoText == _showSpriteIndex)
				{
					_waitForUserInput = true;
					_currentDisplayStatus = DisplayStatus.WaitToDisplaySprite;
				}
				_panelSrt.GetComponentInChildren<Text>().text = _info[_shownInfoText];
			}
		}

		private void HandleUserInput()
		{
			if (!Input.GetKeyDown("space")) return;
			_panelSrt.GetComponentInChildren<Text>().text = "Your reaction took " + _passedTime + " miliseconds.";
			_currentDisplayStatus = DisplayStatus.DisplayingInfo;
			_waitForUserInput = false;
			_passedTime = 0;
		}

		private void ShowSprite()
		{
			_sprite = SpriteHandler.Sh.CreateSprite(SpriteTypes.Baseline, _panelSrt);
			_currentDisplayStatus = DisplayStatus.DisplayingSprite;
		}

		private void RemoveSprite()
		{
			SpriteHandler.Sh.DestroySprite(_sprite);
			_currentDisplayStatus = _waitForUserInput ? DisplayStatus.WaitingUserInput : DisplayStatus.DisplayingInfo;
		}
	}
}
