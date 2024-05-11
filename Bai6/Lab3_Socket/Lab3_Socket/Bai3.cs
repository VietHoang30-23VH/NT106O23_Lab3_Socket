using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Socket
{
    public partial class Bai3 : Form
    {
        public Bai3()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            TCPServer tCPServer = new TCPServer();
            tCPServer.Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            TCPClient tCPClient = new TCPClient();
            tCPClient.Show();
        }
    }
}
