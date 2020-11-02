﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class tool : Form
    {
        public tool()
        {
            InitializeComponent();
        }
        int[] mau1903=new int[256]{ 0, 8388608, 32768, 8421376, 128, 8388736, 32896, 8421504, 12632256, 16711680, 65280, 16776960, 255, 16711935, 65535, 16777215, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 51, 102, 153, 204, 255, 13056, 13107, 13158, 13209, 13260, 13311, 26112, 26163, 26214, 26265, 26316, 26367, 39168, 39219, 39270, 39321, 39372, 39423, 52224, 52275, 52326, 52377, 52428, 52479, 65280, 65331, 65382, 65433, 65484, 65535, 3342336, 3342387, 3342438, 3342489, 3342540, 3342591, 3355392, 3355443, 3355494, 3355545, 3355596, 3355647, 3368448, 3368499, 3368550, 3368601, 3368652, 3368703, 3381504, 3381555, 3381606, 3381657, 3381708, 3381759, 3394560, 3394611, 3394662, 3394713, 3394764, 3394815, 3407616, 3407667, 3407718, 3407769, 3407820, 3407871, 6684672, 6684723, 6684774, 6684825, 6684876, 6684927, 6697728, 6697779, 6697830, 6697881, 6697932, 6697983, 6710784, 6710835, 6710886, 6710937, 6710988, 6711039, 6723840, 6723891, 6723942, 6723993, 6724044, 6724095, 6736896, 6736947, 6736998, 6737049, 6737100, 6737151, 6749952, 6750003, 6750054, 6750105, 6750156, 6750207, 10027008, 10027059, 10027110, 10027161, 10027212, 10027263, 10040064, 10040115, 10040166, 10040217, 10040268, 10040319, 10053120, 10053171, 10053222, 10053273, 10053324, 10053375, 10066176, 10066227, 10066278, 10066329, 10066380, 10066431, 10079232, 10079283, 10079334, 10079385, 10079436, 10079487, 10092288, 10092339, 10092390, 10092441, 10092492, 10092543, 13369344, 13369395, 13369446, 13369497, 13369548, 13369599, 13382400, 13382451, 13382502, 13382553, 13382604, 13382655, 13395456, 13395507, 13395558, 13395609, 13395660, 13395711, 13408512, 13408563, 13408614, 13408665, 13408716, 13408767, 13421568, 13421619, 13421670, 13421721, 13421772, 13421823, 13434624, 13434675, 13434726, 13434777, 13434828, 13434879, 16711680, 16711731, 16711782, 16711833, 16711884, 16711935, 16724736, 16724787, 16724838, 16724889, 16724940, 16724991, 16737792, 16737843, 16737894, 16737945, 16737996, 16738047, 16750848, 16750899, 16750950, 16751001, 16751052, 16751103, 16763904, 16763955, 16764006, 16764057, 16764108, 16764159, 16776960, 16777011, 16777062, 16777113, 16777164, 16777215 };

        private void tool_Load(object sender, EventArgs e)
        {
            int[,] mau = new int[8,256];
            Color[] mmm = new Color[256];

            for (int i = 0; i <256; i++)
            {
                byte r = (byte)(mau1903[i] >> 16);
                byte g = (byte)(mau1903[i] >> 8);
                byte b = (byte)(mau1903[i] );


                mau[0, i] = r * 256 * 256 + g * 256 + b;
                mau[1, i] = (r/2) * 256 * 256 + (g / 2) * 256 + (b / 2);
                mau[2, i] = (r / 3) * 256 * 256 + (g / 3) * 256 + (b / 3);
                mau[3, i] = (r / 4) * 256 * 256 + (g / 4) * 256 + (b / 4);
                mau[4, i] = (r / 5) * 256 * 256 + (g / 5) * 256 + (b / 5);
                mau[5, i] = (r / 6) * 256 * 256 + (g / 6) * 256 + (b / 6);
                mau[6, i] = (r / 7) * 256 * 256 + (g / 7) * 256 + (b / 7);
                mau[7, i] = (r / 8) * 256 * 256 + (g / 8) * 256 + (b / 8);
            }

            string ff = "";
            for (int j = 0; j < 8; j++)
            {
                ff = ff + "{";

                for (int i = 0; i < 256; i++)
                {
                    if (i != 255) ff = ff + mau[j, i].ToString() + ",";
                    else ff = ff + mau[j, i].ToString();
                }
                ff = ff + "},";
            }
            textBox1.Text = ff;



        }
    }
}
