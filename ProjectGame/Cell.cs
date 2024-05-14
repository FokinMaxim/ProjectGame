using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectGame
{
    public class Cell
    {
        public PictureBox Sprite;
        public Entity Entity;
        public Point WindowPosition;
        public Size ImageSize;
        public Point MapPosition;

        public Cell(int x, int y, Image img, Point mapPosition)
        {
            WindowPosition = new Point(x, y);
            MapPosition = mapPosition;
            Sprite = new PictureBox(){Image = img};
            Sprite.Location = WindowPosition;
            ImageSize = Sprite.Size;
            Sprite.Click += CheckClicability;
        }

        private void CheckClicability(object sender, EventArgs e)
        {
            Console.WriteLine("asasas");
        }
    }
}