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
        public Entity Entity;
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
                Size = new Size(130, 130), // Эту проблему я не преодолел. По какой-то причине,
                                           // img.Size возвращает (100, 100), а рисует как (130, 130)
                BackColor = Color.Transparent
                // Picturebox всех клеток прозрачные, чтобы они просто находились поверх клеток и принимали информацию,
                // а изображение за ней будет перерисовываться каждый раз после изменения системы
            };
            Box.Click += CheckClicability;
            Box.Paint += PaintBox;
        }

        public void SetChosen()
        {
            if (Entity != null) Entity.SetChosen();
        }
        
        public void UnSetChosen()
        {
            if (Entity != null) Entity.UnSetChosen();
        }
        
        private void CheckClicability(object sender, EventArgs e)
        {
            Console.WriteLine(MapPosition);
            
            Form myForm = Box.FindForm();
            if (myForm is Form1) ((Form1)myForm).WWWAAAGH(this);
            //Controle.RecieveSignal(this);
        }

        private void PaintBox(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Image, WindowPosition);
            if (Entity != null) e.Graphics.DrawImage(Entity.Sprite, WindowPosition);
        }
    }
}