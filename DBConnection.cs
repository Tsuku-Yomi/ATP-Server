using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.Buffers;
using System.IO;

namespace ATP_Server {
    class DBConnection {
        private string connStr;
        private string updateScoreSqlStr = "INSERT INTO high_score(score,player_name) VALUES (@score,@player_name);";
        private string setBestSqlStr = "DELETE FROM high_score WHERE player_name NOT IN (SELECT TOP 10 player_name FROM high_score ORDER BY score DESC)";
        private string getBestSqlStr = "SELECT TOP 10 * FROM high_score ORDER BY score DESC";
        private MySqlCommand updateCmd;
        private MySqlCommand setBestCmd;
        private MySqlCommand getBestCmd;
        private MySqlConnection connection;
        public DBConnection() {
            Stream cfgFile= File.OpenRead("./mysqlconnect.cfg");
            StreamReader reader = new StreamReader(cfgFile);
            connStr = reader.ReadToEnd();
            reader.Close();
            cfgFile.Close();
            connection = new MySqlConnection(connStr);
            try {
                connection.Open();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            updateCmd = new MySqlCommand(updateScoreSqlStr, connection);
            setBestCmd = new MySqlCommand(setBestSqlStr, connection);
            getBestCmd = new MySqlCommand(getBestSqlStr, connection);
        }

        public void UpdateScore(GameRecord gameRecord) {
            if (gameRecord.score < 0) return;
            updateCmd.Parameters.Clear();
            updateCmd.Parameters.AddWithValue("@score", gameRecord.score);
            updateCmd.Parameters.AddWithValue("@player_name", gameRecord.name);
            if(updateCmd.ExecuteNonQuery() == 0) {
                Console.WriteLine("Mysql插入错误");
            } else {
                Console.WriteLine("新增高分:"+gameRecord.name+"："+gameRecord.score.ToString());
            }
            setBestCmd.ExecuteNonQuery();
        }

        public GameRecord[] GetBest10() {
            GameRecord[] gameRecords = new GameRecord[10];
            MySqlDataReader dataReader = getBestCmd.ExecuteReader();
            for(int index = 0; index < 10; ++index) {
                dataReader.Read();
                gameRecords[index] = new GameRecord(dataReader.GetInt32("score"),dataReader.GetString("player_name"));
            }
            return gameRecords;
        }
    }
}
