using System;

namespace Assets.Scripts.DataTypes
{
	public class Interval
	{
		private int _minTime;
		private int _maxTime;
		private Random rand;

		public Interval(int minTime, int maxTime)
		{
			_minTime = minTime;
			_maxTime = maxTime;
			rand = new Random();
		}

		public int GetTime()
		{
			return rand.Next(_minTime, _maxTime + 1);
		}
	}
}
