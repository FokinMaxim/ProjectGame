using System.Drawing;
using System.Windows.Forms;


namespace ProjectGame
{
    public static class View
    {
        public static void PaintMap(Map map, PaintEventArgs e)
        {
            var cell = map[0, 0];
            var im = Image.FromFile("C:\\Users\\User\\RiderProjects\\ProjectGame\\ProjectGame\\images\\greenCell.png");
            e.Graphics.DrawImage(im, new Point(0, 0));
        }
    }
}