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
        private Map Map;
        public Form1()
        {
            //MaximizeBox = false;
            //WindowState = FormWindowState.Maximized;
            //FormBorderStyle = FormBorderStyle.FixedSingle;
            Map = new Map(3, "images\\greenCell.png", Controls);
            Controle.Map = Map;
            
            var knight1 = new Entity(4, 2, "knight");
            var skeleton = new Entity(4, 1, "skeleton");
            Map.SpawnEntity(new []
            {
                (knight1, new Point(1, 1)),
                (skeleton, new Point(0, 0))
            });
        }

        public void WWWAAAGH(EventArgs e)
        {
            Console.WriteLine("eee");
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            View.PaintMap(Map, e);
        }
    }
}