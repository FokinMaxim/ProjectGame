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
            Map = new Map(3, "images\\greenCell.png", Controls);
            Controle.Map = Map;
            
            var Knight1 = new Entity(2, 1, Image.FromFile("images\\knight.png"), "First");
            //var Knight2 = new Entity(1, 1, Image.FromFile("images\\knight.png"), "Second");
            Map.SpawnEntity(new []{(Knight1, new Point(1, 1)),});
        }
        
        
        protected override void OnPaint(PaintEventArgs e)
        {
            View.PaintMap(Map, e);
        }
        
    }
    
    
}