using System;
using System.Collections.Generic;
using System.Text;

namespace ATP_Server {
    [Serializable]
    class GameRecord {
        GameRecord(int score, string name) {
            this.score = score;
            this.name = name;
        }
        public int score;
        public string name;
    }
}
