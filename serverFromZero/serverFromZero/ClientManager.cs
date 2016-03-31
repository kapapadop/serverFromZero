using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace serverFromZero {
    class ClientManager {

        private readonly int _packetSize = 64;

        public Action<byte[], Client> OnDataReceive;

        public List<Client> users = new List<Client>();

        public void AddUser(Client client) {
            //If client is not on list insert him.
            if(!users.Contains(client)) {
                users.Add(client);
                CommandLine.Write("\n>>>User inserted to list.[OK]");

                var clientThread = new Thread(HandleClient) { IsBackground = true };
                clientThread.Start(client);
            }

        }

        public Array GetAllClientInfo() {  //metatrepei to dict se pinaka kai to kanei return
            return users.ToArray();
        }

        private void HandleClient(object newClient) {
            var client = (Client)newClient;
            var currentMessage = new List<byte>();

            while(true) {
                var readMessage = new byte[_packetSize];
                int readMessageSize;

                try {
                    readMessageSize = client.stream.Read(readMessage, 0, _packetSize);
                }
                catch(Exception e) {
                    CommandLine.Write(e.Message);
                    break;
                }
                //Console.WriteLine("readMessageSize = " + readMessageSize.ToString());
                if(readMessageSize <= 0) {
                    CommandLine.Write("The client [" + client.IP + "] has closed the connection.");
                    break;
                }

                foreach(var b in readMessage) {
                    if(b == 0) break;

                    if(b == 4) {
                        OnDataReceive(currentMessage.ToArray(), client);
                        currentMessage.Clear();
                    }
                    else {
                        currentMessage.Add(b);
                    }
                }
            }
        }
            public void PrintUsers() {
                String user;
                String publicKey;
                String ip;
                foreach(var u in users) {
                    
                    ip = u.IP.ToString();
                    Console.WriteLine("\nIp address: " + ip);
                }

            }


        }
    }
