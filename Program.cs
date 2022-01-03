using System;

namespace ATP_Server {
    class Program {
        static void Main(string[] args) {
            DBConnection dB = new DBConnection();
            Network network = new Network();
            network.Run(dB);
        }
    }
}
