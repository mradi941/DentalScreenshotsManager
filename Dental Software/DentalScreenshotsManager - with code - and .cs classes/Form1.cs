using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;


///<summary>
///int step;
///
//using(StreamReader reader = new StreamReader(@"locatie.ini"))
//            {
//                salveaza_poze.InitialDirectory = reader.ReadToEnd();
//}               step=(int)reader.ReadToEnd();
///<summary>
///s
///s
///s
///s
///
namespace screenshots
{
    public partial class Form1 : Form
    {
        public int chenar_deschis;
        Image sumar1, sumar2, sumar3, sumar4;
        int luna_curenta;
        

       public int Step;
        int TogMove;
        int MValX;
        int MValY;
        SaveFileDialog salveaza_poze = new SaveFileDialog();
        
        string destinatie_fotografii;
        Rectangle[] cropRect = new Rectangle[30]; // dreptunghiuri utile pentru decuparea pozelor la 
                                                       // coordonatele corespunzatoare/
        int nr_scr;
        
        
        
        int[] x1 = new int[10];
        int[] y1 = new int[10];     // coordonatele 
        int[] x2 = new int[10];
        int[] y2 = new int[10];
        
        int[] lungime = new int[10];
        int[] latime = new int[10];

        int lungime_tablou, latime_tablou;
        int deviere_la_stanga = 58;
        int deviere_in_jos = -12;

        float[]m = new float[10];




        Form2 chenar = new Form2();
        Form3 bck = new Form3();
        Image[] poze = new Image[10];
        Image[] imf = new Image[10];

        int nr_poze = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            




        }
        private void button1_Click(object sender, EventArgs e)
        {
            
           //ck.Close();
            //chenar.Show(); 
            
        }
        
        private void sterge_folder(int Step)
        {


            string[] dest_poze = Directory.GetFiles(@"Poze"+Step);
            foreach (string poza in dest_poze)
            {
                File.Delete(poza);
            }
            
           

        }
        private void right_Click(object sender, EventArgs e)
        {
            
        }
        private void up_Click(object sender, EventArgs e)
        {
            
               
             
        }
        private void button6_Click(object sender, EventArgs e)
        {
           // chenar.Close();
            bck.Show();
        }

        private Image FrontLR(Image poza,int i)
        {
            int lungime_poza=lungime[i];
            int latime_poza=latime[i];
            int latime_front = latime[1];
          
            int x, y;//x si y sunt coordonatele in lungime si latime
            x=Convert.ToInt32(((float)latime_tablou-(float)latime_front)/2F);
            y=Convert.ToInt32(((float)lungime_tablou-(float)lungime_poza)/2F)+lungime_poza;
            x = x - deviere_la_stanga;
            y = y + deviere_in_jos;

            int deviereR, deviereL;
            deviereR = Convert.ToInt32((float)latime_front / 5F);
            deviereL = Convert.ToInt32((4F * (float)latime_front) / 5F);

            var font1 = new Font("Arial", 152, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(poza);
            
               // graphics.DrawString("R", font1, Brushes.Red, new Point(30,30));
                graphics.DrawString("R", font1, Brushes.Red, new Point(x + deviereR, y));
                graphics.DrawString("L", font1, Brushes.Red, new Point(x+deviereL,y));
            

            return poza;
        }
        private Image LateralR(Image poza,int i)
        {
            int lungime_poza = lungime[i];
            int latime_poza = latime[i];
            int latime_front = latime[4];

            int x, y;//x si y sunt coordonatele in lungime si latime
            x = Convert.ToInt32(((float)latime_tablou - (float)latime_poza) / 2F);
            y = Convert.ToInt32(((float)lungime_tablou - (float)lungime_poza) / 2F) + lungime_poza;
            x = x - deviere_la_stanga;
            y = y + deviere_in_jos;

            int deviereR, deviereL;
           // deviereR = Convert.ToInt32((float)latime_poza / 5F);
            deviereR = Convert.ToInt32((4F * (float)latime_poza) / 5F);

            var font1 = new Font("Arial", 152, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(poza);

            //graphics.DrawString("R", font1, Brushes.Red, new Point(x + deviereR, y));
            graphics.DrawString("R", font1, Brushes.Red, new Point(x + deviereR, y));


            return poza;
        }
        private Image LateralL(Image poza,int i)
        {
            int lungime_poza = lungime[i];
            int latime_poza = latime[i];
            int latime_front = latime[4];

            int x, y;//x si y sunt coordonatele in lungime si latime
            x = Convert.ToInt32(((float)latime_tablou - (float)latime_poza) / 2F);
            y = Convert.ToInt32(((float)lungime_tablou - (float)lungime_poza) / 2F) + lungime_poza;
            x = x - deviere_la_stanga;
            y = y + deviere_in_jos;

            int deviereR, deviereL;
            deviereL = Convert.ToInt32((float)latime_poza / 5F);
            //deviereL = Convert.ToInt32((4F * (float)latime_poza) / 5F);

            var font1 = new Font("Arial", 152, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(poza);

            graphics.DrawString("L", font1, Brushes.Red, new Point(x + deviereL, y));
            //graphics.DrawString("L", font1, Brushes.Red, new Point(x + deviereL, y));


            return poza;
        }
        private Image OclusionLR(Image poza,int i)
        {
            int lungime_poza = lungime[i];
            int latime_poza = latime[i];
            int latime_front = latime[1];

            int x, y;//x si y sunt coordonatele in lungime si latime
            x = Convert.ToInt32(((float)latime_tablou - (float)latime_front) / 2F);
            y = Convert.ToInt32(((float)lungime_tablou - (float)lungime_poza) / 2F) + lungime_poza;
            x = x - deviere_la_stanga;
            y = y + deviere_in_jos;
            x = x - 130; // 259 este fix latimea tabelului cromatic


            int deviereR, deviereL;
            deviereR = Convert.ToInt32((float)latime_front / 5F);
            deviereL = Convert.ToInt32((4F * (float)latime_front) / 5F);

            var font1 = new Font("Arial", 152, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(poza);

            graphics.DrawString("R", font1, Brushes.Red, new Point(x + deviereR, y));
            graphics.DrawString("L", font1, Brushes.Red, new Point(x + deviereL, y));


            return poza;
        }

        public void crop(Image screen,int nr_poza)
        {
           
            int sus=10000,jos=-1,stanga=10000,dreapta=-1;
            Bitmap bck = new Bitmap(@"background.jpg");
            Bitmap scr = new Bitmap(screen);
            //calculam cel mai de sus punct
            for(int i=0;i<scr.Width;i++)
            {
                for (int j = 0; j < scr.Height; j++)
            
                {
                    if (scr.GetPixel(i, j) != bck.GetPixel(i, j))
                    {
                        if (j < sus) sus = j;
                    }
            
                }
            
            }

            //  
            y1[nr_poza] = sus;
            //calculam cel mai de jos punct
            for (int i = 0; i < scr.Width; i++)
                for (int j=0; j <scr.Height ; j++)
                    if (scr.GetPixel(i, j) != bck.GetPixel(i, j))
                        if (j > jos) jos = j;
            y2[nr_poza] = jos;
            //calculam cel mai din stanga punct
            for (int i = 0; i < scr.Width; i++)
                for (int j = 0; j < scr.Height; j++)
                    if (scr.GetPixel(i, j) != bck.GetPixel(i, j))
                        if (i < stanga) stanga = i;
            x1[nr_poza] = stanga;
            //calculam cel mai din dreapta punct
            for (int i = 0; i < scr.Width; i++)
                for (int j = 0; j < scr.Height; j++)
                    if (scr.GetPixel(i, j) != bck.GetPixel(i, j))
                        if (i > dreapta) dreapta = i;
            x2[nr_poza] = dreapta;

            
           
            //calculam lungimea si latimea:

            lungime[nr_poza] = y2[nr_poza] - y1[nr_poza];
            latime[nr_poza] = x2[nr_poza] - x1[nr_poza];
            

        }  //aici este functia crop ,in care se salveaza
                                                       //toate variabilele utile pentru fiecare poza
                                                       //lungimea,latime etc.
        private void calculeaza_m(int nr_poze)
        {
            if (nr_poze == 6)
            {
                m[1] = (770F * (float)latime[2]) / ((float)lungime[2] * (float)latime[1]);
                m[2] = 770F / (float)lungime[2];
                m[3] = 770F / (float)lungime[3];
                m[4] = 770F / (float)lungime[4];
                m[5] = 1060F / (float)lungime[5];
                m[6] = 1060F / (float)lungime[6];
            }
            if (nr_poze == 7)
            {
                m[1] = (770F * (float)latime[2]) / ((float)lungime[2] * (float)latime[1]);
                m[2] = 770F / (float)lungime[2];
                m[3] = 770F / (float)lungime[3];
                m[4] = 770F / (float)lungime[4];
                m[5] = 770F / (float)lungime[5];
                m[6] = 1000F / (float)lungime[6];
                m[7] = 1000F / (float)lungime[7];
            }
            if (nr_poze == 8)
            {
                m[1] = (770F * (float)latime[2]) / ((float)lungime[2] * (float)latime[1]);
                m[2] = 770F / (float)lungime[2];
                m[3] = 770F / (float)lungime[3];
                m[4] = 770F / (float)lungime[4];
                m[5] = 1000F / (float)lungime[5];
                m[6] = 1000F / (float)lungime[6];
                m[7] = 1000F / (float)lungime[7];
                m[8] = 1000F / (float)lungime[8];

            }
            if (nr_poze == 9)
            {
                m[1] = (770F * (float)latime[2]) / ((float)lungime[2] * (float)latime[1]);
                m[2] = 770F / (float)lungime[2];
                m[3] = 770F / (float)lungime[3];
                m[4] = 770F / (float)lungime[4];
                m[5] = 770F / (float)lungime[5];
                m[6] = 1000F / (float)lungime[6];
                m[7] = 1000F / (float)lungime[7];
                m[8] = 1000F / (float)lungime[8];
                m[9] = 1000F / (float)lungime[9];
            }
        }
        private Bitmap ResizeNow(Image imagine,float m)
        {
            int lungime = imagine.Height;
            float latime = imagine.Width;
           
            int l, L; L = Convert.ToInt32((float)latime * m); l = Convert.ToInt32((float)lungime * m);

            Rectangle dest_rect = new Rectangle(0, 0, L, l);
            Bitmap destImage = new Bitmap(L, l);

            destImage.SetResolution(imagine.HorizontalResolution, imagine.VerticalResolution);
            using (var g = Graphics.FromImage(destImage))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapmode = new ImageAttributes())
                {
                    wrapmode.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(imagine, dest_rect, 0, 0, latime, lungime, GraphicsUnit.Pixel, wrapmode);
                }
            }
            return destImage;
        }
        private Bitmap adaoga_tabel_cromatic(Image poza)
        {
            int latime = poza.Width;
            int lungime = poza.Height;
            
            Image tabel = Image.FromFile("tabel.jpg");
            int latime_tabel = tabel.Width;
            Bitmap rezultat = new Bitmap(latime + latime_tabel, lungime);
            using (var g = Graphics.FromImage(rezultat))
            {
                g.DrawImage(poza,0,0);
                g.DrawImage(tabel, latime, 0);
            }
            return rezultat;

        }
        private Bitmap poze_in_tablou(Image poza,int latime_tablou,int lungime_tablou)
        {
            Bitmap tablou = new Bitmap(latime_tablou, lungime_tablou);
            for (int i = 0; i < tablou.Width; i++)
                for (int j = 0; j < tablou.Height; j++)
                {
                    tablou.SetPixel(i, j, Color.White);
                }
            
            int x = Convert.ToInt32(((float)latime_tablou - (float)poza.Width) / 2F);
            int y = Convert.ToInt32(((float)lungime_tablou - (float)poza.Height) / 2F);
            
            using (var g = Graphics.FromImage(tablou))
            {
                g.DrawImage(poza,x,y);
            }

            return tablou;
        }
        public void locatie_salvare_poze()
        {
            //chenar_poze.Enabled = false;
            chenar.Close();
            
            using (StreamReader reader = new StreamReader(@"locatie.ini"))
            {
                salveaza_poze.InitialDirectory = reader.ReadToEnd();
            }
            salveaza_poze.Title = "Pictures destination:";
            salveaza_poze.Filter = "Jpeg file|*jpg";
            salveaza_poze.FileName = "Save!!!";
            if (salveaza_poze.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                destinatie_fotografii = salveaza_poze.FileName;
                
                string caractere = Path.GetFileName(destinatie_fotografii);

                destinatie_fotografii = destinatie_fotografii.Substring(0, destinatie_fotografii.Length - caractere.Length);

                using (StreamWriter writer = new StreamWriter(@"locatie.ini"))
                {
                    writer.Write(destinatie_fotografii);
                    writer.Close();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //int nu_inchide = 0;

            this.WindowState = FormWindowState.Minimized;
            
            chenar.Show();


            DateTime astazi = DateTime.Now;
            luna_curenta = astazi.Month;
            int x;
            using (StreamReader luna_curenta_reader = new StreamReader(@"luna_curenta.ini"))
            {
                
                x = Convert.ToInt32(luna_curenta_reader.ReadToEnd());
                //MessageBox.Show(x.ToString());
                
            }
            if (luna_curenta != x)
            {

                MessageBox.Show("Tocmai ce a mai trecut o luna. Introdu URL-ul noului folder al lunii curente din export-orto.");

                this.WindowState = FormWindowState.Normal;
                using (StreamWriter luna_curenta_writer = new StreamWriter(@"luna_curenta.ini"))
                {
                    
                    luna_curenta_writer.Write(luna_curenta.ToString());
                    luna_curenta_writer.Close();
                }
            }





            using (StreamReader export_orto_reader = new StreamReader(@"export_orto.ini"))
            {
                textBox1.Text = Convert.ToString(export_orto_reader.ReadToEnd());
            }
            using (StreamReader evidenta_3Shape = new StreamReader(@"Evidenta 3Shape.ini"))
            {
                textBox2.Text = evidenta_3Shape.ReadToEnd();
            }
            
            
            chenar_deschis = 0;
               nr_scr = 1;
           
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //ck.Close();
            chenar.Show();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            locatie_salvare_poze();
        }

        //int TogMove;
        //int MValX;
        //int MValY;

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1) // asta se intampla in timp real!!!
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
       
            }
        }
       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            chenar.minimizare_chenar();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            chenar.maximizare_chenar();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //File.Copy("3Shape - fisiere", @"C:\Users\Adi\Desktop\3Shape - fisiere");
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
               using(StreamWriter export_orto_writer=new StreamWriter(@"export_orto.ini"))
            {
                export_orto_writer.Write(textBox1.Text);
                export_orto_writer.Close();
                MessageBox.Show("URL changed!");
            }

               using (StreamWriter evidenta_3Shape = new StreamWriter(@"Evidenta 3Shape.ini"))
               {
                   evidenta_3Shape.Write(textBox2.Text);
                   evidenta_3Shape.Close();
                       MessageBox.Show("URL changed!");
               }
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }   
    }





}



