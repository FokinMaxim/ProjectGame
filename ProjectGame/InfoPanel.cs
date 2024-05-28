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
        public System.Windows.Forms.Label TextLabel;

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
            var newInfo = new EntityInfo();

            newInfo.HealthPoints = entity.HealthPoints;

            if (entity is Knight)
            {
                var knight = (Knight)entity;

                newInfo.Kills = 3 - knight.killCount;
                newInfo.Attack = knight.Attack;
                newInfo.Name = "Рыцарь";
                newInfo.Information = @"Получает плюс " + knight.AddAtack +
                                      " к атаке за каждого союзника по соседству, когда нападает на врага." +
                                      "Каждые 3 убийства полностью излечивается и увеличивает урон";
            }

            if (entity is Castle)
            {
                var castle = (Castle)entity;

                newInfo.Name = "Замок";
                newInfo.TurnsToReinforcement = castle.TurnsToReinforcement;
                newInfo.Information = @"Каждые 3 хода призывает нового союзника на соседнюю с ним клетку. Если Замок разрушат, вы проиграете ";
            }

            Info = newInfo;
        }
    }
}