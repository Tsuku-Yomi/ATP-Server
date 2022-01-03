﻿using System;
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
        private string getBestSqlStr = "DELETE FROM high_score WHERE player_name NOT IN (SELECT TOP 10 player_name FROM high_score ORDER BY score DESC";
        private MySqlCommand updateCmd;
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
            getBestCmd = new MySqlCommand(getBestSqlStr, connection);
        }

        public void UpdateScore(int highScore,string playerName) {
            updateCmd.Parameters.Clear();
            updateCmd.Parameters.AddWithValue("@score", highScore);
            updateCmd.Parameters.AddWithValue("@player_name", playerName);
            if(updateCmd.ExecuteNonQuery() == 0) {
                Console.WriteLine("Mysql插入错误");
            } else {
                Console.WriteLine("新增高分:"+playerName+"："+highScore.ToString());
            }
            getBestCmd.ExecuteNonQuery();
        }

        public void GetBest10()
    }
}