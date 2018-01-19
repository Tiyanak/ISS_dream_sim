﻿using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using UnityEngine;

namespace Assets.Scripts.Handlers
{
    public static class AntiSpamming
    {
        private static List<double> _spamCounter = new List<double>();
		private static bool _resetFlag;

        public static bool CheckForSpamming(DisplayStatus currentDisplayStatus)
        {
	        if (_resetFlag)
		        Clear();
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
	        _resetFlag = true;
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