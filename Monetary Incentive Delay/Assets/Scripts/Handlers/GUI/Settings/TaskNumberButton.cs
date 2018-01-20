using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Handlers.GUI.Settings
{
	public class TaskNumberButton : MonoBehaviour
	{
		private ITaskSettings _taskSettings;
		public TaskType Type;
		private int _newValue;

		public void OnEnable()
		{
			_taskSettings = GlobalSettings.Gs.GetSettings(Type);
			_newValue = _taskSettings.NumberOfTasks;
			ChangeInputFieldText(_newValue.ToString());
		}

		public void OnValueChanged()
		{
			string text = gameObject.GetComponentInChildren<Text>().text;
			int.TryParse(text, out _newValue);
			SettingsHandler.SetTaskField(Type, SettingsType.TaskNumber, _newValue);
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