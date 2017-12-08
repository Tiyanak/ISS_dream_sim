
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
  
    public Button clientBtn;
	  public Button sendBtn;

	  NetworkClient myClient;

    public class Client : MonoBehaviour
    {
        void Start() {
            Button btnClient = clientBtn.GetComponent<Button>();
            btnClient.onClick.AddListener(SetupClient);

            Button btnSend = sendBtn.GetComponent<Button>();
            sendBtn.onClick.AddListener(SendMsg);
        }

        void SetupClient() {

          myClient = new NetworkClient();
          myClient.RegisterHandler(MsgType.Connect, OnConnected);
          myClient.Connect("127.0.0.1", 4444);
        }

        void OnConnected(NetworkMessage netMsg) {
          Debug.Log("Connected to server");
        }

        MessageBase GetMessage() {
          return new StringMessage("Hello");
        }

        void SendMsg() {
          MyMsg msg = new MyMsg();
          msg.message = "Hello sexi";
          myClient.Send(MsgType.Ready, msg);
        }
    
        public bool SendReaction(TaskType type, bool incentive, double reactionTime, double threshold)
        {
            throw new System.NotImplementedException();
        }

        public bool ReceiveParameters(out double targetDisplayTime, out double cueToTargetTime, out double threshold)
        {
            throw new System.NotImplementedException();
        }
    }
}
