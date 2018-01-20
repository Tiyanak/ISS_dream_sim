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
        private Dictionary<long, Writer> writers;
        private Dictionary<long, List<double>> reactions;

        public Server()
        {
            this.writers = new Dictionary<long, Writer>();
            this.reactions = new Dictionary<long, List<double>>();
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
                            
                            try
                            {
                                Reaction r = new Reaction(data);
                                Console.WriteLine("Reaction: " + r.ToString());

                                if (r.msgType == 1)
                                {
                                    OnDisconnected(r, clientConnection);
                                } else
                                {
                                    OnData(r, clientConnection);
                                }
                            } catch (Exception e)
                            {
                                Console.WriteLine("Cant deserialize that.");
                            }
                            
                            break;

                        case NetIncomingMessageType.StatusChanged:

                            switch (msg.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:

                                    string clientConnectedId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();
                                    string hailMessage = msg.SenderConnection.RemoteHailMessage.ReadString();                   

                                    Console.WriteLine("Client connected: " + clientConnectedId + " : " + hailMessage);

                                    try
                                    {
                                        OnConnected(new Reaction(hailMessage), msg.SenderConnection);
                                    } catch (Exception e)
                                    {
                                        Console.WriteLine("Exception parsing Hail Reaction!");
                                    }
                                 
                                    break;

                                case NetConnectionStatus.Disconnected:

                                    string goodByeMsg = msg.ReadString();
                                    string clientDisconnectedId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();

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

        private void OnConnected(Reaction reaction, NetConnection clientConnection)
        {
            long id = GetMilliseconds();
            string filename = id.ToString() + ".csv";

            this.writers[id] = new Writer(filename);
            this.reactions[id] = new List<double>();

            this.writers[id].writeLine(reaction.getFieldsSemiCSV());

            string conParams = new Parameters(id, 0, 0, 0, 0).Serialize();
            SendMsg(conParams, clientConnection);
            
        }

        private void OnDisconnected(Reaction reaction, NetConnection clientConnection)
        {
            this.writers[reaction.taskId].closeFile();
            this.writers.Remove(reaction.taskId);
            this.reactions.Remove(reaction.taskId);
        }

        private void OnData(Reaction reaction, NetConnection clientConnection)
        {
            this.writers[reaction.taskId].writeLine(reaction.toSemiCSV());
            this.reactions[reaction.taskId].Add(reaction.reactionTime);

            double newThreshold = GaussHandler.GetAcceptableReationTime(this.reactions[reaction.taskId]);
            SendMsg(new Parameters(reaction.taskId, 2, 0, 0, newThreshold).Serialize(), clientConnection);
        }
        
        public static long GetMilliseconds()
        {
            DateTime dt1970 = new DateTime(1970, 1, 1);
            DateTime current = DateTime.Now;
            TimeSpan span = current - dt1970;
            return (long) span.TotalMilliseconds;
        }

    }
}
