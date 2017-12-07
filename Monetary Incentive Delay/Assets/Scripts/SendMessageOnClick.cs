using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Net.Sockets;
using System.Text;

public class HelloMessage
{
    public string name;
};

public class SendMessageOnClick : MonoBehaviour
{

    public GameObject go = null;
    public SocketIOComponent socket = null;
    public InputField uiInput = null;
    public Button sendTestMsgBtn;
    System.Net.Sockets.TcpClient clientSocket = null;

    public void Start()
    {
        go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        
    }

    public void Update()
    {
        
    }

    public void SendMessage(string str)
    {
        str = uiInput.text;
        print("Poruka: " + str);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = str;
        print(socket.IsConnected);
        socket.Emit("/app/hello", new JSONObject(data));
    }
}

