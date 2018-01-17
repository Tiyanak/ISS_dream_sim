using System;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.DataTypes
{
	public class GlobalSettings : MonoBehaviour
	{
		public static GlobalSettings Gs;

		public ITaskSettings BaselineSettings { get; private set; }
		public ITaskSettings ControlSettings { get; private set; }
		public ITaskSettings RewardSettings { get; private set; }
		public ITaskSettings PunishmentSettings { get; private set; }
		public double Threshold { get; private set; }

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
			BaselineSettings = new TaskSettings(new TimeSettings(new Interval(1500, 2500), 160, 4000), TaskType.Baseline, 20);
			ControlSettings = new TaskSettings(new TimeSettings(new Interval(2000, 3000), 160, 4000), TaskType.Control, 20, 0.8f);
			RewardSettings = new TaskSettings(new TimeSettings(new Interval(2000, 3000), 160, 4000), TaskType.Reward, 20, 0.8f);
			PunishmentSettings = new TaskSettings(new TimeSettings(new Interval(2000, 3000), 160, 4000), TaskType.Punishment, 20, 0.8f);
		}

		public void SetSettings(TaskSettings[] allSettings)
		{
			foreach (ITaskSettings t in allSettings)
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

		public ITaskSettings GetSettings(TaskType type)
		{
			switch (type)
			{
				case TaskType.Baseline:
					return BaselineSettings;
				case TaskType.Control:
					return ControlSettings;
				case TaskType.Reward:
					return RewardSettings;
				case TaskType.Punishment:
					return PunishmentSettings;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public void UpdateBaselineSettings(ITaskSettings newSettings)
		{
			BaselineSettings = newSettings;
		}

		public void UpdateControlSettings(ITaskSettings newSettings)
		{
			ControlSettings = newSettings;
		}

		public void UpdateRewardsSettings(ITaskSettings newSettings)
		{
			RewardSettings = newSettings;
		}

		public void UpdatePunishmentSettings(ITaskSettings newSettings)
		{
			PunishmentSettings = newSettings;
		}

		public void UpdateThreshold(double threshold)
		{
			Threshold = threshold;
		}
	}
}