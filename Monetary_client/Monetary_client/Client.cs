using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;

namespace Monetary_client
{
    class Client
    {

        string host;
        int port;
        NetPeerConfiguration config;
        NetClient client;
        NetIncomingMessage msg;

        public Client()
        {
            this.config = new NetPeerConfiguration("MonetaryTest");
            this.client = new NetClient(config);
        }


        public void Connect(string host, int port)
        {
            if (this.client != null)
            {
                this.client.Start();

                NetOutgoingMessage hailMsg = this.client.CreateMessage();
                hailMsg.Write("Hello!");
                NetConnection clientConnection = this.client.Connect(host, port, hailMsg);
                Console.WriteLine("Client connected.");

                Thread.Sleep(1000);

                MsgListener();
            }
        }

        public void Disconnect()
        {
            this.client.Disconnect("Goodbye from client");
        }

        public void MsgListener()
        {            
            while (this.client.Status == NetPeerStatus.Running)
            {
                while ((msg = this.client.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:

                            string data = msg.ReadString();
                            NetConnection clientConnection = msg.SenderConnection;
                            string clientId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();

                            Console.WriteLine("Received msg: " + data);

                            break;

                        case NetIncomingMessageType.StatusChanged:
                          
                            switch (msg.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:

                                    string clientConnectedId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();

                                    Console.WriteLine("Client connected: " + clientConnectedId + " : " + client.UniqueIdentifier);

                                    break;

                                case NetConnectionStatus.Disconnected:

                                    string clientDisconnectedId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();
                                    string goodByeMsg = msg.ReadString();

                                    Console.WriteLine("Client disconnected: " + clientDisconnectedId + " : " + goodByeMsg);

                                    break;
                            }

                            break;
                            
                        default:

                            string unhandledMsgType = msg.MessageType.ToString();
                            
                            break;
                    }
                }

                SendMsg(DateTime.Now.ToString("yyyyMMddHHmmss"));
                Thread.Sleep(5000);
            }
        }

        public void SendMsg(string msg)
        {
            NetOutgoingMessage outMsg = this.client.CreateMessage();
            outMsg.Write(msg);
            NetSendResult sendResult = this.client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);          
        }

    }
}
