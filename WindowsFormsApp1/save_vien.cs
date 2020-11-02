using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class save_vien : Form
    {
        static save_vien hieuung;

        static string ten = "";
        string path_data = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\border\\";
        string path_ext = ".bro";
        string aaaa1 = "Chọn tên khác!";
        string aaaa2 = "Tên sơ đồ";


        public save_vien()
        {
            InitializeComponent();
        }
        public static string ShowBox(Bitmap anh, int ngonngu)
        {
            hieuung = new save_vien();
            hieuung.pictureBox1.Image = anh;
            if (ngonngu == 0)
            {
                hieuung.label18.Text = "Lưu";
                hieuung.label1.Text = "Tên";
                hieuung.button1.Text = "Chấp nhận";
                hieuung.aaaa1 = "Chọn tên khác!";
                hieuung.aaaa2 = "Tên viền";
            }
            else
            {
                hieuung.label18.Text = "Save";
                hieuung.label1.Text = "Name";
                hieuung.button1.Text = "OK";
                hieuung.aaaa1 = "Choose another name!";
                hieuung.aaaa2 = "Border name";
            }


            hieuung.ShowDialog();

            return ten;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(path_data + tenhieung.Text + path_ext))
            {
                tenhieung.Text = aaaa1;
            }
            else
            {
                if (tenhieung.Text != aaaa1 && tenhieung.Text != aaaa2)
                {

                    ten = tenhieung.Text + path_ext;
                    hieuung.Dispose();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ten = "";
            hieuung.Dispose();
        }
    }
}
