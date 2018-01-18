using Assets.Scripts.DataTypes;

namespace Assets.Scripts.Interfaces
{
	public interface IInformationHolder
	{
		void Reset();
		InformationNugget GetNextInformation();
		InformationNugget CurrentNugget { get; }
	}
}