namespace Assets.Scripts.DataTypes
{
    public class TimeSettings
    {
        public int SpriteDelayTimeMilliseconds => _spriteInterval.GetTime();
	    private Interval _spriteInterval;
        public int SpriteDisplayTimeMilliseconds { get; private set; }
        public int InfoDisplayTimeMilliseconds { get; private set; }

        public TimeSettings(Interval spriteDelayInterval, int spriteDisplayTimeMilliseconds, int infoDisplayTimeMilliseconds)
        {
	        _spriteInterval = spriteDelayInterval;
            SpriteDisplayTimeMilliseconds = spriteDisplayTimeMilliseconds;
            InfoDisplayTimeMilliseconds = infoDisplayTimeMilliseconds;
        }

	    public void SetSpriteDelay(Interval spriteDelayInterval)
	    {
		    _spriteInterval = spriteDelayInterval;
	    }

	    public void SetSpriteDisplay(int spriteDisplayTimeMilliseconds)
	    {
		    SpriteDisplayTimeMilliseconds = spriteDisplayTimeMilliseconds;
	    }

	    public void SetInfoDisplay(int infoDisplayTimeMilliseconds)
	    {
		    InfoDisplayTimeMilliseconds = infoDisplayTimeMilliseconds;
	    }
	}
}