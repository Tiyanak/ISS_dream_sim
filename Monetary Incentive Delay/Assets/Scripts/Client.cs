using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
    public class Client : IClient
    {
        public bool SendReaction(TaskType type, bool incentive, double reactionTime, double threshold)
        {
            throw new System.NotImplementedException();
        }

        public bool ReceiveParameters(out double targetDisplayTime, out double cueToTargetTime, out double threshold)
        {
            throw new System.NotImplementedException();
        }
    }
}