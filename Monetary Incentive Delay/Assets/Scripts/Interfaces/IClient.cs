using Assets.Scripts.DataTypes;

namespace Assets.Scripts.Interfaces
{
    public interface IClient
    {
        // sending
        bool SendReaction(TaskType type, bool incentive, double reactionTime, double threshold);
        // receiving
        bool ReceiveParameters(out double targetDisplayTime, out double cueToTargetTime, out double threshold);
    }
}