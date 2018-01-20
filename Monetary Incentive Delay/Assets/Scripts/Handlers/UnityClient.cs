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
		private long _taskId;

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
		}

		public void Connect() {
			_client.Connect("127.0.0.1", 11111, GetStartTaskMsg());
		}

		public void Disconnect() {
			SendEndTaskMsg();
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
				string recMsg = _client.MsgListener();
				if (string.IsNullOrEmpty(recMsg)) return null;
				recParams = new Classes.Msgs.Parameters(recMsg); 
			} catch (Exception) {
				Debug.Log("Exceptin parsing params");
			} 
		
			return recParams;

		}

		private string GetStartTaskMsg()
        {
            return new Classes.Msgs.Reaction(0, 0, "HELLO", false, 0, 0).Serialize();
        }

		public void SendEndTaskMsg()
        {
			SendReaction(_taskId, 1, TaskType.Control, false, 0, 0);
            _client.Disconnect(_taskId.ToString());
        }

		public void SendFeedback(TaskType taskType, bool incentive, double reactionTime, double threshold)
        {
            SendReaction(_taskId, 2, taskType, incentive, reactionTime, threshold);
        }

		public double HandleServerParams()
        {

			double threshold = 0;

            Classes.Msgs.Parameters serverParams = ReceiveParameters();
            if (serverParams == null) return threshold;

            switch (serverParams.MsgType)
            {
                case 0: // Started task - receive my task id
                    _taskId = serverParams.TaskId;
                    break;

                case 1: // Ended task - receive confirm msg from server about ending
                    break;

                case 2: // Receive params from server
                    threshold = serverParams.Threshold;
					SettingsHandler.SetThreshold((int) threshold);
                    break;
            }

			return threshold;
        }
	}
}

