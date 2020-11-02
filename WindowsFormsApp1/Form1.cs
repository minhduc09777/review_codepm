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


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            EnableDoubleBuffering();
            SetDoubleBuffered(this);
            SetDoubleBuffered(this.pic_map);
        }
        int menu_select = 0;
        int menu_draw = 1;
        int menu_wire = 2;
        int menu_tool = 3;
        int menu_redraw = 4;
        int menu_undo= 5;
        int menu_zoomout = 6;
        int menu_zoomin = 7;
        int menu_error = 8;
        int menu_dxf = 9;
        int menu_map =10;
        int menu_save = 11;
        int menu_open = 12;
        int menu_youtube = 13;
        int menu_languala= 14;
        int menu_ok = 15;
        int menu_ok1 = 16;

        int vt_menu = 0;
        int vt_menu1 = 0;
        int menu_ngonngu = 0;
        bool nhay = true;
        bool nhay_map = true;

        
        ///////////////////// 

        int menu_select_copy =0;
        int menu_select_delete = 1;
        int menu_select_align0 = 2;
        int menu_select_align1 = 3;
        int menu_select_align2 = 4;
        int menu_select_align3 = 5;
        int menu_select_align4 = 6;
        int menu_select_align5 = 7;
        int menu_select_save = 8;
        ///////////////////// 
        int menu_draw_led = 0;
        int menu_draw_insert = 1;
        int menu_draw_delete = 2;
        int menu_draw_line = 3;
        int menu_draw_border = 4;
        int menu_draw_halo = 5;
        int menu_draw_matrix = 6;
        int menu_draw_text = 7;
        int menu_draw_dxf = 8;

        ///////////////////// 
        int menu_wire_connect0 = 0;
        int menu_wire_connect1 = 1;
        int menu_wire_port1 = 2;
        int menu_wire_port2 = 3;
        int menu_wire_rotator1 = 4;
        int menu_wire_rotator2 = 5;


        ///////////////////// 
        int menu_tool_move = 0;
        int menu_tool_open = 1;
        int menu_tool_delete = 2;
        int menu_tool_dim = 3;
 
        /////////////////////////////////
        string path_wire = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\wire\\";
        List<LED> led_full = new List<LED>();
        List<LED> led_add = new List<LED>();
      
        List<int> led_select = new List<int>();
        List<int> led_error = new List<int>();
        List<LED[]> led_undo = new List<LED[]>();
        List<int> led_undo_port = new List<int>();



        string[,] ngonngu_menu = new string[,]
        {
            { "Chọn led","Vẽ led","Đi dây","Công cụ","Căn vùng làm việc (R)","Lùi thao tác (Ctrl+Z)","Phóng to (lăn chuột)","Thu nhỏ (lăn chuột)","Kiểm tra led","Xuất file *.dxf","Xuất file in sơ đồ","Lưu dự án","Mở dự án","Kênh hướng dẫn","Đổi ngôn ngữ","Chấp nhận vào soạn hiệu ứng","Chấp nhận vào soạn hiệu ứng" },
            { "Select led","Draw led","Edit wire","Tool","Align (R)","Undo (Ctrl+Z)","Zoom out (Mouse wheel)","Zoom in (Mouse wheel)","Check error","Save file *.dxf","Save map wire","Save","Open","Tutorial","Language","OK & Enter effect","OK & Enter effect"  }
        };
        string[,] ngonngu_select = new string[,]
        {
            { "Sao chép vùng led (Ctrl+C)","xóa vùng led (Del)","Căn ngang vùng led","Căn dọc vùng led","Căn điều dọc vùng led","Căn điều ngang vùng led","Lật ngang vùng led","Lật dọc vùng led","Lưu mẫu vùng led"},
           { "Copy (Ctrl+C)","Delete (Del)","Align middles","Align center","Make vertical spacing equal","Make horizontal spacing equal","Flip canvas vertical","Flip canvas horizontal","Save library"}

        };
        string[,] ngonngu_draw = new string[,]
{
            { "Thêm led","Chèn led","Xóa led","Vẽ led kiểu đường thẳng","Vẽ led kiểu viền","Vẽ led kiểu hào quang","Vẽ led kiểu ma trận","Vẽ led kiểu chữ","Vẽ led từ thư viện hoặc file *.dxf"},
             { "Add led","Insert led","Delete led","Draw line led","Draw border led","Draw halo led","Draw matrix led","Draw text led","Draw led from library or *.dxf file"},

};
        string[,] ngonngu_wire= new string[,]
{
           { "Nối kiểu kéo qua từng led","Nối kiểu kéo đường thẳng qua led","Thiết lập đầu vào cổng 1","Thiết lập đầu vào cổng 2","Đảo chiều dây cổng 1","Đảo chiều dây cổng 2"},
             { "Matching point type","Matching line type","Set port 1","Set port 2","Reverse port 1","Reverse port 2"}

};
        string[,] ngonngu_tool = new string[,]
{
           { "Chọn và chỉnh sửa ảnh","Thêm ảnh","Xóa ảnh đã chọn","làm mờ ảnh đã chọn"},
           { "Select & Edit picture","Add picture","Delete picture","Dimm picture"},

};
        float zoom = 1F;
        float W0 = 9984;
        float H0 = 9984;
        float X0 = 4992;
        float Y0 = 4992;
        int startx = 0;                         // offset of image when mouse was pressed
        int starty = 0;
        int imgx = -4992;                         // current offset of image
        int imgy = -4992;
        Point mouseDown;
        float vt_themx = 0;
        float vt_themy = 0;
        PointF vt_chuot = new PointF(-1, -1);
        PointF vt_chuot_anh = new PointF(-1, -1);
        bool mousepressed = false;
        int diem_select_chen = -1;
        int diem_select_xoa = -1;
        int vt_port2 = -1;
        PointF vt_chen = new PointF(-1, -1);
        private void pic_menu_Paint(object sender, PaintEventArgs e)
        {
            if (nhay == true)
            {
                if (vt_menu >= 0 && vt_menu<=3)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), 2 + vt_menu * 49, 2, 46, 46);
                }

               // if (led_full.Count > 0) { e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2 + menu_redraw * 49, 44, 46, 2); e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2 + menu_save * 49, 44, 46, 2); }
               // if (led_undo.Count > 0) e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2 + menu_undo* 49, 44, 46, 2);

               // if (zoom < 3) e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2 + menu_zoomout * 49, 44, 46, 2);
               // if (zoom >0.2F) e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2 + menu_zoomin * 49, 44, 46, 2);
            }

        }
        int dem_loi = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            label1.Text = led_full.Count.ToString("0000") + " LED";
            if (nhay == false) nhay = true;
            else nhay = false;
            pic_menu.Refresh();
            pic_menu1.Refresh();

            if (vt_menu == menu_draw && vt_menu1 == menu_draw_halo) pic_halo.Refresh();
            if (vt_menu == menu_draw && vt_menu1 == menu_draw_matrix) pic_matrix.Refresh();
            if (vt_menu == menu_select)
            {
                /*
                if(led_full.Count<1000)
                {
                    dem_loi++;
                    if (dem_loi >= 5)
                    {
                        dem_loi = 0;
                        // checkloi();
                    }
                }else if (led_full.Count < 3000)
                {
                    dem_loi++;
                    if (dem_loi >= 10)
                    {
                        dem_loi = 0;
                        // checkloi();
                    }
                }
                else if (led_full.Count < 5000)
                {
                    dem_loi++;
                    if (dem_loi >= 20)
                    {
                        dem_loi = 0;
                        // checkloi();
                    }
                }
                else if (led_full.Count < 7000)
                {
                    dem_loi++;
                    if (dem_loi >= 50)
                    {
                        dem_loi = 0;
                        // checkloi();
                    }
                }
                else if (led_full.Count < 10000)
                {
                    dem_loi++;
                    if (dem_loi >= 100)
                    {
                        dem_loi = 0;
                        // checkloi();
                    }
                }
                else  
                {
                    dem_loi++;
                    if (dem_loi >= 500)
                    {
                        dem_loi = 0;
                        // checkloi();
                    }
                }
                */


            }
        }
        private void set_ngongu()
        {





            if (menu_ngonngu == 1)
            {
                pic_menu.Image = Properties.Resources.menu1;
                dan0.Text = dan1.Text = dan2.Text = dan3.Text = dan4.Text = dan5.Text = dan6.Text = "Setting";
                khoa_day.Text = "Lock";
                label13.Text = "Column";
                label6.Text = "Row";
                label14.Text = "Bore";
                label25.Text = "Column";
                label24.Text = "Row";
                label10.Text = "Size";
                label15.Text = "Distance";
                label4.Text = "Column";
                label12.Text = "Row";
                button1.Text = "Add *.dxf";
                combo_vien.Items[0] = "Round border";
                combo_vien.Items[1] = "Rectangular border";
            }
            else
            {
                pic_menu.Image = Properties.Resources.menu;

                dan0.Text = dan1.Text = dan2.Text = dan3.Text = dan4.Text = dan5.Text = dan6.Text = "Thông số";
                khoa_day.Text = "Khóa";
                label13.Text = "Cột";
                label6.Text = "Hàng";
                label14.Text = "Ruột";
                label25.Text = "Cột";
                label24.Text = "Hàng";
                label10.Text = "Cỡ chữ";
                label15.Text = "Dây";
                label4.Text = "Cột";
                label12.Text = "Hàng";
                button1.Text = "Thêm *.dxf";
                combo_vien.Items[0] = "Viền tròn";
                combo_vien.Items[1] = "Viền chữ nhật";
            }

        }
        private void pic_menu_MouseDown(object sender, MouseEventArgs e)
        {





            int vv = e.X / 49;



            if (vv <= 3)
            {

                panel_draw_led.Visible = false;
                panel_draw_line.Visible = false;
                panel_draw_border.Visible = false;
                panel_draw_halo.Visible = false;
                panel_draw_matrix.Visible = false;
                panel_draw_text.Visible = false;
                panel_draw_dxf.Visible = false;
                vt_themx = -imgx;
                vt_themy = -imgy;
                vt_chuot = new PointF(0, 0);
                vt_menu = vv;

                if (vt_menu == menu_select) pic_menu1.Image = Properties.Resources.select;
                else if (vt_menu == menu_draw) { pic_menu1.Image = Properties.Resources.draw; panel_draw_led.Visible = true; }
                else if (vt_menu == menu_wire) pic_menu1.Image = Properties.Resources.wire;
                else if (vt_menu == menu_tool) pic_menu1.Image = Properties.Resources.tool;
                vt_menu1 = 0;
            }
            else if (vv == menu_redraw)
            {

                reset_moitruong();
                vt_themx = -imgx;
                vt_themy = -imgy;
                vt_chuot = new PointF(0, 0);
                vt_menu = menu_select;

                pic_menu1.Image = Properties.Resources.select;

                vt_menu1 = 0;
            }

            else if (vv == menu_undo)
            {

                undo();

            }
            else if (vv == menu_zoomout)
            {

                if (zoom < 3)
                {

                    Application.DoEvents();
                    Thread.Sleep(5);

                    zoom += 0.1F;


                }

            }
            else if (vv == menu_zoomin)
            {

                if (zoom > 0.2F)
                {

                    Application.DoEvents();
                    Thread.Sleep(5);

                    zoom -= 0.1F;


                }

            }
            else if (vv == menu_error)
            {
                checkloi();

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
                if (vt_port2 < 0) { string ten = thongtin.ShowBox(menu_ngonngu, led_error.Count, (int)thongtin_ban[2], (int)thongtin_ban[3], led_full.Count, 0); }
                else { string ten = thongtin.ShowBox(menu_ngonngu, led_error.Count, (int)thongtin_ban[2], (int)thongtin_ban[3], vt_port2, led_full.Count - vt_port2); }

            }

            else if (vv == menu_dxf)
            {
                if (led_full.Count > 0)
                {

                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                    saveFileDialog1.Filter = "File Cắt lỗ Autocad (Corel) (*.dxf)|*.dxf";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                       

                        double ppp = 0;
                        double qq = 0;

                        for (int x = 0; x < led_full.Count; x++)
                        {
                            if (led_full[x].Y > ppp) ppp = led_full[x].Y;
                        }

                        for (int x = 0; x < led_full.Count; x++)
                        {

                            qq = ppp - (double)led_full[x].Y;



                            
                        }

                       

                    }
                    checkloi();
                }
            }
            else if (vv == menu_map)
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
            else if (vv == menu_youtube)
            {
                if (File.Exists(System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\link_support.txt"))
                {
                    string text = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\link_support.txt");

                    System.Diagnostics.Process.Start(text);
                }


            }
            else if (vv == menu_languala)
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
            else if (vv == menu_save)
            {
                if (led_full.Count > 0)
                {
                    timer.Enabled = false;
                    timer_map.Enabled = false;
                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                    // saveFileDialog1.Filter = "Dự án LEDFULLmini (  *.full)|*.full|File nạp LEDFULLmini(  *.mini)|*.mini";
                    saveFileDialog1.Filter = "File Wire (  *.wri)|*.wri";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        List<ANH_ANHL> anhl = new List<ANH_ANHL>();
                        if (list_anh.Count > 0)
                        {
                            for (int x = 0; x < list_anh.Count; x++) anhl.Add(new ANH_ANHL(ImageToByte2(list_anh[x].ANH), list_anh[x].CENTER, list_anh[x].SIZE, list_anh[x].GOC, list_anh[x].DIM));
                        }
                        LUU_MAPOK data_l = new LUU_MAPOK(led_full, anhl, vt_port2);

                        var rmCrypto = GetAlgorithm();
                        ICryptoTransform encryptor = rmCrypto.CreateEncryptor();

                        using (var writer = new StreamWriter(new CryptoStream(System.IO.File.Create(saveFileDialog1.FileName), encryptor, CryptoStreamMode.Write)))
                        {
                            writer.Write(JsonConvert.SerializeObject(data_l));

                        }
                    }
                    timer.Enabled = true;
                    timer_map.Enabled = true;
                }

            }
            else if (vv == menu_open)
            {
                timer.Enabled = false;
                timer_map.Enabled = false;
                System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
                open.Filter = "File Wire (  *.wri)|*.wri";
                if (open.ShowDialog() == DialogResult.OK)
                {

                    var rmCrypto = GetAlgorithm();

                    ICryptoTransform decryptor = rmCrypto.CreateDecryptor();
                    try
                    {
                        using (var reader = new StreamReader(new CryptoStream(System.IO.File.OpenRead(open.FileName), decryptor, CryptoStreamMode.Read)))
                        {
                            led_add.Clear();
                            led_select.Clear();
                            led_error.Clear();
                            led_undo.Clear();
                            LUU_MAPOK temp1 = JsonConvert.DeserializeObject<LUU_MAPOK>(reader.ReadToEnd());

                            led_full = temp1.DATA;
                            vt_port2 = temp1.PORT;
                            list_anh.Clear();
                            for (int x = 0; x < temp1.ANH.Count; x++)
                            {
                                list_anh.Add(new ANH_ANH(byteArrayToImage(temp1.ANH[x].ANH), temp1.ANH[x].CENTER, temp1.ANH[x].SIZE, temp1.ANH[x].GOC, temp1.ANH[x].DIM));

                            }
                            reset_moitruong();
                            vt_themx = -imgx;
                            vt_themy = -imgy;
                            vt_chuot = new PointF(0, 0);
                            vt_menu = menu_select;

                            pic_menu1.Image = Properties.Resources.select;

                            vt_menu1 = 0;
                            checkloi();
                        }
                    }
                    catch
                    {

                    }

                }
                timer.Enabled = true;
                timer_map.Enabled = true;
            }
            else if (vv == menu_ok|| vv == menu_ok1)
            {
                if (led_full.Count > 0)
                {
                    timer.Enabled = false;
                    timer_map.Enabled = false;
                   
                        List<ANH_ANHL> anhl = new List<ANH_ANHL>();
                        if (list_anh.Count > 0)
                        {
                            for (int x = 0; x < list_anh.Count; x++) anhl.Add(new ANH_ANHL(ImageToByte2(list_anh[x].ANH), list_anh[x].CENTER, list_anh[x].SIZE, list_anh[x].GOC, list_anh[x].DIM));
                        }
                        LUU_MAPOK data_l = new LUU_MAPOK(led_full, anhl, vt_port2);


                    trave = data_l;


                    diday.Dispose();
                }
            }
        }
        List<string> list_wire = new List<string>();
        int vt_chon_wire = 0;
        int vt_hien_wire = 0;
        int max_rong_wire = 1;
        int max_cao_wire = 3;
        int max_hien_wire = 0;
        private void hien_list_diday()
        {
            list_wire.Clear();
            

            
            if (Directory.Exists(path_wire) == false)
            {
                Directory.CreateDirectory(path_wire);
            }
            Application.DoEvents();



            foreach (string link in Directory.GetFiles(path_wire, "*.map"))
            {

                list_wire.Add(link);
                int ddd = max_cao_wire * max_rong_wire;


                max_hien_wire = (list_wire.Count - ddd) / max_rong_wire;
                if ((list_wire.Count - ddd) % max_rong_wire > 0) max_hien_wire++;


                pic_wire.Refresh();


            }

        }


        private void checkloi()
        {

            this.Enabled = false;
           
            Application.DoEvents();


            led_error.Clear();
            int fff = 2;
            for (int x1 = 0; x1 < led_full.Count - 1; x1++)
            {
                for (int x2 = x1 + 1; x2 < led_full.Count; x2++)
                {
                    double cc = chieudailed(led_full[x1], led_full[x2]);
                    if (cc >= 0 && cc < fff)
                    {
                        led_error.Add(x1);
                    }


                }

            }
            this.Enabled = true;
          
            Application.DoEvents();
            // MessageBox.Show(vt_trum.Count.ToString() + " trum : " + vt_sat.Count.ToString() + " sat : ");

            //    if (led_error.Count > 0) so_bong.Text = "Tổng :  " + led_full.Count.ToString() + " đèn. Có " + led_error.Count.ToString() + " đèn trùm nhau (màu xanh). Xóa đèn trùm? -YES-";
            //  else so_bong.Text = "Tổng :  " + led_full.Count.ToString() + " đèn.";


        }
        public void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.


            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.UpdateStyles();
        }
        public static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
        string path_setting = System.IO.Directory.GetCurrentDirectory().ToString() + "\\data\\Setting.ini";

        static LUU_MAPOK trave;
        static Form1 diday;
        public static LUU_MAPOK ShowBox(string ff)
        {
            diday = new Form1();

            diday.ShowDialog();

            return trave;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            ControlMover.Init(this.panel_draw_led);
            ControlMover.Init(this.panel_draw_line);
            ControlMover.Init(this.panel_draw_border);
            ControlMover.Init(this.panel_draw_halo);
            ControlMover.Init(this.panel_draw_matrix);
            ControlMover.Init(this.panel_draw_text);
            ControlMover.Init(this.panel_draw_dxf);
            panel_draw_led.Location = new Point(pic_map.Width - panel_draw_led.Width+ pic_map.Location.X, pic_map.Location.Y);
            panel_draw_line.Location = new Point(pic_map.Width - panel_draw_line.Width + pic_map.Location.X, pic_map.Location.Y);
            panel_draw_border.Location = new Point(pic_map.Width - panel_draw_border.Width + pic_map.Location.X, pic_map.Location.Y);
            panel_draw_halo.Location = new Point(pic_map.Width - panel_draw_halo.Width + pic_map.Location.X, pic_map.Location.Y);
            panel_draw_matrix.Location = new Point(pic_map.Width - panel_draw_matrix.Width + pic_map.Location.X, pic_map.Location.Y);
            panel_draw_text.Location = new Point(pic_map.Width - panel_draw_text.Width + pic_map.Location.X, pic_map.Location.Y);
            panel_draw_dxf.Location = new Point(pic_map.Width - panel_draw_dxf.Width + pic_map.Location.X, pic_map.Location.Y);

            combo_vien.SelectedIndex = 0;
            fontListBox1.SelectedIndex = 0;
            //  panel_draw_line.Location = panel_draw_border.Location = panel_draw_halo.Location = panel_draw_matrix.Location = pic_map.Location;
            hien_list_diday();

            if (File.Exists(path_setting) == true)
            {
                IniParser parser = new IniParser(path_setting);
            
                int aa = Convert.ToInt32(parser.GetSetting("SETTING", "Language"));
             
               
                if (aa >= 0 && aa<2)
                {
                
                    menu_ngonngu = aa;

                    set_ngongu();
                }

            }
            else
            {


            }
            moi = new Bitmap(pic_map.Width, pic_map.Height);
            timer_map.Enabled = true;

        }
        private void copy_vung()
        {
            if (led_select.Count > 0)
            {


                add_undo();

                int bt = led_full.Count;
                int tt = led_select.Count;

                led_select.Sort();
                for (int x = 0; x < led_select.Count; x++)
                {
                    led_full.Add(new LED((int)(led_full[led_select[x]].X + 50), (int)(led_full[led_select[x]].Y )));
                    //  diem_select_chon.Add(vt_den.Count-1);

                }
                led_select.Clear();
                for (int x = 0; x < tt; x++)
                {
                    led_select.Add(bt + x);
                }
               
                get_select_tt();
               
            }

        }
        public static Bitmap ResizeImage0(Image image, int width, int height)
        {
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
    
        private int do_trong_anh = 100;
        List<ANH_ANH> list_anh = new List<ANH_ANH>();
        private void pic_menu1_MouseDown(object sender, MouseEventArgs e)
        {
            int vv = e.Y / 49;
            if (vt_menu == menu_draw)
            {
                if (vv >= 0 && vv <= 8)
                {
                    vt_menu1 = vv;
                    if(vt_menu1==menu_draw_led) panel_draw_led.Visible = true;
                    else panel_draw_led.Visible = false;

                    if (vt_menu1 == menu_draw_line) { panel_draw_line.Visible = true; led_add.Clear(); }
                    else panel_draw_line.Visible = false;

                    if (vt_menu1 == menu_draw_border) { panel_draw_border.Visible = true; led_add.Clear(); }
                    else panel_draw_border.Visible = false;

                    if (vt_menu1 == menu_draw_halo) { panel_draw_halo.Visible = true; led_add.Clear(); }
                    else panel_draw_halo.Visible = false;

                    if (vt_menu1 == menu_draw_matrix) { panel_draw_matrix.Visible = true; led_add.Clear(); }
                    else panel_draw_matrix.Visible = false;

                    if (vt_menu1 == menu_draw_text) { panel_draw_text.Visible = true;  led_add.Clear(); }
                    else panel_draw_text.Visible = false;

                    if (vt_menu1 == menu_draw_dxf)  {panel_draw_dxf.Visible = true; led_add.Clear();
                }
                else panel_draw_dxf.Visible = false;
                }
            }
            else if (vt_menu == menu_wire)
            {
                if (vv >= 0 && vv <= 5)
                {
                    vt_menu1 = vv;
                   
                }
                if (vv == menu_wire_rotator1)
                {
                    if (led_full.Count > 0)
                    {
                        
                        Application.DoEvents();
                        Thread.Sleep(100);
                        List<LED> vt_den_temp = new List<LED>();
                        List<LED> vt_den_temp1 = new List<LED>();
                        add_undo();
                        if(vt_port2<0)
                        {
                            for (int i = 0; i < led_full.Count; i++) vt_den_temp.Add(led_full[i]);
                            for (int i = 0; i < led_full.Count; i++) led_full[i] = vt_den_temp[led_full.Count - i - 1];
                        }
                        else
                        {
                            for (int i = 0; i < vt_port2; i++) vt_den_temp.Add(led_full[i]);
                            for (int i = 0; i < vt_port2; i++) led_full[i] = vt_den_temp[vt_port2 - i - 1];

                        }
  
                    }

                }
                else if (vv == menu_wire_rotator2)
                {
                    if (led_full.Count > 0 && vt_port2>0)
                    {

                        Application.DoEvents();
                        Thread.Sleep(100);
                        List<LED> vt_den_temp = new List<LED>();
                        List<LED> vt_den_temp1 = new List<LED>();
                        add_undo();

                        for (int i = vt_port2; i < led_full.Count; i++) vt_den_temp.Add(led_full[i]);


                        for (int i = vt_port2; i < led_full.Count; i++) led_full[i] = vt_den_temp[led_full.Count- i - 1];

                        

                    }

                }


            }
            else if (vt_menu == menu_tool)
            {
              
                    vt_menu1 = menu_tool_move;
               

                if (vv == menu_tool_open)
                {

                    OpenFileDialog open = new OpenFileDialog();
                    open.Multiselect = true;
                    open.Filter = "Ảnh ( *.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp";
                    if (open.ShowDialog() != DialogResult.OK) return;
                    else
                    {

                        FileStream obj_FileStream = new FileStream(open.FileName, FileMode.OpenOrCreate, FileAccess.Read);
                        Image aa = Image.FromStream(obj_FileStream);
                        if (aa.Width > aa.Height)
                        {
                            if (aa.Width >= 400) aa = ResizeImage0(Image.FromStream(obj_FileStream), 400, (400 * aa.Height) / aa.Width);
                        }
                        else
                        {
                            if (aa.Height >= 400) aa = ResizeImage0(Image.FromStream(obj_FileStream), (400 * aa.Width) / aa.Height, 400);
                        }

                        //aa = ResizeImage(aa, 10, 10);

                        // if (aa.Width >= 400 || aa.Height >= 400) aa=(ResizeImage0(aa, 400, aa.Width * 400 / aa.Height));
                        list_anh.Add(new ANH_ANH(aa, new PointF(200 + X0, 200 + Y0), new SizeF(aa.Width, aa.Height), 0,100));


                        obj_FileStream.Close();

                    }
                }
                else if (vv == menu_tool_dim)
                {
                    if (list_anh.Count > 0 && vt_chon_anh>=0 && vt_chon_anh< list_anh.Count)
                    {

                        list_anh[vt_chon_anh].DIM = list_anh[vt_chon_anh].DIM - 10;
                        if (list_anh[vt_chon_anh].DIM < 0) list_anh[vt_chon_anh].DIM = 100;
                        

                    }
                }
                else if (vv == menu_tool_delete)
                {
                    if (list_anh.Count > 0 && vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count)
                    {

                        list_anh.RemoveAt(vt_chon_anh);
                        vt_chon_anh = -1;

                    }
                }
               


            }
            else if (vt_menu == menu_select)
            {
               
                if(led_select.Count>1)
                {
                    if(vv== menu_select_copy)
                    {
                        copy_vung();
                    }
                    else if (vv == menu_select_delete)
                    {
                        del_selset();
                    }
                    else if (vv == menu_select_align0)
                    {
                        add_undo();
                        for (int x = 1; x < led_select.Count; x++)
                        {
                            led_full[led_select[x]] = new LED(led_full[led_select[x]].X, led_full[led_select[0]].Y);

                        }
                    }
                    else if (vv == menu_select_align1)
                    {
                        add_undo();
                        for (int x = 1; x < led_select.Count; x++)
                        {
                            led_full[led_select[x]] = new LED(led_full[led_select[0]].X, led_full[led_select[x]].Y);

                        }
                    }
                    else if (vv == menu_select_align2)
                    {
                        add_undo();
                        double chia0 = 0, chia1 = 100000, chia = 0;

                        //align_vt.Sort();

                        int tg;
                        for (int i = 0; i < led_select.Count - 1; i++)
                            for (int j = i + 1; j < led_select.Count; j++)
                                if (led_full[led_select[j]].Y < led_full[led_select[i]].Y)
                                {
                                    tg = led_select[i];
                                    led_select[i] = led_select[j];
                                    led_select[j] = tg;
                                }



                        for (int x = 0; x < led_select.Count; x++)
                        {
                            if (led_full[led_select[x]].Y >= chia0) chia0 = led_full[led_select[x]].Y;

                            if (led_full[led_select[x]].Y < chia1) chia1 = led_full[led_select[x]].Y;
                        }



                        chia = (chia0 - chia1) / (led_select.Count - 1);

                        if (led_full[led_select[led_select.Count - 1]].Y >= led_full[led_select[0]].Y)
                        {
                            for (int x = 0; x < led_select.Count; x++)
                            {
                                // diem_x[align_vt[x]] = diem_x[align_vt[0]];
                                led_full[led_select[x]] = new LED(led_full[led_select[x]].X, led_full[led_select[0]].Y + (int)(chia * x));

                            }
                        }
                        else
                        {

                            for (int x = 0; x < led_select.Count; x++)
                            {
                                led_full[led_select[x]] = new LED(led_full[led_select[x]].X, led_full[led_select[0]].Y - (int)(chia * x));
                            }

                        }

                    }
                    else if (vv == menu_select_align3)
                    {
                        add_undo();
                        double chia0 = 0, chia1 = 100000, chia = 0;

                        //align_vt.Sort();

                        int tg;
                        for (int i = 0; i < led_select.Count - 1; i++)
                            for (int j = i + 1; j < led_select.Count; j++)
                                if (led_full[led_select[j]].X < led_full[led_select[i]].X)
                                {
                                    tg = led_select[i];
                                    led_select[i] = led_select[j];
                                    led_select[j] = tg;
                                }



                        for (int x = 0; x < led_select.Count; x++)
                        {
                            if (led_full[led_select[x]].X >= chia0) chia0 = led_full[led_select[x]].X;

                            if (led_full[led_select[x]].X < chia1) chia1 = led_full[led_select[x]].X;
                        }



                        chia = (chia0 - chia1) / (led_select.Count - 1);

                        if (led_full[led_select[led_select.Count - 1]].X >= led_full[led_select[0]].X)
                        {
                            for (int x = 0; x < led_select.Count; x++)
                            {
                                // diem_x[align_vt[x]] = diem_x[align_vt[0]];
                                led_full[led_select[x]] = new LED(led_full[led_select[0]].X + (int)(chia * x), led_full[led_select[x]].Y);

                            }
                        }
                        else
                        {

                            for (int x = 0; x < led_select.Count; x++)
                            {
                                // diem_x[align_vt[x]] = diem_x[align_vt[0]];
                                led_full[led_select[x]] = new LED(led_full[led_select[0]].X - (int)(chia * x), led_full[led_select[x]].Y);
                            }

                        }
                    }
                    else if (vv == menu_select_align4)
                    {
                        double ppp = 0;
                        double qq = 0;
                        add_undo();
                        get_select_tt();
                        ppp = thongtin_select[3] / 2 + thongtin_select[1];

                        for (int x = 0; x < led_select.Count; x++)
                        {
                            if (led_full[led_select[x]].Y > ppp)
                            {
                                qq = led_full[led_select[x]].Y - ppp;

                                led_full[led_select[x]] = new LED(led_full[led_select[x]].X, ppp - qq);

                            }
                            else if (led_full[led_select[x]].Y < ppp)
                            {
                                qq = ppp - led_full[led_select[x]].Y;

                                led_full[led_select[x]] = new LED(led_full[led_select[x]].X, ppp + qq);

                            }

                        }

                    }
                    else if (vv == menu_select_align5)
                    {
                        double ppp = 0;
                        double qq = 0;
                        add_undo();
                        get_select_tt();
                        ppp = thongtin_select[2] / 2 + thongtin_select[0];

                        for (int x = 0; x < led_select.Count; x++)
                        {
                            if (led_full[led_select[x]].X > ppp)
                            {
                                qq = led_full[led_select[x]].X - ppp;

                                led_full[led_select[x]] = new LED(ppp - qq, led_full[led_select[x]].Y);

                            }
                            else if (led_full[led_select[x]].X < ppp)
                            {
                                qq = ppp - led_full[led_select[x]].X;

                                led_full[led_select[x]] = new LED(ppp + qq, led_full[led_select[x]].Y);

                            }

                        }
                    }
                    else if (vv == menu_select_save)
                    {

                        get_select_tt();



                        List<LED> led_l = new List<LED>();
                        

                        for (int x = 0; x < led_select.Count; x++)
                        {
                         
                            led_l.Add(new LED((double)((led_full[led_select[x]].X - thongtin_select[0] + 20) ), (double)((led_full[led_select[x]].Y - thongtin_select[1] + 20))));
                        }





                        int vt = 0;
                        double[] ss = new double[4];
                        List<LED> nn = new List<LED>();
                        for (int x = 0; x < led_select.Count; x++) nn.Add(new LED(led_full[led_select[x]].X, led_full[led_select[x]].Y));
                        ss = getanh(nn);
                        Bitmap aa2 = new Bitmap((int)ss[2] + 24 + 3, (int)ss[3] + 24 + 3);
                        Bitmap ok = new Bitmap(160, 120);
                        Graphics g = Graphics.FromImage(aa2);

                        Graphics g1 = Graphics.FromImage(ok);
                        g.Clear(Color.Black);
                        g1.Clear(Color.Black);

                        double zz1 = (double)(ss[2] + 1) / 160;
                        double zz2 = (double)(ss[3] + 1) / 120;
                        double zz3;
                        if (zz1 >= zz2) { zz3 = zz1; vt = 0; }
                        else { zz3 = zz2; vt = 1; };

                        // MessageBox.Show(ss[2].ToString() + " " + ss[3].ToString() + " " + zz1.ToString() + " " + zz2.ToString() + " " + zz3.ToString());

                        zz1 = (ss[2] + 1) / zz3;
                        zz2 = (ss[3] + 1) / zz3;
                        SolidBrush color_den_duoi = new SolidBrush(Color.FromArgb(250, 250, 250));
                        Pen color_thongtin_mau = new Pen(Color.FromArgb(255, 250, 250, 250), 1);
                        for (int x = 0; x < led_select.Count; x++)
                        {

                            g.FillEllipse(color_den_duoi,(int)( led_full[led_select[x]].X - ss[0]), (int)(led_full[led_select[x]].Y - ss[1]), 12, 12);
                            g.DrawEllipse(color_thongtin_mau, (int)(led_full[led_select[x]].X - ss[0]), (int)(led_full[led_select[x]].Y - ss[1]), 12, 12);
                        }
                        aa2 = (Bitmap)ResizeImage(aa2, (int)Math.Round(zz1, 0), (int)Math.Round(zz2, 0));
                        if (vt == 0) g1.DrawImage(aa2, new Point(0, (int)((120 - zz2) / 2)));
                        else if (vt == 1) g1.DrawImage(aa2, new Point((int)((160 - zz1) / 2), 0));
                        
              


                        string ten = save_wire.ShowBox(ok,menu_ngonngu);
                        
                        if (ten != "")
                        {
                            LUU_MAP data_l = new LUU_MAP(ImageToByte2((Image)ok), new Size((int)thongtin_select[2], (int)thongtin_select[3]), led_l, led_select.Count);

                            var rmCrypto = GetAlgorithm();
                            ICryptoTransform encryptor = rmCrypto.CreateEncryptor();

                            using (var writer = new StreamWriter(new CryptoStream(System.IO.File.Create(path_wire + ten), encryptor, CryptoStreamMode.Write)))
                            {
                                writer.Write(JsonConvert.SerializeObject(data_l));
                            }
                            hien_list_diday();
                          

                        }
                        Application.DoEvents();



                    }

                }
            }
           


        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static byte[] ImageToByte2(Image img)
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
        private byte[] Decrypt(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("chandoi", new byte[] { 0x34, 0x11, 0x46, 0x89, 0x14, 0x58, 0x69, 0x23, 0x72, 0x87 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(input, 0, input.Length);
            // System.Windows.Forms.MessageBox.Show(cs.);
            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                //saifile = true;

            }
            return ms.ToArray();
        }
        private byte[] Encrypt(byte[] input)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("chandoi", new byte[] { 0x34, 0x11, 0x46, 0x89, 0x14, 0x58, 0x69, 0x23, 0x72, 0x87 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
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
        private void del_selset()
        {
            if (led_select.Count > 0)
            {
                add_undo();
                int mm = -1;
                List<LED> vt_den_temp = new List<LED>();
                for (int x = 0; x < led_full.Count; x++)
                {
                    int aa = -1;
                    for (int y = 0; y < led_select.Count; y++)
                    {

                        if (led_select[y] == x) aa = 0;



                    }
                    if (aa == -1)
                    {
                        vt_den_temp.Add(led_full[x]);
                        if(vt_port2>0)
                        {
                            if (vt_port2 == x) mm = vt_den_temp.Count - 1;
                        }

                    }

                }
                led_full.Clear();

                for (int x = 0; x < vt_den_temp.Count; x++) led_full.Add(vt_den_temp[x]);
                if (vt_port2 > 0)
                {
                    vt_port2 = mm;
                }
                led_select.Clear();
                

            }
        }
        private void pic_menu1_Paint(object sender, PaintEventArgs e)
        {
            if (vt_menu == menu_draw)
            {
                if (nhay == true)
                {
                    if (vt_menu1 >= 0 && vt_menu1 <= 9)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(1,151,251), 2), 2, 2 + vt_menu1 * 49, 46, 46);
                    }

                }
            }
            else if (vt_menu == menu_wire)
            {
                if (nhay == true)
                {
                    if (vt_menu1 >= 0 && vt_menu1 <= 5)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(1, 151, 251), 2), 2, 2 + vt_menu1 * 49, 46, 46);
                    }

                }
            }
            else if (vt_menu == menu_tool)
            {
                if (nhay == true)
                {
                    if (vt_menu1 ==menu_tool_move)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(1, 151, 251), 2), 2, 2 + vt_menu1 * 49, 46, 46);
                    }

                }
                if (nhay == true)
                {
                    if (vt_chon_anh>=0)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2, 2 + 2 * 49, 46, 46);
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2, 2 + 3 * 49, 46, 46);
                    }

                }

            }
            else if (vt_menu == menu_select)
            {
                if (nhay == true)
                {
                    if (vt_menu1 >= 1 && vt_menu1 <= 4)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(1, 151, 251), 2), 2, 2 + vt_menu1 * 49, 46, 46);
                    }

                    if(led_select.Count>1)
                    {
                        for (int i = 0; i < 9; i++)e.Graphics.DrawRectangle(new Pen(Color.FromArgb(128, 128, 128), 2), 2, 2 + i * 49, 46, 46);
                    }

                }
            }

        }
        private void add_undo()
        {
            LED[] dd = new LED[led_full.Count];

            for (int i = 0; i < led_full.Count; i++)
            {
                dd[i] = led_full[i];

            }
            led_undo_port.Add(vt_port2);
            led_undo.Add(dd);
        }
 
        private void undo()
        {
            //Application.DoEvents();
            Thread.Sleep(10);

            
                if (led_undo.Count > 0)
                {

                     led_select.Clear();
                     

                     led_full.Clear();

                    LED[] dd = led_undo[led_undo.Count - 1];
                    int vvv = led_undo_port[led_undo.Count - 1];
                for (int i = 0; i < dd.Length; i++)
                    {
                        led_full.Add(dd[i]);


                    }
                vt_port2 = vvv;
                    led_undo.RemoveAt(led_undo.Count - 1);
                led_undo_port.RemoveAt(led_undo_port.Count - 1);
            }
                else
                {
                    led_undo.Clear();

                led_undo_port.Clear();

            }
                


        }

        float[] thongtin_select = new float[4];
        private int check_vung_select(PointF vt)
        {

            int cc = get_vung(vt, new PointF(thongtin_select[0], thongtin_select[1]), thongtin_select[2], thongtin_select[3]);



            return cc;
        }
        private int get_vung(PointF vt, PointF xv, float ww, float hh)
        {

            if (vt.X >= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) - 5 && vt.X <= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) + 5 && vt.Y >= ((xv.Y + imgy - 8) * zoom) - 5 && vt.Y <= ((xv.Y + imgy - 8) * zoom) + 5)
            {
                //MessageBox.Show("goc");
                return 3;
            }
            else if (vt.X >= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) - 5 && vt.X <= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) + 5 && vt.Y >= ((xv.Y + imgy - 8) * zoom) + ((hh + 16) * zoom) / 2 - 5 && vt.Y <= ((xv.Y + imgy - 8) * zoom) + ((hh + 16) * zoom) / 2 + 5)
            {
                // MessageBox.Show("ngang");
                return 1;
            }
            else if (vt.X >= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) / 2 - 5 && vt.X <= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) / 2 + 5 && vt.Y >= ((xv.Y + imgy - 8) * zoom) + ((hh + 16) * zoom) - 5 && vt.Y <= ((xv.Y + imgy - 8) * zoom) + ((hh + 16) * zoom) + 5)
            {
                // MessageBox.Show("doc");
                return 2;
            }
            else if (vt.X >= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) - 5 && vt.X <= ((xv.X + imgx - 8) * zoom) + ((ww + 16) * zoom) + 5 && vt.Y >= ((xv.Y + imgy - 8) * zoom) + ((hh + 16) * zoom) - 5 && vt.Y <= ((xv.Y + imgy - 8) * zoom) + ((hh + 16) * zoom) + 5)
            {
                //MessageBox.Show("cheo");
                return 4;
            }
            else if (vt.X >= (xv.X + imgx - 8) * zoom && vt.X <= (xv.X + imgx - 8) * zoom + (ww + 16) * zoom - 5 && vt.Y >= (xv.Y + imgy - 8) * zoom && vt.Y <= (xv.Y + imgy - 8) * zoom + (hh + 16) * zoom)
            {
                // MessageBox.Show("dichuyen");
                return 0;
            }

            return -1;
        }

        List<LED> led_tem_goc = new List<LED>();
        private void saochep_temp()
        {
            led_tem_goc.Clear();
            for (int x = 0; x < led_full.Count; x++)
            {
                led_tem_goc.Add(led_full[x]);

            }

        }
        bool keo_drawline = false;
        bool keo_drawborder = false;
        bool keo_drawhalo = false;
        bool keo_drawmatrix= false;
        
        int loaikeochon = -1;
        Point vt_select0 = new Point(-1, -1);
        Point vt_select1 = new Point(-1, -1);
        bool kt_select = false;
        int vtc = -1;
        int loai_keo = -1;
        bool keo_select = false;
        PointF vt_dichuyen = new PointF(-1, -1);
        PointF vt_dichuyen1 = new PointF(-1, -1);
        int vt_dichuyen_1den = -1;
        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
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
                if (vt_menu == menu_select)
                {
                    if (led_select.Count > 0)
                    {
                        if ((ModifierKeys & Keys.Control) == Keys.Control || (ModifierKeys & Keys.Shift) == Keys.Shift)
                        {

                            if ((ModifierKeys & Keys.Control) == Keys.Control)
                            {
                                loaikeochon = 1;
                              //  this.Cursor = new Cursor(GetType(), "addvung.cur");
                            }
                            else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                            {
                                loaikeochon = 0;
                               // this.Cursor = new Cursor(GetType(), "truvung.cur");

                            }
                            kt_select = true;
                            vt_select0 = new Point(e.Location.X, e.Location.Y);
                            vt_select1 = new Point(-1, -1);

                        }
                        else
                        {
                            vtc = check_vung_select(mouse.Location);
                            loai_keo = vtc;
                            if (vtc >= 0)
                            {
                              //  if (loai_keo == 0) this.Cursor = new Cursor(GetType(), "edit_dichuyen.cur");
                              //  else if (loai_keo == 1) this.Cursor = new Cursor(GetType(), "edit_ngang.cur");
                               // else if (loai_keo == 2) this.Cursor = new Cursor(GetType(), "edit_doc.cur");
                              //  else if (loai_keo == 3) this.Cursor = new Cursor(GetType(), "edit_goc.cur");
                               // else if (loai_keo == 4) this.Cursor = new Cursor(GetType(), "edit_cheo.cur");
                                add_undo();
                                saochep_temp();
                                vt_dichuyen = new PointF(e.Location.X / zoom - imgx, e.Location.Y / zoom - imgy);
                                vt_dichuyen1 = e.Location;
                                keo_select = true;
                            }
                            else
                            {
                                loai_keo = -1;
                              //  this.Cursor = Cursors.Default;
                                loaikeochon = -1;
                                led_select.Clear();
                                
                            }

                        }



                    }
                    else
                    {
                        int cc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (cc >= 0)
                        {
                            vt_dichuyen_1den = cc;
                            loai_keo = 10;
                            vt_dichuyen = new PointF(e.Location.X / zoom - imgx, e.Location.Y / zoom - imgy);
                            vt_dichuyen1 = e.Location;
                            keo_select = true;
                            
                        }
                        else
                        {
                            vt_dichuyen_1den = -1;
                          //  this.Cursor = new Cursor(GetType(), "addvung.cur");
                            loaikeochon = 1;
                            kt_select = true;
                            vt_select0 = new Point(e.Location.X, e.Location.Y);
                            vt_select1 = new Point(-1, -1);
                        }


                    }

                }
                else  if (vt_menu == menu_draw)
                {
                    if (vt_menu1 == menu_draw_led)
                    {
                        if (mouse.Button == MouseButtons.Left)
                        {
                            double xxx = vt_themx / zoom - imgx;
                            double yyy = vt_themy / zoom - imgy;

                            if (led_full.Count > 0)
                            {

                                if (khoa_day.Checked == false)
                                {
                                    add_undo();
                                    led_full.Add(new LED(xxx, yyy));
                                }
                                else
                                {

                                    int kd = (int)numericUpDown1.Value;
                                    if (kd <= 12) kd = 12;
                                    float kk = (float)(kd);

                                    double dx = led_full[led_full.Count - 1].X - (xxx);
                                    double dy = led_full[led_full.Count - 1].Y - (yyy);
                                    double length = Math.Sqrt(dx * dx + dy * dy);
                                    if (length > 0)
                                    {
                                        dx /= length;
                                        dy /= length;
                                    }
                                    dx *= length - kk;
                                    dy *= length - kk;
                                    int x3 = (int)(xxx + dx);
                                    int y3 = (int)(yyy + dy);

                                    add_undo();
                                    led_full.Add(new LED(x3, y3));
                                }
                            }
                            else
                            {
                                add_undo();
                                led_full.Add(new LED(xxx, yyy));

                            }
                        }

                    }
                    else if (vt_menu1 == menu_draw_insert)
                    {

                        if (diem_select_chen >= 0)
                        {
                            double xxx = vt_themx / zoom - imgx;
                            double yyy = vt_themy / zoom - imgy;
                            add_undo();
                            led_full.Insert(diem_select_chen + 1, new LED(xxx, yyy));
                            if (diem_select_chen < vt_port2) vt_port2++;
                            
                            diem_select_chen = -1;

                            // checkloi();
                            // creart_moitruong();
                            //draw_moitruong();
                        }
                    }
                    else if (vt_menu1 == menu_draw_delete)
                    {

                        if (diem_select_xoa >= 0)
                        {
                            double xxx = vt_themx / zoom - imgx;
                            double yyy = vt_themy / zoom - imgy;
                            add_undo();
                            led_full.RemoveAt(diem_select_xoa);
                            if (diem_select_xoa <= vt_port2) vt_port2--;
                            if (vt_port2 == 0) vt_port2 = -1;
                              diem_select_xoa = -1;

                            // checkloi();
                            // creart_moitruong();
                            //draw_moitruong();
                        }
                    }
                    else if (vt_menu1 == menu_draw_line)
                    {
                        keo_drawline = true;
                        led_add.Clear();
                        if (mouse.Button == MouseButtons.Left)
                        {

                            vt_add_line = new PointF(e.Location.X / zoom - imgx, e.Location.Y / zoom - imgy);
                            vt_add_line1 = new PointF(-1, -1);
                        }
                    }
                    else if (vt_menu1 == menu_draw_border)
                    {
                        keo_drawborder = true;
                        led_add.Clear();
                        if (mouse.Button == MouseButtons.Left)
                        {

                            vt_add_line = new PointF(e.Location.X / zoom - imgx, e.Location.Y / zoom - imgy);
                            vt_add_line1 = new PointF(-1, -1);
                        }
                    }
                    else if (vt_menu1 == menu_draw_halo)
                    {
                        keo_drawhalo = true;
                        led_add.Clear();
                        if (mouse.Button == MouseButtons.Left)
                        {

                            vt_add_line = new PointF(e.Location.X / zoom - imgx, e.Location.Y / zoom - imgy);
                            vt_add_line1 = new PointF(-1, -1);
                        }
                    }
                    else if (vt_menu1 == menu_draw_matrix)
                    {
                        keo_drawmatrix = true;
                        led_add.Clear();
                        if (mouse.Button == MouseButtons.Left)
                        {

                            vt_add_line = new PointF(e.Location.X / zoom - imgx, e.Location.Y / zoom - imgy);
                            vt_add_line1 = new PointF(-1, -1);
                        }
                    }
                    else if (vt_menu1 == menu_draw_text)
                    {

                        add_undo();
                            for (int i = 0; i < led_add.Count; i++) led_full.Add(new LED(led_add[i].X+ e.Location.X / zoom - imgx - text_size[2], led_add[i].Y+ e.Location.Y / zoom - imgy - text_size[3]));
                        led_add.Clear();
                    }
                    else if (vt_menu1 == menu_draw_dxf)
                    {

                        add_undo();
                        for (int i = 0; i < led_add.Count; i++) led_full.Add(new LED(led_add[i].X + e.Location.X / zoom - imgx - text_size[2], led_add[i].Y + e.Location.Y / zoom - imgy - text_size[3]));
                        led_add.Clear();
                    }
                }
                else if (vt_menu == menu_wire)
                {
                    if (vt_menu1 == menu_wire_port1)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {

                            add_undo();
                            List<LED> vt_den_temp = new List<LED>();
                            List<LED> vt_den_temp1 = new List<LED>();
                            for (int i = 0; i < led_full.Count; i++) vt_den_temp.Add(led_full[i]);
                            int chay = led_full.Count - 1;
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
                            for (int i = 0; i < led_full.Count; i++) led_full[i] = vt_den_temp1[i];

                            // checkloi();


                        }

                    }
                    else if (vt_menu1 == menu_wire_port2)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {
                            vt_port2 = vtc;

                            // checkloi();


                        }
                        else
                        {
                            vt_port2 = -1;
                        }

                    }
                  
                    else if (vt_menu1 == menu_wire_connect0)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {
                            add_undo();
                            vt_auto1 = vtc;
                            led_select.Clear();

                            for (int i = 0; i < vtc; i++) led_select.Add(i);
                            led_select.Add(vtc);
                            keo_thucong = true;
                           

                        }

                    }
                    else if (vt_menu1 == menu_wire_connect1)
                    {
                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0)
                        {
                            add_undo();
                           
                            vt_auto1 = vtc;
                            led_select.Clear();

                            for (int i = 0; i < vtc; i++) led_select.Add(i);
                         
                          
                         
                            keo_thucong = true;


                        }

                    }
                }
                else if (vt_menu == menu_tool)
                {
                    if(list_anh.Count>0)
                    {
                        int[] vt = find_video(e.Location);
                        if (vt[0] >= 0)
                        {
                            vt_chon_anh = vt[0];
                            loaikeo_video = vt[1];
                            if (vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count)
                            {
                                goc_luu = list_anh[vt_chon_anh].GOC;
                                center_luu = list_anh[vt_chon_anh].CENTER;
                                size_luu = list_anh[vt_chon_anh].SIZE;
                            }
                            vt_chuot_anh = e.Location;
                            // MessageBox.Show((vt[0]).ToString() + "   " + (vt[1]).ToString());
                        }
                        else
                        {
                            vt_chon_anh = -1;
                            loaikeo_video = -1;
                            
                        }
                    }
                }
            }
        }
        int vt_chon_anh = -1;
        int loaikeo_video = -1;
        double goc_luu = 0;
        PointF center_luu;
        SizeF size_luu;
        private int[] find_video(PointF ll)
        {

            for (int x = list_anh.Count - 1; x >= 0; x--)
            {
                PointF[] temp = get_thongtin_video(list_anh[x]);

                if (ll.X >= temp[0].X - 7 && ll.X <= temp[0].X + 7 && ll.Y >= temp[0].Y - 7 && ll.Y <= temp[0].Y + 7) return new int[2] { x, 0 };
                else if (ll.X >= temp[1].X - 7 && ll.X <= temp[1].X + 7 && ll.Y >= temp[1].Y -7 && ll.Y <= temp[1].Y + 7) return new int[2] { x, 1 };
                else if (ll.X >= temp[2].X - 7 && ll.X <= temp[2].X + 7 && ll.Y >= temp[2].Y - 7 && ll.Y <= temp[2].Y + 7) return new int[2] { x, 2 };
                else if (ll.X >= temp[3].X - 7 && ll.X <= temp[3].X + 7 && ll.Y >= temp[3].Y - 7 && ll.Y <= temp[3].Y + 7) return new int[2] { x, 3 };
                else if (ll.X >= temp[4].X - 7 && ll.X <= temp[4].X + 7 && ll.Y >= temp[4].Y - 7 && ll.Y <= temp[4].Y + 7) return new int[2] { x, 4 };
                else
                {
                    PointF[] aa1 = new PointF[4] { temp[5], temp[6], temp[7], temp[8] };


                    if (IsPointInPolygon4(aa1, ll) == true)
                    {

                        return new int[2] { x, 5 };
                    }
                }

            }




            return new int[2] { -1, -1 };
        }
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
        private PointF[] get_thongtin_video(ANH_ANH ll)
        {
            PointF[] vt = new PointF[10] { new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1) };

            PointF center = new PointF((ll.CENTER.X + imgx) * zoom, (ll.CENTER.Y + imgy) * zoom);




            vt[5] = new PointF((ll.CENTER.X - ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);
            vt[6] = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);
            vt[7] = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + ll.SIZE.Height / 2 + imgy) * zoom);
            vt[8] = new PointF((ll.CENTER.X - ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + ll.SIZE.Height / 2 + imgy) * zoom);
            vt[5] = RotatePoint(vt[5], center, ll.GOC);
            vt[6] = RotatePoint(vt[6], center, ll.GOC);
            vt[7] = RotatePoint(vt[7], center, ll.GOC);
            vt[8] = RotatePoint(vt[8], center, ll.GOC);

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
            vt[9] = center;


            return vt;
        }
        List<int> cu = new List<int>();
        int vt_auto1 = -1;
        bool keo_thucong = false;
        Bitmap moi;
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
                SolidBrush errr = new SolidBrush(Color.FromArgb(128, 128, 128));

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


                if (list_anh.Count > 0 && do_trong_anh > 0)
                {
                    for (int x = 0; x < list_anh.Count; x++)
                    {


                        if (x == vt_chon_anh && vt_menu == menu_tool && nhay == true) Draw_video(g, list_anh[x], true);
                        else Draw_video(g, list_anh[x], false);


                    }

                }



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
                if (nhay == true && led_error.Count > 0)
                {
                    for (int x = 0; x < led_error.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[led_error[x]].X - 6 + imgx) * zoom, (float)(led_full[led_error[x]].Y - 6 + imgy) * zoom);
                        DrawRoundedRectangle_fill(g, errr, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                    }
                }



                if (led_full.Count > 1)
                {

                    for (int i = 0; i < led_full.Count - 1; i++)
                    {
                        PointF point1 = new PointF((float)(led_full[i].X + imgx) * zoom, (float)(led_full[i].Y + imgy) * zoom);
                        PointF point2 = new PointF((float)(led_full[i + 1].X + imgx) * zoom, (float)(led_full[i + 1].Y + imgy) * zoom);
                        if (vt_menu == menu_wire && keo_thucong == true)
                        {
                            DrawArrow(g, maudaykeo, point1, point2, 4 * zoom);
                        }
                        else
                        {
                            DrawArrow(g, mauday, point1, point2, 4 * zoom);
                        }
                    }


                }
                if (vt_menu == menu_select)
                {


                    if (led_select.Count > 0)
                    {
                        for (int x = 0; x < led_select.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_full[led_select[x]].X - 6 + imgx) * zoom, (float)(led_full[led_select[x]].Y - 6 + imgy) * zoom);

                            //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                            //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            DrawRoundedRectangle_fill(g, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                        }


                        g.FillRectangle(new SolidBrush(Color.FromArgb(64, 128, 128, 128)), new Rectangle((int)((thongtin_select[0] + imgx - 8) * zoom), (int)((thongtin_select[1] + imgy - 8) * zoom), (int)((thongtin_select[2] + 16) * zoom), (int)((thongtin_select[3] + 16) * zoom)));


                        g.DrawRectangle(maussss, new Rectangle((int)((thongtin_select[0] + imgx - 8) * zoom), (int)((thongtin_select[1] + imgy - 8) * zoom), (int)((thongtin_select[2] + 16) * zoom), (int)((thongtin_select[3] + 16) * zoom)));

                        if (nhay == true)
                        {
                            g.FillEllipse(new SolidBrush(Color.FromArgb(1, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) - 7, 14, 14);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) + (int)((thongtin_select[3] + 16) * zoom) / 2 - 7, 14, 14);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) / 2 - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) + (int)((thongtin_select[3] + 16) * zoom) - 7, 14, 14);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) + (int)((thongtin_select[3] + 16) * zoom) - 7, 14, 14);
                        }



                    }

                    if (kt_select == true && vt_select1.X >= 0 && vt_select1.Y >= 0)
                    {
                        float[] aa = chuyenvt(vt_select0.X, vt_select0.Y, vt_select1.X, vt_select1.Y);
                        if (aa[2] >= 0 && aa[0] >= 0 && aa[3] >= 0 && aa[1] >= 0)
                        {
                            Pen dashed_pen = new Pen(Color.White, 1);

                            // dashed_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                            dashed_pen.DashPattern = new float[] { 1, (int)1 };
                            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 151, 251)), new Rectangle((int)(aa[0]), (int)(aa[1]), (int)(aa[2] - aa[0]), (int)((aa[3] - aa[1]))));
                            g.DrawRectangle(dashed_pen, new Rectangle((int)(aa[0]), (int)(aa[1]), (int)(aa[2] - aa[0]), (int)((aa[3] - aa[1]))));
                        }
                    }

                }
                else if (vt_menu == menu_draw)
                {
                    if (vt_menu1 == menu_draw_led)
                    {
                        if (khoa_day.Checked == false || (khoa_day.Checked == true && led_full.Count <= 0))
                        {
                            PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        }
                        else
                        {

                            int kd = (int)numericUpDown1.Value;
                            if (kd <= 12) kd = 12;
                            float kk = (float)(kd);

                            float xxx = vt_themx / zoom - imgx;
                            float yyy = vt_themy / zoom - imgy;


                            float dx = (float)led_full[led_full.Count - 1].X - xxx;
                            float dy = (float)led_full[led_full.Count - 1].Y - yyy;
                            float length = (float)Math.Sqrt(dx * dx + dy * dy);
                            if (length > 0)
                            {
                                dx /= length;
                                dy /= length;
                            }
                            dx *= length - kk;
                            dy *= length - kk;
                            int x3 = (int)(xxx + dx);
                            int y3 = (int)(yyy + dy);
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)((x3 + imgx) * zoom) - (int)(6 * zoom), (int)((y3 + imgy) * zoom) - (int)(6 * zoom), (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)((x3 + imgx) * zoom) - (int)(6 * zoom), (int)((y3 + imgy) * zoom) - (int)(6 * zoom), (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }
                    }
                    else if (vt_menu1 == menu_draw_insert)
                    {
                        if (diem_select_chen >= 0)
                        {
                            PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        }
                    }
                    else if (vt_menu1 == menu_draw_delete)
                    {
                        if (diem_select_xoa >= 0)
                        {
                            PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        }
                    }
                    else if (vt_menu1 == menu_draw_line)
                    {
                        if (keo_drawline == true)
                        {

                            if (led_add.Count > 0)
                            {
                                for (int x = 0; x < led_add.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6 + imgy) * zoom);

                                    if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                    else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                                }

                            }

                        }
                        else
                        {
                            PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }


                    }
                    else if (vt_menu1 == menu_draw_border)
                    {
                        if (keo_drawborder == true)
                        {

                            if (led_add.Count > 0)
                            {
                                for (int x = 0; x < led_add.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6 + imgy) * zoom);

                                    if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                    else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                                }

                            }

                        }
                        else
                        {
                            PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }


                    }
                    else if (vt_menu1 == menu_draw_halo)
                    {
                        if (keo_drawhalo == true)
                        {

                            if (led_add.Count > 0)
                            {
                                for (int x = 0; x < led_add.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6 + imgy) * zoom);

                                    if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                    else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                                }

                            }

                        }
                        else
                        {
                            PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }


                    }
                    else if (vt_menu1 == menu_draw_matrix)
                    {
                        if (keo_drawmatrix == true)
                        {

                            if (led_add.Count > 0)
                            {
                                for (int x = 0; x < led_add.Count; x++)
                                {
                                    PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6 + imgy) * zoom);

                                    if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                    else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                                }

                            }

                        }
                        else
                        {
                            PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                            if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                        }


                    }
                    else if (vt_menu1 == menu_draw_text)
                    {


                        if (led_add.Count > 0)
                        {
                            for (int x = 0; x < led_add.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx + vt_add_line.X - text_size[2] + into_text) * zoom, (float)(led_add[x].Y - 6 + imgy + vt_add_line.Y - text_size[3]) * zoom);

                                if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                            }

                        }




                    }
                    else if (vt_menu1 == menu_draw_dxf)
                    {


                        if (led_add.Count > 0)
                        {
                            for (int x = 0; x < led_add.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx + vt_add_line.X - text_size[2] + into_text) * zoom, (float)(led_add[x].Y - 6 + imgy + vt_add_line.Y - text_size[3]) * zoom);

                                if (nhay == true) DrawRoundedRectangle(g, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else DrawRoundedRectangle(g, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                            }

                        }




                    }
                }
                else if (vt_menu == menu_wire)
                {
                    if (keo_thucong == true && (vt_menu1 == menu_wire_connect0 || vt_menu1 == menu_wire_connect1))
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
        private void DrawRoundedRectangle_fill(Graphics g, Brush p,
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
        public static Bitmap ChangeOpacity(System.Drawing.Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
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
            Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(Color.Transparent);
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
            Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(Color.Transparent);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
            g.Dispose();

            return newImg;
        }
        private void Draw_video(Graphics gr, ANH_ANH ll, bool hien)
        {
            PointF[] vt = new PointF[5] { new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1) };
            SolidBrush c = new SolidBrush(Color.FromArgb(64, 128, 128, 128));
            SolidBrush c1 = new SolidBrush(Color.FromArgb(128, 255, 0, 0));
            PointF center = new PointF((ll.CENTER.X + imgx) * zoom, (ll.CENTER.Y + imgy) * zoom);
            Pen p = new Pen(Color.FromArgb(1, 151, 251));
            //Bitmap anh_temp = (Bitmap)ResizeImage0(ll.ANH, (int)(ll.SIZE.Width * zoom), (int)(ll.SIZE.Height * zoom));
            Bitmap anh_temp = (Bitmap)ll.ANH;
            anh_temp = (Bitmap)ResizeImage0(ll.ANH, (int)(ll.SIZE.Width), (int)(ll.SIZE.Height));

            float w0 = anh_temp.Width;
            float h0 = anh_temp.Height;
            float opacityvalue = float.Parse(ll.DIM.ToString()) / 100;
            anh_temp = ChangeOpacity(anh_temp, opacityvalue);
            anh_temp = (Bitmap)RotateImg(anh_temp, (float)ll.GOC);


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
            gr.DrawImage(anh_temp, new RectangleF((int)(center.X - ww / 2 - ll.SIZE.Width / 2 * zoom), (int)(center.Y - hh / 2 - ll.SIZE.Height / 2 * zoom), anh_temp.Width * zoom, anh_temp.Height * zoom), new RectangleF(0, 0, anh_temp.Width, anh_temp.Height), GraphicsUnit.Pixel);

            gr.DrawPolygon(p, new PointF[4] { a1, a2, a3, a4 });
            if (hien == true)
            {
                gr.FillEllipse(new SolidBrush(Color.FromArgb(1, 151, 251)), vt[4].X - 7, vt[4].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(Color.FromArgb(1, 151, 251)), vt[0].X - 7, vt[0].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(Color.FromArgb(1, 151, 251)), vt[1].X - 7, vt[1].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(Color.FromArgb(1, 151, 251)), vt[2].X - 7, vt[2].Y - 7, 14, 14);
                gr.FillRectangle(new SolidBrush(Color.FromArgb(1, 151, 251)), vt[3].X - 7, vt[3].Y - 7, 14, 14);

            }

        }
        int into_text = 0;
        private void pic_day_Paint(object sender, PaintEventArgs e)
        {
            /*
            Pen mau_rule = new Pen(Color.FromArgb(42, 128, 128, 128), 1);
            Pen mau_rulec = new Pen(Color.FromArgb(64, 255, 0, 0), 1);
            SolidBrush mauden = new SolidBrush(Color.FromArgb(250, 0, 0));
            SolidBrush mauden0 = new SolidBrush(Color.FromArgb(250, 255, 0));
            SolidBrush mauden00 = new SolidBrush(Color.FromArgb(0, 0, 255));
            SolidBrush mauden1 = new SolidBrush(Color.FromArgb(0, 255, 0));
            SolidBrush tamden = new SolidBrush(Color.FromArgb(32, 32, 32));
            SolidBrush mauselect = new SolidBrush(Color.FromArgb(250, 250, 250));
            SolidBrush errr = new SolidBrush(Color.FromArgb(128, 128, 128));

            Pen mau_l= new Pen(Color.FromArgb(96, 96, 96), 1);
            Pen maussss = new Pen(Color.FromArgb(1, 151, 251), 2);
            Pen mau_l0 = new Pen(Color.FromArgb(128, 0, 0), 1);
            Pen mauday = new Pen(Color.FromArgb(255, 255,255), 2);
            Pen maudaykeo = new Pen(Color.FromArgb(128, 128, 128), 2);
            Pen maudaythang= new Pen(Color.FromArgb(126, 87, 194), 2);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            // for (int x = 0; x < pic_map.Height / 32+1; x++) e.Graphics.DrawLine(mau_rule, new System.Drawing.Point(0, x * 32), new System.Drawing.Point(pic_map.Width, x * 32));
            //  for (int x = 0; x < pic_map.Width / 32+1; x++) e.Graphics.DrawLine(mau_rule, new System.Drawing.Point(x * 32, 0), new System.Drawing.Point(x * 32, pic_map.Height));
            for (int x = 0; x < W0 / 64 + 1; x++) e.Graphics.DrawLine(mau_rule, new PointF((float)(64 * x + imgx) * zoom, (float)(0 + imgy) * zoom), new PointF((float)(64 * x + imgx) * zoom, (float)(H0 + imgy) * zoom));
            for (int x = 0; x < H0 / 64 + 1; x++) e.Graphics.DrawLine(mau_rule, new PointF((float)(0 + imgx) * zoom, (float)(64 * x + imgy) * zoom), new PointF((float)(W0 + imgx) * zoom, (float)(64 * x + imgy) * zoom));


            if (list_anh.Count > 0 && do_trong_anh > 0)
            {
                for (int x = 0; x < list_anh.Count; x++)
                {


                      if(x==vt_chon_anh && vt_menu==menu_tool && nhay==true) Draw_video(e.Graphics, list_anh[x], true);
                     else Draw_video(e.Graphics, list_anh[x], false);
                
            
                }

            }



            for (int x = 0; x < led_full.Count; x++)
            {
                PointF point1_1 = new PointF((float)(led_full[x].X - 6 + imgx) * zoom, (float)(led_full[x].Y - 6 + imgy) * zoom);
                PointF point1_2 = new PointF((float)(led_full[x].X - 3+ imgx) * zoom, (float)(led_full[x].Y - 3 + imgy) * zoom);
                //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                if (x == 0) DrawRoundedRectangle_fill(e.Graphics, mauden0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                else
                {
                    if (vt_port2 < 0 || vt_port2>= led_full.Count)
                    {
                        if (x > 0) DrawRoundedRectangle_fill(e.Graphics, mauden, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                    }
                    else
                    {
                        if(x< vt_port2) DrawRoundedRectangle_fill(e.Graphics, mauden, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        else if (x == vt_port2) DrawRoundedRectangle_fill(e.Graphics, mauden00, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3); 
                        else DrawRoundedRectangle_fill(e.Graphics, mauden1, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }
                }

                

                e.Graphics.FillEllipse(tamden, new Rectangle((int)point1_2.X, (int)point1_2.Y, (int)(6 * zoom), (int)(6 * zoom)));
               
            }
            if (nhay == true && led_error.Count > 0)
            {
                for (int x = 0; x < led_error.Count; x++)
                {
                    PointF point1_1 = new PointF((float)(led_full[led_error[x]].X - 6 + imgx) * zoom, (float)(led_full[led_error[x]].Y - 6 + imgy) * zoom);
                    DrawRoundedRectangle_fill(e.Graphics, errr, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                }
            }
   


            if (led_full.Count > 1)
            {

                for (int i = 0; i < led_full.Count - 1; i++)
                {
                    PointF point1 = new PointF((float)(led_full[i].X + imgx) * zoom, (float)(led_full[i].Y + imgy) * zoom);
                    PointF point2 = new PointF((float)(led_full[i + 1].X + imgx) * zoom, (float)(led_full[i + 1].Y + imgy) * zoom);
                    if (vt_menu == menu_wire && keo_thucong==true)
                    {
                        DrawArrow(e.Graphics, maudaykeo, point1, point2, 4 * zoom);
                    }
                    else
                    {
                        DrawArrow(e.Graphics, mauday, point1, point2, 4 * zoom);
                    }
                }
                

            }
               if (vt_menu == menu_select)
            {


                if (led_select.Count > 0 )
                {
                    for (int x = 0; x < led_select.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[led_select[x]].X - 6 + imgx) * zoom, (float)(led_full[led_select[x]].Y - 6 + imgy) * zoom);

                        //  e.Graphics.FillEllipse(mauden, new Rectangle((int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom)));
                        //DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        DrawRoundedRectangle_fill(e.Graphics, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                    }

        
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 128, 128, 128)), new Rectangle((int)((thongtin_select[0] + imgx - 8) * zoom), (int)((thongtin_select[1] + imgy - 8) * zoom), (int)((thongtin_select[2] + 16) * zoom), (int)((thongtin_select[3] + 16) * zoom)));
                   
                    
                    e.Graphics.DrawRectangle(maussss, new Rectangle((int)((thongtin_select[0] + imgx - 8) * zoom), (int)((thongtin_select[1] + imgy - 8) * zoom), (int)((thongtin_select[2] + 16) * zoom), (int)((thongtin_select[3] + 16) * zoom)));

                    if (nhay == true)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(1, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) - 7, 14, 14);
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) + (int)((thongtin_select[3] + 16) * zoom) / 2 - 7, 14, 14);
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) / 2 - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) + (int)((thongtin_select[3] + 16) * zoom) - 7, 14, 14);
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 151, 251)), (int)((thongtin_select[0] + imgx - 8) * zoom) + (int)((thongtin_select[2] + 16) * zoom) - 7, (int)((thongtin_select[1] + imgy - 8) * zoom) + (int)((thongtin_select[3] + 16) * zoom) - 7, 14, 14);
                    }

                   

                }

                if (kt_select == true && vt_select1.X >= 0 && vt_select1.Y >= 0)
                {
                    float[] aa = chuyenvt(vt_select0.X, vt_select0.Y, vt_select1.X, vt_select1.Y);
                    if (aa[2] >= 0 && aa[0] >= 0 && aa[3] >= 0 && aa[1] >= 0)
                    {
                        Pen dashed_pen = new Pen(Color.White, 1);

                        // dashed_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        dashed_pen.DashPattern = new float[] { 1, (int)1 };
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 151, 251)), new Rectangle((int)(aa[0]), (int)(aa[1]), (int)(aa[2] - aa[0]), (int)((aa[3] - aa[1]))));
                        e.Graphics.DrawRectangle(dashed_pen, new Rectangle((int)(aa[0]), (int)(aa[1]), (int)(aa[2] - aa[0]), (int)((aa[3] - aa[1]))));
                    }
                }

            }
             else  if (vt_menu == menu_draw )
            {
                if (vt_menu1 == menu_draw_led)
                {
                    if (khoa_day.Checked == false || (khoa_day.Checked == true && led_full.Count <= 0))
                    {
                        PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                    }
                    else
                    {

                        int kd = (int)numericUpDown1.Value;
                        if (kd <= 12) kd = 12;
                        float kk = (float)(kd);

                        float xxx = vt_themx / zoom - imgx;
                        float yyy = vt_themy / zoom - imgy;


                        float dx = (float)led_full[led_full.Count - 1].X - xxx;
                        float dy = (float)led_full[led_full.Count - 1].Y - yyy;
                        float length = (float)Math.Sqrt(dx * dx + dy * dy);
                        if (length > 0)
                        {
                            dx /= length;
                            dy /= length;
                        }
                        dx *= length - kk;
                        dy *= length - kk;
                        int x3 = (int)(xxx + dx);
                        int y3 = (int)(yyy + dy);
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l,  (int)((x3 + imgx) * zoom) - (int)(6 * zoom), (int)((y3 + imgy) * zoom) - (int)(6 * zoom), (int)(12 * zoom), (int)(12 * zoom),3,3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0,  (int)((x3 + imgx) * zoom) - (int)(6 * zoom), (int)((y3 + imgy) * zoom) - (int)(6 * zoom), (int)(12 * zoom), (int)(12 * zoom),3,3);

                    }
                }else if (vt_menu1 == menu_draw_insert)
                {
                    if (diem_select_chen >= 0)
                    {
                        PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                    }
                }
                else if (vt_menu1 == menu_draw_delete)
                {
                    if (diem_select_xoa >= 0)
                    {
                        PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                    }
                }
                else if (vt_menu1 == menu_draw_line)
                {
                    if (keo_drawline == true)
                    {

                        if (led_add.Count > 0)
                        {
                            for (int x = 0; x < led_add.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6+ imgy) * zoom);
                                
                                if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                                else DrawRoundedRectangle(e.Graphics, mau_l0,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);


                            }

                        }

                    }
                    else
                    {
                        PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l,  (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom),3,3);

                    }


                }
                else if (vt_menu1 == menu_draw_border)
                {
                    if (keo_drawborder == true)
                    {

                        if (led_add.Count > 0)
                        {
                            for (int x = 0; x < led_add.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6 + imgy) * zoom);

                                if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                            }

                        }

                    }
                    else
                    {
                        PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }


                }
                else if (vt_menu1 == menu_draw_halo)
                {
                    if (keo_drawhalo == true)
                    {

                        if (led_add.Count > 0)
                        {
                            for (int x = 0; x < led_add.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6 + imgy) * zoom);

                                if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                            }

                        }

                    }
                    else
                    {
                        PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }


                }
                else if (vt_menu1 == menu_draw_matrix)
                {
                    if (keo_drawmatrix == true)
                    {

                        if (led_add.Count > 0)
                        {
                            for (int x = 0; x < led_add.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx) * zoom, (float)(led_add[x].Y - 6 + imgy) * zoom);

                                if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                            }

                        }

                    }
                    else
                    {
                        PointF point1_1 = new PointF((float)(vt_themx - 6 * zoom), (float)(vt_themy - 6 * zoom));
                        if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                        else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }


                }
                else if (vt_menu1 == menu_draw_text)
                {
                    

                        if (led_add.Count > 0)
                        {
                            for (int x = 0; x < led_add.Count; x++)
                            {
                                PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx +vt_add_line.X -text_size[2]+ into_text) * zoom, (float)(led_add[x].Y - 6 + imgy + vt_add_line.Y -text_size[3]) * zoom);

                                if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                                else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                            }

                        }




                }
                else if (vt_menu1 == menu_draw_dxf)
                {


                    if (led_add.Count > 0)
                    {
                        for (int x = 0; x < led_add.Count; x++)
                        {
                            PointF point1_1 = new PointF((float)(led_add[x].X - 6 + imgx + vt_add_line.X - text_size[2] + into_text) * zoom, (float)(led_add[x].Y - 6 + imgy + vt_add_line.Y - text_size[3]) * zoom);

                            if (nhay == true) DrawRoundedRectangle(e.Graphics, mau_l, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);
                            else DrawRoundedRectangle(e.Graphics, mau_l0, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);


                        }

                    }




                }
            }
             else if (vt_menu == menu_wire)
              {
                  if(keo_thucong==true && (vt_menu1==menu_wire_connect0|| vt_menu1 == menu_wire_connect1))
                {
                   

                    for (int x = 0; x < led_select.Count; x++)
                    {
                        PointF point1_1 = new PointF((float)(led_full[led_select[x]].X - 6 + imgx) * zoom, (float)(led_full[led_select[x]].Y - 6 + imgy) * zoom);

                        DrawRoundedRectangle_fill(e.Graphics, mauselect, (int)point1_1.X, (int)point1_1.Y, (int)(12 * zoom), (int)(12 * zoom), 3, 3);

                    }
                    if (led_select.Count > 1)
                    {
                        for (int x = 0; x < led_select.Count - 1; x++)
                        {
                            PointF point1 = new PointF((float)(led_full[led_select[x]].X + imgx) * zoom, (float)(led_full[led_select[x]].Y + imgy) * zoom);
                            PointF point2 = new PointF((float)(led_full[led_select[x+1]].X + imgx) * zoom, (float)(led_full[led_select[x+1]].Y + imgy) * zoom);
                             
                                DrawArrow(e.Graphics, mauday, point1, point2, 4 * zoom);
                            
                        }
                        double xxx = vt_chuot.X / zoom - imgx;
                        double yyy = vt_chuot.Y / zoom - imgy;

                        PointF point3 = new PointF((float)(led_full[led_select[led_select.Count - 1]].X + imgx) * zoom, (float)(led_full[led_select[led_select.Count - 1]].Y + imgy) * zoom);
                        PointF point4 = new PointF((float)(xxx + imgx) * zoom, (float)(yyy + imgy) * zoom);
                        DrawArrow(e.Graphics, mauday, point3, point4, 6 * zoom);
                    }

                    if(vt_menu1 == menu_wire_connect1)
                    {
                        double xxx = vt_chuot.X / zoom - imgx;
                        double yyy = vt_chuot.Y / zoom - imgy;
                        PointF point3 = new PointF((float)(led_full[vt_auto1].X + imgx) * zoom, (float)(led_full[vt_auto1].Y + imgy) * zoom);
                        PointF point4 = new PointF((float)(xxx + imgx) * zoom, (float)(yyy + imgy) * zoom);
                        DrawArrow(e.Graphics, maudaythang, point3, point4, 6 * zoom);
                    }
                }
              }
               */
        }

        PointF vt_add_line = new PointF(-1, -1);
        PointF vt_add_line1 = new PointF(-1, -1);
        double[] chuvivuong = new double[4];
        int chuvi = 10;
        int loai_matrix = 0;
        int loai_halo = 0;
        private void get_select_tt()
        {

            thongtin_select[0] = thongtin_select[1] = thongtin_select[2] = thongtin_select[3] = 0;


            double ttx = 0;
            double tty = 0;

            for (int x1 = 0; x1 < led_select.Count; x1++)
            {
                if (led_full[led_select[x1]].X >= ttx) ttx = led_full[led_select[x1]].X;
                if (led_full[led_select[x1]].Y >= tty) tty = led_full[led_select[x1]].Y;

            }
            double ttw = ttx;
            double tth = tty;
            for (int x1 = 0; x1 < led_select.Count; x1++)
            {
                if (led_full[led_select[x1]].X <= ttw) ttw = led_full[led_select[x1]].X;
                if (led_full[led_select[x1]].Y <= tth) tth = led_full[led_select[x1]].Y;

            }
            thongtin_select[0] = (float)ttw;
            thongtin_select[1] = (float)tth;
            thongtin_select[2] = (float)(ttx - ttw);
            thongtin_select[3] = (float)(tty - tth);



        }
        private void get_select_tt_tem()
        {

            thongtin_select[0] = thongtin_select[1] = thongtin_select[2] = thongtin_select[3] = 0;


            double ttx = 0;
            double tty = 0;

            for (int x1 = 0; x1 < led_select.Count; x1++)
            {
                if (led_tem_goc[led_select[x1]].X >= ttx) ttx = led_tem_goc[led_select[x1]].X;
                if (led_tem_goc[led_select[x1]].Y >= tty) tty = led_tem_goc[led_select[x1]].Y;

            }
            double ttw = ttx;
            double tth = tty;
            for (int x1 = 0; x1 < led_select.Count; x1++)
            {
                if (led_tem_goc[led_select[x1]].X <= ttw) ttw = led_tem_goc[led_select[x1]].X;
                if (led_tem_goc[led_select[x1]].Y <= tth) tth = led_tem_goc[led_select[x1]].Y;

            }

            thongtin_select[0] = (float)ttw;
            thongtin_select[1] = (float)tth;
            thongtin_select[2] = (float)(ttx - ttw);
            thongtin_select[3] = (float)(tty - tth);




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
        private float tim_goc1(PointF diem1, PointF diem2)
        {
            float goc = 0;
            ;

            float deltaY = diem2.Y - diem1.Y;
            float deltaX = diem2.X - diem1.X;
            goc = (float)(Math.Atan2((double)deltaY, (double)deltaX) * 180 / Math.PI);
            return goc;
        }
        float chieudaif(PointF a1, PointF a2)
        {

            return (float)(Math.Sqrt((a2.X - a1.X) * (a2.X - a1.X) + (a2.Y - a1.Y) * (a2.Y - a1.Y)));
        }
        private PointF[] getgoc(ANH_ANH ll)
        {
            PointF[] vt = new PointF[6] { new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1), new PointF(-1, -1) };

            PointF center = new PointF((ll.CENTER.X + imgx) * zoom, (ll.CENTER.Y + imgy) * zoom);

            // Bitmap anh_temp = (Bitmap)ResizeImage0(Properties.Resources.video, (int)(ll.SIZE.Width * zoom), (int)(ll.SIZE.Height * zoom));
            float w0 = ll.SIZE.Width * zoom;
            float h0 = ll.SIZE.Height * zoom;


            PointF[] aa = new PointF[4];

            // anh_temp = (Bitmap)RotateImg(anh_temp, (float)ll.GOC);
            // float w1 = anh_temp.Width;
            // float h1 = anh_temp.Height;
            aa[0] = new PointF((ll.CENTER.X - ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);
            aa[1] = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y - ll.SIZE.Height / 2 + imgy) * zoom);
            aa[2] = new PointF((ll.CENTER.X + ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + ll.SIZE.Height / 2 + imgy) * zoom);
            aa[3] = new PointF((ll.CENTER.X - ll.SIZE.Width / 2 + imgx) * zoom, (ll.CENTER.Y + ll.SIZE.Height / 2 + imgy) * zoom);
            aa[0] = RotatePoint(aa[0], center, ll.GOC);
            aa[1] = RotatePoint(aa[1], center, ll.GOC);
            aa[2] = RotatePoint(aa[2], center, ll.GOC);
            aa[3] = RotatePoint(aa[3], center, ll.GOC);

            float xx = 10000000;
            float yy = 10000000;
            float ww = 0;
            float hh = 0;
            for (int x = 0; x < 4; x++)
            {

                if (aa[x].X >= ww) ww = aa[x].X;

                if (aa[x].Y >= hh) hh = aa[x].Y;

                if (aa[x].X <= xx) xx = aa[x].X;

                if (aa[x].Y <= yy) yy = aa[x].Y;

            }




            float w1 = ww - xx;
            float h1 = hh - yy;


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
            ww = w1 - w0;
            hh = h1 - h0;
            // label2.Text = ww.ToString() + "   " + hh.ToString();
            //  gr.DrawImage(aa, new PointF(center.X - ww / 2 - ll.SIZE_VIDEO.Width / 2 * zoom, center.Y - hh / 2 - ll.SIZE_VIDEO.Height / 2 * zoom));
            vt[5] = new PointF(ww, hh);
            // anh_temp.Dispose();
            return vt;
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
        int toptipcu2 = -1;
        private void pic_day_MouseMove(object sender, MouseEventArgs e)
        {



            MouseEventArgs mouse = e as MouseEventArgs;
            if (mousepressed == true)
            {
                Point mousePosNow = e.Location;

                int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                int deltaY = mousePosNow.Y - mouseDown.Y;

                imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
                imgy = (int)(starty + (deltaY / zoom));
            }
            vt_themx = e.Location.X;
            vt_themy = e.Location.Y;
            vt_chuot = e.Location;
            if (vt_menu == menu_draw)  
            {
                if ( vt_menu1 == menu_draw_led)
                {

                }
                else if ( vt_menu1 == menu_draw_insert)
                {
                    diem_select_chen = check_insert_den(e.Location);
                }
                else if ( vt_menu1 == menu_draw_delete)
                {
                    diem_select_xoa = check_den(e.Location.X, e.Location.Y);
                }
                else if ( vt_menu1 == menu_draw_line)
                {
                    if (keo_drawline == true)
                    {
                        vt_add_line1 = new PointF(mouse.Location.X / zoom - imgx, mouse.Location.Y / zoom - imgy);



                        if (vt_add_line.X >= 0 && vt_add_line.Y >= 0 && vt_add_line1.X >= 0 && vt_add_line1.Y >= 0)
                        {

                            float[] aa = chuyenvt(vt_add_line.X, vt_add_line.Y, vt_add_line1.X, vt_add_line1.Y);
                            PointF pointA1 = new PointF(aa[0], aa[1]);
                            PointF pointB1 = new PointF(aa[2], aa[3]);
                            PointF pointA = new PointF(vt_add_line.X, vt_add_line.Y);
                            PointF pointB = new PointF(vt_add_line1.X, vt_add_line1.Y);

                            // g.DrawLine(Pens.White, pointA, pointB);


                            var diff_X = pointA.X - pointB.X;
                            var diff_Y = pointA.Y - pointB.Y;
                            int pointNum = (int)num_line.Value;

                            float interval_X = diff_X / (pointNum - 1);
                            float interval_Y = diff_Y / (pointNum - 1);

                            led_add.Clear();

                            for (int i = 0; i < pointNum; i++)
                            {

                                // g.DrawEllipse(Pens.Red, new Rectangle((int)(pointA.X - (interval_X) * i), (int)(pointA.Y - (interval_Y) * i), 12, 12));
                                led_add.Add(new LED((pointA.X - (interval_X) * i), (pointA.Y - (interval_Y) * i)));

                            }


                            chuvivuong = getanh(led_add);


                            //  textBox1.Text = led_add.Count.ToString();
                            //toolTip1.SetToolTip(pictureBox1, Math.Round(chieudaiF(pointA, pointB)).ToString() + "mm");



                            // g.FillRectangle(new SolidBrush(Color.FromArgb(32, 128, 128, 128)), new RectangleF(pointA1.X - 6, pointA1.Y - 6, pointB1.X - pointA1.X + 24, pointB1.Y - pointA1.Y + 24));

                        }
                    }

                }
                else if ( vt_menu1 == menu_draw_border)
                {
                    if (keo_drawborder == true)
                    {
                        vt_add_line1 = new PointF(mouse.Location.X / zoom - imgx, mouse.Location.Y / zoom - imgy);



                        if (vt_add_line.X >= 0 && vt_add_line.Y >= 0 && vt_add_line1.X >= 0 && vt_add_line1.Y >= 0)
                        {

                            if (combo_vien.SelectedIndex == 0)
                            {
                                PointF pointA = new PointF(vt_add_line.X, vt_add_line.Y);
                                PointF pointB = new PointF(vt_add_line1.X, vt_add_line1.Y);
                                float cc = chieudaiF(pointA, pointB) * 2;
                                chuvi = (int)cc;
                                var diff_X = (pointA.X - pointB.X) * 2;
                                var diff_Y = (pointA.Y - pointB.Y) * 2;

                                //  g.DrawEllipse(Pens.White, pointA.X - cc / 2 + 6, pointA.Y - cc / 2 + 6, cc, cc);


                                int socot = (int)num_vien1.Value;
                                int sohang = 1;
                                float kichthuoc1 = cc;
                                float kichthuoc2 = cc;
                                List<PointF> diem = new List<PointF>();


                                if (socot > 0 && sohang > 0)
                                {

                                    float khoangcach = (kichthuoc1 - kichthuoc2) / sohang;

                                    for (int aa = 0; aa < sohang; aa++)
                                    {

                                        PointF[] star_points = MakeStarPointsF(-Math.PI / 2, socot, 0, new RectangleF(pointA.X + khoangcach * aa / 2, pointA.Y + khoangcach * aa / 2, kichthuoc1 - khoangcach * aa, kichthuoc1 - khoangcach * aa));


                                        for (int x = 0; x < star_points.Length; x++)
                                        {
                                            diem.Add(new PointF(star_points[x].X - cc / 2, star_points[x].Y - cc / 2));

                                        }

                                    }
                                    led_add.Clear();
                                    for (int a2 = 0; a2 < socot; a2++)
                                    {
                                        for (int a1 = 0; a1 < sohang; a1++)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            // if (kieu_chay == 0) pointList.Add(diem[a1 * socot + a2]);
                                            //  else if (kieu_chay == 1) pointList.Add(diem[a1 * socot + socot - 1 - a2]);

                                            // g.FillEllipse(dd1, diem[a1 * socot + a2].X, diem[a1 * socot + a2].Y, 12, 12);
                                            led_add.Add(new LED(diem[a1 * socot + a2].X, diem[a1 * socot + a2].Y));

                                        }
                                    }
                                    //toolTip1.SetToolTip(pictureBox1, Math.Round(cc).ToString() + " mm");
                                    // g.FillEllipse(new SolidBrush(Color.FromArgb(32, 128, 128, 128)), new RectangleF(pointA.X - cc / 2 - 6, pointA.Y - cc / 2 - 6, cc + 24, cc + 24));


                                }
                            }
                            else if (combo_vien.SelectedIndex == 1)
                            {

                                int tongx = (int)num_vien1.Value;
                                int tongy = (int)num_vien2.Value;

                                //  List<Point> diem = new List<Point>();
                                if (tongx > 0 && tongy > 0)
                                {



                                    float[] aa = chuyenvt(vt_add_line.X, vt_add_line.Y, vt_add_line1.X, vt_add_line1.Y);
                                    PointF pointA = new PointF(aa[0], aa[1]);
                                    PointF pointB = new PointF(aa[2], aa[3]);



                                    var diff_X = pointB.X - pointA.X;
                                    var diff_Y = pointB.Y - pointA.Y;

                                    float interval_X = diff_X / (tongx - 1);
                                    float interval_Y = diff_Y / (tongy - 1);

                                    if (tongx > 0 && tongy > 0 && interval_X >= 0 && interval_Y >= 0)
                                    {

                                        led_add.Clear();
                                        for (int x1 = 0; x1 < tongx; x1++)
                                        {
                                            //  g.FillEllipse(dd1, new Rectangle((int)(pointA.X + interval_X * x1), (int)(pointA.Y + interval_Y * 0),12,12));
                                            led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * 0));
                                        }
                                        for (int x2 = 1; x2 < tongy; x2++)
                                        {
                                            // g.FillEllipse(dd1, new Rectangle((int)(pointA.X + interval_X * (tongx - 1)), (int)(pointA.Y + interval_Y * x2), 12, 12));
                                            led_add.Add(new LED(pointA.X + interval_X * (tongx - 1), pointA.Y + interval_Y * x2));
                                        }
                                        for (int x1 = tongx - 2; x1 >= 0; x1--)
                                        {
                                            // g.FillEllipse(dd1, new Rectangle((int)(pointA.X + interval_X * x1), (int)(pointA.Y + interval_Y * (tongy - 1)), 12, 12));
                                            led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * (tongy - 1)));

                                        }
                                        for (int x2 = tongy - 2; x2 > 0; x2--)
                                        {
                                            // g.FillEllipse(dd1, new Rectangle((int)(pointA.X + interval_X * 0), (int)(pointA.Y + interval_Y * x2), 12, 12));
                                            led_add.Add(new LED(pointA.X + interval_X * 0, pointA.Y + interval_Y * x2));
                                        }

                                        chuvivuong = getanh(led_add);
                                        //  toolTip1.SetToolTip(pictureBox1, (Math.Round(chuvivuong[2]).ToString() + " - " + Math.Round(chuvivuong[3]).ToString() + " mm"));
                                        // g.FillRectangle(new SolidBrush(Color.FromArgb(32, 128, 128, 128)), new Rectangle((int)aa[0] - 6, (int)aa[1] - 6, (int)(aa[2] - aa[0] + 24), (int)(aa[3] - aa[1] + 24)));

                                        //draw_ledadd(g);




                                    }

                                }



                            }

                        }
                    }

                }
                else if ( vt_menu1 == menu_draw_halo)
                {
                    if (keo_drawhalo == true)
                    {
                        vt_add_line1 = new PointF(mouse.Location.X / zoom - imgx, mouse.Location.Y / zoom - imgy);



                        if (vt_add_line.X >= 0 && vt_add_line.Y >= 0 && vt_add_line1.X >= 0 && vt_add_line1.Y >= 0)
                        {
                            PointF pointA = new PointF(vt_add_line.X, vt_add_line.Y);
                            PointF pointB = new PointF(vt_add_line1.X, vt_add_line1.Y);
                            float cc = chieudaiF(pointA, pointB) * 2;
                            chuvi = (int)cc;
                            var diff_X = (pointA.X - pointB.X) * 2;
                            var diff_Y = (pointA.Y - pointB.Y) * 2;

                            // g.DrawEllipse(Pens.White, vt_add_line.X - cc / 2 + 6, vt_add_line.Y - cc / 2 + 6, cc, cc);
                            int socot = (int)num_bachtuot1.Value;
                            int sohang = (int)num_bachtuot2.Value;

                            float kichthuoc1 = cc;
                            float kichthuoc2 = cc;
                            List<LED> diem = new List<LED>();


                            if (socot > 0 && sohang > 0)
                            {

                                float khoangcach = (kichthuoc1 - (int)num_bachtuot3.Value * 10) / sohang;

                                for (int aa = 0; aa < sohang; aa++)
                                {

                                    PointF[] star_points = MakeStarPointsF(-Math.PI / 2, socot, 0, new RectangleF(pointA.X + khoangcach * aa / 2, pointA.Y + khoangcach * aa / 2, kichthuoc1 - khoangcach * aa, kichthuoc1 - khoangcach * aa));


                                    for (int x = 0; x < star_points.Length; x++)
                                    {
                                        diem.Add(new LED(star_points[x].X - cc / 2, star_points[x].Y - cc / 2));

                                    }

                                }




                                led_add.Clear();
                                if (loai_halo == 0)
                                {
                                    for (int a2 = 0; a2 < socot; a2++)
                                    {
                                        for (int a1 = 0; a1 < sohang; a1++)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            led_add.Add(diem[a1 * (int)socot + a2]);




                                        }
                                    }
                                }
                                else if (loai_halo == 1)
                                {
                                    for (int a2 = 0; a2 < socot; a2++)
                                    {
                                        for (int a1 = 0; a1 < sohang; a1++)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            if (a2 % 2 == 0) led_add.Add(diem[a1 * (int)socot + a2]);
                                            else led_add.Add(diem[(sohang - a1 - 1) * (int)socot + a2]);




                                        }
                                    }
                                }
                                if (loai_halo == 2)
                                {
                                    for (int a2 = 0; a2 < socot; a2++)
                                    {
                                        for (int a1 = sohang - 1; a1 >= 0; a1--)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            led_add.Add(diem[a1 * (int)socot + a2]);




                                        }
                                    }
                                }
                                else if (loai_halo == 3)
                                {
                                    for (int a2 = 0; a2 < socot; a2++)
                                    {
                                        for (int a1 = sohang - 1; a1 >= 0; a1--)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            if (a2 % 2 == 0) led_add.Add(diem[a1 * (int)socot + a2]);
                                            else led_add.Add(diem[(sohang - a1 - 1) * (int)socot + a2]);




                                        }
                                    }
                                }
                                else if (loai_halo == 4)
                                {

                                    for (int a1 = 0; a1 < sohang; a1++)
                                    {
                                        led_add.Add(diem[a1 * (int)socot]);
                                    }

                                    for (int a2 = socot - 1; a2 > 0; a2--)
                                    {
                                        for (int a1 = 0; a1 < sohang; a1++)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            led_add.Add(diem[a1 * (int)socot + a2]);




                                        }
                                    }
                                }
                                else if (loai_halo == 5)
                                {

                                    for (int a1 = 0; a1 < sohang; a1++)
                                    {
                                        led_add.Add(diem[a1 * (int)socot]);
                                    }

                                    for (int a2 = socot - 1; a2 > 0; a2--)
                                    {
                                        for (int a1 = 0; a1 < sohang; a1++)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            if (a2 % 2 == 0) led_add.Add(diem[a1 * (int)socot + a2]);
                                            else led_add.Add(diem[(sohang - a1 - 1) * (int)socot + a2]);





                                        }
                                    }
                                }
                                if (loai_halo == 6)
                                {
                                    for (int a1 = sohang - 1; a1 >= 0; a1--)
                                    {
                                        led_add.Add(diem[a1 * (int)socot]);
                                    }
                                    for (int a2 = socot - 1; a2 > 0; a2--)
                                    {
                                        for (int a1 = sohang - 1; a1 >= 0; a1--)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            led_add.Add(diem[a1 * (int)socot + a2]);




                                        }
                                    }
                                }
                                if (loai_halo == 7)
                                {
                                    for (int a1 = sohang - 1; a1 >= 0; a1--)
                                    {
                                        led_add.Add(diem[a1 * (int)socot]);
                                    }
                                    for (int a2 = socot - 1; a2 > 0; a2--)
                                    {
                                        for (int a1 = sohang - 1; a1 >= 0; a1--)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            if (a2 % 2 == 0) led_add.Add(diem[a1 * (int)socot + a2]);
                                            else led_add.Add(diem[(sohang - a1 - 1) * (int)socot + a2]);



                                        }
                                    }
                                }
                                if (loai_halo == 8)
                                {
                                    for (int a1 = 0; a1 < sohang; a1++)
                                    {
                                        for (int a2 = 0; a2 < socot; a2++)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            led_add.Add(diem[a1 * (int)socot + a2]);




                                        }
                                    }
                                }
                                if (loai_halo == 9)
                                {
                                    for (int a1 = sohang - 1; a1 >= 0; a1--)
                                    {
                                        for (int a2 = 0; a2 < socot; a2++)
                                        {
                                            // gr.DrawLine(mau_day, new System.Drawing.Point(diem1[0 + a1 * 16 + a2] + 4, diem2[0 + a1 * 16 + a2] + 4), new System.Drawing.Point(diem1[16 + a1 * 16 + a2] + 4, diem2[16 + a1 * 16 + a2] + 4));
                                            led_add.Add(diem[a1 * (int)socot + a2]);




                                        }
                                    }
                                }



                            }


                        }
                    }

                }
                else if ( vt_menu1 == menu_draw_matrix)
                {
                    if (keo_drawmatrix == true)
                    {
                        vt_add_line1 = new PointF(mouse.Location.X / zoom - imgx, mouse.Location.Y / zoom - imgy);



                        if (vt_add_line.X >= 0 && vt_add_line.Y >= 0 && vt_add_line1.X >= 0 && vt_add_line1.Y >= 0)
                        {

                            int tongx = (int)num_matrix1.Value;
                            int tongy = (int)num_matrix2.Value;
                            //  List<Point> diem = new List<Point>();
                            if (tongx > 0 && tongy > 0)
                            {


                                float[] aa = chuyenvt(vt_add_line.X, vt_add_line.Y, vt_add_line1.X, vt_add_line1.Y);
                                PointF pointA = new PointF(aa[0], aa[1]);
                                PointF pointB = new PointF(aa[2], aa[3]);

                                // g.DrawRectangle(Pens.White, new Rectangle((int)aa[0] + 6, (int)aa[1] + 6, (int)(aa[2] - aa[0]), (int)(aa[3] - aa[1])));


                                var diff_X = pointB.X - pointA.X;
                                var diff_Y = pointB.Y - pointA.Y;

                                float interval_X = diff_X / (tongx - 1);
                                float interval_Y = diff_Y / (tongy - 1);

                                if (tongx > 0 && tongy > 0 && interval_X >= 0 && interval_Y >= 0)
                                {

                                    led_add.Clear();

                                    if (loai_matrix == 0 || loai_matrix == 1)
                                    {
                                        for (int x2 = 0; x2 < tongy; x2++)
                                        {

                                            for (int x1 = 0; x1 < tongx; x1++)
                                            {

                                                if (loai_matrix == 0)
                                                {
                                                    if (x2 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * x1, pointA.Y + interval_Y * x2));
                                                }
                                                else if (loai_matrix == 1)
                                                {
                                                    if (x2 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * (tongx - x1 - 1), pointA.Y + interval_Y * x2));
                                                }



                                            }


                                        }
                                    }
                                    else if (loai_matrix == 2 || loai_matrix == 3)
                                    {
                                        for (int x1 = 0; x1 < tongx; x1++)
                                        {

                                            for (int x2 = 0; x2 < tongy; x2++)
                                            {

                                                if (loai_matrix == 2)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * x2));
                                                }
                                                else if (loai_matrix == 3)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * (tongy - x2 - 1)));
                                                }



                                            }


                                        }
                                    }
                                    else if (loai_matrix == 4 || loai_matrix == 5)
                                    {
                                        for (int x2 = 0; x2 < tongy; x2++)
                                        {

                                            for (int x1 = tongx - 1; x1 >= 0; x1--)
                                            {

                                                if (loai_matrix == 4)
                                                {
                                                    if (x2 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * x1, pointA.Y + interval_Y * x2));
                                                }
                                                else if (loai_matrix == 5)
                                                {
                                                    if (x2 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * (tongx - x1 - 1), pointA.Y + interval_Y * x2));
                                                }



                                            }


                                        }
                                    }
                                    else if (loai_matrix == 6 || loai_matrix == 15)
                                    {
                                        for (int x1 = tongx - 1; x1 >= 0; x1--)
                                        {

                                            for (int x2 = tongy - 1; x2 >= 0; x2--)
                                            {

                                                if (loai_matrix == 6)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * x2));
                                                }
                                                else if (loai_matrix == 15)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * (tongy - x2 - 1)));
                                                }



                                            }


                                        }
                                    }
                                    else if (loai_matrix == 8 || loai_matrix == 9)
                                    {
                                        for (int x1 = 0; x1 < tongx; x1++)
                                        {

                                            for (int x2 = tongy - 1; x2 >= 0; x2--)
                                            {

                                                if (loai_matrix == 8)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * x2));
                                                }
                                                else if (loai_matrix == 9)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * (tongy - x2 - 1)));
                                                }



                                            }


                                        }
                                    }
                                    else if (loai_matrix == 10 || loai_matrix == 11)
                                    {
                                        for (int x2 = tongy - 1; x2 >= 0; x2--)
                                        {

                                            for (int x1 = 0; x1 < tongx; x1++)
                                            {

                                                if (loai_matrix == 10)
                                                {
                                                    if (x2 % 2 == 1) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * x1, pointA.Y + interval_Y * x2));
                                                }
                                                else if (loai_matrix == 11)
                                                {
                                                    if (x2 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * (tongx - x1 - 1), pointA.Y + interval_Y * x2));
                                                }



                                            }


                                        }
                                    }
                                    else if (loai_matrix == 12 || loai_matrix == 13)
                                    {
                                        for (int x2 = tongy - 1; x2 >= 0; x2--)
                                        {

                                            for (int x1 = tongx - 1; x1 >= 0; x1--)
                                            {

                                                if (loai_matrix == 12)
                                                {
                                                    if (x2 % 2 == 1) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * x1, pointA.Y + interval_Y * x2));
                                                }
                                                else if (loai_matrix == 13)
                                                {
                                                    if (x2 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED((pointA.X + interval_X * (tongx - 1)) - interval_X * (tongx - x1 - 1), pointA.Y + interval_Y * x2));
                                                }



                                            }


                                        }
                                    }
                                    else if (loai_matrix == 14 || loai_matrix == 7)
                                    {
                                        for (int x1 = tongx - 1; x1 >= 0; x1--)
                                        {

                                            for (int x2 = 0; x2 < tongy; x2++)
                                            {

                                                if (loai_matrix == 14)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * x2));
                                                }
                                                else if (loai_matrix == 7)
                                                {
                                                    if (x1 % 2 == 0) led_add.Add(new LED(pointA.X + interval_X * x1, pointA.Y + interval_Y * x2));
                                                    else led_add.Add(new LED(pointA.X + interval_X * x1, (pointA.Y + interval_Y * (tongy - 1)) - interval_Y * (tongy - x2 - 1)));
                                                }



                                            }


                                        }

                                    }
                                    chuvivuong = getanh(led_add);


                                }

                            }

                        }
                    }

                }
                else if ( vt_menu1 == menu_draw_text)
                {

                    vt_add_line = new PointF(mouse.Location.X / zoom - imgx, mouse.Location.Y / zoom - imgy);



                    if (vt_add_line.X >= 0 && vt_add_line.Y >= 0)
                    {


                        chuvivuong = getanh(led_add);

                    }


                }
                else if ( vt_menu1 == menu_draw_dxf)
                {

                    vt_add_line = new PointF(mouse.Location.X / zoom - imgx, mouse.Location.Y / zoom - imgy);



                    if (vt_add_line.X >= 0 && vt_add_line.Y >= 0)
                    {


                        chuvivuong = getanh(led_add);

                    }


                }

            }
            else if (vt_menu == menu_select)
            {
                if (kt_select == true)
                {
                    vt_select1 = new Point(mouse.Location.X, mouse.Location.Y);

                }
                else if (keo_select == true)
                {



                    if (mouse.Location.X > 1 && mouse.Location.X < pic_map.Width - 4 && mouse.Location.Y > 1 && mouse.Location.Y < pic_map.Height - 4)
                    {

                        if (loai_keo == 0)
                        {
                            for (int x = 0; x < led_select.Count; x++)
                            {
                                led_full[led_select[x]] = new LED(led_full[led_select[x]].X + (mouse.Location.X / zoom - imgx - vt_dichuyen.X), led_full[led_select[x]].Y + (mouse.Location.Y / zoom - imgy - vt_dichuyen.Y));
                            }
                            // checkloi();
                            get_select_tt();
                            /*
                            if (check_dichuyen.Checked == true)
                            {
                                imgx = (int)(imgx - mouse.Location.X + vt_dichuyen1.X);
                                imgy = (int)(imgy - mouse.Location.Y + vt_dichuyen1.Y);
                            }
                            */
                        }
                        else if (loai_keo == 3)
                        {
                            get_select_tt_tem();
                            float gg1 = tim_goc1(new PointF(thongtin_select[0] + thongtin_select[2] / 2, thongtin_select[1] + thongtin_select[3] / 2), new PointF(thongtin_select[0] + thongtin_select[2], thongtin_select[1]));
                            float gg2 = tim_goc1(new PointF(thongtin_select[0] + thongtin_select[2] / 2, thongtin_select[1] + thongtin_select[3] / 2), new PointF((int)(mouse.Location.X / zoom - imgx), (int)(mouse.Location.Y / zoom - imgy)));

                            PointF aa1 = new PointF(thongtin_select[0] + thongtin_select[2] / 2, thongtin_select[1] + thongtin_select[3] / 2);

                            for (int x = 0; x < led_select.Count; x++)
                            {
                                PointF aa = new PointF((float)led_tem_goc[led_select[x]].X, (float)led_tem_goc[led_select[x]].Y);
                                aa = RotatePoint(aa, aa1, gg2 - gg1);


                                led_full[led_select[x]] = new LED(aa.X, aa.Y);
                            }
                            //checkloi();
                            get_select_tt();

                        }
                        else if (loai_keo == 1)
                        {

                            get_select_tt_tem();

                            float tile = (mouse.Location.X / zoom - imgx - thongtin_select[0]) / thongtin_select[2];
                            for (int x = 0; x < led_select.Count; x++)
                            {


                                led_full[led_select[x]] = new LED(((led_tem_goc[led_select[x]].X - thongtin_select[0])) * tile + thongtin_select[0], led_tem_goc[led_select[x]].Y);

                            }

                            //checkloi();
                            get_select_tt();

                        }
                        else if (loai_keo == 2)
                        {

                            get_select_tt_tem();

                            float tile = (mouse.Location.Y / zoom - imgy - thongtin_select[1]) / thongtin_select[3];
                            for (int x = 0; x < led_select.Count; x++)
                            {
                                led_full[led_select[x]] = new LED(led_tem_goc[led_select[x]].X, ((led_tem_goc[led_select[x]].Y - thongtin_select[1]) * tile) + thongtin_select[1]);
                            }

                            // checkloi();
                            get_select_tt();

                        }
                        else if (loai_keo == 4)
                        {

                            get_select_tt_tem();

                            float tile = (mouse.Location.Y / zoom - imgy - thongtin_select[1]) / thongtin_select[3];
                            float tile1 = (mouse.Location.X / zoom - imgx - thongtin_select[0]) / thongtin_select[2];
                            for (int x = 0; x < led_select.Count; x++)
                            {
                                led_full[led_select[x]] = new LED((led_tem_goc[led_select[x]].X - thongtin_select[0]) * tile1 + thongtin_select[0], (led_tem_goc[led_select[x]].Y - thongtin_select[1]) * tile + thongtin_select[1]);
                            }

                            // checkloi();
                            get_select_tt();

                        }
                        else if (loai_keo == 10)
                        {
                            if (vt_dichuyen_1den >= 0) led_full[vt_dichuyen_1den] = new LED(led_full[vt_dichuyen_1den].X + (mouse.Location.X / zoom - imgx - vt_dichuyen.X), led_full[vt_dichuyen_1den].Y + (mouse.Location.Y / zoom - imgy - vt_dichuyen.Y));


                        }
                    }

                    vt_dichuyen = new PointF(mouse.Location.X / zoom - imgx, mouse.Location.Y / zoom - imgy);

                    // get_select_tt();
                    vt_dichuyen1 = mouse.Location;
                    // imgx = imgx + (int)(e.Location.X / zoom - imgx - vt_dichuyen.X);
                    // imgy = imgy + (int)(e.Location.Y / zoom - imgy - vt_dichuyen.Y);

                    // Application.DoEvents();
                }
                else
                {
                    int vvv = check_den(mouse.Location.X, mouse.Location.Y);
                    if (vvv != toptipcu)
                    {
                        if (vvv >= 0)
                        {

                            toolTip1.SetToolTip(pic_map, "LED: " + vvv.ToString() + " ");

                        }
                        toptipcu2 = vvv;
                    }
                }

            }
            else if (vt_menu == menu_wire)
            {
                if(vt_menu1==menu_wire_connect0)
                {
                    if (keo_thucong == true)
                    {

                        int vtc = check_den(mouse.Location.X, mouse.Location.Y);
                        if (vtc >= 0 && check_den_select(vtc) == -1 && (ModifierKeys & Keys.Shift) != Keys.Shift)
                        {

                            led_select.Add(vtc);       
                        }



                    }
                    vt_chuot = mouse.Location;
                }
                else if (vt_menu1 == menu_wire_connect1)
                {
                    if (keo_thucong == true)
                    {


                       

                           
                            cu.Clear();


                            
                            double xxx = mouse.Location.X / zoom - imgx;
                            double yyy = mouse.Location.Y / zoom - imgy;

                            PointF z1 = new PointF((int)led_full[vt_auto1].X, (int)led_full[vt_auto1].Y);
                            PointF z2 = new PointF((int)xxx, (int)yyy);

                            for (int x = 0; x < led_full.Count; x++)
                            {

                                if (PointOnLineSegment1(z1, z2, new PointF((float)led_full[x].X, (float)led_full[x].Y), 6) == true && check_den_select(x) == -1)
                                {
                                    cu.Add(x);
                                }
                            }

                         


                    }
                    vt_chuot = mouse.Location;
                }
            }
            else if (vt_menu == menu_tool )
            {
                if(list_anh.Count>0 && vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count && loaikeo_video >= 0)
                {
                  
                    if (loaikeo_video == 5)
                        {


                              list_anh[vt_chon_anh].CENTER = new PointF
                                 (
                                        center_luu.X + (e.Location.X - vt_chuot_anh.X) / zoom,
                                        center_luu.Y + (e.Location.Y - vt_chuot_anh.Y) / zoom
                                 );

                           
                            //vt_chuot = e.Location;
                        }
                        else if (loaikeo_video == 0)
                        {
                            if (vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count && list_anh[vt_chon_anh].SIZE.Height > 1)
                            {

                                float cc = chieudaif(e.Location, vt_chuot_anh);

                                if (chieudaif(new PointF(e.Location.X, e.Location.Y), new PointF((center_luu.X + imgx) * zoom, (center_luu.Y + imgy) * zoom)) > size_luu.Height * zoom / 2)
                                {
                                    cc = -cc;

                                }
                                if (size_luu.Height - (cc) / zoom >= 8)
                                {
                                    list_anh[vt_chon_anh].SIZE = new SizeF
                                (
                                       size_luu.Width,
                                       size_luu.Height - (cc) / zoom
                                );
                                    PointF[] vv = getgoc(list_anh[vt_chon_anh]);
                                    float ggg1 = (float)(Math.PI * list_anh[vt_chon_anh].GOC / 180.0);
                                    float yy1 = (float)((cc) / zoom * Math.Cos(ggg1));
                                    float xx1 = (float)((cc) / zoom * Math.Sin(ggg1));

                                    list_anh[vt_chon_anh].CENTER = new PointF
                                    (
                                           center_luu.X - xx1 / 2,
                                           center_luu.Y + yy1 / 2
                                    );

                                }
                            }
                        }
                        else if (loaikeo_video == 1)
                        {

                            if (vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count && list_anh[vt_chon_anh].SIZE.Width > 1)
                            {

                                float cc = -chieudaif(e.Location, vt_chuot_anh);

                                if (chieudaif(new PointF(e.Location.X, e.Location.Y), new PointF((center_luu.X + imgx) * zoom, (center_luu.Y + imgy) * zoom)) > size_luu.Width * zoom / 2)
                                {
                                    cc = -cc;

                                }
                                if (size_luu.Width + (cc) / zoom >= 8)
                                {
                                    list_anh[vt_chon_anh].SIZE = new SizeF
                                (
                                       size_luu.Width + (cc) / zoom,
                                       size_luu.Height
                                );
                                    PointF[] vv = getgoc(list_anh[vt_chon_anh]);
                                    float ggg1 = (float)(Math.PI * list_anh[vt_chon_anh].GOC / 180.0);
                                    float yy1 = (float)((cc) / zoom * Math.Cos(ggg1));
                                    float xx1 = (float)((cc) / zoom * Math.Sin(ggg1));

                                    list_anh[vt_chon_anh].CENTER = new PointF
                                    (
                                           center_luu.X + yy1 / 2,
                                           center_luu.Y + xx1 / 2
                                    );

                                }
                            }
                            //vt_chuot = e.Location;
                        }
                        else if (loaikeo_video == 2)
                        {

                            if (vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count && list_anh[vt_chon_anh].SIZE.Height > 1)
                            {

                                float cc = -chieudaif(e.Location, vt_chuot_anh);

                                if (chieudaif(new PointF(e.Location.X, e.Location.Y), new PointF((center_luu.X + imgx) * zoom, (center_luu.Y + imgy) * zoom)) > size_luu.Height * zoom / 2)
                                {
                                    cc = -cc;

                                }
                                if (size_luu.Height + cc / zoom >= 8)
                                {
                                    list_anh[vt_chon_anh].SIZE = new SizeF
                                (
                                       size_luu.Width,
                                       size_luu.Height + cc / zoom
                                );
                                    PointF[] vv = getgoc(list_anh[vt_chon_anh]);
                                    float ggg1 = (float)(Math.PI * list_anh[vt_chon_anh].GOC / 180.0);
                                    float yy1 = (float)(cc / zoom * Math.Cos(ggg1));
                                    float xx1 = (float)(cc / zoom * Math.Sin(ggg1));

                                    list_anh[vt_chon_anh].CENTER = new PointF
                                    (
                                           center_luu.X - xx1 / 2,
                                           center_luu.Y + yy1 / 2
                                    );
                                }

                            }
                            //vt_chuot = e.Location;
                        }
                        else if (loaikeo_video == 3)
                        {

                            if (vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count && list_anh[vt_chon_anh].SIZE.Width > 1)
                            {
                                float cc = chieudaif(e.Location, vt_chuot_anh);

                                if (chieudaif(new PointF(e.Location.X, e.Location.Y), new PointF((center_luu.X + imgx) * zoom, (center_luu.Y + imgy) * zoom)) > size_luu.Width * zoom / 2)
                                {
                                    cc = -cc;

                                }

                                if (size_luu.Width - (cc) / zoom >= 8)
                                {
                                    list_anh[vt_chon_anh].SIZE = new SizeF
                                (
                                       size_luu.Width - (cc) / zoom,
                                       size_luu.Height
                                );
                                    PointF[] vv = getgoc(list_anh[vt_chon_anh]);
                                    float ggg1 = (float)(Math.PI * list_anh[vt_chon_anh].GOC / 180.0);
                                    float yy1 = (float)((cc) / zoom * Math.Cos(ggg1));
                                    float xx1 = (float)((cc) / zoom * Math.Sin(ggg1));

                                    list_anh[vt_chon_anh].CENTER = new PointF
                                    (
                                           center_luu.X + yy1 / 2,
                                           center_luu.Y + xx1 / 2
                                    );

                                }
                            }
                            //vt_chuot = e.Location;
                        }
                        if (loaikeo_video == 4)
                        {

                            if (vt_chon_anh >= 0 && vt_chon_anh < list_anh.Count)
                            {
                                PointF[] vv = get_thongtin_video(list_anh[vt_chon_anh]);
                                float gg1 = tim_gocf(vv[9], new PointF((vt_chuot_anh.X), (vt_chuot_anh.Y)));
                                float gg2 = tim_gocf(vv[9], new PointF((e.Location.X), (e.Location.Y)));


                                list_anh[vt_chon_anh].GOC = goc_luu + Math.Round(gg2 - gg1);


                            }
                            //  vt_chuot = e.Location;

                        }
                    
                }
            }




        }
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
        private void pic_day_MouseUp(object sender, MouseEventArgs e)
        {
            if (mousepressed == true) mousepressed = false;
            if (keo_thucong == true)
            {
                keo_thucong = false;
                if (vt_menu1 == menu_wire_connect0)
                {
                    





                    int mm = -1;

                    List<LED> vt_den_temp = new List<LED>();

                    for (int x = 0; x < led_select.Count; x++)
                    {
                        vt_den_temp.Add(led_full[led_select[x]]);
                        if (vt_port2 > 0)
                        {
                            if (vt_port2 == led_select[x]) mm = vt_den_temp.Count - 1;
                        }
                    }
                    for (int x = 0; x < led_full.Count; x++)
                    {
                        if (check_den_select(x) == -1)
                        {
                            vt_den_temp.Add(led_full[x]);
                            if (vt_port2 > 0)
                            {
                                if (vt_port2 == x) mm = vt_den_temp.Count - 1;
                            }
                        }

                    }
                    for (int x = 0; x < led_full.Count; x++) led_full[x] = vt_den_temp[x];
                    if (vt_port2 > 0)
                    {
                        vt_port2 = mm;
                    }
                    vt_auto1 = -1;
                    led_select.Clear();
                 

                }
                else if (vt_menu1 == menu_wire_connect1)
                {
                    
                    if (cu.Count >0)
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

                    List<LED> vt_den_temp = new List<LED>();

                    for (int x = 0; x < led_select.Count; x++)
                    {
                        vt_den_temp.Add(led_full[led_select[x]]);
                        if (vt_port2 > 0)
                        {
                            if (vt_port2 == led_select[x]) mm = vt_den_temp.Count - 1;
                        }
                    }
                    for (int x = 0; x < led_full.Count; x++)
                    {
                        if (check_den_select(x) == -1)
                        {
                            vt_den_temp.Add(led_full[x]);
                            if (vt_port2 > 0)
                            {
                                if (vt_port2 == x) mm = vt_den_temp.Count - 1;
                            }
                        }

                    }
                    for (int x = 0; x < led_full.Count; x++) led_full[x] = vt_den_temp[x];
                    if (vt_port2 > 0)
                    {
                        vt_port2 = mm;
                    }
                    vt_auto1 = -1;
                    led_select.Clear();
                    cu.Clear();
                }

               
            }
            if (loaikeo_video >= 0)
            {
                loaikeo_video = -1;
       
            }
            if (keo_drawline == true)
            {
                keo_drawline = false;
                add_undo();
                for (int i = 0; i < led_add.Count; i++) led_full.Add(new LED(led_add[i].X , led_add[i].Y));
                led_add.Clear();
            }
            if (keo_drawborder == true)
            {
                keo_drawborder = false;
                add_undo();
                for (int i = 0; i < led_add.Count; i++) led_full.Add(new LED(led_add[i].X, led_add[i].Y));
                led_add.Clear();
            }
            if (keo_drawhalo == true)
            {
                keo_drawhalo = false;
                add_undo();
                for (int i = 0; i < led_add.Count; i++) led_full.Add(new LED(led_add[i].X, led_add[i].Y));
                led_add.Clear();
            }
            if (keo_drawmatrix== true)
            {
                keo_drawmatrix = false;
                add_undo();
                for (int i = 0; i < led_add.Count; i++) led_full.Add(new LED(led_add[i].X, led_add[i].Y));
                led_add.Clear();
            }

            if (keo_select == true)
            {
                keo_select = false;
               
            }

            if (kt_select == true)
            {
                
                kt_select = false;
                float[] aa = chuyenvt(vt_select0.X, vt_select0.Y, vt_select1.X, vt_select1.Y);

                float xx1 = aa[2], xx2 = aa[0], yy1 = aa[3], yy2 = aa[1];
                if (xx1 >= 0 && xx2 >= 0 && yy1 >= 0 && yy2 >= 0)
                {


                    float vtx1 = xx1 / zoom - imgx;
                    float vty1 = yy1 / zoom - imgy;
                    float vtx2 = xx2 / zoom - imgx;
                    float vty2 = yy2 / zoom - imgy;


                    if (loaikeochon == 0)
                    {
                        for (int x = 0; x < led_full.Count; x++)
                        {

                            if (led_full[x].X >= vtx2 && led_full[x].X <= vtx1 + 6 && led_full[x].Y >= vty2 - 6 && led_full[x].Y <= vty1 + 6)
                            {

                                if (check_den_select(x) >= 0) led_select.RemoveAt(check_den_select(x));



                            }

                        }

                    }
                    else if (loaikeochon == 1)
                    {

                        for (int x = 0; x < led_full.Count; x++)
                        {

                            if (led_full[x].X >= vtx2 && led_full[x].X <= vtx1 + 6 && led_full[x].Y >= vty2 - 6 && led_full[x].Y <= vty1 + 6)
                            {

                                led_select.Add(x);

                            }

                        }
                    }


                    //    e.Graphics.DrawRectangle(dashed_pen, new Rectangle((int)(xx2), (int)(yy2), (int)(xx1 - xx2), (int)((yy1 - yy2))));
                }


                get_select_tt();
                 
            }
        }
        private int check_den_select(int vt)
        {

            for (int x = 0; x < led_select.Count; x++)
            {
                if (vt == led_select[x]) return x;
            }


            return -1;
        }
        double[] text_size = new double[4];
        private void draw_text()
        {
            timer.Enabled = false;
            timer_map.Enabled = false;
            led_add.Clear();
  
          
                StringFormat fmt = new StringFormat();
                fmt.Alignment = StringAlignment.Near;
                GraphicsPath gp1 = new GraphicsPath();
                using (FontFamily ff = new FontFamily(fontListBox1.Items[fontListBox1.SelectedIndex].ToString()))
                    gp1.AddString(text_chu.Text, ff, 1, (int)num_size.Value, new Point(0, 0), StringFormat.GenericTypographic);


                Rectangle r = Rectangle.Round(gp1.GetBounds());

                // MessageBox.Show(r.Width.ToString()+"   "+ r.Height.ToString());
                Bitmap aa = new Bitmap(r.Width + 200, r.Height + 200);
                Graphics g1 = Graphics.FromImage(aa);
                g1.SmoothingMode = SmoothingMode.AntiAlias;

                Pen dashed_pen = new Pen(Color.White, 1);

                // dashed_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                dashed_pen.DashPattern = new float[] { 1, (int)num_khoangcach.Value };

                g1.Clear(Color.Transparent);
                g1.DrawPath(dashed_pen, gp1);

                int minx = 1000000;
                int miny = 1000000;

                for (int x = 0; x < aa.Width; x++)
                {

                    for (int y = 0; y < aa.Height; y++)
                    {
                        if (aa.GetPixel(x, y).A > 0)
                        {
                            if (x <= minx) minx = x;
                            if (y <= miny) miny = y;
                        led_add.Add(new LED(x, y));
                        }

                    }
                }

                List<int> vt_sat = new List<int>();
                for (int xx1 = 0; xx1 < led_add.Count - 1; xx1++)
                {
                    for (int xx2 = xx1 + 1; xx2 < led_add.Count; xx2++)
                    {
                        double cc = chieudailed(led_add[xx1], led_add[xx2]);
                        if (cc <= 3)
                        {
                            vt_sat.Add(xx1);
                        }


                    }

                }
                if (vt_sat.Count > 0)
                {


                    List<LED> vt_den1 = new List<LED>();

                    for (int x = 0; x < led_add.Count; x++)
                    {
                        int kt = -1;
                        for (int y = 0; y < vt_sat.Count; y++)
                        {
                            if (x == vt_sat[y]) kt = 1;
                        }
                        if (kt == -1) vt_den1.Add(led_add[x]);
                    }
                led_add.Clear();
                    for (int x = 0; x < vt_den1.Count; x++)
                    {

                    led_add.Add(new LED(vt_den1[x].X - minx, vt_den1[x].Y - miny));

                    }

                    List<LED> vt_den_temp = new List<LED>();
                    List<LED> vt_den_temp1 = new List<LED>();

                    for (int i = 0; i < led_add.Count; i++) vt_den_temp.Add(led_add[i]);
                    int chay = led_add.Count - 1;
                    double max = 0;
                    int vt = 0;
                    vt_den_temp1.Add(vt_den_temp[0]);
                    vt_den_temp.RemoveAt(0);
                    while (chay > 0)
                    {
                        max = 10000;
                        vt = 0;
                        for (int i = 0; i < vt_den_temp.Count; i++)
                        {
                            // MessageBox.Show(chieudai(vt_den_auto[vt_den_auto.Count - 1], vt_den_temp[i]).ToString());
                            if (chieudailed(vt_den_temp1[vt_den_temp1.Count - 1], vt_den_temp[i]) <= max)
                            {
                                max = chieudailed(vt_den_temp1[vt_den_temp1.Count - 1], vt_den_temp[i]);
                                vt = i;
                            }

                        }
                        vt_den_temp1.Add(vt_den_temp[vt]);
                        vt_den_temp.RemoveAt(vt);
                        chay--;
                    }
                    for (int i = 0; i < led_add.Count; i++) led_add[i] = vt_den_temp1[i];

                    text_size = getanh(led_add);
                }
                g1.Dispose();
            timer.Enabled = true;
            timer_map.Enabled = true;
        }
        float chieudaiF(PointF a1, PointF a2)
        {

            return (float)(Math.Sqrt((a2.X - a1.X) * (a2.X - a1.X) + (a2.Y - a1.Y) * (a2.Y - a1.Y)));
        }
        double chieudailed(LED a1, LED a2)
        {

            return (double)(Math.Sqrt((a2.X - a1.X) * (a2.X - a1.X) + (a2.Y - a1.Y) * (a2.Y - a1.Y)));
        }
        private double CalculateConcaveRadius(int num_points, int skip)
        {

            // For really small numbers of points.
            if (num_points < 5) return 0.33f;

            // Calculate angles to key points.
            double dtheta = 2 * Math.PI / num_points;
            double theta00 = -Math.PI / 2;
            double theta01 = theta00 + dtheta * skip;
            double theta10 = theta00 + dtheta;
            double theta11 = theta10 - dtheta * skip;

            // Find the key points.
            PointF pt00 = new PointF(
                (float)Math.Cos(theta00),
                (float)Math.Sin(theta00));
            PointF pt01 = new PointF(
                (float)Math.Cos(theta01),
                (float)Math.Sin(theta01));
            PointF pt10 = new PointF(
                (float)Math.Cos(theta10),
                (float)Math.Sin(theta10));
            PointF pt11 = new PointF(
                (float)Math.Cos(theta11),
                (float)Math.Sin(theta11));

            // See where the segments connecting the points intersect.
            bool lines_intersect, segments_intersect;
            PointF intersection, close_p1, close_p2;
            FindIntersection(pt00, pt01, pt10, pt11,
                out lines_intersect, out segments_intersect,
                out intersection, out close_p1, out close_p2);

            // Calculate the distance between the
            // point of intersection and the center.
            return Math.Sqrt(
                intersection.X * intersection.X +
                intersection.Y * intersection.Y);
        }

        // Find the point of intersection between
        // the lines p1 --> p2 and p3 --> p4.
        private void FindIntersection(
            PointF p1, PointF p2, PointF p3, PointF p4,
            out bool lines_intersect, out bool segments_intersect,
            out PointF intersection,
            out PointF close_p1, out PointF close_p2)
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 =
                ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34)
                    / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new PointF(float.NaN, float.NaN);
                close_p1 = new PointF(float.NaN, float.NaN);
                close_p2 = new PointF(float.NaN, float.NaN);
                return;
            }
            lines_intersect = true;

            float t2 =
                ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12)
                    / -denominator;

            // Find the point of intersection.
            intersection = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new PointF(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }
        private PointF[] MakeStarPointsF(double start_theta, int num_points, int skip, RectangleF rect)
        {
            double theta, dtheta;
            PointF[] result;
            float rx = rect.Width / 2f;
            float ry = rect.Height / 2f;
            float cx = rect.X + rx;
            float cy = rect.Y + ry;

            // If this is a polygon, don't bother with concave points.


            // Find the radius for the concave vertices.
            double concave_radius = CalculateConcaveRadius(num_points, skip);

            // Make the points.
            result = new PointF[num_points];
            theta = start_theta;
            dtheta = Math.PI / num_points;
            for (int i = 0; i < num_points; i++)
            {
                result[i] = new PointF(
                    (float)(cx + rx * Math.Cos(theta)),
                    (float)(cy + ry * Math.Sin(theta)));
                theta += dtheta;

                theta += dtheta;
            }
            return result;
        }
        private double[] getanh(List<LED> bien)
        {
            double[] aa = new double[4];
            aa[0] = aa[1] = aa[2] = aa[3] = 0;


            double ttx = 0;
            double tty = 0;

            for (int x1 = 0; x1 < bien.Count; x1++)
            {
                if (bien[x1].X >= ttx) ttx = bien[x1].X;
                if (bien[x1].Y >= tty) tty = bien[x1].Y;

            }
            double ttw = ttx;
            double tth = tty;
            for (int x1 = 0; x1 < bien.Count; x1++)
            {
                if (bien[x1].X <= ttw) ttw = bien[x1].X;
                if (bien[x1].Y <= tth) tth = bien[x1].Y;

            }
            aa[0] = ttw;
            aa[1] = tth;
            aa[2] = ttx - ttw;
            aa[3] = tty - tth;


            return aa;
        }
        private float[] chuyenvt(float x1, float y1, float x2, float y2)
        {
            float[] chuyen = new float[4];

            float xx1 = 0, xx2 = 0, yy1 = 0, yy2 = 0;

            if (x1 > x2) { xx1 = x1; xx2 = x2; }
            else { xx1 = x2; xx2 = x1; }
            if (y1 > y2) { yy1 = y1; yy2 = y2; }
            else { yy1 = y2; yy2 = y1; }

            chuyen[0] = xx2;
            chuyen[1] = yy2;
            chuyen[2] = xx1;
            chuyen[3] = yy1;

            return chuyen;

        }
        private int check_insert_den(PointF kt)
        {
            float xxx = kt.X / zoom - imgx;
            float yyy = kt.Y/ zoom - imgy;

            for (int x = 0; x < led_full.Count - 1; x++)
            {
                if (PointOnLineSegment(new Point((int)led_full[x].X, (int)led_full[x].Y), new Point((int)led_full[x + 1].X, (int)led_full[x + 1].Y), new Point((int)(xxx), (int)(yyy))) == true)
                {

                    return x;
                }

            }


            return -1;
        }
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
        public static bool PointOnLineSegment(Point pt1, Point pt2, Point pt)
        {
            double epsilon = 3;
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
        private bool PointIsInPolygon(PointF[] polygon, PointF target_point)
        {
            // Make a GraphicsPath containing the polygon.
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(polygon);

            // See if the point is inside the path.
            return path.IsVisible(target_point);
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

        bool well(Control con, MouseEventArgs e, ref int vt_hien, int max_hien)
        {
            if (con.ClientRectangle.Contains(con.PointToClient(Cursor.Position)))
            {
                bool lan555 = true;
                Point pt_MouseAb555 = Control.MousePosition;
                // MessageBox.Show(e.Delta.ToString());
                do
                {
                    Rectangle r_Ctrl555 = con.RectangleToScreen(con.ClientRectangle);
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
            Point pt_MouseAb555 = Control.MousePosition;
            // MessageBox.Show(e.Delta.ToString());
            do
            {
                Rectangle r_Ctrl555 = con.RectangleToScreen(con.ClientRectangle);
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

                Point mousePosNow = e.Location;
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

        bool check_zoom = true;
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (well(pic_wire, e, ref vt_hien_wire, max_hien_wire))
            { 
            }
            else well1(pic_map, e);


 
        }

        private void num_vien1_ValueChanged(object sender, EventArgs e)
        {
            if (combo_vien.SelectedIndex == 0) num_vien2.Value = num_vien1.Value;
        }

        private void num_vien2_ValueChanged(object sender, EventArgs e)
        {
            if (combo_vien.SelectedIndex == 0) num_vien1.Value = num_vien2.Value;
        }

        private void pic_halo_Paint(object sender, PaintEventArgs e)
        {
            if (nhay == true)
            {
                int xx = loai_halo % 4;
                int yy = loai_halo / 4;

                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(1,151,251), 2), xx * 40+1, yy * 40+1, 39, 39);
                

            }
        }

        private void pic_halo_MouseDown(object sender, MouseEventArgs e)
        {
            int xx = e.Location.X/40;
            int yy = e.Location.Y / 40;
            loai_halo = xx + yy * 4;
            if (loai_halo >= 10) loai_halo = 9;
        }

        private void pic_matrix_MouseDown(object sender, MouseEventArgs e)
        {
            int xx = e.Location.X / 40;
            int yy = e.Location.Y / 40;
            loai_matrix = xx + yy * 4;
            if (loai_matrix >= 16) loai_halo = 16;
        }

        private void pic_matrix_Paint(object sender, PaintEventArgs e)
        {
            if (nhay == true)
            {
                int xx = loai_matrix % 4;
                int yy = loai_matrix / 4;

                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(1, 151, 251), 2), xx * 40 + 1, yy * 40 + 1, 39, 39);


            }
        }

        private void text_chu_TextChanged(object sender, EventArgs e)
        {
          
            if (vt_menu == menu_draw && vt_menu1 == menu_draw_text && text_chu.TextLength > 0)
            {
                draw_text();
                
            }
        }

        private void fontListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fontListBox1.SelectedIndex <= 0) fontListBox1.SelectedIndex = 0;
            if (vt_menu == menu_draw && vt_menu1 == menu_draw_text && text_chu.TextLength > 0)
            {
                draw_text();

            }
        }

        private void num_size_ValueChanged(object sender, EventArgs e)
        {
            draw_text();
        }

        private void num_khoangcach_ValueChanged(object sender, EventArgs e)
        {
            draw_text();
        }

        private void panel_draw_text_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void panel_draw_text_MouseLeave(object sender, EventArgs e)
        {
            into_text =0;
        }

        private void panel_draw_text_MouseEnter(object sender, EventArgs e)
        {
           
        }

        private void fontListBox1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void pic_wire_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 64, 64)), new Rectangle(0, 0,100, 16));
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 64, 64)), new Rectangle(0, pic_wire.Height - 16, 100, 16));
            // e.Graphics.DrawImage(Properties.Resources.len, (pic_thuvien_nen.Width - Properties.Resources.len.Width) / 2, 4);
            // e.Graphics.DrawImage(Properties.Resources.xuong, (pic_thuvien_nen.Width - Properties.Resources.len.Width) / 2, pic_thuvien_nen.Height - Properties.Resources.len.Height - 7);
            using (Font font1 = new Font("Microsoft Sans Serif", 9, FontStyle.Bold, GraphicsUnit.Point))
            {
                e.Graphics.DrawString("Total: "+list_wire.Count.ToString(), font1, Brushes.White, 105, 1) ;

                e.Graphics.DrawString(">>>" , font1, Brushes.White, 5, 0);
                e.Graphics.DrawString("<<<" , font1, Brushes.White, 5, pic_wire.Height - 18);

            }
              


            using (Font font1 = new Font("Microsoft Sans Serif", 9, FontStyle.Regular, GraphicsUnit.Point))
            {
                for (int x = 0; x < max_cao_wire; x++)
                {

                    for (int y = 0; y < max_rong_wire; y++)
                    {

                        if (x * max_rong_wire + y + vt_hien_wire * max_rong_wire < list_wire.Count)
                        {
                            
                            string link = list_wire[x * max_rong_wire + y + vt_hien_wire * max_rong_wire];



                            var rmCrypto = GetAlgorithm();

                            ICryptoTransform decryptor = rmCrypto.CreateDecryptor();
                            try
                            {
                                using (var reader = new StreamReader(new CryptoStream(System.IO.File.OpenRead(link), decryptor, CryptoStreamMode.Read)))
                                {
                                    LUU_MAP temp1 = JsonConvert.DeserializeObject<LUU_MAP>(reader.ReadToEnd());



                                    Bitmap aaaa = (Bitmap)byteArrayToImage(temp1.ANH);
                                    e.Graphics.DrawImage(aaaa, new Rectangle(1 + 164 * y, 19 + 124 * x, aaaa.Width, aaaa.Height), new Rectangle(0, 0, aaaa.Width, aaaa.Height), GraphicsUnit.Pixel);

                                    e.Graphics.DrawRectangle(Pens.Gray, 1 + 164 * y, 19 + 124 * x, 160, 120);

                                    RectangleF rectF1 = new RectangleF(2 + 164 * y, 19 + 124 * x + 104, 160, 20);
                                    string hhhh = Path.GetFileNameWithoutExtension(link);
                                    string ff;
                                    if (hhhh.Length < 16) ff = hhhh;
                                    else ff = hhhh.Substring(0, 16);

                                    e.Graphics.DrawString(ff, font1, Brushes.Red, rectF1);

                                }
                            }
                            catch
                            {
                               
                            }






                            
                           
                          

                        }
                    }
                }
            }
        }

        private void pic_wire_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y <= 16)
            {
                if (vt_hien_wire> 0)
                {
                    vt_hien_wire--;
                    pic_wire.Refresh();
                }
            }
            else if (e.Y >= pic_wire.Height - 16)
            {
                
                if (max_hien_wire > 0)
                {
                    if (vt_hien_wire < max_hien_wire)
                    {
                        vt_hien_wire++;
                        pic_wire.Refresh();
                    }
                }
            }
            else
            {
                if (e.X >= 5 && e.X <= pic_wire.Width - 5)
                {

                    int xx = (e.X - 3) / 164;
                    int yy = (e.Y - 16) / 124;


                    vt_chon_wire = yy * max_rong_wire + xx + vt_hien_wire * max_rong_wire;


                    


                    if (vt_chon_wire >= list_wire.Count) vt_chon_wire = -1;
                    pic_wire.Refresh();
                    if (vt_chon_wire >= 0 && vt_chon_wire< list_wire.Count)
                    {

                        led_add.Clear();

                        var rmCrypto = GetAlgorithm();

                        ICryptoTransform decryptor = rmCrypto.CreateDecryptor();
                        try
                        {
                            using (var reader = new StreamReader(new CryptoStream(System.IO.File.OpenRead(list_wire[vt_chon_wire]), decryptor, CryptoStreamMode.Read)))
                            {
                                LUU_MAP temp1 = JsonConvert.DeserializeObject<LUU_MAP>(reader.ReadToEnd());

                                led_add = temp1.DATA;
                                double minx = 1000000;
                                double miny = 1000000;
                                for (int x = 0; x < led_add.Count; x++)
                                {
                                    if (led_add[x].X <= minx) minx = led_add[x].X;
                                    if (led_add[x].Y <= miny) miny = led_add[x].Y;
                                }
                                for (int x = 0; x < led_add.Count; x++)
                                {
                                    led_add[x] = new LED(led_add[x].X - minx, led_add[x].Y - miny);

                                }

                                text_size = getanh(led_add);


                            }
                        }
                        catch
                        {

                        }





 
                        //toolStripMenuItem2.Enabled = true;
                    }
                    else
                    {


                        // toolStripMenuItem2.Enabled = false;
                    }


                }

            }
        }
        private string getDecimalSymbol()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey("Control Panel\\International");
            return (string)reg.GetValue("sDecimal");
        }
        private List<LED> read_ledfull(string name)
        {

            List<LED> ledok = new List<LED>();
            List<LED> ledok1 = new List<LED>();
            List<List<LED>> spin = new List<List<LED>>();


            int x1 = 10000, y1 = 10000, y2 = 0;
            DXFDocument doc = new DXFDocument();
            doc.Load(name);

            int donvi = 1;
            if (doc.Header.Donvi != null)
            {

                int don = (int)doc.Header.Donvi;
                if (don == 1) donvi = 25;
                else if (don == 2) donvi = 308;
                else if (don == 3) donvi = 1609350;
                else if (don == 4) donvi = 1;
                else if (don == 5) donvi = 10;
                else if (don == 6) donvi = 1000;
                else if (don == 14) donvi = 100;
            }

            List<int> vx = new List<int>();
            List<int> vy = new List<int>();

            string line, li;
            int counter = 0;

            int kk = 0;
            if (getDecimalSymbol() == ".") kk = 1;

            try
            {
                using (StreamReader file = new System.IO.StreamReader(name))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line == "SPLINE")
                        {
                            counter++;
                            line = "";

                            float xx0 = 0;
                            float yy0 = 0;
                            List<float> zx = new List<float>();
                            List<float> zy = new List<float>();
                            while (line != "  0")
                            {
                                line = file.ReadLine();//.Replace(" ", "");

                                if (line == " 10")
                                {
                                    li = file.ReadLine();

                                    Application.DoEvents();
                                    if (kk == 0) li = li.Replace(".", ",");
                                    zx.Add((float)Math.Ceiling(float.Parse(li) * donvi));
                                }

                                if (line == " 20")
                                {
                                    li = file.ReadLine();

                                    Application.DoEvents();
                                    if (kk == 0) li = li.Replace(".", ",");
                                    zy.Add((float)Math.Ceiling(float.Parse(li) * donvi));
                                }

                            };
                            float max = 0;

                            for (int x = 0; x < zx.Count; x++) { xx0 = xx0 + zx[x]; };
                            for (int x = 0; x < zy.Count; x++) { yy0 = yy0 + zy[x]; };
                            float ttx = xx0 / zx.Count;
                            float tty = yy0 / zy.Count;

                            for (int x = 0; x < zx.Count; x++)
                            {
                                float ttt = chieudaiF(new PointF(ttx, tty), new PointF(zx[x], zy[x]));
                                if (ttt >= max) max = ttt;
                            };
                            if (max >= 3F && max <= 12F)
                            {
                                vx.Add((int)ttx);
                                vy.Add((int)tty);
                            }
                        }
                        else if (line == "CIRCLE")
                        {

                            counter++;

                            while ((line = file.ReadLine().Replace(" ", "")) != "10") { };
                            line = file.ReadLine();
                            if (kk == 0) line = line.Replace(".", ",");
                            vx.Add((int)Math.Ceiling(float.Parse(line) * donvi));
                            while ((line = file.ReadLine().Replace(" ", "")) != "20") { };
                            line = file.ReadLine();
                            if (kk == 0) line = line.Replace(".", ",");
                            vy.Add((int)Math.Ceiling(float.Parse(line) * donvi));

                        }
                        else if (line == "ELLIPSE")
                        {
                            float max = 0;
                            float zx = 0;
                            float zy = 0;
                            float tv = 0;
                            counter++;
                            while ((line = file.ReadLine().Replace(" ", "")) != "10") { };
                            line = file.ReadLine();
                            if (kk == 0) line = line.Replace(".", ",");
                            zx = (float)float.Parse(line) * donvi;

                            while ((line = file.ReadLine().Replace(" ", "")) != "20") { };
                            line = file.ReadLine();
                            if (kk == 0) line = line.Replace(".", ",");
                            zy = (float)float.Parse(line) * donvi;

                            while ((line = file.ReadLine().Replace(" ", "")) != "11") { };
                            line = file.ReadLine();
                            if (kk == 0) line = line.Replace(".", ",");
                            tv = float.Parse(line) * 2 * donvi;
                            if (tv >= max) max = tv;

                            while ((line = file.ReadLine().Replace(" ", "")) != "21") { };
                            line = file.ReadLine();
                            if (kk == 0) line = line.Replace(".", ",");
                            tv = float.Parse(line) * 2 * donvi;
                            if (tv >= max) max = tv;

                            while ((line = file.ReadLine().Replace(" ", "")) != "31") { };
                            line = file.ReadLine();
                            if (kk == 0) line = line.Replace(".", ",");
                            tv = float.Parse(line) * 2 * donvi;
                            if (tv >= max) max = tv;

                            while ((line = file.ReadLine().Replace(" ", "")) != "0") { };

                            if (max >= 3 && max <= 13)
                            {
                                vx.Add((int)zx);
                                vy.Add((int)zy);

                            }

                        }

                    }

                    file.Close();

                    for (int x = 0; x < vx.Count; x++)
                    {
                        if (vx[x] < x1) x1 = vx[x];
                        if (vy[x] < y1) y1 = vy[x];
                        if (vy[x] > y2) y2 = vy[x];
                    }


                    if (vx.Count > 0)
                    {


                        // MessageBox.Show(vx.Count.ToString());

                        for (int x = 0; x < vx.Count; x++)
                        {
                            // e.Graphics.DrawLine(m, Convert.ToInt32(listBox3.Items[x].ToString()), Convert.ToInt32(listBox2.Items[x].ToString()), 2, 2);

                            if (x1 < 0)
                            {
                                ledok.Add(new LED(vx[x] + Math.Abs(x1) + 50, y2 - vy[x] + 50));

                            }
                            else
                            {
                                ledok.Add(new LED(vx[x] - x1 + 50, y2 - vy[x] + 50));

                            }


                        }



                    }




                    double[] aaaa = new double[4];
                    double ttx1 = 0;
                    double tty1 = 0;

                    for (int x2 = 0; x2 < ledok.Count; x2++)
                    {
                        if (ledok[x2].X >= ttx1) ttx1 = ledok[x2].X;
                        if (ledok[x2].Y >= tty1) tty1 = ledok[x2].Y;

                    }
                    double ttw1 = ttx1;
                    double tth1 = tty1;
                    for (int x2 = 0; x2 < ledok.Count; x2++)
                    {
                        if (ledok[x2].X <= ttw1) ttw1 = ledok[x2].X;
                        if (ledok[x2].Y <= tth1) tth1 = ledok[x2].Y;

                    }


                    aaaa[0] = ttw1;
                    aaaa[1] = tth1;
                    aaaa[2] = ttx1 - ttw1;
                    aaaa[3] = tty1 - tth1;


                    for (int x = 0; x < ledok.Count; x++)
                    {
                        ledok1.Add(new LED(ledok[x].X - ttw1 + 10, ledok[x].Y - tth1 + 10));


                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ledok1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
            open.Filter = "DXF (*.dxf)|*.dxf";
            if (open.ShowDialog() != DialogResult.OK)
            {

                return;

            }
            else
            {
               
                Application.DoEvents();
                add_undo();
                 led_add= read_ledfull(open.FileName);
                
                get_select_tt();
                text_size = getanh(led_add);
               
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && e.Control)
            {

                undo(); //list_undo.Items.RemoveAt(list_undo.Items.Count - 1);
            };


            if (e.KeyCode == Keys.C && e.Control && vt_menu == menu_select)
            {
                copy_vung();

            };


            if (e.KeyCode == Keys.A && e.Control && vt_menu==menu_select)
            {

                led_select.Clear();
                for (int x = 0; x < led_full.Count; x++)
                {
                    led_select.Add(x);

                }
                get_select_tt();
               
            }
            if (e.KeyCode == Keys.Delete)
            {
                del_selset();
            }
            if (text_chu.ClientRectangle.Contains(text_chu.PointToClient(Cursor.Position))==false)
            {
                if (e.KeyCode == Keys.R)
                {
                    reset_moitruong();
                }
            }
        }

        private void pic_map_DragLeave(object sender, EventArgs e)
        {
            
        }

        private void pic_map_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            if (vt_chon_wire>= 0 && vt_chon_wire < list_wire.Count)
            {

                string ff = list_wire[vt_chon_wire];

                if (File.Exists(ff) == true)
                {
                    File.Delete(ff);

                }
                
                hien_list_diday();
                

            }
        }
        int toptipcu = 0;
        private void pic_menu_MouseMove(object sender, MouseEventArgs e)
        {
            int vv = e.X / 49;
            
            
                if (vv >= 0 && vv < 17)
                {

                  toolTip1.SetToolTip(pic_menu, ngonngu_menu[menu_ngonngu, vv] + "   ");
            }
            
            
       

        }


        int toptipcu1 = 0;

        private void pic_menu1_MouseMove(object sender, MouseEventArgs e)
        {
            int vv = e.Y / 49;
           
                if (vt_menu == menu_select)
                {
                    if (vv >= 0 && vv < 9)
                    {
                        toolTip1.SetToolTip(pic_menu1, ngonngu_select[menu_ngonngu, vv] + "   ");
                    }
                }
                else if (vt_menu == menu_draw)
                {
                    if (vv >= 0 && vv < 9)
                    {
                        toolTip1.SetToolTip(pic_menu1, ngonngu_draw[menu_ngonngu, vv] + "   ");
                    }
                }
                else if (vt_menu == menu_wire)
                {
                    if (vv >= 0 && vv < 6)
                    {
                        toolTip1.SetToolTip(pic_menu1, ngonngu_wire[menu_ngonngu, vv] + "   ");
                    }
                }
                else if (vt_menu == menu_tool)
                {
                    if (vv >= 0 && vv < 4)
                    {
                        toolTip1.SetToolTip(pic_menu1, ngonngu_tool[menu_ngonngu, vv] + "   ");
                    }
                }
             
        }

        private void pic_menu_MouseLeave(object sender, EventArgs e)
        {
            toptipcu = -1;
        }

        private void pic_menu1_MouseLeave(object sender, EventArgs e)
        {
            toptipcu1 = -1;
        }

        

        private void Form1_Resize(object sender, EventArgs e)
        {
            timer_map.Enabled = false;
            if (pic_map.Width > 0 && pic_map.Height > 0)
                moi = new Bitmap(pic_map.Width, pic_map.Height);
            timer_map.Enabled = true;
        }

        private void pic_map_MouseEnter(object sender, EventArgs e)
        {
            pic_map.Focus();
        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            Font f = new Font("Microsoft Sans Serif", 10f);
            toolTip1.BackColor = Color.FromArgb(32, 32, 32);
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.White, new PointF(2, 2));
        }
    }

    public class LED
    {
        public double X { get; set; }
        public double Y { get; set; }
        
        public LED(double x, double y)
        {
            X = x;
            Y = y;
          
        }
        // Other properties, methods, events...
    }
    public class StringNum : IComparable<StringNum>
    {

        private List<string> _strings;
        private List<int> _numbers;

        public StringNum(string value)
        {
            _strings = new List<string>();
            _numbers = new List<int>();
            int pos = 0;
            bool number = false;
            while (pos < value.Length)
            {
                int len = 0;
                while (pos + len < value.Length && Char.IsDigit(value[pos + len]) == number)
                {
                    len++;
                }
                if (number)
                {
                    _numbers.Add(int.Parse(value.Substring(pos, len)));
                }
                else
                {
                    _strings.Add(value.Substring(pos, len));
                }
                pos += len;
                number = !number;
            }
        }

        public int CompareTo(StringNum other)
        {
            int index = 0;
            while (index < _strings.Count && index < other._strings.Count)
            {
                int result = _strings[index].CompareTo(other._strings[index]);
                if (result != 0) return result;
                if (index < _numbers.Count && index < other._numbers.Count)
                {
                    result = _numbers[index].CompareTo(other._numbers[index]);
                    if (result != 0) return result;
                }
                else
                {
                    return index == _numbers.Count && index == other._numbers.Count ? 0 : index == _numbers.Count ? -1 : 1;
                }
                index++;
            }
            return index == _strings.Count && index == other._strings.Count ? 0 : index == _strings.Count ? -1 : 1;
        }

    }
    class ControlMover
    {
        public enum Direction
        {
            Any,
            Horizontal,
            Vertical
        }

        public static void Init(Control control)
        {
            Init(control, Direction.Any);
        }

        public static void Init(Control control, Direction direction)
        {
            Init(control, control, direction);
        }

        public static void Init(Control control, Control container, Direction direction)
        {
            bool Dragging = false;
            Point DragStart = Point.Empty;
            control.MouseDown += delegate (object sender, MouseEventArgs e)
            {
                Dragging = true;
                DragStart = new Point(e.X, e.Y);
                control.Capture = true;
            };
            control.MouseUp += delegate (object sender, MouseEventArgs e)
            {
                Dragging = false;
                control.Capture = false;
            };
            control.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                if (Dragging)
                {
                    if (direction != Direction.Vertical)
                        container.Left = Math.Max(0, e.X + container.Left - DragStart.X);
                    if (direction != Direction.Horizontal)
                        container.Top = Math.Max(0, e.Y + container.Top - DragStart.Y);
                }
            };
        }
    }
    public class ANH_ANH
    {
        public PointF CENTER { get; set; }
        public SizeF SIZE { get; set; }
        public Image ANH { get; set; }
        public double GOC { get; set; }
        public int DIM { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public ANH_ANH(Image ANH, PointF CENTER, SizeF SIZE, double GOC, int DIM)
        {
            this.ANH = ANH;
            this.CENTER = CENTER;
            this.SIZE = SIZE;
            this.GOC = GOC;
            this.DIM = DIM;
        }
    }
    public class ANH_ANHL
    {
        public PointF CENTER { get; set; }
        public SizeF SIZE { get; set; }
        public byte[] ANH { get; set; }
        public double GOC { get; set; }
        public int DIM { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public ANH_ANHL(byte[] ANH, PointF CENTER, SizeF SIZE, double GOC, int DIM)
        {
            this.ANH = ANH;
            this.CENTER = CENTER;
            this.SIZE = SIZE;
            this.GOC = GOC;
            this.DIM = DIM;
        }
    }


    public class LUU_MAP
    {
        public byte[] ANH { get; set; }
        public SizeF SIZE { get; set; }
        public List<LED> DATA { get; set; }
        public int TONG { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
   
        public LUU_MAP(byte[] ANH, Size SIZE, List<LED> DATA, int TONG)
        {
            this.ANH = ANH;
            this.SIZE = SIZE;
            this.DATA = DATA;
            this.TONG = TONG;
           
        }
    }
    public class LUU_MAPOK
    {
        public List<LED> DATA { get; set; }
        public List<ANH_ANHL> ANH { get; set; }
 

        public int PORT { get; set; }
  
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public LUU_MAPOK(List<LED> DATA, List<ANH_ANHL> ANH, int PORT)
        {
            this.DATA = DATA;
            this.ANH = ANH;
            this.PORT = PORT;

        }
    }
    class IniFile   // revision 11
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
    public class IniParser
    {
        private Hashtable keyPairs = new Hashtable();
        private String iniFilePath;

        private struct SectionPair
        {
            public String Section;
            public String Key;
        }

        /// <summary>
        /// Opens the INI file at the given path and enumerates the values in the IniParser.
        /// </summary>
        /// <param name="iniPath">Full path to INI file.</param>
        public IniParser(String iniPath)
        {
            TextReader iniFile = null;
            String strLine = null;
            String currentRoot = null;
            String[] keyPair = null;

            iniFilePath = iniPath;

            if (File.Exists(iniPath))
            {
                try
                {
                    iniFile = new StreamReader(iniPath);

                    strLine = iniFile.ReadLine();

                    while (strLine != null)
                    {
                        strLine = strLine.Trim();

                        if (strLine != "")
                        {
                            if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                            {
                                currentRoot = strLine.Substring(1, strLine.Length - 2);
                            }
                            else
                            {
                                keyPair = strLine.Split(new char[] { '=' }, 2);

                                SectionPair sectionPair;
                                String value = null;

                                if (currentRoot == null)
                                    currentRoot = "ROOT";

                                sectionPair.Section = currentRoot;
                                sectionPair.Key = keyPair[0];

                                if (keyPair.Length > 1)
                                    value = keyPair[1];

                                keyPairs.Add(sectionPair, value);
                            }
                        }

                        strLine = iniFile.ReadLine();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (iniFile != null)
                        iniFile.Close();
                }
            }
            else
                throw new FileNotFoundException("Unable to locate " + iniPath);

        }

        /// <summary>
        /// Returns the value for the given section, key pair.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <param name="settingName">Key name.</param>
        public String GetSetting(String sectionName, String settingName)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;
            String ff = (String)keyPairs[sectionPair];
            if (ff != null)
            {
                ff = ff.Replace("##", System.Environment.NewLine);
            }
            else
            {

            }

            return ff;
        }

        /// <summary>
        /// Enumerates all lines for given section.
        /// </summary>
        /// <param name="sectionName">Section to enum.</param>
        public String[] EnumSection(String sectionName)
        {
            ArrayList tmpArray = new ArrayList();

            foreach (SectionPair pair in keyPairs.Keys)
            {
                if (pair.Section == sectionName)
                    tmpArray.Add(pair.Key);
            }

            return (String[])tmpArray.ToArray(typeof(String));
        }

        /// <summary>
        /// Adds or replaces a setting to the table to be saved.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        /// <param name="settingValue">Value of key.</param>
        public void AddSetting(String sectionName, String settingName, String settingValue)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;

            if (keyPairs.ContainsKey(sectionPair))
                keyPairs.Remove(sectionPair);

            keyPairs.Add(sectionPair, settingValue);
        }

        /// <summary>
        /// Adds or replaces a setting to the table to be saved with a null value.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        public void AddSetting(String sectionName, String settingName)
        {
            AddSetting(sectionName, settingName, null);
        }

        /// <summary>
        /// Remove a setting.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        public void DeleteSetting(String sectionName, String settingName)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;

            if (keyPairs.ContainsKey(sectionPair))
                keyPairs.Remove(sectionPair);
        }

        /// <summary>
        /// Save settings to new file.
        /// </summary>
        /// <param name="newFilePath">New file path.</param>
        public void SaveSettings(String newFilePath)
        {
            ArrayList sections = new ArrayList();
            String tmpValue = "";
            String strToSave = "";

            foreach (SectionPair sectionPair in keyPairs.Keys)
            {
                if (!sections.Contains(sectionPair.Section))
                    sections.Add(sectionPair.Section);
            }

            foreach (String section in sections)
            {
                strToSave += ("[" + section + "]\r\n");

                foreach (SectionPair sectionPair in keyPairs.Keys)
                {
                    if (sectionPair.Section == section)
                    {
                        tmpValue = (String)keyPairs[sectionPair];

                        if (tmpValue != null)
                            tmpValue = "=" + tmpValue;

                        strToSave += (sectionPair.Key + tmpValue + "\r\n");
                    }
                }

                strToSave += "\r\n";
            }

            try
            {
                TextWriter tw = new StreamWriter(newFilePath);
                tw.Write(strToSave);
                tw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save settings back to ini file.
        /// </summary>
        public void SaveSettings()
        {
            SaveSettings(iniFilePath);
        }
    }
}
