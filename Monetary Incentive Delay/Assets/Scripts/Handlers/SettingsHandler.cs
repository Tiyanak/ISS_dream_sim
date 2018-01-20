using System;
using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Handlers
{
	public static class SettingsHandler
	{
		private static readonly List<Change> Changes = new List<Change>();
		private static readonly List<Change> RedoChanges = new List<Change>();

		public static void SetThreshold(int newValue)
		{
			Changes.Add(new Change(SettingsType.Threshold, newValue));
		}

		public static void SetBaselineField(SettingsType type, int newValue)
		{
			if (type != SettingsType.TaskNumber) return;
			Changes.Add(new Change(type, newValue));
		}

		public static void SetTaskField(TaskType taskType, SettingsType type, int newValue)
		{
			Changes.Add(new Change(taskType, type, newValue));
		}

		public static void SetSpriteField(SpriteTypes spriteType, SettingsType type, int newValue)
		{
			Changes.Add(new Change(spriteType, type, newValue));
		}

		public static void ApplyChanges()
		{
			foreach (var change in Changes)
				ApplyChange(change);
			foreach (var redoChange in RedoChanges)
				ApplyChange(redoChange);

			Changes.Clear();
			RedoChanges.Clear();
		}

		private static void ApplyChange(Change change)
		{
			if (change.IsBaseline)
			{
				int newValue = Mathf.Clamp(change.NewValue, 1, int.MaxValue);
				GlobalSettings.Gs.BaselineSettings.SetNumberOfTasks(newValue);
			}
			else if (change.IsSprite)
			{

				Interval waitInterval = GlobalSettings.Gs.SpriteSettings.GetTimeSettings(change.SpriteType).WaitInterval;
				Interval displayInterval = GlobalSettings.Gs.SpriteSettings.GetTimeSettings(change.SpriteType).DisplayInterval;

				switch (change.SettingsType)
				{
					case SettingsType.WaitIntervalMin:
						if (change.NewValue > waitInterval.MaxTime && !RedoChanges.Contains(change))
							RedoChanges.Add(change);
						else if (change.NewValue <= waitInterval.MaxTime)
						{
							int newValue = Mathf.Clamp(change.NewValue, 0, int.MaxValue);
							GlobalSettings.Gs.SpriteSettings.GetTimeSettings(change.SpriteType).WaitInterval.SetMinTime(newValue);
						}
						break;
					case SettingsType.WaitIntervalMax:
						if (change.NewValue < waitInterval.MinTime && !RedoChanges.Contains(change))
							RedoChanges.Add(change);
						else if(change.NewValue >= waitInterval.MinTime)
						{
							int newValue = Mathf.Clamp(change.NewValue, 0, int.MaxValue);
							GlobalSettings.Gs.SpriteSettings.GetTimeSettings(change.SpriteType).WaitInterval.SetMaxTime(newValue);
						}
						break;
					case SettingsType.DisplayIntervalMin:
						if (change.NewValue > displayInterval.MaxTime && !RedoChanges.Contains(change))
							RedoChanges.Add(change);
						else if(change.NewValue <= displayInterval.MaxTime)
						{
							int newValue = Mathf.Clamp(change.NewValue, 0, int.MaxValue);
							GlobalSettings.Gs.SpriteSettings.GetTimeSettings(change.SpriteType).DisplayInterval.SetMinTime(newValue);
						}
						break;
					case SettingsType.DisplayIntervalMax:
						if (change.NewValue < displayInterval.MinTime && !RedoChanges.Contains(change))
							RedoChanges.Add(change);
						else if (change.NewValue >= displayInterval.MinTime)
						{
							int newValue = Mathf.Clamp(change.NewValue, 0, int.MaxValue);
							GlobalSettings.Gs.SpriteSettings.GetTimeSettings(change.SpriteType).DisplayInterval.SetMaxTime(newValue);
						}
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else if (change.IsTask)
			{
				switch (change.SettingsType)
				{
					case SettingsType.TaskNumber:
						int newValue = Mathf.Clamp(change.NewValue, 1, int.MaxValue);
						GlobalSettings.Gs.GetSettings(change.TaskChange).SetNumberOfTasks(newValue);
						break;
					case SettingsType.NonIncentivePercentage:
						float newPercentage = Mathf.Clamp(change.NewValue, 50f, 100f)/100;
						GlobalSettings.Gs.GetSettings(change.TaskChange).SetNonIncentivePercentage(newPercentage);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else if (change.IsThreshold)
			{
				
			}
		}
	}
}