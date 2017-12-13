
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Assets.Scripts.DataTypes;

public class Client : MonoBehaviour
{
 	const short MyReactionMsg = 1002;
	 const short MyParametersMsg = 1003;
	public Button clientBtn;
	public Button sendBtn;
	public int counter = 0;

	NetworkClient myClient;

	void Start()
	{
		Button btnClient = clientBtn.GetComponent<Button>();
		btnClient.onClick.AddListener(SetupClient);

		Button btnSend = sendBtn.GetComponent<Button>();
		sendBtn.onClick.AddListener(SendMsg);
	}

	void SetupClient()
	{

		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);
		myClient.Connect("127.0.0.1", 11111);
	}

	void OnConnected(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server");
	}

	MessageBase GetMessage()
	{
		return new StringMessage("Hello");
	}

	void SendMsg()
	{
		System.Random rand = new System.Random();

		if (counter % 2 == 0) {
			SendReaction(TaskType.Control, true, 10.0, 20.0);
			counter++;
		} else {
			ReceiveParameters(30.0,  20.0, 50.0);
			counter++;
		}
	}

	public bool SendReaction(TaskType type, bool incentive, double reactionTime, double threshold)
	{

		NetworkWriter writer = new NetworkWriter();
        writer.StartMessage(12345);
        writer.Write(new Reaction(type, incentive, reactionTime, threshold).ToString());
        writer.FinishMessage();

        return myClient.SendWriter(writer, Channels.DefaultReliable);
		
		//return myClient.Send(MyReactionMsg,  new Reaction(type, incentive, reactionTime, threshold));
	}

	public bool ReceiveParameters(double targetDisplayTime,  double cueToTargetTime, double threshold)
	{
		return myClient.Send(MyParametersMsg,  new Parameters(targetDisplayTime, cueToTargetTime, threshold));
	}
}

