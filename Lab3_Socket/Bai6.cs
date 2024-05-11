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
    public partial class Bai6 : Form
    {
        public Bai6()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            Bai6Server bai6Server = new Bai6Server();
            bai6Server.Show();  
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            Bai6Client bai6Client = new Bai6Client();   
            bai6Client.Show();  
        }
    }
}
