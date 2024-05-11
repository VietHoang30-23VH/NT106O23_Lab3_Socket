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
    public partial class TCPServer : Form
    {
        public TCPServer()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            btnListen.Enabled = false;
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.Start();
        }
        public void StartUnsafeThread()
        {
            Socket listener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            listener.Bind(iPEndPoint);
            listener.Listen(5);
            while (true)
            {

                Socket clientSocket = listener.Accept();
                Thread receiveThread = new Thread(() =>
                {
                    while (clientSocket.Connected)
                    {
                        string text = "Server started!";
                        do
                        {
                            byte[] buffer = new byte[1];
                            clientSocket.Receive(buffer);
                            text += Encoding.UTF8.GetString(buffer);
                        } while (text[text.Length - 1] != '.');


                        if (listView.InvokeRequired)
                        {
                            listView.Invoke((MethodInvoker)delegate
                            {
                                listView.Items.Add(text);
                            });
                        }
                    }
                });
                receiveThread.Start();
            }
        }

        private void TCPServer_Load(object sender, EventArgs e)
        {
            listView.View = View.Details;
            listView.Columns.Add("Message", 1000);
        }
    }
}

