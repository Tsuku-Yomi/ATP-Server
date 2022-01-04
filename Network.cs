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
            try {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
                serverSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(ip);
                serverSocket.Listen(10); 
            } catch(Exception e) {
                Console.WriteLine(e);
            }
        }

        public void Run(DBConnection dB) {
            Console.WriteLine("成功开启，正在监听");
            while (true) {
                Socket accept = serverSocket.Accept();
                Console.WriteLine("获取新连接");
                byte[] receive = new byte[1000];
                int len=
                accept.Receive(receive);
#if DEBUG
                Console.WriteLine(Encoding.ASCII.GetString(receive, 0, len));
#endif
                dB.UpdateScore(JsonSerializer.Deserialize<GameRecord>(Encoding.ASCII.GetString(receive,0,len)));
                GameRecord[] gameRecords = dB.GetBest10();
                accept.Send(Encoding.ASCII.GetBytes(JsonSerializer.Serialize<GameRecord[]>(gameRecords)));
                Console.WriteLine("连接传输结束，正在关闭");
                accept.Close();
            }
        } 
    }
}
