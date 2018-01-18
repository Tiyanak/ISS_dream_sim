using System;

namespace Assets.Scripts.Handlers
{
	public static class Randomness
	{
		public static readonly Random Rand  = new Random();

		public static int[] RandomizeField(int fieldSize, float percentageOfZeroes)
		{
			int[] randomField = new int[fieldSize];
			for (int i = 0; i < fieldSize * (1 - percentageOfZeroes); i++)
			{
				while (true)
				{
					int location = Rand.Next(fieldSize);
					if ((location == 0 || randomField[location - 1] != 1 ) && (location == fieldSize - 1 || randomField[location + 1] != 1 ))
					{
						randomField[location] = 1;
						break;
					}
						
				}
			}
			return randomField;
		}
	}
}
