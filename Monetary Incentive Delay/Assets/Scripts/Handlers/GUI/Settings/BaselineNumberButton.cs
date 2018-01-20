using Assets.Scripts.DataTypes;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Handlers.GUI.Settings
{
	public class BaselineNumberButton : MonoBehaviour
	{
		private int _newValue;

		public void OnEnable()
		{
			_newValue = GlobalSettings.Gs.BaselineSettings.NumberOfTasks;
			ChangeInputFieldText(_newValue.ToString());
		}

		public void OnValueChanged()
		{
			string text = gameObject.GetComponentInChildren<Text>().text;
			int.TryParse(text, out _newValue);
			SettingsHandler.SetBaselineField(SettingsType.TaskNumber, _newValue);
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