using System.Drawing;
using System.Windows.Forms;
using static System.Math;

namespace ProjectGame
{
    public static class View
    {
        public static void PaintMap(Map map, PaintEventArgs e)
        {
            var dy = 0;
            var dx = 0; 
            for (int j = 0; j < 3; j++)
            {
               for (int i = 0; i < 3; i++)
               {
                   var cell = map[i, j];
                   PaintCell(cell, e);
                   //e.Graphics.DrawImage(cell.Sprite.Image, new Point(i * (130) + dx, dy));
               }
               if (j % 2 == 0) dx += 65;
               else dx -= 65;
               dy += 100;
            }
        }

        public static void RedrawCell(Cell[] cells)
        {
            foreach (var cell in cells)
            {
                var gr = cell.Box.CreateGraphics();
                gr.DrawImage(cell.Image, cell.WindowPosition);
                if (cell.Entity != null)// На моменте отрисови произходит проблема
                {
                    var delta = (cell.Image.Size.Height -  cell.Entity.Sprite.Size.Height)/2;
                    
                    gr.DrawImage(cell.Entity.Sprite, new Point(
                        cell.WindowPosition.X + delta, 
                        cell.WindowPosition.Y + delta));
                }
            }
        }

        public static void PaintCell(Cell cell, PaintEventArgs e)
        {
            e.Graphics.DrawImage(cell.Image, cell.WindowPosition);
            if (cell.Entity != null)
            {
                var delta = (cell.Image.Size.Height -  cell.Entity.Sprite.Size.Height)/2;
                e.Graphics.DrawImage(cell.Entity.Sprite, new Point(
                    cell.WindowPosition.X + delta, 
                    cell.WindowPosition.Y + delta));
            }
        }
    }
}