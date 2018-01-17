using System;
using Interfaces;
using UnityEngine;

namespace Assets.Scripts.DataTypes
{
	public class TaskSettings : ITaskSettings
	{
		public TimeSettings Time { get; private set; }
		public TaskType Task { get; }
		public int NumberOfTasks { get; private set; }
		public float NonIncentivePercentage { get; private set; }

		public TaskSettings(TimeSettings time, TaskType task, int taskNumber, float nonIncentivePercentage = -1)
		{
			Time = time;
			Task = task;
			NumberOfTasks = taskNumber;
			if (nonIncentivePercentage >= 0)
				nonIncentivePercentage = Mathf.Clamp(nonIncentivePercentage, 0, 1);

			switch (task)
			{
				case TaskType.Baseline:
					NonIncentivePercentage = nonIncentivePercentage < 0 ? 0 : nonIncentivePercentage;
					break;
				case TaskType.Control:
					NonIncentivePercentage = nonIncentivePercentage < 0 ? 0.8f : nonIncentivePercentage;
					break;
				case TaskType.Reward:
					NonIncentivePercentage = nonIncentivePercentage < 0 ? 0.8f : nonIncentivePercentage;
					break;
				case TaskType.Punishment:
					NonIncentivePercentage = nonIncentivePercentage < 0 ? 0.8f : nonIncentivePercentage;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(task), task, null);
			}
		}

		public void SetTimeSettings(TimeSettings time)
		{
			Time = time;
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
