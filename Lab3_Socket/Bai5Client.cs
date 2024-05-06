using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab3_Socket
{
    public partial class Bai5Client : Form
    {
        Socket clientSocket;
        bool isConnected = false;
        public Bai5Client()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 8080);
                isConnected = true;
                MessageBox.Show("Connected", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connect error: " + ex.Message, "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnContribute_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Please connect first", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFood.Text) || string.IsNullOrWhiteSpace(txtContributor.Text))
            {
                MessageBox.Show("Please fill in both food and contributor fields", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string message = "ContributeFood;" + txtFood.Text + ";" + txtContributor.Text;
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await clientSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                txtFood.Clear();
                txtContributor.Clear();
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

                // Mở kết nối mới
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 8080);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending message: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnGroup_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Please connect first", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Gửi yêu cầu để lấy món ăn ngẫu nhiên từ cộng đồng
                string message = "Randomcongdong;";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await clientSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                // Nhận và hiển thị món ăn từ Server
                string randomFoodFromCommunity = await ReceiveResponseAsync();
                textBox3.Text = randomFoodFromCommunity;
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

                // Mở kết nối mới
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 8080);
            }
            catch
            {
                MessageBox.Show("Không thể gửi tin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSelf_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Please connect first", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                // Gửi yêu cầu để lấy món ăn do người đóng góp đóng góp
                string message = "Randomdonggop;" + txtContributor.Text;
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await clientSocket.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                // Nhận và hiển thị món ăn từ Server
                string randomFoodFromContributor = await ReceiveResponseAsync();
                textBox3.Text = randomFoodFromContributor;
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

                // Mở kết nối mới
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 8080);
            }
            catch
            {
                MessageBox.Show("Không thể gửi tin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> ReceiveResponseAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return text;
        }
    }
}
