using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

namespace serverFromZero {
    class Program {
        private static Server _server;
        
        public const string who = "<Who>";
        public const string uname = "<uname>";
        public const string pKey = "<pKey>";
        public const string OK = "<OK>";
        public const string Exists = "<Exists>";
        public const string NotExists = "<NotExists>";
        public const string IsAvailable = "<IsAvailable>";
        public const string Send = "<Send>";
        public const string Received = "<Received>";



        static void Main(string[] args) {
            Console.WriteLine("USAGE:\n---------------------------------------\n -s Start as server\n -c Start as client and connect to a server\n -p If running as server use -p to print clients\n");
            while(true) {
                Console.Write(":>>");

                string input = Console.ReadLine();
                switch(input) {
                    case "-s": {
                            //Console.Write("Choose port number, between [1000-65535]: ");
                            //string _port = Console.ReadLine();
                            int _portNumber = 2222;
                            //Int32.TryParse(_port, out _portNumber);

                            if(_portNumber > 1000 || _portNumber <= 65535) {
                                _server = new Server(_portNumber);
                                _server.OnDataReceive += OnReceive;
                                //new Thread(_server.startListening).Start(); //petaei error on the run malon giati anigo 2 tcplistener threads @@TODO
                            }
                            else {
                                Console.WriteLine("Port not valid.");
                                continue;
                            }
                        }
                        break;
                    case "-p": {
                            if(_server != null) {
                                
                                //foreach(var entry in _server.Clients) {
                                //    if(entry.Value == null) return;
                                //    CommandLine.Write(++counter + "- " + entry.Key + "\n");
                                //}
                                //_server.PrintClients();
                                //_server.printClientData();
                                ClientManager cm = new ClientManager();
                                _server.CM.PrintUsers();
                              
                            }
                            else {
                                Console.WriteLine("No clients connected.");
                                continue;
                            }
                        }
                        break;
                    case "-r": {


                        } break;
                    //case "-c": {
                    //        //if(_server == null) return;
                    //    Console.Title = "Client";

                    //    var client = new TcpClient();
                    //    //ip adress below will be taken by user input after tests.
                    //    var serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.7"), 2222);
                    //    try {
                    //        client.Connect(serverEndPoint);
                    //    }
                    //    catch(SocketException se) {
                    //        Console.WriteLine(se.ToString());
                    //        continue;
                    //    }
                    //    _client = new Client(client);
                    //    new Thread(_client.Receive).Start();
                    //    }
                    //    break;
                    default: {
                            //if(_client != null) // if user is a client.
                            //    {
                            //    _client.Send(input);
                            //}
                            if(_server != null) // if user is the server.
                            {
                                _server.SendAll(input); //ama o server grapsei kati sti consola to stelnei se olous 
                            }
                        }
                        break;
                }
            }
        }
        private static void OnReceive(byte[] data, Client client) {
            string msg = new ASCIIEncoding().GetString(data);
            //if(msg.Contains("[info]")) {                                //o client stelnei data me tag uname kovo to [data] stelno sto upolipo

            //    _server.InsertClientInfoData(msg.Substring(6), client);                 //@@@@@Prosoxi kovi sosta to string??
            //}
            //else if(msg.Contains("[UserList]")) {
            //    Array usersInfo = _server.CM.GetAllClientInfo();
            //    Console.WriteLine();
            //    //_server.Send(usersInfo);
            //    _server.testSerialization();
            //}
            //else {
                _server.SendAll(msg);
                //if(msg == "requestClientList") {
                //    Console.WriteLine("DO");
                //}
            }
        }
    }



