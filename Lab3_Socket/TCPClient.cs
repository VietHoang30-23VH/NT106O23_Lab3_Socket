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
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Lab3_Socket
{
    public partial class TCPClient : Form
    {
        private TcpClient tcpClient;
        private NetworkStream ns;
        public TCPClient()
        {
            InitializeComponent();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.tcpClient = new TcpClient();
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8080);
            this.tcpClient.Connect(iPEndPoint);
            this.ns = this.tcpClient.GetStream();
            btnConnect.Enabled = false;
            btnSend.Enabled = true;
            btnDisconnect.Enabled = true;
            MessageBox.Show("Connected successfully!","Annoucement",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {

                    string message = txtMessage.Text.Trim();
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(message + "\n");
                    ns.Write(data, 0, data.Length);
                    MessageBox.Show("Sent successfully!", "Annoucement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMessage.Text = "";
                }
                else
                {
                    MessageBox.Show("Client is not connected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
      
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnConnect.Enabled == false)
                {
                    NetworkStream ns = tcpClient.GetStream();
                    Byte[] data = Encoding.UTF8.GetBytes("Quit\n");
                    ns.Write(data, 0, data.Length);
                    ns.Close();
                    tcpClient.Close();
                    MessageBox.Show("Disconnected!");
                    btnConnect.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Not connected to server.");
                    btnConnect.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void TCPClient_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            btnDisconnect.Enabled = false;
            MessageBox.Show("Thông điệp kết thúc bằng .", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
