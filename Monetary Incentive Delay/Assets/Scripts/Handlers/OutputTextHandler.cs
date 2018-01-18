namespace Assets.Scripts.Handlers
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
    }
}