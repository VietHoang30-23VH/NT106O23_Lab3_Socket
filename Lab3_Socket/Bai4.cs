using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Socket
{
    public partial class Bai4 : Form
    {
        public Bai4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bai4_server bai4_Server = new Bai4_server();
            bai4_Server.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bai4_client client = new Bai4_client();
            client.Show();
        }
    }
}
