using System.Windows;
using System.Text;
using System.Linq;
using System.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace serverFromZero {
    class Server {       
        private TcpListener _tcpListener;
        public Action<byte[], Client> OnDataReceive;

        private readonly object _token = new object();
        public bool Running { get; set; }
        
        //Create instance of client manager.
        public ClientManager CM = new ClientManager();

        //Server consructor.
        public Server(int port) {
        try {
                _tcpListener = new TcpListener(IPAddress.Any, port);
            }
            catch(Exception e) {
                CommandLine.Write(e.Message);
            }
            startServer();
        }

        //Start server listener thread.
        public void startServer() {
            Thread serverThread = new Thread(new ThreadStart(startListening));
            serverThread.Start();
        }

        public void startListening() {
            try {             
                _tcpListener.Start();
                Running = true;
                CommandLine.Write("Started listening at " + _tcpListener.Server.LocalEndPoint);
            }
            catch(Exception e) {
                CommandLine.Write(e.Message);
            }

            while (Running)
            {
                if (_tcpListener.Pending())
                {
                    //Non-blocking method determines if there are any pending connection requests.
                    //Inserting client to list.
                    CM.AddUser(new Client(_tcpListener.AcceptTcpClient()));
                }
            }
        }

        public void StopListening() {
            try {
                Running = false;
                _tcpListener.Stop();
                CommandLine.Write("Listener stopped.");
            }
            catch(Exception e) {
                CommandLine.Write(e.Message);
            }
        }

        
        

        //private void AddClient(TcpClient newClient) {
        //    if(newClient == null) return;

        //    var client = new Client(newClient);
        //    try {
        //        _clientsDict.Add(client.IP.ToString(), client);
        //        //SendClientList(_clientsDict); //otan mpenei enas kainourios client stelnei se-
        //        ////olous tous client tin lista me tis ip pou einai sindemenes.
        //        ////Den to stelnei omos ston client p sinde8ike-
        //        ////molis twra(telefteos pou energopoihse tin addclient().
        //        ////Prepi na iparxei mia function ston kodika tou client pou zitaei-
        //        ////-apo ton server na tou stilei tin lista-
        //        ////-an den tin exei(afto 8a ginete molis sinde8ei me ton server)

        //        IncreaseClientCount();
        //        var clientThread = new Thread(HandleClient) { IsBackground = true };
        //        //clientThread.Priority = ThreadPriority.Lowest;
        //        clientThread.Start(client);
        //    }
        //    catch(Exception e) { CommandLine.Write(e.ToString()); }
        //    CommandLine.Write("A new client connected. Client count is " + _clientCount + ".");
        //}

        //private void RemoveClient(Client client) { //@todo remove key
        //    if(client == null) return;
            
        //    _clientsDict.Remove(client.IP.ToString());
            
        //    //SendClientList(_clientsDict); //stelno gia na 3eroun oi xristes oti kapios client efige wste na- 
        //                                  //-alla3oun to ip table tous .
            
        //    DecreaseClientCount();
        //    client.Close();
        //}

        //public void PrintClients() {
        //    int counter = 0;
        //    foreach(var entry in Clients) {
        //        if(entry.Value == null) return;
        //        CommandLine.Write(++counter + "- " + entry.Key + "\n");
        //    }
        //}




        public void Send(Client client, string data) {
            if(client == null || !client.Connected) return;

            var msg = new Message(data);

            try {
                client.stream.Write(msg.Data, 0, msg.Data.Length);
            }
            catch(Exception e) {
                CommandLine.Write(e.Message);
            }
        }

        public void SendAll(string data) {
            foreach(var entry in CM.users) { Send(entry, data); }
        }

        //private void HandleClient(object newClient) {
        //    var client = (Client)newClient;
        //    var currentMessage = new List<byte>();
            
        //    while(true) {
        //        var readMessage = new byte[_packetSize];
        //        int readMessageSize;

        //        try {
        //            readMessageSize = client.stream.Read(readMessage, 0, _packetSize);
        //        }
        //        catch(Exception e) {
        //            CommandLine.Write(e.Message);
        //            break;
        //        }
        //        //Console.WriteLine("readMessageSize = " + readMessageSize.ToString());
        //        if(readMessageSize <= 0) {
        //            CommandLine.Write("The client [" + client.IP + "] has closed the connection.");
        //            break;
        //        }

        //        foreach(var b in readMessage) {
        //            if(b == 0) break;

        //            if(b == 4) {
        //                OnDataReceive(currentMessage.ToArray(), client);
        //                currentMessage.Clear();
        //            }
        //            else {
        //                currentMessage.Add(b);
        //            }
        //        }
        //    }

        //    CommandLine.Write("Communication ended with client [" + client.IP + "].");
        //    RemoveClient(client);
        //}

        //private void IncreaseClientCount() {
        //    lock(_token) { _clientCount++; }
        //}

        //private void DecreaseClientCount() {
        //    lock(_token) { _clientCount--; }
        //}

        //public void InsertClientInfoData(String data, Client client) {
        //    string[] splitMsg = data.Split(',');
        //    ClientInfo ci = new ClientInfo(splitMsg[0], splitMsg[1], client.IP.ToString());
        //    CM.AddUser(splitMsg[0], ci); //insert client to dictionary   [ uname ] , [ clientInfo obj ]
        //}

        //public void SendClientList(Dictionary<String, Client> _clientsDict) {
        //    string stringWithIPs = "[IPtable]: "; //pernei tis ip pou apo8ikevonte sto dict _clientDict(dld to proto pedio) kai-
        //                                          //-to stelnei stous client. 
        //                                          //To [IPtable] mpenei prin to loop giati alios to pernaei se ka8e epanalipsi.

        //    foreach(KeyValuePair<string, Client> entry in _clientsDict) {
        //        stringWithIPs = stringWithIPs + entry.Key + ",";   // do something with entry.Value or entry.Key
                
        //    }

        //    var msg = new Message(stringWithIPs);

        //    //try {
        //    //    client.stream.Write(msg.Data, 0, msg.Data.Length);
        //    //}
        //    //catch(Exception e) {
        //    //    CommandLine.Write(e.Message);
        //    //}
        //    SendAll(stringWithIPs);
        //    Console.WriteLine("--------------");
        //    Console.WriteLine(stringWithIPs);
        //    Console.WriteLine("--------------");
        //}



       

         
        

        //public string SearchForPublicKeyInDict(String ip) { //psaxnei ip sto dict meso tis method getClientData(IP) 
        //    return clientDict.GetClientData(ip);

        //}

        //public void printClientData() {
        //    CommandLine.Write("\n>>> Printing key dictionary.");
        //    clientDict.ShowClientData();
        //}
        
        //private void GenerateKeys(int keySize) {
        //    AsymmetricEncryption.GenerateKeys(keySize, out serverPublicKey, out serverPublicAndPrivateKey);
        //}
    }
}
