using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using System.Windows.Forms;

namespace ProjectGame
{
    public class Cell
    {
        public PictureBox Box;
        public Image Image;
        public IEntity Entity;
        public Point WindowPosition;
        public Point MapPosition;

        public Cell(int x, int y, Image img, Point mapPosition)
        {
            WindowPosition = new Point(x, y);
            MapPosition = mapPosition;
            Image = img;
            Box = new PictureBox()
            {
                Location = WindowPosition,
                Size = new Size(130, 130), 
                BackColor = Color.Transparent
            };
            Box.Click += CheckClicability;
        }

        public void SetChosen()
        {
            if (Entity != null && Entity is IPlayable) ((IPlayable)Entity).SetChosen();
        }
        
        public void UnSetChosen()
        {
            if (Entity != null && Entity is IPlayable)((IPlayable)Entity).UnSetChosen();
        }
        
        private void CheckClicability(object sender, EventArgs e)
        {
            Console.WriteLine(MapPosition);
            
            Form myForm = Box.FindForm();
            if (myForm is Form1) ((Form1)myForm).TransferSignal(this);
        }
        
    }
}