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
    public partial class thongtin : Form
    {
        static thongtin hieuung;

        static string ten = "";

 

        public static string ShowBox( int n_ngu,int l_loi,int c_dai,int c_rong,int p_1,int p_2)
        {
            hieuung = new thongtin();
           
     
            
            if(n_ngu == 0)
            {
                hieuung.label18.Text= "Thông tin";
                hieuung.label1.Text = "- Led lỗi: "+ l_loi.ToString() + " led";
                hieuung.label2.Text = "- Chiều rộng: " + c_dai.ToString()+" mm";
                hieuung.label3.Text = "- Chiều cao:" + c_rong.ToString() + " mm";
                hieuung.label4.Text = "- Port 1: " + p_1.ToString() + " led";
                hieuung.label5.Text = "- Port 2: " + p_2.ToString() + " led";
                hieuung.button1.Text = "Chấp nhận";

            }else
            {

                hieuung.label18.Text = "Info";
                hieuung.label1.Text = "- Led error: " + l_loi.ToString() + " led";
                hieuung.label2.Text = "- Width: " + c_dai.ToString() + " mm";
                hieuung.label3.Text = "- Height:" + c_rong.ToString() + " mm";
                hieuung.label4.Text = "- Port 1: " + p_1.ToString() + " led";
                hieuung.label5.Text = "- Port 2: " + p_2.ToString() + " led";
                hieuung.button1.Text = "Ok";

            }







                hieuung.ShowDialog();

            return ten;
        }


        public thongtin()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ten = "";
            hieuung.Dispose();
        }
    }
}
