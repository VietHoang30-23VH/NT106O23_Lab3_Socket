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
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Drawing.Imaging;

namespace Lab3_Socket
{
    public partial class Bai6Client : Form
    {
        private string Username;
        private StreamWriter swSender;
        private StreamReader srReceiver;
        private TcpClient tcpServer;
        private delegate void UpdateStatusClientCallback(string Msg);
        private delegate void CloseStatusConnectionCallback(string info);
        private delegate void AddUserToListBoxCallback(string username);
        private Thread thread;
        private bool Connected;
        private IPAddress ip;
        public Bai6Client()
        {
            InitializeComponent();
            rtbClient.ReadOnly = true;
            btnDisconnect.Enabled = false;
            btnSend.Enabled = false;
            btnPrivate.Enabled = false;
            txtContent.Enabled = false;
            txtFriend.Enabled = false;
            txtMessFriend.Enabled = false;
        }
        public void StartConnection()
        {
            ip = IPAddress.Parse("127.0.0.1");
            tcpServer = new TcpClient();
            try
            {
                tcpServer.Connect(ip, 8080);
                srReceiver = new StreamReader(tcpServer.GetStream());
                swSender = new StreamWriter(tcpServer.GetStream());
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Failed to connect to server: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Connected = true;
            Username = txtName.Text;
            txtName.Enabled = false;
            txtContent.Enabled = true;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
            btnSend.Enabled = true;
            btnPrivate.Enabled = true;
            txtFriend.Enabled = true;
            txtMessFriend.Enabled = true;
            swSender.WriteLine(txtName.Text);
            swSender.Flush();

            thread = new Thread(new ThreadStart(ReceiveMessage));
            thread.Start();
            timer1.Start();
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
                StartConnection();
            else
            {
                MessageBox.Show("Bạn chưa nhập tên!", "Không thể kết nối!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }
        }
        private void UpdateStatusClient(string Msg)
        {
            rtbClient.AppendText(Msg + "\n\r");
        }

        private void CloseStatusConnection(string info)
        {
            try
            {
                if (Connected)
                {
                    rtbClient.AppendText(info + "\n\r");

                    if (listBoxClients.InvokeRequired)
                    {
                        listBoxClients.Invoke(new Action(() => listBoxClients.Items.Remove(Username)));
                    }
                    else
                    {
                        listBoxClients.Items.Remove(Username);
                    }

                    txtName.Text = string.Empty;
                    btnConnect.Enabled = true;
                    txtContent.Enabled = false;
                    btnSend.Enabled = false;
                    btnDisconnect.Enabled = false;
                    txtName.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while closing status connection: " + ex.Message);
            }
        }


        private void SendMessage()
        {
            if (txtContent.Lines.Length >= 1)
            {
                {
                    swSender.WriteLine(txtContent.Text);
                    swSender.Flush();
                }
                txtContent.Text = "";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
        private void ReceiveMessage()
        {
            try
            {
                srReceiver = new StreamReader(tcpServer.GetStream());
                string ConResponse = srReceiver.ReadLine();
                if (ConResponse[0] == '1')
                {
                    this.Invoke(new UpdateStatusClientCallback(this.UpdateStatusClient), new object[] { "Connected Successfully!" });
                }
                else
                {
                    string Reason = "Not Connected: ";
                    Reason += ConResponse.Substring(2, ConResponse.Length - 2);
                    this.Invoke(new CloseStatusConnectionCallback(this.CloseStatusConnection), new object[] { Reason });
                    return;
                }
                string message;
                while (true) // Loại bỏ kiểm tra trạng thái kết nối
                {
                    message = srReceiver.ReadLine();
                    if (message == null)
                    {
                        break; // Thoát khỏi vòng lặp nếu client đã đóng kết nối
                    }
                    this.Invoke(new UpdateStatusClientCallback(this.UpdateStatusClient), new object[] { message });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ReceiveMessages: " + ex.Message);
            }
        }

        private void btnPrivate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFriend.Text) && txtFriend.Text != txtName.Text && listBoxClients.Items.Contains(txtFriend.Text))
            {
                Server.SendPrivateMessage(txtName.Text, txtFriend.Text, txtMessFriend.Text);
                this.Invoke(new UpdateStatusClientCallback(this.UpdateStatusClient), new object[] { $"You privately sent to {txtFriend.Text}: {txtMessFriend.Text}" });
            }
            else if (txtFriend.Text == txtName.Text)
            {
                MessageBox.Show("Bạn không thể gửi cho chính mình.");
                txtFriend.Text = string.Empty;
                return;
            }
            else if (string.IsNullOrEmpty(txtMessFriend.Text) || string.IsNullOrEmpty(txtFriend.Text))
            {
                MessageBox.Show("Việc gửi tin nhắn không hợp lệ.\nVì người nhận chưa kết nối với Server hoặc chưa có nội dung gửi.", "Thông báo");
                return;
            }
            else
            {
                return;
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Server server = new Server(ip);
            List<string> connectedClients = server.GetConnectedClients();
            listBoxClients.BeginInvoke((Action)(() =>
            {
                listBoxClients.Items.Clear();
                foreach (string client in connectedClients)
                {
                    listBoxClients.Items.Add(client);
                }
            }));
        }

        private void Bai6Client_Load(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // Đặt lại trạng thái giao diện người dùng
            txtName.Text = string.Empty;
            btnConnect.Enabled = true;
            txtContent.Enabled = false;
            btnSend.Enabled = false;
            btnDisconnect.Enabled = false;
            txtName.Enabled = true;

            try
            {
                if (Connected)
                {
                    Server.SendMyMessage(Username + " đã rời khỏi cuộc trò chuyện.\n");
                }
                listBoxClients.Items.Remove(Username);
                if (srReceiver != null)
                {
                    srReceiver.Close();
                }
                if (swSender != null)
                {
                    swSender.Close();
                }
                if (tcpServer != null)
                {
                    tcpServer.Close();
                }
                Connected = false;
                // Đóng form
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while disconnecting: " + ex.Message);
            }
        }
    }
}
