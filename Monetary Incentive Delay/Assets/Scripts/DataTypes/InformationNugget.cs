namespace Assets.Scripts.DataTypes
{
	public class InformationNugget
	{
		public string InfoText { get; private set; }
		public DisplayStatus NextDisplayStatus { get; private set; }
		public int DisplayTime { get; private set; }
		public bool Skippable { get; private set; }
		public SpriteTypes Type { get; private set; }

		public InformationNugget(string infoText, DisplayStatus nextDisplayStatus, int displayTime, bool skippable, SpriteTypes type = SpriteTypes.None)
		{
			InfoText = infoText;
			NextDisplayStatus = nextDisplayStatus;
			DisplayTime = displayTime;
			Skippable = skippable;
			Type = type;
		}

		public void SetInfoText(string newInfoText)
		{
			InfoText = newInfoText;
		}

		public void SetNextDisplayStatus(DisplayStatus newStatus)
		{
			NextDisplayStatus = newStatus;
		}

		public void SetDisplayTime(int newDIsplayTime)
		{
			DisplayTime = newDIsplayTime;
		}

		public void SetSkippable(bool skippable)
		{
			Skippable = skippable;
		}

		public void SetSpriteType(SpriteTypes newType)
		{
			Type = newType;
		}
	}
}