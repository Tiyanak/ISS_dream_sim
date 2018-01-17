using Assets.Scripts.DataTypes;

namespace Interfaces
{
    public interface ISpriteTime
    {
        int SpriteDisplayTime { get; }
        int SpriteDelayTime { get; }
        Interval DisplayInterval { get; }
        Interval WaitInterval { get; }
        SpriteTypes Type { get; }
        void SetDisplayInterval(Interval newDisplayInterval);
        void SetWaitInterval(Interval newWaitInterval);
    }
}