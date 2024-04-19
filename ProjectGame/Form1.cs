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
            var gr = e.Graphics;
            Image im = Image.FromFile("images/greenCell.png");
            gr.DrawImage(im, new Point(0, 0));
        }

    }
}