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
using System.Runtime.InteropServices;
using System.IO;
namespace screenshots
{
    public partial class Form2 : Form
    {
     //   #region variabile form1
     //   public int chenar_deschis;




     //   int TogMove;
     //   int MValX;
     //   int MValY;

     ////   Rectangle[] cropRect = new Rectangle[30]; // dreptunghiuri utile pentru decuparea pozelor la 
     //   // coordonatele corespunzatoare/
        



     //   //int[] x1 = new int[10];
     //   //int[] y1 = new int[10];     // coordonatele 
     //   //int[] x2 = new int[10];
     //   //int[] y2 = new int[10];

     //   //int[] lungime = new int[10];
     //   //int[] latime = new int[10];

     //   //int lungime_tablou, latime_tablou;
     //   //int deviere_la_stanga = 58;
     //   //int deviere_in_jos = -12;

     //   //float[] m = new float[10];




     //   //Form2 chenar = new Form2();
     //   //Form3 bck = new Form3();
     //   //Image[] poze = new Image[10];
     //   //Image[] imf = new Image[10];
     //   //int nr_poze = 0;
     //   #endregion
 
        int TogMove;
        int MValX;
        int MValY;
        int coordonate_chenar_X, coordonate_chenar_Y;
       public int lungime_Y, latime_X;
        Image sumar1, sumar2, sumar3, sumar4;
        Form4 f4=new Form4();
        int nr_scr;
        public int Step;
        public int exista_3Shape;
        SaveFileDialog salveaza_poze = new SaveFileDialog();

        public string destinatie_fotografii;
        //public string finalizare_destinatie(string destinatie_fotografii)
        //{
        //    string target;
            
        //    if (Path.GetFileName(destinatie_fotografii) == "poze")
        //        target = destinatie_fotografii;
        //    else if (Path.GetFileName(destinatie_fotografii) == "3Shape - fisiere")
        //        target = destinatie_fotografii + @"poze\";
        //    else
        //    {
        //        File.Copy("3Shape - fisiere", @"C:\Users\Adi\Desktop\3Shape - fisiere");
        //    }



        //    target = "bla";
        //    return target;
        //}
        
        private void sterge_folder(int Step)
        {


            string[] dest_poze = Directory.GetFiles(@"Poze" + Step);
            foreach (string poza in dest_poze)
            {
                File.Delete(poza);
            }

        }

        


        #region Windows API

        [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll")]
        public static extern IntPtr RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern IntPtr UnRegisterHotKey(IntPtr hWnd, int id);
        #endregion
        public enum fsModifiers
        {
            Alt = 0x0001,
            Control = 0x0002,
            Shift = 0x0003,
            Window = 0x0004,
        }

        
        private IntPtr thisWindow;

        public Form2()
        {
            InitializeComponent();
            this.TransparencyKey = (BackColor);
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            exista_3Shape = 0;
            using (StreamReader step_reader = new StreamReader(@"Step.ini"))
            {
                Step = Convert.ToInt32(step_reader.ReadToEnd());
            }
            sterge_folder(Step);

            if (Step == 1) Step = 2;
            else
                if (Step == 2) Step = 1;


            f4.Step = Step;
            using (StreamWriter step_writer = new StreamWriter(@"Step.ini"))
            {

                step_writer.Write(Step.ToString());
                step_writer.Close();
            }

            using (StreamReader chenar_X = new StreamReader(@"chenar_X.ini"))
            {
                coordonate_chenar_X = Convert.ToInt32(chenar_X.ReadToEnd());
            }
            using (StreamReader chenar_Y = new StreamReader(@"chenar_Y.ini"))
            {
                coordonate_chenar_Y = Convert.ToInt32(chenar_Y.ReadToEnd());
            }
            this.SetDesktopLocation(coordonate_chenar_X,coordonate_chenar_Y);
            
            using (StreamReader latime_form= new StreamReader(@"latime.ini"))
            {
                latime_X = Convert.ToInt32(latime_form.ReadToEnd());
            }
            using (StreamReader lungime_form = new StreamReader(@"lungime.ini"))
            {
                lungime_Y = Convert.ToInt32(lungime_form.ReadToEnd());
            }
            this.SetClientSizeCore(latime_X, lungime_Y);
            
            nr_scr = 1;
            thisWindow = FindWindow(null, "Form2");

            RegisterHotKey(thisWindow, 1, (uint)fsModifiers.Control, (uint)Keys.F);
            RegisterHotKey(thisWindow, 2, (uint)fsModifiers.Control, (uint)Keys.S);
        }

        
     
        protected override void WndProc(ref Message keyPressed)
        {

            if (keyPressed.Msg == 0x0312)
            {

                IntPtr i = keyPressed.WParam;
                int j = (int)i;
                if(j==1)
                   #region CTRL+F
                if (nr_scr <= 9)
                {
                    int lungime, latime; // lungimea si latimea pozei crop-ate.
                    lungime = Convert.ToInt32(Convert.ToDouble(this.Size.Height) - Convert.ToDouble(this.Size.Height)/4.399F);
                    latime = Convert.ToInt32(Convert.ToDouble(this.Size.Width) - Convert.ToDouble(this.Size.Width) / 44.111F);

                   // MessageBox.Show("this.Size.Height=" + this.Size.Height.ToString());
                   //MessageBox.Show("this.Size.Width=" + this.Size.Width.ToString());
                    Bitmap scr = new Bitmap(latime, lungime);
                    int x, y; // coordonatele punctului de unde se cropuieste poza
                    x = Convert.ToInt32(Convert.ToDouble(this.Location.X) + Convert.ToDouble(this.Size.Width) / 88.2222F);
                    y = Convert.ToInt32(Convert.ToDouble(this.Location.Y) + Convert.ToDouble(this.Size.Height) / 4.7173F);
                    //MessageBox.Show("this.Location.X="+this.Location.X);
                    //MessageBox.Show("this.Location.Y=" + this.Location.Y);
                    Graphics grp = Graphics.FromImage(scr as Image);
                    //MessageBox.Show(x.ToString()+" "+y.ToString()+" "+latime.ToString()+" "+lungime+"")
                    grp.CopyFromScreen(x, y, 0, 0, scr.Size);
                    
                    scr.Save(@"Poze"  + Step + @"\" + nr_scr + @".jpg");
                    nr_scr++;
                    if (nr_scr != 10)
                        this.BackgroundImage = Image.FromFile(@"Fundaluri\3d-image-" + nr_scr + @".png");
                    else
                    {
                        this.Close();
                        locatie_salvare_poze();

                        f4.destinatie_fotografii = destinatie_fotografii;
                        f4.Show();
                        this.Close();
                    }
                }
                #endregion
                if (j == 2) //Ctrl+S

                {
                    if (nr_scr > 5)
                    {
                        this.Close();
                        locatie_salvare_poze();

                        f4.destinatie_fotografii = destinatie_fotografii;
                        f4.Show();
                    }
                    else
                    {
                        
                        
                    }
               }
               


            }
            base.WndProc(ref keyPressed);
        } 

        public void locatie_salvare_poze()
        {
           // chenar_poze.Enabled = false;
            this.Close();
            //MessageBox.Show(salveaza_poze.InitialDirectory);
            using (StreamReader reader = new StreamReader(@"locatie.ini"))
            {
                salveaza_poze.InitialDirectory = reader.ReadToEnd();
            }
            salveaza_poze.Title = "Pictures destination:";
            salveaza_poze.Filter = "Jpeg file|*jpg";
            salveaza_poze.FileName = "Save!!!";
            if (salveaza_poze.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                Application.Exit();
            if (salveaza_poze.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {   

                destinatie_fotografii = salveaza_poze.FileName;
                // MessageBox.Show(salveaza_poze.FileName);
                string caractere = Path.GetFileName(destinatie_fotografii);

                destinatie_fotografii = destinatie_fotografii.Substring(0, destinatie_fotografii.Length - caractere.Length);

                //destinatie_fotografii = finalizare_destinatie(destinatie_fotografii);



                using (StreamWriter writer = new StreamWriter(@"locatie.ini"))
                {
                    writer.Write(destinatie_fotografii);
                    writer.Close();
                }
            }
        }




        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            
            
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
           // program.chenar_deschis = 0;
            using (StreamWriter chenar_X = new StreamWriter(@"chenar_X.ini"))
            {
                
                chenar_X.Write(coordonate_chenar_X.ToString());
                chenar_X.Close();
            }
            using (StreamWriter chenar_Y = new StreamWriter(@"chenar_Y.ini"))
            {

                chenar_Y.Write(coordonate_chenar_Y.ToString());
                chenar_Y.Close();
            }
            using (StreamWriter latime_form = new StreamWriter(@"latime.ini"))
            {
                latime_form.Write(latime_X.ToString());
                latime_form.Close();
            }
            using (StreamWriter lungime_form = new StreamWriter(@"lungime.ini"))
            {
                lungime_form.Write(lungime_Y.ToString());
                lungime_form.Close();
            }


        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (TogMove == 1)
            {
                coordonate_chenar_X = MousePosition.X - MValX;
                coordonate_chenar_Y = MousePosition.Y - MValY;
                this.SetDesktopLocation(coordonate_chenar_X, coordonate_chenar_Y);
                
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    public void minimizare_chenar()
    {
            latime_X = this.Size.Width - 31;
            lungime_Y = this.Size.Height - 26;
            this.SetClientSizeCore(latime_X, lungime_Y);
    }
    public void maximizare_chenar()
    {
        latime_X = this.Size.Width + 31;
        lungime_Y = this.Size.Height + 26;
        this.SetClientSizeCore(latime_X, lungime_Y);
    }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            latime_X = this.Size.Width - 31;
            lungime_Y = this.Size.Height - 26;
            this.SetClientSizeCore(latime_X, lungime_Y);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            latime_X = this.Size.Width + 31;
            lungime_Y = this.Size.Height + 26;
            this.SetClientSizeCore(latime_X, lungime_Y);
        }





    }
}
    