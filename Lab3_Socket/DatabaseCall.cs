using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace _DatabaseCall
{
    public class DatabaseCall
    {
        private SQLiteConnection connection;
        private string connectionString;
        public DatabaseCall()
        {
            connectionString = @"Data Source=D:\HK2_2024\laptrinhmangcanban\NT106O23_Lab3_Socket\Database.db;Version=3;";
            connection = new SQLiteConnection(connectionString);
        }
        public void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

        }
        public void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();
        }
        public SQLiteDataReader ExecuteQuery(string query)
        {
            OpenConnection();
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            return cmd.ExecuteReader();
        }
    }
}
