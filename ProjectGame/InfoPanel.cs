using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace ProjectGame
{
    public class InfoPanel
    {
        public Rectangle FonRectangle;
        public EntityInfo Info;
        public readonly Label TextLabel;

        public InfoPanel(Rectangle rectangle)
        {
            FonRectangle = rectangle;
            Info = null;
            TextLabel = new Label();
            TextLabel.Text = "";
            TextLabel.Location = new Point(20 + FonRectangle.X, 20 + FonRectangle.Y);
            TextLabel.Size = new Size(FonRectangle.Width - 40, FonRectangle.Height - 40);
        }

        public void SetNewInfo(IEntity entity)
        {
            Info = entity.GetInfo();
        }
    }
}