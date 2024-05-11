using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Lab3_Socket
{
    public partial class UDP_Server : Form
    {
        public UDP_Server()
        {
            InitializeComponent();
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            Thread thUDPServer = new Thread(ServerThread);
            thUDPServer.Start();
        }

        private void UDP_Server_Load(object sender, EventArgs e)
        {
            listView.View = View.Details;
            listView.Columns.Add("IP Address", 100);
            listView.Columns.Add("Message", 300);
        }

        public void ServerThread()
        {
            int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("Invalid port number.");
                return;
            }
            UdpClient udpClient = new UdpClient(port);
            while (true)
            {
                IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint);
                string returnData = Encoding.UTF8.GetString(receiveBytes);
                string IPpoint = remoteIpEndPoint.Address.ToString();
                string mess =IPpoint + ":" + returnData.ToString();
                listView.BeginInvoke((MethodInvoker)delegate 
                {
                    ListViewItem item = new ListViewItem(IPpoint);
                    item.SubItems.Add(returnData);
                    listView.Items.Add(item);
                });
            }
        }

    }
}
