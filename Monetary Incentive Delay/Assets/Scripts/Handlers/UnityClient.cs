using Assets.Scripts.DataTypes;
using JetBrains.Annotations;
using Monetary_client;
using UnityEngine;

namespace Assets.Scripts
{
	public class UnityClient : MonoBehaviour
	{
		public static UnityClient Communicator;
		
		public int Counter;

		Client _client;

		public UnityClient()
		{
			Counter = 0;
		}
		
		[UsedImplicitly]
		private void Awake()
		{
			if (Communicator != null)
				Destroy(Communicator);
			else
				Communicator = this;

			DontDestroyOnLoad(this);
		}

		[UsedImplicitly]
		void Start()
		{
		}

		[UsedImplicitly]
		void Update()
		{
			if (_client != null)
			{
				ReceiveParameters();
			}
		}

		void SetupClient()
		{
			_client = new Client();
			_client.Connect("127.0.0.1", 11111);
		}

		void SendMsg()
		{
			SendReaction(TaskType.Control, true, 190, 140);
		}

		public bool SendReaction(TaskType type, bool incentive, double reactionTime, double threshold)
		{
			Reaction reaction = new Reaction(type.ToString(), incentive, reactionTime, threshold);
			_client.SendMsg(reaction.serialize());

			return true;
		}

		public bool ReceiveParameters()
		{

			_client.MsgListener();

			return true;

		}
	}
}

