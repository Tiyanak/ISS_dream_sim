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
                hailMsg.Write("Hello server!");

                NetConnection clientConnection = this.client.Connect(host, port, hailMsg);

                Thread.Sleep(1000);

                Console.WriteLine("Client connected: " + clientConnection.Status);
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
                            // handle custom messages
                            string data = msg.ReadString();
                            Console.WriteLine("Client received data: " + data);
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            switch (msg.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    Console.WriteLine("Client connected");
                                    SendMsg("OnConnectedTestMsg: Winter is coming, prepare yourself server!");
                                    break;

                                case NetConnectionStatus.Disconnected:
                                    Console.WriteLine("Client disconnected");
                                    break;
                            }
                            break;
                            
                        default:
                            Console.WriteLine("unhandled message with type: " + msg.MessageType);
                            break;
                    }
                }
            }
        }

        public void SendMsg(string msg)
        {
            NetOutgoingMessage outMsg = this.client.CreateMessage();
            outMsg.Write(msg);

            NetSendResult sendResult = this.client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
            Console.WriteLine("Sent message status: " + sendResult + " -> Message: " + msg);

        }

    }
}
