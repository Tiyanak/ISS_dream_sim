using System;
using Interfaces;
using UnityEngine;

namespace Assets.Scripts.DataTypes
{
	public class TaskSettings : ITaskSettings
	{
		public int InfoTime { get; private set; }
		public TaskType Task { get; }
		public int NumberOfTasks { get; private set; }
		public float NonIncentivePercentage { get; private set; }
		public SpriteTypes[] IncentiveOrder { get; }
		public SpriteTypes[] NonIncentiveOrder { get; }

		public TaskSettings(int infoTime, TaskType task, int taskNumber, float nonIncentivePercentage = -1)
		{
			InfoTime = infoTime;
			Task = task;
			NumberOfTasks = taskNumber;
			if (nonIncentivePercentage >= 0)
				nonIncentivePercentage = Mathf.Clamp(nonIncentivePercentage, 0, 1);

			switch (task)
			{
				case TaskType.Control:
					NonIncentivePercentage = nonIncentivePercentage < 0 ? 0.8f : nonIncentivePercentage;
					IncentiveOrder = new SpriteTypes[]{SpriteTypes.ControlCue, SpriteTypes.Target, SpriteTypes.Incorrect};
					NonIncentiveOrder = new SpriteTypes[]{SpriteTypes.ControlNonIncentive, SpriteTypes.Target, SpriteTypes.Incorrect};
					break;
				case TaskType.Reward:
					NonIncentivePercentage = nonIncentivePercentage < 0 ? 0.8f : nonIncentivePercentage;
					IncentiveOrder = new SpriteTypes[]{SpriteTypes.RewardCue, SpriteTypes.Target, SpriteTypes.Incorrect};
					NonIncentiveOrder = new SpriteTypes[]{SpriteTypes.RewardNonIncentive, SpriteTypes.Target, SpriteTypes.Incorrect};
					break;
				case TaskType.Punishment:
					NonIncentivePercentage = nonIncentivePercentage < 0 ? 0.8f : nonIncentivePercentage;
					IncentiveOrder = new SpriteTypes[]{SpriteTypes.PunishmentCue, SpriteTypes.Target, SpriteTypes.Incorrect};
					NonIncentiveOrder = new SpriteTypes[]{SpriteTypes.PunishmentNonIncentive, SpriteTypes.Target, SpriteTypes.Incorrect};
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(task), task, null);
			}
		}
		
		public void SetInfoTime(int newTime)
		{
			InfoTime = newTime;
		}

		public void SetNumberOfTasks(int newNumber)
		{
			NumberOfTasks = newNumber;
		}

		public void SetNonIncentivePercentage(float percentage)
		{
			NonIncentivePercentage = Mathf.Clamp(percentage, 0, 1);
		}
	}
}
