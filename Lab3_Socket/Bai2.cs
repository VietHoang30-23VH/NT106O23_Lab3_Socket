using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab3_Socket
{
    public partial class Bai2 : Form
    {
        public Bai2()
        {
            InitializeComponent();
        }
        public void StartUnsafeThread()
        {
            Socket listener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            listener.Bind(iPEndPoint);
            listener.Listen(20);
            while (true)
            {

                Socket clientSocket = listener.Accept();
                Thread receiveThread = new Thread(() =>
                {
                    while (clientSocket.Connected)
                    {
                        string text = "";
                        do
                        {
                            byte[] buffer = new byte[1];
                            clientSocket.Receive(buffer);
                            text += Encoding.UTF8.GetString(buffer);
                        } while (text[text.Length - 1] != '\n');


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
        private void btn_Click(object sender, EventArgs e)
        {
            btn.Enabled = false;
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.Start();
        }

        private void Bai2_Load(object sender, EventArgs e)
        {
            listView.View = View.Details;
            listView.Columns.Add("Message", 10000);
        }
    }
}
