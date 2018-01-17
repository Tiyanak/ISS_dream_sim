using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.DataTypes
{
	public class GlobalSettings : MonoBehaviour
	{
		public static GlobalSettings Gs;

		public TaskSettings BaselineSettings { get; private set; }
		public TaskSettings ControlSettings { get; private set; }
		public TaskSettings RewardSettings { get; private set; }
		public TaskSettings PunishmentSettings { get; private set; }

		[UsedImplicitly]
		private void Awake()
		{
			if (Gs != null)
				Destroy(Gs);
			else
				Gs = this;

			DontDestroyOnLoad(this);
		}

		public GlobalSettings()
		{
			BaselineSettings = new TaskSettings(new TimeSettings(new Interval(500, 1000), 200, 4000), TaskType.Baseline, 30);
			ControlSettings = new TaskSettings(new TimeSettings(new Interval(2000, 3000), 200, 4000), TaskType.Control, 40, 0.8f);
			RewardSettings = new TaskSettings(new TimeSettings(new Interval(2000, 3000), 150, 4000), TaskType.Reward, 40, 0.8f);
			PunishmentSettings = new TaskSettings(new TimeSettings(new Interval(2000, 3000), 150, 4000), TaskType.Punishment, 40, 0.8f);
		}

		public void SetSettings(TaskSettings[] allSettings)
		{
			foreach (TaskSettings t in allSettings)
			{
				if(t == null)
					continue;
				switch (t.Task)
				{
					case TaskType.Baseline:
						BaselineSettings = t;
						break;
					case TaskType.Control:
						ControlSettings = t;
						break;
					case TaskType.Reward:
						RewardSettings = t;
						break;
					case TaskType.Punishment:
						PunishmentSettings = t;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void UpdateBaselineSettings(TaskSettings newSettings)
		{
			BaselineSettings = newSettings;
		}

		public void UpdateControlSettings(TaskSettings newSettings)
		{
			ControlSettings = newSettings;
		}

		public void UpdateRewardsSettings(TaskSettings newSettings)
		{
			RewardSettings = newSettings;
		}

		public void UpdatePunishmentSettings(TaskSettings newSettings)
		{
			PunishmentSettings = newSettings;
		}

	}
}