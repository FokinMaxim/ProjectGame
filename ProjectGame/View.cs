using System.Drawing;
using System.Windows.Forms;
using static System.Math;

namespace ProjectGame
{
    public class View
    {
        public  void PaintMap(Map map, PaintEventArgs e)
        {
            var dy = 0;
            var dx = 0; 
            for (var j = 0; j < map.Size; j++)
            {
               for (var i = 0; i < map.Size; i++)
               {
                   var cell = map[i, j];
                   PaintCell(cell, e.Graphics);
                   //e.Graphics.DrawImage(cell.Sprite.Image, new Point(i * (130) + dx, dy));
               }
               if (j % 2 == 0) dx += 65;
               else dx -= 65;
               dy += 100;
            }
        }

        public  void RedrawCell(Cell[] cells)
        {
            var myForm = cells[0].Box.FindForm();
            var formgraf = myForm.CreateGraphics();
            foreach (var cell in cells)
            {
                var gr = cell.Box.CreateGraphics();
                gr.DrawImage(cell.Image, new Point(0, 0));
               if (cell.Entity != null)
               { 
                   var delta = (cell.Image.Size.Height -  cell.Entity.Sprite.Size.Height)/2;
                   DrawEntity(gr,cell, new Point(delta, delta)); 
               }
            }
        }

        public  void PaintCell(Cell cell, Graphics gr)
        {
            gr.DrawImage(cell.Image, cell.WindowPosition);
            if (cell.Entity != null)
            {
                var delta = (cell.Image.Size.Height -  cell.Entity.Sprite.Size.Height)/2;
                DrawEntity(gr, cell, new Point(cell.WindowPosition.X + delta, cell.WindowPosition.Y + delta));
                //gr.DrawImage(cell.Entity.Sprite, new Point(cell.WindowPosition.X + delta, cell.WindowPosition.Y + delta));
            }
        }

        public void DrawEntity(Graphics gr, Cell cell, Point delta)
        {
            gr.DrawImage(cell.Entity.Sprite, new Point(delta.X, delta.Y));
            
            var fontFamily = new FontFamily("Arial");
            var font = new Font(fontFamily, 28, FontStyle.Bold, GraphicsUnit.Pixel);
            
            gr.DrawString(cell.Entity.HealthPoints.ToString(), 
                font, new SolidBrush(Color.Red), new PointF(
                    (float)delta.X + cell.Entity.Sprite.Size.Height, 
                    (float)delta.Y + cell.Entity.Sprite.Size.Width));
        }
    }
}