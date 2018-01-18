using Assets.Scripts.Interfaces;

namespace Assets.Scripts.DataTypes
{
    public class SpriteTime : ISpriteTime
    {
        public int SpriteDisplayTime => DisplayInterval.GetTime();
        public int SpriteDelayTime => WaitInterval.GetTime();
        public Interval DisplayInterval { get; private set; }
        public Interval WaitInterval { get; private set; }
        public SpriteTypes Type { get; private set; }
        
        public SpriteTime(Interval waitInterval, Interval displayInterval, SpriteTypes type)
        {
            DisplayInterval = displayInterval;
            WaitInterval = waitInterval;
            Type = type;
        }

        public void SetDisplayInterval(Interval newDisplayInterval)
        {
            DisplayInterval = newDisplayInterval;
        }
        
        public void SetWaitInterval(Interval newWaitInterval)
        {
            WaitInterval = newWaitInterval;
        }
    }
}