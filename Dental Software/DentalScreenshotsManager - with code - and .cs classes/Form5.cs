using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace screenshots
{
    public partial class Form5 : Form
    {
       public int start_cronometru;
        public string nume_pacient;
        public string destinatie_fotografii_folder, nume_pacient_folder;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

           start_cronometru = 55;
            label1.Text = "Pozele & Sumarele pacientului \"" + nume_pacient_folder + "\" au fost realizate!!!";


            #region

            #endregion


        }
        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (start_cronometru==55)
            {
                if (i == 2)
                {
                    Application.Exit();
                }
                i++;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
