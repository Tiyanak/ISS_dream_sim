using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Server : MonoBehaviour {

	public Button serverBtn;
	const short MyReactionMsg = 1002;
	 const short MyParametersMsg = 1003;

	// Use this for initialization
	void Start () {
		Button btn = serverBtn.GetComponent<Button>();
		btn.onClick.AddListener(SetupServer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetupServer() {

		NetworkServer.Listen(4444);
		NetworkServer.RegisterHandler(MyReactionMsg, OnReactionReceived);
		NetworkServer.RegisterHandler(MyParametersMsg, OnParametersReceived);

	}

	void OnReactionReceived(NetworkMessage msg) {

		Debug.Log("Id: " + msg.msgType.ToString());

		if (msg.msgType.Equals(1002)) {
			Reaction reaction = msg.ReadMessage<Reaction>();
			Debug.Log("Server received: " + reaction.ToString());
		} else {
			Debug.Log("Server received: unknown message");
		}
	
	}

	void OnParametersReceived(NetworkMessage msg) {

		Debug.Log("Id: " + msg.msgType.ToString());

		if (msg.msgType.Equals(1003)) {
			Parameters parameters = msg.ReadMessage<Parameters>();
			Debug.Log("Server received parameters: " + parameters.ToString());
		} else {
			Debug.Log("Server received: unknown message");
		}
	
	}

}
