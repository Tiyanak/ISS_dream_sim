using System;

namespace Assets.Scripts.DataTypes
{
	public class Change
	{
		public bool IsBaseline { get; }
		public bool IsTask { get; }
		public bool IsSprite { get; }
		public SpriteTypes SpriteType { get; }
		public TaskType TaskChange { get; }
		public SettingsType SettingsType { get; }
		public int NewValue { get; }

		/// <summary>
		/// Task change
		/// </summary>
		/// <param name="taskType">Control, Reward or Punishment task</param>
		/// <param name="settingsType">what is being changed</param>
		/// <param name="newValue"></param>
		public Change(TaskType taskType, SettingsType settingsType, int newValue)
		{
			IsTask = true;
			TaskChange = taskType;
			SettingsType = settingsType;
			NewValue = newValue;
		}

		/// <summary>
		/// Change of baseline task number
		/// </summary>
		/// <param name="newTaskNumber"></param>
		public Change(int newTaskNumber)
		{
			IsBaseline = true;
			NewValue = newTaskNumber;
		}

		/// <summary>
		/// Change of Sprite intervals
		/// </summary>
		/// <param name="spriteType"></param>
		/// <param name="type"></param>
		/// <param name="newValue"></param>
		public Change(SpriteTypes spriteType, SettingsType type, int newValue)
		{
			switch (type)
			{
				case SettingsType.WaitIntervalMin:
				case SettingsType.WaitIntervalMax:
				case SettingsType.DisplayIntervalMin:
				case SettingsType.DisplayIntervalMax:
					IsSprite = true;
					SpriteType = spriteType;
					SettingsType = type;
					NewValue = newValue;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, "Not applicable type");
			}
		}
	}
}