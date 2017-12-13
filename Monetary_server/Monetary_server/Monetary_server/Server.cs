using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

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
            this.config = new NetPeerConfiguration("Monetary_server") { Port = this.port };
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
                Console.WriteLine("Server started!");
                msgListener();
                
            }
        }

        public void msgListener()
        {
            while (this.server.Status == NetPeerStatus.Running)
            {
                while ((msg = this.server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("Data: " + msg.ReadString());
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            switch (msg.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    Console.WriteLine("Client status: Connected");
                                    break;

                                case NetConnectionStatus.Disconnected:
                                    Console.WriteLine("Client status: Disconnected");
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

    }
}
