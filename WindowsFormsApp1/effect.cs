using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DXFLib;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Drawing.Text;
using UsbHid;
using UsbHid.USB.Classes.Messaging;


namespace WindowsFormsApp1
{
    public partial class effect : Form
    {
        public effect()
        {
            InitializeComponent();
        }

        private Color[] color_layer0 = new Color[19]{

                                                      Color.FromArgb(200,132, 213, 164),
                                                      Color.FromArgb(200,189, 89, 203),
                                                      Color.FromArgb(200,206, 169, 83),
                                                      Color.FromArgb(200,145, 212, 75),
                                                      Color.FromArgb(200,205, 91, 137),
                                                      Color.FromArgb(200,121, 126, 203),
                                                      Color.FromArgb(200,86, 117, 57),
                                                      Color.FromArgb(200,114, 66, 47),
                                                      Color.FromArgb(200,93, 55, 98),
                                                      Color.FromArgb(200,250, 237, 111),
                                                      Color.FromArgb(200,204, 235, 197),
                                                      Color.FromArgb(200,188, 128, 189),
                                                      Color.FromArgb(200,252, 205, 229),
                                                      Color.FromArgb(200,128,177, 211),
                                                      Color.FromArgb(200,253, 180,98),
                                                      Color.FromArgb(200,178, 222, 105),
                                                      Color.FromArgb(200,251, 128, 114),
                                                      Color.FromArgb(200,190, 186, 218),
                                                      Color.FromArgb(200,141, 211, 199),
        };
        List<LED> led_full = new List<LED>();
        List<LED1> led_full1 = new List<LED1>();
        List<int> led_select = new List<int>();
        List<ANH_ANH> list_anh = new List<ANH_ANH>();
        float zoom = 1F;
        float W0 = 9984;
        float H0 = 9984;
        float X0 = 4992;
        float Y0 = 4992;
        int startx = 0;                         // offset of image when mouse was pressed
        int starty = 0;
        int imgx = -4992;                         // current offset of image
        int imgy = -4992;
        System.Drawing.Point mouseDown;
        PointF vt_chuot = new PointF(-1, -1);
        PointF vt_chuot1 = new PointF(-1, -1);
        bool mousepressed = false;
        int vt_port2 = -1;

        int vt_menu = 0;
        int menu_chinh = 0;

        int menu_effect_wire = 1;
        int menu_effect_edit = 2;
        int menu_effect_add = 3;

        int menu_vien_wire = 4;
        int menu_vien_edit = 5;
        int menu_vien_add = 6;

        int menu_text_wire = 7;
        int menu_text_edit = 8;

        int menu_thucong_wire = 9;
        int menu_thucong_edit = 10;

        int menu_vay_wire = 11;
        int menu_vay_edit = 12;

        int menu_haoquang_wire = 13;
        int menu_haoquang_edit = 14;
        int menu_haoquang_add = 15;
        int menu_nap= 16;


        int vt_hieuung = -1;

        int hieuung_effect = 0;
        int hieuung_text = 1;
        int hieuung_vien = 2;
        int hieuung_vay = 3;
        int hieuung_thucong = 4;
        int hieuung_haoquang = 5;

        string path_hieuung = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\effect\\";
        List<string> list_effect = new List<string>();
        List<string> list_path_effect = new List<string>();
        List<string> list_vien = new List<string>();
        List<string> list_halo = new List<string>();
        private void hien_list_path_effect()
        {

            com_path_effect.Items.Clear();
            list_path_effect.Clear();
            foreach (string pat in Directory.GetDirectories(path_hieuung))
            {


                com_path_effect.Items.Add(Path.GetFileName(pat));
                list_path_effect.Add(pat);



            }
            if (com_path_effect.Items.Count > 0)
            {
                com_path_effect.SelectedIndex = 0;
                hien_list_effect();
            }

        }

        int vt_chon_effect = -1;
        int vt_hien_effect = 0;
        int max_rong_effect = 2;
        int max_cao_effect = 5;
        int max_hien_effect = 0;

        Bitmap anh_doc;
        private void hien_list_effect()
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            panel_wail.Visible = true;
            Application.DoEvents();

            list_effect.Clear();
            if (com_path_effect.Items.Count > 0 && com_path_effect.SelectedIndex >= 0 && com_path_effect.SelectedIndex < com_path_effect.Items.Count)
            {
                foreach (string link in Directory.GetFiles(list_path_effect[com_path_effect.SelectedIndex], "*.eff"))
                {
                    list_effect.Add(link);


                    if (File.Exists(list_path_effect[com_path_effect.SelectedIndex] + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp") == false)
                    {

                        docanh = File.ReadAllBytes(list_path_effect[com_path_effect.SelectedIndex] + "\\" + Path.GetFileNameWithoutExtension(link) + ".eff");
                        Mahoa_moi.giaimaok(ref docanh);



                        int tonga = docanh[328 + 0] + docanh[328 + 1] * 256 + docanh[328 + 2] * 256 * 256 + docanh[328 + 3] * 256 * 256 * 256;
                        int wwa = docanh[328 + 4] + docanh[328 + 5] * 256 + docanh[328 + 6] * 256 * 256 + docanh[328 + 7] * 256 * 256 * 256;
                        int hha = docanh[328 + 8] + docanh[328 + 9] * 256 + docanh[328 + 10] * 256 * 256 + docanh[328 + 11] * 256 * 256 * 256;
                        int vtt = tonga / 3;
                        try
                        {
                            Bitmap ss = new Bitmap(wwa, hha);
                            for (int x1 = 0; x1 < wwa; x1++)
                            {
                                for (int y1 = 0; y1 < hha; y1++)
                                {
                                    // ss.SetPixel(x1, y1, Color.FromArgb(255,0,0));
                                    // ss.SetPixel(x1, y1, mamau[docanh[vtt * wwa * hha + y1 * 4 + hha * x1 * 4 + 12]]);

                                    if (12 + vtt * wwa * hha * 4 + x1 * hha * 4 + y1 * 4 > 0) ss.SetPixel(x1, y1, Color.FromArgb(docanh[328 + 12 + vtt * wwa * hha * 4 + x1 * hha * 4 + y1 * 4 + 1], docanh[328 + 12 + vtt * wwa * hha * 4 + x1 * hha * 4 + y1 * 4 + 2], docanh[328 + 12 + vtt * wwa * hha * 4 + x1 * hha * 4 + y1 * 4 + 3]));
                                }
                            }


                            anh_doc = (Bitmap)ResizeImage((Image)ss, 82, 60);
                            // e.Graphics.DrawImage(aaaa, new Rectangle(1 + 82 * y, 1 + 75 * x, aaaa.Width, aaaa.Height), new Rectangle(0, 0, aaaa.Width, aaaa.Height), GraphicsUnit.Pixel);
                            anh_doc.Save(list_path_effect[com_path_effect.SelectedIndex] + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp");

                            //pictureBox9.Image = ss;
                            // Application.DoEvents();
                        }
                        catch (Exception ex)
                        {

                        }
                        /*
                        Mahoa_moi.mahoaok(ref docanh);
                        FileStream fWrite = new FileStream(list_path_effect[com_path_effect.SelectedIndex] + "\\" + Path.GetFileNameWithoutExtension(link) + ".eff", FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.None);
                        fWrite.Write(docanh, 0, docanh.Length);
                        fWrite.Close();
                        */
                        docanh = null;
                    }

                }
            }
            int ddd = max_cao_effect * max_rong_effect;
            max_hien_effect = (list_effect.Count - ddd) / max_rong_effect;
            if ((list_effect.Count - ddd) % max_rong_effect > 0) max_hien_effect++;
           
            vScrollBar1.Value = 0;
            if (max_hien_effect >= 1) vScrollBar1.Maximum = max_hien_effect+9;
            else vScrollBar1.Maximum = 1;

           

            //  MessageBox.Show(vScrollBar1.Maximum.ToString());
            vt_chon_effect = -1;
            vt_hien_effect = 0;
            pic_hu_nen.Refresh();
            panel_wail.Visible = false; ;
            timer.Enabled = true;
            timer_map.Enabled = true;
        }

        private byte[] Encrypt_cu(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("Linh_an", new byte[] { 0x13, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78, 0x89, 0x90, 0x01 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }
        private byte[] Decrypt_cu(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("Linh_an", new byte[] { 0x13, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78, 0x89, 0x90, 0x01 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(input, 0, input.Length);

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {

                byte[] nn = new byte[0];
                return nn;
            }
            return ms.ToArray();
        }
        private byte[] Encrypt_moi(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("chanqua", new byte[] { 0x65, 0x25, 0x38, 0x76, 0x89, 0x11, 0x12, 0x77, 0x88, 0x09 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }
        private byte[] Decrypt_moi(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("chanqua", new byte[] { 0x65, 0x25, 0x38, 0x76, 0x89, 0x11, 0x12, 0x77, 0x88, 0x09 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(input, 0, input.Length);

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {

                byte[] nn = new byte[0];
                return nn;
            }
            return ms.ToArray();
        }
        int vt_chon_halo = -1;
        int vt_hien_halo = 0;
        int max_rong_halo = 2;
        int max_cao_halo = 5;
        int max_hien_halo = 0;
        int haoquang_tia = 0;
        int haoquang_hang = 0;
        string path_halo = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\halo\\";
        List<Color[,]> list_color_haoquang = new List<Color[,]>();
        List<Color[,]> list_color_haoquang_play = new List<Color[,]>();
        List<int[]> list_set_haoquang = new List<int[]>();
        private void hien_list_halo()
        {
            panel_wail.Visible = true;
            Application.DoEvents();

            /*
            foreach (string link in Directory.GetFiles(path_halo, "*.hau"))
            {
                docanh = File.ReadAllBytes(link);
                docanh = Decrypt_cu(docanh);
                halo_tia = docanh[0] + docanh[1] * 256 + docanh[2] * 256 * 256 + docanh[3] * 256 * 256 * 256;
                halo_hang = docanh[4] + docanh[5] * 256 + docanh[6] * 256 * 256 + docanh[7] * 256 * 256 * 256;


                halo_color.Clear();
                halo_set.Clear();


                int tt1 = docanh[12] + docanh[13] * 256 + docanh[14] * 256 * 256 + docanh[15] * 256 * 256 * 256;

                //MessageBox.Show(tt1.ToString());
                int dem = 16;

                for (int i = 0; i < tt1; i++)
                {
                    Color[,] temp = new Color[halo_tia, halo_hang];

                    for (int x = 0; x < halo_tia; x++)
                    {
                        for (int y = 0; y < halo_hang; y++)
                        {
                            if (docanh[dem] == 0)
                            {
                                temp[x,y] =Color.Transparent;
                            }
                            else
                            {
                                temp[x, y] = Color.FromArgb(docanh[dem], docanh[dem + 1], docanh[dem + 2], docanh[dem + 3]);
                            }


                            dem = dem + 4;
                        }
                    }


                    halo_color.Add(temp);
                }

                for (int i = 0; i < tt1; i++)
                {
                    int[] temp = new int[8];
                    for (int j = 0; j < 8; j++)
                    {
                        temp[j] = docanh[dem] + docanh[dem + 1] * 256 + docanh[dem + 2] * 256 * 256 + docanh[dem + 3] * 256 * 256 * 256; dem = dem + 4;
                    }
                    halo_set.Add(temp);
                     
                }

                HAOQUANG temp1 = new HAOQUANG(halo_tia, halo_hang, halo_color.Count, halo_color, halo_set);
                var rmCrypto = GetAlgorithm();
                ICryptoTransform encryptor = rmCrypto.CreateEncryptor();

                using (var writer = new StreamWriter(new CryptoStream(System.IO.File.Create(path_halo   + "\\" + Path.GetFileNameWithoutExtension(link) + ".hal"), encryptor, CryptoStreamMode.Write)))
                {
                    writer.Write(JsonConvert.SerializeObject(temp1));

                }


            }
            */

            list_halo.Clear();

            foreach (string link in Directory.GetFiles(path_halo, "*.hal"))
            {
                list_halo.Add(link);


                if (File.Exists(path_halo + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp") == false)
                {


                    var rmCrypto = GetAlgorithm();

                    ICryptoTransform decryptor = rmCrypto.CreateDecryptor();
                    try
                    {
                        using (var reader = new StreamReader(new CryptoStream(System.IO.File.OpenRead(link), decryptor, CryptoStreamMode.Read)))
                        {

                            HAOQUANG temp = JsonConvert.DeserializeObject<HAOQUANG>(reader.ReadToEnd());

                            Color[,] temp_chay = new Color[temp.TIA, temp.HANG];
                            for (int i = 0; i < temp.TONG; i++)
                            {
                                for (int x = 0; x < temp.TIA; x++)
                                {
                                    for (int y = 0; y < temp.HANG; y++)
                                    {
                                        if (temp.MAU[i][x,y] != Color.Transparent) temp_chay[x,y] = temp.MAU[i][x, y];
                                    }
                                }
                            }

                            Bitmap aa = new Bitmap(temp.TIA, temp.HANG);
                            Graphics gg = Graphics.FromImage(aa);

                            gg.Clear(Color.Black);


                            PointF center = new PointF(0, 0);

                            for (int x = 0; x < temp.TIA; x++)
                            {
                                for (int y = 0; y < temp.HANG; y++)
                                {

                                    //float goc = (float)GetAngle(center, new PointF((float)x, y));



                                    //int  zx = (int)chuyendoiF((float)(goc * (tia / 2) / 180), (float)hang, (float)tia);

                                    // int zy = (int)chuyendoiF(chieudaiF(center, new PointF((float)x, (float)y)), hang / 2, hang);
                                    gg.FillEllipse(new SolidBrush(temp_chay[x, y]), new RectangleF(x, y, 2, 2));


                                }
                            }
                            gg.Dispose();
                            aa = (Bitmap)ResizeImage(aa, 80, 60);
                            aa.Save(path_halo + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp");
                        }
                    }
                    catch
                    {

                    }
 
 

                    docanh = null;
                }

            }

            int ddd = max_cao_halo * max_rong_halo;
            max_hien_halo = (list_halo.Count - ddd) / max_rong_halo;
            if ((list_halo.Count - ddd) % max_rong_halo > 0) max_hien_halo++;
            button28.Enabled = false;
            vScrollBar3.Value = 0;
            if (max_hien_halo >= 1) vScrollBar3.Maximum = max_hien_halo+9;
            else vScrollBar3.Maximum = 1;
            vt_chon_halo = -1;
            vt_hien_halo = 0;
            pic_hu_halo.Refresh();
            panel_wail.Visible = false; ;
        }
        public Color ddmau0(Color color)
        {

            var temp = new HSV();
            temp.h = color.GetHue();
            temp.s = color.GetSaturation();
            temp.v = color.GetBrightness();
            temp.h = temp.h + (360 / (float)haoquang_tia);
            //  temp.v = temp.v - giammau * 0.01F;
            // if (temp.v <= 0) temp.v = 0;

            return ColorFromHSL(temp);


        }
        public Color ddmauhq1(Color color, int tia, int hang)
        {

            var temp = new HSV();
            temp.h = color.GetHue();
            temp.s = color.GetSaturation();
            temp.v = color.GetBrightness();
            temp.h = temp.h + (360 / (float)tia);
            //  temp.v = temp.v - giammau * 0.01F;
            // if (temp.v <= 0) temp.v = 0;

            return ColorFromHSL(temp);


        }
        private List<Color> doimau1(List<Color> ddd)
        {

            for (int x = 0; x < ddd.Count; x++) if (ddd[x] != Color.Transparent) ddd[x] = ddmau0(ddd[x]);

            return ddd;
        }
        private List<Color> doimau_haoquang(List<Color> ddd,int tia, int hang)
        {

            for (int x = 0; x < ddd.Count; x++) if (ddd[x] != Color.Transparent) ddd[x] = ddmauhq1(ddd[x], tia,hang);

            return ddd;
        }
        private  Color[,] doimau_haoquang_moi(Color[,] ddd, int tia, int hang)
        {
            for (int x = 0; x < tia; x++)
            {
                for (int y = 0; y < hang; y++)
                {
                    if (ddd[x, y] != Color.Transparent) ddd[x, y] = ddmauhq1(ddd[x, y], tia, hang);
                }
            }

        

            return ddd;
        }
        private List<Color> doimau2(List<Color> ddd)
        {

            for (int x = 0; x < ddd.Count; x++) if (ddd[x] != Color.Transparent) ddd[x] = ddmau1(ddd[x]);

            return ddd;
        }
        public Color ddmau1(Color color)
        {

            var temp = new HSV();
            temp.h = color.GetHue();
            temp.s = color.GetSaturation();
            temp.v = color.GetBrightness();
            temp.h = temp.h - (360 / (float)haoquang_tia);
            //  temp.v = temp.v - giammau * 0.01F;
            // if (temp.v <= 0) temp.v = 0;

            return ColorFromHSL(temp);


        }
        private List<Color> toidan(List<Color> ddd,int tia, int hang)
        {


            for (int x = 0; x < ddd.Count; x++)
            {
                if (ddd[x].A > 0)
                {
                    int aa = ddd[x].A;
                    aa = aa - ((255 /tia));
                    if (aa <= 0) aa = 255;
                    ddd[x] = Color.FromArgb(aa, ddd[x].R, ddd[x].G, ddd[x].B);
                }
            }


            return ddd;
        }

        private Color[,] toidan_moi(Color[,] ddd, int tia, int hang)
        {
            for (int x = 0; x < tia; x++)
            {
                for (int y = 0; y < hang; y++)
                {
                    if (ddd[x, y].A > 0)
                    {
                        int aa = ddd[x, y].A;
                        aa = aa - ((255 / tia));
                        if (aa <= 0) aa = 255;
                        ddd[x, y] = Color.FromArgb(aa, ddd[x, y].R, ddd[x, y].G, ddd[x, y].B);
                    }
                }
            }


 


            return ddd;
        }
        private Color[,] sangdan_moi(Color[,] ddd, int tia, int hang)
        {

            for (int x = 0; x < tia; x++)
            {
                for (int y = 0; y < hang; y++)
                {
                    if (ddd[x, y].A > 0)
                    {
                        if (ddd[x, y].A >= 254)ddd[x, y] = Color.FromArgb(0, ddd[x, y].R, ddd[x, y].G, ddd[x, y].B);
                        int aa = ddd[x, y].A;
                        aa = aa + ((255 / (int)tia));
                        if (aa <= 0) aa = 255;
                        if (aa >= 256) aa = 255;
                        ddd[x, y] = Color.FromArgb(aa, ddd[x, y].R, ddd[x, y].G, ddd[x, y].B);
                    }
                }
            }


           


            return ddd;
        }
        private List<Color> sangdan(List<Color> ddd, int tia, int hang)
        {

            for (int x = 0; x < ddd.Count; x++)
            {
                if (ddd[x].A > 0)
                {

                    if (ddd[x].A >= 254) ddd[x] = Color.FromArgb(0, ddd[x].R, ddd[x].G, ddd[x].B);
                    int aa = ddd[x].A;
                    aa = aa + ((255 / (int)tia));
                    if (aa <= 0) aa = 255;
                    if (aa >= 256) aa = 255;
                    ddd[x] = Color.FromArgb(aa, ddd[x].R, ddd[x].G, ddd[x].B);
                }
            }


            return ddd;
        }
        private List<Color> toara(List<Color> ddd, int tia, int hang)
        {
            // Color[] temp = new Color[haoquang_hang];

            List<List<Color>> ttt = new List<List<Color>>();


            for (int x = 0; x < tia; x++)
            {
                List<Color> t1 = new List<Color>();
                for (int y = 0; y < hang; y++)
                {
                    t1.Add(ddd[x * hang + y]);
                }
                ttt.Add(t1);
            }
            List<Color> t0 = ttt[ttt.Count - 1];

            ttt.RemoveAt(ttt.Count - 1);
            ttt.Insert(0, t0);

            List<Color> ok = new List<Color>();

            for (int x = 0; x <tia; x++)
            {

                for (int y = 0; y <hang; y++)
                {
                    ok.Add(ttt[x][y]);
                }

            }

            return ok;
        }
        private List<Color> toavao(List<Color> ddd, int tia, int hang)
        {
            // Color[] temp = new Color[haoquang_hang];

            List<List<Color>> ttt = new List<List<Color>>();


            for (int x = 0; x < tia; x++)
            {
                List<Color> t1 = new List<Color>();
                for (int y = 0; y < hang; y++)
                {
                    t1.Add(ddd[x * hang + y]);
                }
                ttt.Add(t1);
            }
            List<Color> t0 = ttt[0];

            ttt.RemoveAt(0);
            ttt.Insert(ttt.Count, t0);

            List<Color> ok = new List<Color>();

            for (int x = 0; x <tia; x++)
            {

                for (int y = 0; y <hang; y++)
                {
                    ok.Add(ttt[x][y]);
                }

            }

            return ok;
        }
        private List<Color> chaytoi(List<Color> ddd, int tia, int hang)
        {
            List<List<Color>> ttt = new List<List<Color>>();


            for (int x = 0; x <tia; x++)
            {
                List<Color> t1 = new List<Color>();
                for (int y = 0; y < hang; y++)
                {
                    t1.Add(ddd[x *hang + y]);
                }
                ttt.Add(t1);
            }

            for (int x = 0; x < tia; x++)
            {
                List<Color> t1 = ttt[x];

                Color c = t1[0];
                t1.RemoveAt(0);
                t1.Insert(t1.Count, c);

                ttt[x] = t1;
            }

            List<Color> ok = new List<Color>();

            for (int x = 0; x < tia; x++)
            {

                for (int y = 0; y <hang; y++)
                {
                    ok.Add(ttt[x][y]);
                }

            }

            return ok;
        }
        private Color[,] chaytoi_moi(Color[,] ddd, int tia, int hang)
        {
            Color[,] ttt = new Color[tia, hang];


            for (int x = 0; x < tia - 1; x++)
            {
                for (int y = 0; y < hang; y++)
                {
                    ttt[x, y] = ddd[x + 1, y];
                }
            }


            for (int x = 0; x < hang; x++)
            {
                ttt[tia - 1, x] = ddd[0, x];
            }
            

 

            return ttt;
        }

        private Color[,] chaylui_moi(Color[,] ddd, int tia, int hang)
        {
            Color[,] ttt = new Color[tia, hang];

            for (int x = 1; x < tia; x++)
            {
                for (int y = 0; y < hang; y++)
                {
                    ttt[x, y] = ddd[x - 1, y];
                }
            }


            for (int x = 0; x < hang; x++)
            {
                ttt[0, x] = ddd[tia-1, x];
            }
           



            return ttt;
        }

        private Color[,] toara_moi(Color[,] ddd, int tia, int hang)
        {
            Color[,] ttt = new Color[tia, hang];


            for (int x = 0; x < hang - 1; x++)
            {
                for (int y = 0; y < tia; y++)
                {
                    ttt[y,x] = ddd[y,x + 1];
                }
            }


            for (int x = 0; x < tia; x++)
            {
                ttt[x,hang - 1 ] = ddd[x,0];
            }




            return ttt;
        }

        private Color[,]toavao_moi(Color[,] ddd, int tia, int hang)
        {
            Color[,] ttt = new Color[tia, hang];

            for (int x = 1; x < hang; x++)
            {
                for (int y = 0; y < tia; y++)
                {
                    ttt[y,x] = ddd[y,x - 1];
                }
            }


            for (int x = 0; x < tia; x++)
            {
                ttt[x,0 ] = ddd[x,hang - 1];
            }




            return ttt;
        }


        private List<Color> chaylui(List<Color> ddd, int tia, int hang)
        {
            List<List<Color>> ttt = new List<List<Color>>();


            for (int x = 0; x < tia; x++)
            {
                List<Color> t1 = new List<Color>();
                for (int y = 0; y < hang; y++)
                {
                    t1.Add(ddd[x * hang + y]);
                }
                ttt.Add(t1);
            }

            for (int x = 0; x < tia; x++)
            {
                List<Color> t1 = ttt[x];

                Color c = t1[t1.Count - 1];
                t1.RemoveAt(t1.Count - 1);
                t1.Insert(0, c);

                ttt[x] = t1;
            }

            List<Color> ok = new List<Color>();

            for (int x = 0; x < tia; x++)
            {

                for (int y = 0; y <hang; y++)
                {
                    ok.Add(ttt[x][y]);
                }

            }

            return ok;
        }



        int vt_chon_vien = -1;
        int vt_hien_vien = 0;
        int max_rong_vien = 2;
        int max_cao_vien = 5;
        int max_hien_vien = 0;

        string path_vien = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\border\\";

        private void hien_list_vien()
        {
            panel_wail.Visible = true;
            Application.DoEvents();

            list_vien.Clear();
           
                foreach (string link in Directory.GetFiles(path_vien, "*.bro"))
                {
                    list_vien.Add(link);


                    if (File.Exists(path_vien + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp") == false)
                    {

                    docanh = File.ReadAllBytes(link);
                    docanh = Decrypt_moi(docanh);
                  
                    List<Point> dddd = new List<Point>();
                    for (int x = 0; x < 40; x++) dddd.Add(new Point(x * 2, 0));
                    for (int x = 0; x < 32; x++) dddd.Add(new Point(78, x * 2));
                    for (int x = 39; x >= 0; x--) dddd.Add(new Point( x * 2, 58));
                    for (int x = 31; x >= 0; x--) dddd.Add(new Point(0, x * 2));


                    //data[0] = (byte)num_nen;
                    // data[1] = (byte)num_vien;
                    // data[2] = (byte)loai_chay_nen.SelectedIndex;
                    //data[3] = (byte)loai_chay_vien.SelectedIndex;
                    int t_chay_nen = trave_tong((int)docanh[2], (int)docanh[0]);
                    int t_chay_vien = trave_tong((int)docanh[3], (int)docanh[1]);
                    int dem = 8;
                    Color[] dnn = new Color[32];
                    Color[] dvv = new Color[32];
                    for (int x = 0; x < docanh[0]; x++)
                    {

                        dnn[x] = Color.FromArgb(docanh[dem], docanh[dem + 1], docanh[dem + 2], docanh[dem + 3]);
                        dem = dem + 4;
                    }
                    for (int x = 0; x < docanh[1]; x++)
                    {
                        dvv[x] = Color.FromArgb(docanh[dem], docanh[dem + 1], docanh[dem + 2], docanh[dem + 3]);
                        dem = dem + 4;
                    }

                    int[] temp = get_vien(dnn, t_chay_nen / 3, docanh[0], t_chay_nen / 3, docanh[2]);





                    int[] temp1 = get_vien(dvv, t_chay_vien / 3, docanh[1], t_chay_vien / 3, docanh[3]);


                    try
                    {
                        Bitmap aa = new Bitmap(80, 60);
                        Graphics g1 = Graphics.FromImage(aa);
                        g1.Clear(Color.Black);

                        for (int x = 0; x < dddd.Count; x++)
                        {
                            g1.FillRectangle(new SolidBrush(travemau(temp[x % (int)docanh[0]])), dddd[x].X, dddd[x].Y, 2, 2);

                        }
                        for (int x = 0; x < dddd.Count; x++)
                        {
                            if (temp1[x % (int)docanh[1]] != 0) g1.FillRectangle(new SolidBrush(travemau(temp1[x % (int)docanh[1]])), dddd[x].X, dddd[x].Y, 2, 2);

                        }
                       
                        aa.Save(path_vien + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp");
                   
                        // il.Images.Add(Path.GetFileNameWithoutExtension(link), aa);
                    }
                    catch (Exception ex)
                    {

                    }


                    docanh = null;
                    }

                }
             
            int ddd = max_cao_vien * max_rong_vien;
            max_hien_vien = (list_vien.Count - ddd) / max_rong_vien;
            if ((list_vien.Count - ddd) % max_rong_vien > 0) max_hien_vien++;

            vScrollBar2.Value = 0;
            if (max_hien_vien >= 1) vScrollBar2.Maximum = max_hien_vien+9;
            else vScrollBar2.Maximum = 1;
            vt_chon_vien = -1;
            vt_hien_vien = 0;
            pic_hu_vien.Refresh();
            panel_wail.Visible = false; ;
        }


        int dem_vien = 0;
        int dem_nen = 0;
        int tong_chay_nen = 0;
        int tong_chay_vien = 0;
        const int HU_DUNGYEN = 0;
        const int HU_CHAYDUOI_T = 1;
        const int HU_CHAYDUOI_N = 2;
        const int HU_CHAYDUOI_TN = 3;
        const int HU_DOIMAU_T = 4;
        const int HU_DOIMAU_N = 5;
        const int HU_GOCHU_T = 6;
        const int HU_GOCHU_N = 7;
        const int HU_XEPCHU_T = 8;
        const int HU_XEPCHU_N = 9;
        const int HU_XEPCHU1_T = 10;
        const int HU_XEPCHU1_N = 11;
        const int HU_BANCHU_T = 12;
        const int HU_BANCHU_N = 13;
        const int HU_BANCHU1_T = 14;
        const int HU_BANCHU1_N = 15;
        const int HU_XEPCHUBANCHU = 16;
        const int HU_XEPCHUBANCHU1 = 17;
        const int HU_CHAYTOA_T = 18;
        const int HU_CHAYTOA_N = 19;
        const int HU_CHAYTOA_TN = 20;
        const int HU_CHAYTOI_T = 21;
        const int HU_CHAYTOI_N = 22;
        const int HU_CHAYTOI_TN = 23;
        int num_vien = 8;
        int num_nen = 1;
        private Color[] color_v = new Color[32] { Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent };
        private Color[] color_n = new Color[32] { Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent };
        private int[] get_vien(Color[] da, int vt, int num, int maxchay, int loai_hu)
        {
            Color[] temp = new Color[32];
            Color[] temp0 = new Color[32];
            int tong = 0;
            int ttt = 0;

            for (int x = 0; x < 32; x++)
            {
                temp[x] = Color.Transparent;
            }

            for (int x = 0; x < 32; x++) temp0[x] = da[x];
            tong = num;


            if (loai_hu >= 0 && loai_hu <= 5 && tong > 0) vt = vt % tong;

            if (loai_hu == HU_DUNGYEN)
            {
                for (int x = 0; x < tong; x++)
                {
                    if (temp0[x] != Color.Transparent) temp[x] = temp0[x];
                }
            }
            else if (loai_hu == HU_CHAYTOA_T || loai_hu == HU_CHAYTOA_N)
            {
                // int gg = tong - vt - 1;

                if (vt > 0)
                {
                    if (loai_hu == HU_CHAYTOA_N) vt = maxchay - vt;
                    if (tong % 2 == 0)
                    {
                        int vt_giua = tong / 2;
                        for (int x = 0; x < vt; x++)
                        {
                            if (vt_giua - x >= 0)
                            {
                                if (vt_giua - x - 1 >= 0) if (temp0[vt_giua - x - 1] != Color.Transparent) temp[vt_giua - x - 1] = temp0[vt_giua - x - 1];

                            }
                            if (vt_giua + x < 32)
                            {
                                if (temp0[vt_giua + x] != Color.Transparent) temp[vt_giua + x] = temp0[vt_giua + x];

                            }

                        }
                    }
                    else
                    {
                        int vt_giua = tong / 2;



                        for (int x = 0; x < vt; x++)
                        {
                            if (vt_giua - x >= 0)
                            {
                                if (temp0[vt_giua - x] != Color.Transparent) temp[vt_giua - x] = temp0[vt_giua - x];

                            }
                            if (vt_giua + x < 32)
                            {
                                if (temp0[vt_giua + x] != Color.Transparent) temp[vt_giua + x] = temp0[vt_giua + x];

                            }

                        }

                        if (temp0[vt_giua] != Color.Transparent) temp[vt_giua] = temp0[vt_giua];
                    }



                }
            }
            else if (loai_hu == HU_CHAYTOA_TN)
            {
                // int gg = tong - vt - 1;

                if (vt > 0)
                {

                    int v1 = vt % (maxchay / 2);
                    int v2 = vt / (maxchay / 2);

                    if (v2 == 0) vt = v1;
                    else vt = maxchay / 2 - v1;

                    if (tong % 2 == 0)
                    {
                        int vt_giua = tong / 2;
                        for (int x = 0; x < vt; x++)
                        {
                            if (vt_giua - x >= 0)
                            {
                                if (vt_giua - x - 1 >= 0) if (temp0[vt_giua - x - 1] != Color.Transparent) temp[vt_giua - x - 1] = temp0[vt_giua - x - 1];

                            }
                            if (vt_giua + x < 32)
                            {
                                if (temp0[vt_giua + x] != Color.Transparent) temp[vt_giua + x] = temp0[vt_giua + x];

                            }

                        }
                    }
                    else
                    {
                        int vt_giua = tong / 2;



                        for (int x = 0; x < vt; x++)
                        {
                            if (vt_giua - x >= 0)
                            {
                                if (temp0[vt_giua - x] != Color.Transparent) temp[vt_giua - x] = temp0[vt_giua - x];

                            }
                            if (vt_giua + x < 32)
                            {
                                if (temp0[vt_giua + x] != Color.Transparent) temp[vt_giua + x] = temp0[vt_giua + x];

                            }

                        }

                        if (temp0[vt_giua] != Color.Transparent) temp[vt_giua] = temp0[vt_giua];
                    }



                }
            }
            else if (loai_hu == HU_CHAYTOI_T || loai_hu == HU_CHAYTOI_N)
            {
                if (loai_hu == HU_CHAYTOI_T) vt = maxchay - vt - 1;
                int gg = vt - tong;
                for (int x = 0; x < tong; x++)
                {
                    if (gg >= 0 && gg < tong)
                    {

                        if (temp0[x] != Color.Transparent) temp[x] = temp0[tong - 1 - gg];

                    }
                    gg++;
                }
            }

            else if (loai_hu == HU_CHAYTOI_TN)
            {
                int v1 = vt % (maxchay / 2);
                int v2 = vt / (maxchay / 2);

                if (v2 == 1) vt = v1;
                else vt = maxchay / 2 - v1;


                int gg = vt - tong;


                for (int x = 0; x < tong; x++)
                {
                    if (gg >= 0 && gg < tong)
                    {

                        if (temp0[x] != Color.Transparent) temp[x] = temp0[tong - 1 - gg];

                    }
                    gg++;
                }
            }
            else if (loai_hu == HU_CHAYDUOI_T)
            {
                int gg = tong - vt - 1;
                for (int x = 0; x < tong; x++)
                {
                    if (temp0[gg % tong] != Color.Transparent) temp[x] = temp0[gg % tong];
                    gg++;
                }
            }
            else if (loai_hu == HU_CHAYDUOI_N)
            {
                int gg = vt;
                for (int x = 0; x < tong; x++)
                {
                    if (temp0[tong - 1 - gg % tong] != Color.Transparent) temp[x] = temp0[tong - 1 - gg % tong];
                    gg++;
                }
            }
            else if (loai_hu == HU_CHAYDUOI_TN)
            {
                int gg = tong - vt - 1;
                for (int x = 0; x < tong; x++)
                {
                    if (temp0[gg % tong] != Color.Transparent) temp[x] = temp0[gg % tong];
                    gg++;
                }
                gg = vt;
                for (int x = 0; x < tong; x++)
                {
                    if (temp0[tong - 1 - gg % tong] != Color.Transparent) temp[x] = temp0[tong - 1 - gg % tong];
                    gg++;
                }
            }
            else if (loai_hu == HU_DOIMAU_T)
            {

                for (int x = 0; x < tong; x++)
                {
                    if (temp0[vt] != Color.Transparent) temp[x] = temp0[vt];

                }
            }
            else if (loai_hu == HU_DOIMAU_N)
            {

                for (int x = 0; x < tong; x++)
                {
                    if (temp0[tong - 1 - vt] != Color.Transparent) temp[x] = temp0[tong - 1 - vt];

                }
            }
            else if (loai_hu == HU_GOCHU_T)
            {
                int ss = vt % 6, ss1 = vt / 6;
                for (int x = 0; x < ss1; x++)
                {
                    if (temp0[x] != Color.Transparent) temp[x] = temp0[x];
                }

                if (ss == 0 || ss == 2 || ss == 4)
                {
                    if (temp0[ss1] != Color.Transparent) temp[ss1] = temp0[ss1];

                }
                else
                {
                    temp[ss1] = Color.Transparent;
                }
            }
            else if (loai_hu == HU_GOCHU_N)
            {
                int ss = vt % 6, ss1 = vt / 6;
                for (int x = 0; x < ss1; x++)
                {
                    if (temp0[x] != Color.Transparent) temp[tong - 1 - x] = temp0[x];
                }

                if (ss == 0 || ss == 2 || ss == 4)
                {
                    if (temp0[ss1] != Color.Transparent) temp[tong - 1 - ss1] = temp0[ss1];

                }
                else
                {
                    temp[tong - 1 - ss1] = Color.Transparent;
                }
            }
            else if (loai_hu == HU_XEPCHU_T || loai_hu == HU_XEPCHU_N || loai_hu == HU_XEPCHU1_T || loai_hu == HU_XEPCHU1_N || loai_hu == HU_BANCHU_T || loai_hu == HU_BANCHU_N || loai_hu == HU_BANCHU1_T || loai_hu == HU_BANCHU1_N)
            {
                if (loai_hu == HU_BANCHU_T || loai_hu == HU_BANCHU_N || loai_hu == HU_BANCHU1_T || loai_hu == HU_BANCHU1_N) vt = maxchay - vt - 1;
                int dd = 0, dd1 = 0;
                int qq = 0, oo = 0;
                dd = tong;
                qq = dd - 1;
                while (dd < vt)
                {

                    dd += qq;
                    qq--;
                    oo++;
                }

                dd1 = vt - dd + qq;
                if (dd1 >= 0 && oo <= tong)
                {

                    if (loai_hu == HU_XEPCHU_T || loai_hu == HU_XEPCHU1_T || loai_hu == HU_BANCHU_T || loai_hu == HU_BANCHU1_T)
                    {

                        if (loai_hu == HU_XEPCHU_T || loai_hu == HU_BANCHU_T)
                        {
                            if (temp0[tong - 1 - dd1] != Color.Transparent) temp[dd1] = temp0[dd1];
                        }
                        else
                        {
                            if (temp0[tong - 1 - dd1] != Color.Transparent) temp[dd1] = temp0[tong - 1];

                        }

                        for (int x = 0; x < oo; x++)
                        {
                            if (temp0[x] != Color.Transparent) temp[tong - x - 1] = temp0[tong - 1 - x];
                        }
                    }
                    else if (loai_hu == HU_XEPCHU_N || loai_hu == HU_XEPCHU1_N || loai_hu == HU_BANCHU_N || loai_hu == HU_BANCHU1_N)
                    {

                        if (loai_hu == HU_XEPCHU_N || loai_hu == HU_BANCHU_N)
                        {
                            if (temp0[tong - 1 - dd1] != Color.Transparent) temp[tong - 1 - dd1] = temp0[dd1];
                        }
                        else
                        {
                            if (temp0[tong - 1 - dd1] != Color.Transparent) temp[tong - 1 - dd1] = temp0[tong - 1];
                        }
                        for (int x = 0; x < oo; x++)
                        {
                            if (temp0[x] != Color.Transparent) temp[x] = temp0[tong - 1 - x];
                        }
                    }



                }
                else
                {
                    if (loai_hu == HU_XEPCHU_T || loai_hu == HU_XEPCHU1_T || loai_hu == HU_BANCHU_T || loai_hu == HU_BANCHU1_T)
                    {
                        for (int x = 0; x < tong; x++)
                        {
                            if (temp0[tong - 1 - x] != Color.Transparent) temp[x] = temp0[tong - 1 - x];
                        }
                    }
                    else if (loai_hu == HU_XEPCHU_N || loai_hu == HU_XEPCHU1_N || loai_hu == HU_BANCHU_N || loai_hu == HU_BANCHU1_N)
                    {
                        for (int x = 0; x < tong; x++)
                        {
                            if (temp0[x] != Color.Transparent) temp[x] = temp0[tong - 1 - x];
                        }
                    }

                }


            }
            else if (loai_hu == HU_XEPCHUBANCHU || loai_hu == HU_XEPCHUBANCHU1)
            {
                vt++;
                if (vt < maxchay - 1)
                {
                    int v1 = vt % (maxchay / 2);
                    int v2 = vt / (maxchay / 2);

                    if (v2 == 0) vt = v1;
                    else vt = maxchay / 2 - v1;


                    int dd = 0, dd1 = 0;
                    int qq = 0, oo = 0;
                    dd = tong;
                    qq = dd - 1;
                    while (dd < vt)
                    {

                        dd += qq;
                        qq--;
                        oo++;
                    }

                    dd1 = vt - dd + qq;
                    if (dd1 >= 0 && oo <= tong)
                    {


                        if (loai_hu == HU_XEPCHUBANCHU)
                        {
                            if (temp0[tong - 1 - dd1] != Color.Transparent) temp[dd1] = temp0[dd1];
                        }
                        else
                        {
                            if (temp0[tong - 1 - dd1] != Color.Transparent) temp[dd1] = temp0[tong - 1];

                        }

                        for (int x = 0; x < oo; x++)
                        {
                            if (temp0[x] != Color.Transparent) temp[tong - x - 1] = temp0[tong - 1 - x];
                        }




                    }
                    else
                    {

                        for (int x = 0; x < tong; x++)
                        {
                            if (temp0[tong - 1 - x] != Color.Transparent) temp[x] = temp0[x];
                        }



                    }

                }



            }


            int[] daa = new int[32];



            for (int x = 0; x < 32; x++)
            {
                if (temp[x] == Color.Transparent) daa[x] = 0;
                else daa[x] = BitConverter.ToInt32(new byte[4] { 255, temp[x].R, temp[x].G, temp[x].B }, 0);

            }
            return daa;
        }
        private  void get_tong_vien()
        {
            
            dem_nen = 0;
            dem_vien = 0;

            tong_chay_nen = trave_tong(loai_chay_nen.SelectedIndex, num_nen);
            tong_chay_vien = trave_tong(loai_chay_vien.SelectedIndex, num_vien);

            
        }


        private void taotong()
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            dem_nen = 0;
            dem_vien = 0;
            tong_chay_nen = trave_tong(loai_chay_nen.SelectedIndex, num_nen);
            tong_chay_vien = trave_tong(loai_chay_vien.SelectedIndex, num_vien);

            timer.Enabled = true;
            timer_map.Enabled = true;
        }
        private int trave_tong(int loai, int hu)
        {
            int tt = 0;



            if (loai == HU_DUNGYEN || loai == HU_CHAYDUOI_T || loai == HU_CHAYDUOI_N || loai == HU_CHAYDUOI_TN || loai == HU_DOIMAU_T || loai == HU_DOIMAU_N) tt = hu;
            else if (loai == HU_GOCHU_T || loai == HU_GOCHU_N) tt = hu * 6;
            else if (loai == HU_XEPCHU_T || loai == HU_XEPCHU_N || loai == HU_XEPCHU1_T || loai == HU_XEPCHU1_N || loai == HU_BANCHU_T || loai == HU_BANCHU_N || loai == HU_BANCHU1_T || loai == HU_BANCHU1_N)
            {

                tt = 0;
                for (int aa = 0; aa < hu + 1; aa++) tt += aa;

            }
            else if (loai == HU_XEPCHUBANCHU || loai == HU_XEPCHUBANCHU1)
            {

                tt = 0;
                for (int aa = 0; aa < hu + 1; aa++) tt += aa;
                tt = tt * 2 - 1;

            }
            else if (loai == HU_CHAYTOA_T || loai == HU_CHAYTOA_N)
            {
                if (hu % 2 == 0) tt = hu / 2 + 1;
                else tt = hu / 2 + 2;

            }
            else if (loai == HU_CHAYTOA_TN)
            {
                if (hu % 2 == 0) tt = hu / 2 + 1;
                else tt = hu / 2 + 2;
                tt = tt * 2;

            }
            else if (loai == HU_CHAYTOI_T || loai == HU_CHAYTOI_N)
            {
                tt = hu * 2;
            }
            else if (loai == HU_CHAYTOI_TN)
            {
                tt = hu * 4;
            }

            return tt;

        }


        private Image read_image(byte[] aa, int vt)
        {
            try
            {

                int bd = aa[332 + 8 * vt] + aa[332 + 8 * vt + 1] * 256 + aa[332 + 8 * vt + 2] * 256 * 256 + aa[332 + 8 * vt + 3] * 256 * 256 * 256;
                int lengt = aa[332 + 8 * vt + 4] + aa[332 + 8 * vt + 5] * 256 + aa[332 + 8 * vt + 6] * 256 * 256 + aa[332 + 8 * vt + 7] * 256 * 256 * 256;
                byte[] gg = new byte[lengt];
                // for (int x = 0; x < lengt; x++) gg[x] = aa[bd + x];
                Array.Copy(aa, bd, gg, 0, lengt);
                Image ff = byteArrayToImage(gg);
                return ff;
            }
            catch
            {

            }
            return (Image)(new Bitmap(100, 100));
        }
        private static RijndaelManaged GetAlgorithm()
        {
            PasswordDeriveBytes key = new PasswordDeriveBytes("chandoi", new byte[] { 0x34, 0x11, 0x46, 0x89, 0x14, 0x58, 0x69, 0x23, 0x72, 0x87 });
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            return algorithm;
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public static byte[] ImageToByte2(System.Drawing.Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
        

        private void button14_Click(object sender, EventArgs e)
        {

        }
        bool well(Control con, MouseEventArgs e, ref int vt_hien, int max_hien)
        {
            if (con.ClientRectangle.Contains(con.PointToClient(Cursor.Position)))
            {
                bool lan555 = true;
                System.Drawing.Point pt_MouseAb555 = Control.MousePosition;
                // MessageBox.Show(e.Delta.ToString());
                do
                {
                    System.Drawing.Rectangle r_Ctrl555 = con.RectangleToScreen(con.ClientRectangle);
                    if (!r_Ctrl555.Contains(pt_MouseAb555))
                    {
                        base.OnMouseWheel(e);
                        lan555 = false;
                        break;
                    }
                    con = con.Parent;
                }
                while (con != null && con != this);
                // MessageBox.Show(lan555.ToString());
                if (lan555 == true)
                {
                    if (e.Delta < 0)
                    {

                        if (vt_hien < max_hien)
                        {
                            vt_hien++;
                            con.Refresh();
                            // return vt_hien;

                        }
                    }
                    else if (e.Delta > 0)
                    {
                        if (vt_hien > 0)
                        {
                            vt_hien--;
                            con.Refresh();
                            //  return vt_hien;

                        }
                    }
                }
                return true;
            }
            //pic_thuvien_mau
            return false;
            // return vt_hien;
        }

        void well1(Control con, MouseEventArgs e)
        {

            //pic_thuvien_mau
            bool lan555 = true;
            System.Drawing.Point pt_MouseAb555 = Control.MousePosition;
            // MessageBox.Show(e.Delta.ToString());
            do
            {
                System.Drawing.Rectangle r_Ctrl555 = con.RectangleToScreen(con.ClientRectangle);
                if (!r_Ctrl555.Contains(pt_MouseAb555))
                {
                    base.OnMouseWheel(e);
                    lan555 = false;
                    break;
                }
                con = con.Parent;
            }
            while (con != null && con != this);
            // MessageBox.Show(lan555.ToString());
            if (lan555 == true)
            {
                float oldzoom = zoom;
                if (e.Delta < 0)
                {
                    if (zoom < 3)
                    {

                        zoom = zoom + 0.1F;
                    }
                }
                else if (e.Delta > 0)
                {
                    if (zoom > 0.2F)
                    {

                        zoom = zoom - 0.1F;
                    }
                }

                System.Drawing.Point mousePosNow = e.Location;
                int x = mousePosNow.X - pic_map.Location.X;   // Where location of the mouse in the pictureframe
                int y = mousePosNow.Y - pic_map.Location.Y;
                int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
                int oldimagey = (int)(y / oldzoom);
                int newimagex = (int)(x / zoom);     // Where in the IMAGE will it be when the new zoom i made
                int newimagey = (int)(y / zoom);
                imgx = newimagex - oldimagex + imgx;  // Where to move image to keep focus on one point
                imgy = newimagey - oldimagey + imgy;
            }
            // return vt_hien;
        }

        
      

        Color[] mau_led;
        bool play_chinh = false;

        string[,] name_halo_1 = new string[,]
      {
            { "Xoay phải","Tỏa vào","Mờ dần","Đổi màu","Xoay trái","Tỏa ra","Sáng dần" },
            { "Turn right","Radiate on","Fading","Change color","Turn left","Radiate","Light up"}
      };
        string[,] name_halo_2 = new string[,]
     {
            { "Vẽ điểm","Vẽ đối xứng","Vẽ xoáy trái","Vẽ xoáy phải","Vẽ xoáy điều","Vẽ vòng tròn","Vẽ đường thẳng" ,"Vẽ bước"},
            { "Draw points","Symmetric drawing","Draw left vortex","Draw right vortex","Drawing swirling thing","Draw a circle","Draw a straight line","Drawing steps"}
     };
        string[,] name_halo_3 = new string[,]
     {
            { "Giảm độ sáng","Đổi màu","Xoay phải","Tỏa vào","Tăng độ sáng","Đổi màu","Xoay trái","Tỏa ra"},
            { "Reduce the brightness","Change color","Turn right","Radiate on","Increase the brightness","Change color","Turn left","Radiate"}
     };



        string[] name_halo_4 = new string[2] { "Thêm lớp", "Add layer" };
        string[] name_halo_5_0 = new string[2] { "Chạy", "Play" };
        string[] name_halo_5_1 = new string[2] { "Dừng", "Stop" };
        string[] name_halo_6= new string[2] { "Bước", "Steps" };
        string[] name_halo_7 = new string[2] { "Lưu", "Save" };
        string[] name_halo_8 = new string[2] { "Trở về", "Home" };
        string[] name_halo_9 = new string[2] { "Xóa", "Delete" };
        string[] name_halo_10 = new string[2] { "Vẽ", "Draw" };
        string[] name_halo_11 = new string[2] { "Bước", "Step" };


        string[] name_haoquang_1 = new string[2] { "T.độ", "Speed" };
        string[] name_haoquang_2 = new string[2] { "Đổi màu", "Change colors" };
        string[] name_haoquang_7 = new string[2] { "Tạo Hào quang", "New Halo" };
        string[] name_haoquang_4 = new string[2] { "Chọn", "ADD" };
        string[] name_haoquang_5 = new string[2] { "Trở về", "Home" };
        string[] name_haoquang_6 = new string[2] { "Xóa", "Delete" };
        string[] name_haoquang_3 = new string[2] { "Sửa Hào quang", "Edit Halo" };

        string[] name_wire = new string[2] { "Lớp: ", "Layer: " };
        string[] name_wire2 = new string[2] { "Nhóm: ", "Group: " };
        string[] name_wire1 = new string[2] { "Tia: ", "Column: " };

        string[] name_layer1 = new string[2] { "Hiệu ứng", "Effect" };
        string[] name_layer2 = new string[2] { "Chữ", "Text" };
        string[] name_layer3 = new string[2] { "Viền", "Border" };
        string[] name_layer4 = new string[2] { "Vẫy", "RGB" };
        string[] name_layer5 = new string[2] { "Thủ công", "Manual" };
        string[] name_layer6 = new string[2] { "Hào quang", "Halo" };


        string[] name_wire_effect1 = new string[2] { "Chọn hết", "Select all" };
        string[] name_wire_effect2 = new string[2] { "Xóa hết", "Clear all" };
        string[] name_wire_effect3 = new string[2] { "Bỏ chọn(Ctrl)", "Select- (Ctrl)" };
        string[] name_wire_effect4 = new string[2] { "Chấp nhận", "OK" };

        string[] name_wire_effect5 = new string[2] { "Không có sơ đồ, bạn muốn vẽ lại ?", "There is no map, you want to redraw ?" };

        string[] name_thucong1 = new string[2] { "T.độ", "Speed" };

        string[] name_effect1 = new string[2] { "Xoay góc", "Rotator" };
        string[] name_effect2 = new string[2] { "Đảo chiều", "reverse" };
        string[] name_effect3 = new string[2] { "Lật ngang", "Vertical" };
        string[] name_effect4 = new string[2] { "Lật dọc", "Horizontal" };
        string[] name_effect5 = new string[2] { " hiệu ứng đổi màu", "Effect Change colors" };
        string[] name_effect6 = new string[2] { "Xoay tròn", "Spin around" };
        string[] name_effect7 = new string[2] { "Đặt lại thông số", "Reset" };
        string[] name_effect8 = new string[2] { "Co giãn", "Elasticity" };
        string[] name_effect9 = new string[2] { "Đổi màu", "Change colors" };
        string[] name_effect10 = new string[2] { "Chọn", "ADD" };
        string[] name_effect11 = new string[2] { "Trở về", "Home" };
        string[] name_effect12 = new string[2] { "Loại", "Type" };
        string[] name_effect13 = new string[2] { "Xóa", "Delete" };



        string[] name_text1 = new string[2] { "Cỡ", "Size" };
        string[] name_text2 = new string[2] { "Kiểu", "Style" };
        string[] name_text3 = new string[2] { "Nét", "Render" };
        string[] name_text4 = new string[2] { "H.ứng", "Effect" };

        string[] name_text5_1 = new string[2] { "Bình thường", "Regular" };
        string[] name_text5_2 = new string[2] { "Nghiêng", "Italic" };
        string[] name_text5_3 = new string[2] { "Đậm", "Bold" };
        string[] name_text5_4 = new string[2] { "Gạch dưới", "Underline" };

        string[] name_text6_1 = new string[2] { "Mặc định", "SystemDefault" };
        string[] name_text6_2 = new string[2] { "Nét cứng 0", "SingleBitPerPixelGridFit" };
        string[] name_text6_3 = new string[2] { "Nét cứng 1", "SingleBitPerPixel" };
        string[] name_text6_4 = new string[2] { "Nét mềm 0", "AntiAliasGridFit" };
        string[] name_text6_5 = new string[2] { "Nét mềm 1", "AntiAlias" };
        string[] name_text6_6 = new string[2] { "Siêu mềm", "ClearTypeGridFit" };

        string[] name_text7_1 = new string[2] { "Phải->trái", "Right->left" };
        string[] name_text7_2 = new string[2] { "Trái->phải", "Left->right" };
        string[] name_text7_3 = new string[2] { "Đứng yên", "None" };
        string[] name_text7_4 = new string[2] { "Phải->đứng yên", "Right->None" };
        string[] name_text7_5 = new string[2] { "Dưới->giữa", "Bottom->Center" };
        string[] name_text7_6 = new string[2] { "Trên->giữa", "Top->center" };
        string[] name_text7_7 = new string[2] { "Dưới->giữa->trái", "Bottom->center->left" };
        string[] name_text7_8 = new string[2] { "Trên->giữa->trái", "Top->center->left" };


        string[] name_text8= new string[2] { "Xóa", "Clear" };
        string[] name_text9 = new string[2] { "Tô", "Fill" };
        string[] name_text10 = new string[2] { "Lưu", "Save" };

        string[] name_text11_1 = new string[2] { "Kiểu đứng yên", "Stand still type" };
        string[] name_text11_2 = new string[2] { "Kiểu chạy đuổi->", "Type of chasing->" };
        string[] name_text11_3 = new string[2] { "Kiểu chạy đuổi<-", "Type of chasing<-" };
        string[] name_text11_4 = new string[2] { "Kiểu chạy đuổi<->", "Type of chasing<->" };
        string[] name_text11_5 = new string[2] { "Kiểu đổi màu->", "Color change type->" };
        string[] name_text11_6 = new string[2] { "Kiểu đổi màu<-", "Color change type<-" };
        string[] name_text11_7 = new string[2] { "Kiểu gõ chữ->", "Typing method->" };
        string[] name_text11_8 = new string[2] { "Kiểu gõ chữ<-", "Typing method<-" };
        string[] name_text11_9 = new string[2] { "Kiểu xếp chữ->", "Typeface style->" };
        string[] name_text11_10 = new string[2] { "Kiểu xếp chữ<-", "Typeface style<-" };
        string[] name_text11_11 = new string[2] { "Kiểu xếp chữ  1->", "Typeface style 1->" };
        string[] name_text11_12 = new string[2] { "Kiểu xếp chữ  1<-", "Typeface style  1<-" };
        string[] name_text11_13 = new string[2] { "Kiểu bắn chữ->", "Letter shot style->" };
        string[] name_text11_14 = new string[2] { "Kiểu bắn chữ<-", "Letter shot style<-" };
        string[] name_text11_15 = new string[2] { "Kiểu bắn chữ 1->", "Letter shot style 1->" };
        string[] name_text11_16 = new string[2] { "Kiểu bắn chữ 1<-", "Letter shot style 1<-" };
        string[] name_text11_17 = new string[2] { "Kiểu xếp chữ<->", "Typeface style<->" };
        string[] name_text11_18 = new string[2] { "Kiểu xếp chữ 1<->", "Typeface style 1<->" };
        string[] name_text11_19 = new string[2] { "Kiểu chạy tỏa->", "Type radiate run->" };
        string[] name_text11_20 = new string[2] { "Kiểu chạy tỏa<-", "Type radiate run<-" };
        string[] name_text11_21 = new string[2] { "Kiểu chạy tỏa<->", "Type radiate run<->" };
        string[] name_text11_22 = new string[2] { "Kiểu chạy tới->", "The kind of run to->" };
        string[] name_text11_23 = new string[2] { "Kiểu chạy tới<-", "The kind of run to<-" };
        string[] name_text11_24 = new string[2] { "Kiểu chạy tới<->", "The kind of run to<->" };






















        string[,] ngonngu_wire = new string[,]
{
           { "Nối kiểu kéo qua từng led","Nối kiểu kéo đường thẳng qua led","Thiết lập đầu vào cổng 1","Thiết lập đầu vào cổng 2","Đảo chiều dây cổng 1","Đảo chiều dây cổng 2"},
             { "Matching point type","Matching line type","Set port 1","Set port 2","Reverse port 1","Reverse port 2"}

};




        string[] name_vien_1 = new string[2] { "Đảo chiều", "reverse" };
        string[] name_vien_2 = new string[2] { " hiệu ứng đổi màu", "Effect Change colors" };
        string[] name_vien_3 = new string[2] { "Đổi màu", "Change colors" };
        string[] name_vien_4 = new string[2] { "T.độ", "Speed" };
        string[] name_vien_5 = new string[2] { "Nhóm", "Group" };
        string[] name_vien_6 = new string[2] { "Thêm viền", "New border" };
        string[] name_vien_6_1 = new string[2] { "Thêm vẫy", "New RGB" };
        string[] name_vien_7 = new string[2] { "Chọn", "ADD" };
        string[] name_vien_8 = new string[2] { "Trở về", "Home" };
        string[] name_vien_9 = new string[2] { "Xóa", "Delete" };

        string[] name_wire_vien_1 = new string[2] { "+ dây", "Add wire" };
        string[] name_wire_vien_2 = new string[2] { "+++ dây", "Add wire+" };
        string[] name_wire_vien_3 = new string[2] { "Chọn hết", "Fill" };
        string[] name_wire_vien_4 = new string[2] { "Xóa", "Clear" };
        string[] name_wire_vien_5_0 = new string[2] { "Chạy", "Play" };
        string[] name_wire_vien_5_1 = new string[2] { "Dừng", "Stop" };
        string[] name_wire_vien_6 = new string[2] { "Kéo thẳng", "Line select" };
        string[] name_wire_vien_7 = new string[2] { "Chấp nhận", "OK" };
        string[] name_wire_vien_8 = new string[2] { "Xóa", "Delete" };


        string[] name_wail = new string[2] { "Chờ một lát...", "Waiting..." };

        string[] name_menu1 = new string[2] { "          Thêm lớp", "          Add layer" };
        string[] name_menu2 = new string[2] { "          Chạy", "          Play" };
        string[] name_menu2_1 = new string[2] { "          Dừng", "          Stop" };
        string[] name_menu3 = new string[2] { "          In sơ đồ", "          Printer map" };
        string[] name_menu4 = new string[2] { "          Hiện ảnh", "          View picture" };
        string[] name_menu5 = new string[2] { "          Về mặc định(R)", "          Reset view(R)" };
        string[] name_menu6 = new string[2] { "          Ngôn ngữ", "          Language" };
        string[] name_menu7 = new string[2] { "          Nạp mạch", "          Program "};
        string[] name_menu8 = new string[2] { "          Lưu dự án", "          Save" };
        string[] name_menu9 = new string[2] { "          Mở dự án", "          Load" };
        string[] name_menu10 = new string[2] { "          Hướng dẫn", "          Tutorial" };
        string[] name_menu11 = new string[2] { "          Tạo dự án", "          New" };

        string[] name_menu12 = new string[2] { "Lên", "Up" };
        string[] name_menu13 = new string[2] { "Xuống", "Down" };
        string[] name_menu14 = new string[2] { "Xóa", "Delete" };

        string[] name_menu15 = new string[2] { "Di chuyển->", "Move->" };
        string[] name_menu16 = new string[2] { "<-Di chuyển", "<-Move" };
        string[] name_menu17 = new string[2] { "Chọn/bỏ co giãn", "Yes/No align effect" };
        string[] name_menu18 = new string[2] { "Xóa", "Delete" };

        string[] name_nap1 = new string[2] { "Loại đèn", "Type led" };
        string[] name_nap2 = new string[2] { "Nạp mạch", "Program" };
        string[] name_nap3 = new string[2] { "Không tìm thấy mạch!", "No board found!" };
        string[] name_nap4 = new string[2] { "Chuẩn bị dữ liệu!", "Data preparation!" };
        string[] name_nap5 = new string[2] { "Dung lượng: ", "Capacity:" };
        string[] name_nap6 = new string[2] { "Đang xóa bộ nhớ !(30s)", "Clearing memory !(30s)" };
        string[] name_nap7 = new string[2] { "Đang nạp... ", "Loading.." };
        string[] name_nap8 = new string[2] { "Vượt quá số bóng(vui lòng tách cổng)", "Exceeded number of led (please separate gate)" };
        string[] name_nap9 = new string[2] { "Vượt quá dung lượng!", "Storage exceeded!" };

        string[] name_haoquang1 = new string[2] { "Hiệu ứng xoay", "Rotation effect" };
        string[] name_haoquang2 = new string[2] { "Hiệu ứng tỏa", "Radiating effect" };
        string[] name_haoquang3 = new string[2] { "Hiệu ứng đổi màu", "Change colors effect" };
        string[] name_haoquang4 = new string[2] { "Tô màu nền", "Fill color" };




        private void set_ngongu()
        {
           // hieuung1.Text= name_haoquang1[menu_ngonngu];
            //hieuung2.Text = name_haoquang2[menu_ngonngu];
           // hieuung3.Text = name_haoquang3[menu_ngonngu];


            button49.Text = name_nap2[menu_ngonngu];

            toolStripMenuItem7.Text = name_menu12[menu_ngonngu];
            toolStripMenuItem6.Text = name_menu13[menu_ngonngu];
            toolStripMenuItem5.Text = name_menu14[menu_ngonngu];

            toolStripMenuItem8.Text = name_menu15[menu_ngonngu];
            toolStripMenuItem9.Text = name_menu16[menu_ngonngu];
            toolStripMenuItem12.Text = name_menu17[menu_ngonngu];
            toolStripMenuItem10.Text = name_menu18[menu_ngonngu];


            label15.Text = name_nap1[menu_ngonngu];

            button31.Text = name_menu1[menu_ngonngu];
            button32.Text = name_menu2[menu_ngonngu];
           
            label14.Text = name_wail[menu_ngonngu];

            button35.Text = name_halo_4[menu_ngonngu];
            button39.Text = name_halo_5_0[menu_ngonngu];
            num_nhan.Text = name_halo_6[menu_ngonngu];
            button37.Text = name_halo_7[menu_ngonngu];
            button36.Text = name_halo_8[menu_ngonngu];
            toolStripMenuItem4.Text = name_halo_9[menu_ngonngu];
            label11.Text = name_halo_10[menu_ngonngu];
            label16.Text = name_halo_11[menu_ngonngu];
            

            label10.Text = name_haoquang_1[menu_ngonngu];
            
            button28.Text = name_haoquang_3[menu_ngonngu];
            button30.Text = name_haoquang_4[menu_ngonngu];
            button29.Text = name_haoquang_5[menu_ngonngu];
            toolStripMenuItem3.Text = name_haoquang_6[menu_ngonngu];
            button44.Text = name_haoquang_7[menu_ngonngu];


            button14.Text = name_wire_vien_1[menu_ngonngu];
            button16.Text = name_wire_vien_2[menu_ngonngu];
            button17.Text = name_wire_vien_3[menu_ngonngu];
            button13.Text = name_wire_vien_4[menu_ngonngu];
            button27.Text = name_wire_vien_5_0[menu_ngonngu];
            keo_thang.Text = name_wire_vien_6[menu_ngonngu];
            button12.Text = name_wire_vien_7[menu_ngonngu];
            toolStripMenuItem1.Text = name_wire_vien_8[menu_ngonngu];


            label7.Text = name_vien_4[menu_ngonngu];
            check_group.Text = name_vien_5[menu_ngonngu];
            button15.Text = name_vien_6[menu_ngonngu];
            button11.Text = name_vien_7[menu_ngonngu];
            button10.Text = name_vien_8[menu_ngonngu];
            toolStripMenuItem16.Text = name_vien_9[menu_ngonngu];



            label8.Text = name_thucong1[menu_ngonngu];

            button4.Text = name_wire_effect1[menu_ngonngu];
            button5.Text = name_wire_effect2[menu_ngonngu];
            button3.Text = name_wire_effect3[menu_ngonngu];
            button6.Text = name_wire_effect4[menu_ngonngu];



            button9.Text = name_effect10[menu_ngonngu];
            button8.Text = name_effect11[menu_ngonngu];
            button1.Text = name_effect10[menu_ngonngu];
            button2.Text = name_effect11[menu_ngonngu];
            label2.Text = name_effect12[menu_ngonngu];
            toolStripMenuItem2.Text = name_effect13[menu_ngonngu];


            label6.Text = name_text1[menu_ngonngu];
            label9.Text = name_text2[menu_ngonngu];
            label5.Text = name_text3[menu_ngonngu];
            label4.Text = name_text4[menu_ngonngu];
            com_style_font.Items[0] = name_text5_1[menu_ngonngu];
            com_style_font.Items[1] = name_text5_2[menu_ngonngu];
            com_style_font.Items[2] = name_text5_3[menu_ngonngu];
            com_style_font.Items[3] = name_text5_4[menu_ngonngu];

            com_font_render.Items[0] = name_text6_1[menu_ngonngu];
            com_font_render.Items[1] = name_text6_2[menu_ngonngu];
            com_font_render.Items[2] = name_text6_3[menu_ngonngu];
            com_font_render.Items[3] = name_text6_4[menu_ngonngu];
            com_font_render.Items[4] = name_text6_5[menu_ngonngu];
            com_font_render.Items[5] = name_text6_6[menu_ngonngu];

            com_hu.Items[0] = name_text7_1[menu_ngonngu];
            com_hu.Items[1] = name_text7_2[menu_ngonngu];
            com_hu.Items[2] = name_text7_3[menu_ngonngu];
            com_hu.Items[3] = name_text7_4[menu_ngonngu];
            com_hu.Items[4] = name_text7_5[menu_ngonngu];
            com_hu.Items[5] = name_text7_6[menu_ngonngu];
            com_hu.Items[6] = name_text7_7[menu_ngonngu];
            com_hu.Items[7] = name_text7_8[menu_ngonngu];


            button18.Text = name_text8[menu_ngonngu];
            button25.Text = name_text8[menu_ngonngu];
            button19.Text = name_text9[menu_ngonngu];
            button20.Text = name_text9[menu_ngonngu];
            button26.Text = name_text10[menu_ngonngu];

            loai_chay_nen.Items[0] = name_text11_1[menu_ngonngu];
            loai_chay_nen.Items[1] = name_text11_2[menu_ngonngu];
            loai_chay_nen.Items[2] = name_text11_3[menu_ngonngu];
            loai_chay_nen.Items[3] = name_text11_4[menu_ngonngu];
            loai_chay_nen.Items[4] = name_text11_5[menu_ngonngu];
            loai_chay_nen.Items[5] = name_text11_6[menu_ngonngu];
            loai_chay_nen.Items[6] = name_text11_7[menu_ngonngu];
            loai_chay_nen.Items[7] = name_text11_8[menu_ngonngu];
            loai_chay_nen.Items[8] = name_text11_9[menu_ngonngu];
            loai_chay_nen.Items[9] = name_text11_10[menu_ngonngu];
            loai_chay_nen.Items[10] = name_text11_11[menu_ngonngu];
            loai_chay_nen.Items[11] = name_text11_12[menu_ngonngu];
            loai_chay_nen.Items[12] = name_text11_13[menu_ngonngu];
            loai_chay_nen.Items[13] = name_text11_14[menu_ngonngu];
            loai_chay_nen.Items[14] = name_text11_15[menu_ngonngu];
            loai_chay_nen.Items[15] = name_text11_16[menu_ngonngu];
            loai_chay_nen.Items[16] = name_text11_17[menu_ngonngu];
            loai_chay_nen.Items[17] = name_text11_18[menu_ngonngu];
            loai_chay_nen.Items[18] = name_text11_19[menu_ngonngu];
            loai_chay_nen.Items[19] = name_text11_20[menu_ngonngu];
            loai_chay_nen.Items[20] = name_text11_21[menu_ngonngu];
            loai_chay_nen.Items[21] = name_text11_22[menu_ngonngu];
            loai_chay_nen.Items[22] = name_text11_23[menu_ngonngu];
            loai_chay_nen.Items[23] = name_text11_24[menu_ngonngu];

            loai_chay_vien.Items[0] = name_text11_1[menu_ngonngu];
            loai_chay_vien.Items[1] = name_text11_2[menu_ngonngu];
            loai_chay_vien.Items[2] = name_text11_3[menu_ngonngu];
            loai_chay_vien.Items[3] = name_text11_4[menu_ngonngu];
            loai_chay_vien.Items[4] = name_text11_5[menu_ngonngu];
            loai_chay_vien.Items[5] = name_text11_6[menu_ngonngu];
            loai_chay_vien.Items[6] = name_text11_7[menu_ngonngu];
            loai_chay_vien.Items[7] = name_text11_8[menu_ngonngu];
            loai_chay_vien.Items[8] = name_text11_9[menu_ngonngu];
            loai_chay_vien.Items[9] = name_text11_10[menu_ngonngu];
            loai_chay_vien.Items[10] = name_text11_11[menu_ngonngu];
            loai_chay_vien.Items[11] = name_text11_12[menu_ngonngu];
            loai_chay_vien.Items[12] = name_text11_13[menu_ngonngu];
            loai_chay_vien.Items[13] = name_text11_14[menu_ngonngu];
            loai_chay_vien.Items[14] = name_text11_15[menu_ngonngu];
            loai_chay_vien.Items[15] = name_text11_16[menu_ngonngu];
            loai_chay_vien.Items[16] = name_text11_17[menu_ngonngu];
            loai_chay_vien.Items[17] = name_text11_18[menu_ngonngu];
            loai_chay_vien.Items[18] = name_text11_19[menu_ngonngu];
            loai_chay_vien.Items[19] = name_text11_20[menu_ngonngu];
            loai_chay_vien.Items[20] = name_text11_21[menu_ngonngu];
            loai_chay_vien.Items[21] = name_text11_22[menu_ngonngu];
            loai_chay_vien.Items[22] = name_text11_23[menu_ngonngu];
            loai_chay_vien.Items[23] = name_text11_24[menu_ngonngu];
        }

        List<LAYER_CHINH> list_hieuung = new List<LAYER_CHINH>();
        LUU_MAPOK map;
        private void effect_Load(object sender, EventArgs e)
        {
            panel_wail.Visible = true;
            if (File.Exists(path_setting) == true)
            {
                IniParser parser = new IniParser(path_setting);

                int aa = Convert.ToInt32(parser.GetSetting("SETTING", "Language"));


                if (aa >= 0 && aa < 2)
                {

                    menu_ngonngu = aa;
                    set_ngongu();

                }

            }
            else
            {


            }

            Application.DoEvents();
            ControlMover.Init(this.panel_vien1);

           


            
            moi = new Bitmap(pic_map.Width, pic_map.Height);
            hien_list_path_effect();
            hien_list_vien();
            hien_list_halo();
            panel_effect.Location = panel_chinh.Location;
            panel_effect0.Location = panel_chinh.Location;
            panel_nap.Location = panel_chinh.Location;
            panel_text.Location = panel_chinh.Location;
            panel_vien.Location = panel_chinh.Location;
            panel_vien0.Location = panel_chinh.Location;
            panel_haoquang.Location = panel_chinh.Location;
            panel_haoquang1.Location = panel_chinh.Location;
            panel_vien1.Location = new Point(pic_map.Width - panel_vien1.Width + pic_map.Location.X, pic_map.Location.Y);

            panel_wail_nap.Location = new Point((this.Width - panel_wail_nap.Width)/2, (this.Height - panel_wail_nap.Height) / 2);
            panel_wail.Location = new Point((this.Width - panel_wail.Width) / 2, (this.Height - panel_wail.Height) / 2);



            panel_thucong_chon.Location = pictureBox7.Location;
            ma_rgb.SelectedIndex = 0;
            com_loaiden.SelectedIndex = 0;
            panel_wail.Visible = false;
            com_style_font.SelectedIndex = 0;
            com_font_render.SelectedIndex = 1;
          
            com_hu.SelectedIndex = 0;

            loai_chay_nen.SelectedIndex = 0;
            loai_chay_vien.SelectedIndex = 0;
            Application.DoEvents();

            load_file("3.led");

            
            timer_map.Enabled = true;
            timer.Enabled = true;
            this.ActiveControl = pic_map;
        }
 
        private void DrawArrow(Graphics gr, Pen pen, PointF p1, PointF p2, float length)
        {
            // Draw the shaft.
            gr.DrawLine(pen, p1, p2);

            // Find the arrow shaft unit vector.
            float vx = p2.X - p1.X;
            float vy = p2.Y - p1.Y;
            float dist = (float)Math.Sqrt(vx * vx + vy * vy);
            vx /= dist;
            vy /= dist;



            DrawArrowhead(gr, pen, p2, vx, vy, length);


        }
        private void DrawArrowhead(Graphics gr, Pen pen,
          PointF p, float nx, float ny, float length)
        {
            float ax = length * (-ny - nx);
            float ay = length * (nx - ny);
            PointF[] points =
            {
                new PointF(p.X + ax, p.Y + ay),
                p,
                new PointF(p.X - ay, p.Y + ax)
            };
            gr.DrawLines(pen, points);
        }
        private void DrawRoundedRectangle(Graphics g, Pen p,
            int x, int y, int w, int h, int rx, int ry)
        {
            System.Drawing.Drawing2D.GraphicsPath path
              = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(x, y, rx + rx, ry + ry, 180, 90);
            path.AddLine(x + rx, y, x + w - rx, y);
            path.AddArc(x + w - 2 * rx, y, 2 * rx, 2 * ry, 270, 90);
            path.AddLine(x + w, y + ry, x + w, y + h - ry);
            path.AddArc(x + w - 2 * rx, y + h - 2 * ry, rx + rx, ry + ry, 0, 91);
            path.AddLine(x + rx, y + h, x + w - rx, y + h);
            path.AddArc(x, y + h - 2 * ry, 2 * rx, 2 * ry, 90, 91);
            path.CloseFigure();
            g.DrawPath(p, path);
        }
        private void DrawRoundedRectangle_fill(Graphics g, System.Drawing.Brush p,
        int x, int y, int w, int h, int rx, int ry)
        {
            System.Drawing.Drawing2D.GraphicsPath path
              = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(x, y, rx + rx, ry + ry, 180, 90);
            path.AddLine(x + rx, y, x + w - rx, y);
            path.AddArc(x + w - 2 * rx, y, 2 * rx, 2 * ry, 270, 90);
            path.AddLine(x + w, y + ry, x + w, y + h - ry);
            path.AddArc(x + w - 2 * rx, y + h - 2 * ry, rx + rx, ry + ry, 0, 91);
            path.AddLine(x + rx, y + h, x + w - rx, y + h);
            path.AddArc(x, y + h - 2 * ry, 2 * rx, 2 * ry, 90, 91);
            path.CloseFigure();
            g.FillPath(p, path);
        }
        public static System.Drawing.Bitmap ChangeOpacity(System.Drawing.Image img, float opacityvalue)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }
        int w0 = 0, h0 = 0;
        private System.Drawing.Image RotateImg(System.Drawing.Image bmp, float angle, int zoom)
        {
            angle = angle % 360;
            if (angle > 180)
                angle -= 360;

            System.Drawing.Imaging.PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);

            pf = System.Drawing.Imaging.PixelFormat.Format32bppArgb;


            float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            float newImgWidth = sin * bmp.Height + cos * bmp.Width;
            float newImgHeight = sin * bmp.Width + cos * bmp.Height;
            float originX = 0f;
            float originY = 0f;

            if (angle > 0)
            {
                if (angle <= 90)
                    originX = sin * bmp.Height;
                else
                {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * bmp.Width;
                }
            }
            else
            {
                if (angle >= -90)
                    originY = sin * bmp.Width;
                else
                {
                    originX = newImgWidth - sin * bmp.Height;
                    originY = newImgHeight;
                }
            }
            w0 = (int)(newImgWidth / 2);
            h0 = (int)(newImgHeight / 2);
            System.Drawing.Bitmap newImg = new System.Drawing.Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(System.Drawing.Color.Transparent);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
            g.Dispose();

            return newImg;
        }
        private System.Drawing.Image RotateImg(System.Drawing.Image bmp, float angle)
        {
            angle = angle % 360;
            if (angle > 180)
                angle -= 360;

            System.Drawing.Imaging.PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);

            pf = System.Drawing.Imaging.PixelFormat.Format32bppArgb;


            float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            float newImgWidth = sin * bmp.Height + cos * bmp.Width;
            float newImgHeight = sin * bmp.Width + cos * bmp.Height;
            float originX = 0f;
            float originY = 0f;

            if (angle > 0)
            {
                if (angle <= 90)
                    originX = sin * bmp.Height;
                else
                {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * bmp.Width;
                }
            }
            else
            {
                if (angle >= -90)
                    originY = sin * bmp.Width;
                else
                {
                    originX = newImgWidth - sin * bmp.Height;
                    originY = newImgHeight;
                }
            }
            // w0 = (int)(newImgWidth / 2);
            //  h0 = (int)(newImgHeight / 2);
            System.Drawing.Bitmap newImg = new System.Drawing.Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(System.Drawing.Color.Transparent);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
            g.Dispose();

            return newImg;
        }
        public static System.Drawing.Bitmap ResizeImage0(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new System.Drawing.Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        static PointF RotatePoint(PointF pointToRotate, PointF centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new PointF
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
        private void Draw_anh(Graphics gr, ANH_ANH ll, bool hien)
        {
            PointF[] vt = new PointF[5] { new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1) };
            SolidBrush c = new SolidBrush(System.Drawing.Color.FromArgb(64, 128, 128, 128));
            SolidBrush c1 = new SolidBrush(System.Drawing.Color.FromArgb(128, 255, 0, 0));
            PointF center = new PointF((ll.CENTER.X + imgx) * zoom, (ll.CENTER.Y + imgy) * zoom);
            Pen p = new Pen(System.Drawing.Color.FromArgb(1, 151, 251));
            //Bitmap anh_temp = (Bitmap)ResizeImage0(ll.ANH, (int)(ll.SIZE.Width * zoom), (int)(ll.SIZE.Height * zoom));
            System.Drawing.Bitmap anh_temp = (System.Drawing.Bitmap)ll.ANH;
            anh_temp = (System.Drawing.Bitmap)ResizeImage0(ll.ANH, (int)(ll.SIZE.Width), (int)(ll.SIZE.Height));

            float w0 = anh_temp.Width;
            float h0 = anh_temp.Height;
            float opacityvalue = float.Parse(ll.DIM.ToString()) / 100;
            anh_temp = ChangeOpacity(anh_temp, opacityvalue);
            anh_temp = (System.Drawing.Bitmap)RotateImg(anh_temp, (float)ll.GOC);


            float w1 = anh_temp.Width;
            float h1 = anh_temp.Height;
            PointF a1 = new PointF((ll.CENTER.X - ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);
            PointF a2 = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);
            PointF a3 = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + ll.SIZE.Height / 2 + imgy) * zoom);
            PointF a4 = new PointF((ll.CENTER.X - ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + ll.SIZE.Height / 2 + imgy) * zoom);
            a1 = RotatePoint(a1, center, ll.GOC);
            a2 = RotatePoint(a2, center, ll.GOC);
            a3 = RotatePoint(a3, center, ll.GOC);
            a4 = RotatePoint(a4, center, ll.GOC);

            vt[0] = new PointF((ll.CENTER.X + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);
            vt[1] = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + imgy) * zoom);
            vt[2] = new PointF((ll.CENTER.X + imgx) * zoom, (ll.CENTER.Y + ll.SIZE.Height / 2 + imgy) * zoom);
            vt[3] = new PointF((ll.CENTER.X - ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + imgy) * zoom);
            vt[4] = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);


            vt[0] = RotatePoint(vt[0], center, ll.GOC);
            vt[1] = RotatePoint(vt[1], center, ll.GOC);
            vt[2] = RotatePoint(vt[2], center, ll.GOC);
            vt[3] = RotatePoint(vt[3], center, ll.GOC);
            vt[4] = RotatePoint(vt[4], center, ll.GOC);
            float ww = (w1 - w0) * zoom;
            float hh = (h1 - h0) * zoom;
            //label1.Text = ww.ToString() + "   " + hh.ToString();
            // gr.DrawImage(anh_temp, new PointF(center.X - ww / 2 - ll.SIZE.Width / 2 * zoom, center.Y - hh / 2 - ll.SIZE.Height / 2 * zoom));
            // gr.DrawImage(anh_temp, new Rectangle((int)(center.X - ww / 2 - ll.SIZE.Width / 2 * zoom), (int)(center.Y - hh / 2 - ll.SIZE.Height / 2 * zoom), (int)(ll.SIZE.Width * zoom), (int)(ll.SIZE.Height * zoom)),0,0, anh_temp.Width * zoom, anh_temp.Height * zoom, GraphicsUnit.Pixel);
            gr.DrawImage(anh_temp, new System.Drawing.RectangleF((int)(center.X - ww / 2 - ll.SIZE.Width / 2 * zoom), (int)(center.Y - hh / 2 - ll.SIZE.Height / 2 * zoom), anh_temp.Width * zoom, anh_temp.Height * zoom), new System.Drawing.RectangleF(0, 0, anh_temp.Width, anh_temp.Height), GraphicsUnit.Pixel);

            gr.DrawPolygon(p, new PointF[4] { a1, a2, a3, a4 });
            if (hien == true)
            {
                gr.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(1, 151, 251)), vt[4].X - 7, vt[4].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(1, 151, 251)), vt[0].X - 7, vt[0].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(1, 151, 251)), vt[1].X - 7, vt[1].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(1, 151, 251)), vt[2].X - 7, vt[2].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(1, 151, 251)), vt[3].X - 7, vt[3].Y - 7, 14, 14);

            }

        }
        private int get_color_effect(float px, float py, int vt )
        {
            if (thongtin_effect_goc!= null)
            {
               
                int j = (int)(vt * thongtin_effect_goc.SIZE.Width * thongtin_effect_goc.SIZE.Height * 4 + py * 4 + thongtin_effect_goc.SIZE.Height * px * 4 + 12+ 328);
                if (j >= 0 && j < thongtin_effect_goc.DATA.Length)
                {
                    return BitConverter.ToInt32(new byte[4] { thongtin_effect_goc.DATA[j], thongtin_effect_goc.DATA[j + 1], thongtin_effect_goc.DATA[j + 2], thongtin_effect_goc.DATA[j + 3] }, 0);
                }

            }

            return 0;
        }
        private Color travemau(int dd)
        {
            Color c = Color.Transparent;
            Byte[] bb = BitConverter.GetBytes(dd);
            if (bb[0] == 0) return c;
            else return Color.FromArgb(bb[1], bb[2], bb[3]);
            return c;
        }
      
        public static double GetAngle(PointF centre, PointF point1)
        {
            // Thanks to Dave Hill
            // Turn into a vector (from the origin)
            double x = point1.X - centre.X;
            double y = point1.Y - centre.Y;
            // Dot product u dot v = mag u * mag v * cos theta
            // Therefore theta = cos -1 ((u dot v) / (mag u * mag v))
            // Horizontal v = (1, 0)
            // therefore theta = cos -1 (u.x / mag u)
            // nb, there are 2 possible angles and if u.y is positive then angle is in first quadrant, negative then second quadrant
            double magnitude = Math.Sqrt(x * x + y * y);
            double angle = 0;
            if (magnitude > 0)
                angle = Math.Acos(x / magnitude);

            angle = angle * 180 / Math.PI;
            if (y < 0) angle = 360 - angle;
            angle += 90;
            if (angle >= 360) angle = angle - 360;

            return angle;
        }
        private int chuyendoi(int x, int cu, int moi)
        {
            float ff = (float)x * (float)moi / (float)cu;

            return (int)ff;
        }
        private float chuyendoiF(float x, float moi, float cu)
        {
            float ff = x * cu / moi;

            return ff;
        }
        private int[] get_data_effect(int vt)
        {
            int[] den = new int[mau_led.Length];
            float zx = 0;
            float zy = 0;
            float tem = 0;
            int cc = 0;
            int mo = 0;
            float goc = 0;
            float zx0 = 0;
            float zy0 = 0;

            if (thongtin_effect_goc.DAO == true) vt = thongtin_effect_goc.HIENTAI - 1 - vt;
            if (thongtin_effect_goc.CODAN==true)
            {
                vt =  vt%thongtin_effect_goc.TONG;
            }
            else
            {
                vt = (int)chuyendoiF(vt, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.TONG);
            }
            


            int mauu = (int)chuyendoiF(vt, thongtin_effect_goc.TONG, 359);
            PointF center = new PointF(thongtin_effect_edit.SIZE.Width / 2 + thongtin_effect_edit.LOCATION.X, thongtin_effect_edit.SIZE.Height / 2 + thongtin_effect_edit.LOCATION.Y);

            float newsize_x = center.X - thongtin_effect_edit.SIZE.Width / 2;
            float newsize_y = center.Y - thongtin_effect_edit.SIZE.Height / 2;


            for (int x = 0; x < thongtin_wire_vung.TONG; x++)
            {


                if (thongtin_effect_goc.TRON == false)
                {
                    zx = (float)led_full[thongtin_wire_vung.WIRE[x]].X - thongtin_effect_edit.LOCATION.X;
                    zy = (float)led_full[thongtin_wire_vung.WIRE[x]].Y - thongtin_effect_edit.LOCATION.Y;

                    zx = chuyendoiF(zx, thongtin_effect_edit.SIZE.Width, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_edit.SIZE.Height, thongtin_effect_goc.SIZE.Height);

                }
                else
                {
                    zx = (float)led_full[thongtin_wire_vung.WIRE[x]].X - newsize_x;
                    zy = (float)led_full[thongtin_wire_vung.WIRE[x]].Y - newsize_y;



                    goc = (float)GetAngle(center, new PointF((float)led_full[thongtin_wire_vung.WIRE[x]].X, (float)led_full[thongtin_wire_vung.WIRE[x]].Y));

                    //zx = chuyendoiF(zx, newsize_w, thongtin_effect_goc.SIZE.Width);
                    zx = chuyendoiF(goc * (thongtin_effect_edit.SIZE.Width / 2) / 180, thongtin_effect_edit.SIZE.Width, thongtin_effect_goc.SIZE.Width);
                    // zy = chuyendoiF(zy, newsize_h, thongtin_effect_goc.SIZE.Height);
                    zy = chuyendoiF(chieudaiF(center, new PointF((float)led_full[thongtin_wire_vung.WIRE[x]].X, (float)led_full[thongtin_wire_vung.WIRE[x]].Y)), thongtin_effect_edit.SIZE.Height / 2, thongtin_effect_goc.SIZE.Height);


                }

                if (thongtin_effect_goc.GOC == 1)
                {
                    tem = zy;
                    zy = zx;
                    zx = tem;
                    zx = chuyendoiF(zx, thongtin_effect_goc.SIZE.Height, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_goc.SIZE.Width, thongtin_effect_goc.SIZE.Height);
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }
                else if (thongtin_effect_goc.GOC == 2)
                {
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }
                else if (thongtin_effect_goc.GOC == 3)
                {
                    tem = zy;
                    zy = zx;
                    zx = tem;
                    zx = chuyendoiF(zx, thongtin_effect_goc.SIZE.Height, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_goc.SIZE.Width, thongtin_effect_goc.SIZE.Height);
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                }

                if (thongtin_effect_goc.DOC == true)
                {
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                }

                if (thongtin_effect_goc.NGANG == true)
                {
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }


                if (zx >= 0 && zx < thongtin_effect_goc.SIZE.Width && zy >= 0 && zy < thongtin_effect_goc.SIZE.Height)
                {

                    den[thongtin_wire_vung.WIRE[x]] = get_color_effect((int)zx, (int)zy, vt);
                    if (thongtin_effect_goc.MAU == true) den[thongtin_wire_vung.WIRE[x]] = doimau24(den[thongtin_wire_vung.WIRE[x]], mauu);
                }
                else
                {
                    den[thongtin_wire_vung.WIRE[x]] = 0;
                }


                if (thongtin_effect_goc.DMAU> 0 && den[thongtin_wire_vung.WIRE[x]] != 0 )
                {


                    den[thongtin_wire_vung.WIRE[x]] = doimau24(den[thongtin_wire_vung.WIRE[x]], thongtin_effect_goc.DMAU * 9);
                     
                }
            }
            

            return den;
        }
        private int[] get_data_thucong(int vt,int vt_thucong_hien)
        {
            int[] den = new int[mau_led.Length];
            float zx = 0;
            float zy = 0;
            float tem = 0;
            int cc = 0;
            int mo = 0;
            float goc = 0;
            float zx0 = 0;
            float zy0 = 0;

            if (thongtin_effect_goc.DAO == true) vt = thongtin_effect_goc.HIENTAI - 1 - vt;
            if (thongtin_effect_goc.CODAN == true)
            {
                vt = vt % thongtin_effect_goc.TONG;
            }
            else
            {
                vt = (int)chuyendoiF(vt, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.TONG);
            }



            int mauu = (int)chuyendoiF(vt, thongtin_effect_goc.TONG, 359);
            PointF center = new PointF(thongtin_effect_edit.SIZE.Width / 2 + thongtin_effect_edit.LOCATION.X, thongtin_effect_edit.SIZE.Height / 2 + thongtin_effect_edit.LOCATION.Y);

            float newsize_x = center.X - thongtin_effect_edit.SIZE.Width / 2;
            float newsize_y = center.Y - thongtin_effect_edit.SIZE.Height / 2;


            for (int x = 0; x < thongtin_wire_vung.WIREV[vt_thucong_hien].Count; x++)
            {


                if (thongtin_effect_goc.TRON == false)
                {
                    zx = (float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].X - thongtin_effect_edit.LOCATION.X;
                    zy = (float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].Y - thongtin_effect_edit.LOCATION.Y;

                    zx = chuyendoiF(zx, thongtin_effect_edit.SIZE.Width, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_edit.SIZE.Height, thongtin_effect_goc.SIZE.Height);

                }
                else
                {
                    zx = (float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].X - newsize_x;
                    zy = (float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].Y - newsize_y;



                    goc = (float)GetAngle(center, new PointF((float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].X, (float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].Y));

                    //zx = chuyendoiF(zx, newsize_w, thongtin_effect_goc.SIZE.Width);
                    zx = chuyendoiF(goc * (thongtin_effect_edit.SIZE.Width / 2) / 180, thongtin_effect_edit.SIZE.Width, thongtin_effect_goc.SIZE.Width);
                    // zy = chuyendoiF(zy, newsize_h, thongtin_effect_goc.SIZE.Height);
                    zy = chuyendoiF(chieudaiF(center, new PointF((float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].X, (float)led_full[thongtin_wire_vung.WIREV[vt_thucong_hien][x]].Y)), thongtin_effect_edit.SIZE.Height / 2, thongtin_effect_goc.SIZE.Height);


                }

                if (thongtin_effect_goc.GOC == 1)
                {
                    tem = zy;
                    zy = zx;
                    zx = tem;
                    zx = chuyendoiF(zx, thongtin_effect_goc.SIZE.Height, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_goc.SIZE.Width, thongtin_effect_goc.SIZE.Height);
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }
                else if (thongtin_effect_goc.GOC == 2)
                {
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }
                else if (thongtin_effect_goc.GOC == 3)
                {
                    tem = zy;
                    zy = zx;
                    zx = tem;
                    zx = chuyendoiF(zx, thongtin_effect_goc.SIZE.Height, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_goc.SIZE.Width, thongtin_effect_goc.SIZE.Height);
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                }

                if (thongtin_effect_goc.DOC == true)
                {
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                }

                if (thongtin_effect_goc.NGANG == true)
                {
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }


                if (zx >= 0 && zx < thongtin_effect_goc.SIZE.Width && zy >= 0 && zy < thongtin_effect_goc.SIZE.Height)
                {

                    den[thongtin_wire_vung.WIREV[vt_thucong_hien][x]] = get_color_effect((int)zx, (int)zy, vt);
                    if (thongtin_effect_goc.MAU == true) den[thongtin_wire_vung.WIREV[vt_thucong_hien][x]] = doimau24(den[thongtin_wire_vung.WIREV[vt_thucong_hien][x]], mauu);
                }
                else
                {
                    den[thongtin_wire_vung.WIREV[vt_thucong_hien][x]] = 0;
                }


                if (thongtin_effect_goc.DMAU > 0 && den[thongtin_wire_vung.WIREV[vt_thucong_hien][x]] != 0)
                {


                    den[thongtin_wire_vung.WIREV[vt_thucong_hien][x]] = doimau24(den[thongtin_wire_vung.WIREV[vt_thucong_hien][x]], thongtin_effect_goc.DMAU * 9);

                }
            }


            return den;
        }

       
        private int[] get_data_vien(int vt)
        {
            int[] den = new int[mau_led.Length];
            float zx = 0;
            float zy = 0;
            float tem = 0;
            int cc = 0;
            int mo = 0;
            float goc = 0;
            float zx0 = 0;
            float zy0 = 0;

            if (thongtin_effect_goc.DAO == true) vt = thongtin_effect_goc.HIENTAI - 1 - vt;

             vt = vt / (int)num_vien_speed.Value;

            int mauu = (int)chuyendoiF(vt, thongtin_effect_goc.HIENTAI, 359);
     
            int[] temp = get_vien(color_n, vt% tong_chay_nen, num_nen, tong_chay_nen, loai_chay_nen.SelectedIndex);
            int[] temp1 = get_vien(color_v, vt % tong_chay_vien, num_vien, tong_chay_vien, loai_chay_vien.SelectedIndex);


            
                int mm = 0;
                for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV.Count; y++)
                {

                    for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV[y].Count; x++)
                    {

                        mm = temp[x % num_vien];
                        if (temp1[x % num_vien] != 0) mm = temp1[x % num_vien];

                        if (thongtin_effect_goc.MAU == true) mm = doimau24(mm, mauu);

                        if (thongtin_effect_goc.DMAU > 0)
                        {


                            mm = doimau24(mm, thongtin_effect_goc.DMAU * 9);

                        }

                        den[list_hieuung[vt_chon_vung].WIREV[y][x]] = mm;


                    }
                }
             


             

            return den;
        }
        private int[] get_data_vay(int vt)
        {
            int[] den = new int[mau_led.Length];
            float zx = 0;
            float zy = 0;
            float tem = 0;
            int cc = 0;
            int mo = 0;
            float goc = 0;
            float zx0 = 0;
            float zy0 = 0;

            if (thongtin_effect_goc.DAO == true) vt = thongtin_effect_goc.HIENTAI - 1 - vt;

            vt = vt / (int)num_vien_speed.Value;

            int mauu = (int)chuyendoiF(vt, thongtin_effect_goc.HIENTAI, 359);

            int[] temp = get_vien(color_n, vt % tong_chay_nen, num_nen, tong_chay_nen, loai_chay_nen.SelectedIndex);
            int[] temp1 = get_vien(color_v, vt % tong_chay_vien, num_vien, tong_chay_vien, loai_chay_vien.SelectedIndex);


            if (check_group.Checked == true)
            {
                int mm = 0;
                for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV.Count; y++)
                {

                    for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV[y].Count; x++)
                    {

                        mm = temp[x % num_vien];
                        if (temp1[x % num_vien] != 0) mm = temp1[x % num_vien];

                        if (thongtin_effect_goc.MAU == true) mm = doimau24(mm, mauu);

                        if (thongtin_effect_goc.DMAU > 0)
                        {


                            mm = doimau24(mm, thongtin_effect_goc.DMAU * 9);

                        }

                        den[list_hieuung[vt_chon_vung].WIREV[y][x]] = mm;


                    }
                }
            }
            else
            {
                int mm = 0;
                for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV.Count; y++)
                {

                    for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV[y].Count; x++)
                    {

                        mm = temp[y % num_vien];
                        if (temp1[y % num_vien] != 0) mm = temp1[y % num_vien];

                        if (thongtin_effect_goc.MAU == true) mm = doimau24(mm, mauu);

                        if (thongtin_effect_goc.DMAU > 0)
                        {


                            mm = doimau24(mm, thongtin_effect_goc.DMAU * 9);

                        }

                        den[list_hieuung[vt_chon_vung].WIREV[y][x]] = mm;


                    }
                }
            }





            return den;
        }
        private int[] get_data_haoquang(int vt )
        {

                 int[] den = new int[mau_led.Length];

                 Color[,] temp = new Color[haoquang_tia, haoquang_hang];

                 
                for (int x = 0; x < haoquang_tia; x++)
                {
                   for (int y = 0; y < haoquang_hang; y++)
                    {
                       temp[x,y]=Color.Black;
                    }
                 }


                for (int x = 0; x < list_color_haoquang_play.Count; x++)
                {
                   if (vt % (int)num_vantoc_haoquang.Value == 0)
                     {
                    if (list_set_haoquang[x][1] == 1) list_color_haoquang_play[x] = chaytoi_moi(list_color_haoquang_play[x], haoquang_tia, haoquang_hang);
                    else if (list_set_haoquang[x][1] == 2) list_color_haoquang_play[x] = chaylui_moi(list_color_haoquang_play[x], haoquang_tia, haoquang_hang);

                    if (list_set_haoquang[x][0] == 1) list_color_haoquang_play[x] = toara_moi(list_color_haoquang_play[x], haoquang_tia, haoquang_hang);
                    else if (list_set_haoquang[x][0] == 2) list_color_haoquang_play[x] = toavao_moi(list_color_haoquang_play[x], haoquang_tia, haoquang_hang);


                    if (list_set_haoquang[x][2] == 1) list_color_haoquang_play[x] = doimau_haoquang_moi(list_color_haoquang_play[x], haoquang_tia, haoquang_hang);
                    else if (list_set_haoquang[x][2] == 2) list_color_haoquang_play[x] = toidan_moi(list_color_haoquang_play[x], haoquang_tia, haoquang_hang);
                    else if (list_set_haoquang[x][2] == 3) list_color_haoquang_play[x] = sangdan_moi(list_color_haoquang_play[x], haoquang_tia, haoquang_hang);
                    }
                }
            for (int i = 0; i < list_color_haoquang_play.Count; i++)
            {

                for (int x = 0; x < haoquang_tia; x++)
                {
                    for (int y = 0; y < haoquang_hang; y++)
                    {
                        if(list_color_haoquang_play[i][x,y] != Color.Transparent)  temp[x, y] = list_color_haoquang_play[i][x, y];
                    }
                }
            }

            
 

                int mm = 0;
                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                {
                    int xxx = x % haoquang_tia;
                    
                        for (int y = 0;y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                        {
                            int yyy = y % haoquang_hang;
                             
                                mm = BitConverter.ToInt32(new byte[4] { 255, temp[xxx,yyy].R, temp[xxx,yyy].G, temp[xxx,yyy].B }, 0);
                             
 
                            if (thongtin_effect_goc.DMAU > 0)
                            {


                                mm = doimau24(mm, thongtin_effect_goc.DMAU * 9);

                            }

                          den[list_hieuung[vt_chon_vung].WIREV[x][y]] = mm;


                        }
                    
                   
                }


 
 


            return den;
        }


        private List<int[]> render_effect()
        {
          
            float zx = 0;
            float zy = 0;
            float tem = 0;
            int cc = 0;
            int mo = 0;
            float goc = 0;
            float zx0 = 0;
            float zy0 = 0;
          
            PointF center = new PointF(thongtin_effect_edit.SIZE.Width / 2 + thongtin_effect_edit.LOCATION.X, thongtin_effect_edit.SIZE.Height / 2 + thongtin_effect_edit.LOCATION.Y);

            float newsize_x = center.X - thongtin_effect_edit.SIZE.Width / 2;
            float newsize_y = center.Y - thongtin_effect_edit.SIZE.Height / 2;

            int[] XX = new int[thongtin_wire_vung.TONG];
            int[] YY = new int[thongtin_wire_vung.TONG];

          

            for (int x = 0; x < thongtin_wire_vung.TONG; x++)
            {


                if (thongtin_effect_goc.TRON == false)
                {
                    zx = (float)led_full[thongtin_wire_vung.WIRE[x]].X - thongtin_effect_edit.LOCATION.X;
                    zy = (float)led_full[thongtin_wire_vung.WIRE[x]].Y - thongtin_effect_edit.LOCATION.Y;

                    zx = chuyendoiF(zx, thongtin_effect_edit.SIZE.Width, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_edit.SIZE.Height, thongtin_effect_goc.SIZE.Height);

                }
                else
                {
                    zx = (float)led_full[thongtin_wire_vung.WIRE[x]].X - newsize_x;
                    zy = (float)led_full[thongtin_wire_vung.WIRE[x]].Y - newsize_y;



                    goc = (float)GetAngle(center, new PointF((float)led_full[thongtin_wire_vung.WIRE[x]].X, (float)led_full[thongtin_wire_vung.WIRE[x]].Y));

                    //zx = chuyendoiF(zx, newsize_w, thongtin_effect_goc.SIZE.Width);
                    zx = chuyendoiF(goc * (thongtin_effect_edit.SIZE.Width / 2) / 180, thongtin_effect_edit.SIZE.Width, thongtin_effect_goc.SIZE.Width);
                    // zy = chuyendoiF(zy, newsize_h, thongtin_effect_goc.SIZE.Height);
                    zy = chuyendoiF(chieudaiF(center, new PointF((float)led_full[thongtin_wire_vung.WIRE[x]].X, (float)led_full[thongtin_wire_vung.WIRE[x]].Y)), thongtin_effect_edit.SIZE.Height / 2, thongtin_effect_goc.SIZE.Height);


                }

                if (thongtin_effect_goc.GOC == 1)
                {
                    tem = zy;
                    zy = zx;
                    zx = tem;
                    zx = chuyendoiF(zx, thongtin_effect_goc.SIZE.Height, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_goc.SIZE.Width, thongtin_effect_goc.SIZE.Height);
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }
                else if (thongtin_effect_goc.GOC == 2)
                {
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }
                else if (thongtin_effect_goc.GOC == 3)
                {
                    tem = zy;
                    zy = zx;
                    zx = tem;
                    zx = chuyendoiF(zx, thongtin_effect_goc.SIZE.Height, thongtin_effect_goc.SIZE.Width);
                    zy = chuyendoiF(zy, thongtin_effect_goc.SIZE.Width, thongtin_effect_goc.SIZE.Height);
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                }

                if (thongtin_effect_goc.DOC == true)
                {
                    zx = thongtin_effect_goc.SIZE.Width - zx - 1;
                }

                if (thongtin_effect_goc.NGANG == true)
                {
                    zy = thongtin_effect_goc.SIZE.Height - zy - 1;
                }

                XX[x] = (int)zx;
                YY[x] = (int)zy;

               

            }
            List<int[]> trave = new List<int[]>();
            for (int y = 0; y< thongtin_effect_goc.TONG; y++)
            {
                int mauu = (int)chuyendoiF(y, thongtin_effect_goc.TONG, 359);
                int[] den = new int[mau_led.Length];

                for (int x = 0; x < thongtin_wire_vung.TONG; x++)
                {
                    if (XX[x] >= 0 && XX[x] < thongtin_effect_goc.SIZE.Width && YY[x] >= 0 && YY[x] < thongtin_effect_goc.SIZE.Height)
                    {

                        den[thongtin_wire_vung.WIRE[x]] = get_color_effect(XX[x], YY[x], y);
                        if (thongtin_effect_goc.MAU == true) den[thongtin_wire_vung.WIRE[x]] = doimau24(den[thongtin_wire_vung.WIRE[x]], mauu);
                    }
                    else
                    {
                        den[thongtin_wire_vung.WIRE[x]] = 0;
                    }
                }
                trave.Add(den);
              //  progressBar2.Value = (int)(chuyendoiF(y, thongtin_effect_goc.TONG, 100));
                Application.DoEvents();
            }


                return trave;
        }


        public struct HSV { public float h; public float s; public float v; }
        public int doimau24(int i, int cc)
        {
            //if (cc == 0) return i;

            Byte[] b = BitConverter.GetBytes(i);
            Color color = Color.FromArgb(b[1], b[2], b[3]);
            var temp = new HSV();
            temp.h = color.GetHue();
            temp.s = color.GetSaturation();
            temp.v = color.GetBrightness();
            temp.h = temp.h - cc;
            Color color1 = ColorFromHSL(temp);


            return BitConverter.ToInt32(new byte[4] { b[0], color1.R, color1.G, color1.B }, 0);


        }
        static public Color ColorFromHSL(HSV hsl)
        {
            if (hsl.s == 0)
            { int L = (int)hsl.v; return Color.FromArgb(255, L, L, L); }

            double min, max, h;
            h = hsl.h / 360d;

            max = hsl.v < 0.5d ? hsl.v * (1 + hsl.s) : (hsl.v + hsl.s) - (hsl.v * hsl.s);
            min = (hsl.v * 2d) - max;


            Color c = Color.FromArgb(255, (int)(255 * RGBChannelFromHue(min, max, h + 1 / 3d)),
                                          (int)(255 * RGBChannelFromHue(min, max, h)),
                                          (int)(255 * RGBChannelFromHue(min, max, h - 1 / 3d)));



            return c;
        }

        static double RGBChannelFromHue(double m1, double m2, double h)
        {
            h = (h + 1d) % 1d;
            if (h < 0) h += 1;
            if (h * 6 < 1) return m1 + (m2 - m1) * 6 * h;
            else if (h * 2 < 1) return m2;
            else if (h * 3 < 2) return m1 + (m2 - m1) * 6 * (2d / 3d - h);
            else return m1;

        }
        private int[] get_vt_lop_timeline(int vt, int vt_lop)
        {
            int[] tt = new int[2] { -1, -1 };// vt hueuung, truoc, khoangr cah, ngoai
            int toi = 0;
          
            if (vt_lop >= 0 && vt_lop < list_hieuung.Count)
            {
                if (list_hieuung[vt_lop].HIEUUNG != null && list_hieuung[vt_lop].HIEUUNG.Count>0)
                    for (int x = 0; x < list_hieuung[vt_lop].HIEUUNG.Count; x++)
                {


                    if (vt >= toi + list_hieuung[vt_lop].HIEUUNG[x].BATDAU && vt < list_hieuung[vt_lop].HIEUUNG[x].BATDAU + list_hieuung[vt_lop].HIEUUNG[x].TONG_HIENTAI + toi)
                    {
                        tt[0] = x;
                        tt[1] = vt - (toi + list_hieuung[vt_lop].HIEUUNG[x].BATDAU);
                        return tt;
                    }

                    toi = toi + list_hieuung[vt_lop].HIEUUNG[x].BATDAU + list_hieuung[vt_lop].HIEUUNG[x].TONG_HIENTAI;
                }
            }
            return tt;
        }


       
 

        


        private void Draw_thucong(Graphics gr, bool hien, int vt_famer)
        {

            SolidBrush c = new SolidBrush(System.Drawing.Color.FromArgb(64, 128, 128, 128));
            SolidBrush c1 = new SolidBrush(System.Drawing.Color.FromArgb(128, 255, 0, 0));

            Pen p = new Pen(System.Drawing.Color.FromArgb(1, 151, 251));
          
            if (tong_chay_thucong>0)
            {
                
                int vt_thucong_hien =( vt_famer/ (int)speed_thucong.Value) %  tong_chay_thucong;
                int[] mtong = new int[mau_led.Length];

                int[] mmmm = get_data_thucong(vt_famer, vt_thucong_hien);
                int bd = get_tong_vung(vt_chon_vung);

                for (int x = 0; x < list_hieuung.Count; x++)
                {
                    int[] tt = get_vt_lop_timeline(vt_famer + thongtin_effect_goc.BATDAU + bd, x);
                    if (x != vt_chon_vung)
                    {
                        if (tt[0] >= 0 && tt[1] >= 0)
                        {

                            int ddd = tt[1];
                            if (list_hieuung[x].HIEUUNG[tt[0]].COGIAN == true)
                            {
                                ddd = (int)(chuyendoiF(ddd, list_hieuung[x].HIEUUNG[tt[0]].TONG_HIENTAI, list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC));
                            }
                            else
                            {
                                ddd = ddd % list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC;
                            }

                            int[] mm = list_hieuung[x].HIEUUNG[tt[0]].DATA[ddd];

                            for (int ii = 0; ii < mm.Length; ii++)
                            {
                                if (mm[ii] != 0) mtong[ii] = mm[ii];
                            }
                        }


                    }
                    else
                    {
                        for (int ii = 0; ii < mmmm.Length; ii++)
                        {
                            if (mmmm[ii] != 0) mtong[ii] = mmmm[ii];
                        }
                    }

                }

                for (int x = 0; x < mau_led.Length; x++) mau_led[x] = travemau(mtong[x]);
                if (hien == true)
                {


                }


            }
          

           

        }
        private void Draw_vien(Graphics gr, bool hien, int vt_famer)
        {

            SolidBrush c = new SolidBrush(System.Drawing.Color.FromArgb(64, 128, 128, 128));
            SolidBrush c1 = new SolidBrush(System.Drawing.Color.FromArgb(128, 255, 0, 0));

            Pen p = new Pen(System.Drawing.Color.FromArgb(1, 151, 251));
            

            int[] mtong = new int[mau_led.Length];

            int[] mmmm = get_data_vien(vt_famer);
            int bd = get_tong_vung(vt_chon_vung);

            for (int x = 0; x < list_hieuung.Count; x++)
            {
                int[] tt = get_vt_lop_timeline(vt_famer + thongtin_effect_goc.BATDAU + bd, x);
                if (x != vt_chon_vung)
                {
                    if (tt[0] >= 0 && tt[1] >= 0)
                    {

                        int ddd = tt[1];
                        if (list_hieuung[x].HIEUUNG[tt[0]].COGIAN == true)
                        {
                            ddd = (int)(chuyendoiF(ddd, list_hieuung[x].HIEUUNG[tt[0]].TONG_HIENTAI, list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC));
                        }
                        else
                        {
                            ddd = ddd % list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC;
                        }

                        int[] mm = list_hieuung[x].HIEUUNG[tt[0]].DATA[ddd];

                        for (int ii = 0; ii < mm.Length; ii++)
                        {
                            if (mm[ii] != 0) mtong[ii] = mm[ii];
                        }
                    }


                }
                else
                {
                    for (int ii = 0; ii < mmmm.Length; ii++)
                    {
                        if (mmmm[ii] != 0) mtong[ii] = mmmm[ii];
                      //  mtong[ii] = BitConverter.ToInt32(new byte[4] { 255, 255, 0, 0}, 0);
                    }
                }

            }

            for (int x = 0; x < mau_led.Length; x++) mau_led[x] = travemau(mtong[x]);
            if (hien == true)
            {


            }

        }
        private void Draw_vay(Graphics gr, bool hien, int vt_famer)
        {

            SolidBrush c = new SolidBrush(System.Drawing.Color.FromArgb(64, 128, 128, 128));
            SolidBrush c1 = new SolidBrush(System.Drawing.Color.FromArgb(128, 255, 0, 0));

            Pen p = new Pen(System.Drawing.Color.FromArgb(1, 151, 251));


            int[] mtong = new int[mau_led.Length];

            int[] mmmm = get_data_vay(vt_famer);
            int bd = get_tong_vung(vt_chon_vung);

            for (int x = 0; x < list_hieuung.Count; x++)
            {
                int[] tt = get_vt_lop_timeline(vt_famer + thongtin_effect_goc.BATDAU + bd, x);
                if (x != vt_chon_vung)
                {
                    if (tt[0] >= 0 && tt[1] >= 0)
                    {

                        int ddd = tt[1];
                        if (list_hieuung[x].HIEUUNG[tt[0]].COGIAN == true)
                        {
                            ddd = (int)(chuyendoiF(ddd, list_hieuung[x].HIEUUNG[tt[0]].TONG_HIENTAI, list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC));
                        }
                        else
                        {
                            ddd = ddd % list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC;
                        }

                        int[] mm = list_hieuung[x].HIEUUNG[tt[0]].DATA[ddd];

                        for (int ii = 0; ii < mm.Length; ii++)
                        {
                            if (mm[ii] != 0) mtong[ii] = mm[ii];
                        }
                    }


                }
                else
                {
                    for (int ii = 0; ii < mmmm.Length; ii++)
                    {
                        if (mmmm[ii] != 0) mtong[ii] = mmmm[ii];
                        //  mtong[ii] = BitConverter.ToInt32(new byte[4] { 255, 255, 0, 0}, 0);
                    }
                }

            }

            for (int x = 0; x < mau_led.Length; x++) mau_led[x] = travemau(mtong[x]);
            if (hien == true)
            {


            }

        }

        private void Draw_haoquang(Graphics gr, bool hien, int vt_famer)
        {

          

            int[] mtong = new int[mau_led.Length];

            int[] mmmm = get_data_haoquang( vt_famer);
            //MessageBox.Show(mmmm.Length.ToString());
            int bd = get_tong_vung(vt_chon_vung);

            for (int x = 0; x < list_hieuung.Count; x++)
            {
                int[] tt = get_vt_lop_timeline(vt_famer + thongtin_effect_goc.BATDAU + bd, x);
                if (x != vt_chon_vung)
                {
                    if (tt[0] >= 0 && tt[1] >= 0)
                    {

                        int ddd = tt[1];
                        if (list_hieuung[x].HIEUUNG[tt[0]].COGIAN == true)
                        {
                            ddd = (int)(chuyendoiF(ddd, list_hieuung[x].HIEUUNG[tt[0]].TONG_HIENTAI, list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC));
                        }
                        else
                        {
                            ddd = ddd % list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC;
                        }

                        int[] mm = list_hieuung[x].HIEUUNG[tt[0]].DATA[ddd];

                        for (int ii = 0; ii < mm.Length; ii++)
                        {
                            if (mm[ii] != 0) mtong[ii] = mm[ii];
                        }
                    }


                }
                else
                {
                    for (int ii = 0; ii < mmmm.Length; ii++)
                    {
                        if (mmmm[ii] != 0) mtong[ii] = mmmm[ii];
                     //  mtong[ii] = BitConverter.ToInt32(new byte[4] { 255, 255, 0, 0}, 0);
                    }
                }

            }

            for (int x = 0; x < mau_led.Length; x++) mau_led[x] = travemau(mtong[x]);
            if (hien == true)
            {


            }

        }

        bool nhay = true;
        bool nhay_map = true;
        System.Drawing.Bitmap moi;
        private int get_vung_chunhat(PointF vt)
        {
            float xxx = vt.X / zoom - imgx;
            float yyy = vt.Y / zoom - imgy;
         

            if (xxx >= thongtin_effect_edit.LOCATION.X + thongtin_effect_edit.SIZE.Width - 12 && xxx <= thongtin_effect_edit.LOCATION.X + thongtin_effect_edit.SIZE.Width + 12 && yyy >= thongtin_effect_edit.LOCATION.Y && yyy <= thongtin_effect_edit.LOCATION.Y + thongtin_effect_edit.SIZE.Height)
            {
                // MessageBox.Show("ngang");
                return 1;
            }
            else if (xxx >= thongtin_effect_edit.LOCATION.X && xxx <= thongtin_effect_edit.LOCATION.X + thongtin_effect_edit.SIZE.Width && yyy >= thongtin_effect_edit.LOCATION.Y + thongtin_effect_edit.SIZE.Height - 12 && yyy <= thongtin_effect_edit.LOCATION.Y + thongtin_effect_edit.SIZE.Height + 12)
            {
                // MessageBox.Show("ngang");
                return 2;
            }

            else if (xxx >= thongtin_effect_edit.LOCATION.X && xxx <= thongtin_effect_edit.LOCATION.X + thongtin_effect_edit.SIZE.Width - 12 && yyy >= thongtin_effect_edit.LOCATION.Y && yyy <= thongtin_effect_edit.LOCATION.Y + thongtin_effect_edit.SIZE.Height)
            {
                // MessageBox.Show("dichuyen");
                return 0;
            }

            return -1;
        }
        private int get_vung_tron(PointF vt)
        {
            /*
            float cc = chieudaiF(vt, new PointF(thongtin_effect_edit.CENTER   xv.X + RN / 2, xv.Y + RN / 2));

            if (cc >= RN / 2 - 5 && cc < RN / 2 + 10)
            {
                //  MessageBox.Show("ngang");
                return 1;
            }
            else if (cc < RN / 2)
            {
                // MessageBox.Show("ngang");
                return 0;
            }


            */
            return -1;
        }
        int vt_luu_keo = -1;
        private void pic_map_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Right)
            {


                if (!mousepressed)
                {
                    mousepressed = true;
                    mouseDown = mouse.Location;
                    startx = imgx;
                    starty = imgy;
                }
            }
            else
            {
                if (vt_menu == menu_effect_wire)
                {
                        
                        day_select.Clear();
                        vt_select_vung[0] = vt_select_vung[1] = new PointF(-1, -1);
                        keo_vung = true;
                        vt_chuot = e.Location;

                        float vtx0 = vt_chuot.X / zoom - imgx;
                        float vty0 = vt_chuot.Y / zoom - imgy;


                        for (int x = 0; x < led_full.Count; x++)
                        {

                            if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                            {
                                day_select.Add(x);
                                break;

                            }

                        }

                }
                else if (vt_menu == menu_effect_edit)
                {

                    keo_hu = get_vung_chunhat(e.Location);

                    // MessageBox.Show(keo_hu.ToString());
                    vt_chuot = e.Location;
                }
                else if (vt_menu == menu_thucong_edit)
                {

                    keo_hu = get_vung_chunhat(e.Location);

                    // MessageBox.Show(keo_hu.ToString());
                    vt_chuot = e.Location;
                }
                else if (vt_menu == menu_text_wire)
                {

                    day_select.Clear();
                    vt_select_vung[0] = vt_select_vung[1] = new PointF(-1, -1);
                    keo_vung = true;
                    vt_chuot = e.Location;

                    float vtx0 = vt_chuot.X / zoom - imgx;
                    float vty0 = vt_chuot.Y / zoom - imgy;


                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                        {
                            day_select.Add(x);
                            break;

                        }

                    }

                }
                else if (vt_menu == menu_text_edit)
                {

                    keo_hu = get_vung_chunhat(e.Location);

                    // MessageBox.Show(keo_hu.ToString());
                    vt_chuot = e.Location;
                }
             
                else if (vt_menu == menu_vien_wire)
                {
                    
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {

                            vt_auto1 = vtc;
                            day_select.Clear();

                            vt_luu_keo = vtc;
                            day_select.Add(vtc);
                            keo_thucong = true;


                        }
                        else
                        {
                            vt_luu_keo = -1;
                        }
                    

                }
                else if (vt_menu == menu_vay_wire)
                {
                    
                     
                        day_select.Clear();
                        vt_select_vung[0] = vt_select_vung[1] = new PointF(-1, -1);
                        keo_vung = true;
                        vt_chuot = e.Location;

                        float vtx0 = vt_chuot.X / zoom - imgx;
                        float vty0 = vt_chuot.Y / zoom - imgy;


                        for (int x = 0; x < led_full.Count; x++)
                        {

                            if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                            {
                                day_select.Add(x);
                                break;

                            }

                        }
 

                }
                else if (vt_menu == menu_thucong_wire)
                {


                    if (play_thucong == true)
                    {
                        dem_play_thucong = 0;
                        play_thucong = false;
                        button27.Text = "Play";
                    }
                    day_select.Clear();
                    vt_select_vung[0] = vt_select_vung[1] = new PointF(-1, -1);
                    keo_vung = true;
                    vt_chuot = e.Location;

                    float vtx0 = vt_chuot.X / zoom - imgx;
                    float vty0 = vt_chuot.Y / zoom - imgy;


                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                        {
                            day_select.Add(x);
                            break;

                        }

                    }



                }
                else if (vt_menu == menu_haoquang_wire)
                {

                    int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                    if (vtc >= 0)
                    {

                        vt_auto1 = vtc;
                        day_select.Clear();

                        vt_luu_keo = vtc;
                        day_select.Add(vtc);
                        keo_thucong = true;


                    }
                    else
                    {
                        vt_luu_keo = -1;
                    }


                }
                else if (vt_menu == menu_haoquang_add)
                {
                    if (play_haoquang == false)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {
                            if (list_hieung_haoquang.SelectedIndex >= 0)
                            {
                                int[] vv = check_den_haoquang(vtc);
                                if (vv[0] >= 0 && vv[1] >= 0)
                                {
                                    if (halo_draw == 0)
                                    {
                                        halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                    }
                                    else if (halo_draw == 1)
                                    {
                                        halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                      
                                       int tiamoi = halo_tia - vv[0] ;
                                        if(tiamoi>=0 && tiamoi< halo_tia) halo_color[list_hieung_haoquang.SelectedIndex][tiamoi, vv[1]] = mauvien_chon;

                                    }
                                    else if (halo_draw == 2)
                                    {
                                        // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                        int vh = vv[1];
                                        int vc = vv[0];
                                        for (int x = 0; x < halo_tia; x++)
                                        {

                                            halo_color[list_hieung_haoquang.SelectedIndex][vc, vh] = mauvien_chon;

                                            vh++;if (vh >= halo_hang) vh = 0;
                                            vc++; if (vc >= halo_tia) vc = 0;
                                        }

                                    }
                                    else if (halo_draw ==3)
                                    {
                                        // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                        int vh = vv[1];
                                        int vc = vv[0];
                                        for (int x = 0; x < halo_tia; x++)
                                        {

                                            halo_color[list_hieung_haoquang.SelectedIndex][vc, vh] = mauvien_chon;

                                            vh--; if (vh <0) vh = halo_hang - 1;
                                            vc++; if (vc >= halo_tia) vc = 0;
                                        }

                                    }
                                    else if (halo_draw == 4)
                                    {
                                        // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                        int vh = vv[1];
                                        int vc = vv[0];
                                        for (int x = 0; x < halo_tia; x++)
                                        {

                                            halo_color[list_hieung_haoquang.SelectedIndex][vc, vh] = mauvien_chon;

                                            vh--; if (vh < 0) vh = halo_hang - 1;
                                            vc++; if (vc >= halo_tia) vc = 0;
                                        }
                                         vh = vv[1];
                                         vc = vv[0];
                                        for (int x = 0; x < halo_tia; x++)
                                        {

                                            halo_color[list_hieung_haoquang.SelectedIndex][vc, vh] = mauvien_chon;

                                            vh++; if (vh >= halo_hang) vh = 0;
                                            vc++; if (vc >= halo_tia) vc = 0;
                                        }
                                    }
                                    else if (halo_draw == 5)
                                    {
                                        // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                       
                                        for (int x = 0; x < halo_tia; x++)
                                        {

                                            halo_color[list_hieung_haoquang.SelectedIndex][x, vv[1]] = mauvien_chon;
 
                                        }

                                    }
                                    else if (halo_draw == 6)
                                    {
                                        // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;

                                        for (int x = 0; x < halo_hang; x++)
                                        {

                                            halo_color[list_hieung_haoquang.SelectedIndex][vv[0],x] = mauvien_chon;

                                        }

                                    }
                                    else if (halo_draw == 7)
                                    {
                                        // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;

                                       

                                        for (int x = 0; x < halo_tia/num_nhan.Value; x++)
                                        {
                                            
                                          if(x * (int)num_nhan.Value + vv[0] < halo_tia)  halo_color[list_hieung_haoquang.SelectedIndex][x* (int)num_nhan.Value + vv[0], vv[1]] = mauvien_chon;
                                          else halo_color[list_hieung_haoquang.SelectedIndex][x * (int)num_nhan.Value + vv[0]- halo_tia, vv[1]] = mauvien_chon;



                                        }

                                    }
                                }
                            }


                        }
                        else
                        {

                        }

                    }
                }
                else if (vt_menu == menu_nap)
                {
                    if (vt_menu1 == menu_wire_port1)
                    {
                        panel_wail.Visible = true;
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {

                            led_full1.Clear();
                            for (int i = 0; i < led_full.Count; i++) led_full1.Add(new LED1(led_full[i].X, led_full[i].Y, i));


                            List<LED1> vt_den_temp = new List<LED1>();
                            List<LED1> vt_den_temp1 = new List<LED1>();
                            for (int i = 0; i < led_full1.Count; i++) vt_den_temp.Add(led_full1[i]);
                            int chay = led_full1.Count - 1;
                            double max = 0;
                            int vt = 0;
                            vt_den_temp1.Add(vt_den_temp[vtc]);
                            vt_den_temp.RemoveAt(vtc);
                            while (chay > 0)
                            {
                                max = 100000;
                                vt = 0;
                                for (int i = 0; i < vt_den_temp.Count; i++)
                                {
                                    // MessageBox.Show(chieudai(vt_den_auto[vt_den_auto.Count - 1], vt_den_temp[i]).ToString());
                                    if (chieudailed(vt_den_temp1[vt_den_temp1.Count - 1], vt_den_temp[i]) <= max) { max = chieudailed(vt_den_temp1[vt_den_temp1.Count - 1], vt_den_temp[i]); vt = i; };
                                }
                                vt_den_temp1.Add(vt_den_temp[vt]);
                                vt_den_temp.RemoveAt(vt);
                                //chuyen3d(vt, vt_den_temp1.Count - 1);
                                chay--;
                            }
                            // vt_den.Clear();
                            for (int i = 0; i < led_full1.Count; i++) led_full1[i] = vt_den_temp1[i];

                            for (int i = 0; i < led_full1.Count; i++) led_full[i] = new LED(led_full1[i].X, led_full1[i].Y);




                            for (int x = 0; x < list_hieuung.Count; x++)
                            {
                                for (int y = 0; y < list_hieuung[x].HIEUUNG.Count; y++)
                                {
                                    for (int z = 0; z < list_hieuung[x].HIEUUNG[y].DATA.Count; z++)
                                    {
                                        int[] temp = new int[list_hieuung[x].HIEUUNG[y].DATA[z].Length];
                                        for (int i = 0; i < list_hieuung[x].HIEUUNG[y].DATA[z].Length; i++)
                                        {
                                            temp[i] = list_hieuung[x].HIEUUNG[y].DATA[z][(int)led_full1[i].VT];

                                        }
                                        list_hieuung[x].HIEUUNG[y].DATA[z] = temp;
                                    }
                                }

                            }
                            // checkloi();


                        }
                        panel_wail.Visible = false;
                    }
                    else if (vt_menu1 == menu_wire_port2)
                    {
                        panel_wail.Visible = true;
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc > 0)
                        {
                            vt_port2 = vtc;

                            // checkloi();


                        }
                        else
                        {
                            vt_port2 = -1;
                        }
                        panel_wail.Visible = false;
                    }
                    else if (vt_menu1 == menu_wire_connect0)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {
                          
                            vt_auto1 = vtc;
                            led_select.Clear();

                            for (int i = 0; i < vtc; i++) led_select.Add(i);
                            led_select.Add(vtc);
                            keo_nap = true;


                        }

                    }
                    else if (vt_menu1 == menu_wire_connect1)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {
                            

                            vt_auto1 = vtc;
                            led_select.Clear();

                            for (int i = 0; i < vtc; i++) led_select.Add(i);



                            keo_nap = true;


                        }

                    }


                }


            }
        }
        bool keo_nap = false;
        double chieudailed(LED1 a1, LED1 a2)
        {

            return (double)(Math.Sqrt((a2.X - a1.X) * (a2.X - a1.X) + (a2.Y - a1.Y) * (a2.Y - a1.Y)));
        }
        private int[] check_den_haoquang(int vt)
        {
          

            for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
            {
                for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                {
                    if (vt == list_hieuung[vt_chon_vung].WIREV[x][y]) return new int[2] { x, y };
                }

            }


            return new int[2] { -1, -1 };
        }


        int vt_auto1 = -1;
        bool keo_thucong = false;

        private int check_den(float vx, float vy)
        {
            float vtx = vx / zoom - imgx;
            float vty = vy / zoom - imgy;
            for (int x = 0; x < led_full.Count; x++)
            {

                if (vtx >= led_full[x].X - 6 && vtx <= led_full[x].X + 6 && vty >= led_full[x].Y - 6 && vty <= led_full[x].Y + 6)
                {
                    return x;

                }

            }

            return -1;
        }


        int keo_hu = -1;
       

        public static bool IsPointInPolygon4(PointF[] polygon, PointF testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
        
        private float tim_gocf(PointF diem1, PointF diem2)
        {
            float goc = 0;
            ;

            float deltaY = diem2.Y - diem1.Y;
            float deltaX = diem2.X - diem1.X;
            goc = (float)(Math.Atan2((double)deltaY, (double)deltaX) * 180 / Math.PI);
            return goc;
        }
        private PointF[] get_select(PointF v1, PointF v2)
        {
            PointF[] aa = new PointF[2];

            float xx1 = 0, xx2 = 0, yy1 = 0, yy2 = 0;

            if (v1.X > v2.X) { xx1 = v1.X; xx2 = v2.X; }
            else { xx1 = v2.X; xx2 = v1.X; }
            if (v1.Y > v2.Y) { yy1 = v1.Y; yy2 = v2.Y; }
            else { yy1 = v2.Y; yy2 = v1.Y; }

            aa[0] = new PointF(xx2, yy2);
            aa[1] = new PointF(xx1, yy1);
            return aa;

        }
        float chieudaif(PointF a1, PointF a2)
        {

            return (float)(Math.Sqrt((a2.X - a1.X) * (a2.X - a1.X) + (a2.Y - a1.Y) * (a2.Y - a1.Y)));
        }
        List<int> day_select = new List<int>();
        PointF[] vt_select_vung = new PointF[2];
        bool keo_vung = false;
        private void pic_map_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;
            if (mousepressed == true)
            {
                System.Drawing.Point mousePosNow = e.Location;

                int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                int deltaY = mousePosNow.Y - mouseDown.Y;

                imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
                imgy = (int)(starty + (deltaY / zoom));
            }

            if (vt_menu == menu_effect_wire)
            {
                if (keo_vung == true)
                {
                    vt_chuot1 = e.Location;


                    vt_select_vung = get_select(vt_chuot, vt_chuot1);

                    float vtx0 = vt_chuot.X / zoom - imgx;
                    float vty0 = vt_chuot.Y / zoom - imgy;
                    float vtx1 = vt_select_vung[0].X / zoom - imgx;
                    float vty1 = vt_select_vung[0].Y / zoom - imgy;
                    float vtx2 = vt_select_vung[1].X / zoom - imgx;
                    float vty2 = vt_select_vung[1].Y / zoom - imgy;
                    day_select.Clear();

                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                        {
                            day_select.Add(x);
                            break;

                        }

                    }
                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (led_full[x].X >= vtx1 - 12 && led_full[x].X < vtx2 && led_full[x].Y >= vty1 - 12 && led_full[x].Y < vty2)
                        {
                            day_select.Add(x);

                        }

                    }


                }
            }
            else if (vt_menu == menu_effect_edit)
            {
                if (keo_hu == 0)
                {
                    thongtin_effect_edit.LOCATION = new PointF(thongtin_effect_edit.LOCATION.X + (e.X / zoom - imgx - (vt_chuot.X / zoom - imgx)), thongtin_effect_edit.LOCATION.Y + (e.Y / zoom - imgy - (vt_chuot.Y / zoom - imgy)));


                    vt_chuot = e.Location;
                }
                else if (keo_hu == 1)
                {
                    thongtin_effect_edit.SIZE = new SizeF(thongtin_effect_edit.SIZE.Width + (e.X / zoom - imgx - (vt_chuot.X / zoom - imgx)), thongtin_effect_edit.SIZE.Height);


                    vt_chuot = e.Location;
                }
                else if (keo_hu == 2)
                {
                    thongtin_effect_edit.SIZE = new SizeF(thongtin_effect_edit.SIZE.Width, thongtin_effect_edit.SIZE.Height + (e.Y / zoom - imgy - (vt_chuot.Y / zoom - imgy)));
                    vt_chuot = e.Location;
                }
            }
            else if (vt_menu == menu_thucong_edit)
            {
                if (keo_hu == 0)
                {
                    thongtin_effect_edit.LOCATION = new PointF(thongtin_effect_edit.LOCATION.X + (e.X / zoom - imgx - (vt_chuot.X / zoom - imgx)), thongtin_effect_edit.LOCATION.Y + (e.Y / zoom - imgy - (vt_chuot.Y / zoom - imgy)));


                    vt_chuot = e.Location;
                }
                else if (keo_hu == 1)
                {
                    thongtin_effect_edit.SIZE = new SizeF(thongtin_effect_edit.SIZE.Width + (e.X / zoom - imgx - (vt_chuot.X / zoom - imgx)), thongtin_effect_edit.SIZE.Height);


                    vt_chuot = e.Location;
                }
                else if (keo_hu == 2)
                {
                    thongtin_effect_edit.SIZE = new SizeF(thongtin_effect_edit.SIZE.Width, thongtin_effect_edit.SIZE.Height + (e.Y / zoom - imgy - (vt_chuot.Y / zoom - imgy)));
                    vt_chuot = e.Location;
                }
            }

            else if (vt_menu == menu_text_wire)
            {
                if (keo_vung == true)
                {
                    vt_chuot1 = e.Location;


                    vt_select_vung = get_select(vt_chuot, vt_chuot1);

                    float vtx0 = vt_chuot.X / zoom - imgx;
                    float vty0 = vt_chuot.Y / zoom - imgy;
                    float vtx1 = vt_select_vung[0].X / zoom - imgx;
                    float vty1 = vt_select_vung[0].Y / zoom - imgy;
                    float vtx2 = vt_select_vung[1].X / zoom - imgx;
                    float vty2 = vt_select_vung[1].Y / zoom - imgy;
                    day_select.Clear();

                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                        {
                            day_select.Add(x);
                            break;

                        }

                    }
                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (led_full[x].X >= vtx1 - 12 && led_full[x].X < vtx2 && led_full[x].Y >= vty1 - 12 && led_full[x].Y < vty2)
                        {
                            day_select.Add(x);

                        }

                    }


                }
            }
            else if (vt_menu == menu_text_edit)
            {
                if (keo_hu == 0)
                {
                    thongtin_effect_edit.LOCATION = new PointF(thongtin_effect_edit.LOCATION.X + (e.X / zoom - imgx - (vt_chuot.X / zoom - imgx)), thongtin_effect_edit.LOCATION.Y + (e.Y / zoom - imgy - (vt_chuot.Y / zoom - imgy)));


                    vt_chuot = e.Location;
                }
                else if (keo_hu == 1)
                {
                    thongtin_effect_edit.SIZE = new SizeF(thongtin_effect_edit.SIZE.Width + (e.X / zoom - imgx - (vt_chuot.X / zoom - imgx)), thongtin_effect_edit.SIZE.Height);


                    vt_chuot = e.Location;
                }
                else if (keo_hu == 2)
                {
                    thongtin_effect_edit.SIZE = new SizeF(thongtin_effect_edit.SIZE.Width, thongtin_effect_edit.SIZE.Height + (e.Y / zoom - imgy - (vt_chuot.Y / zoom - imgy)));
                    vt_chuot = e.Location;
                }
            }
            else if (vt_menu == menu_thucong_wire)
            {

                if (keo_vung == true)
                {
                    vt_chuot1 = e.Location;


                    vt_select_vung = get_select(vt_chuot, vt_chuot1);

                    float vtx0 = vt_chuot.X / zoom - imgx;
                    float vty0 = vt_chuot.Y / zoom - imgy;
                    float vtx1 = vt_select_vung[0].X / zoom - imgx;
                    float vty1 = vt_select_vung[0].Y / zoom - imgy;
                    float vtx2 = vt_select_vung[1].X / zoom - imgx;
                    float vty2 = vt_select_vung[1].Y / zoom - imgy;
                    day_select.Clear();

                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                        {
                            day_select.Add(x);
                            break;

                        }

                    }
                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (led_full[x].X >= vtx1 - 12 && led_full[x].X < vtx2 && led_full[x].Y >= vty1 - 12 && led_full[x].Y < vty2)
                        {
                            day_select.Add(x);

                        }

                    }


                }

            }
            else if (vt_menu == menu_haoquang_wire)
            {

                if (keo_thucong == true)
                {

                    if (keo_thang.Checked == false)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0 && check_den_select(vtc) == -1 && (ModifierKeys & Keys.Shift) != Keys.Shift)
                        {

                            day_select.Add(vtc);
                        }
                    }
                    else
                    {

                        day_select.Clear();
                        if (vt_luu_keo >= 0 && vt_luu_keo < led_full.Count) day_select.Add(vt_luu_keo);



                        double xxx = mouse.Location.X / zoom - imgx;
                        double yyy = mouse.Location.Y / zoom - imgy;

                        PointF z1 = new PointF((int)led_full[vt_auto1].X, (int)led_full[vt_auto1].Y);
                        PointF z2 = new PointF((int)xxx, (int)yyy);

                        for (int x = 0; x < led_full.Count; x++)
                        {

                            if (PointOnLineSegment1(z1, z2, new PointF((float)led_full[x].X, (float)led_full[x].Y), 6) == true && check_den_select(x) == -1)
                            {
                                day_select.Add(x);
                            }
                        }
                    }



                }
                vt_chuot = mouse.Location;

            }
            else if(vt_menu== menu_haoquang_add)
            {
                if (play_haoquang == false)
                {
                    int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                    if (vtc >= 0)
                    {
                       vt_haoquang = check_den_haoquang(vtc);

                    }
                    else
                    {
                        vt_haoquang[0] = -1;
                        vt_haoquang[1] = -1;
                    }
                }
                else
                {
                    vt_haoquang[0] = -1;
                    vt_haoquang[1] = -1;
                }
            }
            else if (vt_menu == menu_vien_wire)
            {

                if (keo_thucong == true)
                {

                    if (keo_thang.Checked == false)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0 && check_den_select(vtc) == -1 && (ModifierKeys & Keys.Shift) != Keys.Shift)
                        {

                            day_select.Add(vtc);
                        }
                    }
                    else
                    {

                        day_select.Clear();
                        if (vt_luu_keo >= 0 && vt_luu_keo < led_full.Count) day_select.Add(vt_luu_keo);



                        double xxx = mouse.Location.X / zoom - imgx;
                        double yyy = mouse.Location.Y / zoom - imgy;

                        PointF z1 = new PointF((int)led_full[vt_auto1].X, (int)led_full[vt_auto1].Y);
                        PointF z2 = new PointF((int)xxx, (int)yyy);

                        for (int x = 0; x < led_full.Count; x++)
                        {

                            if (PointOnLineSegment1(z1, z2, new PointF((float)led_full[x].X, (float)led_full[x].Y), 6) == true && check_den_select(x) == -1)
                            {
                                day_select.Add(x);
                            }
                        }
                    }



                }
                vt_chuot = mouse.Location;

            }
            else if (vt_menu == menu_vay_wire)
            {

                if (keo_vung == true)
                {
                    vt_chuot1 = e.Location;


                    vt_select_vung = get_select(vt_chuot, vt_chuot1);

                    float vtx0 = vt_chuot.X / zoom - imgx;
                    float vty0 = vt_chuot.Y / zoom - imgy;
                    float vtx1 = vt_select_vung[0].X / zoom - imgx;
                    float vty1 = vt_select_vung[0].Y / zoom - imgy;
                    float vtx2 = vt_select_vung[1].X / zoom - imgx;
                    float vty2 = vt_select_vung[1].Y / zoom - imgy;
                    day_select.Clear();

                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (vtx0 >= led_full[x].X && vtx0 <= led_full[x].X + 12 && vty0 >= led_full[x].Y && vty0 <= led_full[x].Y + 12)
                        {
                            day_select.Add(x);
                            break;

                        }

                    }
                    for (int x = 0; x < led_full.Count; x++)
                    {

                        if (led_full[x].X >= vtx1 - 12 && led_full[x].X < vtx2 && led_full[x].Y >= vty1 - 12 && led_full[x].Y < vty2)
                        {
                            day_select.Add(x);

                        }

                    }


                }

            }
            else if (vt_menu == menu_nap)
            {
                if (vt_menu1 == menu_wire_connect0)
                {
                    if (keo_nap == true)
                    {

                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0 && check_den_select1(vtc) == -1 && (ModifierKeys & Keys.Shift) != Keys.Shift)
                        {

                            led_select.Add(vtc);
                        }



                    }
                    vt_chuot = mouse.Location;
                }
                else if (vt_menu1 == menu_wire_connect1)
                {
                    if (keo_nap == true)
                    {





                        cu.Clear();



                        double xxx = mouse.Location.X / zoom - imgx;
                        double yyy = mouse.Location.Y / zoom - imgy;

                        PointF z1 = new PointF((int)led_full[vt_auto1].X, (int)led_full[vt_auto1].Y);
                        PointF z2 = new PointF((int)xxx, (int)yyy);

                        for (int x = 0; x < led_full.Count; x++)
                        {

                            if (PointOnLineSegment1(z1, z2, new PointF((float)led_full[x].X, (float)led_full[x].Y), 6) == true && check_den_select1(x) == -1)
                            {
                                cu.Add(x);
                            }
                        }




                    }
                    vt_chuot = mouse.Location;
                }else
                {
                    int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                    if (vtc >= 0)
                    {
                        if (vtc != toptipcu)
                        {
                             

                                toolTip1.SetToolTip(pic_map, "LED: " + vtc.ToString() + " ");

                            toptipcu = vtc;
                        }
                    }
                    else
                    {
                        toptipcu = -1;
                    }
                }


            }
        }
        int toptipcu = -1;
        int[] vt_haoquang = new int[2] { -1, -1 };
        public static bool PointOnLineSegment1(PointF pt1, PointF pt2, PointF pt, double epsilon = 0.001)
        {
            if (pt.X - Math.Max(pt1.X, pt2.X) > epsilon ||
                Math.Min(pt1.X, pt2.X) - pt.X > epsilon ||
                pt.Y - Math.Max(pt1.Y, pt2.Y) > epsilon ||
                Math.Min(pt1.Y, pt2.Y) - pt.Y > epsilon)
                return false;

            if (Math.Abs(pt2.X - pt1.X) < epsilon)
                return Math.Abs(pt1.X - pt.X) < epsilon || Math.Abs(pt2.X - pt.X) < epsilon;
            if (Math.Abs(pt2.Y - pt1.Y) < epsilon)
                return Math.Abs(pt1.Y - pt.Y) < epsilon || Math.Abs(pt2.Y - pt.Y) < epsilon;

            double x = pt1.X + (pt.Y - pt1.Y) * (pt2.X - pt1.X) / (pt2.Y - pt1.Y);
            double y = pt1.Y + (pt.X - pt1.X) * (pt2.Y - pt1.Y) / (pt2.X - pt1.X);

            return Math.Abs(pt.X - x) < epsilon || Math.Abs(pt.Y - y) < epsilon;
        }
        private int check_den_select(int vt)
        {

            for (int x = 0; x < day_select.Count; x++)
            {
                if (vt == day_select[x]) return x;
            }


            return -1;
        }
        private int check_den_select1(int vt)
        {

            for (int x = 0; x < led_select.Count; x++)
            {
                if (vt == led_select[x]) return x;
            }


            return -1;
        }
        bool key_Ctrl = false;
        List<int> cu = new List<int>();
        private void pic_map_MouseUp(object sender, MouseEventArgs e)
        {
            if (mousepressed == true) mousepressed = false;

            if (keo_hu >= 0)
            {
                keo_hu = -1;

            }
            if (vt_menu == menu_effect_wire)
            {
                if (keo_vung == true)
                {

                    keo_vung = false;
                    //  vt_chuot = e.Location;
                    // add_den_select(e.Location);
                    //add_vung_select(vt_chuot, e.Location);
                    
                        List<int> temp = list_hieuung[vt_chon_vung].WIRE;


                        for (int i = 0; i < day_select.Count; i++)
                        {
                            int kk = -1;
                            for (int x = 0; x < temp.Count; x++)
                            {
                                if (temp[x] == day_select[i])
                                {
                                    kk = x;
                                    break;
                                }
                            }
                            if ((ModifierKeys & Keys.Control) != Keys.Control)
                        {
                                if (kk == -1)
                                {
                                    temp.Add(day_select[i]);

                                }
                            }
                            else
                            {
                                if (kk >= 0)
                                {
                                    temp.RemoveAt(kk);

                                }
                            }
                        }

                    list_hieuung[vt_chon_vung].WIRE = temp;
                    
                    day_select.Clear();

                }
            }
            else if (vt_menu == menu_text_wire)
            {
                if (keo_vung == true)
                {

                    keo_vung = false;
                    //  vt_chuot = e.Location;
                    // add_den_select(e.Location);
                    //add_vung_select(vt_chuot, e.Location);

                    List<int> temp = list_hieuung[vt_chon_vung].WIRE;


                    for (int i = 0; i < day_select.Count; i++)
                    {
                        int kk = -1;
                        for (int x = 0; x < temp.Count; x++)
                        {
                            if (temp[x] == day_select[i])
                            {
                                kk = x;
                                break;
                            }
                        }
                        if ((ModifierKeys & Keys.Control) != Keys.Control)
                        {
                            if (kk == -1)
                            {
                                temp.Add(day_select[i]);

                            }
                        }
                        else
                        {
                            if (kk >= 0)
                            {
                                temp.RemoveAt(kk);

                            }
                        }
                    }

                    list_hieuung[vt_chon_vung].WIRE = temp;

                    day_select.Clear();

                }
            }
            else if (vt_menu == menu_thucong_wire)
            {
                
                    if (keo_vung == true)
                    {

                        keo_vung = false;
                        //  vt_chuot = e.Location;
                        // add_den_select(e.Location);
                        //add_vung_select(vt_chuot, e.Location);

                        List<int> temp = list_vien_wire[list_vien_wire_ten.SelectedIndex];


                        for (int i = 0; i < day_select.Count; i++)
                        {
                            int kk = -1;
                            for (int x = 0; x < temp.Count; x++)
                            {
                                if (temp[x] == day_select[i])
                                {
                                    kk = x;
                                    break;
                                }
                            }
                            if ((ModifierKeys & Keys.Control) != Keys.Control)
                            {
                                if (kk == -1)
                                {
                                    temp.Add(day_select[i]);

                                }
                            }
                            else
                            {
                                if (kk >= 0)
                                {
                                    temp.RemoveAt(kk);

                                }
                            }
                        }

                        list_vien_wire[list_vien_wire_ten.SelectedIndex] = temp;
                       
                        day_select.Clear();

                    }
                
            }
            else if (vt_menu == menu_haoquang_wire)
            {
                
                    if (keo_thucong == true)
                    {
                        vt_luu_keo = -1;
                        keo_thucong = false;
                        //  vt_chuot = e.Location;
                        // add_den_select(e.Location);
                        //add_vung_select(vt_chuot, e.Location);
                        if (keo_thang.Checked == true)
                        {
                            if (day_select.Count > 2)
                            {

                                List<int> te = new List<int>();
                                te.Add(day_select[0]);
                                te.Add(day_select[1]);

                                for (int x = 2; x < day_select.Count; x++)
                                {
                                    float ss = chieudaiF(new PointF((float)led_full[te[0]].X, (float)led_full[te[0]].Y), new PointF((float)led_full[day_select[x]].X, (float)led_full[day_select[x]].Y));

                                    int vt1 = -1;
                                    for (int y = 1; y < te.Count; y++)
                                    {
                                        float ss1 = chieudaiF(new PointF((float)led_full[te[0]].X, (float)led_full[te[0]].Y), new PointF((float)led_full[te[y]].X, (float)led_full[te[y]].Y));
                                        if (ss <= ss1) { vt1 = y; }
                                    }

                                    if (vt1 == -1) te.Add(day_select[x]);
                                    else te.Insert(vt1, day_select[x]);
                                }
                                day_select = te;


                                List<int> vt_den_temp = new List<int>();
                                List<int> vt_den_temp1 = new List<int>();


                                for (int i = 0; i < day_select.Count; i++) vt_den_temp.Add(day_select[i]);
                                int chay = day_select.Count - 1;
                                float max = 0;
                                int vt = 0;
                                vt_den_temp1.Add(vt_den_temp[0]);
                                vt_den_temp.RemoveAt(0);
                                while (chay > 0)
                                {
                                    max = 100000;
                                    vt = 0;
                                    for (int i = 0; i < vt_den_temp.Count; i++)
                                    {
                                        // MessageBox.Show(chieudai(vt_den_auto[vt_den_auto.Count - 1], vt_den_temp[i]).ToString());
                                        float ss = chieudaiF(new PointF((float)led_full[vt_den_temp1[vt_den_temp1.Count - 1]].X, (float)led_full[vt_den_temp1[vt_den_temp1.Count - 1]].Y), new PointF((float)led_full[vt_den_temp[i]].X, (float)led_full[vt_den_temp[i]].Y));
                                        if (ss <= max) { max = ss; vt = i; };
                                    }
                                    vt_den_temp1.Add(vt_den_temp[vt]);
                                    vt_den_temp.RemoveAt(vt);
                                    //chuyen3d(vt, vt_den_temp1.Count - 1);
                                    chay--;
                                }
                                // vt_den.Clear();
                                for (int i = 0; i < day_select.Count; i++) day_select[i] = vt_den_temp1[i];


                            }
                        }


                        List<int> temp = list_vien_wire[list_vien_wire_ten.SelectedIndex];
                        for (int i = 0; i < day_select.Count; i++)
                        {
                            int kk = -1;
                            for (int x = 0; x < temp.Count; x++)
                            {
                                if (temp[x] == day_select[i])
                                {
                                    kk = x;
                                    break;
                                }
                            }
                            if ((ModifierKeys & Keys.Control) != Keys.Control)
                            {
                                if (kk == -1)
                                {
                                    temp.Add(day_select[i]);

                                }
                            }
                            else
                            {
                                if (kk >= 0)
                                {
                                    temp.RemoveAt(kk);

                                }
                            }
                        }

                        list_vien_wire[list_vien_wire_ten.SelectedIndex] = temp;
                        // textBox1.Text = list_vien_wire[list_vien_wire_ten.SelectedIndex].Count.ToString();
                        day_select.Clear();
                        vt_auto1 = -1;

                        
                    }

                 
            }
            else if (vt_menu == menu_vien_wire)
            {
                 
                    if (keo_thucong == true)
                    {
                        vt_luu_keo = -1;
                        keo_thucong = false;
                        //  vt_chuot = e.Location;
                        // add_den_select(e.Location);
                        //add_vung_select(vt_chuot, e.Location);
                        if (keo_thang.Checked == true)
                        {
                            if (day_select.Count > 2)
                            {

                                List<int> te = new List<int>();
                                te.Add(day_select[0]);
                                te.Add(day_select[1]);

                                for (int x = 2; x < day_select.Count; x++)
                                {
                                    float ss = chieudaiF(new PointF((float)led_full[te[0]].X, (float)led_full[te[0]].Y), new PointF((float)led_full[day_select[x]].X, (float)led_full[day_select[x]].Y));

                                    int vt1 = -1;
                                    for (int y = 1; y < te.Count; y++)
                                    {
                                        float ss1 = chieudaiF(new PointF((float)led_full[te[0]].X, (float)led_full[te[0]].Y), new PointF((float)led_full[te[y]].X, (float)led_full[te[y]].Y));
                                        if (ss <= ss1) { vt1 = y; }
                                    }

                                    if (vt1 == -1) te.Add(day_select[x]);
                                    else te.Insert(vt1, day_select[x]);
                                }
                                day_select = te;


                                List<int> vt_den_temp = new List<int>();
                                List<int> vt_den_temp1 = new List<int>();


                                for (int i = 0; i < day_select.Count; i++) vt_den_temp.Add(day_select[i]);
                                int chay = day_select.Count - 1;
                                float max = 0;
                                int vt = 0;
                                vt_den_temp1.Add(vt_den_temp[0]);
                                vt_den_temp.RemoveAt(0);
                                while (chay > 0)
                                {
                                    max = 100000;
                                    vt = 0;
                                    for (int i = 0; i < vt_den_temp.Count; i++)
                                    {
                                        // MessageBox.Show(chieudai(vt_den_auto[vt_den_auto.Count - 1], vt_den_temp[i]).ToString());
                                        float ss = chieudaiF(new PointF((float)led_full[vt_den_temp1[vt_den_temp1.Count - 1]].X, (float)led_full[vt_den_temp1[vt_den_temp1.Count - 1]].Y), new PointF((float)led_full[vt_den_temp[i]].X, (float)led_full[vt_den_temp[i]].Y));
                                        if (ss <= max) { max = ss; vt = i; };
                                    }
                                    vt_den_temp1.Add(vt_den_temp[vt]);
                                    vt_den_temp.RemoveAt(vt);
                                    //chuyen3d(vt, vt_den_temp1.Count - 1);
                                    chay--;
                                }
                                // vt_den.Clear();
                                for (int i = 0; i < day_select.Count; i++) day_select[i] = vt_den_temp1[i];


                            }
                        }


                        List<int> temp = list_vien_wire[list_vien_wire_ten.SelectedIndex];
                        for (int i = 0; i < day_select.Count; i++)
                        {
                            int kk = -1;
                            for (int x = 0; x < temp.Count; x++)
                            {
                                if (temp[x] == day_select[i])
                                {
                                    kk = x;
                                    break;
                                }
                            }
                            if ((ModifierKeys & Keys.Control) != Keys.Control)
                            {
                                if (kk == -1)
                                {
                                    temp.Add(day_select[i]);

                                }
                            }
                            else
                            {
                                if (kk >= 0)
                                {
                                    temp.RemoveAt(kk);

                                }
                            }
                        }

                        list_vien_wire[list_vien_wire_ten.SelectedIndex] = temp;
                        // textBox1.Text = list_vien_wire[list_vien_wire_ten.SelectedIndex].Count.ToString();
                        day_select.Clear();
                        vt_auto1 = -1;

                      
                    }
                   
                 
            }
            else if (vt_menu == menu_vay_wire)
            {
                
                    if (keo_vung == true)
                    {

                        keo_vung = false;
                        //  vt_chuot = e.Location;
                        // add_den_select(e.Location);
                        //add_vung_select(vt_chuot, e.Location);

                        List<int> temp = list_vien_wire[list_vien_wire_ten.SelectedIndex];


                        for (int i = 0; i < day_select.Count; i++)
                        {
                            int kk = -1;
                            for (int x = 0; x < temp.Count; x++)
                            {
                                if (temp[x] == day_select[i])
                                {
                                    kk = x;
                                    break;
                                }
                            }
                            if ((ModifierKeys & Keys.Control) != Keys.Control)
                            {
                                if (kk == -1)
                                {
                                    temp.Add(day_select[i]);

                                }
                            }
                            else
                            {
                                if (kk >= 0)
                                {
                                    temp.RemoveAt(kk);

                                }
                            }
                        }

                        list_vien_wire[list_vien_wire_ten.SelectedIndex] = temp;
                       
                        day_select.Clear();

                    }
                
            }
            else if (vt_menu == menu_nap )
            {



                if (keo_nap== true)
                {
                    keo_nap = false;
                    panel_wail.Visible = true;
                    if (vt_menu1 == menu_wire_connect0)
                    {


                        led_full1.Clear();
                        for (int i = 0; i < led_full.Count; i++) led_full1.Add(new LED1(led_full[i].X, led_full[i].Y, i));



                        int mm = -1;

                        List<LED1> vt_den_temp = new List<LED1>();

                        for (int x = 0; x < led_select.Count; x++)
                        {
                            vt_den_temp.Add(led_full1[led_select[x]]);
                            if (vt_port2 > 0)
                            {
                                if (vt_port2 == led_select[x]) mm = vt_den_temp.Count - 1;
                            }


                        }
                        for (int x = 0; x < led_full1.Count; x++)
                        {
                            if (check_den_select1(x) == -1)
                            {
                                vt_den_temp.Add(led_full1[x]);
                                if (vt_port2 > 0)
                                {
                                    if (vt_port2 == x) mm = vt_den_temp.Count - 1;
                                }
                            }

                        }
                        for (int i = 0; i < led_full1.Count; i++) led_full1[i] = vt_den_temp[i];

                        for (int i = 0; i < led_full1.Count; i++) led_full[i] = new LED(led_full1[i].X, led_full1[i].Y);




                        for (int x = 0; x < list_hieuung.Count; x++)
                        {
                            for (int y = 0; y < list_hieuung[x].HIEUUNG.Count; y++)
                            {
                                for (int z = 0; z < list_hieuung[x].HIEUUNG[y].DATA.Count; z++)
                                {
                                    int[] temp = new int[list_hieuung[x].HIEUUNG[y].DATA[z].Length];
                                    for (int i = 0; i < list_hieuung[x].HIEUUNG[y].DATA[z].Length; i++)
                                    {
                                        temp[i] = list_hieuung[x].HIEUUNG[y].DATA[z][(int)led_full1[i].VT];

                                    }
                                    list_hieuung[x].HIEUUNG[y].DATA[z] = temp;
                                }
                            }

                        }



                        if (vt_port2 > 0)
                        {
                            vt_port2 = mm;
                        }
                        vt_auto1 = -1;
                        led_select.Clear();


                    }
                    else if (vt_menu1 == menu_wire_connect1)
                    {
                        led_full1.Clear();
                        for (int i = 0; i < led_full.Count; i++) led_full1.Add(new LED1(led_full[i].X, led_full[i].Y, i));

                        if (cu.Count > 0)
                        {

                            if (cu.Count > 1)
                            {
                                List<int> te = new List<int>();
                                te.Add(cu[0]);
                                te.Add(cu[1]);

                                for (int x = 2; x < cu.Count; x++)
                                {
                                    float ss = chieudaiF(new PointF((float)led_full[te[0]].X, (float)led_full[te[0]].Y), new PointF((float)led_full[cu[x]].X, (float)led_full[cu[x]].Y));

                                    int vt1 = -1;
                                    for (int y = 1; y < te.Count; y++)
                                    {
                                        float ss1 = chieudaiF(new PointF((float)led_full[te[0]].X, (float)led_full[te[0]].Y), new PointF((float)led_full[te[y]].X, (float)led_full[te[y]].Y));
                                        if (ss <= ss1) { vt1 = y; }
                                    }

                                    if (vt1 == -1) te.Add(cu[x]);
                                    else te.Insert(vt1, cu[x]);
                                }
                                cu = te;


                                List<int> vt_den_temp0 = new List<int>();
                                List<int> vt_den_temp1 = new List<int>();


                                for (int i = 0; i < cu.Count; i++) vt_den_temp0.Add(cu[i]);
                                int chay = cu.Count - 1;
                                float max = 0;
                                int vt = 0;
                                vt_den_temp1.Add(vt_den_temp0[0]);
                                vt_den_temp0.RemoveAt(0);
                                while (chay > 0)
                                {
                                    max = 100000;
                                    vt = 0;
                                    for (int i = 0; i < vt_den_temp0.Count; i++)
                                    {
                                        // MessageBox.Show(chieudai(vt_den_auto[vt_den_auto.Count - 1], vt_den_temp[i]).ToString());
                                        float ss = chieudaiF(new PointF((float)led_full[vt_den_temp1[vt_den_temp1.Count - 1]].X, (float)led_full[vt_den_temp1[vt_den_temp1.Count - 1]].Y), new PointF((float)led_full[vt_den_temp0[i]].X, (float)led_full[vt_den_temp0[i]].Y));
                                        if (ss <= max) { max = ss; vt = i; };
                                    }
                                    vt_den_temp1.Add(vt_den_temp0[vt]);
                                    vt_den_temp0.RemoveAt(vt);
                                    //chuyen3d(vt, vt_den_temp1.Count - 1);
                                    chay--;
                                }
                                // vt_den.Clear();
                                for (int i = 0; i < cu.Count; i++) cu[i] = vt_den_temp1[i];
                            }


                            for (int i = 0; i < cu.Count; i++) led_select.Add(cu[i]);
                        }









                        int mm = -1;

                        List<LED1> vt_den_temp = new List<LED1>();

                        for (int x = 0; x < led_select.Count; x++)
                        {
                            vt_den_temp.Add(led_full1[led_select[x]]);
                            if (vt_port2 > 0)
                            {
                                if (vt_port2 == led_select[x]) mm = vt_den_temp.Count - 1;
                            }


                        }
                        for (int x = 0; x < led_full1.Count; x++)
                        {
                            if (check_den_select1(x) == -1)
                            {
                                vt_den_temp.Add(led_full1[x]);
                                if (vt_port2 > 0)
                                {
                                    if (vt_port2 == x) mm = vt_den_temp.Count - 1;
                                }
                            }

                        }
                        for (int i = 0; i < led_full1.Count; i++) led_full1[i] = vt_den_temp[i];

                        for (int i = 0; i < led_full1.Count; i++) led_full[i] = new LED(led_full1[i].X, led_full1[i].Y);




                        for (int x = 0; x < list_hieuung.Count; x++)
                        {
                            for (int y = 0; y < list_hieuung[x].HIEUUNG.Count; y++)
                            {
                                for (int z = 0; z < list_hieuung[x].HIEUUNG[y].DATA.Count; z++)
                                {
                                    int[] temp = new int[list_hieuung[x].HIEUUNG[y].DATA[z].Length];
                                    for (int i = 0; i < list_hieuung[x].HIEUUNG[y].DATA[z].Length; i++)
                                    {
                                        temp[i] = list_hieuung[x].HIEUUNG[y].DATA[z][(int)led_full1[i].VT];

                                    }
                                    list_hieuung[x].HIEUUNG[y].DATA[z] = temp;
                                }
                            }

                        }



                        if (vt_port2 > 0)
                        {
                            vt_port2 = mm;
                        }
                        vt_auto1 = -1;
                        led_select.Clear();
                        cu.Clear();
                    }
                    panel_wail.Visible = false;

                }
                upled();
            }
        }

        private void effect_KeyDown(object sender, KeyEventArgs e)
        {
            // if (text_chu.ClientRectangle.Contains(text_chu.PointToClient(Cursor.Position)) == false)
            // {
            if (text_chu.ClientRectangle.Contains(text_chu.PointToClient(Cursor.Position)) == false)
            {
                if (e.KeyCode == Keys.R)
                {
                    reset_moitruong();
                }
            }
            /*
            if (e.Control)
            {
                key_Ctrl = true;
                
            }
            else key_Ctrl = false;
            */
            //}
        }
        private void reset_moitruong()
        {
            if (led_full.Count > 0)
            {
                float[] thongtin_ban = new float[4];


                float xx = 1000000;
                float yy = 1000000;
                float ww = 0;
                float hh = 0;
                for (int x = 0; x < led_full.Count; x++)
                {

                    PointF point1_1 = new PointF((float)(led_full[x].X - 6), (float)(led_full[x].Y - 6));
                    PointF point1_2 = new PointF((float)(led_full[x].X - 6), (float)(led_full[x].Y + 6));
                    PointF point2_2 = new PointF((float)(led_full[x].X + 6), (float)(led_full[x].Y + 6));
                    PointF point2_1 = new PointF((float)(led_full[x].X + 6), (float)(led_full[x].Y - 6));
                    /////////////////


                    if (point1_1.X >= ww) ww = point1_1.X;
                    if (point1_2.X >= ww) ww = point1_2.X;
                    if (point2_2.X >= ww) ww = point2_2.X;
                    if (point2_1.X >= ww) ww = point2_1.X;

                    if (point1_1.Y >= hh) hh = point1_1.Y;
                    if (point1_2.Y >= hh) hh = point1_2.Y;
                    if (point2_2.Y >= hh) hh = point2_2.Y;
                    if (point2_1.Y >= hh) hh = point2_1.Y;

                    if (point1_1.X <= xx) xx = point1_1.X;
                    if (point1_2.X <= xx) xx = point1_2.X;
                    if (point2_2.X <= xx) xx = point2_2.X;
                    if (point2_1.X <= xx) xx = point2_1.X;

                    if (point1_1.Y <= yy) yy = point1_1.Y;
                    if (point1_2.Y <= yy) yy = point1_2.Y;
                    if (point2_2.Y <= yy) yy = point2_2.Y;
                    if (point2_1.Y <= yy) yy = point2_1.Y;

                }



                thongtin_ban[0] = xx;
                thongtin_ban[1] = yy;
                thongtin_ban[2] = ww - xx;
                thongtin_ban[3] = hh - yy;


                float zo1 = pic_map.Width / (thongtin_ban[2] + 50);
                float zo2 = pic_map.Height / (thongtin_ban[3] + 50);


                if ((thongtin_ban[2] + 50) * zo2 > pic_map.Width)
                {
                    zo2 = zo1;
                }



                zoom = zo2;
                imgx = -(int)((thongtin_ban[0] - 20));
                imgy = -(int)((thongtin_ban[1] - 20));
                // draw_moitruong();
            }
            else
            {
                imgx = -(int)(X0);
                imgy = -(int)(Y0);
                zoom = 1F;
                // draw_moitruong();
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            if (nhay == false) nhay = true;
            else nhay = false;
            pic_time.Refresh();
            pic_vung.Refresh();

           // pic_hu_haoquang.Refresh();
            pictureBox2.Refresh();
            pictureBox23.Refresh();
            if (vt_menu == menu_nap) pic_menu1.Refresh();
            if (vt_menu == menu_vien_edit|| vt_menu == menu_vay_edit)
            {
                pic_vien_vien.Refresh();
                pic_vien_nen.Refresh();
                pic_vien_mau.Refresh();
            }
        }


        private void effect_Resize(object sender, EventArgs e)
        {
            timer_map.Enabled = false;
            if(pic_map.Width>0 && pic_map.Height > 0)
            moi = new Bitmap(pic_map.Width, pic_map.Height);
            get_tong();
            timer_map.Enabled = true;
        }
        int vtx_time = 0;
        int vty_time = 0;
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {


            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), 0, 0, pic_time.Width, 20);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 1), -1, -1, pic_time.Width, 21);
           
            for (int x = 0; x < pic_time.Width  + 10; x++)
            {
                 
                if ((x + vtx_time) % 100 == 0)
                {
                
                    e.Graphics.DrawString(((x + vtx_time)).ToString(), new Font("Microsoft Sans Serif", 8, FontStyle.Regular), Brushes.White, new PointF(x, 4));

                    e.Graphics.DrawLine(new Pen(Color.FromArgb(250, 250, 250), 1), x, 0, x, 4);
                }

                if ((x + vtx_time) % 10 == 0)
                {

                    e.Graphics.DrawLine(new Pen(Color.FromArgb(250, 250, 250), 1), x , 0, x , 1);


                }
            }


            for (int i = 0; i < pic_time.Height / 20; i++)
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(128, 128, 128)), 0, i * 20 + 20, pic_time.Width, i * 20 + 20);
            }
             


            for (int y = 0; y < 4; y++)
            {
                int nn = y + vt_hien_vung;
                if (nn >= 0 && nn < list_hieuung.Count)
                {
                    int toi = -vtx_time;
                    for (int x = 0; x < list_hieuung[nn].HIEUUNG.Count; x++)
                    {




                        e.Graphics.FillRectangle(new SolidBrush(color_layer0[nn % color_layer0.Length]), new Rectangle((int)(list_hieuung[nn].HIEUUNG[x].BATDAU+ toi) , y * 20 + 20, (int)list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI, 20));
                       
                        e.Graphics.DrawRectangle(Pens.White, new Rectangle((int)(list_hieuung[nn].HIEUUNG[x].BATDAU  + toi) , y * 20 + 20, (int)list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI, 20));


                        if (list_hieuung[nn].HIEUUNG[x].COGIAN == false)
                        {
                            if (list_hieuung[nn].HIEUUNG[x].TONG_GOC > 0 && list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI > 0)
                            {
                                int k = list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI / list_hieuung[nn].HIEUUNG[x].TONG_GOC;
                                if (k <= 0)
                                {

                                }
                                else
                                {
                                    for (int i = 0; i < k +1; i++)
                                    {
                                       if(i!=0)    e.Graphics.DrawLine(new Pen(Color.FromArgb(64, 64, 64)),  (int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi)  + i * list_hieuung[nn].HIEUUNG[x].TONG_GOC , y * 20 + 22, (int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi) + i * list_hieuung[nn].HIEUUNG[x].TONG_GOC, y * 20 + 38);

                                    }
                                }
                            }

                            DrawArrow(e.Graphics, Pens.White, new PointF((int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi)  , y * 20 + 30), new PointF((int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi + list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI)  , y * 20 + 30), 4);
                        }
                        else
                        {
                            DrawArrow(e.Graphics, Pens.Red, new PointF((int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi + list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI) , y * 20 + 32), new PointF((int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi)  , y * 20 + 32), 4);
                            DrawArrow(e.Graphics, Pens.Red, new PointF((int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi)  , y * 20 + 32), new PointF((int)(list_hieuung[nn].HIEUUNG[x].BATDAU + toi + list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI)  , y * 20 + 32), 4);

                        }
                        toi = toi + list_hieuung[nn].HIEUUNG[x].BATDAU + list_hieuung[nn].HIEUUNG[x].TONG_HIENTAI;
                    }
                }
            }


           if (vt_menu == menu_chinh)
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0), 1), new Rectangle(dem_play - vtx_time, 0, 1, pic_time.Height));
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255, 255), 1), new Rectangle(tong_play - vtx_time, 0, 1, pic_time.Height));
            }
            else 
            {

                if (thongtin_effect_goc == null)
                {


                }
                else
                {
                    int vt_add = vt_chon_vung - vt_hien_vung;
                    if (vt_add >= 0 && vt_add < 4)
                    {
                        int bd = get_tong_vung(vt_chon_vung) - vtx_time;


                        if (nhay == true)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0)), new Rectangle((int)(thongtin_effect_goc.BATDAU + bd), vt_add * 20 + 20, thongtin_effect_goc.HIENTAI, 20)); 
                        
                        }else
                        {
                       

                        }
                        e.Graphics.DrawRectangle(Pens.White, new Rectangle((int)(thongtin_effect_goc.BATDAU + bd), vt_add * 20 + 20, thongtin_effect_goc.HIENTAI, 20));

                        DrawArrow(e.Graphics, new Pen(Color.FromArgb(255, 255, 255), 2), new PointF(thongtin_effect_goc.BATDAU + bd , vt_add * 20 + 20 + 9), new PointF(thongtin_effect_goc.BATDAU + bd  + 10, vt_add * 20 + 20 + 9), 4);

                        
                            DrawArrow(e.Graphics, new Pen(Color.FromArgb(255, 255, 255), 2), new PointF(thongtin_effect_goc.BATDAU + bd + thongtin_effect_goc.HIENTAI, vt_add * 20 + 20 + 9), new PointF(thongtin_effect_goc.BATDAU + bd + thongtin_effect_goc.HIENTAI + 10, vt_add * 20 + 20 + 9), 4);
 

                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0),1), new Rectangle((int)(thongtin_effect_goc.BATDAU + bd + thongtin_effect_goc.DEM), 0, 1, pic_time.Height));
                       }

                }
            }
        }
        int vt_hien_vung = 0;
        int vt_chon_vung = -1;

        int vt_add_vung = -1;
 

        private int get_tong_vung(int vtvung)
        {
            int toi = 0;
            if (vtvung >= 0 && vtvung < list_hieuung.Count)
            {
                if (list_hieuung[vtvung].HIEUUNG != null)
                {
                    for (int x = 0; x < list_hieuung[vtvung].HIEUUNG.Count; x++)
                    {

                        toi = toi + list_hieuung[vtvung].HIEUUNG[x].BATDAU + list_hieuung[vtvung].HIEUUNG[x].TONG_HIENTAI;
                    }
                }
            }
            return toi;
        }
            private void pic_vung_Paint(object sender, PaintEventArgs e)
        {




            for (int x = 0; x < 4; x++)
            {
                if (x + vt_hien_vung >= 0 && x + vt_hien_vung < list_hieuung.Count)
                {

                    if (x + vt_hien_vung == vt_chon_vung) e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 151, 251)), new Rectangle(0, 20 * x+20, pic_vung.Width, 20));
                    else e.Graphics.FillRectangle(new SolidBrush(color_layer0[(x + vt_hien_vung)% color_layer0.Length]), new Rectangle(0, 20 * x+20, pic_vung.Width, 20));

                    e.Graphics.DrawString("L:"+(x + vt_hien_vung+1).ToString("00"), new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(2, 20 * x + 22));

                     if(list_hieuung[x + vt_hien_vung].LOAI==0)  e.Graphics.DrawString(name_layer1[menu_ngonngu], new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(38, 20 * x + 22));
                    else if (list_hieuung[x + vt_hien_vung].LOAI == 1) e.Graphics.DrawString(name_layer2[menu_ngonngu], new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(38, 20 * x + 22));
                    else if (list_hieuung[x + vt_hien_vung].LOAI == 2) e.Graphics.DrawString(name_layer3[menu_ngonngu], new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(38, 20 * x + 22));
                    else if (list_hieuung[x + vt_hien_vung].LOAI == 3) e.Graphics.DrawString(name_layer4[menu_ngonngu], new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(38, 20 * x + 22));
                    else if (list_hieuung[x + vt_hien_vung].LOAI == 4) e.Graphics.DrawString(name_layer5[menu_ngonngu], new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(38, 20 * x + 22));
                    else if (list_hieuung[x + vt_hien_vung].LOAI == 5) e.Graphics.DrawString(name_layer6[menu_ngonngu], new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(38, 20 * x + 22));

                    e.Graphics.DrawString(":" + list_hieuung[x + vt_hien_vung].TEN, new Font("Microsoft Sans Serif", 10, FontStyle.Bold), new SolidBrush(Color.White), new PointF(110, 20 * x + 22));

                }
            }
            for (int x = 0; x < 4; x++)
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 1), new Rectangle(0-1, 20 * x + 20, pic_vung.Width, 20));
            }

           


            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), 0, 0, pic_time.Width, 20);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 1), -1, -1, pic_time.Width, 21);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), 0, 100, pic_time.Width, 20);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 1), -1, 99, pic_time.Width, 21);

            int vt_add = vt_chon_vung - vt_hien_vung;
            if (vt_add >= 0 && vt_add < 4)
            {


                if (nhay == true) e.Graphics.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0), 2), new Rectangle(1, 20 * vt_add + 20, pic_vung.Width-4, 20));

            }

        }
        int menu_ngonngu = 0;
        private void button1_Click(object sender, EventArgs e)
        {
           


        }
       
        int dem_play_thucong = 0;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
          

        }
        bool play_thucong = false;
        private void com_path_effect_SelectedIndexChanged(object sender, EventArgs e)
        {
            hien_list_effect();
             
        }
        byte[] docanh;
        private void pic_hu_nen_Paint(object sender, PaintEventArgs e)
        {

            if (vt_chon_effect >= 0)
            {
                int xx = vt_chon_effect - vt_hien_effect * max_rong_effect;
                if (xx >= 0 && xx < max_rong_effect* max_cao_effect)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 128, 128)),   (xx % max_rong_effect) * 86,   (xx / max_rong_effect) * 75, 88, 76);
            }



            using (Font font1 = new Font("Microsoft Sans Serif", 8, FontStyle.Regular, GraphicsUnit.Point))
            {
                for (int x = 0; x < max_cao_effect; x++)
                {

                    for (int y = 0; y < max_rong_effect; y++)
                    {

                        if (x * max_rong_effect + y + vt_hien_effect * max_rong_effect < list_effect.Count)
                        {

                            string link = list_effect[x * max_rong_effect + y + vt_hien_effect * max_rong_effect];

                            if (File.Exists(list_path_effect[com_path_effect.SelectedIndex] + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp") )
                            {

                                FileStream fs = new System.IO.FileStream(list_path_effect[com_path_effect.SelectedIndex] + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp", FileMode.Open, FileAccess.Read);
                                anh_doc = (Bitmap)Image.FromStream(fs);
                                
                                e.Graphics.DrawImage(anh_doc, new Rectangle(2 + 86 * y, 2 + 75 * x, anh_doc.Width, anh_doc.Height), new Rectangle(0, 0, anh_doc.Width, anh_doc.Height), GraphicsUnit.Pixel);

                                fs.Close();

                            }


                               
                          


                            // Bitmap aaaa = (Bitmap)byteArrayToImage(temp1.ANH);
                            // e.Graphics.DrawImage(aaaa, new Rectangle(1 + 164 * y, 19 + 124 * x, aaaa.Width, aaaa.Height), new Rectangle(0, 0, aaaa.Width, aaaa.Height), GraphicsUnit.Pixel);

                                 e.Graphics.DrawRectangle(Pens.Gray, 2 + 86 * y,2  + 75 * x, 82, 60);

                                    RectangleF rectF1 = new RectangleF(4 + 86 * y, 2 + 75 * x + 60, 82, 20);
                                    string hhhh = Path.GetFileNameWithoutExtension(link);
                                    string ff;
                                    if (hhhh.Length < 16) ff = hhhh;
                                    else ff = hhhh.Substring(0, 16);

                                    e.Graphics.DrawString(ff, font1, Brushes.White, rectF1);

                             










                        }
                    }
                }
            }
        }
        public System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        {
            if (width > 0 && height > 0)
            {
                //if (width <= 0) width = 1;
                // if (height <= 0) height = 1;
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
            return null;
        }

        
        int tong_chay_thucong = 0;
        private void set_vung()
        {
            panel_nap.Visible = false;
            panel_effect0.Visible = false;
            panel_effect.Visible = false;
            panel_text.Visible = false;
            panel_vien.Visible = false;
            panel_vien0.Visible = false;
            panel_vien1.Visible = false;
            panel_haoquang.Visible = false;
            panel_haoquang1.Visible = false;
            thongtin_effect_goc = null;
            effect_nap = null;
            tong_chay_thucong =0;
            dem_play_thucong = 0;
            panel_thucong_chon.Visible = false;
            if (vt_chon_vung >= 0)
            {
                set_play(false);
                if (list_hieuung[vt_chon_vung].LOAI == hieuung_effect)
                {
                    panel_effect.Visible = true;
                    vt_menu = menu_effect_edit;
                   
                    get_thongtin_vung();
                    //   textBox1.Text = " X:" + thongtin_video.CENTER.X.ToString() + " Y:" + thongtin_video.CENTER.Y.ToString() + " G:" + thongtin_video.GOC.ToString() + " W:" + thongtin_video.SIZE.Width.ToString() + " H:" + thongtin_video.SIZE.Height.ToString();
                }
                else if (list_hieuung[vt_chon_vung].LOAI == hieuung_text)
                {
                    panel_text.Visible = true;
                    vt_menu = menu_text_edit;
                   
                    get_thongtin_vung();
                    //   textBox1.Text = " X:" + thongtin_video.CENTER.X.ToString() + " Y:" + thongtin_video.CENTER.Y.ToString() + " G:" + thongtin_video.GOC.ToString() + " W:" + thongtin_video.SIZE.Width.ToString() + " H:" + thongtin_video.SIZE.Height.ToString();
                }
                else if (list_hieuung[vt_chon_vung].LOAI == hieuung_vien)
                {
                    panel_vien.Visible = true;
                    vt_menu = menu_vien_edit;
                    button15.Text = name_vien_6[menu_ngonngu];
                    check_group.Enabled = false;
                    get_thongtin_vung();
                    //   textBox1.Text = " X:" + thongtin_video.CENTER.X.ToString() + " Y:" + thongtin_video.CENTER.Y.ToString() + " G:" + thongtin_video.GOC.ToString() + " W:" + thongtin_video.SIZE.Width.ToString() + " H:" + thongtin_video.SIZE.Height.ToString();
                }
                else if (list_hieuung[vt_chon_vung].LOAI == hieuung_vay)
                {
                    panel_vien.Visible = true;
                    vt_menu = menu_vay_edit;
                    button15.Text = name_vien_6_1[menu_ngonngu];
                    check_group.Enabled = true;
                    check_group.Checked = false;
                    get_thongtin_vung();
                    //   textBox1.Text = " X:" + thongtin_video.CENTER.X.ToString() + " Y:" + thongtin_video.CENTER.Y.ToString() + " G:" + thongtin_video.GOC.ToString() + " W:" + thongtin_video.SIZE.Width.ToString() + " H:" + thongtin_video.SIZE.Height.ToString();
                }
                else if (list_hieuung[vt_chon_vung].LOAI == hieuung_thucong)
                {
                    panel_thucong_chon.Visible = true;
                    panel_effect.Visible = true;
                    vt_menu = menu_thucong_edit;
                    tong_chay_thucong = list_hieuung[vt_chon_vung].WIREV.Count;
                    
                    get_thongtin_vung();



                    //   textBox1.Text = " X:" + thongtin_video.CENTER.X.ToString() + " Y:" + thongtin_video.CENTER.Y.ToString() + " G:" + thongtin_video.GOC.ToString() + " W:" + thongtin_video.SIZE.Width.ToString() + " H:" + thongtin_video.SIZE.Height.ToString();
                }
                else if (list_hieuung[vt_chon_vung].LOAI == hieuung_haoquang)
                {
                    panel_haoquang.Visible = true;
                    vt_menu = menu_haoquang_edit;
                    tong_chay_thucong = list_hieuung[vt_chon_vung].WIREV.Count;
               
                    get_thongtin_vung();
                }
                up_setvung();
            }
            else
            {
                get_tong();
                if(tong_play>0)
                {
                    dem_play = 0;
                    set_play(true);
                }
            }
            label3.Text = (vt_chon_vung + 1).ToString() + "/" + (list_hieuung.Count).ToString();
        }
            private void pic_vung_MouseDown(object sender, MouseEventArgs e)
        {
            if(vt_menu== menu_effect_wire || vt_menu == menu_text_wire || vt_menu == menu_vien_wire || vt_menu == menu_vay_wire || vt_menu == menu_thucong_wire || vt_menu == menu_haoquang_wire || vt_menu == menu_nap)
            {
                toolStripMenuItem5.Enabled = false;
                toolStripMenuItem6.Enabled = false;
                toolStripMenuItem7.Enabled = false;
            }
            else
            {
                int vv = e.Location.Y / 20 + vt_hien_vung;
                vv--;
                vt_menu = menu_chinh;
                if (vv >= 0 && vv < list_hieuung.Count) vt_chon_vung = vv;
                else vt_chon_vung = -1;

                if (e.Button == MouseButtons.Left) set_vung();
                else
                {
                    if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                    {
                        toolStripMenuItem5.Enabled = true;
                        if (list_hieuung.Count > 1)
                        {
                            if (vt_chon_vung > 0) toolStripMenuItem7.Enabled = true;
                            else toolStripMenuItem7.Enabled = false;

                            if (vt_chon_vung < list_hieuung.Count - 1) toolStripMenuItem6.Enabled = true;
                            else toolStripMenuItem6.Enabled = false;
                        }

                    }
                    else
                    {
                        toolStripMenuItem5.Enabled = false;
                        toolStripMenuItem6.Enabled = false;
                        toolStripMenuItem7.Enabled = false;
                    }

                }


            }
           
           
            

        }
        THONGTIN_EFFECT_GOC thongtin_effect_goc=null;
        THONGTIN_EFFECT_EDIT thongtin_effect_edit = null;
        THONGTIN_WIRE_VUNG thongtin_wire_vung = null;

        byte[] effect_nap;
        private void pic_hu_nen_MouseDown(object sender, MouseEventArgs e)
        {
           
                

                    int xx = (e.X - 2) / 86;
                    int yy = (e.Y - 2) / 75;


                    vt_chon_effect = yy * max_rong_effect + xx + vt_hien_effect * max_rong_effect;





                    if (vt_chon_effect >= list_effect.Count) vt_chon_effect = -1;
                    pic_hu_nen.Refresh();
            if (vt_chon_effect >= 0 && vt_chon_effect < list_effect.Count)
            {
                toolStripMenuItem2.Enabled = true;
                speed_thucong.Value = 1;
                mau_hue = 0;
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
                effect_nap = File.ReadAllBytes(list_effect[vt_chon_effect]);
                Mahoa_moi.giaimaok(ref effect_nap);
                int effect_tong = effect_nap[328 + 0] + effect_nap[328 + 1] * 256 + effect_nap[328 + 2] * 256 * 256 + effect_nap[328 + 3] * 256 * 256 * 256;
                int effect_ww = effect_nap[328 + 4] + effect_nap[328 + 5] * 256 + effect_nap[328 + 6] * 256 * 256 + effect_nap[328 + 7] * 256 * 256 * 256;
                int effect_hh = effect_nap[328 + 8] + effect_nap[328 + 9] * 256 + effect_nap[328 + 10] * 256 * 256 + effect_nap[328 + 11] * 256 * 256 * 256;

                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, effect_tong, effect_tong, 0, new SizeF(effect_ww, effect_hh), 0, false, false, false, false, false, 0, false, 0);
                panel_wail.Visible = false;
                get_tong();
                up_setvung();
                Application.DoEvents();
                timer_map.Enabled = true;
                timer.Enabled = true;
            }

            else
            {
                toolStripMenuItem2.Enabled = false;
                thongtin_effect_goc = null;

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
               

            
            if (list_hieuung[vt_chon_vung].WIRE.Count <= 0)
            {
               
                string ten = thongbao.ShowBox(name_wire_effect5[menu_ngonngu], menu_ngonngu);
                if (ten != "")
                {

                }
                else
                {
                    if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count) list_hieuung.RemoveAt(vt_chon_vung);
                    vt_menu = menu_chinh;

                    vt_chon_vung = -1;

                    set_vung();
                    int jj = get_tong_vung(vt_chon_vung);
                    if (dem_play <= jj) dem_play = jj;
                }
            }else
            {
                panel_effect0.Visible = false;
                set_vung();
            }




           
        }
        private double _hue = 0.0;
        private double _saturation = 0.0;
        private double _lightness = 0.0;
        private System.Drawing.Image ExecuteRgb8(System.Drawing.Image img )
        {
            const double c1o60 = 1 / (double)60;
            const double c1o255 = 1 / (double)255;

            Bitmap result = new Bitmap(img);
            result.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            BitmapData bmpData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int pixelBytes = System.Drawing.Image.GetPixelFormatSize(img.PixelFormat) / 8;
            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;
            int size = bmpData.Stride * result.Height;
            byte[] pixels = new byte[size - 1 + 1];
            int index;
            double R, G, B;
            double H, S, L, H1;
            double min, max, dif, sum;
            double f1, f2;
            double v1, v2, v3;
            double sat = 127 * _saturation / (double)100;
            double lum = 127 * _lightness / (double)100;
            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, size);
            // Main loop.
            for (int row = 0; row <= result.Height - 1; row++)
            {
                for (int col = 0; col <= result.Width - 1; col++)
                {
                    index = (row * bmpData.Stride) + (col * pixelBytes);
                    R = pixels[index + 2];
                    G = pixels[index + 1];
                    B = pixels[index + 0];
                    // Conversion to HSL space.
                    min = R;
                    if ((G < min))
                        min = G;
                    if ((B < min))
                        min = B;
                    max = R; f1 = 0.0; f2 = G - B;
                    if ((G > max))
                    {
                        max = G; f1 = 120.0; f2 = B - R;
                    }
                    if ((B > max))
                    {
                        max = B; f1 = 240.0; f2 = R - G;
                    }
                    dif = max - min;
                    sum = max + min;
                    L = 0.5 * sum;
                    if ((dif == 0))
                    {
                        H = 0.0; S = 0.0;
                    }
                    else
                    {
                        if ((L < 127.5))
                            S = 255.0 * dif / sum;
                        else
                            S = 255.0 * dif / (510.0 - sum);
                        H = (f1 + 60.0 * f2 / dif);
                        if (H < 0.0)
                            H += 360.0;
                        if (H >= 360.0)
                            H -= 360.0;
                    }
                    // Apply transformation.
                    H = H + mau_hue;
                    if (H >= 360.0)
                        H = H - 360.0;
                    S = S + sat;
                    if (S < 0.0)
                        S = 0.0;
                    if (S > 255.0)
                        S = 255.0;
                    L = L + lum;
                    if (L < 0.0)
                        L = 0.0;
                    if (L > 255.0)
                        L = 255.0;
                    // Conversion back to RGB space.
                    if ((S == 0))
                    {
                        R = L; G = L; B = L;
                    }
                    else
                    {
                        if ((L < 127.5))
                            v2 = c1o255 * L * (255 + S);
                        else
                            v2 = L + S - c1o255 * S * L;
                        v1 = 2 * L - v2;
                        v3 = v2 - v1;
                        H1 = H + 120.0;
                        if ((H1 >= 360.0))
                            H1 -= 360.0;
                        if ((H1 < 60.0))
                            R = v1 + v3 * H1 * c1o60;
                        else if ((H1 < 180.0))
                            R = v2;
                        else if ((H1 < 240.0))
                            R = v1 + v3 * (4 - H1 * c1o60);
                        else
                            R = v1;
                        H1 = H;
                        if ((H1 < 60.0))
                            G = v1 + v3 * H1 * c1o60;
                        else if ((H1 < 180.0))
                            G = v2;
                        else if ((H1 < 240.0))
                            G = v1 + v3 * (4 - H1 * c1o60);
                        else
                            G = v1;
                        H1 = H - 120.0;
                        if ((H1 < 0.0))
                            H1 += 360.0;
                        if ((H1 < 60.0))
                            B = v1 + v3 * H1 * c1o60;
                        else if ((H1 < 180.0))
                            B = v2;
                        else if ((H1 < 240.0))
                            B = v1 + v3 * (4 - H1 * c1o60);
                        else
                            B = v1;
                    }
                    // Save new values.
                    pixels[index + 2] = System.Convert.ToByte(R);
                    pixels[index + 1] = System.Convert.ToByte(G);
                    pixels[index + 0] = System.Convert.ToByte(B);
                }
            }
            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, size);
            // Unlock the bits.
            result.UnlockBits(bmpData);
            return result;
        }
        int mau_hue = 0;
      
        int tong_play=0;
        int dem_play = 0;


        private void get_tong( )
        {
            timer_map.Enabled = false;
            timer.Enabled = false;
            tong_play = 0;

            for (int y = 0; y < list_hieuung.Count; y++)
            {
                 
                int ll= get_tong_vung(y);
                if (ll >= tong_play) tong_play = ll;
            }


            if (vt_menu == menu_effect_edit||vt_menu == menu_vien_edit || vt_menu == menu_vay_edit || vt_menu == menu_haoquang_edit || vt_menu == menu_thucong_edit || vt_menu == menu_text_edit)
            {

                if (thongtin_effect_goc != null)
                {
                     
                        tong_play = tong_play + thongtin_effect_goc.BATDAU + thongtin_effect_goc.HIENTAI ;
                     
                  
                }
            }


            /*
            if (tong_play <= pic_time.Width )
            {
                hScrollBar2.Value = 0;
                hScrollBar2.Maximum = 400;
              
            }
            else
            {

                int hh = tong_play - pic_time.Width + 400;
                if (hh >= pic_time.Width  + 400) hScrollBar2.Value = 0;
                hScrollBar2.Maximum = hh+400 ;
            }
            */
            label1.Text = tong_play.ToString();
          
            if(tong_play<= pic_time.Width)
            {
                hScrollBar2.Value = 0;
              //  hScrollBar2.Maximum =0;
            }
            else
            {
                int tt = tong_play - pic_time.Width + 200;

                if (hScrollBar2.Value >= tt) hScrollBar2.Value = 0;
              //  hScrollBar2.Maximum =tt;
            }
            up_timeline();
            
           

            timer_map.Enabled = true;
            timer.Enabled = true;
             
        }
         
            private void timer_map_Tick(object sender, EventArgs e)
        {
            
            timer_map.Enabled = false;
            if (vt_port2 >= led_full.Count) vt_port2 = -1;
            if (nhay_map == false) nhay_map = true;
            else nhay_map = false;
           
            using (Graphics g = Graphics.FromImage(moi))
            {
                //pic_map.Refresh();
                Pen mau_rule = new Pen(Color.FromArgb(42, 128, 128, 128), 1);
                Pen mau_rulec = new Pen(Color.FromArgb(64, 255, 0, 0), 1);
                SolidBrush mauden = new SolidBrush(Color.FromArgb(250, 0, 0));
                SolidBrush mauden0 = new SolidBrush(Color.FromArgb(250, 255, 0));
                SolidBrush mauden00 = new SolidBrush(Color.FromArgb(0, 0, 255));
                SolidBrush mauden1 = new SolidBrush(Color.FromArgb(0, 255, 0));
                SolidBrush tamden = new SolidBrush(Color.FromArgb(32, 32, 32));
                SolidBrush mauselect = new SolidBrush(Color.FromArgb(250, 250, 250));
                SolidBrush errr = new SolidBrush(Color.FromArgb(164, 164, 164));
                SolidBrush mauselect0 = new SolidBrush(Color.FromArgb(128, 128, 128));
                Pen mau_l = new Pen(Color.FromArgb(96, 96, 96), 1);
                Pen maussss = new Pen(Color.FromArgb(1, 151, 251), 2);
                Pen mau_l0 = new Pen(Color.FromArgb(128, 0, 0), 1);
                Pen mauday = new Pen(Color.FromArgb(255, 255, 255), 2);
                Pen maudaykeo = new Pen(Color.FromArgb(128, 128, 128), 2);
                Pen maudaythang = new Pen(Color.FromArgb(126, 87, 194), 2);
                g.Clear(Color.FromArgb(32, 32, 32));
                g.SmoothingMode = SmoothingMode.HighQuality;
                // for (int x = 0; x < pic_map.Height / 32+1; x++) e.Graphics.DrawLine(mau_rule, new System.Drawing.Point(0, x * 32), new System.Drawing.Point(pic_map.Width, x * 32));
                //  for (int x = 0; x < pic_map.Width / 32+1; x++) e.Graphics.DrawLine(mau_rule, new System.Drawing.Point(x * 32, 0), new System.Drawing.Point(x * 32, pic_map.Height));
                for (int x = 0; x < W0 / 64 + 1; x++) g.DrawLine(mau_rule, new PointF((float)(64 * x + imgx) * zoom, (float)(0 + imgy) * zoom), new PointF((float)(64 * x + imgx) * zoom, (float)(H0 + imgy) * zoom));
                for (int x = 0; x < H0 / 64 + 1; x++) g.DrawLine(mau_rule, new PointF((float)(0 + imgx) * zoom, (float)(64 * x + imgy) * zoom), new PointF((float)(W0 + imgx) * zoom, (float)(64 * x + imgy) * zoom));

                if (vt_menu == menu_chinh)
                {

                    if(viewmap == true)
                    {
                        if (list_anh.Count > 0)
                        {
                            for (int x = 0; x < list_anh.Count; x++)
                            {



                                Draw_anh(g, list_anh[x], false);


                            }

                        }
                    }
                    /*

                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);
                       
                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }
                    */
                    if(play_chinh==true)
                    {
                        int[] mtong = new int[mau_led.Length];

                     
                        

                        for (int x = 0; x < list_hieuung.Count; x++)
                        {
                            int[] tt = get_vt_lop_timeline(dem_play , x);
                             
                                if (tt[0] >= 0 && tt[1] >= 0)
                                {

                                    int ddd = tt[1];
                                    if (list_hieuung[x].HIEUUNG[tt[0]].COGIAN == true)
                                    {
                                        ddd = (int)(chuyendoiF(ddd, list_hieuung[x].HIEUUNG[tt[0]].TONG_HIENTAI, list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC));
                                    }
                                    else
                                    {
                                        ddd = ddd % list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC;
                                    }

                                    int[] mm = list_hieuung[x].HIEUUNG[tt[0]].DATA[ddd];

                                    for (int ii = 0; ii < mm.Length; ii++)
                                    {
                                        if (mm[ii] != 0) mtong[ii] = mm[ii];
                                    }
                                }

 

                        }

                        for (int x = 0; x < mau_led.Length; x++)
                        {
                            if (mtong[x] == 0)
                            {
                                mau_led[x] = Color.Black;
                               
                            }
                            else
                            {
                                mau_led[x] = travemau(mtong[x]);
                            }
                            
                        }
                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, new SolidBrush(mau_led[x]), (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                        dem_play++;
                        if (dem_play >= tong_play) dem_play = 0;
                    }else
                    {
                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                    }
                }
                
                else if (vt_menu == menu_effect_wire)
                {
                    


                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);
                       
                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }
                    if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                    {
                        SolidBrush cc = new SolidBrush(color_layer0[vt_chon_vung % color_layer0.Length]);
                        for (int x = 0; x < list_hieuung[vt_chon_vung].WIRE.Count; x++)
                        {

                            PointF point1_1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].X - 6 + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].Y - 6 + imgy) * zoom);
                           
                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                        }
                    }


                    if (keo_vung == true)
                    {
                        if ((ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            for (int x = 0; x < day_select.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                            }
                        }
                        else
                        {
                            for (int x = 0; x < day_select.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                            }
                        }

                        if (vt_select_vung[0].X != -1 && vt_select_vung[0].Y != -1 && vt_select_vung[1].X != -1 && vt_select_vung[1].Y != -1)
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 128, 128, 128)), new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                            if ((ModifierKeys & Keys.Control) == Keys.Control) g.DrawRectangle(Pens.Red, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));
                            else g.DrawRectangle(Pens.White, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                        }

                    }
                }
                else if (vt_menu == menu_effect_edit)
                {

                    if (thongtin_effect_goc == null)
                    {

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                        

                            if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                            {


                                //  SolidBrush cc = new SolidBrush(color_layer0[vt_chon_vung % color_layer0.Length]);
                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIRE.Count; x++)
                                {

                                    PointF point1_1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].X - 6 + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].Y - 6 + imgy) * zoom);

                                    //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                                    //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                    DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                                }
                            }
                         
                    }
                    else
                    {
                        
                            thongtin_effect_goc.DEM++;
                            if (thongtin_effect_goc.DEM >= thongtin_effect_goc.HIENTAI) thongtin_effect_goc.DEM = 0;



                           
                         

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, new SolidBrush(mau_led[x]), (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }

                        if (thongtin_effect_goc.TRON == false)
                        {
                            g.DrawRectangle(Pens.White, (int)((thongtin_effect_edit.LOCATION.X + imgx - 6) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + imgy - 6) * zoom), thongtin_effect_edit.SIZE.Width * zoom, thongtin_effect_edit.SIZE.Height * zoom);
                        }
                        else
                        {


                            g.DrawEllipse(Pens.White, (int)((thongtin_effect_edit.LOCATION.X + imgx - 6) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + imgy - 6) * zoom), thongtin_effect_edit.SIZE.Width * zoom, thongtin_effect_edit.SIZE.Height * zoom);

                        }
                        g.FillRectangle(new SolidBrush(Color.White), (int)((thongtin_effect_edit.LOCATION.X + (int)thongtin_effect_edit.SIZE.Width - 6 - 5 + imgx) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + (int)thongtin_effect_edit.SIZE.Height / 2 - 6 + imgy) * zoom), 10, 10);
                        g.FillRectangle(new SolidBrush(Color.White), (int)((thongtin_effect_edit.LOCATION.X + (int)thongtin_effect_edit.SIZE.Width / 2 - 6 + imgx) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + (int)thongtin_effect_edit.SIZE.Height - 6 - 5 + imgy) * zoom), 10, 10);

                        pic_time.Refresh();
                    }





                }
                else if (vt_menu == menu_text_wire)
                {



                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }
                    if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                    {
                        SolidBrush cc = new SolidBrush(color_layer0[vt_chon_vung % color_layer0.Length]);
                        for (int x = 0; x < list_hieuung[vt_chon_vung].WIRE.Count; x++)
                        {

                            PointF point1_1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].X - 6 + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                        }
                    }


                    if (keo_vung == true)
                    {
                        if ((ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            for (int x = 0; x < day_select.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                            }
                        }
                        else
                        {
                            for (int x = 0; x < day_select.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                            }
                        }

                        if (vt_select_vung[0].X != -1 && vt_select_vung[0].Y != -1 && vt_select_vung[1].X != -1 && vt_select_vung[1].Y != -1)
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 128, 128, 128)), new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                            if ((ModifierKeys & Keys.Control) == Keys.Control) g.DrawRectangle(Pens.Red, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));
                            else g.DrawRectangle(Pens.White, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                        }

                    }
                }
                else if (vt_menu == menu_text_edit)
                {

                    if (thongtin_effect_goc == null)
                    {

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                       

                            if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                            {
                               

                                //  SolidBrush cc = new SolidBrush(color_layer0[vt_chon_vung % color_layer0.Length]);
                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIRE.Count; x++)
                                {

                                    PointF point1_1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].X - 6 + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIRE[x]].Y - 6 + imgy) * zoom);

                                    //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                                    //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                    DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                                }
                            }
                      
                    }
                    else
                    {
                        
                            thongtin_effect_goc.DEM++;
                          if (thongtin_effect_goc.DEM >= thongtin_effect_goc.HIENTAI) thongtin_effect_goc.DEM = 0;


                       
                           

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, new SolidBrush(mau_led[x]), (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }

                        if (thongtin_effect_goc.TRON == false)
                        {
                            g.DrawRectangle(Pens.White, (int)((thongtin_effect_edit.LOCATION.X + imgx-6) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + imgy-6) * zoom), thongtin_effect_edit.SIZE.Width * zoom, thongtin_effect_edit.SIZE.Height * zoom);
                        }
                        else
                        {


                           g.DrawEllipse(Pens.White, (int)((thongtin_effect_edit.LOCATION.X + imgx-6) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + imgy-6) * zoom), thongtin_effect_edit.SIZE.Width * zoom, thongtin_effect_edit.SIZE.Height * zoom);

                        }
                        g.FillRectangle(new SolidBrush(Color.White), (int)((thongtin_effect_edit.LOCATION.X + (int)thongtin_effect_edit.SIZE.Width    - 6  -5+ imgx) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + (int)thongtin_effect_edit.SIZE.Height / 2 - 6 + imgy) * zoom), 10, 10);
                        g.FillRectangle(new SolidBrush(Color.White), (int)((thongtin_effect_edit.LOCATION.X + (int)thongtin_effect_edit.SIZE.Width / 2    - 6 + imgx) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + (int)thongtin_effect_edit.SIZE.Height - 6-5 + imgy) * zoom), 10, 10);

                        pic_time.Refresh();
                    }





                }
                                        
                else if (vt_menu == menu_vien_wire)
                {



                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }

 
                        if (list_vien_wire_ten.Items.Count > 0)
                        {

                            for (int x = 0; x < list_vien_wire.Count; x++)
                            {
                                SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));
                                for (int y = 0; y < list_vien_wire[x].Count; y++)
                                {

                                    PointF point1 = new PointF((float)(led_full[list_vien_wire[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[x][y]].Y + imgy - 6) * zoom);

                                    DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                }

                                if (list_vien_wire[x].Count > 1 && x != list_vien_wire_ten.SelectedIndex)
                                {
                                    for (int y = 0; y < list_vien_wire[x].Count - 1; y++)
                                    {



                                        PointF point1 = new PointF((float)(led_full[list_vien_wire[x][y]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][y]].Y + imgy) * zoom);
                                        PointF point2 = new PointF((float)(led_full[list_vien_wire[x][y + 1]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][y + 1]].Y + imgy) * zoom);


                                        DrawArrow(g, Pens.White, point1, point2, 4 * zoom);

                                    }
                                }




                            }


                            if (list_vien_wire_ten.SelectedIndex >= 0 && list_vien_wire_ten.SelectedIndex < list_vien_wire_ten.Items.Count)
                            {

                                for (int x = 0; x < list_vien_wire[list_vien_wire_ten.SelectedIndex].Count; x++)
                                {

                                    PointF point1 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].Y + imgy - 6) * zoom);

                                    DrawRoundedRectangle_fill(g, mauden, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                }
                                if (list_vien_wire[list_vien_wire_ten.SelectedIndex].Count > 1)
                                {
                                    for (int x = 0; x < list_vien_wire[list_vien_wire_ten.SelectedIndex].Count - 1; x++)
                                    {

                                        PointF point1 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].X + imgx) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].Y + imgy) * zoom);
                                        PointF point2 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x + 1]].X + imgx) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x + 1]].Y + imgy) * zoom);


                                        DrawArrow(g, Pens.White, point1, point2, 4 * zoom);

                                    }
                                }

                            }

                            for (int x = 0; x < list_vien_wire.Count; x++)
                            {

                                if (list_vien_wire[x].Count > 0)
                                {
                                    if (list_vien_wire[x][0] != -1)
                                    {
                                        int zz = (int)(8 * zoom);
                                        if (zz > 1)
                                        {
                                            using (Font font1 = new Font("Microsoft Sans Serif", zz, FontStyle.Bold, GraphicsUnit.Point))
                                            {


                                                PointF point1 = new PointF((float)(led_full[list_vien_wire[x][0]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][0]].Y + imgy) * zoom);


                                                g.DrawString((1 + x).ToString(), font1, Brushes.White, point1);




                                            }
                                        }
                                    }

                                }



                            }

                        }


                        if (keo_thucong == true)
                        {
                            if ((ModifierKeys & Keys.Control) == Keys.Control)
                            {
                                for (int x = 0; x < day_select.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                    DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                                }
                            }
                            else
                            {
                                for (int x = 0; x < day_select.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                    DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                }
                            }



                        }
                    
                }
                else if (vt_menu == menu_vien_edit)
                {






                    if (thongtin_effect_goc == null)
                    {

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }

                        if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                        {
                            if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                            {

                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {

                                    SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));

                                    for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                                    {

                                        PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy - 6) * zoom);

                                        DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                    }

                                    if (list_hieuung[vt_chon_vung].WIREV[x].Count > 1)
                                    {
                                        for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count - 1; y++)
                                        {



                                            PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy) * zoom);
                                            PointF point2 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y + 1]].X + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y + 1]].Y + imgy) * zoom);


                                            DrawArrow(g, Pens.White, point1, point2, 4 * zoom);

                                        }
                                    }

                                }

                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {

                                    if (list_hieuung[vt_chon_vung].WIREV[x].Count > 0)
                                    {
                                        if (list_hieuung[vt_chon_vung].WIREV[x][0] != -1)
                                        {
                                            int zz = (int)(8 * zoom);
                                            if (zz > 1)
                                            {
                                                using (Font font1 = new Font("Microsoft Sans Serif", zz, FontStyle.Bold, GraphicsUnit.Point))
                                                {
                                                    PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][0]].X + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][0]].Y + imgy) * zoom);
                                                    g.DrawString((1 + x).ToString(), font1, Brushes.YellowGreen, point1);

                                                }
                                            }
                                        }

                                    }

                                }

                            }
                        }

                    }
                    else
                    {

                        thongtin_effect_goc.DEM++;
                        if (thongtin_effect_goc.DEM >= thongtin_effect_goc.HIENTAI) thongtin_effect_goc.DEM = 0;

                        Draw_vien(g, true, thongtin_effect_goc.DEM);

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, new SolidBrush(mau_led[x]), (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                        pic_time.Refresh();
                    }




                }

                else if (vt_menu == menu_vay_wire)
                {



                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }


                    
                        
                            if (list_vien_wire_ten.Items.Count > 0)
                            {

                                for (int x = 0; x < list_vien_wire.Count; x++)
                                {
                                    SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));
                                    for (int y = 0; y < list_vien_wire[x].Count; y++)
                                    {

                                        PointF point1 = new PointF((float)(led_full[list_vien_wire[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[x][y]].Y + imgy - 6) * zoom);

                                        DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                    }




                                }


                                if (list_vien_wire_ten.SelectedIndex >= 0 && list_vien_wire_ten.SelectedIndex < list_vien_wire_ten.Items.Count)
                                {

                                    for (int x = 0; x < list_vien_wire[list_vien_wire_ten.SelectedIndex].Count; x++)
                                    {

                                        PointF point1 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].Y + imgy - 6) * zoom);

                                        DrawRoundedRectangle_fill(g, mauden, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                    }


                                }

                                for (int x = 0; x < list_vien_wire.Count; x++)
                                {

                                    if (list_vien_wire[x].Count > 0)
                                    {
                                        if (list_vien_wire[x][0] != -1)
                                        {
                                            int zz = (int)(8 * zoom);
                                            if (zz > 1)
                                            {
                                                using (Font font1 = new Font("Microsoft Sans Serif", zz, FontStyle.Bold, GraphicsUnit.Point))
                                                {


                                                    PointF point1 = new PointF((float)(led_full[list_vien_wire[x][0]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][0]].Y + imgy) * zoom);


                                                    g.DrawString((1 + x).ToString(), font1, Brushes.White, point1);




                                                }
                                            }
                                        }

                                    }



                                }

                            }

                            if (keo_vung == true)
                            {
                                if ((ModifierKeys & Keys.Control) == Keys.Control)
                                {
                                    for (int x = 0; x < day_select.Count; x++)
                                    {
                                        PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                        DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                                    }
                                }
                                else
                                {
                                    for (int x = 0; x < day_select.Count; x++)
                                    {
                                        PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                        DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                    }
                                }

                                if (vt_select_vung[0].X != -1 && vt_select_vung[0].Y != -1 && vt_select_vung[1].X != -1 && vt_select_vung[1].Y != -1)
                                {
                                    g.FillRectangle(new SolidBrush(Color.FromArgb(64, 128, 128, 128)), new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                                    if ((ModifierKeys & Keys.Control) == Keys.Control) g.DrawRectangle(Pens.Red, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));
                                    else g.DrawRectangle(Pens.White, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                                }

                            }
 
                    
                }
                else if (vt_menu == menu_vay_edit)
                {
                    if (thongtin_effect_goc == null)
                    {

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                        if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                        {
                            if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                            {

                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {

                                    SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));

                                    for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                                    {

                                        PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy - 6) * zoom);

                                        DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                    }



                                }

                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {

                                    if (list_hieuung[vt_chon_vung].WIREV[x].Count > 0)
                                    {
                                        if (list_hieuung[vt_chon_vung].WIREV[x][0] != -1)
                                        {
                                            int zz = (int)(8 * zoom);
                                            if (zz > 1)
                                            {
                                                using (Font font1 = new Font("Microsoft Sans Serif", zz, FontStyle.Bold, GraphicsUnit.Point))
                                                {
                                                    PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][0]].X + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][0]].Y + imgy) * zoom);
                                                    g.DrawString((1 + x).ToString(), font1, Brushes.YellowGreen, point1);

                                                }
                                            }
                                        }

                                    }

                                }

                            }
                        }

                    }
                    else
                    {

                        thongtin_effect_goc.DEM++;
                        if (thongtin_effect_goc.DEM >= thongtin_effect_goc.HIENTAI) thongtin_effect_goc.DEM = 0;

                        Draw_vay(g, true, thongtin_effect_goc.DEM);

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, new SolidBrush(mau_led[x]), (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                        pic_time.Refresh();
                    }




                }
               
                
                else if (vt_menu == menu_thucong_wire)
                {



                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }



                    if (play_thucong == false)
                    {
                        if (list_vien_wire_ten.Items.Count > 0)
                        {

                            for (int x = 0; x < list_vien_wire.Count; x++)
                            {
                                SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));
                                for (int y = 0; y < list_vien_wire[x].Count; y++)
                                {

                                    PointF point1 = new PointF((float)(led_full[list_vien_wire[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[x][y]].Y + imgy - 6) * zoom);

                                    DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                }




                            }


                            if (list_vien_wire_ten.SelectedIndex >= 0 && list_vien_wire_ten.SelectedIndex < list_vien_wire_ten.Items.Count)
                            {

                                for (int x = 0; x < list_vien_wire[list_vien_wire_ten.SelectedIndex].Count; x++)
                                {

                                    PointF point1 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].Y + imgy - 6) * zoom);

                                    DrawRoundedRectangle_fill(g, mauden, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                }


                            }

                            for (int x = 0; x < list_vien_wire.Count; x++)
                            {

                                if (list_vien_wire[x].Count > 0)
                                {
                                    if (list_vien_wire[x][0] != -1)
                                    {
                                        int zz = (int)(8 * zoom);
                                        if (zz > 1)
                                        {
                                            using (Font font1 = new Font("Microsoft Sans Serif", zz, FontStyle.Bold, GraphicsUnit.Point))
                                            {


                                                PointF point1 = new PointF((float)(led_full[list_vien_wire[x][0]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][0]].Y + imgy) * zoom);


                                                g.DrawString((1 + x).ToString(), font1, Brushes.White, point1);




                                            }
                                        }
                                    }

                                }



                            }

                        }

                        if (keo_vung == true)
                        {
                            if ((ModifierKeys & Keys.Control) == Keys.Control)
                            {
                                for (int x = 0; x < day_select.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                    DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                                }
                            }
                            else
                            {
                                for (int x = 0; x < day_select.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                    DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                }
                            }

                            if (vt_select_vung[0].X != -1 && vt_select_vung[0].Y != -1 && vt_select_vung[1].X != -1 && vt_select_vung[1].Y != -1)
                            {
                                g.FillRectangle(new SolidBrush(Color.FromArgb(64, 128, 128, 128)), new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                                if ((ModifierKeys & Keys.Control) == Keys.Control) g.DrawRectangle(Pens.Red, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));
                                else g.DrawRectangle(Pens.White, new Rectangle((int)vt_select_vung[0].X, (int)vt_select_vung[0].Y, (int)vt_select_vung[1].X - (int)vt_select_vung[0].X, (int)vt_select_vung[1].Y - (int)vt_select_vung[0].Y));


                            }

                        }
                    }
                    else
                    {


                        for (int y = 0; y < list_vien_wire[dem_play_thucong].Count; y++)
                        {

                            PointF point1 = new PointF((float)(led_full[list_vien_wire[dem_play_thucong][y]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[dem_play_thucong][y]].Y + imgy - 6) * zoom);

                            DrawRoundedRectangle_fill(g, mauden, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }






                        dem_play_thucong++;
                        if (dem_play_thucong >= list_vien_wire.Count) dem_play_thucong = 0;
                    }

                }
                else if (vt_menu == menu_thucong_edit)
                {

                    if (thongtin_effect_goc == null)
                    {

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                         
                            if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                            {

                                if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                                {
                                    for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV[dem_play_thucong].Count; x++)
                                    {

                                        PointF point1_1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[dem_play_thucong][x]].X - 6 + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[dem_play_thucong][x]].Y - 6 + imgy) * zoom);

                                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                        DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                                    }
                                    dem_play_thucong++;
                                    if (dem_play_thucong >= list_hieuung[vt_chon_vung].WIREV.Count) dem_play_thucong = 0;
                                }

                            }
                         
                    }
                    else
                    {
                        
                            if (tong_chay_thucong > 0)
                            {
                                thongtin_effect_goc.DEM++;
                                if (thongtin_effect_goc.DEM >= thongtin_effect_goc.HIENTAI) thongtin_effect_goc.DEM = 0;
                                if (thongtin_effect_edit != null) Draw_thucong(g, true, thongtin_effect_goc.DEM);
                            }
                         

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, new SolidBrush(mau_led[x]), (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }

                        if (thongtin_effect_goc.TRON == false)
                        {
                            g.DrawRectangle(Pens.White, (int)((thongtin_effect_edit.LOCATION.X + imgx - 6) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + imgy - 6) * zoom), thongtin_effect_edit.SIZE.Width * zoom, thongtin_effect_edit.SIZE.Height * zoom);
                        }
                        else
                        {


                            g.DrawEllipse(Pens.White, (int)((thongtin_effect_edit.LOCATION.X + imgx - 6) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + imgy - 6) * zoom), thongtin_effect_edit.SIZE.Width * zoom, thongtin_effect_edit.SIZE.Height * zoom);

                        }
                        g.FillRectangle(new SolidBrush(Color.White), (int)((thongtin_effect_edit.LOCATION.X + (int)thongtin_effect_edit.SIZE.Width - 6 - 5 + imgx) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + (int)thongtin_effect_edit.SIZE.Height / 2 - 6 + imgy) * zoom), 10, 10);
                        g.FillRectangle(new SolidBrush(Color.White), (int)((thongtin_effect_edit.LOCATION.X + (int)thongtin_effect_edit.SIZE.Width / 2 - 6 + imgx) * zoom), (int)((thongtin_effect_edit.LOCATION.Y + (int)thongtin_effect_edit.SIZE.Height - 6 - 5 + imgy) * zoom), 10, 10);

                        pic_time.Refresh();
                    }





                }
                
             

               
                else if (vt_menu == menu_haoquang_wire)
                {



                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }



                    if (list_vien_wire_ten.Items.Count > 0)
                    {

                        for (int x = 0; x < list_vien_wire.Count; x++)
                        {
                            SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));
                            for (int y = 0; y < list_vien_wire[x].Count; y++)
                            {

                                PointF point1 = new PointF((float)(led_full[list_vien_wire[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[x][y]].Y + imgy - 6) * zoom);

                                DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                            }

                            if (list_vien_wire[x].Count > 1 && x != list_vien_wire_ten.SelectedIndex)
                            {
                                for (int y = 0; y < list_vien_wire[x].Count - 1; y++)
                                {



                                    PointF point1 = new PointF((float)(led_full[list_vien_wire[x][y]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][y]].Y + imgy) * zoom);
                                    PointF point2 = new PointF((float)(led_full[list_vien_wire[x][y + 1]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][y + 1]].Y + imgy) * zoom);


                                    DrawArrow(g, Pens.White, point1, point2, 4 * zoom);

                                }
                            }




                        }


                        if (list_vien_wire_ten.SelectedIndex >= 0 && list_vien_wire_ten.SelectedIndex < list_vien_wire_ten.Items.Count)
                        {

                            for (int x = 0; x < list_vien_wire[list_vien_wire_ten.SelectedIndex].Count; x++)
                            {

                                PointF point1 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].X + imgx - 6) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].Y + imgy - 6) * zoom);

                                DrawRoundedRectangle_fill(g, mauden, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                            }
                            if (list_vien_wire[list_vien_wire_ten.SelectedIndex].Count > 1)
                            {
                                for (int x = 0; x < list_vien_wire[list_vien_wire_ten.SelectedIndex].Count - 1; x++)
                                {

                                    PointF point1 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].X + imgx) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x]].Y + imgy) * zoom);
                                    PointF point2 = new PointF((float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x + 1]].X + imgx) * zoom, (float)(led_full[list_vien_wire[list_vien_wire_ten.SelectedIndex][x + 1]].Y + imgy) * zoom);


                                    DrawArrow(g, Pens.White, point1, point2, 4 * zoom);

                                }
                            }

                        }

                        for (int x = 0; x < list_vien_wire.Count; x++)
                        {

                            if (list_vien_wire[x].Count > 0)
                            {
                                if (list_vien_wire[x][0] != -1)
                                {
                                    int zz = (int)(8 * zoom);
                                    if (zz > 1)
                                    {
                                        using (Font font1 = new Font("Microsoft Sans Serif", zz, FontStyle.Bold, GraphicsUnit.Point))
                                        {


                                            PointF point1 = new PointF((float)(led_full[list_vien_wire[x][0]].X + imgx) * zoom, (float)(led_full[list_vien_wire[x][0]].Y + imgy) * zoom);


                                            g.DrawString((1 + x).ToString(), font1, Brushes.White, point1);




                                        }
                                    }
                                }

                            }



                        }

                    }


                    if (keo_thucong == true)
                    {
                        if ((ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            for (int x = 0; x < day_select.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);



                            }
                        }
                        else
                        {
                            for (int x = 0; x < day_select.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_full[day_select[x]].X - 6 + imgx) * zoom, (float)(led_full[day_select[x]].Y - 6 + imgy) * zoom);
                                DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                            }
                        }



                    }

                }
                else if (vt_menu == menu_haoquang_edit)
                {






                    if (thongtin_effect_goc == null)
                    {

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }

                        if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                        {
                            if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                            {

                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {

                                    SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));

                                    for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                                    {

                                        PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy - 6) * zoom);

                                        DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                    }



                                }

                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {

                                    if (list_hieuung[vt_chon_vung].WIREV[x].Count > 0)
                                    {
                                        if (list_hieuung[vt_chon_vung].WIREV[x][0] != -1)
                                        {
                                            int zz = (int)(8 * zoom);
                                            if (zz > 1)
                                            {
                                                using (Font font1 = new Font("Microsoft Sans Serif", zz, FontStyle.Bold, GraphicsUnit.Point))
                                                {
                                                    PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][0]].X + imgx) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][0]].Y + imgy) * zoom);
                                                    g.DrawString((1 + x).ToString(), font1, Brushes.YellowGreen, point1);

                                                }
                                            }
                                        }

                                    }

                                }
                            }

                        }

                    }
                    else
                    {

                        thongtin_effect_goc.DEM++;
                        if (thongtin_effect_goc.DEM >= thongtin_effect_goc.HIENTAI) thongtin_effect_goc.DEM = 0;

                        Draw_haoquang(g, true, thongtin_effect_goc.DEM);

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, new SolidBrush(mau_led[x]), (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                        pic_time.Refresh();
                    }




                }
                else if (vt_menu == menu_haoquang_add)
                {






                    if (play_haoquang == false)
                    {


                      




                        if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
                        {
                            if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                            {

                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {

                                    SolidBrush mmmmmm = new SolidBrush(Color.FromArgb(128, color_layer0[x % color_layer0.Length].R, color_layer0[x % color_layer0.Length].G, color_layer0[x % color_layer0.Length].B));

                                    for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                                    {

                                        PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy - 6) * zoom);

                                        DrawRoundedRectangle(g, new Pen(Color.FromArgb(64,64,64)), (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                    }



                                }

                                
                            }

                        }


                       if(list_hieung_haoquang.SelectedIndex>=0)
                        {
                            if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                            {
                            
                                for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                {



                                    for (int z = 0; z < list_hieung_haoquang.Items.Count; z++)

                                    {
                                        for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                                        {
                                            SolidBrush mmmmmm = new SolidBrush(halo_color[z][x, y]);
                                            PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy - 6) * zoom);

                                            if (z == list_hieung_haoquang.SelectedIndex) DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                            else DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X + 3, (int)point1.Y + 3, (int)(6 * zoom), (int)(6 * zoom), 3, 3);

                                        }
                                    }


                                }


                                
                        
                            }
                        }

                        if (vt_haoquang[0] >= 0 && vt_haoquang[1] >= 0)
                        {
                            Color[,] halo_color_temp = new Color[halo_tia, halo_hang];

                            if (halo_draw == 0)
                            {
                                halo_color_temp[vt_haoquang[0], vt_haoquang[1]] = mauvien_chon;
                            }
                            else if (halo_draw == 1)
                            {
                                halo_color_temp[vt_haoquang[0], vt_haoquang[1]] = mauvien_chon;

                                int tiamoi = halo_tia - vt_haoquang[0];
                                if (tiamoi >= 0 && tiamoi < halo_tia) halo_color_temp[tiamoi, vt_haoquang[1]] = mauvien_chon;

                            }
                            else if (halo_draw == 2)
                            {
                                // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                int vh = vt_haoquang[1];
                                int vc = vt_haoquang[0];
                                for (int x = 0; x < halo_tia; x++)
                                {

                                    halo_color_temp[vc, vh] = mauvien_chon;

                                    vh++; if (vh >= halo_hang) vh = 0;
                                    vc++; if (vc >= halo_tia) vc = 0;
                                }

                            }
                            else if (halo_draw == 3)
                            {
                                // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                int vh = vt_haoquang[1];
                                int vc = vt_haoquang[0];
                                for (int x = 0; x < halo_tia; x++)
                                {

                                    halo_color_temp[vc, vh] = mauvien_chon;

                                    vh--; if (vh < 0) vh = halo_hang - 1;
                                    vc++; if (vc >= halo_tia) vc = 0;
                                }

                            }
                            else if (halo_draw == 4)
                            {
                                // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;
                                int vh = vt_haoquang[1];
                                int vc = vt_haoquang[0];
                                for (int x = 0; x < halo_tia; x++)
                                {

                                    halo_color_temp[vc, vh] = mauvien_chon;

                                    vh--; if (vh < 0) vh = halo_hang - 1;
                                    vc++; if (vc >= halo_tia) vc = 0;
                                }
                                vh = vt_haoquang[1];
                                vc = vt_haoquang[0];
                                for (int x = 0; x < halo_tia; x++)
                                {

                                    halo_color_temp[vc, vh] = mauvien_chon;

                                    vh++; if (vh >= halo_hang) vh = 0;
                                    vc++; if (vc >= halo_tia) vc = 0;
                                }
                            }
                            else if (halo_draw == 5)
                            {
                                // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;

                                for (int x = 0; x < halo_tia; x++)
                                {

                                    halo_color_temp[x, vt_haoquang[1]] = mauvien_chon;

                                }

                            }
                            else if (halo_draw == 6)
                            {
                                // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;

                                for (int x = 0; x < halo_hang; x++)
                                {

                                    halo_color_temp[vt_haoquang[0], x] = mauvien_chon;

                                }

                            }
                            else if (halo_draw == 7)
                            {
                                // halo_color[list_hieung_haoquang.SelectedIndex][vv[0], vv[1]] = mauvien_chon;


                                for (int x = 0; x < halo_tia / num_nhan.Value; x++)
                                {
                                   
                                    // halo_color_temp[x * (int)num_nhan.Value, vt_haoquang[1]] = mauvien_chon;
                                    if (x * (int)num_nhan.Value + vt_haoquang[0] < halo_tia)  halo_color_temp[x * (int)num_nhan.Value + vt_haoquang[0], vt_haoquang[1]] = mauvien_chon;
                                    else halo_color_temp[x * (int)num_nhan.Value + vt_haoquang[0]- halo_tia, vt_haoquang[1]] = mauvien_chon;

                                }

                            }

                            if (nhay == true)
                            {
                                if (list_hieung_haoquang.SelectedIndex >= 0)
                                {
                                    if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                                    {

                                        for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                                        {




                                            for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                                            {
                                                SolidBrush mmmmmm = new SolidBrush(halo_color_temp[x, y]);
                                                PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy - 6) * zoom);

                                                DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                                            }


                                        }




                                    }
                                }
                            }


                        }



                    }
                    else
                    {
                         
                        dem_play_haoquang++;
                        if (dem_play_haoquang >= halo_tia) dem_play_haoquang = 0;



                        if (list_hieuung[vt_chon_vung].WIREV.Count > 0)
                        {
                            Color[,] temp= new Color[halo_tia,halo_hang];

                            for (int x = 0; x < halo_tia; x++)
                            {
                                for (int y = 0; y < halo_hang; y++)
                                {
                                    temp[x, y] = Color.Black;
                                }
                            }


                            for (int z = 0; z < list_hieung_haoquang.Items.Count; z++)
                            {
                                 
                                    if (halo_set[z][1] == 1) halo_color_play[z] = chaytoi_moi(halo_color_play[z], halo_tia, halo_hang);
                                    else if (halo_set[z][1] == 2) halo_color_play[z] = chaylui_moi(halo_color_play[z], halo_tia, halo_hang);

                                    if (halo_set[z][0] == 1) halo_color_play[z] = toara_moi(halo_color_play[z], halo_tia, halo_hang);
                                    else if (halo_set[z][0] == 2) halo_color_play[z] = toavao_moi(halo_color_play[z], halo_tia, halo_hang);


                                    if (halo_set[z][2] == 1) halo_color_play[z] = doimau_haoquang_moi(halo_color_play[z], halo_tia, halo_hang);
                                    else if (halo_set[z][2] == 2) halo_color_play[z] = toidan_moi(halo_color_play[z], halo_tia, halo_hang);
                                    else if (halo_set[z][2] == 3) halo_color_play[z] = sangdan_moi(halo_color_play[z], halo_tia, halo_hang);

                                for (int x = 0; x < halo_tia; x++)
                                {
                                    for (int y = 0; y < halo_hang; y++)
                                    {
                                        if (halo_color_play[z][x, y] != Color.Transparent) temp[x, y] = halo_color_play[z][x, y];
                                    }
                                }

                            }


                            for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
                            {



                                 
                                    for (int y = 0; y < list_hieuung[vt_chon_vung].WIREV[x].Count; y++)
                                    {
                                        SolidBrush mmmmmm = new SolidBrush(temp[x, y]);
                                        PointF point1 = new PointF((float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].X + imgx - 6) * zoom, (float)(led_full[list_hieuung[vt_chon_vung].WIREV[x][y]].Y + imgy - 6) * zoom);

                                       DrawRoundedRectangle_fill(g, mmmmmm, (int)point1.X, (int)point1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                        

                                    }
                                

                            }








                        }
 
                    }




                }
                else if (vt_menu == menu_nap)
                {


                    for (int x = 0; x < led_full.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);
                        PointF point1_2 = new PointF((float)(led_full[x].X - 3 + imgx) * zoom, (float)(led_full[x].Y - 3 + imgy) * zoom);
                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        if (x == 0) DrawRoundedRectangle_fill(g, mauden0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        else
                        {
                            if (vt_port2 < 0 || vt_port2 >= led_full.Count)
                            {
                                if (x > 0) DrawRoundedRectangle_fill(g, mauden, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            }
                            else
                            {
                                if (x < vt_port2) DrawRoundedRectangle_fill(g, mauden, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else if (x == vt_port2) DrawRoundedRectangle_fill(g, mauden00, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else DrawRoundedRectangle_fill(g, mauden1, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                            }
                        }



                        g.FillEllipse(tamden, new Rectangle((int)point1_2.X, (int)point1_2.Y, (int)(6 * zoom), (int)(6 * zoom)));

                    }

                    if (led_full.Count > 1)
                    {

                        for (int i = 0; i < led_full.Count - 1; i++)
                        {
                            PointF point1 = new PointF((float)(led_full[i].X + imgx) * zoom, (float)(led_full[i].Y + imgy) * zoom);
                            PointF point2 = new PointF((float)(led_full[i + 1].X + imgx) * zoom, (float)(led_full[i + 1].Y + imgy) * zoom);
                            if ( keo_thucong == true)
                            {
                                DrawArrow(g, maudaykeo, point1, point2, 4 * zoom);
                            }
                            else
                            {
                                DrawArrow(g, mauday, point1, point2, 4 * zoom);
                            }
                        }


                    }

                    if (keo_nap== true && (vt_menu1 == menu_wire_connect0 || vt_menu1 == menu_wire_connect1))
                    {


                        for (int x = 0; x < led_select.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[led_select[x]].X - 6 + imgx) * zoom, (float)(led_full[led_select[x]].Y - 6 + imgy) * zoom);

                            DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                        for (int x = 0; x < cu.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[cu[x]].X - 6 + imgx) * zoom, (float)(led_full[cu[x]].Y - 6 + imgy) * zoom);

                            DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }



                        if (led_select.Count > 1)
                        {
                            for (int x = 0; x < led_select.Count - 1; x++)
                            {
                                PointF point1 = new PointF((float)(led_full[led_select[x]].X + imgx) * zoom, (float)(led_full[led_select[x]].Y + imgy) * zoom);
                                PointF point2 = new PointF((float)(led_full[led_select[x + 1]].X + imgx) * zoom, (float)(led_full[led_select[x + 1]].Y + imgy) * zoom);

                                DrawArrow(g, mauday, point1, point2, 4 * zoom);

                            }

                        }

                        if (vt_menu1 == menu_wire_connect0)
                        {
                            double xxx = vt_chuot.X / zoom - imgx;
                            double yyy = vt_chuot.Y / zoom - imgy;

                            PointF point3 = new PointF((float)(led_full[led_select[led_select.Count - 1]].X + imgx) * zoom, (float)(led_full[led_select[led_select.Count - 1]].Y + imgy) * zoom);
                            PointF point4 = new PointF((float)(xxx + imgx) * zoom, (float)(yyy + imgy) * zoom);
                            DrawArrow(g, mauday, point3, point4, 6 * zoom);
                        }

                        if (vt_menu1 == menu_wire_connect1)
                        {
                            double xxx = vt_chuot.X / zoom - imgx;
                            double yyy = vt_chuot.Y / zoom - imgy;
                            PointF point3 = new PointF((float)(led_full[vt_auto1].X + imgx) * zoom, (float)(led_full[vt_auto1].Y + imgy) * zoom);
                            PointF point4 = new PointF((float)(xxx + imgx) * zoom, (float)(yyy + imgy) * zoom);
                            DrawArrow(g, maudaythang, point3, point4, 6 * zoom);
                        }
                    }
                }



            }
            pic_map.Image = moi;
            pic_map.Invalidate();
            timer_map.Enabled = true;
        }
       
        private void get_thongtin_vung()
        {
            if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count)
            {
                if (list_hieuung[vt_chon_vung].WIRE.Count > 0)
                {
                   

                    float ttx = 0;
                    float tty = 0;

                    for (int x1 = 0; x1 < list_hieuung[vt_chon_vung].WIRE.Count; x1++)
                    {
                        if (led_full[list_hieuung[vt_chon_vung].WIRE[x1]].X >= ttx) ttx =(float) led_full[list_hieuung[vt_chon_vung].WIRE[x1]].X;
                        if (led_full[list_hieuung[vt_chon_vung].WIRE[x1]].Y >= tty) tty = (float)led_full[list_hieuung[vt_chon_vung].WIRE[x1]].Y;

                    }
                    float ttw = ttx;
                    float tth = tty;
                    for (int x1 = 0; x1 < list_hieuung[vt_chon_vung].WIRE.Count; x1++)
                    {
                        if (led_full[list_hieuung[vt_chon_vung].WIRE[x1]].X <= ttw) ttw = (float)led_full[list_hieuung[vt_chon_vung].WIRE[x1]].X;
                        if (led_full[list_hieuung[vt_chon_vung].WIRE[x1]].Y <= tth) tth = (float)led_full[list_hieuung[vt_chon_vung].WIRE[x1]].Y;

                    }
                   
 


                    float max = 0;
                    for (int x = 0; x < list_hieuung[vt_chon_vung].WIRE.Count; x++)
                    {
                        float aa = chieudaiF(new PointF(ttx + (ttx - ttw) / 2 + 12, tty + (tty - tth) / 2 + 12), new PointF((float)led_full[list_hieuung[vt_chon_vung].WIRE[x]].X, (float)led_full[list_hieuung[vt_chon_vung].WIRE[x]].Y));
                        if (aa >= max) max = aa;

                    }
                     
                    thongtin_wire_vung=new THONGTIN_WIRE_VUNG(list_hieuung[vt_chon_vung].WIRE, list_hieuung[vt_chon_vung].WIREV, list_hieuung[vt_chon_vung].WIRE.Count,new PointF(ttw, tth),new SizeF(ttx - ttw, tty - tth),max,new PointF(ttx + (ttx - ttw) / 2+6, tty + (tty - tth) / 2+6));
                    thongtin_effect_edit = new THONGTIN_EFFECT_EDIT(new PointF(ttw, tth), new PointF(ttx + (ttx - ttw) / 2+6, tty + (tty - tth) / 2 + 6), new SizeF(ttx - ttw+12, tty - tth+12), 0);


                }
            }

        }
       
        private void up_setvung()
        {
               

             int xxx = get_tong_vung(vt_chon_vung);
            if (xxx - 10 >= 16000)
            {
                hScrollBar2.Value = 15999;
            }
            else
            {
                if (xxx - 10 >= 0) hScrollBar2.Value = xxx - 10;
                else if (xxx >= 0) hScrollBar2.Value = xxx;
            }

           

            //if(xxx>= hScrollBar2.Maximum)
            //  {
            //   if (hScrollBar2.Maximum - 1 >= 0) hScrollBar2.Value = xxx;
            //   else hScrollBar2.Value = 0;
            // }else hScrollBar2.Value = xxx;



            up_timeline();
        }

      


        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.DAO == false) thongtin_effect_goc.DAO = true;
                else thongtin_effect_goc.DAO = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.TRON == false) thongtin_effect_goc.TRON = true;
                else thongtin_effect_goc.TRON = false;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.NGANG == false) thongtin_effect_goc.NGANG = true;
                else thongtin_effect_goc.NGANG = false;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.DOC == false) thongtin_effect_goc.DOC = true;
                else thongtin_effect_goc.DOC = false;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.MAU == false) thongtin_effect_goc.MAU = true;
                else thongtin_effect_goc.MAU = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
                progressBar2.Value = 0;
                progressBar2.Value = 11;
                List<int[]> dulieu = new List<int[]>();
                int hhh = 0;
                int cu = 0;
                
                if(vt_menu==menu_effect_edit)
                {
                    for (int x = 0; x < thongtin_effect_goc.TONG; x++)
                    {
                        int[] dd = get_data_effect(x);

                        dulieu.Add(dd);
                        hhh = (int)(chuyendoiF(x, thongtin_effect_goc.TONG, 10));
                        if (cu != hhh)
                        {
                            progressBar2.Value = hhh;
                            cu = hhh;
                            Application.DoEvents();
                        }

                        // 
                    }
                }
                else if (vt_menu == menu_text_edit)
                {
                    for (int x = 0; x < thongtin_effect_goc.TONG; x++)
                    {
                        int[] dd = get_data_effect(x);

                        dulieu.Add(dd);
                        hhh = (int)(chuyendoiF(x, thongtin_effect_goc.TONG, 10));
                        if (cu != hhh)
                        {
                            progressBar2.Value = hhh;
                            cu = hhh;
                            Application.DoEvents();
                        }

                        // 
                    }
                }
                else if (vt_menu == menu_thucong_edit)
                {
                    if (tong_chay_thucong > 0)
                    {

                      
                        for (int x = 0; x < thongtin_effect_goc.TONG; x++)
                        {
                            int vt_thucong_hien = (x / (int)speed_thucong.Value) % tong_chay_thucong;

                            int[] dd = get_data_thucong(x, vt_thucong_hien);

                            dulieu.Add(dd);
                            hhh = (int)(chuyendoiF(x, thongtin_effect_goc.TONG, 10));
                            if (cu != hhh)
                            {
                                progressBar2.Value = hhh;
                                cu = hhh;
                                Application.DoEvents();
                            }

                            // 
                        }
                    }
                }
               

                list_hieuung[vt_chon_vung].HIEUUNG.Add(new LAYER_HIEUUNG(thongtin_effect_goc.BATDAU, thongtin_effect_goc.TONG, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.CODAN, dulieu));
                get_tong();
                up_vt_keo(vt_chon_vung);
                thongtin_effect_goc = null;
           
                set_vung();

              
                     panel_wail.Visible = false;
                timer_map.Enabled = true;
                timer.Enabled = true;
                Application.DoEvents();
            }

        }

        private void up_vt_keo(int vv)
        {
            if (vv >= 0 && vv < list_hieuung.Count)
            {
                
                int ff = get_tong_vung(vv);
                if (ff >= 0 && ff < hScrollBar2.Maximum && hScrollBar2.Maximum > 1)
                {

                    hScrollBar2.Value = hScrollBar2.Maximum - 1;
                    vtx_time = hScrollBar2.Value;
                }
                else
                {
                    if (ff >= 0 && ff < hScrollBar2.Maximum && hScrollBar2.Maximum > 1)
                    {
                        hScrollBar2.Value = ff;
                        vtx_time = hScrollBar2.Value;
                    }
                    else
                    {
                        hScrollBar2.Value = 0;
                        vtx_time = hScrollBar2.Value;
                    }

                }

             
            }

        }

            private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.TRON == false) thongtin_effect_goc.TRON = true;
                else thongtin_effect_goc.TRON = false;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.GOC++;


                if (thongtin_effect_goc.GOC >= 4) thongtin_effect_goc.GOC = 0;
            }
         
        }

        private void button7_Click(object sender, EventArgs e)
        {
            timer_map.Enabled = false;
            timer.Enabled = false;
            panel_wail.Visible = true;
            Application.DoEvents();
            progressBar2.Value = 0;
            progressBar2.Maximum = 101;
            List<int[]> dulieu = render_effect();

             

            panel_wail.Visible = false;
            timer_map.Enabled = true;
            timer.Enabled = true;
            Application.DoEvents();
        }

        private void pic_time_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        int keo_time_add = -1;

        private void pic_time_MouseDown(object sender, MouseEventArgs e)
        {

            if (vt_menu != menu_chinh)
            {

                if (thongtin_effect_goc == null)
                {


                }
                else
                {
                    int vt_add = vt_chon_vung - vt_hien_vung;
                    if (vt_add >= 0 && vt_add < 4)
                    {
                        int bd = get_tong_vung(vt_chon_vung);

                        if (e.X + vtx_time >= thongtin_effect_goc.BATDAU + bd && e.X + vtx_time < thongtin_effect_goc.BATDAU + bd + thongtin_effect_goc.HIENTAI)
                        {
                            keo_time_add = 0;
                            vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                        }
                        else if (e.X + vtx_time >= thongtin_effect_goc.BATDAU + bd + thongtin_effect_goc.HIENTAI && e.X + vtx_time < thongtin_effect_goc.BATDAU + bd + thongtin_effect_goc.HIENTAI + 10)
                        {
                            keo_time_add = 1;
                            vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                            /*
                            if(vt_menu == menu_effect_edit && vien_thucong==true)
                            {
                                keo_time_add = -1;
                            }
                            else
                            {
                                keo_time_add = 1;
                                vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                            }
                           */
                        }
                        else
                        {
                            keo_time_add = -1;

                            vt_keo_time_xxx = -1;
                            vt_keo_time_yyy = -1;
                            vt_keo_time_loai = -1;
                            int vv = (e.Location.Y - 20) / 20 + vt_hien_vung;
                            if (vv >= 0 && vv < list_hieuung.Count)
                            {
                                vt_keo_time_yyy = vv;

                                int[] tave = get_vt_lop_timeline(e.Location.X + vtx_time, vt_keo_time_yyy);

                                if (tave[0] >= 0)
                                {
                                    vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                                    vt_keo_time_xxx = tave[0];

                                    // MessageBox.Show("vt_hang " + vt_keo_time_yyy.ToString()  +"vt_lop " + vt_keo_time_xxx.ToString() + " loai" + tave[1].ToString());
                                    if (tave[1] >= list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI - 3) vt_keo_time_loai = 1;
                                    else vt_keo_time_loai = 0;
                                }
                            }


                        }

                    }

                }
                toolStripMenuItem8.Enabled = false;
                toolStripMenuItem9.Enabled = false;
                toolStripMenuItem10.Enabled = false;
               // toolStripMenuItem11.Enabled = false;
                toolStripMenuItem12.Enabled = false;

            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (e.Location.X + vtx_time >= 0 && e.Location.X + vtx_time < tong_play) dem_play = e.Location.X + vtx_time;
                    toolStripMenuItem8.Enabled = false;
                    toolStripMenuItem9.Enabled = false;
                    toolStripMenuItem10.Enabled = false;
                   // toolStripMenuItem11.Enabled = false;
                    toolStripMenuItem12.Enabled = false;
                    vt_keo_time_xxx = -1;
                    vt_keo_time_yyy = -1;
                    vt_keo_time_loai = -1;
                    int vv = (e.Location.Y - 20) / 20 + vt_hien_vung;
                    if (vv >= 0 && vv < list_hieuung.Count)
                    {
                        vt_keo_time_yyy = vv;

                        int[] tave = get_vt_lop_timeline(e.Location.X + vtx_time, vt_keo_time_yyy);

                        if (tave[0] >= 0)
                        {
                            vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                            vt_keo_time_xxx = tave[0];

                            // MessageBox.Show("vt_hang " + vt_keo_time_yyy.ToString()  +"vt_lop " + vt_keo_time_xxx.ToString() + " loai" + tave[1].ToString());
                            if (tave[1] >= list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI - 3) vt_keo_time_loai = 1;
                            else vt_keo_time_loai = 0;
                        }
                    }
                }else
                {
                   
                    
                  

                    vt_keo_time_xxx = -1;
                    vt_keo_time_yyy = -1;
                    int vv = (e.Location.Y - 20) / 20 + vt_hien_vung;
                    toolStripMenuItem8.Enabled = false;
                    toolStripMenuItem9.Enabled = false;
                 
                        toolStripMenuItem10.Enabled = false;
                        //toolStripMenuItem11.Enabled = false;
                        toolStripMenuItem12.Enabled = false;

                        if (vv >= 0 && vv < list_hieuung.Count)
                    {
                        
                        int[] tave = get_vt_lop_timeline(e.Location.X + vtx_time, vv);



                        if (tave[0] >= 0)
                        {
                            vt_keo_time_yyy = vv;
                            vt_keo_time_xxx = tave[0];
                            if (tave[0] >= 0 && vv >= 0 )
                            {
                                toolStripMenuItem10.Enabled = true;
                                //toolStripMenuItem11.Enabled = true;
                                toolStripMenuItem12.Enabled = true;
                            }else
                            {
                                toolStripMenuItem10.Enabled = false;
                                //toolStripMenuItem11.Enabled = false;
                                toolStripMenuItem12.Enabled = false;
                            }


                            if (tave[0] >= 0 && vv >= 0 && tave[0] < list_hieuung[vv].HIEUUNG.Count - 1)
                            {
                                toolStripMenuItem8.Enabled = true;

                            }
                            else
                            {
                                toolStripMenuItem8.Enabled = false;
                            }
                            if (tave[0] > 0 && vv >= 0  )
                            {
                                toolStripMenuItem9.Enabled = true;

                            }
                            else
                            {
                                toolStripMenuItem9.Enabled = false;
                            }
                        }else
                        {
                           
                        }
                    }

                }

            }
        }
        int vt_keo_time_xxx = -1;
        int vt_keo_time_yyy = -1;
        int vt_keo_time_loai= -1;
        private void pic_time_MouseMove(object sender, MouseEventArgs e)
        {
            if (vt_menu != menu_chinh)
            {

                if (thongtin_effect_goc != null)
                {
 
                    if (keo_time_add == 0)
                    {
                        thongtin_effect_goc .BATDAU = (int)(thongtin_effect_goc.BATDAU + (e.X - vt_chuot.X+vtx_time));
                        vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                        if (thongtin_effect_goc.BATDAU <= 0) thongtin_effect_goc.BATDAU = 0;
                      
                    }
                    else if (keo_time_add == 1)
                    {
                        thongtin_effect_goc.HIENTAI = (int)(thongtin_effect_goc.HIENTAI + (e.X - vt_chuot.X + vtx_time));
                        vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                        if (thongtin_effect_goc.HIENTAI <= 0) thongtin_effect_goc.HIENTAI = 3;
                        
                    }
                }
                if (vt_keo_time_loai == 0)
                {
                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU = (int)(list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU + (e.X - vt_chuot.X + vtx_time));
                    vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                    if (list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU <= 0) list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU = 0;
                }
                else if (vt_keo_time_loai == 1)
                {
                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI = (int)(list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI + (e.X - vt_chuot.X + vtx_time));
                    if (list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI <= 0) list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI = 3;
                    vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);


                }
            }
            else
            {
                if(vt_keo_time_loai==0)
                {
                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU = (int)(list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU + (e.X - vt_chuot.X + vtx_time));
                    vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                    if (list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU <= 0) list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].BATDAU = 0;
                }
                else if (vt_keo_time_loai == 1)
                {
                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI = (int)(list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI + (e.X - vt_chuot.X + vtx_time));
                    if (list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI <= 0) list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].TONG_HIENTAI = 3;
                    vt_chuot = new PointF(e.Location.X + vtx_time, e.Location.Y + vtx_time);
                  

                }

            }
                 
        }

        private void pic_time_MouseUp(object sender, MouseEventArgs e)
        {
            if (keo_time_add >= 0) { keo_time_add = -1; get_tong();     /* int vv = (int)hScrollBar2.Maximum - 100; if (vv >= 0) { hScrollBar2.Value = vv; vtx_time = hScrollBar2.Value; }*/ };
            if (vt_keo_time_loai >= 0) { vt_keo_time_loai = -1; get_tong(); /*int vv = (int)hScrollBar2.Maximum - 100; if (vv >= 0) { hScrollBar2.Value = vv; vtx_time = hScrollBar2.Value; }*/ };
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.CODAN == false) thongtin_effect_goc.CODAN = true;
                else thongtin_effect_goc.CODAN = false;
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            set_vung();
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.DMAU++;
                if (thongtin_effect_goc.DMAU >= 41) thongtin_effect_goc.DMAU = 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            list_hieuung[vt_chon_vung].WIRE = new List<int>();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int>temp= new List<int>();

            for (int x = 0; x < led_full.Count; x++) temp.Add(x);
                 list_hieuung[vt_chon_vung].WIRE = temp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            vt_menu = menu_chinh;
           
           vt_chon_vung = -1;

            set_vung();
            int jj = get_tong_vung(vt_chon_vung);
            if (dem_play <= jj) dem_play = jj;
        }

        private void up_timeline( )
        {
            vtx_time = hScrollBar2.Value;

        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            up_timeline();


        }

        Bitmap text_goc = new Bitmap(10000, 100);
 
        private int[] get_size(Bitmap ddd, int w0, int h0)
        {
            int xxx = 0;
            int yyy = 0;
            int xxx0 = w0;
            int yyy0 = h0;
            int[] anh_text = new int[4];
            LockBitmap lockBitmap = new LockBitmap(ddd);
            lockBitmap.LockBits();
          
            for (int xx = 0; xx < w0; xx++)
            {
                for (int yy = 0; yy < h0; yy++)
                {
                    
                    if (lockBitmap.GetPixel(xx, yy).A >= 10)
                    {
                        if (yy < yyy0) yyy0 = yy;
                        if (xx < xxx0) xxx0 = xx;
                        if (xx > xxx) xxx = xx;
                        if (yy > yyy) yyy = yy;
                    }

                }

            }
            lockBitmap.UnlockBits();
            anh_text[0] = xxx0;
            anh_text[1] = yyy0;

            anh_text[2] = xxx - xxx0 + 1;
            anh_text[3] = yyy - yyy0 + 1;

            return anh_text;
        }
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int memcpy(byte* dest, byte* src, long count);
        private unsafe Bitmap Crop(Bitmap srcImg, Rectangle rectangle)
        {
            if ((srcImg.Width == rectangle.Width) && (srcImg.Height == rectangle.Height))
                return srcImg;

            var srcImgBitmapData = srcImg.LockBits(new Rectangle(0, 0, srcImg.Width, srcImg.Height), ImageLockMode.ReadOnly, srcImg.PixelFormat);
            var bpp = srcImgBitmapData.Stride / srcImgBitmapData.Width; // 3 or 4
            var srcPtr = (byte*)srcImgBitmapData.Scan0.ToPointer() + rectangle.Y * srcImgBitmapData.Stride + rectangle.X * bpp;
            var srcStride = srcImgBitmapData.Stride;

            var dstImg = new Bitmap(rectangle.Width, rectangle.Height, srcImg.PixelFormat);
            var dstImgBitmapData = dstImg.LockBits(new Rectangle(0, 0, dstImg.Width, dstImg.Height), ImageLockMode.WriteOnly, dstImg.PixelFormat);
            var dstPtr = (byte*)dstImgBitmapData.Scan0.ToPointer();
            var dstStride = dstImgBitmapData.Stride;

            for (int y = 0; y < rectangle.Height; y++)
            {
                memcpy(dstPtr, srcPtr, dstStride);
                srcPtr += srcStride;
                dstPtr += dstStride;
            }

            srcImg.UnlockBits(srcImgBitmapData);
            dstImg.UnlockBits(dstImgBitmapData);
            return dstImg;
        }
        private void getchu()
        {
            timer_map.Enabled = false;
            timer.Enabled = false;
            panel_wail.Visible = true;
            Application.DoEvents();

            if (vt_menu == menu_text_edit && text_chu.Text.Length>0 && fontListBox1.SelectedIndex>=0)
            {
                text_goc = new Bitmap(10000, 100);
                Graphics g = Graphics.FromImage(text_goc);
                g.Clear(Color.Transparent);
                var stringFormatFlags = default(StringFormatFlags);
                StringFormat StringFormat = new StringFormat(stringFormatFlags);
                StringFormat.LineAlignment = StringAlignment.Near;
                StringFormat.Alignment = StringAlignment.Near;

                int style = 0;
                if (com_style_font.SelectedIndex == 0) style = 0;
                else if (com_style_font.SelectedIndex == 1) style = 2;
                else if (com_style_font.SelectedIndex == 2) style = 1;
                else if (com_style_font.SelectedIndex == 3) style = 4;


                var rect = new Rectangle(0, 0, 10000, 100);
                Font stringFont = new Font(fontListBox1.Items[fontListBox1.SelectedIndex].ToString(), (int)num_size_font.Value, (FontStyle)style);

                if (com_font_render.SelectedIndex >= 0) g.TextRenderingHint = (TextRenderingHint)com_font_render.SelectedIndex;
                else g.TextRenderingHint = (TextRenderingHint)0;
                g.TextContrast = 4;
                g.SmoothingMode = SmoothingMode.Default;
                g.CompositingQuality = CompositingQuality.Default;

                var pen = new Pen(Color.Blue);
                var measuredSize = g.MeasureString(text_chu.Text, stringFont, rect.Size, StringFormat);

                g.DrawString(text_chu.Text, stringFont, new SolidBrush(Color.White), rect, StringFormat);
                int[] anh_text = get_size(text_goc, (int)measuredSize.Width, (int)measuredSize.Height);
                //  MessageBox.Show(anh_x.ToString() + "  " + anh_y.ToString() + "  " + anh_w.ToString() + "  " + anh_h.ToString());
                if (anh_text[0] > 0) anh_text[0]--;
                if (anh_text[1] > 0) anh_text[1]--;
               // g.DrawRectangle(Pens.Red, anh_text[0], anh_text[1], anh_text[2], anh_text[3]);
                g.Dispose();
                text_goc = Crop(text_goc, new Rectangle(anh_text[0], anh_text[1], anh_text[2] + 2, anh_text[3] + 2));

                Color aa;
                LockBitmap lockBitmap = new LockBitmap(text_goc);
                lockBitmap.LockBits();
                int[,] du = new int[text_goc.Width, text_goc.Height];
                for (int x = 0; x < text_goc.Width; x++)
                {
                    for (int y = 0; y < text_goc.Height; y++)
                    {
                        aa = lockBitmap.GetPixel(x, y);
                        if (aa.R > 10)
                        {
                            du[x, y] = BitConverter.ToInt32(new byte[4] { 255, 255, 0, 0 }, 0);
                        }
                        else du[x, y] = 0;

                    }
                }
                 lockBitmap.UnlockBits();

                float tile = text_goc.Width / text_goc.Height;

                int hh0 = 0;
                int ww0 = 0;
                int to = 0;

                if (com_hu.SelectedIndex == 0 || com_hu.SelectedIndex == 1 || com_hu.SelectedIndex == 2)
                {
                    hh0 = text_goc.Height;
                    ww0 = (int)(hh0 * tile);
                    to = (int)(text_goc.Width + ww0);
                }
                else if (com_hu.SelectedIndex == 3)
                {
                    hh0 = text_goc.Height;
                    ww0 = (int)(hh0 * tile);
                    to = (int)(30 + ww0);
                }
                else if (com_hu.SelectedIndex == 4 || com_hu.SelectedIndex == 5)
                {
                    hh0 = text_goc.Height;
                    ww0 = (int)(hh0 * tile);
                    to = (int)(30 + hh0);
                }
                else if (com_hu.SelectedIndex == 6 || com_hu.SelectedIndex == 7)
                {
                    hh0 = text_goc.Height;
                    ww0 = (int)(hh0 * tile);
                    to = (int)(30 + hh0 + text_goc.Width);
                }



                byte[] temp_text = new byte[12 + hh0 * ww0 * to * 4 +328];
                int dem = 12+328;

                Byte[] a1 = BitConverter.GetBytes(to);
                Byte[] a2 = BitConverter.GetBytes(ww0);
                Byte[] a3 = BitConverter.GetBytes(hh0);

                temp_text[0 + 328] = a1[0];
                temp_text[1 + 328] = a1[1];
                temp_text[2 + 328] = a1[2];
                temp_text[3 + 328] = a1[3];
                temp_text[4 + 328] = a2[0];
                temp_text[5 + 328] = a2[1];
                temp_text[6 + 328] = a2[2];
                temp_text[7 + 328] = a2[3];
                temp_text[8 + 328] = a3[0];
                temp_text[9 + 328] = a3[1];
                temp_text[10 + 328] = a3[2];
                temp_text[11 + 328] = a3[3];


                if (com_hu.SelectedIndex == 0)
                {
                    for (int i = -ww0; i < to - ww0; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x + i < text_goc.Width && x + i >= 0)
                                {
                                    Byte[] bytes2 = BitConverter.GetBytes(du[x + i, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];



                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                }
                else if (com_hu.SelectedIndex == 1)
                {
                    for (int i = to - ww0 - 1; i >= -ww0; i--)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x + i < text_goc.Width && x + i >= 0)
                                {

                                    Byte[] bytes2 = BitConverter.GetBytes(du[x + i, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                }
                else if (com_hu.SelectedIndex == 2)
                {
                    for (int i = 0; i < to; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0)
                                {
                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;
                            }
                        }

                    }
                }
                else if (com_hu.SelectedIndex == 3)
                {
                    for (int i = -ww0; i < 0; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x + i < text_goc.Width && x + i >= 0)
                                {


                                    Byte[] bytes2 = BitConverter.GetBytes(du[x + i, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                    for (int i = 0; i < 30; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0)
                                {
                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                }
                else if (com_hu.SelectedIndex == 4)
                {
                    for (int i = -hh0; i < 0; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0 && i + y >= 0 && i + y < text_goc.Height)
                                {


                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y + i]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;


                            }
                        }

                    }
                    for (int i = 0; i < 30; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0)
                                {

                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                }
                else if (com_hu.SelectedIndex == 5)
                {
                    for (int i = -hh0; i < 0; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0 && y - i >= 0 && y - i < text_goc.Height)
                                {

                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y - 1]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];

                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                    for (int i = 0; i < 30; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0)
                                {
                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                }
                else if (com_hu.SelectedIndex == 6)
                {
                    for (int i = -hh0; i < 0; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0 && i + y >= 0 && i + y < text_goc.Height)
                                {

                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y + i]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];

                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                    for (int i = 0; i < 30; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0)
                                {
                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4; ;

                            }
                        }

                    }
                    for (int i = 0; i < text_goc.Width; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x + i < text_goc.Width && x + i >= 0)
                                {

                                    Byte[] bytes2 = BitConverter.GetBytes(du[x + i, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];

                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4; ;

                            }
                        }

                    }
                }
                else if (com_hu.SelectedIndex == 7)
                {
                    for (int i = -hh0; i < 0; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0 && y - i >= 0 && y - i < text_goc.Height)
                                {

                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y - i]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];

                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                    for (int i = 0; i < 30; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x < text_goc.Width && x >= 0)
                                {
                                    Byte[] bytes2 = BitConverter.GetBytes(du[x, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];
                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                    for (int i = 0; i < text_goc.Width; i++)
                    {
                        for (int x = 0; x < ww0; x++)
                        {
                            for (int y = 0; y < hh0; y++)
                            {
                                if (x + i < text_goc.Width && x + i >= 0)
                                {

                                    Byte[] bytes2 = BitConverter.GetBytes(du[x + i, y]);
                                    temp_text[dem] = bytes2[0];
                                    temp_text[dem + 1] = bytes2[1];
                                    temp_text[dem + 2] = bytes2[2];
                                    temp_text[dem + 3] = bytes2[3];

                                }
                                else
                                {
                                    temp_text[dem] = 0;
                                    temp_text[dem + 1] = 0;
                                    temp_text[dem + 2] = 0;
                                    temp_text[dem + 3] = 0;
                                }
                                dem = dem + 4;

                            }
                        }

                    }
                }


                
                    mau_hue = 0;
                 
                     
                        thongtin_effect_goc = new THONGTIN_EFFECT_GOC(temp_text, to, to, 0, new SizeF(ww0, hh0), 0, false, false, false, false, false, 0, false, 0);
                  
               
          

              //  pictureBox20.Image = text_goc;
            }
            else
            {
                text_goc = null;
                thongtin_effect_goc = null;
            }

            panel_wail.Visible = false;
            get_tong();
            up_setvung();
            Application.DoEvents();
            timer_map.Enabled = true;
            timer.Enabled = true;
            //  MessageBox.Show(dd.PixelFormat.ToString());
            //  text_kt[0] = 0;
            //text_kt[1] = 0;



        }
        private void text_chu_TextChanged(object sender, EventArgs e)
        {
            getchu();
        }

        private void fontListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getchu();
        }

        private void num_size_font_ValueChanged(object sender, EventArgs e)
        {
            getchu();
        }

        private void com_style_font_SelectedIndexChanged(object sender, EventArgs e)
        {
            getchu();
        }

        private void com_font_render_SelectedIndexChanged(object sender, EventArgs e)
        {
            getchu();
        }

        private void combo_doc_SelectedIndexChanged(object sender, EventArgs e)
        {
            getchu();
        }

        private void pic_map_MouseEnter(object sender, EventArgs e)
        {
            pic_map.Focus();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.GOC++;


                if (thongtin_effect_goc.GOC >= 4) thongtin_effect_goc.GOC = 0;
            }
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.DAO == false) thongtin_effect_goc.DAO = true;
                else thongtin_effect_goc.DAO = false;
            }
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.NGANG == false) thongtin_effect_goc.NGANG = true;
                else thongtin_effect_goc.NGANG = false;
            }
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.DOC == false) thongtin_effect_goc.DOC = true;
                else thongtin_effect_goc.DOC = false;
            }
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.MAU == false) thongtin_effect_goc.MAU = true;
                else thongtin_effect_goc.MAU = false;
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.TRON == false) thongtin_effect_goc.TRON = true;
                else thongtin_effect_goc.TRON = false;
            }
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.CODAN == false) thongtin_effect_goc.CODAN = true;
                else thongtin_effect_goc.CODAN = false;
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            set_vung();
        }

        private void pictureBox16_MouseDown(object sender, MouseEventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.DMAU++;
                if (thongtin_effect_goc.DMAU >= 41) thongtin_effect_goc.DMAU = 0;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
                progressBar2.Value = 0;
                progressBar2.Maximum = 11;
                List<int[]> dulieu = new List<int[]>();
                int hhh = 0;
                int cu = 0;
                for (int x = 0; x < thongtin_effect_goc.TONG; x++)
                {
                    int[] dd = get_data_effect(x);

                    dulieu.Add(dd);
                    hhh = (int)(chuyendoiF(x, thongtin_effect_goc.TONG, 10));
                    if (cu != hhh)
                    {
                        progressBar2.Value = hhh;
                        cu = hhh;
                        Application.DoEvents();
                    }

                    // 
                }

                list_hieuung[vt_chon_vung].HIEUUNG.Add(new LAYER_HIEUUNG(thongtin_effect_goc.BATDAU, thongtin_effect_goc.TONG, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.CODAN, dulieu));
                get_tong();
                up_vt_keo(vt_chon_vung);
                thongtin_effect_goc = null;

                set_vung();


                panel_wail.Visible = false;
                timer_map.Enabled = true;
                timer.Enabled = true;
                Application.DoEvents();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            vt_menu = menu_chinh;

            vt_chon_vung = -1;

            set_vung();
            int jj = get_tong_vung(vt_chon_vung);
            if (dem_play <= jj) dem_play = jj;
        }
        List<List<int>> list_vien_wire = new List<List<int>>();

        private void button14_Click_1(object sender, EventArgs e)
        {
            if (play_thucong == true)
            {
                dem_play_thucong = 0;
                play_thucong = false;
                button27.Text = name_wire_vien_5_0[menu_ngonngu];
            }
            list_vien_wire.Add(new List<int>());
           
            
        if(vt_menu==menu_vien_wire)  list_vien_wire_ten.Items.Add(name_wire[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
            else if (vt_menu == menu_thucong_wire) list_vien_wire_ten.Items.Add(name_wire2[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
            else if (vt_menu == menu_vay_wire) list_vien_wire_ten.Items.Add(name_wire2[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
            else if (vt_menu == menu_haoquang_wire) list_vien_wire_ten.Items.Add(name_wire1[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());


            list_vien_wire_ten.SelectedIndex = list_vien_wire_ten.Items.Count - 1;

        }

        private void list_vien_wire_ten_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (play_thucong == true)
            {
                dem_play_thucong = 0;
                play_thucong = false;
                button27.Text = "Play";
            }
            if (list_vien_wire.Count>0 && list_vien_wire_ten.SelectedIndex>0 && list_vien_wire_ten.SelectedIndex< list_vien_wire.Count )
            {
                //   day_select = list_vien_wire[list_vien_wire_ten.SelectedIndex];
                toolStripMenuItem1.Enabled = true;
            }
            else
            {
                toolStripMenuItem1.Enabled = false ;
                // day_select.Clear();
            }
           
        }

        private void button17_Click(object sender, EventArgs e)
        {

            if (list_vien_wire_ten.SelectedIndex >= 0 && list_vien_wire_ten.SelectedIndex < list_vien_wire.Count)
            {
                if (play_thucong == true)
                {
                    dem_play_thucong = 0;
                    play_thucong = false;
                    button27.Text = "Play";
                }
                List<int> temp = new List<int>();

                for (int x = 0; x < led_full.Count; x++) temp.Add(x);
                list_vien_wire[list_vien_wire_ten.SelectedIndex] = temp;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (list_vien_wire_ten.SelectedIndex >= 0 && list_vien_wire_ten.SelectedIndex < list_vien_wire.Count)
            {

                if (play_thucong == true)
                {
                    dem_play_thucong = 0;
                    play_thucong = false;
                    button27.Text = "Play";
                }

                list_vien_wire[list_vien_wire_ten.SelectedIndex].Clear();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            int ttt = 0;

            for (int x = 0; x < list_vien_wire.Count; x++)
            {
                ttt += list_vien_wire[x].Count;
                

            }
            if (ttt <= 0)
            {
                string tennn = "Không có sơ đồ, bạn muốn thoát vẽ lại?";
                string ten = thongbao.ShowBox(tennn, menu_ngonngu);
                if (ten != "")
                {

                }
                else
                {
                   if(vt_chon_vung>=0 && vt_chon_vung< list_hieuung.Count) list_hieuung.RemoveAt(vt_chon_vung);
                    vt_menu = menu_chinh;

                    vt_chon_vung = -1;

                    set_vung();
                    int jj = get_tong_vung(vt_chon_vung);
                    if (dem_play <= jj) dem_play = jj;
                }
            }
            else
            {

                if (vt_menu == menu_vien_wire)
                {

                    List<List<int>> temp = new List<List<int>>();
                    for (int x = 0; x < list_vien_wire.Count; x++)
                    {
                        List<int> temp0 = new List<int>();
                        for (int y = 0; y < list_vien_wire[x].Count; y++)
                        {
                            temp0.Add(list_vien_wire[x][y]);

                        }
                        temp.Add(temp0);

                    }
                    list_hieuung[vt_chon_vung].WIREV = temp;
                }
                if (vt_menu == menu_vay_wire)
                {

                    List<List<int>> temp = new List<List<int>>();
                    for (int x = 0; x < list_vien_wire.Count; x++)
                    {
                        List<int> temp0 = new List<int>();
                        for (int y = 0; y < list_vien_wire[x].Count; y++)
                        {
                            temp0.Add(list_vien_wire[x][y]);

                        }
                        temp.Add(temp0);

                    }
                    list_hieuung[vt_chon_vung].WIREV = temp;
                }
                if (vt_menu == menu_thucong_wire)
                {

                    List<List<int>> temp = new List<List<int>>();
                    for (int x = 0; x < list_vien_wire.Count; x++)
                    {
                        List<int> temp0 = new List<int>();
                        for (int y = 0; y < list_vien_wire[x].Count; y++)
                        {
                            temp0.Add(list_vien_wire[x][y]);

                        }
                        temp.Add(temp0);

                    }
                    list_hieuung[vt_chon_vung].WIREV = temp;
                }
                if (vt_menu == menu_haoquang_wire)
                {

                    List<List<int>> temp = new List<List<int>>();
                    for (int x = 0; x < list_vien_wire.Count; x++)
                    {
                        List<int> temp0 = new List<int>();
                        for (int y = 0; y < list_vien_wire[x].Count; y++)
                        {
                            temp0.Add(list_vien_wire[x][y]);

                        }
                        temp.Add(temp0);

                    }
                    list_hieuung[vt_chon_vung].WIREV = temp;
                }

                list_vien_wire.Clear();
                list_vien_wire_ten.Items.Clear();
                if (play_thucong == true)
                {
                    dem_play_thucong = 0;
                    play_thucong = false;
                    button27.Text = "Play";
                }
                panel_vien0.Visible = false;
                set_vung();
            }
        }

        private void pic_hu_vien_Paint(object sender, PaintEventArgs e)
        {
            if (vt_chon_vien >= 0)
            {
                int xx = vt_chon_vien - vt_hien_vien * max_rong_vien;
                if (xx >= 0 && xx < max_rong_vien * max_cao_vien)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 128, 128)), (xx % max_rong_vien) * 86, (xx / max_rong_vien) * 75, 88, 76);
            }



            using (Font font1 = new Font("Microsoft Sans Serif", 8, FontStyle.Regular, GraphicsUnit.Point))
            {
                for (int x = 0; x < max_cao_vien; x++)
                {

                    for (int y = 0; y < max_rong_vien; y++)
                    {

                        if (x * max_rong_vien + y + vt_hien_vien * max_rong_vien < list_vien.Count)
                        {

                            string link = list_vien[x * max_rong_vien + y + vt_hien_vien * max_rong_vien];

                            if (File.Exists(path_vien + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp"))
                            {

                                FileStream fs = new System.IO.FileStream(path_vien + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp", FileMode.Open, FileAccess.Read);
                                anh_doc = (Bitmap)Image.FromStream(fs);

                                e.Graphics.DrawImage(anh_doc, new Rectangle(2 + 86 * y, 2 + 75 * x, anh_doc.Width, anh_doc.Height), new Rectangle(0, 0, anh_doc.Width, anh_doc.Height), GraphicsUnit.Pixel);

                                fs.Close();

                            }






                            // Bitmap aaaa = (Bitmap)byteArrayToImage(temp1.ANH);
                            // e.Graphics.DrawImage(aaaa, new Rectangle(1 + 164 * y, 19 + 124 * x, aaaa.Width, aaaa.Height), new Rectangle(0, 0, aaaa.Width, aaaa.Height), GraphicsUnit.Pixel);

                            e.Graphics.DrawRectangle(Pens.Gray, 2 + 86 * y, 2 + 75 * x, 82, 60);

                            RectangleF rectF1 = new RectangleF(4 + 86 * y, 2 + 75 * x + 60, 82, 20);
                            string hhhh = Path.GetFileNameWithoutExtension(link);
                            string ff;
                            if (hhhh.Length < 16) ff = hhhh;
                            else ff = hhhh.Substring(0, 16);

                            e.Graphics.DrawString(ff, font1, Brushes.White, rectF1);












                        }
                    }
                }
            }
        }

        
       
        private void pic_hu_vien_MouseDown(object sender, MouseEventArgs e)
        {

            int xx = (e.X - 2) / 86;
            int yy = (e.Y - 2) / 75;


            vt_chon_vien = yy * max_rong_vien + xx + vt_hien_vien * max_rong_vien;

            if (vt_chon_vien >= list_vien.Count) vt_chon_vien = -1;
            pic_hu_vien.Refresh();
            if (vt_chon_vien >= 0 && vt_chon_vien < list_vien.Count)
            {
                toolStripMenuItem16.Enabled = true;
                mau_hue = 0;
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
                effect_nap = File.ReadAllBytes(list_vien[vt_chon_vien]);
                 effect_nap = Decrypt_moi(effect_nap);

                num_nen = effect_nap[0];
                num_vien = effect_nap[1];
                loai_chay_nen.SelectedIndex = effect_nap[2];
                loai_chay_vien.SelectedIndex = effect_nap[3];
               // num_vantoc_vien.Value = effect_nap[4];
                //num_lap_vien.Value = effect_nap[4];
                dem_vien = 0;
                dem_nen = 0;
                int dem = 8;
                //num_vien_speed.Value = 1;
                for (int v = 0; v < 32; v++) color_n[v] = Color.Transparent;
                for (int v = 0; v < 32; v++) color_v[v] = Color.Transparent;

                for (int v = 0; v < effect_nap[0]; v++)
                {
                    if (effect_nap[dem] == 0)
                    {
                        color_n[v] = Color.Transparent;
                    }
                    else
                    {
                        color_n[v] = Color.FromArgb(effect_nap[dem], effect_nap[dem + 1], effect_nap[dem + 2], effect_nap[dem + 3]);
                    }

                    dem = dem + 4;
                }
                for (int v = 0; v < effect_nap[1]; v++)
                {
                    if (effect_nap[dem] == 0)
                    {
                        color_v[v] = Color.Transparent;
                    }
                    else
                    {
                        color_v[v] = Color.FromArgb(effect_nap[dem], effect_nap[dem + 1], effect_nap[dem + 2], effect_nap[dem + 3]);
                    }

                    dem = dem + 4;
                }



                dem_vantoc_vien = 0;

                get_tong_vien();

                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien * (int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);
              
                panel_wail.Visible = false;
                get_tong();
                up_setvung();
                Application.DoEvents();
                timer_map.Enabled = true;
                timer.Enabled = true;
            }else
            {
                toolStripMenuItem16.Enabled = false;
                thongtin_effect_goc = null;
            }

       
        }
        int dem_vantoc_vien = 0;

       
        private void button15_Click(object sender, EventArgs e)
        {
           
        }

        private void pic_map_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox25_MouseDown(object sender, MouseEventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.DMAU++;
                if (thongtin_effect_goc.DMAU >= 41) thongtin_effect_goc.DMAU = 0;
            }
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.MAU == false) thongtin_effect_goc.MAU = true;
                else thongtin_effect_goc.MAU = false;
            }
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                if (thongtin_effect_goc.DAO == false) thongtin_effect_goc.DAO = true;
                else thongtin_effect_goc.DAO = false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            vt_menu = menu_chinh;

            vt_chon_vung = -1;

            set_vung();
            int jj = get_tong_vung(vt_chon_vung);
            if (dem_play <= jj) dem_play = jj;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
                progressBar2.Value = 0;
                progressBar2.Maximum = 11;
                List<int[]> dulieu = new List<int[]>();
                int hhh = 0;
                int cu = 0;
                if (vt_menu == menu_vay_edit)
                {
                    for (int x = 0; x < thongtin_effect_goc.HIENTAI; x++)
                    {
                        int[] dd = get_data_vay(x);

                        dulieu.Add(dd);
                        hhh = (int)(chuyendoiF(x, thongtin_effect_goc.HIENTAI, 10));
                        if (cu != hhh)
                        {
                            progressBar2.Value = hhh;
                            cu = hhh;
                            Application.DoEvents();
                        }

                        // 
                    }
                }else
                {
                    for (int x = 0; x < thongtin_effect_goc.HIENTAI; x++)
                    {
                        int[] dd = get_data_vien(x);

                        dulieu.Add(dd);
                        hhh = (int)(chuyendoiF(x, thongtin_effect_goc.HIENTAI, 10));
                        if (cu != hhh)
                        {
                            progressBar2.Value = hhh;
                            cu = hhh;
                            Application.DoEvents();
                        }

                        // 
                    }
                }

                list_hieuung[vt_chon_vung].HIEUUNG.Add(new LAYER_HIEUUNG(thongtin_effect_goc.BATDAU, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.CODAN, dulieu));
                get_tong();
                up_vt_keo(vt_chon_vung);
                thongtin_effect_goc = null;

                set_vung();


                panel_wail.Visible = false;
                timer_map.Enabled = true;
                timer.Enabled = true;
                Application.DoEvents();
            }
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            panel_vien1.Visible = true;
            if(vt_chon_vien<0)
            {
                mau_hue = 0;
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
               

                num_nen =1;
                num_vien =8;
                loai_chay_nen.SelectedIndex =0;
                loai_chay_vien.SelectedIndex = 1;
                // num_vantoc_vien.Value = effect_nap[4];
                //num_lap_vien.Value = effect_nap[4];
                dem_vien = 0;
                dem_nen = 0;
               
               // num_vien_speed.Value = 1;
                for (int v = 0; v < 32; v++) color_n[v] = Color.Transparent;
                for (int v = 0; v < 32; v++) color_v[v] = Color.Transparent;
                for (int v = 0; v < 3; v++) color_v[v]= Color.Red;




                dem_vantoc_vien = 0;

                get_tong_vien();

                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien*(int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);
        
                panel_wail.Visible = false;
                get_tong();
                Application.DoEvents();
                timer_map.Enabled = true;
                timer.Enabled = true;
            }
        }

        private void pic_vien_vien_Paint(object sender, PaintEventArgs e)
        {
            for (int x = 0; x < num_nen; x++)
            {
                if (x >= 0 && x < num_nen)
                {
                    

                        if (color_n[x] == Color.Transparent)
                        {
                            DrawRoundedRectangle(e.Graphics, new Rectangle(x * 12 + 1, 1, 9, 18), 3, new Pen(Color.FromArgb(100, 255, 255, 255), 1), Color.Black);
                            DrawRoundedRectangle(e.Graphics, new Rectangle(x * 12 + 3, 3, 5, 14), 3, new Pen(Color.FromArgb(100, 255, 255, 255), 1), Color.FromArgb(32, 32, 32));
                        }
                        else
                        {
                            DrawRoundedRectangle(e.Graphics, new Rectangle(x * 12 + 1, 1, 9, 18), 3, new Pen(Color.FromArgb(100, 255, 255, 255), 1), color_n[x]);

                        }
                   


                }

            }
        }
        private void DrawRoundedRectangle(Graphics gfx, Rectangle Bounds, int CornerRadius, Pen DrawPen, Color FillColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(DrawPen.Width));
            Bounds = Rectangle.Inflate(Bounds, -strokeOffset, -strokeOffset);

            DrawPen.EndCap = DrawPen.StartCap = LineCap.Round;

            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(Bounds.X, Bounds.Y, CornerRadius, CornerRadius, 180, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width, Bounds.Y, CornerRadius, CornerRadius, 270, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
            gfxPath.AddArc(Bounds.X, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
            gfxPath.CloseAllFigures();

            gfx.FillPath(new SolidBrush(FillColor), gfxPath);
            gfx.DrawPath(DrawPen, gfxPath);
        }

        private void pic_vien_nen_Paint(object sender, PaintEventArgs e)
        {
            for (int x = 0; x < num_vien; x++)
            {
                if (x >= 0 && x < num_vien)
                {
                   

                        if (color_v[x] == Color.Transparent)
                        {
                            DrawRoundedRectangle(e.Graphics, new Rectangle(x * 12 + 1, 1, 9, 18), 3, new Pen(Color.FromArgb(100, 255, 255, 255), 1), Color.Black);
                            DrawRoundedRectangle(e.Graphics, new Rectangle(x * 12 + 3, 3, 5, 14), 3, new Pen(Color.FromArgb(100, 255, 255, 255), 1), Color.FromArgb(32, 32, 32));
                        }
                        else
                        {
                            DrawRoundedRectangle(e.Graphics, new Rectangle(x * 12 + 1, 1, 9, 18), 3, new Pen(Color.FromArgb(100, 255, 255, 255), 1), color_v[x]);

                        }
                   
                }

            }
        }
        int mau_vtx = 6;
        int mau_vty = 13;
        private void pic_vien_mau_Paint(object sender, PaintEventArgs e)
        {
            if (nhay == true)
            {
                if (mau_vtx == 0 && mau_vty == 0)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 1, 1, 8, 68);
                }
                else if (mau_vtx == 0 && mau_vty == 1)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 1, 70, 8, 68);
                }
                else
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), mau_vtx * 10 +1, mau_vty * 10 + 1, 8, 8);
                }
            }
            else
            {
                if (mau_vtx == 0 && mau_vty == 0)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 2), 1, 1, 8, 68);
                }
                else if (mau_vtx == 0 && mau_vty == 1)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 2), 1, 70, 8, 68);
                }
                else
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 2), mau_vtx * 10 + 1, mau_vty * 10 + 1, 8, 8);
                }

 
            }
        }
        Color mauvien_chon = Color.White;
        private void pic_vien_mau_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Location.X < 10)
            {
                if (e.Y > 70)
                {
                    mauvien_chon = Color.Transparent;
                    mau_vtx = 0;
                    mau_vty = 1;
                }
                else
                {
                    mauvien_chon = Color.Black;
                    mau_vtx = 0;
                    mau_vty = 0;
                }


                
            }
            else
            {
                mau_vtx = (e.Location.X - 2) / 10;
                mau_vty = (e.Location.Y - 2) / 10;
                Color m = ((Bitmap)pic_vien_mau.Image).GetPixel(mau_vtx * 10 + 4, mau_vty * 10 + 4);
                mauvien_chon = m;
                // mau_vt = BitConverter.ToInt32(new byte[4] { 255, m.R, m.G, m.B }, 0);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            for (int aa = 0; aa < 32; aa++) color_n[aa] = Color.Transparent;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            for (int aa = 0; aa < 32; aa++) color_v[aa] = Color.Transparent;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            for (int aa = 0; aa < 32; aa++) color_n[aa] = mauvien_chon;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            for (int aa = 0; aa < 32; aa++) color_v[aa] = mauvien_chon;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (num_nen > 1)
            {
                
                dem_nen = 0;
                num_nen--;

                get_tong_vien();

                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien * (int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);
               
                get_tong();

            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (num_vien > 1)
            {
                
                dem_vien = 0;
                num_vien--;
               
                 get_tong_vien();

                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien * (int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);

                get_tong();
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (num_nen < 32)
            {
              
                dem_nen = 0;
                num_nen++;
                 get_tong_vien();

                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien * (int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);

                get_tong();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (num_vien < 32)
            {
                
                dem_vien = 0;
                num_vien++;
                 get_tong_vien();

                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien * (int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);

                get_tong();

            }
        }

        private void pic_vien_vien_MouseDown(object sender, MouseEventArgs e)
        {
            int vt = (e.X - 1) / 12;
            if (vt >= 0 && vt < num_nen)
            {

                color_n[vt] = mauvien_chon;
                
            }
        }

        private void pic_vien_nen_MouseDown(object sender, MouseEventArgs e)
        {
            int vt = (e.X - 1) / 12;

            if (vt >= 0 && vt < num_vien)
            {


                color_v[vt] = mauvien_chon;
               
            }

        }

        private void loai_chay_nen_SelectedIndexChanged(object sender, EventArgs e)
        {
             get_tong_vien();

            thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien * (int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);

            get_tong();
        }

        private void loai_chay_vien_SelectedIndexChanged(object sender, EventArgs e)
        {
             get_tong_vien();

            thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, tong_chay_vien, tong_chay_vien * (int)num_vien_speed.Value, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);

            get_tong();
        }

        private void button26_Click(object sender, EventArgs e)
        {

            int[] temp = get_vien(color_n, tong_chay_nen /3, num_nen, tong_chay_nen, loai_chay_nen.SelectedIndex);
            int[] temp1 = get_vien(color_v, tong_chay_vien / 3, num_vien, tong_chay_vien, loai_chay_vien.SelectedIndex);

            
                Bitmap aa = new Bitmap(80, 60);
                Graphics g1 = Graphics.FromImage(aa);
                g1.Clear(Color.Black);
                List<Point> dddd = new List<Point>();
                for (int x = 0; x < 40; x++) dddd.Add(new Point(x * 2, 0));
                for (int x = 0; x < 32; x++) dddd.Add(new Point(78, x * 2));
                for (int x = 39; x >= 0; x--) dddd.Add(new Point(x * 2, 58));
                for (int x = 31; x >= 0; x--) dddd.Add(new Point(0, x * 2));
                for (int x = 0; x < dddd.Count; x++)
                {
                    g1.FillRectangle(new SolidBrush(travemau(temp[x % (int)num_nen])), dddd[x].X, dddd[x].Y, 2, 2);

                }
                for (int x = 0; x < dddd.Count; x++)
                {
                    if (temp1[x % (int)num_vien] != 0) g1.FillRectangle(new SolidBrush(travemau(temp1[x % (int)num_vien])), dddd[x].X, dddd[x].Y, 2, 2);

                }

                string teen = save_vien.ShowBox(aa, 0);
            if (teen != "")
            {

                byte[] data = new byte[num_nen * 4 + num_vien * 4 + 8];

                data[0] = (byte)num_nen;
                data[1] = (byte)num_vien;
                data[2] = (byte)loai_chay_nen.SelectedIndex;
                data[3] = (byte)loai_chay_vien.SelectedIndex;
                data[4] = 1;
                data[5] = 1;
                int dem = 8;
                for (int x = 0; x < num_nen; x++)
                {

                    data[dem] = color_n[x].A; dem++;
                    data[dem] = color_n[x].R; dem++;
                    data[dem] = color_n[x].G; dem++;
                    data[dem] = color_n[x].B; dem++;
                }
                for (int x = 0; x < num_vien; x++)
                {
                    data[dem] = color_v[x].A; dem++;
                    data[dem] = color_v[x].R; dem++;
                    data[dem] = color_v[x].G; dem++;
                    data[dem] = color_v[x].B; dem++;
                }
                data = Encrypt_moi(data);
                File.WriteAllBytes(path_vien + teen, data);
                hien_list_vien();
               
            }
          

 
                // il.Images.Add(Path.GetFileNameWithoutExtension(link), aa);
            

         
            Application.DoEvents();


            panel_vien1.Visible = false;
        }

        private void pictureBox22_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            if (vt_chon_vien >= 0 && vt_chon_vien < list_vien.Count)
            {
                timer.Enabled = false;
                timer_map.Enabled = false;
                if (File.Exists(list_vien[vt_chon_vien]) == true)
                {
                    File.Delete(list_vien[vt_chon_vien]);
                    hien_list_vien();
                }

                timer.Enabled = true;
                timer_map.Enabled = true;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (play_thucong == true)
            {
                dem_play_thucong = 0;
                play_thucong = false;
                button27.Text = name_wire_vien_5_0[menu_ngonngu];
            }
            if (list_vien_wire.Count<=0)
            {
                list_vien_wire.Add(new List<int>());
                if (vt_menu == menu_vien_wire) list_vien_wire_ten.Items.Add(name_wire[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                else if (vt_menu == menu_thucong_wire) list_vien_wire_ten.Items.Add(name_wire2[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                else if (vt_menu == menu_vay_wire) list_vien_wire_ten.Items.Add(name_wire2[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                else if (vt_menu == menu_haoquang_wire) list_vien_wire_ten.Items.Add(name_wire1[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                list_vien_wire_ten.SelectedIndex = list_vien_wire_ten.Items.Count - 1;
            }else
            {
                List<int> tem = new List<int>();
                for (int x = 0; x < list_vien_wire[list_vien_wire.Count - 1].Count; x++) tem.Add(list_vien_wire[list_vien_wire.Count - 1][x]);

                    list_vien_wire.Add(tem);
                if (vt_menu == menu_vien_wire) list_vien_wire_ten.Items.Add(name_wire[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                else if (vt_menu == menu_thucong_wire) list_vien_wire_ten.Items.Add(name_wire2[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                else if (vt_menu == menu_vay_wire) list_vien_wire_ten.Items.Add(name_wire2[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                else if (vt_menu == menu_haoquang_wire) list_vien_wire_ten.Items.Add(name_wire1[menu_ngonngu] + list_vien_wire_ten.Items.Count.ToString());
                list_vien_wire_ten.SelectedIndex = list_vien_wire_ten.Items.Count - 1;
            }

            
        }

        private void button27_Click(object sender, EventArgs e)
        {
            dem_play_thucong = 0;
            if (play_thucong == true)
            {
                play_thucong = false;
              
                button27.Text = name_wire_vien_5_0[menu_ngonngu];
            }
            else
            {
                play_thucong = true;
                button27.Text = name_wire_vien_5_1[menu_ngonngu];
            }
        }
        private void set_play(bool play)
        {
            dem_play  = 0;
            if (play == false)
            {
                play_chinh = false;
                button32.Text = name_menu2[menu_ngonngu];
            }
            else
            {
                play_chinh = true;
                button32.Text = name_menu2_1[menu_ngonngu];
            }
        }
        bool play_haoquang = false;
        int dem_play_haoquang = 0;
        private void set_play_haoquang(bool play)
        {
            dem_play_haoquang = 0;
            if (play == false)
            {
                play_haoquang = false;
                button39.Text = name_halo_5_0[menu_ngonngu];
            }
            else
            {
                halo_color_play.Clear();

                for (int i = 0; i < halo_color.Count; i++)
                {

                    halo_color_play.Add(halo_color[i]);
                }


                play_haoquang = true;
                button39.Text = name_halo_5_1[menu_ngonngu];
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox20_MouseDown(object sender, MouseEventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.DMAU++;
                if (thongtin_effect_goc.DMAU >= 41) thongtin_effect_goc.DMAU = 0;
            }
        }

        private void speed_thucong_ValueChanged(object sender, EventArgs e)
        {
          //  if (thongtin_effect_goc != null) thongtin_effect_goc.HIENTAI = (int)speed_thucong.Value * tong_chay_thucong;
        }

        private void num_vien_speed_ValueChanged(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.HIENTAI = (int)num_vien_speed.Value * thongtin_effect_goc.TONG;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (list_vien_wire.Count > 0 && list_vien_wire_ten.SelectedIndex > 0 && list_vien_wire_ten.SelectedIndex < list_vien_wire.Count)
            {
                timer.Enabled = false;
                timer_map.Enabled = false;
                dem_play_thucong = 0;
             
                    play_thucong = false;
                    button27.Text = "Play";

                    list_vien_wire.RemoveAt(list_vien_wire_ten.SelectedIndex);
                list_vien_wire_ten.Items.RemoveAt(list_vien_wire_ten.SelectedIndex);
                list_vien_wire_ten.SelectedIndex = 0;
                timer.Enabled = true;
                timer_map.Enabled = true;

            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (vt_chon_effect >= 0 && vt_chon_effect < list_effect.Count)
            {
              
                timer_map.Enabled = false;
                timer.Enabled = false;
                thongtin_effect_goc = null;
                string ff = list_path_effect[com_path_effect.SelectedIndex] + "\\" + Path.GetFileNameWithoutExtension(list_effect[vt_chon_effect]) + ".bmp" ;

              
                if (File.Exists(ff) == true)
                {
                    File.Delete(ff);
                }

                try
                {
                    if (File.Exists(list_effect[vt_chon_effect]) == true)
                    {
                        File.Delete(list_effect[vt_chon_effect]);
                    }


                }
                catch
                {

                }
                hien_list_effect();
                timer_map.Enabled = true;
                timer.Enabled = true;
            }

        }

        private void pic_hu_halo_Paint(object sender, PaintEventArgs e)
        {
            if (vt_chon_halo >= 0)
            {
                int xx = vt_chon_halo - vt_hien_halo * max_rong_halo;
                if (xx >= 0 && xx < max_rong_halo * max_cao_halo)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 128, 128)), (xx % max_rong_halo) * 86, (xx / max_rong_halo) * 75, 88, 76);
            }



            using (Font font1 = new Font("Microsoft Sans Serif", 8, FontStyle.Regular, GraphicsUnit.Point))
            {
                for (int x = 0; x < max_cao_halo; x++)
                {

                    for (int y = 0; y < max_rong_halo; y++)
                    {

                        if (x * max_rong_halo + y + vt_hien_halo * max_rong_halo < list_halo.Count)
                        {

                            string link = list_halo[x * max_rong_halo + y + vt_hien_halo * max_rong_halo];

                            if (File.Exists(path_halo + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp"))
                            {

                                FileStream fs = new System.IO.FileStream(path_halo + "\\" + Path.GetFileNameWithoutExtension(link) + ".bmp", FileMode.Open, FileAccess.Read);
                                anh_doc = (Bitmap)Image.FromStream(fs);

                                e.Graphics.DrawImage(anh_doc, new Rectangle(2 + 86 * y, 2 + 75 * x, anh_doc.Width, anh_doc.Height), new Rectangle(0, 0, anh_doc.Width, anh_doc.Height), GraphicsUnit.Pixel);

                                fs.Close();

                            }






                            // Bitmap aaaa = (Bitmap)byteArrayToImage(temp1.ANH);
                            // e.Graphics.DrawImage(aaaa, new Rectangle(1 + 164 * y, 19 + 124 * x, aaaa.Width, aaaa.Height), new Rectangle(0, 0, aaaa.Width, aaaa.Height), GraphicsUnit.Pixel);

                            e.Graphics.DrawRectangle(Pens.Gray, 2 + 86 * y, 2 + 75 * x, 82, 60);

                            RectangleF rectF1 = new RectangleF(4 + 86 * y, 2 + 75 * x + 60, 82, 20);
                            string hhhh = Path.GetFileNameWithoutExtension(link);
                            string ff;
                            if (hhhh.Length < 16) ff = hhhh;
                            else ff = hhhh.Substring(0, 16);

                            e.Graphics.DrawString(ff, font1, Brushes.White, rectF1);












                        }
                    }
                }
            }
        }

        



       

            private void pic_hu_halo_MouseDown(object sender, MouseEventArgs e)
        {
            int xx = (e.X - 2) / 86;
            int yy = (e.Y - 2) / 75;


            vt_chon_halo = yy * max_rong_halo + xx + vt_hien_halo * max_rong_halo;

            if (vt_chon_halo >= list_halo.Count) vt_chon_halo = -1;
            pic_hu_halo.Refresh();
            if (vt_chon_halo >= 0 && vt_chon_halo < list_halo.Count)
            {
                toolStripMenuItem3.Enabled = true;
                mau_hue = 0;
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
                list_color_haoquang.Clear();
                list_color_haoquang_play.Clear();
                list_set_haoquang.Clear();
                list_hieung_haoquang.Items.Clear();

                //path_huchon = path_hieuung_vien +  "\\" + listten_vien[vt_chon_tv_vien] + ".bor";




                var rmCrypto = GetAlgorithm();

                ICryptoTransform decryptor = rmCrypto.CreateDecryptor();
                try
                {
                    using (var reader = new StreamReader(new CryptoStream(System.IO.File.OpenRead(list_halo[vt_chon_halo]), decryptor, CryptoStreamMode.Read)))
                    {

                        HAOQUANG temp = JsonConvert.DeserializeObject<HAOQUANG>(reader.ReadToEnd());
                     

                        haoquang_tia = temp.TIA;
                        haoquang_hang = temp.HANG;

                        list_color_haoquang = temp.MAU;

                        for (int i = 0; i < list_color_haoquang.Count; i++)
                        {

                            list_color_haoquang_play.Add(list_color_haoquang[i]);
                        }

                            

                        list_set_haoquang = temp.SET;
                       // for (int i = 0; i < list_color_haoquang.Count; i++) list_hieung_haoquang.Items.Add(i.ToString());
                        button28.Enabled = true;
                    }
                }
                catch
                {


                }

 

                //if (list_hieung_haoquang.Items.Count > 0) list_hieung_haoquang.SelectedIndex = list_hieung_haoquang.Items.Count - 1;

                dem_vantoc_vien = 0;

                
                thongtin_effect_goc = new THONGTIN_EFFECT_GOC(effect_nap, haoquang_hang, haoquang_hang, 0, new SizeF(0, 0), 0, false, false, false, false, false, 0, false, 0);
               
             

                panel_wail.Visible = false;
                get_tong();
                up_setvung();
                Application.DoEvents();
                timer_map.Enabled = true;
                timer.Enabled = true;
            }
            else
               
            {
                button28.Enabled = false;
                list_color_haoquang.Clear();
                list_set_haoquang.Clear();
                list_hieung_haoquang.Items.Clear();
                toolStripMenuItem3.Enabled = false;
                thongtin_effect_goc = null;
            }
        }

        private void pictureBox22_MouseDown(object sender, MouseEventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                thongtin_effect_goc.DMAU++;
                if (thongtin_effect_goc.DMAU >= 41) thongtin_effect_goc.DMAU = 0;
            }
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc.MAU == false) thongtin_effect_goc.MAU = true;
            else thongtin_effect_goc.MAU = false;
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc.DAO == false) thongtin_effect_goc.DAO = true;
            else thongtin_effect_goc.DAO = false;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (thongtin_effect_goc != null)
            {
                timer_map.Enabled = false;
                timer.Enabled = false;
                panel_wail.Visible = true;
                Application.DoEvents();
                progressBar2.Value = 0;
                progressBar2.Maximum = 11;
                List<int[]> dulieu = new List<int[]>();
                int hhh = 0;
                int cu = 0;
                list_color_haoquang_play.Clear();
                for (int i = 0; i < list_color_haoquang.Count; i++)
                {
                    list_color_haoquang_play.Add(list_color_haoquang[i]);
                   
                }


                for (int x = 0; x < thongtin_effect_goc.HIENTAI; x++)
                {
                    int[] dd = get_data_haoquang(x);
                    dulieu.Add(dd);
                    hhh = (int)(chuyendoiF(x, thongtin_effect_goc.HIENTAI, 10));
                    if (cu != hhh)
                    {
                        progressBar2.Value = hhh;
                        cu = hhh;
                        Application.DoEvents();
                    }

                  
                }

                list_hieuung[vt_chon_vung].HIEUUNG.Add(new LAYER_HIEUUNG(thongtin_effect_goc.BATDAU, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.HIENTAI, thongtin_effect_goc.CODAN, dulieu));
                get_tong();
                up_vt_keo(vt_chon_vung);
                thongtin_effect_goc = null;

                set_vung();
                panel_wail.Visible = false;
                timer_map.Enabled = true;
                timer.Enabled = true;
                Application.DoEvents();
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            vt_menu = menu_chinh;

            vt_chon_vung = -1;

            set_vung();
            int jj = get_tong_vung(vt_chon_vung);
            if (dem_play <= jj) dem_play = jj;
        }

       

        private void button32_Click(object sender, EventArgs e)
        {
            if (play_chinh == false) set_play(true);
            else set_play(false);
        }

        private void button33_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            // saveFileDialog1.Filter = "Dự án LEDFULLmini (  *.full)|*.full|File nạp LEDFULLmini(  *.mini)|*.mini";
            saveFileDialog1.Filter = "File Ledfull (  *.led)|*.led";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<int> port = new List<int>();
                port.Add(map.PORT);
                int den = com_loaiden.SelectedIndex;
                if (den <= 0) den = 0;
                int rgb = ma_rgb.SelectedIndex;
                if (rgb <= 0) rgb = 0;

                SETTING set = new SETTING(den, rgb, port);
                FILE_SAVE save = new FILE_SAVE("ledfull_v1", list_hieuung, map, set);

                var rmCrypto = GetAlgorithm();
                ICryptoTransform encryptor = rmCrypto.CreateEncryptor();

                using (var writer = new StreamWriter(new CryptoStream(System.IO.File.Create(saveFileDialog1.FileName), encryptor, CryptoStreamMode.Write)))
                {
                    writer.Write(JsonConvert.SerializeObject(save));

                }
            }
            timer.Enabled = true;
            timer_map.Enabled = true;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
            open.Filter = "File Ledfull (  *.led)|*.led";
            if (open.ShowDialog() == DialogResult.OK)
            {
                load_file(open.FileName);
                

            }
          
            timer.Enabled = true;
            timer_map.Enabled = true;
        }
        private void load_file(string path)
        {

            timer.Enabled = false;
            timer_map.Enabled = false;
            
                var rmCrypto = GetAlgorithm();

                ICryptoTransform decryptor = rmCrypto.CreateDecryptor();
                try
                {
                    using (var reader = new StreamReader(new CryptoStream(System.IO.File.OpenRead(path), decryptor, CryptoStreamMode.Read)))
                    {

                        FILE_SAVE temp = JsonConvert.DeserializeObject<FILE_SAVE>(reader.ReadToEnd());
                        led_select.Clear();
                        list_hieuung = temp.HIEUUNG;
                        map = temp.MAP;
                        com_loaiden.SelectedIndex = temp.SET.DEN;
                        ma_rgb.SelectedIndex = temp.SET.RGB;
                        led_full = map.DATA;
                        vt_port2 = map.PORT;
                        list_anh.Clear();
                        for (int x = 0; x < map.ANH.Count; x++)
                        {
                            list_anh.Add(new ANH_ANH(byteArrayToImage(map.ANH[x].ANH), map.ANH[x].CENTER, map.ANH[x].SIZE, map.ANH[x].GOC, map.ANH[x].DIM));

                        }
                        mau_led = new Color[led_full.Count];
                        vt_chuot = new PointF(0, 0);
                        vt_menu = menu_chinh;
                        vt_chon_vung = -1;
                        set_vung();
                        int jj = get_tong_vung(vt_chon_vung);
                        if (dem_play <= jj) dem_play = jj;
                        reset_moitruong();
                    }
                }
                catch
                {

                }

           
            label3.Text = (vt_chon_vung + 1).ToString() + "/" + (list_hieuung.Count).ToString();
            timer.Enabled = true;
            timer_map.Enabled = true;

        }
            int halo_set_traiphai = 0;
        int halo_set_ravao = 0;
        int halo_set_doimau = 0;
        int halo_draw = 0;
        int halo_tia = 0;
        int halo_hang= 0;
        List<Color[,]> halo_color = new List<Color[,]>();
        List<Color[,]> halo_color_play = new List<Color[,]>();
        List<int[]> halo_set = new List<int[]>();


        private void pictureBox26_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            int x =(int)( e.Location.X / 47.5F);
            int y = (int)(e.Location.Y / 47.5F);
            if (x == 0 && y == 0)
            {
                if (halo_set_traiphai == 0|| halo_set_traiphai == 1) halo_set_traiphai = 2;
                else halo_set_traiphai = 0;
            }else if (x == 0 && y == 1)
            {
                if (halo_set_traiphai == 0 || halo_set_traiphai == 2) halo_set_traiphai = 1;
                else halo_set_traiphai = 0;
            }


            else if (x == 1 && y == 0)
            {
                if (halo_set_ravao == 0 || halo_set_ravao == 2) halo_set_ravao = 1;
                else halo_set_ravao = 0;
            }
            else if (x == 1 && y == 1)
            {
                if (halo_set_ravao == 0 || halo_set_ravao == 1) halo_set_ravao = 2;
                else halo_set_ravao = 0;
            }


            else if (x == 2 && y == 0)
            {
                if (halo_set_doimau == 0 || halo_set_doimau == 1 || halo_set_doimau == 3) halo_set_doimau = 2;
                else halo_set_doimau = 0;
            }
            else if (x == 2 && y == 1)
            {
                if (halo_set_doimau == 0 || halo_set_doimau == 1 || halo_set_doimau ==2) halo_set_doimau = 3;
                else halo_set_doimau = 0;
            }
            else if (x == 3 && y == 0)
            {
                if (halo_set_doimau == 0 || halo_set_doimau == 2 || halo_set_doimau == 3) halo_set_doimau = 1;
                else halo_set_doimau = 0;
            }


           if(list_hieung_haoquang.SelectedIndex>=0)
            {
                halo_set[list_hieung_haoquang.SelectedIndex][1] = halo_set_traiphai;
                halo_set[list_hieung_haoquang.SelectedIndex][0] = halo_set_ravao;
              
                
                halo_set[list_hieung_haoquang.SelectedIndex][2] = halo_set_doimau;
              
                
                set_play_haoquang(true);
            }
         */

        }

        private void pic_hu_haoquang_Paint(object sender, PaintEventArgs e)
        {
            /*
            if (nhay == true)
            {
                if (halo_set_traiphai == 2)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 1, 1, 45, 45);
                }
                else if (halo_set_traiphai == 1)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 1, 47, 45, 45);
                }

                if (halo_set_ravao == 1)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 47, 1, 45, 45);
                }
                else if (halo_set_ravao == 2)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 47, 47, 45, 45);
                }


                if (halo_set_doimau == 2)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 95, 1, 45, 45);
                }
                else if (halo_set_doimau == 3)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 95, 47, 45, 45);
                }
                else if (halo_set_doimau == 1)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 141, 1, 45, 45);
                }
            }
            */
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (nhay == true)
            {
                if (mau_vtx == 0 && mau_vty == 0)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 1, 1, 8, 68);
                }
                else if (mau_vtx == 0 && mau_vty == 1)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 1, 70, 8, 68);
                }
                else
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), mau_vtx * 10 + 1, mau_vty * 10 + 1, 8, 8);
                }
            }
            else
            {
                if (mau_vtx == 0 && mau_vty == 0)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 2), 1, 1, 8, 68);
                }
                else if (mau_vtx == 0 && mau_vty == 1)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 2), 1, 70, 8, 68);
                }
                else
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 2), mau_vtx * 10 + 1, mau_vty * 10 + 1, 8, 8);
                }


            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Location.X < 10)
            {
                if (e.Y > 70)
                {
                    mauvien_chon = Color.Transparent;
                    mau_vtx = 0;
                    mau_vty = 1;
                }
                else
                {
                    mauvien_chon = Color.Black;
                    mau_vtx = 0;
                    mau_vty = 0;
                }



            }
            else
            {
                mau_vtx = (e.Location.X - 2) / 10;
                mau_vty = (e.Location.Y - 2) / 10;
                Color m = ((Bitmap)pic_vien_mau.Image).GetPixel(mau_vtx * 10 + 4, mau_vty * 10 + 4);
                mauvien_chon = m;
                // mau_vt = BitConverter.ToInt32(new byte[4] { 255, m.R, m.G, m.B }, 0);
            }
        }

        private void pictureBox23_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (int)(e.Location.X / 31);
            int y = (int)(e.Location.Y / 35);
            halo_draw = x   + y*4;
            
            int vv = x + y * 4;
            if (vv >= 0 && vv < 8) toolTip1.SetToolTip(pictureBox23, name_halo_2[menu_ngonngu, vv] + "   ");
        }

        private void pictureBox23_Paint(object sender, PaintEventArgs e)
        {
            if (nhay == true)
            {
                if (halo_draw>=0 && halo_draw<8)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 1+ (halo_draw%4)*31,1+ (halo_draw / 4) * 31, 29, 29);
                }
               
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {

        }
        private void set_haoquang(int vt)
        {
            if (vt >= 0 && vt < halo_set.Count)
            {
                halo_set_traiphai = halo_set[vt][1];
                halo_set_ravao = halo_set[vt][0];
                halo_set_doimau = halo_set[vt][2];
                hien_buton_haoquang();
            }
        }


        private void button28_Click(object sender, EventArgs e)
        {
            panel_haoquang1.Visible = true;
            panel_haoquang.Visible = false;
            vt_menu = menu_haoquang_add;
            set_play_haoquang(false);

             halo_set_traiphai = 0;
             halo_set_ravao = 0;
             halo_set_doimau = 0;
             halo_draw = 0;

            //List<Color[,]> halo_color = new List<Color[,]>();
            // List<int[]> halo_set = new List<int[]>();
            list_hieung_haoquang.Items.Clear();
            halo_color.Clear();
            halo_set.Clear();
            int mm = 0;
            halo_tia = list_hieuung[vt_chon_vung].WIREV.Count;
               halo_hang = 0;
            for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
            {
                if (list_hieuung[vt_chon_vung].WIREV[x].Count > halo_hang) halo_hang = list_hieuung[vt_chon_vung].WIREV[x].Count;
            }
            num_nhan.Value = 1;
            if (halo_tia > 2) num_nhan.Value = 2;
            num_nhan.Maximum = halo_tia;
            for (int i = 0; i < list_color_haoquang.Count; i++)
            {
                Color[,] temp = new Color[halo_tia, halo_hang];
                for (int x = 0; x < halo_tia; x++)
                {
                    for (int y = 0; y < halo_hang; y++)
                    {
                        temp[x, y] = list_color_haoquang[i][x % haoquang_tia, y % haoquang_hang];
                    }
                }
                halo_color.Add(temp);
                list_hieung_haoquang.Items.Add(i.ToString());
                int[] set = new int[list_set_haoquang[i].Length];

                for (int x = 0; x < list_set_haoquang[i].Length; x++)
                {
                    set[x] = list_set_haoquang[i][x];
                }
                    halo_set.Add(set);
            }
            if (list_hieung_haoquang.Items.Count > 0) list_hieung_haoquang.SelectedIndex = 0;

            set_haoquang(list_hieung_haoquang.SelectedIndex);


        }

        private void list_hieung_haoquang_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (list_hieung_haoquang.SelectedIndex >= 0 && list_hieung_haoquang.SelectedIndex < list_hieung_haoquang.Items.Count && list_hieung_haoquang.Items.Count>1) toolStripMenuItem4.Enabled = true;
            else toolStripMenuItem4.Enabled = false;




            set_haoquang(list_hieung_haoquang.SelectedIndex);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            if (play_haoquang == false) set_play_haoquang(true);
            else set_play_haoquang(false);
        }

        private void button35_Click(object sender, EventArgs e)
        {
            set_play_haoquang(false);
            Color[,] temp = new Color[halo_tia, halo_hang];
            for (int x = 0; x < halo_tia; x++)
            {
                for (int y = 0; y < halo_hang; y++)
                {
                    temp[x, y] = Color.Transparent;
                }
            }
            halo_color.Add(temp);
            list_hieung_haoquang.Items.Add((halo_color.Count-1).ToString());
            int[] set = new int[halo_set[0].Length];
            set[0] = 0;
            set[1] = 0;
            set[2] = 0;
            
            halo_set.Add(set);
        }

        private void pictureBox21_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (int)(e.Location.X / 31);
            int y = (int)(e.Location.Y / 35);
           int  llll = x + y * 4;
            if(llll==0)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = toidan_moi(halo_color[list_hieung_haoquang.SelectedIndex],halo_tia,halo_hang);
            }
            else if (llll == 4)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = sangdan_moi(halo_color[list_hieung_haoquang.SelectedIndex], halo_tia, halo_hang);
            }
            else if (llll == 1)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = doimau_haoquang_moi(halo_color[list_hieung_haoquang.SelectedIndex], halo_tia, halo_hang);
            }
            else if (llll == 5)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = doimau_haoquang_moi(halo_color[list_hieung_haoquang.SelectedIndex], halo_tia, halo_hang);
            }
            else if (llll == 6)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = chaylui_moi(halo_color[list_hieung_haoquang.SelectedIndex], halo_tia, halo_hang);
            }
            else if (llll == 2)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = chaytoi_moi(halo_color[list_hieung_haoquang.SelectedIndex], halo_tia, halo_hang);
            }
            else if (llll == 7)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = toavao_moi(halo_color[list_hieung_haoquang.SelectedIndex], halo_tia, halo_hang);
            }
            else if (llll == 3)
            {
                if (list_hieung_haoquang.SelectedIndex >= 0) halo_color[list_hieung_haoquang.SelectedIndex] = toara_moi(halo_color[list_hieung_haoquang.SelectedIndex], halo_tia, halo_hang);
            }
            
            int vv = x + y * 4;
            if (vv >= 0 && vv < 8) toolTip1.SetToolTip(pictureBox21, name_halo_3[menu_ngonngu, vv] + "   ");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            HAOQUANG temp = new HAOQUANG(halo_tia, halo_hang, halo_color.Count, halo_color, halo_set);
            Color[,] temp_chay = new Color[halo_tia, halo_hang] ;
            for (int i = 0; i < temp.TONG; i++)
            {
                for (int x = 0; x < temp.TIA; x++)
                {
                    for (int y = 0; y < temp.HANG; y++)
                    {
                        if (temp.MAU[i][x, y] != Color.Transparent) temp_chay[x, y] = temp.MAU[i][x, y];
                    }
                }
            }

            Bitmap aa = new Bitmap(temp.TIA, temp.HANG);
            Graphics gg = Graphics.FromImage(aa);

            gg.Clear(Color.Black);


            PointF center = new PointF(0, 0);

            for (int x = 0; x < temp.TIA; x++)
            {
                for (int y = 0; y < temp.HANG; y++)
                {

                    //float goc = (float)GetAngle(center, new PointF((float)x, y));



                    //int  zx = (int)chuyendoiF((float)(goc * (tia / 2) / 180), (float)hang, (float)tia);

                    // int zy = (int)chuyendoiF(chieudaiF(center, new PointF((float)x, (float)y)), hang / 2, hang);
                    gg.FillEllipse(new SolidBrush(temp_chay[x, y]), new RectangleF(x, y, 2, 2));


                }
            }
            gg.Dispose();
            aa = (Bitmap)ResizeImage(aa, 80, 60);
            string teen = save_haoquang.ShowBox(aa, 0);
            if (teen != "")
            {
                var rmCrypto = GetAlgorithm();
                ICryptoTransform encryptor = rmCrypto.CreateEncryptor();

                using (var writer = new StreamWriter(new CryptoStream(System.IO.File.Create(path_halo+teen), encryptor, CryptoStreamMode.Write)))
                {
                    writer.Write(JsonConvert.SerializeObject(temp));

                }
            }
            hien_list_halo();
            timer.Enabled = true;
            timer_map.Enabled = true;
            set_vung();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            set_vung();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            set_play_haoquang(false);
            if (list_hieung_haoquang.SelectedIndex >= 0 && list_hieung_haoquang.SelectedIndex < list_hieung_haoquang.Items.Count && list_hieung_haoquang.Items.Count > 1)
            {
                halo_color.RemoveAt(list_hieung_haoquang.SelectedIndex);
                halo_set.RemoveAt(list_hieung_haoquang.SelectedIndex);
                list_hieung_haoquang.Items.RemoveAt(list_hieung_haoquang.SelectedIndex);
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            vt_hien_effect = vScrollBar1.Value;
            pic_hu_nen.Refresh();
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            vt_hien_vien = vScrollBar2.Value;
            pic_hu_vien.Refresh();
        }

        private void vScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            vt_hien_halo = vScrollBar3.Value;
            pic_hu_halo.Refresh();
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            vt_hien_vung--;
            if (vt_hien_vung <= 0) vt_hien_vung = 0;
            label3.Text = (vt_chon_vung + 1).ToString() + "/" + (list_hieuung.Count).ToString();
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            if (list_hieuung.Count <= 4)
            {

            }
            else
            {
                vt_hien_vung++;
                if (vt_hien_vung >= list_hieuung.Count - 4) vt_hien_vung = list_hieuung.Count - 4;
              
            }
            label3.Text = (vt_chon_vung + 1).ToString() + "/" + (list_hieuung.Count).ToString();
        }

        private void button38_Click_1(object sender, EventArgs e)
        {
            if (led_full.Count > 0)
            {

                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.Filter = "Map wire (  *.bmp)|*.bmp";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {


                    double[] thongtin_ban = new double[4];

                    double ttx = 0;
                    double tty = 0;

                    for (int x1 = 0; x1 < led_full.Count; x1++)
                    {
                        if (led_full[x1].X >= ttx) ttx = led_full[x1].X;
                        if (led_full[x1].Y >= tty) tty = led_full[x1].Y;

                    }
                    double ttw = ttx;
                    double tth = tty;
                    for (int x1 = 0; x1 < led_full.Count; x1++)
                    {
                        if (led_full[x1].X <= ttw) ttw = led_full[x1].X;
                        if (led_full[x1].Y <= tth) tth = led_full[x1].Y;

                    }


                    thongtin_ban[0] = ttw;
                    thongtin_ban[1] = tth;
                    thongtin_ban[2] = ttx - ttw;
                    thongtin_ban[3] = tty - tth;
                    Bitmap aa = new Bitmap((int)(thongtin_ban[2]) * 2 + 60, (int)(thongtin_ban[3]) * 2 + 60);
                    Pen color_day = new Pen(Color.FromArgb(180, 0, 151, 251), 1);
                    SolidBrush color_den = new SolidBrush(Color.FromArgb(255, 0, 0));
                    SolidBrush color_den1 = new SolidBrush(Color.FromArgb(9, 0, 0));
                    Pen colorten = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
                    Pen mau_rule = new Pen(Color.FromArgb(42, 128, 128, 128), 1);
                    Pen mau_rulec = new Pen(Color.FromArgb(64, 255, 0, 0), 1);
                    SolidBrush mauden = new SolidBrush(Color.FromArgb(250, 0, 0));
                    SolidBrush mauden0 = new SolidBrush(Color.FromArgb(250, 255, 0));
                    SolidBrush mauden00 = new SolidBrush(Color.FromArgb(0, 0, 255));
                    SolidBrush mauden1 = new SolidBrush(Color.FromArgb(0, 255, 0));
                    SolidBrush tamden = new SolidBrush(Color.FromArgb(32, 32, 32));
                    SolidBrush mauselect = new SolidBrush(Color.FromArgb(250, 250, 250));
                    Pen mau_l = new Pen(Color.FromArgb(96, 96, 96), 1);
                    Pen maussss = new Pen(Color.FromArgb(1, 151, 251), 2);
                    Pen mau_l0 = new Pen(Color.FromArgb(128, 0, 0), 1);
                    Pen mauday = new Pen(Color.FromArgb(255, 255, 255), 2);
                    Pen maudaykeo = new Pen(Color.FromArgb(128, 128, 128), 2);
                    Pen maudaythang = new Pen(Color.FromArgb(126, 87, 194), 2);
                    Graphics g = Graphics.FromImage(aa);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.Clear(Color.White);
                    for (int x = 0; x < led_full.Count; x++)
                    {
                        if (x == 0) DrawRoundedRectangle_fill(g, mauden0, (int)(led_full[x].X - thongtin_ban[0]) * 2 + 30, (int)(led_full[x].Y - thongtin_ban[1]) * 2 + 30, 12, 12, 3, 3);
                        else
                        {
                            if (vt_port2 < 0 || vt_port2 >= led_full.Count)
                            {
                                if (x > 0) DrawRoundedRectangle_fill(g, mauden, (int)(led_full[x].X - thongtin_ban[0]) * 2 + 30, (int)(led_full[x].Y - thongtin_ban[1]) * 2 + 30, 12, 12, 3, 3);
                            }
                            else
                            {
                                if (x < vt_port2) DrawRoundedRectangle_fill(g, mauden, (int)(led_full[x].X - thongtin_ban[0]) * 2 + 30, (int)(led_full[x].Y - thongtin_ban[1]) * 2 + 30, 12, 12, 3, 3);
                                else if (x == vt_port2) DrawRoundedRectangle_fill(g, mauden00, (int)(led_full[x].X - thongtin_ban[0]) * 2 + 30, (int)(led_full[x].Y - thongtin_ban[1]) * 2 + 30, 12, 12, 3, 3);
                                else DrawRoundedRectangle_fill(g, mauden1, (int)(led_full[x].X - thongtin_ban[0]) * 2 + 30, (int)(led_full[x].Y - thongtin_ban[1]) * 2 + 30, 12, 12, 3, 3);

                            }
                        }





                    }
                    // DrawRoundedRectangle_fill(g, color_den, (int)(led_full[0].X - thongtin_ban[0]) * 2 + 30, (int)(led_full[0].Y - thongtin_ban[1]) * 2 + 30, 12, 12, 3, 3);
                    //  g.ro

                    String drawString = "Total " + led_full.Count.ToString() + " LED-Behind";




                    // Draw string to screen.

                    if (led_full.Count >= 2)
                    {
                        for (int x = 0; x < led_full.Count - 1; x++)
                        {
                            DrawArrow(g, colorten, new PointF((int)(led_full[x].X - thongtin_ban[0]) * 2 + 30 + 6, (int)(led_full[x].Y - thongtin_ban[1]) * 2 + 30 + 6), new PointF((int)(led_full[x + 1].X - thongtin_ban[0]) * 2 + 30 + 6, (int)(led_full[x + 1].Y - thongtin_ban[1]) * 2 + 30 + 6), 6);
                            //  g.DrawLine(mau_den_hien, new System.Drawing.Point(vt_den[x].X, vt_den[x].Y), new System.Drawing.Point(vt_den[x + 1].X, vt_den[x + 1].Y));


                        }
                    }
                    aa.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    Font drawFont = new Font("Microsoft Sans Serif", 14);
                    SolidBrush drawBrush = new SolidBrush(Color.Black);
                    PointF drawPoint = new PointF(0, 0);
                    g.DrawString(drawString, drawFont, drawBrush, drawPoint);
                    g.Dispose();


                    aa.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
                }

            }
        }
        string path_setting = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\Setting.ini";
        private void button43_Click(object sender, EventArgs e)
        {
            if (menu_ngonngu == 0)
            {
                menu_ngonngu = 1;

            }
            else
            {
                menu_ngonngu = 0;

            }

            set_ngongu();
            if (File.Exists(path_setting) == true)
            {

                IniParser parser = new IniParser(path_setting);

                parser.AddSetting("SETTING", "Language", menu_ngonngu.ToString());

                parser.SaveSettings();

            }
        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            Font f = new Font("Microsoft Sans Serif", 10f);
            toolTip1.BackColor = Color.FromArgb(32, 32, 32);
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.White, new PointF(2, 2));
        }

        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox9_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void pictureBox10_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox8_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
          //  toolTip1.SetToolTip(button1, name_effect10[menu_ngonngu] + "   ");
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
           // toolTip1.SetToolTip(button2, name_effect11[menu_ngonngu] + "   ");
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
           // toolTip1.SetToolTip(label2, name_effect12[menu_ngonngu] + "   ");
        }
        int vt_menu1 = -1;
        int menu_wire_connect0 = 0;
        int menu_wire_connect1 = 1;
        int menu_wire_port1 = 2;
        int menu_wire_port2 = 3;
        int menu_wire_rotator1 = 4;
        int menu_wire_rotator2 = 5;
        private void button40_Click(object sender, EventArgs e)
        {
            get_tong();
            if (led_full.Count>0 && tong_play>0)
                {
                vt_menu = menu_nap;
                panel_effect0.Visible = false;
                panel_effect.Visible = false;
                panel_vien.Visible = false;
                panel_vien0.Visible = false;
                panel_haoquang.Visible = false;
                panel_nap.Visible = true;

                upled();



            }
        }
        private void upled( )
        {
            listBox1.Items.Clear();
            if(com_loaiden.SelectedIndex==1|| com_loaiden.SelectedIndex == 8)
            {
                listBox1.Items.Add("Port1:" + led_full.Count.ToString());
            }
            else
            {
                if (vt_port2 <= 0) listBox1.Items.Add("Port1:" + led_full.Count.ToString());
                else
                {
                    listBox1.Items.Add("Port1:" + vt_port2.ToString());
                    listBox1.Items.Add("Port2:" + (led_full.Count - vt_port2).ToString());
                }
            }

           
        }
        private void pictureBox13_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox19_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox18_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox17_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox15_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox14_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox11_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox12_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void pictureBox16_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void pictureBox28_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox24_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox24_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox25_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void pictureBox22_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void button44_Click(object sender, EventArgs e)
        {
            panel_haoquang1.Visible = true;
            panel_haoquang.Visible = false;
            vt_menu = menu_haoquang_add;
            set_play_haoquang(false);

            halo_set_traiphai = 0;
            halo_set_ravao = 0;
            halo_set_doimau = 0;
            halo_draw = 0;

            //List<Color[,]> halo_color = new List<Color[,]>();
            // List<int[]> halo_set = new List<int[]>();
            list_hieung_haoquang.Items.Clear();
            halo_color.Clear();
            halo_set.Clear();
            int mm = 0;
            halo_tia = list_hieuung[vt_chon_vung].WIREV.Count;
            halo_hang = 0;
            for (int x = 0; x < list_hieuung[vt_chon_vung].WIREV.Count; x++)
            {
                if (list_hieuung[vt_chon_vung].WIREV[x].Count > halo_hang) halo_hang = list_hieuung[vt_chon_vung].WIREV[x].Count;
            }
            num_nhan.Value = 1;
            if (halo_tia > 2) num_nhan.Value = 2;
            num_nhan.Maximum = halo_tia;
            
                Color[,] temp = new Color[halo_tia, halo_hang];
                for (int x = 0; x < halo_tia; x++)
                {
                    for (int y = 0; y < halo_hang; y++)
                    {
                        temp[x, y] = Color.Transparent;
                    }
                }
                halo_color.Add(temp);
                list_hieung_haoquang.Items.Add("0");
                int[] set = new int[8];

                for (int x = 0; x < 8; x++)
                {
                    set[x] = 0;
                }
                halo_set.Add(set);
            
            if (list_hieung_haoquang.Items.Count > 0) list_hieung_haoquang.SelectedIndex = 0;

            set_haoquang(list_hieung_haoquang.SelectedIndex);

        }

        private void pic_hu_haoquang_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            int x = (int)(e.Location.X / 47.5F);
            int y = (int)(e.Location.Y / 47.5F);
            int vv = x + y * 4;
             if(vv>=0 && vv<7) toolTip1.SetToolTip(pic_hu_haoquang, name_halo_1[menu_ngonngu, vv] + "   ");
             */
        }

        private void pictureBox23_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox21_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void button45_Click(object sender, EventArgs e)
        {
            if (File.Exists(System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\link_support.txt"))
            {
                string text = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\link_support.txt");

                System.Diagnostics.Process.Start(text);
            }

        }

        private void button42_Click(object sender, EventArgs e)
        {
            reset_moitruong();
        }
        bool viewmap = true;
        private void button41_Click(object sender, EventArgs e)
        {
            if (viewmap == false) viewmap = true;
            else viewmap = false;
        }

        private void button46_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            this.Hide();
            LUU_MAPOK dd = Form1.ShowBox("");
            if (dd != null)
            {
                led_select.Clear();

                map = dd;

                led_full = map.DATA;
                vt_port2 = map.PORT;
                list_anh.Clear();
                for (int x = 0; x < map.ANH.Count; x++)
                {
                    list_anh.Add(new ANH_ANH(byteArrayToImage(map.ANH[x].ANH), map.ANH[x].CENTER, map.ANH[x].SIZE, map.ANH[x].GOC, map.ANH[x].DIM));

                }
                mau_led = new Color[led_full.Count];
                list_hieuung.Clear();
                vt_chon_vung = -1;
                get_tong();
               

                set_vung();
                com_loaiden.SelectedIndex = 0;
                ma_rgb.SelectedIndex = 0;
                vt_chuot = new PointF(0, 0);
                reset_moitruong();
            }
            this.Show();
            timer_map.Enabled = true;
            timer_map.Enabled = true;
        }

        private void button51_Click(object sender, EventArgs e)
        {
            string ten = thuvien.ShowBox(list_path_effect[com_path_effect.SelectedIndex]+"\\", menu_ngonngu);
            if (ten != "")
            {
                hien_list_effect();
            }
        }

        private void pic_menu1_MouseMove(object sender, MouseEventArgs e)
        {
            int vv = e.Y / 49;
            if (vv >= 0 && vv < 6)
            {
                toolTip1.SetToolTip(pic_menu1, ngonngu_wire[menu_ngonngu, vv] + "   ");
            }
        }

        private void pic_menu1_MouseDown(object sender, MouseEventArgs e)
        {
            int vv = e.Y / 49;
            if (vv >= 0 && vv <= 5)
            {
                vt_menu1 = vv;

            }else
            {
                vt_menu1 = -1;
            }
            if (vv == menu_wire_rotator1)
            {
                if (led_full.Count > 0)
                {
                    panel_wail.Visible = true;
                    led_full1.Clear();
                    for (int i = 0; i < led_full.Count; i++) led_full1.Add(new LED1(led_full[i].X, led_full[i].Y, i));

                        Application.DoEvents();
                    Thread.Sleep(100);
                    List<LED1> vt_den_temp = new List<LED1>();
                   
                    if (vt_port2 < 0)
                    {
                        for (int i = 0; i < led_full1.Count; i++) vt_den_temp.Add(led_full1[i]);
                        for (int i = 0; i < led_full1.Count; i++) led_full1[i] = vt_den_temp[led_full1.Count - i - 1];
                    }
                    else
                    {
                        for (int i = 0; i < vt_port2; i++) vt_den_temp.Add(led_full1[i]);
                        for (int i = 0; i < vt_port2; i++) led_full1[i] = vt_den_temp[vt_port2 - i - 1];

                    }
                    for (int i = 0; i < led_full1.Count; i++) led_full[i] = new LED(led_full1[i].X, led_full1[i].Y);




                        for (int x = 0; x < list_hieuung.Count; x++)
                    {
                        for (int y = 0; y < list_hieuung[x].HIEUUNG.Count; y++)
                        {
                            for (int z = 0; z < list_hieuung[x].HIEUUNG[y].DATA.Count; z++)
                            {
                                int[] temp = new int[list_hieuung[x].HIEUUNG[y].DATA[z].Length];
                                for (int i = 0; i < list_hieuung[x].HIEUUNG[y].DATA[z].Length; i++)
                                {
                                    temp[i] = list_hieuung[x].HIEUUNG[y].DATA[z][(int)led_full1[i].VT];

                                }
                                list_hieuung[x].HIEUUNG[y].DATA[z] = temp;
                            }
                        }

                    }

                    panel_wail.Visible = false;
                }

            }
            else if (vv == menu_wire_rotator2)
            {
                if (led_full.Count > 0 && vt_port2 > 0)
                {
                    panel_wail.Visible = true;
                    led_full1.Clear();
                    for (int i = 0; i < led_full.Count; i++) led_full1.Add(new LED1(led_full[i].X, led_full[i].Y, i));



                    Application.DoEvents();
                    Thread.Sleep(100);
                    List<LED1> vt_den_temp = new List<LED1>();
                   
                   
                    for (int i = vt_port2; i < led_full1.Count; i++) vt_den_temp.Add(led_full1[i]);


                    for (int i = vt_port2; i < led_full1.Count; i++) led_full1[i] = vt_den_temp[led_full1.Count - i - 1];
                    for (int i = 0; i < led_full1.Count; i++) led_full[i] = new LED(led_full1[i].X, led_full1[i].Y);




                    for (int x = 0; x < list_hieuung.Count; x++)
                    {
                        for (int y = 0; y < list_hieuung[x].HIEUUNG.Count; y++)
                        {
                            for (int z = 0; z < list_hieuung[x].HIEUUNG[y].DATA.Count; z++)
                            {
                                int[] temp = new int[list_hieuung[x].HIEUUNG[y].DATA[z].Length];
                                for (int i = 0; i < list_hieuung[x].HIEUUNG[y].DATA[z].Length; i++)
                                {
                                    temp[i] = list_hieuung[x].HIEUUNG[y].DATA[z][(int)led_full1[i].VT];

                                }
                                list_hieuung[x].HIEUUNG[y].DATA[z] = temp;
                            }
                        }

                    }
                    panel_wail.Visible = false;

                }

            }
            upled();
        }

        private void pic_menu1_Paint(object sender, PaintEventArgs e)
        {
            if (nhay == true)
            {
                if (vt_menu1 >= 0 && vt_menu1 <= 5)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(1, 151, 251), 2), 2, 2 + vt_menu1 * 49, 46, 46);
                }

            }
        }

        private void button47_Click(object sender, EventArgs e)
        {
            vt_menu = menu_chinh;

            vt_chon_vung = -1;

            set_vung();
            int jj = get_tong_vung(vt_chon_vung);
            if (dem_play <= jj) dem_play = jj;

        }

        private void panel_nap_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void com_loaiden_SelectedIndexChanged(object sender, EventArgs e)
        {
            upled();
        }
        private void ThreadSafe(MethodInvoker method)
        {
            if (InvokeRequired)
                Invoke(method);
            else
                method();
        }


        private static string ByteArrayToString(ICollection<byte> input)
        {
            var result = string.Empty;

            if (input != null && input.Count > 0)
            {
                var isFirst = true;
                foreach (var b in input)
                {
                    result += isFirst ? string.Empty : ",";
                    result += b.ToString("X2");
                    isFirst = false;
                }
            }
            return result;
        }
        private void AppendText(string p)
        {
            // ThreadSafe(() => textBox1.AppendText(p + Environment.NewLine));
        }
        
        private void DeviceOnDisConnected()
        {
            ThreadSafe(() => txtkq.Text = "Chưa kết nối với LEDFULLmini !");
          

        }

        private void DeviceOnConnected()
        {
            ThreadSafe(() => txtkq.Text = "Đã kết nối với LEDFULLmini !");
            

        }
        private void button48_Click(object sender, EventArgs e)
        {
            byte[] pass_bd = new byte[1024];

            Random random = new Random();
            for (int i = 0; i < 145; i++) random.NextBytes(pass_bd);
            for (int i = 0; i < 26; i++) random.NextBytes(pass_bd);
            string ff = "";
            for (int i = 0; i < 1024; i++) ff = ff + pass_bd[i].ToString() + ",";
            textBox1.Text = ff;

        }
        byte[] gamma8 ={
0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  1,
    1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,
    2,  3,  3,  3,  3,  3,  3,  3,  4,  4,  4,  4,  4,  5,  5,  5,
    5,  6,  6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  9,  9,  9, 10,
   10, 10, 11, 11, 11, 12, 12, 13, 13, 13, 14, 14, 15, 15, 16, 16,
   17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 24, 24, 25,
   25, 26, 27, 27, 28, 29, 29, 30, 31, 32, 32, 33, 34, 35, 35, 36,
   37, 38, 39, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 50,
   51, 52, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 66, 67, 68,
   69, 70, 72, 73, 74, 75, 77, 78, 79, 81, 82, 83, 85, 86, 87, 89,
   90, 92, 93, 95, 96, 98, 99,101,102,104,105,107,109,110,112,114,
  115,117,119,120,122,124,126,127,129,131,133,135,137,138,140,142,
  144,146,148,150,152,154,156,158,160,162,164,167,169,171,173,175,
  177,180,182,184,186,189,191,193,196,198,200,203,205,208,210,213,
  215,218,220,223,225,228,231,233,236,239,241,244,247,249,252,255 };
        int MAU0BIT = 0;
        int MAU8BIT = 1;


        int LOAI1903 = 0;
        int LOAI6803= 1;
        int LOAI1CONG = 0;
        int LOAI2CONG = 1;
        static byte[] pass_goc = new byte[1024] { 216, 242, 89, 109, 228, 32, 143, 252, 34, 6, 163, 158, 148, 138, 14, 14, 165, 39, 163, 204, 248, 74, 41, 14, 37, 9, 81, 98, 228, 91, 160, 84, 115, 17, 79, 165, 90, 176, 120, 10, 127, 147, 136, 64, 217, 198, 217, 155, 50, 101, 117, 117, 218, 112, 234, 141, 201, 75, 72, 218, 207, 45, 24, 198, 101, 79, 43, 130, 58, 104, 179, 244, 174, 152, 76, 101, 193, 232, 52, 94, 48, 181, 47, 126, 230, 42, 121, 3, 39, 193, 220, 15, 103, 158, 59, 82, 123, 194, 218, 137, 155, 86, 96, 201, 177, 128, 199, 65, 36, 133, 203, 224, 22, 233, 170, 25, 253, 154, 224, 58, 213, 39, 91, 120, 140, 164, 140, 16, 93, 250, 234, 255, 14, 171, 195, 217, 85, 102, 204, 102, 99, 56, 223, 161, 246, 251, 248, 126, 243, 34, 84, 224, 225, 159, 179, 115, 251, 231, 61, 12, 243, 182, 227, 41, 155, 204, 210, 107, 38, 208, 195, 151, 206, 122, 215, 157, 72, 185, 130, 144, 172, 14, 28, 59, 165, 9, 29, 110, 247, 79, 222, 110, 41, 191, 114, 172, 84, 181, 6, 41, 41, 140, 88, 34, 94, 189, 18, 103, 200, 22, 42, 65, 100, 172, 96, 229, 153, 168, 132, 145, 174, 99, 116, 214, 242, 85, 110, 14, 7, 42, 72, 147, 179, 89, 103, 32, 182, 250, 220, 232, 246, 182, 166, 224, 36, 156, 10, 124, 95, 141, 18, 171, 49, 116, 123, 197, 23, 129, 48, 9, 78, 3, 95, 158, 205, 151, 142, 11, 69, 63, 46, 159, 204, 155, 154, 247, 189, 147, 177, 85, 75, 241, 175, 122, 23, 156, 98, 62, 221, 161, 8, 52, 202, 211, 153, 243, 86, 7, 18, 141, 14, 254, 55, 31, 94, 115, 223, 149, 217, 131, 8, 131, 208, 218, 190, 93, 84, 228, 134, 49, 53, 80, 45, 163, 55, 249, 213, 249, 2, 167, 160, 181, 129, 36, 71, 76, 186, 144, 28, 164, 189, 204, 100, 90, 153, 132, 100, 239, 20, 60, 159, 114, 129, 225, 87, 189, 209, 147, 231, 100, 157, 230, 147, 50, 226, 82, 2, 171, 146, 114, 163, 195, 200, 226, 115, 104, 235, 211, 10, 179, 149, 229, 228, 197, 8, 46, 52, 159, 204, 137, 122, 38, 168, 183, 6, 214, 57, 50, 119, 70, 130, 185, 93, 162, 152, 219, 169, 158, 109, 239, 210, 253, 137, 52, 207, 183, 2, 205, 41, 179, 30, 98, 223, 9, 247, 125, 26, 16, 219, 156, 47, 185, 92, 196, 48, 219, 136, 65, 44, 45, 133, 149, 50, 221, 183, 124, 156, 116, 232, 78, 212, 107, 8, 195, 39, 31, 217, 84, 171, 27, 192, 152, 195, 209, 192, 24, 161, 197, 3, 243, 47, 192, 160, 252, 46, 136, 48, 2, 81, 122, 224, 166, 40, 140, 200, 195, 176, 152, 156, 17, 2, 52, 149, 17, 108, 236, 210, 97, 28, 158, 218, 215, 112, 244, 31, 19, 203, 12, 148, 158, 238, 215, 2, 48, 59, 26, 112, 54, 8, 252, 103, 9, 40, 241, 241, 251, 42, 143, 143, 66, 182, 207, 229, 178, 159, 9, 53, 51, 108, 180, 247, 164, 3, 253, 34, 42, 49, 101, 213, 82, 123, 155, 88, 32, 54, 209, 174, 126, 3, 36, 233, 60, 124, 82, 232, 31, 242, 79, 145, 50, 228, 60, 201, 84, 5, 195, 5, 43, 207, 198, 201, 197, 185, 61, 198, 27, 119, 197, 123, 206, 90, 182, 48, 72, 202, 187, 39, 177, 21, 3, 56, 225, 211, 163, 109, 63, 209, 3, 27, 115, 203, 131, 175, 60, 90, 35, 130, 62, 139, 204, 168, 45, 212, 195, 215, 46, 11, 129, 138, 73, 155, 83, 21, 203, 142, 231, 242, 22, 208, 135, 73, 116, 170, 8, 2, 215, 6, 244, 237, 167, 57, 233, 38, 72, 91, 11, 13, 16, 203, 62, 51, 79, 121, 209, 215, 119, 109, 227, 173, 115, 49, 108, 110, 4, 130, 52, 131, 203, 192, 0, 39, 23, 147, 226, 16, 178, 45, 205, 112, 131, 217, 225, 74, 145, 83, 250, 251, 216, 49, 138, 106, 35, 70, 122, 118, 205, 123, 33, 197, 39, 136, 66, 79, 203, 23, 28, 188, 150, 193, 36, 73, 159, 114, 42, 153, 79, 33, 220, 176, 136, 57, 170, 154, 54, 149, 3, 208, 25, 108, 66, 55, 12, 7, 73, 251, 151, 145, 126, 122, 55, 61, 100, 22, 12, 64, 202, 176, 28, 224, 38, 171, 158, 113, 60, 237, 221, 167, 24, 53, 19, 76, 162, 41, 126, 236, 61, 151, 40, 47, 1, 189, 163, 97, 121, 75, 212, 148, 141, 246, 203, 83, 180, 56, 70, 151, 152, 154, 203, 91, 30, 239, 120, 72, 102, 235, 154, 58, 151, 32, 2, 50, 135, 237, 223, 104, 8, 60, 248, 241, 25, 72, 26, 34, 106, 192, 151, 106, 227, 231, 83, 163, 204, 205, 16, 17, 69, 90, 251, 142, 176, 153, 252, 109, 243, 153, 203, 198, 89, 222, 143, 92, 162, 218, 66, 213, 213, 86, 222, 166, 83, 48, 87, 176, 204, 95, 101, 186, 220, 205, 35, 173, 65, 106, 64, 127, 76, 173, 46, 209, 244, 209, 17, 5, 87, 247, 0, 242, 138, 59, 60, 238, 123, 84, 58, 128, 66, 76, 161, 148, 51, 17, 234, 139, 186, 225, 26, 56, 153, 194, 137, 40, 39, 13, 178, 129, 30, 81, 88, 212, 94, 115, 47, 160, 145, 52, 49, 237, 47, 192, 60, 0, 11, 153, 157, 227, 230, 133, 74, 118, 221, 200, 89, 199, 178, 19, 199, 110, 162, 185, 97, 241, 243, 204, 54, 192, 225, 73, 250, 134, 175, 44, 9, 216, 133, 137, 28, 141, 111, 207, 155, 153, 6, 226, 246, 150, 25, 104, 238, 125, 109, 195, 75, 118, 95, 75, 12, 62, 99, 221, 1, 157, 138, 196, 198, 176, 190, 128, 66, 41, 247, 57, 255, 210, 29, 200, 234, 17, 214, 160, 166, 121, 90, 125, 25, 236, 225, 147, 121, 58, 124, 221, 42, 146, 205};
        private int nap_mach( )
        {
            UsbHidDevice Device;
            Device = new UsbHidDevice(0x0484, 0x2804);
            Device.OnConnected += DeviceOnConnected;
            Device.OnDisConnected += DeviceOnDisConnected;

            Device.Connect();
            if (Device.Connect() == true)
            {

            }
            else
            {
                string ten = thongbao.ShowBox(name_nap3[menu_ngonngu], menu_ngonngu);
                return -1;
            }


           


            int tong_byte = tong_play * mau_led.Length;
            int bit = 0;
            int loai_den = LOAI1903;
            int loai_day = LOAI1903;
            int loai_cong = LOAI1CONG;
            int vtport1 = 0;
            int tongport1 = 0;
            int vtport2 = 0;
            int tongport2 = 0;
            int bongtruyen = 0;
            byte vantoc = 1;
            byte dosang = 1;
            loai_den = com_loaiden.SelectedIndex;
            if (com_loaiden.SelectedIndex == 0)//1903
            {
                 
                 loai_day = LOAI1903;
            }
            else if (com_loaiden.SelectedIndex == 1)//6803
            {
                
                loai_day = LOAI6803;
            }
            else if (com_loaiden.SelectedIndex == 2)//1914
            {
                
                loai_day = LOAI1903;
            }
            else if (com_loaiden.SelectedIndex == 3)//8025
            {
                
                loai_day = LOAI1903;
            }
            else if (com_loaiden.SelectedIndex == 4)//8026
            {
               
                loai_day = LOAI1903;
            }
            else if (com_loaiden.SelectedIndex == 5)//9883
            {
                
                loai_day = LOAI1903;
            }
            else if (com_loaiden.SelectedIndex == 6)//WS2812
            {
               
                loai_day = LOAI1903;
            }
            else if (com_loaiden.SelectedIndex == 7)//1904
            {
                
                loai_day = LOAI1903;
            }
            else if (com_loaiden.SelectedIndex == 8)//9803
            {

                loai_day = LOAI6803;
            }
            if (vt_port2 > 0) loai_cong = LOAI2CONG;

            if (loai_day== LOAI6803)
            {
                vtport1 = 0;
                tongport1 = mau_led.Length;
                vtport2 = 0;
                tongport2 = 0;
                bongtruyen = mau_led.Length;
            }
            else
            {

                if (loai_cong == LOAI1CONG)
                {
                    vtport1 = 0;
                    tongport1 = mau_led.Length;
                    vtport2 = 0;
                    tongport2 = 0;
                    bongtruyen = mau_led.Length;
                }
                else
                {
                    vtport1 = 0;
                    tongport1 = vt_port2;
                    vtport2 = vt_port2;
                    tongport2 = mau_led.Length - vt_port2;
                    bongtruyen = vt_port2;
                    if (mau_led.Length - vt_port2 >= bongtruyen) bongtruyen = mau_led.Length - vt_port2;
                }

            }

           

            


            if (loai_day == LOAI1903)
            {

                if (bongtruyen > 3072)
                {
                    string ten = thongbao.ShowBox(name_nap8[menu_ngonngu], menu_ngonngu);
                    return -1;
                }
                else
                {
                    if (tong_byte > 16777184)
                    {
                        string ten = thongbao.ShowBox(name_nap9[menu_ngonngu], menu_ngonngu);
                        return -1;
                    }
                    else
                    {
                        bit = MAU8BIT;
                    }
                }
                if (bongtruyen < 100) vantoc = 30;
                else if (bongtruyen < 300) vantoc = 25;
                else if (bongtruyen < 500) vantoc = 20;
                else if (bongtruyen < 800) vantoc = 15;
                else if (bongtruyen < 1000) vantoc = 12;
                else if (bongtruyen < 1300) vantoc = 10;
                else if (bongtruyen < 1600) vantoc = 6;
                else if (bongtruyen < 2000) vantoc = 4;
                else if (bongtruyen < 2300) vantoc = 3;
                else if (bongtruyen < 2600) vantoc = 2;
                else   vantoc = 0;
            }
            else if (loai_day == LOAI6803)
            {
                if (bongtruyen > 6144)
                {
                    string ten = thongbao.ShowBox(name_nap8[menu_ngonngu], menu_ngonngu);
                    return -1;
                }
                else
                {
                    if (tong_byte > 16777184)
                    {
                        string ten = thongbao.ShowBox(name_nap9[menu_ngonngu], menu_ngonngu);
                        return -1;
                    }
                    else
                    {

                    }
                }
                vantoc = 50;
            }




            timer.Enabled = false;
            timer_map.Enabled = false;
            List<Color[]> data = new List<Color[]>();

            text_nap.Text = name_nap4[menu_ngonngu];
            text_nap1.Text = "";
            this.panel_wail_nap.Visible = true;
            Application.DoEvents();
            progressBar1.Value = 0;
            progressBar1.Maximum = 11;

            int hhh = 0;
            int cu = 0;
            Color c;

            for (int i = 0; i < tong_play; i++)
            {
                Color[] mau = new Color[mau_led.Length];
                int[] mtong = new int[mau_led.Length];
                for (int x = 0; x < mau_led.Length; x++) mtong[x] = 0;


                for (int x = 0; x < list_hieuung.Count; x++)
                {
                    int[] tt = get_vt_lop_timeline(i, x);

                    if (tt[0] >= 0 && tt[1] >= 0)
                    {

                        int ddd = tt[1];
                        if (list_hieuung[x].HIEUUNG[tt[0]].COGIAN == true)
                        {
                            ddd = (int)(chuyendoiF(ddd, list_hieuung[x].HIEUUNG[tt[0]].TONG_HIENTAI, list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC));
                        }
                        else
                        {
                            ddd = ddd % list_hieuung[x].HIEUUNG[tt[0]].TONG_GOC;
                        }

                        int[] mm = list_hieuung[x].HIEUUNG[tt[0]].DATA[ddd];

                        for (int ii = 0; ii < mm.Length; ii++)
                        {
                            if (mm[ii] != 0) mtong[ii] = mm[ii];
                        }
                    }



                }
                for (int x = 0; x < mau_led.Length; x++)
                {
                    if (mtong[x] == 0)
                    {
                        mau[x] = Color.Black;

                    }
                    else
                    {
                         /*
                        c = travemau(mtong[x]);
                        if (ma_rgb.SelectedIndex == 1) c = Color.FromArgb(gamma8[c.R], gamma8[c.B], gamma8[c.G]);
                        else if (ma_rgb.SelectedIndex == 2) c = Color.FromArgb(gamma8[c.G], gamma8[c.R], gamma8[c.B]);
                        else if (ma_rgb.SelectedIndex == 3) c = Color.FromArgb(gamma8[c.G], gamma8[c.B], gamma8[c.R]);
                        else if (ma_rgb.SelectedIndex == 4) c = Color.FromArgb(gamma8[c.B], gamma8[c.R], gamma8[c.G]);
                        else if (ma_rgb.SelectedIndex == 5) c = Color.FromArgb(gamma8[c.B], gamma8[c.G], gamma8[c.R]);
                        else c = Color.FromArgb(gamma8[c.R], gamma8[c.G], gamma8[c.B]);
                         */
                        
                        c = travemau(mtong[x]);
                        if (ma_rgb.SelectedIndex == 1) c = Color.FromArgb(c.R, c.B, c.G);
                        else if (ma_rgb.SelectedIndex == 2) c = Color.FromArgb(c.G, c.R, c.B);
                        else if (ma_rgb.SelectedIndex == 3) c = Color.FromArgb(c.G,c.B, c.R);
                        else if (ma_rgb.SelectedIndex == 4) c = Color.FromArgb(c.B,c.R, c.G);
                        else if (ma_rgb.SelectedIndex == 5) c = Color.FromArgb(c.B, c.G, c.R);
                        else c = Color.FromArgb(c.R, c.G,c.B);
                        
                        mau[x] = c;
                    }

                }
                data.Add(mau);
                hhh = (int)(chuyendoiF(i, tong_play, 10));
                if (cu != hhh)
                {
                    progressBar1.Value = hhh;
                    cu = hhh;
                    Application.DoEvents();
                }
            }
            


            //label_nap.Text = "Dung lượng: " + String.Format("{0:0.00}", (tong_byte * 100) / 16777216) + "%";

            int ttt = (32 + tong_byte) / 2048;
            if ((32 + tong_byte) % 2048 > 0) ttt++;

            byte[] data_nap = new byte[ttt * 2048];

            byte[] pass = new byte[8];
            byte[] mahoa = new byte[8];
            Random random = new Random();
            for (int i = 0; i < 145; i++) random.NextBytes(pass);
            for (int i = 0; i < 145; i++) random.NextBytes(data_nap);
            mahoa[0] = pass_goc[pass[0]];
            mahoa[1] = pass_goc[pass[1]];
            mahoa[2] = pass_goc[pass[2]];
            mahoa[3] = pass_goc[pass[3]];
            mahoa[4] = pass_goc[pass[4]];
            mahoa[5] = pass_goc[pass[5]];
            mahoa[6] = pass_goc[pass[6]];
            mahoa[7] = pass_goc[pass[7]];

            Byte[] dd1 = BitConverter.GetBytes(vtport1);
            Byte[] dd2 = BitConverter.GetBytes(tongport1);
            Byte[] dd3 = BitConverter.GetBytes(vtport2);
            Byte[] dd4 = BitConverter.GetBytes(tongport2);
            Byte[] dd5 = BitConverter.GetBytes(bongtruyen);
            Byte[] dd6 = BitConverter.GetBytes(tong_play);

            data_nap[0] = (byte)'L';// 
            data_nap[1] = (byte)'E';//
            data_nap[2] = (byte)'D';// 
            data_nap[3] = (byte)loai_den;//loaiden
            data_nap[4] = (byte)loai_day;//loaiday
            data_nap[5] = vantoc;//vantoc
            data_nap[6] = dosang;//dosang
            data_nap[7] = dd1[0];//vtport1 
            data_nap[8] = dd1[1];//vtport1
            data_nap[9] = dd2[0];//tongport1
            data_nap[10] = dd2[1];//tongport1
            data_nap[11] = dd3[0];//vtport2 
            data_nap[12] = dd3[1];//vtport2
            data_nap[13] = dd4[0];//tongport2
            data_nap[14] = dd4[1];//tongport2
            data_nap[15] = dd5[0];//bongtruyen
            data_nap[16] = dd5[1];//bongtruyen
            data_nap[17] = dd6[0];//tong play
            data_nap[18] = dd6[1];//tong play
            data_nap[18] = dd6[1];//tong play

            Bitmap bmpDest = new Bitmap(
                        10,
                        10,
                        System.Drawing.Imaging.PixelFormat.Format8bppIndexed
                        );

            progressBar1.Value = 0;
            progressBar1.Maximum = 11;
            hhh = 0;
            cu = 0;
            for (int i = 0; i < data.Count; i++)
            {
                byte[] buffer = new byte[mau_led.Length * 3];
                byte[] destBuffer = new byte[mau_led.Length];
                for (int j = 0; j < mau_led.Length; j++)
                {
                    buffer[j * 3 + 2] = data[i][j].R;
                    buffer[j * 3 + 1] = data[i][j].G;
                    buffer[j * 3] = data[i][j].B;

                }
                MatchColors(buffer, destBuffer, 3, bmpDest.Palette);

                for (int j = 0; j < mau_led.Length; j++)
                {

                    data_nap[32 + i * mau_led.Length + j] = (byte)(destBuffer[j]^mahoa[j%8]);
                }
                hhh = (int)(chuyendoiF(i, data.Count, 10));
                if (cu != hhh)
                {
                    progressBar1.Value = hhh;
                    cu = hhh;
                    Application.DoEvents();
                }
            }

           

                Application.DoEvents();
                byte[] DataWrite = new byte[64];
                int tong_data = data_nap.Length / 2048;
                if (data_nap.Length % 2048 > 0) tong_data++;
                int ttnap = 2048 * tong_data;
                Application.DoEvents();
                progressBar1.Value = 0;
                progressBar1.Maximum = ttnap / 64+1;
                hhh = 0;
                cu = 0;
                for (int i2 = 0; i2 < 64; i2++) { DataWrite[i2] = 0; }
                Byte[] bbb = BitConverter.GetBytes(tong_data);
                DataWrite[0] = (byte)'D';
                DataWrite[1] = (byte)'A';
                DataWrite[2] = (byte)'T';

                DataWrite[4] = bbb[0];
                DataWrite[5] = bbb[1];
                DataWrite[6] = bbb[2];
            DataWrite[7] = vantoc;
            DataWrite[8] = dosang;
            DataWrite[9] = (byte)'C';
            DataWrite[10] = (byte)'H';
            DataWrite[11] = (byte)'E';
            DataWrite[12] = (byte)'C';
            DataWrite[13] = (byte)'K';
            DataWrite[20] = pass[0];
            DataWrite[21] = pass[1];
            DataWrite[22] = pass[2];
            DataWrite[23] = pass[3];
            DataWrite[24] = pass[4];
            DataWrite[25] = pass[5];
            DataWrite[26] = pass[6];
            DataWrite[27] = pass[7];

            text_nap.Text = name_nap5[menu_ngonngu] + String.Format("{0:0.00}", ((float)tong_byte * 100) / 16777216) + "%";
                text_nap1.Text = name_nap6[menu_ngonngu];
                Application.DoEvents();
                var command0 = new CommandMessage(48, DataWrite);
                Device.SendMessage(command0);


                for (int i1 = 0; i1 < ttnap / 64; i1++)
                {



                    for (int i2 = 0; i2 < 64; i2++) { DataWrite[i2] = 0; DataWrite[i2] = data_nap[i2 + i1 * 64]; }
                    var command1 = new CommandMessage(48, DataWrite);
                    Device.SendMessage(command1);

                    progressBar1.Value++;
                    text_nap1.Text = name_nap7[menu_ngonngu] + ((int)chuyendoiF(i1, ttnap / 64, 100)).ToString() + "%";
                    Application.DoEvents();


                }


 



            // byte[] data_nap=new byte[]

            this.panel_wail_nap.Visible = false;
            Application.DoEvents();
            timer.Enabled = true;
            timer_map.Enabled = true;
            return 0;
        }
            private void button49_Click(object sender, EventArgs e)
        {
            nap_mach();
        }
        private Hashtable m_knownColors = new Hashtable((int)Math.Pow(2, 20), 1.0f);
        private void MatchColors(
           byte[] buffer,
           byte[] destBuffer,
           int pixelSize,
           ColorPalette pallete)
        {
            int length = destBuffer.Length;
            byte[] temp = new byte[pixelSize];

            int palleteSize = pallete.Entries.Length;

            int mult_1 = 256;
            int mult_2 = 256 * 256;

            int currentKey = 0;

            for (int i = 0; i < length; i++)
            {
                Array.Copy(buffer, i * pixelSize, temp, 0, pixelSize);

                currentKey = temp[0] + temp[1] * mult_1 + temp[2] * mult_2;

                if (!m_knownColors.ContainsKey(currentKey))
                {
                    destBuffer[i] = GetSimilarColor(pallete, temp, palleteSize);
                    m_knownColors.Add(currentKey, destBuffer[i]);
                }
                else
                {
                    destBuffer[i] = (byte)m_knownColors[currentKey];
                }
            }// for
        }
        private byte GetSimilarColor(ColorPalette palette, byte[] color, int palleteSize)
        {

            byte minDiff = byte.MaxValue;
            byte index = 0;

            if (color.Length == 3)
            {
                for (int i = 0; i < palleteSize - 1; i++)
                {

                    byte currentDiff = GetMaxDiff(color, palette.Entries[i]);

                    if (currentDiff < minDiff)
                    {
                        minDiff = currentDiff;
                        index = (byte)i;
                    }
                }// for
            }
            else
            {
                throw new ApplicationException("Only 24bit colors supported now");
            }

            return index;
        }

        /// <summary>
        /// Return similar color
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static byte GetMaxDiff(byte[] a, Color b)
        {
            byte bDiff = a[0] > b.B ? (byte)(a[0] - b.B) : (byte)(b.B - a[0]);

            byte gDiff = a[1] > b.G ? (byte)(a[1] - b.G) : (byte)(b.G - a[1]);

            byte rDiff = a[2] > b.R ? (byte)(a[2] - b.R) : (byte)(b.R - a[2]);

            byte max = bDiff > gDiff ? bDiff : gDiff;

            max = max > rDiff ? max : rDiff;

            return max;
        }
        private void txtkq_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (vt_chon_vung >= 0 && vt_chon_vung < list_hieuung.Count) list_hieuung.RemoveAt(vt_chon_vung);
            vt_chon_vung = -1;
            set_vung();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            
                timer.Enabled = false;
                timer_map.Enabled = false;
            // hieuung_data_dulieu.Add(new List<List<byte[]>>());

            LAYER_CHINH temp = list_hieuung[vt_chon_vung - 1];
            list_hieuung[vt_chon_vung - 1] = list_hieuung[vt_chon_vung];
            list_hieuung[vt_chon_vung] = temp;

             



                vt_chon_vung = -1;
            set_vung();
            timer.Enabled = true;
            timer_map.Enabled = true;

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            // hieuung_data_dulieu.Add(new List<List<byte[]>>());
 

            LAYER_CHINH temp = list_hieuung[vt_chon_vung + 1];
            list_hieuung[vt_chon_vung + 1] = list_hieuung[vt_chon_vung];
            list_hieuung[vt_chon_vung] = temp;


            vt_chon_vung = -1;
            set_vung();
            timer.Enabled = true;
            timer_map.Enabled = true;
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (vt_keo_time_xxx >= 0 && vt_keo_time_yyy >= 0 && vt_keo_time_xxx < list_hieuung[vt_keo_time_yyy].HIEUUNG.Count - 1)
            {
                try
                {
                    timer.Enabled = false;
                    timer_map.Enabled = false;

                    LAYER_HIEUUNG temp = list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx];

                   



                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx] = list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx+1];
                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx + 1]= temp;


 


                    get_tong();
                    timer.Enabled = true;
                    timer_map.Enabled = true;
                }
                catch
                {
                }





            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            if (vt_keo_time_xxx > 0 && vt_keo_time_yyy >= 0 )
            {
                try
                {
                    timer.Enabled = false;
                    timer_map.Enabled = false;

                    LAYER_HIEUUNG temp = list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx];





                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx] = list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx - 1];
                    list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx - 1] = temp;





                    get_tong();
                    timer.Enabled = true;
                    timer_map.Enabled = true;
                }
                catch
                {
                }





            }
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            if (vt_keo_time_xxx >= 0 && vt_keo_time_yyy >= 0)
            {
                try
                {
                    if (list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].COGIAN == false) list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].COGIAN = true;
                    else list_hieuung[vt_keo_time_yyy].HIEUUNG[vt_keo_time_xxx].COGIAN = false;

                    //  MessageBox.Show(hieuung_set[vt_chon_vung][tave[0]][3].ToString());

                    get_tong();
                }
                catch
                {
                }
            }
            timer.Enabled = true;
            timer_map.Enabled = true;
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            if (vt_keo_time_xxx >= 0 && vt_keo_time_yyy >= 0)
            {
                try
                {
                    list_hieuung[vt_keo_time_yyy].HIEUUNG.RemoveAt(vt_keo_time_xxx);

                    get_tong();
                }
                catch
                {
                }
            }
            timer.Enabled = true;
            timer_map.Enabled = true;
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {

        }

        private void pic_menu1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void num_vantoc_haoquang_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pic_map_Move(object sender, EventArgs e)
        {

        }
        private void hien_buton_haoquang()
        {
            if (halo_set_traiphai == 1) hieuung1.Image = Properties.Resources.hu3;
            else if (halo_set_traiphai == 2) hieuung1.Image = Properties.Resources.hu2;
            else  hieuung1.Image = Properties.Resources.hu1;

            if (halo_set_ravao == 1) hieuung2.Image = Properties.Resources.hu6;
            else if (halo_set_ravao == 2) hieuung2.Image = Properties.Resources.hu5;
            else hieuung2.Image = Properties.Resources.hu4;

            if (halo_set_doimau == 1) hieuung3.Image = Properties.Resources.hu8;
            else if (halo_set_doimau == 2) hieuung3.Image = Properties.Resources.hu10;
            else if (halo_set_doimau == 3) hieuung3.Image = Properties.Resources.hu11;
            else hieuung3.Image = Properties.Resources.hu7;


 


        }
        private void hieuung1_Click(object sender, EventArgs e)
        {
            halo_set_traiphai++;
            if (halo_set_traiphai >= 3) halo_set_traiphai = 0;
            if (list_hieung_haoquang.SelectedIndex >= 0)
            {
                halo_set[list_hieung_haoquang.SelectedIndex][1] = halo_set_traiphai;
                halo_set[list_hieung_haoquang.SelectedIndex][0] = halo_set_ravao;


                halo_set[list_hieung_haoquang.SelectedIndex][2] = halo_set_doimau;


                set_play_haoquang(true);
            }
            hien_buton_haoquang();
        }

        private void hieuung2_Click(object sender, EventArgs e)
        {
            halo_set_ravao++;
            if (halo_set_ravao >= 3) halo_set_ravao = 0;
            if (list_hieung_haoquang.SelectedIndex >= 0)
            {
                halo_set[list_hieung_haoquang.SelectedIndex][1] = halo_set_traiphai;
                halo_set[list_hieung_haoquang.SelectedIndex][0] = halo_set_ravao;


                halo_set[list_hieung_haoquang.SelectedIndex][2] = halo_set_doimau;


                set_play_haoquang(true);
            }
            hien_buton_haoquang();
        }

        private void hieuung3_Click(object sender, EventArgs e)
        {
            halo_set_doimau++;
            if (halo_set_doimau >= 4) halo_set_doimau = 0;
            if (list_hieung_haoquang.SelectedIndex >= 0)
            {
                halo_set[list_hieung_haoquang.SelectedIndex][1] = halo_set_traiphai;
                halo_set[list_hieung_haoquang.SelectedIndex][0] = halo_set_ravao;


                halo_set[list_hieung_haoquang.SelectedIndex][2] = halo_set_doimau;


                set_play_haoquang(true);
            }
            hien_buton_haoquang();
        }

        private void button50_Click(object sender, EventArgs e)
        {
          
            if(list_hieung_haoquang.SelectedIndex>=0 && list_hieung_haoquang.SelectedIndex< list_hieung_haoquang.Items.Count)
            {
                for (int x = 0; x < halo_color[list_hieung_haoquang.SelectedIndex].GetLength(0); x++)
                {
                    for (int y = 0; y < halo_color[list_hieung_haoquang.SelectedIndex].GetLength(1); y++)
                    {
                        halo_color[list_hieung_haoquang.SelectedIndex][x,y] = mauvien_chon;
                    }
                }

                 
            }
         
        }

        private void button50_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void hieuung1_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void hieuung2_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void hieuung3_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void pic_hu_nen_MouseEnter(object sender, EventArgs e)
        {
            pic_hu_nen.Focus();
        }

        private void pic_hu_vien_MouseEnter(object sender, EventArgs e)
        {
            pic_hu_vien.Focus();
        }

        private void pic_hu_halo_MouseEnter(object sender, EventArgs e)
        {
            pic_hu_halo.Focus();
        }

        private void pictureBox22_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox22, name_haoquang_2[menu_ngonngu] + "   ");
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox6, name_effect1[menu_ngonngu] + "   ");
        }

        private void pictureBox9_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox9, name_effect2[menu_ngonngu] + "   ");
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox4, name_effect3[menu_ngonngu] + "   ");
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox3, name_effect4[menu_ngonngu] + "   ");
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox5, name_effect5[menu_ngonngu] + "   ");
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox1, name_effect6[menu_ngonngu] + "   ");
        }

        private void pictureBox10_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox10, name_effect7[menu_ngonngu] + "   ");
        }

        private void pictureBox8_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox8, name_effect8[menu_ngonngu] + "   ");
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox7, name_effect9[menu_ngonngu] + "   ");
        }

        private void pictureBox13_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox13, name_effect1[menu_ngonngu] + "   ");
        }

        private void pictureBox19_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox19, name_effect2[menu_ngonngu] + "   ");
        }

        private void pictureBox18_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox18, name_effect3[menu_ngonngu] + "   ");
        }

        private void pictureBox17_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox17, name_effect4[menu_ngonngu] + "   ");
        }

        private void pictureBox15_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox15, name_effect5[menu_ngonngu] + "   ");
        }

        private void pictureBox14_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox14, name_effect6[menu_ngonngu] + "   ");
        }

        private void pictureBox11_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox11, name_effect7[menu_ngonngu] + "   ");
        }

        private void pictureBox12_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox12, name_effect8[menu_ngonngu] + "   ");
        }

        private void pictureBox16_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox16, name_effect9[menu_ngonngu] + "   ");
        }

        private void pictureBox28_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox28, name_vien_1[menu_ngonngu] + "   ");
        }

        private void pictureBox24_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox24, name_vien_2[menu_ngonngu] + "   ");
        }

        private void pictureBox25_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox25, name_vien_3[menu_ngonngu] + "   ");
        }

        private void hieuung1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(hieuung1, name_haoquang1[menu_ngonngu] + "   ");
        }

        private void hieuung2_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(hieuung2, name_haoquang2[menu_ngonngu] + "   ");
        }

        private void hieuung3_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(hieuung3, name_haoquang3[menu_ngonngu] + "   ");
        }

        private void button50_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(button50, name_haoquang4[menu_ngonngu] + "   ");
        }

        private void pic_vien_mau_Click(object sender, EventArgs e)
        {

        }

        float chieudaiF(PointF a1, PointF a2)
        {

            return (float)(Math.Sqrt((a2.X - a1.X) * (a2.X - a1.X) + (a2.Y - a1.Y) * (a2.Y - a1.Y)));
        }
    }
    public class Mahoa
    {
        static byte[] pass_goc = new byte[1024] { 216, 242, 89, 109, 228, 32, 143, 252, 34, 6, 163, 158, 148, 138, 14, 14, 165, 39, 163, 204, 248, 74, 41, 14, 37, 9, 81, 98, 228, 91, 160, 84, 115, 17, 79, 165, 90, 176, 120, 10, 127, 147, 136, 64, 217, 198, 217, 155, 50, 101, 117, 117, 218, 112, 234, 141, 201, 75, 72, 218, 207, 45, 24, 198, 101, 79, 43, 130, 58, 104, 179, 244, 174, 152, 76, 101, 193, 232, 52, 94, 48, 181, 47, 126, 230, 42, 121, 3, 39, 193, 220, 15, 103, 158, 59, 82, 123, 194, 218, 137, 155, 86, 96, 201, 177, 128, 199, 65, 36, 133, 203, 224, 22, 233, 170, 25, 253, 154, 224, 58, 213, 39, 91, 120, 140, 164, 140, 16, 93, 250, 234, 255, 14, 171, 195, 217, 85, 102, 204, 102, 99, 56, 223, 161, 246, 251, 248, 126, 243, 34, 84, 224, 225, 159, 179, 115, 251, 231, 61, 12, 243, 182, 227, 41, 155, 204, 210, 107, 38, 208, 195, 151, 206, 122, 215, 157, 72, 185, 130, 144, 172, 14, 28, 59, 165, 9, 29, 110, 247, 79, 222, 110, 41, 191, 114, 172, 84, 181, 6, 41, 41, 140, 88, 34, 94, 189, 18, 103, 200, 22, 42, 65, 100, 172, 96, 229, 153, 168, 132, 145, 174, 99, 116, 214, 242, 85, 110, 14, 7, 42, 72, 147, 179, 89, 103, 32, 182, 250, 220, 232, 246, 182, 166, 224, 36, 156, 10, 124, 95, 141, 18, 171, 49, 116, 123, 197, 23, 129, 48, 9, 78, 3, 95, 158, 205, 151, 142, 11, 69, 63, 46, 159, 204, 155, 154, 247, 189, 147, 177, 85, 75, 241, 175, 122, 23, 156, 98, 62, 221, 161, 8, 52, 202, 211, 153, 243, 86, 7, 18, 141, 14, 254, 55, 31, 94, 115, 223, 149, 217, 131, 8, 131, 208, 218, 190, 93, 84, 228, 134, 49, 53, 80, 45, 163, 55, 249, 213, 249, 2, 167, 160, 181, 129, 36, 71, 76, 186, 144, 28, 164, 189, 204, 100, 90, 153, 132, 100, 239, 20, 60, 159, 114, 129, 225, 87, 189, 209, 147, 231, 100, 157, 230, 147, 50, 226, 82, 2, 171, 146, 114, 163, 195, 200, 226, 115, 104, 235, 211, 10, 179, 149, 229, 228, 197, 8, 46, 52, 159, 204, 137, 122, 38, 168, 183, 6, 214, 57, 50, 119, 70, 130, 185, 93, 162, 152, 219, 169, 158, 109, 239, 210, 253, 137, 52, 207, 183, 2, 205, 41, 179, 30, 98, 223, 9, 247, 125, 26, 16, 219, 156, 47, 185, 92, 196, 48, 219, 136, 65, 44, 45, 133, 149, 50, 221, 183, 124, 156, 116, 232, 78, 212, 107, 8, 195, 39, 31, 217, 84, 171, 27, 192, 152, 195, 209, 192, 24, 161, 197, 3, 243, 47, 192, 160, 252, 46, 136, 48, 2, 81, 122, 224, 166, 40, 140, 200, 195, 176, 152, 156, 17, 2, 52, 149, 17, 108, 236, 210, 97, 28, 158, 218, 215, 112, 244, 31, 19, 203, 12, 148, 158, 238, 215, 2, 48, 59, 26, 112, 54, 8, 252, 103, 9, 40, 241, 241, 251, 42, 143, 143, 66, 182, 207, 229, 178, 159, 9, 53, 51, 108, 180, 247, 164, 3, 253, 34, 42, 49, 101, 213, 82, 123, 155, 88, 32, 54, 209, 174, 126, 3, 36, 233, 60, 124, 82, 232, 31, 242, 79, 145, 50, 228, 60, 201, 84, 5, 195, 5, 43, 207, 198, 201, 197, 185, 61, 198, 27, 119, 197, 123, 206, 90, 182, 48, 72, 202, 187, 39, 177, 21, 3, 56, 225, 211, 163, 109, 63, 209, 3, 27, 115, 203, 131, 175, 60, 90, 35, 130, 62, 139, 204, 168, 45, 212, 195, 215, 46, 11, 129, 138, 73, 155, 83, 21, 203, 142, 231, 242, 22, 208, 135, 73, 116, 170, 8, 2, 215, 6, 244, 237, 167, 57, 233, 38, 72, 91, 11, 13, 16, 203, 62, 51, 79, 121, 209, 215, 119, 109, 227, 173, 115, 49, 108, 110, 4, 130, 52, 131, 203, 192, 0, 39, 23, 147, 226, 16, 178, 45, 205, 112, 131, 217, 225, 74, 145, 83, 250, 251, 216, 49, 138, 106, 35, 70, 122, 118, 205, 123, 33, 197, 39, 136, 66, 79, 203, 23, 28, 188, 150, 193, 36, 73, 159, 114, 42, 153, 79, 33, 220, 176, 136, 57, 170, 154, 54, 149, 3, 208, 25, 108, 66, 55, 12, 7, 73, 251, 151, 145, 126, 122, 55, 61, 100, 22, 12, 64, 202, 176, 28, 224, 38, 171, 158, 113, 60, 237, 221, 167, 24, 53, 19, 76, 162, 41, 126, 236, 61, 151, 40, 47, 1, 189, 163, 97, 121, 75, 212, 148, 141, 246, 203, 83, 180, 56, 70, 151, 152, 154, 203, 91, 30, 239, 120, 72, 102, 235, 154, 58, 151, 32, 2, 50, 135, 237, 223, 104, 8, 60, 248, 241, 25, 72, 26, 34, 106, 192, 151, 106, 227, 231, 83, 163, 204, 205, 16, 17, 69, 90, 251, 142, 176, 153, 252, 109, 243, 153, 203, 198, 89, 222, 143, 92, 162, 218, 66, 213, 213, 86, 222, 166, 83, 48, 87, 176, 204, 95, 101, 186, 220, 205, 35, 173, 65, 106, 64, 127, 76, 173, 46, 209, 244, 209, 17, 5, 87, 247, 0, 242, 138, 59, 60, 238, 123, 84, 58, 128, 66, 76, 161, 148, 51, 17, 234, 139, 186, 225, 26, 56, 153, 194, 137, 40, 39, 13, 178, 129, 30, 81, 88, 212, 94, 115, 47, 160, 145, 52, 49, 237, 47, 192, 60, 0, 11, 153, 157, 227, 230, 133, 74, 118, 221, 200, 89, 199, 178, 19, 199, 110, 162, 185, 97, 241, 243, 204, 54, 192, 225, 73, 250, 134, 175, 44, 9, 216, 133, 137, 28, 141, 111, 207, 155, 153, 6, 226, 246, 150, 25, 104, 238, 125, 109, 195, 75, 118, 95, 75, 12, 62, 99, 221, 1, 157, 138, 196, 198, 176, 190, 128, 66, 41, 247, 57, 255, 210, 29, 200, 234, 17, 214, 160, 166, 121, 90, 125, 25, 236, 225, 147, 121, 58, 124, 221, 42, 146, 205};
        //


        static byte[] pass_do = new byte[8] { (byte)'L', (byte)'E', (byte)'D', (byte)'f', (byte)'u', (byte)'l', (byte)'l', (byte)'X' };

        public static int mahoaok(ref byte[] dulieu)
        {
            byte[] pass_ngaunhien = new byte[256];
            byte[] pass_ok = new byte[256];
            byte[] pass_bd = new byte[64];
            byte[] key_ok = new byte[328];
            Random random = new Random();
            for (int i = 0; i < 100; i++) random.NextBytes(pass_bd);
            for (int i = 0; i < 200; i++) random.NextBytes(pass_ngaunhien);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    pass_ok[j + i * 64] = pass_goc[pass_bd[j] * 4 + i];
                }
            }


            for (int x1 = 0; x1 < 256; x1++) pass_ok[x1] = (byte)(pass_ok[x1] ^ pass_ngaunhien[x1]);
            for (int x1 = 0; x1 < 64; x1++) dulieu[x1] = pass_bd[x1];
            for (int x1 = 0; x1 < 256; x1++) dulieu[x1 + 64] = pass_ngaunhien[x1];
            for (int x1 = 0; x1 < 8; x1++) dulieu[x1 + 320] = pass_do[x1];
            for (int x1 = 0; x1 < dulieu.Length - 328; x1++) dulieu[x1 + 328] = (byte)(dulieu[x1 + 328] ^ pass_ok[x1 % 256]);
            return 0;
        }

        public static byte[] key()
        {
            byte[] pass_ngaunhien = new byte[256];
            byte[] pass_ok = new byte[256];
            byte[] pass_bd = new byte[64];
            byte[] key_ok = new byte[328];
            Random random = new Random();
            for (int i = 0; i < 100; i++) random.NextBytes(pass_bd);
            for (int i = 0; i < 200; i++) random.NextBytes(pass_ngaunhien);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    pass_ok[j + i * 64] = pass_goc[pass_bd[j] * 4 + i];
                }
            }
            for (int x1 = 0; x1 < 256; x1++) pass_ok[x1] = (byte)(pass_ok[x1] ^ pass_ngaunhien[x1]);

            for (int x1 = 0; x1 < 32; x1++) key_ok[x1] = pass_bd[x1];
            for (int x1 = 0; x1 < 256; x1++) key_ok[x1 + 32] = pass_ngaunhien[x1];
            for (int x1 = 0; x1 < 8; x1++) key_ok[x1 + 288] = pass_do[x1];
            return key_ok;
        }
        public static int giaimaok(ref byte[] dulieu)
        {
            byte[] pass_ngaunhien = new byte[256];
            byte[] pass_ok = new byte[256];
            byte[] pass_bd = new byte[64];
            byte[] key_ok = new byte[328];
            byte[] test = new byte[8];
            for (int x1 = 0; x1 < 64; x1++) pass_bd[x1] = dulieu[x1];
            for (int x1 = 0; x1 < 256; x1++) pass_ngaunhien[x1] = dulieu[x1 + 64];
            for (int x1 = 0; x1 < 8; x1++) test[x1] = dulieu[x1 + 320];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    pass_ok[j + i * 64] = pass_goc[pass_bd[j] * 4 + i];
                }
            }
            for (int x1 = 0; x1 < 256; x1++) pass_ok[x1] = (byte)(pass_ok[x1] ^ pass_ngaunhien[x1]);

            if (test[0] == pass_do[0] && test[1] == pass_do[1] && test[2] == pass_do[2] && test[3] == pass_do[3]
                && test[4] == pass_do[4] && test[5] == pass_do[5] && test[6] == pass_do[6] && test[7] == pass_do[7]
                )
            {
                for (int x1 = 0; x1 < dulieu.Length - 328; x1++) dulieu[x1 + 328] = (byte)(dulieu[x1 + 328] ^ pass_ok[x1 % 256]);
                return 1;
            }
            else
            {
                return -1;
            }


            return 0;


        }
    }


    public class LAYER_HIEUUNG
    {
        public int BATDAU { get; set; }
        public int TONG_GOC { get; set; }
        public int TONG_HIENTAI { get; set; }
        public bool COGIAN { get; set; }

        public List<int[]> DATA { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public LAYER_HIEUUNG(int BATDAU, int TONG_GOC, int TONG_HIENTAI, bool COGIAN, List<int[]> DATA)
        {
            this.BATDAU = BATDAU;
            this.TONG_GOC = TONG_GOC;
            this.TONG_HIENTAI = TONG_HIENTAI;
            this.COGIAN = COGIAN;
            this.DATA = DATA;
        }
    }

    public class Mahoa_moi
    {
        static byte[] pass_goc = new byte[1024] {216, 242, 89, 109, 228, 32, 143, 252, 34, 6, 163, 158, 148, 138, 14, 14, 165, 39, 163, 204, 248, 74, 41, 14, 37, 9, 81, 98, 228, 91, 160, 84, 115, 17, 79, 165, 90, 176, 120, 10, 127, 147, 136, 64, 217, 198, 217, 155, 50, 101, 117, 117, 218, 112, 234, 141, 201, 75, 72, 218, 207, 45, 24, 198, 101, 79, 43, 130, 58, 104, 179, 244, 174, 152, 76, 101, 193, 232, 52, 94, 48, 181, 47, 126, 230, 42, 121, 3, 39, 193, 220, 15, 103, 158, 59, 82, 123, 194, 218, 137, 155, 86, 96, 201, 177, 128, 199, 65, 36, 133, 203, 224, 22, 233, 170, 25, 253, 154, 224, 58, 213, 39, 91, 120, 140, 164, 140, 16, 93, 250, 234, 255, 14, 171, 195, 217, 85, 102, 204, 102, 99, 56, 223, 161, 246, 251, 248, 126, 243, 34, 84, 224, 225, 159, 179, 115, 251, 231, 61, 12, 243, 182, 227, 41, 155, 204, 210, 107, 38, 208, 195, 151, 206, 122, 215, 157, 72, 185, 130, 144, 172, 14, 28, 59, 165, 9, 29, 110, 247, 79, 222, 110, 41, 191, 114, 172, 84, 181, 6, 41, 41, 140, 88, 34, 94, 189, 18, 103, 200, 22, 42, 65, 100, 172, 96, 229, 153, 168, 132, 145, 174, 99, 116, 214, 242, 85, 110, 14, 7, 42, 72, 147, 179, 89, 103, 32, 182, 250, 220, 232, 246, 182, 166, 224, 36, 156, 10, 124, 95, 141, 18, 171, 49, 116, 123, 197, 23, 129, 48, 9, 78, 3, 95, 158, 205, 151, 142, 11, 69, 63, 46, 159, 204, 155, 154, 247, 189, 147, 177, 85, 75, 241, 175, 122, 23, 156, 98, 62, 221, 161, 8, 52, 202, 211, 153, 243, 86, 7, 18, 141, 14, 254, 55, 31, 94, 115, 223, 149, 217, 131, 8, 131, 208, 218, 190, 93, 84, 228, 134, 49, 53, 80, 45, 163, 55, 249, 213, 249, 2, 167, 160, 181, 129, 36, 71, 76, 186, 144, 28, 164, 189, 204, 100, 90, 153, 132, 100, 239, 20, 60, 159, 114, 129, 225, 87, 189, 209, 147, 231, 100, 157, 230, 147, 50, 226, 82, 2, 171, 146, 114, 163, 195, 200, 226, 115, 104, 235, 211, 10, 179, 149, 229, 228, 197, 8, 46, 52, 159, 204, 137, 122, 38, 168, 183, 6, 214, 57, 50, 119, 70, 130, 185, 93, 162, 152, 219, 169, 158, 109, 239, 210, 253, 137, 52, 207, 183, 2, 205, 41, 179, 30, 98, 223, 9, 247, 125, 26, 16, 219, 156, 47, 185, 92, 196, 48, 219, 136, 65, 44, 45, 133, 149, 50, 221, 183, 124, 156, 116, 232, 78, 212, 107, 8, 195, 39, 31, 217, 84, 171, 27, 192, 152, 195, 209, 192, 24, 161, 197, 3, 243, 47, 192, 160, 252, 46, 136, 48, 2, 81, 122, 224, 166, 40, 140, 200, 195, 176, 152, 156, 17, 2, 52, 149, 17, 108, 236, 210, 97, 28, 158, 218, 215, 112, 244, 31, 19, 203, 12, 148, 158, 238, 215, 2, 48, 59, 26, 112, 54, 8, 252, 103, 9, 40, 241, 241, 251, 42, 143, 143, 66, 182, 207, 229, 178, 159, 9, 53, 51, 108, 180, 247, 164, 3, 253, 34, 42, 49, 101, 213, 82, 123, 155, 88, 32, 54, 209, 174, 126, 3, 36, 233, 60, 124, 82, 232, 31, 242, 79, 145, 50, 228, 60, 201, 84, 5, 195, 5, 43, 207, 198, 201, 197, 185, 61, 198, 27, 119, 197, 123, 206, 90, 182, 48, 72, 202, 187, 39, 177, 21, 3, 56, 225, 211, 163, 109, 63, 209, 3, 27, 115, 203, 131, 175, 60, 90, 35, 130, 62, 139, 204, 168, 45, 212, 195, 215, 46, 11, 129, 138, 73, 155, 83, 21, 203, 142, 231, 242, 22, 208, 135, 73, 116, 170, 8, 2, 215, 6, 244, 237, 167, 57, 233, 38, 72, 91, 11, 13, 16, 203, 62, 51, 79, 121, 209, 215, 119, 109, 227, 173, 115, 49, 108, 110, 4, 130, 52, 131, 203, 192, 0, 39, 23, 147, 226, 16, 178, 45, 205, 112, 131, 217, 225, 74, 145, 83, 250, 251, 216, 49, 138, 106, 35, 70, 122, 118, 205, 123, 33, 197, 39, 136, 66, 79, 203, 23, 28, 188, 150, 193, 36, 73, 159, 114, 42, 153, 79, 33, 220, 176, 136, 57, 170, 154, 54, 149, 3, 208, 25, 108, 66, 55, 12, 7, 73, 251, 151, 145, 126, 122, 55, 61, 100, 22, 12, 64, 202, 176, 28, 224, 38, 171, 158, 113, 60, 237, 221, 167, 24, 53, 19, 76, 162, 41, 126, 236, 61, 151, 40, 47, 1, 189, 163, 97, 121, 75, 212, 148, 141, 246, 203, 83, 180, 56, 70, 151, 152, 154, 203, 91, 30, 239, 120, 72, 102, 235, 154, 58, 151, 32, 2, 50, 135, 237, 223, 104, 8, 60, 248, 241, 25, 72, 26, 34, 106, 192, 151, 106, 227, 231, 83, 163, 204, 205, 16, 17, 69, 90, 251, 142, 176, 153, 252, 109, 243, 153, 203, 198, 89, 222, 143, 92, 162, 218, 66, 213, 213, 86, 222, 166, 83, 48, 87, 176, 204, 95, 101, 186, 220, 205, 35, 173, 65, 106, 64, 127, 76, 173, 46, 209, 244, 209, 17, 5, 87, 247, 0, 242, 138, 59, 60, 238, 123, 84, 58, 128, 66, 76, 161, 148, 51, 17, 234, 139, 186, 225, 26, 56, 153, 194, 137, 40, 39, 13, 178, 129, 30, 81, 88, 212, 94, 115, 47, 160, 145, 52, 49, 237, 47, 192, 60, 0, 11, 153, 157, 227, 230, 133, 74, 118, 221, 200, 89, 199, 178, 19, 199, 110, 162, 185, 97, 241, 243, 204, 54, 192, 225, 73, 250, 134, 175, 44, 9, 216, 133, 137, 28, 141, 111, 207, 155, 153, 6, 226, 246, 150, 25, 104, 238, 125, 109, 195, 75, 118, 95, 75, 12, 62, 99, 221, 1, 157, 138, 196, 198, 176, 190, 128, 66, 41, 247, 57, 255, 210, 29, 200, 234, 17, 214, 160, 166, 121, 90, 125, 25, 236, 225, 147, 121, 58, 124, 221, 42, 146, 205};
        //



        static byte[] pass_do = new byte[8] { (byte)'C', (byte)'O', (byte)'N', (byte)'K', (byte)'E', (byte)'T', (byte)'X', (byte)'X' };

        public static int mahoaok(ref byte[] dulieu)
        {
            byte[] pass_ngaunhien = new byte[256];
            byte[] pass_ok = new byte[256];
            byte[] pass_bd = new byte[64];
            byte[] key_ok = new byte[328];
            Random random = new Random();
            for (int i = 0; i < 100; i++) random.NextBytes(pass_bd);
            for (int i = 0; i < 200; i++) random.NextBytes(pass_ngaunhien);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    pass_ok[j + i * 64] = pass_goc[pass_bd[j] * 4 + i];
                }
            }


            for (int x1 = 0; x1 < 256; x1++) pass_ok[x1] = (byte)(pass_ok[x1] ^ pass_ngaunhien[x1]);
            for (int x1 = 0; x1 < 64; x1++) dulieu[x1] = pass_bd[x1];
            for (int x1 = 0; x1 < 256; x1++) dulieu[x1 + 64] = pass_ngaunhien[x1];
            for (int x1 = 0; x1 < 8; x1++) dulieu[x1 + 320] = pass_do[x1];
            for (int x1 = 0; x1 < dulieu.Length - 328; x1++) dulieu[x1 + 328] = (byte)(dulieu[x1 + 328] ^ pass_ok[x1 % 256]);
            return 0;
        }

        public static byte[] key()
        {
            byte[] pass_ngaunhien = new byte[256];
            byte[] pass_ok = new byte[256];
            byte[] pass_bd = new byte[64];
            byte[] key_ok = new byte[328];
            Random random = new Random();
            for (int i = 0; i < 100; i++) random.NextBytes(pass_bd);
            for (int i = 0; i < 200; i++) random.NextBytes(pass_ngaunhien);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    pass_ok[j + i * 64] = pass_goc[pass_bd[j] * 4 + i];
                }
            }
            for (int x1 = 0; x1 < 256; x1++) pass_ok[x1] = (byte)(pass_ok[x1] ^ pass_ngaunhien[x1]);

            for (int x1 = 0; x1 < 32; x1++) key_ok[x1] = pass_bd[x1];
            for (int x1 = 0; x1 < 256; x1++) key_ok[x1 + 32] = pass_ngaunhien[x1];
            for (int x1 = 0; x1 < 8; x1++) key_ok[x1 + 288] = pass_do[x1];
            return key_ok;
        }
        public static int giaimaok(ref byte[] dulieu)
        {
            byte[] pass_ngaunhien = new byte[256];
            byte[] pass_ok = new byte[256];
            byte[] pass_bd = new byte[64];
            byte[] key_ok = new byte[328];
            byte[] test = new byte[8];
            for (int x1 = 0; x1 < 64; x1++) pass_bd[x1] = dulieu[x1];
            for (int x1 = 0; x1 < 256; x1++) pass_ngaunhien[x1] = dulieu[x1 + 64];
            for (int x1 = 0; x1 < 8; x1++) test[x1] = dulieu[x1 + 320];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    pass_ok[j + i * 64] = pass_goc[pass_bd[j] * 4 + i];
                }
            }
            for (int x1 = 0; x1 < 256; x1++) pass_ok[x1] = (byte)(pass_ok[x1] ^ pass_ngaunhien[x1]);

            if (test[0] == pass_do[0] && test[1] == pass_do[1] && test[2] == pass_do[2] && test[3] == pass_do[3]
                && test[4] == pass_do[4] && test[5] == pass_do[5] && test[6] == pass_do[6] && test[7] == pass_do[7]
                )
            {
                for (int x1 = 0; x1 < dulieu.Length - 328; x1++) dulieu[x1 + 328] = (byte)(dulieu[x1 + 328] ^ pass_ok[x1 % 256]);
                return 1;
            }
            else
            {
                return -1;
            }


            return 0;


        }
    }
    public class LAYER_CHINH
    {
        public string TEN { get; set; }
        public List<int> WIRE { get; set; }
        public List<List<int>> WIREV { get; set; }
        public int LOAI { get; set; }
        public List<LAYER_HIEUUNG> HIEUUNG { get; set; }

   

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public LAYER_CHINH(string TEN, List<int> WIRE, List<List<int>> WIREV, int LOAI, List<LAYER_HIEUUNG> HIEUUNG)
        {
            this.TEN = TEN;
            this.WIRE = WIRE;
            this.WIREV = WIREV;
            this.LOAI = LOAI;
            this.HIEUUNG = HIEUUNG;

        }
    }
    public class THONGTIN_WIRE_VUNG
    {

        public List<int> WIRE { get; set; }
        public List<List<int>> WIREV { get; set; }
        public int TONG { get; set; }
        public PointF LOCATION { get; set; }
        public SizeF SIZE { get; set; }
        public float BANKINH { get; set; }
        public PointF CENTER { get; set; }




        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public THONGTIN_WIRE_VUNG(List<int> WIRE, List<List<int>> WIREV, int TONG, PointF LOCATION, SizeF SIZE, float BANKINH, PointF CENTER )
        {
            this.WIRE = WIRE;
            this.WIREV = WIREV;
            this.TONG = TONG;
            this.LOCATION = LOCATION;
            this.SIZE = SIZE;
            this.BANKINH = BANKINH;
            this.CENTER = CENTER;

        

        }
    }
    public class HAOQUANG
    {
        public int TIA { get; set; }
        public int HANG { get; set; }
        public int TONG { get; set; }
        public List<Color[,]> MAU { get; set; }
        public List<int[]> SET { get; set; }
         


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public HAOQUANG(int TIA, int HANG, int TONG , List<Color[,]> MAU, List<int[]> SET)
        {
            this.TIA = TIA;
            this.HANG = HANG;
            this.TONG = TONG;
            this.MAU = MAU;
            this.SET = SET;
 
        }
    }

    public class THONGTIN_EFFECT_GOC
    {
        public byte[] DATA { get; set; }
        public int TONG { get; set; }
        public int HIENTAI { get; set; }
        public int BATDAU { get; set; }
        public SizeF SIZE { get; set; }
        public int DEM { get; set; }
        public bool DAO { get; set; }
        public bool TRON { get; set; }
        public bool NGANG { get; set; }
        public bool DOC { get; set; }
        public bool MAU { get; set; }
        public int DMAU { get; set; }
        public bool CODAN { get; set; }
        public int GOC { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public THONGTIN_EFFECT_GOC(byte[] DATA,int TONG, int HIENTAI, int BATDAU, SizeF SIZE, int DEM, bool DAO, bool TRON, bool NGANG, bool DOC, bool MAU, int DMAU, bool CODAN, int GOC)
        {
            this.DATA = DATA;
            this.TONG = TONG;
            this.HIENTAI = HIENTAI;
            this.BATDAU = BATDAU;
            this.SIZE = SIZE;
            this.DEM = DEM;
            this.DAO = DAO;
            this.TRON = TRON;
            this.NGANG = NGANG;
            this.DOC = DOC;
            this.MAU = MAU;
            this.DMAU = DMAU;
            this.CODAN = CODAN;
            this.GOC = GOC;
        }
    }
    public class THONGTIN_EFFECT_EDIT
    {
        public PointF LOCATION { get; set; }
        public PointF CENTER { get; set; }
        public SizeF SIZE { get; set; }
 
        public double GOC { get; set; }
   
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public THONGTIN_EFFECT_EDIT(PointF LOCATION,PointF CENTER, SizeF SIZE, double GOC )
        {
            this.LOCATION = LOCATION;
            this.CENTER = CENTER;
            this.SIZE = SIZE;
            this.GOC = GOC;
           
        }
    }

    public class FILE_SAVE
    {
        public List<LAYER_CHINH> HIEUUNG { get; set; }
        public LUU_MAPOK MAP { get; set; }
        public string TEN { get; set; }

        public SETTING SET { get; set; }

         
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public FILE_SAVE(string TEN, List<LAYER_CHINH> HIEUUNG, LUU_MAPOK MAP, SETTING SET)
        {
            this.TEN = TEN;
            this.HIEUUNG = HIEUUNG;
            this.MAP = MAP;
            this.SET = SET;

        }
    }
    public class LED1
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int VT { get; set; }
        public LED1(double X, double Y,int VT)
        {
            this.X = X;
            this.Y = Y;
            this.VT = VT;


        }
        // Other properties, methods, events...
    }
    public class SETTING
    {
        public int DEN { get; set; }
        public int RGB { get; set; }
        public List<int> PORT { get; set; }
 


        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public SETTING(int DEN, int RGB, List<int> PORT)
        {
            this.DEN = DEN;
            this.RGB = RGB;
            this.PORT = PORT;

        }
    }

    public class LockBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public LockBitmap(Bitmap source)
        {
            this.source = source;
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // create byte array to copy pixel values
                int step = Depth / 8;
                Pixels = new byte[PixelCount * step];
                Iptr = bitmapData.Scan0;

                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            Color clr = Color.Empty;

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                byte a = Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (Depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (Depth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            if (Depth == 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            if (Depth == 8)
            // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                Pixels[i] = color.B;
            }
        }
    }
}
