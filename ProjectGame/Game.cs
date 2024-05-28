using System;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Windows.Forms;

namespace ProjectGame
{
    public class Game
    {
        private object chosenElement;
        private Map map;
        private View visualiser;
        private Control.ControlCollection Controls;
        private PictureBox endTurnButton;
        private InfoPanel infoPanel;
        public Game(Control.ControlCollection controls)
        {
            Controls = controls;
            visualiser = new View();
            chosenElement = new object();
            map = new Map(9, "images\\greenCell.png", Controls);
            endTurnButton = CreateEndButton();
            infoPanel = CreateInfoPanel();
            
            var knight1 = new Knight(2, 2, "knight");
            var skeleton = new Sceleton(4, 2, "skeleton");
            //var castle = new Castle(20, "castle");
            map.SpawnEntity(new (IEntity, Point)[]
            {
                (knight1, new Point(0, 4)),
                (skeleton, new Point(5, 0)),
                //(castle, new Point(map.Size/2, map.Size/2))
            });
        }

        public void PaintMap(PaintEventArgs e)
        {
            visualiser.PaintMap(map, e);
            visualiser.DrawInfoPanel(infoPanel, map[0, 0]);
        }
        
        public void SetNewChosenElement(object newChosenElement)
        {
            if (newChosenElement is Cell)
            {
                var GoTo = (Cell)newChosenElement;
                if (GoTo.Entity != null)
                {
                    infoPanel.SetNewInfo(GoTo.Entity);
                    visualiser.DrawInfoPanel(infoPanel, GoTo);
                }
                if (chosenElement != null && chosenElement is Cell)
                {
                    var GoFrom = (Cell)chosenElement;
                    map.Move(GoFrom, GoTo);
                    GoFrom.UnSetChosen();
                    visualiser.RedrawCell(new[] { GoFrom });
                }
                GoTo.SetChosen();
                visualiser.RedrawCell(new[] { GoTo });
            }
            chosenElement = newChosenElement;
        }

        private void EndTurn(object sender, EventArgs e)
        {
            Console.WriteLine("EEE");
            map.EndTurn();
            visualiser.RedrawCell(map.GetCells().ToArray());
        }

        private PictureBox CreateEndButton()
        {
            var delta = map[map.Size - 1, map.Size - 1].WindowPosition;
            var endTurnButton = new PictureBox()
            {
                Image = Image.FromFile("images\\endTurnButton.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(200, 200),
                Location = new Point(delta.X +300, delta.Y - 100)
            }; 
            endTurnButton.Click += EndTurn;
            Controls.Add(endTurnButton);
            return endTurnButton;
        }

        private InfoPanel CreateInfoPanel()
        {
            var delta = map[map.Size - 1, 0].WindowPosition;
            var ip = new InfoPanel(new Rectangle(delta.X + 200, delta.Y, 400, 700));
            Controls.Add(ip.TextLabel);
            return ip;
        }
    }
}