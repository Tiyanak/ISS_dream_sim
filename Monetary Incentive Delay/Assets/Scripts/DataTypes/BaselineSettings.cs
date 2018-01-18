using Assets.Scripts.Interfaces;

namespace Assets.Scripts.DataTypes
{
    public class BaselineSettings : IBaselineSettings
    {
	    public int InfoDisplayTime { get; private set; }
	    public int NumberOfTasks { get; private set; }

	    public BaselineSettings(int infoDisplayTime, int numberOfTasks)
	    {
		    InfoDisplayTime = infoDisplayTime;
	        NumberOfTasks = numberOfTasks;
        }

	    public void SetInfoDisplay(int infoDisplayTime)
	    {
		    InfoDisplayTime = infoDisplayTime;
	    }
	    
	    public void SetNumberOfTasks(int numberOfTasks)
	    {
		    NumberOfTasks = numberOfTasks;
	    }
	}
}