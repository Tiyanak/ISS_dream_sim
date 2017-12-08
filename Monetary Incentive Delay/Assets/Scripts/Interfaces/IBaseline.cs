namespace Assets.Scripts.Interfaces
{
    public interface IBaseline
    {
        void SendResults(double[] results);
        double GetSrt();
    }
}