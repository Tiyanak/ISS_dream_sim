using System.Collections.Generic;
using Assets.Scripts.DataTypes;
using UnityEngine;

namespace Assets.Scripts.Handlers
{
    public static class AntiSpamming
    {
        private static readonly List<double> SpamCounter = new List<double>();
		private static bool _resetFlag;

        public static bool CheckForSpamming(DisplayStatus currentDisplayStatus, bool spacebarPressed)
        {                       
            if (spacebarPressed && (currentDisplayStatus == DisplayStatus.WaitToDisplaySprite))
            {
                SpamCounter.Add(TimeHandler.GetMilliseconds());
            }
            return false;
        }

	    public static bool SoftCheck(int allowSpammTimes)
	    {
			int littleTime = 0;
		    for (int i = 1; i < SpamCounter.Count; i++)
		    {
                double spamDif = SpamCounter[i] - SpamCounter[i - 1];
			    Debug.Log("Spam DIF: " + SpamCounter[i].ToString() + " - " + SpamCounter[i - 1].ToString() + " = " + spamDif.ToString());
                if (spamDif < 200) littleTime++;
		    }
		    return littleTime > allowSpammTimes;
		}

        public static bool DidHeSpam(int allowSpammTimes) {
            return SoftCheck(allowSpammTimes);
        }

        public static void Clear()
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