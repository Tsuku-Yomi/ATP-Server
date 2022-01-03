using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ATP_Server {
    class Network {
        Socket serverSocket;
        public Network(int port=7332) {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
            serverSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ip);
            serverSocket.Listen(10);
        }

        public void Run(DBConnection dB) {
            while (true) {
                Socket accept = serverSocket.Accept();

                byte[] receive = new byte[100];
                accept.Receive(receive);
                dB.UpdateScore(JsonSerializer.Deserialize<GameRecord>(Encoding.Default.GetString(receive)));
                byte[] send = new byte[600];
                GameRecord[] gameRecords = dB.GetBest10();
                accept.Send(Encoding.Default.GetBytes(JsonSerializer.Serialize<GameRecord[]>(gameRecords)));

                accept.Close();
            }
        } 
    }
}
