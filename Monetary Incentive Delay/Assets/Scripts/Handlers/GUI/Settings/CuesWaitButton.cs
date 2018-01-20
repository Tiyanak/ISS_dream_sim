using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Handlers.GUI.Settings
{
	public class CuesWaitButton : MonoBehaviour
	{
		public IntervalType IntervalType;
		private ISpriteTime _spriteTime;
		private int _newValue;

		public void Start()
		{
			_spriteTime = GlobalSettings.Gs.SpriteSettings.GetTimeSettings(SpriteTypes.ControlCue);
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
					SettingsHandler.SetSpriteField(SpriteTypes.ControlCue, SettingsType.WaitIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.ControlNonIncentive, SettingsType.WaitIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardCue, SettingsType.WaitIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardNonIncentive, SettingsType.WaitIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentCue, SettingsType.WaitIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentNonIncentive, SettingsType.WaitIntervalMin, _newValue);
					break;
				case IntervalType.MaxValue:
					SettingsHandler.SetSpriteField(SpriteTypes.ControlCue, SettingsType.WaitIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.ControlNonIncentive, SettingsType.WaitIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardCue, SettingsType.WaitIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardNonIncentive, SettingsType.WaitIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentCue, SettingsType.WaitIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentNonIncentive, SettingsType.WaitIntervalMax, _newValue);
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
