using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Handlers.GUI.Settings
{
	public class CuesDisplayButton : MonoBehaviour
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
					_newValue = _spriteTime.DisplayInterval.MinTime;
					break;
				case IntervalType.MaxValue:
					_newValue = _spriteTime.DisplayInterval.MaxTime;
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
					SettingsHandler.SetSpriteField(SpriteTypes.ControlCue, SettingsType.DisplayIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.ControlNonIncentive, SettingsType.DisplayIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardCue, SettingsType.DisplayIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardNonIncentive, SettingsType.DisplayIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentCue, SettingsType.DisplayIntervalMin, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentNonIncentive, SettingsType.DisplayIntervalMin, _newValue);
					break;
				case IntervalType.MaxValue:
					SettingsHandler.SetSpriteField(SpriteTypes.ControlCue, SettingsType.DisplayIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.ControlNonIncentive, SettingsType.DisplayIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardCue, SettingsType.DisplayIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.RewardNonIncentive, SettingsType.DisplayIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentCue, SettingsType.DisplayIntervalMax, _newValue);
					SettingsHandler.SetSpriteField(SpriteTypes.PunishmentNonIncentive, SettingsType.DisplayIntervalMax, _newValue);
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
