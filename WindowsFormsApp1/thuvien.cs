using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Accord.Video.FFMPEG;




namespace WindowsFormsApp1
{
    public partial class thuvien : Form
    {
        static thuvien hieuung;

        static string ten = "";
        string path_hieuung = System.IO.Directory.GetCurrentDirectory().ToString() + "\\hieuung\\";
        private List<AnimatedGifFrame> mImages = new List<AnimatedGifFrame>();
        public List<AnimatedGifFrame> Images { get { return mImages; } }
        public void AnimatedGif(string path)
        {

            Image img = Image.FromFile(path);
            int frames = img.GetFrameCount(FrameDimension.Time);
            // if (frames <= 1)
            // {
            //     System.Windows.Forms.MessageBox.Show("Thư viện lỗi!");
            // }
            // else
            if (frames > 0)
            {
                byte[] times = img.GetPropertyItem(0x5100).Value;
                int frame = 0;
                for (; ; )
                {
                    System.Windows.Forms.Application.DoEvents();
                    int dur = BitConverter.ToInt32(times, 4 * frame);
                    mImages.Add(new AnimatedGifFrame(new Bitmap(img), dur));
                    if (++frame >= frames) break;
                    img.SelectActiveFrame(FrameDimension.Time, frame);
                }
            }
            img.Dispose();

        }
        public static string ShowBox(string path,int ngonngu)
        {
            hieuung = new thuvien();
            hieuung.path_hieuung = path;
            if (ngonngu == 0)
            {

                // hieuung.label1.Text = te;
                // hieuung.button1.Text = "Chấp nhận";
                hieuung.label1.Text = "Tên";
                hieuung.aaaa1 = "Chọn tên khác!";
                hieuung.aaaa2 = "Tên hiệu ứng";

                hieuung.button49.Text = "Mở ảnh";
                hieuung.button48.Text = "Mở ảnh động";
                hieuung.button50.Text = "Mở phim";
                hieuung.tenhieung.Text = "Tên";
                hieuung.button47.Text = "Lưu";
            }
            else
            {
                hieuung.label1.Text = "Name";
                hieuung.aaaa1 = "Choose another name!";
                hieuung.aaaa2 = "Map name";
                hieuung.button49.Text = "Add picture";
                hieuung.button48.Text = "Add gif";
                hieuung.button50.Text = "Add video";
                hieuung.tenhieung.Text = "Name";
                hieuung.button47.Text = "Save";
                // hieuung.label18.Text = "Info";
                // hieuung.label1.Text = te;
                // hieuung.button1.Text = "OK";

            }


            hieuung.ShowDialog();

            return ten;
        }

        string aaaa1 = "Chọn tên khác!";
        string aaaa2 = "Tên sơ đồ";

        public thuvien()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private List<string> anh = new List<string>();
        private void button49_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "Picture ( *.bmp;*.jpg;*.png;)| *.bmp;*.jpg;*.png;";

            if (open.ShowDialog() == DialogResult.OK)
            {
                timer1.Enabled = false;
                button48.Enabled = false;
                button49.Enabled = false;
                button50.Enabled = false;
                button47.Enabled = false;

                byte r, g, b, index;

                anh.Clear();
                foreach (String file in open.FileNames)
                {
                            anh.Add(file);

                }



                if (anh.Count > 0)
                {
                   

                        //  anh.Sort((a, b) => new StringNum(a).CompareTo(new StringNum(b)));
                        int tongchay = anh.Count;
                      progressBar1.Value = 0;
                      progressBar1.Maximum = tongchay;
                         dulieu = new byte[120 * 100 * 4 * tongchay + 12 + 328];
                        Byte[] a1 = BitConverter.GetBytes(tongchay);
                        Byte[] a2 = BitConverter.GetBytes(120);
                        Byte[] a3 = BitConverter.GetBytes(100);
                        dulieu[328 + 0] = a1[0];
                        dulieu[328 + 1] = a1[1];
                        dulieu[328 + 2] = a1[2];
                        dulieu[328 + 3] = a1[3];
                        dulieu[328 + 4] = a2[0];
                        dulieu[328 + 5] = a2[1];
                        dulieu[328 + 6] = a2[2];
                        dulieu[328 + 7] = a2[3];
                        dulieu[328 + 8] = a3[0];
                        dulieu[328 + 9] = a3[1];
                        dulieu[328 + 10] = a3[2];
                        dulieu[328 + 11] = a3[3];
                        for (int m = 0; m < tongchay; m++)
                        {



                        bmp_render = (Bitmap)ResizeImage(Image.FromFile(anh[m]), 120, 100);
                            pictureBox1.Image = bmp_render;

                            for (int x = 0; x < 120; x++)
                            {
                                for (int y = 0; y < 100; y++)
                                {
                                    Color c = bmp_render.GetPixel(x, y);
                                    r = (byte)gamma8[c.R];
                                    g = (byte)gamma8[c.G];
                                    b = (byte)gamma8[c.B];
                                    if (r <= 5 && g <= 5 && b <= 5)
                                    {
                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 0] = 0;
                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 1] = 0;
                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 2] = 0;
                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 3] = 0;
                                    }
                                    else
                                    {

                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 0] = 255;
                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 1] = r;
                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 2] = g;
                                        dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 3] = b;
                                    }



                                }
                            }

                        progressBar1.Value++;

                            Application.DoEvents();
                        }

                       
                    
                }


                button48.Enabled = true;
                button49.Enabled = true;
                button50.Enabled = true;
                button47.Enabled = true;

                button47.Enabled = true;
                timer1.Enabled = true;
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

        byte[] dulieu;
        private void button48_Click(object sender, EventArgs e)
        {
           


            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            //  open.Filter = "video ( *.avi;*.wmv;*.mp4;*.flv;*.swf)|*.avi;*.wmv;*.mp4;*.flv;*.swf";
            open.Filter = "Thư viện (*.gif)|*.gif";
            if (open.ShowDialog() == DialogResult.OK)
            {
                timer1.Enabled = false;
                button48.Enabled = false;
                button49.Enabled = false;
                button50.Enabled = false;
                button47.Enabled = false;

                byte r, g, b, index;

                mImages.Clear();
                AnimatedGif(open.FileName);

                int tongchay = mImages.Count;
                progressBar1.Value = 0;
                progressBar1.Maximum = mImages.Count;
                 dulieu = new byte[120 * 100 * 4 * tongchay + 12 + 328];
                Byte[] a1 = BitConverter.GetBytes(tongchay);
                Byte[] a2 = BitConverter.GetBytes(120);
                Byte[] a3 = BitConverter.GetBytes(100);
                dulieu[328 + 0] = a1[0];
                dulieu[328 + 1] = a1[1];
                dulieu[328 + 2] = a1[2];
                dulieu[328 + 3] = a1[3];
                dulieu[328 + 4] = a2[0];
                dulieu[328 + 5] = a2[1];
                dulieu[328 + 6] = a2[2];
                dulieu[328 + 7] = a2[3];
                dulieu[328 + 8] = a3[0];
                dulieu[328 + 9] = a3[1];
                dulieu[328 + 10] = a3[2];
                dulieu[328 + 11] = a3[3];
                //MessageBox.Show(tongchay.ToString());
                for (int m = 0; m < tongchay; m++)
                {
                  
                   bmp_render = (Bitmap)ResizeImage(mImages[m].Image, 120, 100);
                    pictureBox1.Image = bmp_render;
                    progressBar1.Value++;

                    for (int x = 0; x < 120; x++)
                    {
                        for (int y = 0; y < 100; y++)
                        {
                            Color c = bmp_render.GetPixel(x, y);
                            r = (byte)gamma8[c.R];
                            g = (byte)gamma8[c.G];
                            b = (byte)gamma8[c.B];
                            if (r <= 5 && g <= 5 && b <= 5)
                            {
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 0] = 0;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 1] = 0;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 2] = 0;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 3] = 0;
                            }
                            else
                            {

                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 0] = 255;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 1] = r;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 2] = g;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 3] = b;
                            }



                        }
                    }

                    Application.DoEvents();


                }

                button48.Enabled = true;
                button49.Enabled = true;
                button50.Enabled = true;
                button47.Enabled = true;

                button47.Enabled = true;
                timer1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ten = "";
            hieuung.Dispose();
        }

        private void button47_Click(object sender, EventArgs e)
        {
            if (File.Exists(path_hieuung + tenhieung.Text + ".eff"))
            {
                tenhieung.Text = aaaa1;
            }
            else
            {
                if (tenhieung.Text != aaaa1 && tenhieung.Text != aaaa2)
                {

                    Mahoa_moi.mahoaok(ref dulieu);
                    FileStream fWrite = new FileStream(path_hieuung + tenhieung.Text + ".eff", FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.None);
                    fWrite.Write(dulieu, 0, dulieu.Length);
                    fWrite.Close();

                    ten = "ok";
                    hieuung.Dispose();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        Bitmap bmp_render;
        private void button50_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Filter = "video ( *.avi;*.wmv;*.mp4;)|*.avi;*.wmv;*.mp4;";
           
            if (open.ShowDialog() == DialogResult.OK)
            {
                timer1.Enabled = false;
                button48.Enabled = false;
                button49.Enabled = false;
                button50.Enabled = false;
                button47.Enabled = false;

                byte r, g, b, index;

                VideoFileReader reader = new VideoFileReader();
                reader.Open(open.FileName);
                int tongchay = (int)reader.FrameCount;

               
                progressBar1.Value = 0;
                progressBar1.Maximum = tongchay;
                dulieu = new byte[120 * 100 * 4 * tongchay + 12 + 328];
                Byte[] a1 = BitConverter.GetBytes(tongchay);
                Byte[] a2 = BitConverter.GetBytes(120);
                Byte[] a3 = BitConverter.GetBytes(100);
                dulieu[328 + 0] = a1[0];
                dulieu[328 + 1] = a1[1];
                dulieu[328 + 2] = a1[2];
                dulieu[328 + 3] = a1[3];
                dulieu[328 + 4] = a2[0];
                dulieu[328 + 5] = a2[1];
                dulieu[328 + 6] = a2[2];
                dulieu[328 + 7] = a2[3];
                dulieu[328 + 8] = a3[0];
                dulieu[328 + 9] = a3[1];
                dulieu[328 + 10] = a3[2];
                dulieu[328 + 11] = a3[3];
                //MessageBox.Show(tongchay.ToString());
                for (int m = 0; m < tongchay; m++)
                {
                    bmp_render = reader.ReadVideoFrame();

                    bmp_render = (Bitmap)ResizeImage(bmp_render, 120, 100);
                    pictureBox1.Image = bmp_render;
                    progressBar1.Value++;

                    for (int x = 0; x < 120; x++)
                    {
                        for (int y = 0; y < 100; y++)
                        {
                            Color c = bmp_render.GetPixel(x, y);
                            r = (byte)gamma8[c.R];
                            g = (byte)gamma8[c.G];
                            b = (byte)gamma8[c.B];
                            if (r <= 5 && g <= 5 && b <= 5)
                            {
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 0] = 0;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 1] = 0;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 2] = 0;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 3] = 0;
                            }
                            else
                            {

                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 0] = 255;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 1] = r;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 2] = g;
                                dulieu[328 + 12 + m * 120 * 100 * 4 + x * 100 * 4 + y * 4 + 3] = b;
                            }



                        }
                    }

                    Application.DoEvents();


                }
                reader.Close();
                button48.Enabled = true;
                button49.Enabled = true;
                button50.Enabled = true;
                button47.Enabled = true;

                button47.Enabled = true;
                timer1.Enabled = true;
            }
        }
    }
    public class AnimatedGifFrame
    {
        private int mDuration;
        private Image mImage;
        internal AnimatedGifFrame(Image img, int duration)
        {
            mImage = img; mDuration = duration;
        }
        public Image Image { get { return mImage; } }
        public int Duration { get { return mDuration; } }


    }
}
