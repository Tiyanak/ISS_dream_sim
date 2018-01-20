using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Handlers.GUI.Settings
{
	public class IntervalWaitButton : MonoBehaviour
	{
		public SpriteTypes SpriteType;
		public IntervalType IntervalType;
		private ISpriteTime _spriteTime;
		private int _newValue;

		public void Start()
		{
			_spriteTime = GlobalSettings.Gs.SpriteSettings.GetTimeSettings(SpriteType);
			switch (IntervalType)
			{
				case IntervalType.MinValue:
					_newValue = _spriteTime.WaitInterval.MinTime;
					break;
				case IntervalType.MaxValue:
					_newValue = _spriteTime.WaitInterval.MaxTime;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			ChangeInputFieldText(_newValue.ToString());
		}

		public void OnValueChanged()
		{
			string text = gameObject.GetComponentInChildren<Text>().text;
			int.TryParse(text, out _newValue);
			switch (IntervalType)
			{
				case IntervalType.MinValue:
					SettingsHandler.SetSpriteField(SpriteType, SettingsType.WaitIntervalMin, _newValue);
					break;
				case IntervalType.MaxValue:
					SettingsHandler.SetSpriteField(SpriteType, SettingsType.WaitIntervalMax, _newValue);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ChangeInputFieldText(string message)
		{
			if (gameObject != null)
			{
				InputField textscript = gameObject.GetComponentInChildren<InputField>(); // This will get the script responsable for editing text
				if (textscript == null) { Debug.LogError("Script not found"); return; }
				textscript.text = message; // This will change the text inside it
			}
		}
	}
}