﻿using System;
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
    public partial class save_haoquang : Form
    {


        static save_haoquang hieuung;

        static string ten = "";
        string path_data = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\halo\\";
        string path_ext = ".hal";
        string aaaa1 = "Chọn tên khác!";
        string aaaa2 = "Tên hào quang";

        public static string ShowBox(Bitmap anh, int ngonngu)
        {
            hieuung = new save_haoquang();
            hieuung.pictureBox1.Image = anh;
            if (ngonngu == 0)
            {
                hieuung.label18.Text = "Lưu";
                hieuung.label1.Text = "Tên";
                hieuung.button1.Text = "Chấp nhận";
                hieuung.aaaa1 = "Chọn tên khác!";
                hieuung.aaaa2 = "Tên hào quang";
            }
            else
            {
                hieuung.label18.Text = "Save";
                hieuung.label1.Text = "Name";
                hieuung.button1.Text = "OK";
                hieuung.aaaa1 = "Choose another name!";
                hieuung.aaaa2 = "Halo name";
            }


            hieuung.ShowDialog();

            return ten;
        }

        public save_haoquang()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ten = "";
            hieuung.Dispose();
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
    }
}
