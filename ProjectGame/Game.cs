using System;
using System.Drawing;
using System.Linq;
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
        public Game(Control.ControlCollection controls)
        {
            Controls = controls;
            visualiser = new View();
            chosenElement = new object();
            
            map = new Map(7, "images\\greenCell.png", Controls);
            endTurnButton = EndButtonCreate();
            var knight1 = new Knight(2, 2, "knight");
            var skeleton = new Sceleton(4, 2, "skeleton");
            map.SpawnEntity(new (IEntity, Point)[]
            {
                (knight1, new Point(1, 1)),
                (skeleton, new Point(0, 0))
            });
        }

        public void PaintMap(PaintEventArgs e)
        {
            visualiser.PaintMap(map, e);
        }
        
        public void SetNewChosenElement(object newChosenElement)
        {
            if (newChosenElement is Cell)
            {
                var GoTo = (Cell)newChosenElement;
                
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

        private PictureBox EndButtonCreate()
        {
            var delta = map[map.Size - 1, 0].WindowPosition;
            var endTurnButton = new PictureBox()
            {
                Image = Image.FromFile("images\\endTurnButton.png"),
                Location = new Point(delta.X + 100, delta.Y + 0)
            }; 
            endTurnButton.Click += EndTurn;
            Controls.Add(endTurnButton);
            return endTurnButton;
        }
    }
}