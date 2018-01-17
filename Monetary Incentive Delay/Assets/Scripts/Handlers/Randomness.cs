using System;

namespace Assets.Scripts.Handlers
{
	public static class Randomness
	{
		private static Random _rand  = new Random();

		public static int[] RandomizeField(int fieldSize, int percentageOfZeroes)
		{
			int[] randomField = new int[fieldSize];
			for (int i = 0; i < fieldSize * (1 - percentageOfZeroes); i++)
			{
				while (true)
				{
					int location = _rand.Next(fieldSize);
					if (randomField[location - 1] != 1 && randomField[location + 1] != 1)
						break;
				}
			}
			return randomField;
		}
	}
}
