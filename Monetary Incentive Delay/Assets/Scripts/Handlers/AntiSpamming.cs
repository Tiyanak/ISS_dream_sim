using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using UnityEngine;

namespace Handlers
{
    public static class AntiSpamming
    {
        private static List<double> _spamCounter = new List<double>();

        public static bool CheckForSpamming(DisplayStatus currentDisplayStatus)
        {
            bool spacebarPressed = CheckIfSpacebarPressed();
            if (spacebarPressed && (currentDisplayStatus == DisplayStatus.DisplayingSprite ||
                                     currentDisplayStatus == DisplayStatus.WaitToDisplaySprite))
            {
                _spamCounter.Add(TimeHandler.GetMilliseconds());
            }
            return spacebarPressed;
        }

        public static bool DidHeSpam(int numberOfTasks)
        {
            int littleTime = 0;
            for (int i = 1; i < _spamCounter.Count; i++)
            {			
                if (_spamCounter[i] - _spamCounter[i - 1] < 200)
                    littleTime++;
            }
            double percentage = (double) littleTime / numberOfTasks;
            return _spamCounter.Count > 2 * numberOfTasks || percentage > 0.3;
        }

        private static void Clear()
        {
            _spamCounter.Clear();
        }

        private static bool CheckIfSpacebarPressed()
        {
            return Input.GetKeyDown("space");
        }
    }
}