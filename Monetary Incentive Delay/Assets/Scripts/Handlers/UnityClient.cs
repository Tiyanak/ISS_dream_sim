using System;
using JetBrains.Annotations;
using Monetary_client;
using UnityEngine;
using Assets.Scripts.DataTypes;

namespace Assets.Scripts.Handlers
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
			if (Communicator == null)
			{
				Communicator = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}

		[UsedImplicitly]
		void Start()
		{
			SetupClient();
		}

		[UsedImplicitly]
		void SetupClient()
		{
			_client = new Client();
			_client.Connect("127.0.0.1", 11111);
		}

		[UsedImplicitly]
		void SendMsg(string msg)
		{
			_client.SendMsg(msg);
		}

		public void SendReaction(long taskId, int msgType, TaskType type, bool incentive, double reactionTime, double threshold)
		{
			Classes.Msgs.Reaction reaction = new Classes.Msgs.Reaction(taskId, msgType, type.ToString(), incentive, reactionTime, threshold);
			_client.SendMsg(reaction.Serialize());
		}

		public Classes.Msgs.Parameters ReceiveParameters()
		{
			Classes.Msgs.Parameters recParams = null;
			try {
				recParams = new Classes.Msgs.Parameters(_client.MsgListener()); 
				print("Serialized params: " + recParams);
			} catch (Exception) {
				print("Could not deserialize server data to class Parameters.");
			} 
		
			return recParams;

		}
	}
}

