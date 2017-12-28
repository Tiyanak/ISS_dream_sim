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
        Writer writer;

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
                Console.WriteLine("Server started.");

                Thread.Sleep(1000);
               
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
                            NetConnection clientConnection = msg.SenderConnection;
                            string clientId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();
                            writer.writeLine("1,2,3," + data);

                            Console.WriteLine("Received msg: " + data);
                           
                            break;

                        case NetIncomingMessageType.StatusChanged:

                            switch (msg.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:

                                    string clientConnectedId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();
                                    string hailMessage = msg.SenderConnection.RemoteHailMessage.ReadString();
                                    writer = new Writer();

                                    Console.WriteLine("Client connected: " + clientConnectedId + " : " + hailMessage);

                                    break;

                                case NetConnectionStatus.Disconnected:

                                    string clientDisconnectedId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();
                                    string goodByeMsg = msg.ReadString();
                                    this.writer.closeFile();

                                    Console.WriteLine("Client disconnected: " + clientDisconnectedId + " : " + goodByeMsg);

                                    break;
                            }

                            break;

                        default:
                            string unhandledMsgType = msg.MessageType.ToString();
                          
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
            }            
        }

    }
}
