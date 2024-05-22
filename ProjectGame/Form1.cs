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
        private Game game;
        public Form1()
        {
            game = new Game(Controls);
            //MaximizeBox = false;
            //WindowState = FormWindowState.Maximized;
            //FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public void WWWAAAGH(object newChosenElement)
        {
            Console.WriteLine("eee");
            game.SetNewChosenElement(newChosenElement);
            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            game.PaintMap(e);
        }
    }
}