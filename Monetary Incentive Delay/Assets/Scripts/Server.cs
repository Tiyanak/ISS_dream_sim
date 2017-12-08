using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Server : MonoBehaviour {

	public Button serverBtn;

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
		NetworkServer.RegisterHandler(MsgType.Ready, OnMsgReceived);

	}

	void OnMsgReceived(NetworkMessage msg) {
		MyMsg myMsg = msg.ReadMessage<MyMsg>();
		Debug.Log("Server received: " + myMsg.message);
	}

}
