using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using UnityEngine;

namespace Assets.Scripts.Handlers
{
    public static class AntiSpamming
    {
        private static readonly List<double> SpamCounter = new List<double>();
		private static bool _resetFlag;

        public static bool CheckForSpamming(DisplayStatus currentDisplayStatus, bool spacebarHolding)
        {
	        if (_resetFlag)
		        Clear();
            bool spacebarPressed = CheckIfSpacebarPressed();
            if (spacebarPressed && !spacebarHolding && (currentDisplayStatus == DisplayStatus.DisplayingSprite ||
                                     currentDisplayStatus == DisplayStatus.WaitToDisplaySprite))
            {
                SpamCounter.Add(TimeHandler.GetMilliseconds());
            }
            return spacebarPressed;
        }

	    public static bool SoftCheck(int currentTask)
	    {
			int littleTime = 0;
		    for (int i = 1; i < SpamCounter.Count; i++)
		    {
			    if (SpamCounter[i] - SpamCounter[i - 1] < 200)
				    littleTime++;
		    }
		    double percentage = (double)littleTime / currentTask;
		    return SpamCounter.Count > 2 * currentTask || percentage > 0.3;
		}

        public static bool DidHeSpam(int numberOfTasks)
        {
	        _resetFlag = true;
	        return SoftCheck(numberOfTasks);
        }

        private static void Clear()
        {
			_resetFlag = false;
            SpamCounter.Clear();
        }

        private static bool CheckIfSpacebarPressed()
        {
            return Input.GetKeyDown("space");
        }
    }
}