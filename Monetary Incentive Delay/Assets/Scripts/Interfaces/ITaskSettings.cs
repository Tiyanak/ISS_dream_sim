using Assets.Scripts.DataTypes;

namespace Interfaces
{
	public interface ITaskSettings
	{
		TimeSettings Time { get; }
		TaskType Task { get; }
		int NumberOfTasks { get; }
		float NonIncentivePercentage { get; }
	
		void SetTimeSettings(TimeSettings time);
		void SetNumberOfTasks(int newNumber);
		void SetNonIncentivePercentage(float percentage);
	}
}
