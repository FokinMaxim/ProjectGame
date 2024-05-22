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
        public Control.ControlCollection Controls;
        public Game(Control.ControlCollection controls)
        {
            Controls = controls;
            visualiser = new View();
            chosenElement = new object();
            map = new Map(3, "images\\greenCell.png", Controls);
            
            var knight1 = new Entity(4, 2, "knight");
            var skeleton = new Entity(4, 1, "skeleton");
            map.SpawnEntity(new []
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
                    visualiser.RedrawCell(new []{GoFrom});
                }
                GoTo.SetChosen();
                chosenElement = newChosenElement;
                visualiser.RedrawCell(new[] { GoTo });
            }
        }
    }
}