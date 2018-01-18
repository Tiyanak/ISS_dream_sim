namespace Assets.Scripts.Interfaces
{
    public interface IBaselineSettings
    {
        int InfoDisplayTime{ get; }
        int NumberOfTasks { get; }
        void SetInfoDisplay(int infoDisplayTimeMilliseconds);
        void SetNumberOfTasks(int numberOfTasks);
    }
}