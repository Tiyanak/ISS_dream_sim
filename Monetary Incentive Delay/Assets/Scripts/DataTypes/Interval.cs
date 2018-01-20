using Assets.Scripts.Handlers;

namespace Assets.Scripts.DataTypes
{
	public class Interval
	{
		public int MinTime { get; private set; }
		public int MaxTime { get; private set; }

		public Interval(int minTime, int maxTime)
		{
			MinTime = minTime;
			MaxTime = maxTime;
		}

		public int GetTime()
		{
			return Randomness.Rand.Next(MinTime, MaxTime);
		}

		public void SetMinTime(int newMinTime)
		{
			MinTime = newMinTime;
		}

		public void SetMaxTime(int newMaxTime)
		{
			MaxTime = newMaxTime;
		}
	}
}
