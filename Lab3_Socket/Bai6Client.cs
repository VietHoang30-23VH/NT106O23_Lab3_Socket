using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Lab3_Socket
{
    public partial class Bai6Client : Form
    {
        private TcpClient tcpClient;
        private NetworkStream ns;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread receiveThread;

        public Bai6Client()
        {
            InitializeComponent();
        }

        private void Bai6Client_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            txtInput.ReadOnly = true;
            btnAttach.Enabled = false;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    string message = txtInput.Text.Trim();
                    writer.WriteLine(message);
                    MessageBox.Show("Sent successfully!", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtInput.Clear();
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

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Bạn phải nhập tên để kết nối", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    this.tcpClient = new TcpClient();
                    IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
                    IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8080);
                    this.tcpClient.Connect(iPEndPoint);
                    this.ns = this.tcpClient.GetStream();
                    this.reader = new StreamReader(ns);
                    this.writer = new StreamWriter(ns);
                    writer.WriteLine(txtName.Text.Trim());
                    listBox1.Items.Add(txtName.Text.Trim());
                    btnSend.Enabled = true;
                    txtInput.ReadOnly = false;
                    btnAttach.Enabled = true;
                    receiveThread = new Thread(ReceiveMessages);
                    receiveThread.Start();

                    MessageBox.Show("Connected successfully!", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            MessageBox.Show("Bạn đã đính kèm Attachment", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadLine();
                    if (message != null)
                    {
                        AppendTextToRichTextBox(rtbClient, message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving message: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUser = listBox1.SelectedItem.ToString();
            OpenClientWindow(selectedUser);
        }

        private void OpenClientWindow(string username)
        {
            Bai6Client clientWindow = new Bai6Client();
            clientWindow.Text = username + "'s Client";
            clientWindow.Show();
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
    }
}
