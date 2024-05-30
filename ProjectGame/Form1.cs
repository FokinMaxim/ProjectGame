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
             MaximizeBox = false;
             WindowState = FormWindowState.Maximized;
             //Size = new Size(10000, 10000);
             FormBorderStyle = FormBorderStyle.FixedSingle;
            game = new Game(Controls);
        }

        public void TransferSignal(object newChosenElement)
        {
            game.SetNewChosenElement(newChosenElement);
            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            game.PaintMap(e);
        }
    }
}