using System.Drawing;


namespace ProjectGame
{
    public static class View
    {
        private static Graphics graphics;
        public static void PaintMap(Map map)
        {
            var cell = map[0, 0];
            var im = Image.FromFile(cell.ImageName);
            graphics.DrawImage(im, new Point(0, 0));
        }
    }
}