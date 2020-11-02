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
    public partial class add_layer : Form
    {


        static add_layer hieuung;

        static string[] ten;
  
        


 

        public add_layer()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void add_layer_Load(object sender, EventArgs e)
        {
            combo_loai.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] aa = new string[2];
            aa[0] = tenhieung.Text;
            aa[1] = combo_loai.SelectedIndex.ToString();

            ten = aa;
            hieuung.Dispose();
              
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ten = null;
            hieuung.Dispose();
        }
    }
}
