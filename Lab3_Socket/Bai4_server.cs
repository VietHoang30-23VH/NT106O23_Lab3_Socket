using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SQLite;
using _DatabaseCall;


namespace Lab3_Socket
{
    public partial class Bai4_server : Form
    {
        private DatabaseCall DBCall;
        public Bai4_server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            DBCall = new DatabaseCall();
            Connect();

        }
        string messucess = "Bạn đã mua vé thành công";
        string messerror = "Ghế đã được đặt";
        IPEndPoint IP;
        Socket Server;
        List<Socket> clientList;
        string localmess;
        void Connect()
        {
            clientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 8080);
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            Server.Bind(IP);
            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Server.Listen(100);
                        Socket client = Server.Accept();
                        clientList.Add(client);
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Any, 8080);
                    Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }


            });
            Listen.IsBackground = true;
            Listen.Start();
        }
        private void Send(Socket client, string mess)
        {

            client.Send(Serialize(mess));
        }
        private void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string mess = (string)Deserialize(data);
                    localmess = mess;
                    Kiemtrachonguoi(client, localmess);
                }

            }
            catch
            {
                clientList.Remove(client);
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
        private string Tinhtienve(string localmess)
        {
            string[] tmp = localmess.Split(';');
            string query = "select Gia_chuan " +
                            "from Phim " +
                            "where Ten_phim='" + tmp[1] + "'";
            SQLiteDataReader reader = DBCall.ExecuteQuery(query);
            int Tongtien = 0;
            int tmptien = 0;
            int i = 3;
            int length = tmp.Length - i;
            string[] ghedachon = new string[length];
            Array.Copy(tmp, i, ghedachon, 0, length);
            while (reader.Read())
            {
                tmptien = reader.GetInt32(0);
                foreach (string s in ghedachon)
                {
                    if (s == "A1" || s == "A5" || s == "C1" || s == "C5")
                    {
                        Tongtien += tmptien / 4;
                    }
                    else if (s == "B2" || s == "B3" || s == "B4")
                    {
                        Tongtien += tmptien * 2;
                    }
                    else if (s == "A2" || s == "A3" || s == "A4" || s == "C2" || s == "C3" || s == "C4" || s == "B1" || s == "B5")
                    {
                        Tongtien += tmptien;
                    }
                    break;
                }
            }


            return Tongtien.ToString();
        }
        private void Kiemtrachonguoi(Socket client, string mess)
        {
            string[] tmp = localmess.Split(';');

            int i = 3;
            int length = tmp.Length - i;
            string[] ghedachon = new string[length];
            Array.Copy(tmp, i, ghedachon, 0, length);
            string sophong = tmp[2];
            bool outfor = true;


            foreach (string s in ghedachon)
            {
                string query = "select ID_vitringoi,Dadat " +
                          "from Vitringoi_Phong vtp,Phong " +
                          "where vtp.ID_phong=Phong.ID_phong and Ten_phong='" + sophong + "' and ID_vitringoi='" + s + "'";


                SQLiteDataReader reader = DBCall.ExecuteQuery(query);
                while (reader.Read())
                {
                    bool dadat = reader.GetBoolean(1);
                    if (dadat)
                    {
                        outfor = false;
                        reader.Close();
                        DBCall.CloseConnection();
                        break;


                    }

                }
                if (outfor == false)
                {
                    Send(client, messerror);
                    break;
                }
            }
            if (outfor == true)
            {
                foreach (string s in ghedachon)
                {
                    string query2 = "UPDATE Vitringoi_Phong " +
                "SET Dadat = 1 " +
                "WHERE ID_vitringoi = '" + s + "' " +
                "AND ID_phong = (SELECT ID_phong FROM Phong WHERE Ten_phong = '" + sophong + "');";

                    SQLiteDataReader reader = DBCall.ExecuteQuery(query2);
                }
                string ghe = "";
                foreach (string s in ghedachon)
                {
                    ghe += s;
                    ghe += " ";
                }
                string messucess2 = messucess + "\n" + "Phim: " + tmp[1] + "\n" + "Phòng: " + tmp[2]
                                + "\n" + "Vị trí ghế: " + ghe + "\n" + "Thanh toán: " + Tinhtienve(localmess) + "đ" + "\n";
                Send(client, messucess2);
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

