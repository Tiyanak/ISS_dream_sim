using Assets.Scripts.DataTypes;

namespace Assets.Scripts.Interfaces
{
	public interface ITaskSettings
	{
		int InfoTime { get; }
		TaskType Task { get; }
		int NumberOfTasks { get; }
		SpriteTypes[] IncentiveOrder { get; }
		SpriteTypes[] NonIncentiveOrder { get; }

		float NonIncentivePercentage { get; }
		void SetNumberOfTasks(int newNumber);
		void SetNonIncentivePercentage(float percentage);
		void SetInfoTime(int newTime);
	}
}
