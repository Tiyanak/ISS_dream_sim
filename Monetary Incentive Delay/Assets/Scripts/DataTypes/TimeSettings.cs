using UnityEngine;

namespace DataTypes
{
    public class TimeSettings
    {
        public int SpriteDelayTimeMiliseconds { get; private set; }
        public int SpriteDisplayTimeMiliseconds { get; private set; }
        public int InfoDisplayTimeMiliseconds { get; private set; }

        public TimeSettings(int spriteDelayTimeMiliseconds, int spriteDisplayTimeMiliseconds, int infoDisplayTimeMiliseconds)
        {
            SpriteDelayTimeMiliseconds = spriteDelayTimeMiliseconds;
            SpriteDisplayTimeMiliseconds = spriteDisplayTimeMiliseconds;
            InfoDisplayTimeMiliseconds = infoDisplayTimeMiliseconds;
        }
    }
}