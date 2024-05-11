using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
namespace Lab3_Socket
{
    public partial class Bai1 : Form
    {
        public Bai1()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            UDP_Server uDP_Server = new UDP_Server();
            uDP_Server.Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            UDPClient uDP_Client = new UDPClient(); 
            uDP_Client.Show();
        }
    }
}
