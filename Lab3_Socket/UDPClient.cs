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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace Lab3_Socket
{
    public partial class UDPClient : Form
    {
        public UDPClient()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIP.Text) || string.IsNullOrEmpty(txtPort2.Text) || string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("Nội dung bị thiếu", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
            else
            {
                UdpClient uDPClient = new UdpClient();
                Byte[] sendBytes = Encoding.UTF8.GetBytes(txtMessage.Text.Trim());
                string ipAddress = txtIP.Text;
                int SendPoint = Convert.ToInt32(txtPort2.Text);
                uDPClient.Send(sendBytes, sendBytes.Length,ipAddress,SendPoint);
                MessageBox.Show("Gửi thành công!");
            }
        }
    }
}
