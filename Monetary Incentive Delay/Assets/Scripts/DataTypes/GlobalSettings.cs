using System;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.DataTypes
{
	public class GlobalSettings : MonoBehaviour
	{
		public static GlobalSettings Gs;

		public IBaselineSettings BaselineSettings { get; private set; }
		public ITaskSettings ControlSettings { get; private set; }
		public ITaskSettings RewardSettings { get; private set; }
		public ITaskSettings PunishmentSettings { get; private set; }
		public double Threshold { get; private set; }
		public ISpriteSettings SpriteSettings { get; private set; }

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
			const int numberOfTasks = 10;
			BaselineSettings = new BaselineSettings(3000, 10);
			ControlSettings = new TaskSettings(3000, TaskType.Control, numberOfTasks, 0.8f);
			RewardSettings = new TaskSettings(3000, TaskType.Reward, numberOfTasks, 0.8f);
			PunishmentSettings = new TaskSettings(3000, TaskType.Punishment, numberOfTasks, 0.8f);
			SpriteSettings = new SpriteSettings();
			Threshold = 400;
		}

		public void SetSettings(TaskSettings[] allSettings)
		{
			foreach (ITaskSettings t in allSettings)
			{
				if(t == null)
					continue;
				switch (t.Task)
				{
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
		
		public void SetSpriteSettings(ISpriteSettings newSettings)
		{
			SpriteSettings = newSettings;
		}

		public void UpdateBaselineSettings(IBaselineSettings newSettings)
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