
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
using System.IO;

namespace Lab3_Socket
{
    public partial class Bai6Server : Form
    {
        private TCPServer server;
        private Dictionary<string, Socket> clientSockets = new Dictionary<string, Socket>();
        public Bai6Server()
        {
            InitializeComponent();
        }
        private void AppendTextToRichTextBox(RichTextBox rtb, string text)
        {
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((MethodInvoker)delegate { AppendTextToRichTextBox(rtb, text); });
            }
            else
            {
                rtb.AppendText(text + Environment.NewLine);
            }
        }
        private void AppendToChatHistory(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => AppendToChatHistory(message)));
            }
            else
            {
                rtbServer.Text += message;
            }
        }
        public void StartServer()
        {
            Socket listener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            listener.Bind(iPEndPoint);
            listener.Listen(Convert.ToInt32(txtUser.Text.Trim()));
            while (true)
            {
                Socket clientSocket = listener.Accept();
                Thread receiveThread = new Thread(() =>
                {
                    while (clientSocket.Connected)
                    {
                        try
                        {
                            byte[] buffer = new byte[3072];
                            int bytesRead = clientSocket.Receive(buffer);
                            string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            foreach (var socket in clientSockets.Values)
                            {
                                if (socket != clientSocket)
                                {
                                    socket.Send(buffer, bytesRead, SocketFlags.None);
                                }
                            }
                            AppendTextToRichTextBox(rtbServer, text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"SocketException: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                });
                string clientAddress = clientSocket.RemoteEndPoint.ToString();
                clientSockets.Add(clientAddress, clientSocket);
                receiveThread.Start();
            }
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            int integer;
            if (!string.IsNullOrEmpty(txtUser.Text) && int.TryParse(txtUser.Text.Trim(), out integer))
            {
                string text = $"Server starting ...{Environment.NewLine}";
                rtbServer.AppendText(text);
                btnListen.Enabled = false;
                Thread serverThread = new Thread(new ThreadStart(StartServer));
                serverThread.Start();

            }
            else
            {
                MessageBox.Show("Hãy nhập số lượng người dùng tối đa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Bai6Server_Load(object sender, EventArgs e)
        {

        }
    }
}