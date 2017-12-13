using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;

namespace Monetary_server
{

    class Server
    {


        NetPeerConfiguration config;
        NetServer server;
        NetIncomingMessage msg;
        int port = 11111;

        public Server()
        {
            this.config = new NetPeerConfiguration("MonetaryTest") { Port = this.port };
            this.server = new NetServer(this.config);
        }

        public Server(int port)
        {
            this.port = port;
            this.config = new NetPeerConfiguration("Monetary_server") { Port = this.port };
            this.server = new NetServer(this.config);
        }

        public void StartServer()
        {
            if (this.server != null)
            {
                this.server.Start();

                Thread.Sleep(1000);

                Console.WriteLine("Server started!");     
                MsgListener();
                
            }
        }

        public void MsgListener()
        {
            while (this.server.Status == NetPeerStatus.Running)
            {
                while ((msg = this.server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:

                            string data = msg.ReadString();
                            Console.WriteLine("Client: " + msg.SenderConnection.RemoteUniqueIdentifier.ToString() + " -> Data: " + data);

                            SendMsg("Message is received to server!",  msg.SenderConnection);                            
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            switch (msg.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    Console.WriteLine("Client status: Connected -> " + msg.SenderConnection.RemoteUniqueIdentifier.ToString() 
                                        + " -> Hail message: " + msg.SenderConnection.RemoteHailMessage.ReadString());
                                    break;

                                case NetConnectionStatus.Disconnected:
                                    Console.WriteLine("Client status: Disconnected -> " + msg.SenderConnection.RemoteUniqueIdentifier.ToString()
                                         + " -> Goodbye message: " + msg.ReadString());
                                    break;
                            }
                            break;

                        default:
                            Console.WriteLine("Unhandles message with type: " + msg.MessageType);
                            break;

                    }
                }
            }
        }

        public void SendMsg(string msg, NetConnection clientConnection)
        {
            NetOutgoingMessage outMsg = this.server.CreateMessage();
            outMsg.Write(msg);
            
            if (clientConnection != null)
            {
                NetSendResult sendResult = this.server.SendMessage(outMsg, clientConnection, NetDeliveryMethod.ReliableOrdered);
                Console.WriteLine("Sent message status: " + sendResult + " -> Message: " + msg);
            }

           
        }

    }
}
