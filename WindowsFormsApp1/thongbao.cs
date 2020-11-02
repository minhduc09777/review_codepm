using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class thongbao : Form
    {
        static thongbao hieuung;

        static string ten = "";

        public static string ShowBox(String te, int ngonngu)
        {
            hieuung = new thongbao();
          
            if (ngonngu == 0)
            {
                hieuung.label18.Text = "Thông báo";
                hieuung.label1.Text = te;
                hieuung.button1.Text = "Chấp nhận";
               
            }
            else
            {
                hieuung.label18.Text = "Info";
                hieuung.label1.Text = te;
                hieuung.button1.Text = "OK";
           
            }


            hieuung.ShowDialog();

            return ten;
        }

        public thongbao()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ten = "OK";
            hieuung.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ten = "";
            hieuung.Dispose();
        }
    }
}
