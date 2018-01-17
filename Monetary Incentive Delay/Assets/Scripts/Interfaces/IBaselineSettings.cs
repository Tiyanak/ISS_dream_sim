using Assets.Scripts.DataTypes;

namespace Interfaces
{
    public interface IBaselineSettings
    {
        int InfoDisplayTime{ get; }
        int NumberOfTasks { get; }
        void SetInfoDisplay(int infoDisplayTimeMilliseconds);
        void SetNumberOfTasks(int numberOfTasks);
    }
}