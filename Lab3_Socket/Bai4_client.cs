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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Lab3_Socket
{
    public partial class Bai4_client : Form
    {
        public Bai4_client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            Connect();
        }
        IPEndPoint IP;
        Socket client;

        private void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }
        private void Send(string mess)
        {
            try
            {
                client.Send(Serialize(mess));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string mess = (string)Deserialize(data);
                    MessageBox.Show(mess);
                }
            }
            catch
            {
                client.Close();
            }


        }
        Byte[] Serialize(object obj)
        {
            MemoryStream streamable = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(streamable, obj);
            return streamable.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream streamable = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(streamable);

        }
        private string[] LayPhanTuDaChon()
        {
            List<string> phanTuDaChon = new List<string>();

            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                string phanTu = itemChecked.ToString();
                phanTuDaChon.Add(phanTu);
            }

            return phanTuDaChon.ToArray();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbphong.Items.Clear();
            string phimdachon = comboBox1.Text;
            if (phimdachon == "Đào, phở và piano")
            {
                cbphong.Items.Add("Phòng 1");
                cbphong.Items.Add("Phòng 2");
                cbphong.Items.Add("Phòng 3");

            }
            else if (phimdachon == "Mai")
            {
                cbphong.Items.Add("Phòng 2");
                cbphong.Items.Add("Phòng 3");
            }
            else if (phimdachon == "Gặp lại chị bầu")
            {
                cbphong.Items.Add("Phòng 1");
            }
            else if (phimdachon == "Tarot")
            {
                cbphong.Items.Add("Phòng 3");
            }
        }
        private void btxuatve_Click(object sender, EventArgs e)
        {
            string[] soghe = LayPhanTuDaChon();


            string soghedadat = "";
            foreach (string s in soghe)
            {
                soghedadat += s;
                soghedadat += ";";
            }
            string mess = txbhoten.Text + ";" + comboBox1.Text + ";" + cbphong.Text + ";" + soghedadat;
            Send(mess);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btxoa_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txbhoten_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cbphong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
