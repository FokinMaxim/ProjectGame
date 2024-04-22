using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var Map = new Map(3, "C:\\Users\\User\\RiderProjects\\ProjectGame\\ProjectGame\\images\\greenCell.png");
            View.PaintMap(Map);
        }

    }
}