namespace Assets.Scripts.Handlers.GUI
{
    public static class OutputTextHandler
    {
        public static string Performance(double reactionTime, int numberOfTasks)
        {
            string performance;
            if (reactionTime < 300)
                performance = "You did great!";
            else if (reactionTime < 600)
                performance = "You did O.K.";
            else if (reactionTime < double.MaxValue)
                performance = "You did rather poorly.";
            else
                performance = "You did nothing!";
            
            if (AntiSpamming.DidHeSpam(numberOfTasks))
            {
                performance += " \nBut you spammed the keyboard";
            }
            return performance;
        }

	    public static string HowManyValid(double[] reactionTimes, int[] taskTypes, double threshold)
	    {
		    int howManyIncentive;
			int howManyNonincentive;
		    int howManyValid = SrtHandler.HowManyValid(reactionTimes, threshold);
			string howMany = "You achieved " + howManyValid + " / " + reactionTimes.Length + " tasks.";
		    SrtHandler.HowManyOfIncentive(reactionTimes, taskTypes, threshold, out howManyValid, out howManyIncentive);
		    howMany += "\nOf those are " + howManyValid + " / " + howManyIncentive + " incentive tasks, ";
		    SrtHandler.HowManyOfNonincentive(reactionTimes, taskTypes, threshold, out howManyValid, out howManyNonincentive);
		    howMany += "\nand " + howManyValid + " / " + howManyNonincentive + " nonincentive tasks.";
			return howMany;
	    }
    }
}