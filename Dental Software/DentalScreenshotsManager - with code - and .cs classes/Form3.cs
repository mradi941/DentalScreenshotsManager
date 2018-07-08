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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.TransparencyKey = (BackColor);
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemtilde)
            {
                    
                    int lungime, latime; // lungimea si latimea pozei crop-ate.
                    lungime = this.Size.Height - 148;
                    latime = this.Size.Width - 18;
                    Bitmap scr = new Bitmap(latime, lungime);
                    int x, y; // coordonatele punctului de unde se cropuieste poza
                    x = this.Location.X + 9;
                    y = this.Location.Y + 138;
                    Graphics grp = Graphics.FromImage(scr as Image);
                    grp.CopyFromScreen(x, y, 0, 0, scr.Size);
                    scr.Save(@"background.jpg");
                    MessageBox.Show("BACKGROUND set!"); 
                    this.Close();
                
            }
        }
    }
}
