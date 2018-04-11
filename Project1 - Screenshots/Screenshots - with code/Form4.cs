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
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace screenshots
{
    public partial class Form4 : Form
    {


        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;

        string ultimele_litere;
        Form5 f5 = new Form5();
        int swit;
        public string destinatie_fotografii; string destination_X, destinatie_fotografii_folder;
        public int ziua, luna, anul;
        public string nume_pacient_folder;
        public string ziua_string, luna_string, anul_string;
        public string export_orto_target;
        int luna_curenta;
        DateTime data ;
        Image a;
           
        public Form4()
        {
            InitializeComponent();
        }
        int TogMove;
        int MValX;
        int MValY;
        public int Step;
        Image sumar1, sumar2, sumar3, sumar4;
        Rectangle[] cropRect = new Rectangle[30]; // dreptunghiuri utile pentru decuparea pozelor la 
        // coordonatele corespunzatoare/


        int[] x1 = new int[10];
        int[] y1 = new int[10];     // coordonatele 
        int[] x2 = new int[10];
        int[] y2 = new int[10];

        int[] lungime = new int[10];
        int[] latime = new int[10];

        int lungime_tablou, latime_tablou;
        int deviere_la_stanga = 58;
        int deviere_in_jos = -12;

        float[] m = new float[10];
        Image[] poze = new Image[10];
        Image[] imf = new Image[10];
        int nr_poze = 0;


        private int verifica_caz_multiplu()
        {
            int nr_pacienti = 0;
           // MessageBox.Show(export_orto_target);
            string[] pacienti = Directory.GetDirectories(export_orto_target);
            
            foreach (string pac in pacienti)
            {
                    
                string nume_pacient_scurt;
                int lungime_pacient = nume_pacient.Length;

                if (Path.GetFileName(pac).Length >= lungime_pacient)
                    nume_pacient_scurt = Path.GetFileName(pac).Substring(0, lungime_pacient);
                else nume_pacient_scurt = Path.GetFileName(pac);
                if (nume_pacient_scurt == nume_pacient)
                {
                    string verificare,numex;
                    verificare = " ";
                    numex = (Path.GetFileName(pac));
                    if(numex.Length>nume_pacient_scurt.Length)
                    verificare = numex.Substring(nume_pacient_scurt.Length,1);
                    if(verificare==" ")
                    nr_pacienti++;
                }
            }


            int exista_totusi = 0;
            foreach (string pac in pacienti)
            {
                if (Path.GetFileName(pac) == nume_pacient)
                    exista_totusi++;
            }



            if (nr_pacienti == 1) return 1;
            if (nr_pacienti > 1)
                return 2;
            return 0;
        }
        private void MoveSTL()
        {
            string destinatie_STL;
          
            destinatie_STL = destinatie_fotografii.Substring(0, destinatie_fotografii.Length - 5);
            destinatie_STL = destinatie_STL + @"stl";
          
            string[] files_destinatie_STL = Directory.GetFiles(destinatie_STL);
            int nr_fisiere = 0;
            foreach (string file in files_destinatie_STL)
            {
                nr_fisiere++;
            }

            

            if (nr_fisiere != 0)
                MessageBox.Show("STL-urile sunt deja mutate in folderul pacientului: \"" + nume_pacient_folder+"\".");
            else
            if (!Directory.Exists(export_orto_target))

                MessageBox.Show("Programul nu gaseste folderul 'export-orto'.Copiaza STL-urile manual!! Cel mai probabil este o problema de retea.");
            else
            {

                if (verifica_caz_multiplu() == 2)
                    MessageBox.Show("Exista mai multi pacienti cu acelasi nume.Muta STL-urile manual.");
                else if (verifica_caz_multiplu() == 1)
                {
                    string locatie_export_orto;
                    locatie_export_orto = export_orto_target + @"\" + nume_pacient;
                    string[] files_export_orto = Directory.GetFiles(locatie_export_orto);
                    int nr_STL = 0;
                    foreach (string file in files_export_orto)
                    {
                        if (Path.GetExtension(file) == ".stl")
                            nr_STL++;
                    }

                    if (nr_STL >= 1)
                        CopyFolder(locatie_export_orto, destinatie_STL);
                    else
                        MessageBox.Show("Nu exista stl-uri in folderul export-orto.Te rog sa le creezi si sa le muti manual si in folderul pacientului: \""+nume_pacient+"\"");
                }
                else
                    MessageBox.Show("Nu exista folderul pacientului \"" + nume_pacient + "\" in Export orto.Te rog sa creezi STL-urile si sa le muti manual.");
            }
        
        }
        private void string_data()
        {
            anul_string = anul.ToString();
            if (luna < 10)
                luna_string = "0" + luna.ToString();
            else luna_string = luna.ToString();
            if (ziua < 10)
                ziua_string = "0" + ziua.ToString();
            else ziua_string = ziua.ToString();
        }
        private Image FrontLR(Image poza, int i)
        {
            int lungime_poza = lungime[i];
            int latime_poza = latime[i];
            int latime_front = latime[1];

            int x, y;//x si y sunt coordonatele in lungime si latime
            x = Convert.ToInt32(((float)latime_tablou - (float)latime_front) / 2F);
            y = Convert.ToInt32(((float)lungime_tablou - (float)lungime_poza) / 2F) + lungime_poza;
            x = x - deviere_la_stanga;
            y = y + deviere_in_jos;

            int deviereR, deviereL;
            deviereR = Convert.ToInt32((float)latime_front / 5F);
            deviereL = Convert.ToInt32((4F * (float)latime_front) / 5F);

            var font1 = new Font("Arial", 152, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(poza);

            // graphics.DrawString("R", font1, Brushes.Red, new Point(30,30));
            graphics.DrawString("R", font1, Brushes.Red, new Point(x + deviereR, y));
            graphics.DrawString("L", font1, Brushes.Red, new Point(x + deviereL, y));


            return poza;
        }
        private Image LateralR(Image poza, int i)
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
        private Image LateralL(Image poza, int i)
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
        private Image OclusionLR(Image poza, int i)
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
        public void crop(Image screen, int nr_poza)
        {

            int sus = 10000, jos = -1, stanga = 10000, dreapta = -1;
            Bitmap bck = new Bitmap(@"background.jpg");
            Bitmap scr = new Bitmap(screen);
            //calculam cel mai de sus punct
            for (int i = 0; i < scr.Width; i++)
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
                for (int j = 0; j < scr.Height; j++)
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


            // MessageBox.Show(y1[nr_poza].ToString()+" "+y2[nr_poza].ToString()+" "+x1[nr_poza].ToString()+" "+x2[nr_poza].ToString()+" ");
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
        private Bitmap ResizeNow(Image imagine, float m)
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
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapmode = new ImageAttributes())
                {
                    g.DrawImage(poza, 0, 0);
                    g.DrawImage(tabel, latime, 0);
                }
            }
            return rezultat;

        }
        private Bitmap poze_in_tablou(Image poza, int latime_tablou, int lungime_tablou)
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
                                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapmode = new ImageAttributes())
                {
                    g.DrawImage(poza, x, y);
                }
            }

            return tablou;
        }


        public void CopyFolder(string source, string destination)
        {
            DirectoryInfo sourceinfo = new DirectoryInfo(source);
            DirectoryInfo destinationinfo = new DirectoryInfo(destination);
            CopyFilesRecursively(sourceinfo, destinationinfo);
        }
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
        private int exista_3Shape()
        {
           
            
            
            string[] dirs = Directory.GetDirectories(destination_X);
            foreach (string dir in dirs)
            {
                if (Path.GetFileName(dir) == "3Shape - fisiere" | Path.GetFileName(dir) == "poze")
                {
                    if (Path.GetFileName(dir) == "3Shape - fisiere")
                        return 1;
                    else
                    {
                         //   3Shape - fisiere\poze\
                        ultimele_litere=dir.Substring(dir.Length-21,21);
                       // MessageBox.Show("sugi pula  "+ultimele_litere);
                        return 2;
                    }

                
                }
            }
            return 0;
        }



        public void creeaza_poze_pentru_pacient()
        {
            if (nr_poze == 6)
            {
                sumar1 = (IncadrareImagini2(ResizeNow2(imf[1]), ResizeNow2(imf[2])));

           //     MessageBox.Show("plm,,");
                sumar1 = ScrieTot(sumar1, 1); sumar1.Save(destinatie_fotografii + "sumar 1.jpg");
                 
                sumar2 = (IncadrareImagini2(ResizeNow2(imf[3]), ResizeNow2(imf[4])));
                sumar2 = ScrieTot(sumar2, 2); sumar2.Save(destinatie_fotografii + "sumar 2.jpg");

                sumar3 = (IncadrareImagini2(ResizeNow2(imf[5]), ResizeNow2(imf[6])));
                sumar3 = ScrieTot(sumar3, 3); sumar3.Save(destinatie_fotografii + "sumar 3.jpg");
            }
            if (nr_poze == 7)
            {
                
                sumar1 = (IncadrareImagini3(ResizeNow3(imf[1]), ResizeNow3(imf[2]), ResizeNow3(imf[3])));
                sumar1 = ScrieTot(sumar1, 1); sumar1.Save(destinatie_fotografii + "sumar 1.jpg");

                sumar2 = (IncadrareImagini2(ResizeNow2(imf[4]), ResizeNow2(imf[5])));
                sumar2 = ScrieTot(sumar2, 2); sumar2.Save(destinatie_fotografii + "sumar 2.jpg");

                sumar3 = (IncadrareImagini2(ResizeNow2(imf[6]), ResizeNow2(imf[7])));
                sumar3 = ScrieTot(sumar3, 3); sumar3.Save(destinatie_fotografii + "sumar 3.jpg");

            }
            if (nr_poze == 8)
            {
                sumar1 = (IncadrareImagini2(ResizeNow2(imf[1]), ResizeNow2(imf[2])));
                sumar1 = ScrieTot(sumar1, 1); sumar1.Save(destinatie_fotografii + "sumar 1.jpg");

                sumar2 = (IncadrareImagini2(ResizeNow2(imf[3]), ResizeNow2(imf[4])));
                sumar2 = ScrieTot(sumar2, 2); sumar2.Save(destinatie_fotografii + "sumar 2.jpg");

                sumar3 = (IncadrareImagini2(ResizeNow2(imf[5]), ResizeNow2(imf[6])));
                sumar3 = ScrieTot(sumar3, 3); sumar3.Save(destinatie_fotografii + "sumar 3.jpg");
                if (swit == 0)
                {
                    sumar4 = (IncadrareImagini2(ResizeNow2(imf[7]), ResizeNow2(imf[8])));
                    sumar4 = ScrieTot(sumar4, 4); sumar4.Save(destinatie_fotografii + "sumar 4.jpg");
                }
                else
                {
                    sumar4 = (IncadrareImagini2(ResizeNow2(imf[8]), ResizeNow2(imf[7])));
                    sumar4 = ScrieTot(sumar4, 4); sumar4.Save(destinatie_fotografii + "sumar 4.jpg");
                }
            }
            if (nr_poze == 9)
            {
                sumar1 = (IncadrareImagini3(ResizeNow3(imf[1]), ResizeNow3(imf[2]), ResizeNow3(imf[3])));
                sumar1 = ScrieTot(sumar1, 1); sumar1.Save(destinatie_fotografii + "sumar 1.jpg");

                sumar2 = (IncadrareImagini2(ResizeNow2(imf[4]), ResizeNow2(imf[5])));
                sumar2 = ScrieTot(sumar2, 2); sumar2.Save(destinatie_fotografii + "sumar 2.jpg");

                sumar3 = (IncadrareImagini2(ResizeNow2(imf[6]), ResizeNow2(imf[7])));
                sumar3 = ScrieTot(sumar3, 3); sumar3.Save(destinatie_fotografii + "sumar 3.jpg");
                if (swit == 0)
                {
                    sumar4 = (IncadrareImagini2(ResizeNow2(imf[8]), ResizeNow2(imf[9])));
                    sumar4 = ScrieTot(sumar4, 4); sumar4.Save(destinatie_fotografii + "sumar 4.jpg");
                }
                else
                {
                    sumar4 = (IncadrareImagini2(ResizeNow2(imf[9]), ResizeNow2(imf[8])));
                    sumar4 = ScrieTot(sumar4, 4); sumar4.Save(destinatie_fotografii + "sumar 4.jpg");
                }
            }

        }
        private Image ScrieTot(Image imagine, int pagina)
        {

            var font1 = new Font("Arial", 100, FontStyle.Bold, GraphicsUnit.Pixel);
            var font2 = new Font("Arial", 69, FontStyle.Bold, GraphicsUnit.Pixel);
            var font3 = new Font("Arial", 94, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(imagine);
            //graphics.DrawString("F.M. MEDIDENT DENTAL X-RAY INSTITUTE", font3, Brushes.ForestGreen, new Point(715, 2));
            //graphics.DrawString("3D IMAGES", font1, Brushes.MidnightBlue, new Point(1440, 118));
            graphics.DrawString("Nume: " + nume_pacient_cu_initiala, font2, Brushes.MidnightBlue, new Point(27, 320));
            graphics.DrawString("Data: " + data_curenta, font2, Brushes.MidnightBlue, new Point(2926, 320));
            graphics.DrawString("Data nasterii: " + data_nasterii, font2, Brushes.MidnightBlue, new Point(2667, 401));
            graphics.DrawString("Pag. " + pagina.ToString(), font2, Brushes.MidnightBlue, new Point(3250, 2380));

            return imagine;
        }
        private Bitmap ResizeNow2(Image imagine)
        {
            int lungime = imagine.Height;
            int latime = imagine.Width;
            float m = 1690F / (float)latime;
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
        private Bitmap ResizeNow3(Image imagine)
        {
            float lungime = imagine.Height;
            float latime = imagine.Width;
            float m = 952F / (float)lungime;
            int l, L; L = Convert.ToInt32((float)latime * m); l = Convert.ToInt32((float)lungime * m);
            //if (L > 1690)
            //{
            //    m = 1690 / L;
            //    L = Convert.ToInt32(L * m);
            //    l = Convert.ToInt32(l * m);
            //}
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
        private Bitmap IncadrareImagini2(Image imagine1, Image imagine2)
        {
            int latime_img, lungime_img;
            latime_img = imagine1.Width;
            lungime_img = imagine1.Height;
            int sss;
            sss = Convert.ToInt32((2480F - (float)lungime_img) / 2F);
            Bitmap destImage = new Bitmap(@"white.jpg");
            destImage.SetResolution(imagine1.HorizontalResolution, imagine1.VerticalResolution);
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
                    // g.DrawImage(imagine1, chenar1, 50, (2480 - lungime_img) / 2, latime_img, lungime_img , GraphicsUnit.Pixel, wrapmode);
                    //g.DrawImage(imagine2, chenar2, 80 + latime_img, (2480 - lungime_img) / 2, latime_img, lungime_img, GraphicsUnit.Pixel, wrapmode);

                    g.DrawImage(imagine1, 50, sss + 100);
                    g.DrawImage(imagine2, 80 + latime_img, sss + 100);
                }
            }

            return destImage;
        }
        private Bitmap IncadrareImagini3(Image imagine1, Image imagine2, Image imagine3)
        {
            int latime_img, lungime_img;
            latime_img = imagine1.Width;
            lungime_img = imagine1.Height;

            Bitmap destImage = new Bitmap(@"white.jpg");
            destImage.SetResolution(imagine1.HorizontalResolution, imagine1.VerticalResolution);
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
                    // g.DrawImage(imagine1, chenar1, 50, (2480 - lungime_img) / 2, latime_img, lungime_img , GraphicsUnit.Pixel, wrapmode);
                    //g.DrawImage(imagine2, chenar2, 80 + latime_img, (2480 - lungime_img) / 2, latime_img, lungime_img, GraphicsUnit.Pixel, wrapmode);

                    g.DrawImage(imagine1, Convert.ToInt32(((3508F - 2F * (float)latime_img) / 3F) - ((((3508F - 2F * (float)latime_img) / 3F)) / 3F)), 496F);
                    g.DrawImage(imagine2, Convert.ToInt32((2F * ((3508F - 2F * (float)latime_img) / 3F) + (float)latime_img) + ((((3508F - 2F * (float)latime_img) / 3F)) / 3F)), 496F);
                    g.DrawImage(imagine3, Convert.ToInt32((3508F - (float)latime_img) / 2F), 526F + (float)lungime_img);
                }
            }

            return destImage;
        }
        public void rezultat_final()
        {
            //this.WindowState = FormWindowState.Minimized;

            nr_poze = 0;
                       string[] dest_poze = Directory.GetFiles(@"Poze" + Step);
                      foreach (string poza in dest_poze)
            {
                nr_poze++;
                poze[nr_poze] = Image.FromFile(poza);

            }
           
            // aici salvam global coordonatele de crop pentru fiecare poza+ rapoartele lat/lung
            //a fiecarei poza "i"

            for (int i = 1; i <= nr_poze; i++)
            {
                crop(poze[i], i);
            }



            //mai departe vom realiza cropul la punctele corespunzatoare si corecte
            for (int i = 1; i <= nr_poze; i++)
            {

                cropRect[i] = new Rectangle(x1[i], y1[i], latime[i], lungime[i]);
                Bitmap src = poze[i] as Bitmap;
                Bitmap target = new Bitmap(latime[i], lungime[i]);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, new Rectangle(0, 0, latime[i], lungime[i]), cropRect[i], GraphicsUnit.Pixel);

                }
                imf[i] = target;

            }
            //apoi vom redimensiona la marimea potrivita cu functia resizenow fiecare poza in parte si
            //o vom mari cu m[i] pe fiecare ca sa fie la "lungimea" care trrebuie

            calculeaza_m(nr_poze);

            for (int i = 1; i <= nr_poze; i++)
            {
                imf[i] = ResizeNow(imf[i], m[i]);
            }



            if (nr_poze == 8)
            {
                imf[7] = adaoga_tabel_cromatic(imf[7]);
                imf[8] = adaoga_tabel_cromatic(imf[8]);
            }

            if (nr_poze == 9)
            {
                imf[8] = adaoga_tabel_cromatic(imf[8]);
                imf[9] = adaoga_tabel_cromatic(imf[9]);
            }

            for (int i = 1; i <= nr_poze; i++)
            {
                latime[i] = imf[i].Width;
                lungime[i] = imf[i].Height;
            }


            //////////////////////////////////////    -aici cream tabloul alb
            lungime_tablou = -1;
            latime_tablou = -1;
            for (int i = 1; i <= nr_poze; i++)
            {
                if (lungime[i] > lungime_tablou) lungime_tablou = lungime[i];
                if (latime[i] > latime_tablou) latime_tablou = latime[i];
            }
            lungime_tablou += 300;
            latime_tablou += 40;

            /////////////////////////////////////



            for (int i = 1; i <= nr_poze; i++)
            {
                imf[i] = poze_in_tablou(imf[i], latime_tablou, lungime_tablou);
            }




            if (nr_poze == 6)
            {
                imf[1] = FrontLR(imf[1], 1); imf[2] = FrontLR(imf[2], 2);
                imf[5] = FrontLR(imf[5], 5); imf[6] = FrontLR(imf[6], 6);
                imf[3] = LateralR(imf[3], 3);
                imf[4] = LateralL(imf[4], 4);
            }
            if (nr_poze == 7)
            {
                imf[1] = FrontLR(imf[1], 1); imf[2] = FrontLR(imf[2], 2); imf[3] = FrontLR(imf[3], 3);
                imf[6] = FrontLR(imf[6], 6); imf[7] = FrontLR(imf[7], 7);
                imf[4] = LateralR(imf[4], 4);
                imf[5] = LateralL(imf[5], 5);
            }
            if (nr_poze == 8)
            {
                imf[1] = FrontLR(imf[1], 1); imf[2] = FrontLR(imf[2], 2);
                imf[5] = FrontLR(imf[5], 5); imf[6] = FrontLR(imf[6], 6);
                imf[3] = LateralR(imf[3], 3);
                imf[4] = LateralL(imf[4], 4);
                imf[7] = OclusionLR(imf[7], 7);
                imf[8] = OclusionLR(imf[8], 8);
            }
            if (nr_poze == 9)
            {
                imf[1] = FrontLR(imf[1], 1); imf[2] = FrontLR(imf[2], 2); imf[3] = FrontLR(imf[3], 3);
                imf[6] = FrontLR(imf[6], 6); imf[7] = FrontLR(imf[7], 7);
                imf[4] = LateralR(imf[4], 4);
                imf[5] = LateralL(imf[5], 5);
                imf[8] = OclusionLR(imf[8], 8);
                imf[9] = OclusionLR(imf[9], 9);
            }

            if (destinatie_fotografii != "0")
            {
                
                for (int i = 1; i <= nr_poze; i++)
                {
                 
                    imf[i].Save(destinatie_fotografii + @"3d image " + i + @".jpg");
                }
             
            }
           // destinatie_fotografii = "0";


        }


        
        

        #region  form2

        #endregion


        public string nume_pacient, data_nasterii, data_curenta;
        public string initiala_tata,nume_pacient_cu_initiala;
        private void nume_final_pacient()
        {
            int i = 0;
            while (nume_pacient[i] != ' ')
            {
                i++;
            }
            i++; 
            string prima_parte, a_doua_parte;
            prima_parte = nume_pacient.Substring(0, i);
            if (initiala_tata == "")
                a_doua_parte = nume_pacient.Substring(i, nume_pacient.Length - i);
            else
            {
                initiala_tata = char.ToUpper(initiala_tata[0]) + initiala_tata.Substring(1).ToLower().ToString();
                MessageBox.Show(initiala_tata);
                a_doua_parte = " " + nume_pacient.Substring(i, nume_pacient.Length - i);
            }
           
 
            nume_pacient_cu_initiala = prima_parte + initiala_tata + a_doua_parte;
           

        }


        private void Form4_Load(object sender, EventArgs e)
        {

            nume_pacient = textBox1.Text;
            data_curenta = textBox2.Text;
            int nr_pacienti_evidenta = xlWorkSheet.Rows.Count;

       
            for (int i = 4; i <= nr_pacienti_evidenta; i++)
            {
                if (xlWorkSheet.Cells[i,1] == data_curenta && xlWorkSheet.Cells[i,4])
                {
                    textBox3.Text = xlWorkSheet.Cells[i,2];
                    textBox4.Text = xlWorkSheet.Cells[i,3];
                }
            }

           
            
            swit = 0; a = pictureBox6.BackgroundImage;
            using (StreamReader export_orto_reader = new StreamReader(@"export_orto.ini"))
            {
                export_orto_target = export_orto_reader.ReadToEnd();
            }

            using (StreamReader luna_reader = new StreamReader(@"date_LL.ini"))
            {
                luna = Convert.ToInt32(luna_reader.ReadToEnd());
            }
            using (StreamReader anul_reader = new StreamReader(@"date_AA.ini"))
            {
                anul = Convert.ToInt32(anul_reader.ReadToEnd());
            }
            using (StreamReader ziua_reader = new StreamReader(@"date_ZZ.ini"))
            {
                ziua = Convert.ToInt32(ziua_reader.ReadToEnd());
            }
            data = new DateTime(anul, luna, ziua);
            string_data();
            textBox2.Text = ziua_string + "." + luna_string + "." + anul_string;

            destination_X = destinatie_fotografii.Substring(0, destinatie_fotografii.Length - 1);

            if (Path.GetFileName(destination_X) == "stl")
            {
                destinatie_fotografii = destinatie_fotografii.Substring(0, destinatie_fotografii.Length - 4);
                destinatie_fotografii = destinatie_fotografii + @"poze\";
              
            }
            else
            if(Path.GetFileName(destination_X)=="poze")
            {
                
            }
            else if (Path.GetFileName(destination_X) == "3Shape - fisiere")
            {
               // MessageBox.Show(Path.GetFileName(destination_X));
                destinatie_fotografii = destinatie_fotografii + @"poze\";
            }
            else
            {
                if (exista_3Shape() == 0)
                {
                    CopyFolder(@"3Shape", destination_X);
                    destinatie_fotografii = destinatie_fotografii + @"3Shape - fisiere\poze\";
                   
                }
                else if (exista_3Shape() == 1)
                    destinatie_fotografii = destinatie_fotografii + @"3Shape - fisiere\poze\";
                else if (exista_3Shape() == 2)
                {
                    CopyFolder(@"3Shape", destination_X);
                    destinatie_fotografii = destinatie_fotografii + @"3Shape - fisiere\poze\";
                   
                }
            }

   
            destinatie_fotografii_folder = destinatie_fotografii.Substring(0, destinatie_fotografii.Length - 22);
            nume_pacient_folder = new DirectoryInfo(destinatie_fotografii_folder).Name;
     
            f5.destinatie_fotografii_folder = destinatie_fotografii_folder;
            f5.nume_pacient_folder = nume_pacient_folder;
            textBox1.Text = nume_pacient_folder;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            nume_pacient = textBox1.Text;
            data_curenta = textBox2.Text;
            data_nasterii = textBox3.Text;
            initiala_tata = textBox4.Text;
            if (initiala_tata == "-") initiala_tata = "";
            nume_final_pacient();


            this.Close();

            MoveSTL();
            rezultat_final();

            creeaza_poze_pentru_pacient();
           
            f5.nume_pacient = nume_pacient;
            f5.Show();
            
        }


        #region Nimic important
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            ziua = Convert.ToInt32(textBox2.Text.Substring(0, 2));
            luna = Convert.ToInt32(textBox2.Text.Substring(3, 2));
            anul = Convert.ToInt32(textBox2.Text.Substring(6, 4));
           

            using (StreamWriter ziua_writer = new StreamWriter(@"date_ZZ.ini"))
            {

                ziua_writer.Write(ziua.ToString());
                ziua_writer.Close();
            }

            using (StreamWriter luna_writer = new StreamWriter(@"date_LL.ini"))
            {

                luna_writer.Write(luna.ToString());
                luna_writer.Close();
            }

            using (StreamWriter anul_writer = new StreamWriter(@"date_AA.ini"))
            {

                anul_writer.Write(anul.ToString());
                anul_writer.Close();
            }

        }
        #endregion



        private void Form4_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }
        private void Form4_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }
        private void Form4_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
                        Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ziua =Convert.ToInt32(textBox2.Text.Substring(0, 2));
            luna = Convert.ToInt32(textBox2.Text.Substring(3, 2));
            anul = Convert.ToInt32(textBox2.Text.Substring(6, 4));
           
            DateTime aux2;
            aux2 = new DateTime(anul, luna, ziua);
            data = aux2;


            DateTime aux;
            aux = data.AddDays(-1);
            data = aux;
           
            ziua = data.Day;
            luna = data.Month;
            anul = data.Year;
            string_data();
            textBox2.Text = ziua_string + "." + luna_string + "." + anul_string;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ziua = Convert.ToInt32(textBox2.Text.Substring(0, 2));
            luna = Convert.ToInt32(textBox2.Text.Substring(3, 2));
            anul = Convert.ToInt32(textBox2.Text.Substring(6, 4));

            DateTime aux2;
            aux2 = new DateTime(anul, luna, ziua);
            data = aux2;


            DateTime aux;
            aux = data.AddDays(1);
            data = aux;
           
            ziua = data.Day;
            luna = data.Month;
            anul = data.Year;
            string_data();
            textBox2.Text = ziua_string + "." + luna_string + "." + anul_string;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
           

            if (swit == 0)
                swit = 1;
            else swit = 0;

            if (swit == 1)
                pictureBox6.BackgroundImage = null;
            else
                pictureBox6.BackgroundImage = a;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            nume_pacient = textBox1.Text;
            data_curenta = textBox2.Text;
        //   int nr_pacienti_evidenta= xlWorkSheet.Rows.Count;
           for (int i = 4; i <= 50000; i++)
           {
               string a = xlWorkSheet.Cells[i,1].ToString();
               string b = xlWorkSheet.Cells[i,4];
               if (data_curenta == a && nume_pacient == b)
               {
                   textBox3.Text = xlWorkSheet.Cells[i,2];
                   textBox4.Text = xlWorkSheet.Cells[i,3];
               }
               else
               {
                   textBox3.Text = "";
                   textBox4.Text = "";
               }
           }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

            nume_pacient = textBox1.Text;
            data_curenta = textBox2.Text;
            int nr_pacienti_evidenta = xlWorkSheet.Rows.Count;
            for (int i = 4; i <= nr_pacienti_evidenta; i++)
            {
                string a = xlWorkSheet.Cells[i,1];
                string b = xlWorkSheet.Cells[i,4];
                if (a == data_curenta && b==nume_pacient)
                {
                    textBox3.Text = xlWorkSheet.Cells[i,2];
                    textBox4.Text = xlWorkSheet.Cells[i,3];
                }
                else
                {
                    textBox3.Text = "";
                    textBox4.Text = "";
                }
            }
        }



    }
}
