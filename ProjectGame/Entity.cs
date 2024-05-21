using System.Drawing;
using System.IO;
using System.Net.Mime;

namespace ProjectGame
{
    public class Entity
    {
        public int HealthPoints;
        public  int Attack;
        public Image Sprite;
        public int KillCount;
        public string FileName;
        public bool IsActive;

        public Entity(int health, int attack, string img)
        {
            HealthPoints = health;
            Attack = attack;
            Sprite = Image.FromFile("images\\" + img + ".png");
            //KillCount = 0;
            FileName = img;
            IsActive = true;
        }

        public void SetActive()
        {
            IsActive = true;
        }
        
        public void UnSetActive()
        {
            IsActive = false;
        }

        public void SetChosen()
        {
            var path = "images\\" + FileName + "Chosen.png";
            if(File.Exists(path))Sprite = Image.FromFile(path);
        }
        
        public void UnSetChosen()
        {
            Sprite = Image.FromFile("images\\" + FileName + ".png");
        }
    }
}