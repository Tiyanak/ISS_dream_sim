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
        private Dictionary<string, Writer> writers;

        public Server()
        {
            this.writers = new Dictionary<string, Writer>();
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
                                HandleReaction(r, clientConnection);
                                data = r.ToString();
                            } catch (Exception e)
                            {
                                Console.WriteLine("Cant deserialize that.");
                            }

                            Console.WriteLine("Received msg: " + data);
                           
                            break;

                        case NetIncomingMessageType.StatusChanged:

                            switch (msg.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:

                                    string clientConnectedId = msg.SenderConnection.RemoteUniqueIdentifier.ToString();
                                    string hailMessage = msg.SenderConnection.RemoteHailMessage.ReadString();                   

                                    Console.WriteLine("Client connected: " + clientConnectedId + " : " + hailMessage);

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

        private void HandleReaction(Reaction reaction, NetConnection clientConnection)
        {

            switch (reaction.msgType)
            {
                case 0: // start task
                    long id = DateTime.Now.Ticks;
                    Writer writer = new Writer(reaction.taskType + "_" + id.ToString() + ".csv");
                    writer.writeLine(reaction.getFieldsCSV());
                    this.writers[id.ToString()] = writer;
                    break;

                case 1: // end task
                    this.writers[reaction.taskId.ToString()].closeFile();
                    break;

                case 2: // reaction msg
                    this.writers[reaction.taskId.ToString()].writeLine(reaction.toCSV());
                    // izracun parametara iz reakcije igraca sa strane klijenta
                    double targetDisplayTime = 0;
                    double cueToTargetTime = 0;
                    double threshold = 0;
                    SendMsg(new Parameters(reaction.taskId, 2, targetDisplayTime, cueToTargetTime, threshold).serialize(), clientConnection);

                    break;

                default:
                    Console.WriteLine("Reaction message: ", reaction.ToString());
                    break;
            }

        }

    }
}
