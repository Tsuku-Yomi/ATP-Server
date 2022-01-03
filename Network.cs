using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ATP_Server {
    class Network {
        Socket serverSocket;
        public Network(int port=7332) {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
            serverSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ip);
            serverSocket.Listen(10);
        }

        public void Run() {

        } 
    }
}
