using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace serverFromZero {
    class Client {
        private TcpClient _client;
        private int _packetSize = 64;


        public Client(TcpClient client) {
            _client = client;
        }

        public NetworkStream stream {
            get { return _client.GetStream(); }
        }
        public IPAddress IP {
            get { return ((IPEndPoint)_client.Client.RemoteEndPoint).Address; }
        }

        public bool Connected {
            get { return _client.Connected; }
        }

        public void setClient(TcpClient tcpClient){
            this._client = tcpClient;
        }

       
        public void Send(string data) {
            var msg = new Message(data);

            try {
                stream.Write(msg.Data, 0, msg.Data.Length);
            }
            catch(Exception e) {
                CommandLine.Write(e.Message);
            }
        }
        public void Receive() {
            var currentMessage = new List<byte>();

            while(true) {
                var readMessage = new byte[_packetSize];
                int readMessageSize;

                try {
                    readMessageSize = stream.Read(readMessage, 0, _packetSize);
                }
                catch(Exception e) {
                    CommandLine.Write(e.Message);
                    break;
                }

                if(readMessageSize <= 0) break;         

                foreach(var b in readMessage) {
                    if(b == 0) break;

                    if(b == 4) {
                        CommandLine.Write("[SRV] : " + new ASCIIEncoding().GetString(currentMessage.ToArray()));
                        currentMessage.Clear();
                    }
                    else {
                        currentMessage.Add(b);
                    }
                }
            }
        }
        public void Close() {
            try {
                _client.Close();
                _client = null;
            }
            catch(Exception e) {
                CommandLine.Write(e.Message);
            }
        }
    }
}
