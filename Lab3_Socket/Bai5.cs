using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Socket
{
    public partial class Bai5 : Form
    {
        static string connectionString = "Data Source=food_database.db;Version=3;";
        public Bai5()
        {
            InitializeComponent();
        }

        private void server_Click(object sender, EventArgs e)
        {
            Bai5Server bai5server = new Bai5Server();
            bai5server.Show();
        }

        private void client_Click(object sender, EventArgs e)
        {
            Bai5Client bai5client = new Bai5Client();
            bai5client.Show();
        }

        private void Bai5_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearDatabase();
        }

        private void ClearDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM Foods";
                SQLiteCommand command = new SQLiteCommand(deleteQuery, conn);
                command.ExecuteNonQuery();
            }
        }
    }
}
