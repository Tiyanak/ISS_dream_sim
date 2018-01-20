using System;

namespace Assets.Scripts.Handlers
{
	public static class Randomness
	{
		public static readonly Random Rand  = new Random();
		private static int MaxIter = 100;

		public static int[] RandomizeField(int fieldSize, float percentageOfZeroes)
		{
			bool success = false;
			int numberOfOnes = 0;
			int[] randomField = new int[fieldSize];
			float limit = fieldSize * (1 - percentageOfZeroes);
			for (int i = 0; i < limit; i++)
			{
				int iter = 0;
				while (iter++ < MaxIter)
				{
					int location = Rand.Next(fieldSize);
					if (randomField[location] != 1 && (location == 0 || randomField[location - 1] != 1 ) && (location == fieldSize - 1 || randomField[location + 1] != 1 ))
					{
						randomField[location] = 1;
						numberOfOnes++;
						success = true;
						break;
					}
				}
				if (iter < MaxIter) continue;
				success = false;
				break;
			}

			if (success)
				return randomField;

			for (int i = 0; i < randomField.Length; i++)
			{
				if (randomField[i] == 1 || i != 0 && randomField[i - 1] == 1 || i != fieldSize - 1 && randomField[i + 1] == 1) continue;
				randomField[i] = 1;
				numberOfOnes++;
				if ((float) numberOfOnes / fieldSize >= 1 - percentageOfZeroes)
				{
					success = true;
					break;
				}
			}

			if (success)
				return randomField;

			int current = 0;
			for (int i = 0; i < randomField.Length; i++)
			{
				randomField[i] = current;
				current = 1 - current;
			}

			return randomField;
		}
	}
}
