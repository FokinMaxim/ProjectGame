using System.Drawing;
using System.IO;
using System.Net.Mime;

namespace ProjectGame
{
    public class Knight: IEntity
    {
        public int HealthPoints { get; set; }
        public int MaxHealth;
        public  int Attack { get; set; }
        public Image Sprite{ get; set; }
        private int killCount;
        public string FileName;
        private int CurrentActivityPoints;
        private int MaxAtivityPoints;
        public EntityType Type { get; }


        public Knight(int health, int attack, string img)
        {
            Type = EntityType.Ally;
            MaxAtivityPoints = 3;
            CurrentActivityPoints = MaxAtivityPoints;
            HealthPoints = MaxHealth = health;
            Attack = attack;
            Sprite = Image.FromFile("images\\" + img + ".png");
            killCount = 0;
            FileName = img;
        }

        public void SetActive()
        {
            CurrentActivityPoints = MaxAtivityPoints;
        }
        

        public void SetChosen()
        {
            if(!IsActive()) return;
            var path = "images\\" + FileName + "Chosen.png";
            if(File.Exists(path))Sprite = Image.FromFile(path);
        }
        
        public void UnSetChosen()
        {
            Sprite = Image.FromFile("images\\" + FileName + ".png");
        }

        
        public void RiseKillCount()
        {
            killCount += 1;
            if (killCount == 3)
            {
                killCount = 0;
                Attack += 2;
                MaxAtivityPoints += 4;
                HealthPoints = MaxHealth;
                MaxAtivityPoints += 1;
            }
        }

        public void DoSmallAction()
        {
            CurrentActivityPoints -= 1;
        }
        
        public void DoBigAction()
        {
            CurrentActivityPoints = 0;
        }

        public bool IsActive() => (CurrentActivityPoints > 0);
    }

    public class Sceleton : IEntity
    {
        public int HealthPoints { get; set; }
        public int MaxHealth;
        public  int Attack { get; set; }
        public Image Sprite{ get; set; }
        public EntityType Type { get; }
        public Cell Cell { set; get; }
        private bool ActivityFlag;
        public string FileName;

        public Sceleton(int health, int attack, string img)
        {
            Type = EntityType.Foe;
            HealthPoints = MaxHealth = health;
            Attack = attack;
            Sprite = Image.FromFile("images\\" + img + ".png");
            FileName = img;
        }

        public void SetActive()
        {
            ActivityFlag = true;}

        public void UnsetActive()
        {
            ActivityFlag = false;
        }

        public void SetChosen()
        { }

        public void UnSetChosen()
        { }

        public void RiseKillCount()
        { }

        public bool IsActive()
        {
            return ActivityFlag;
        }
    }
}