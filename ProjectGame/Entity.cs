using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Windows.Forms.VisualStyles;

namespace ProjectGame
{
    public class Knight: IEntity
    {
        public int HealthPoints { get; set; }
        private int MaxHealth;
        public  int Attack { get; set; }
        public Image Sprite{ get; set; }
        public int AddAtack{ get; set; }
        public int killCount;
        private string FileName;
        private int CurrentActivityPoints;
        private int MaxAtivityPoints;
        public EntityType Type { get; }


        public Knight(string img)
        {
            Type = EntityType.Ally;
            MaxAtivityPoints = 3;
            CurrentActivityPoints = MaxAtivityPoints;
            HealthPoints = MaxHealth = 10;
            Attack = 5;
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
                AddAtack += 1;
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

        public Sceleton(string img)
        {
            Type = EntityType.Foe;
            HealthPoints = MaxHealth = 10;
            Attack = 2;
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

    public class Castle: IEntity
    {
        public int HealthPoints { get; set; }
        public int Attack { get; set; }
        public Image Sprite { get; set; }
        public EntityType Type { get; }
        public int TurnsToReinforcement{ get; set; }
        private string FileName;

        public Castle(int health, string img)
        {
            HealthPoints = health;
            Attack = 0;
            Sprite = Image.FromFile("images\\" + img + ".png");
            FileName = img;
            Type = EntityType.Castle;
            TurnsToReinforcement = 3;
        }

        public void SetActive()
        {
        }

        public bool TrySpawn()
        {
            if (TurnsToReinforcement == 1)
            {
                TurnsToReinforcement = 3;
                return true;
            }

            TurnsToReinforcement -= 1;
            return false;
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
        public void RiseKillCount(){}

        public bool IsActive()
        {
            return false;
        }
    }
    
    public class Grave: IEntity
    {
        public int HealthPoints { get; set; }
        public int Attack { get; set; }
        public Image Sprite { get; set; }
        public EntityType Type { get; }
        public int TurnsToReinforcement{ get; set; }
        private string FileName;

        public Grave(string img)
        {
            HealthPoints = 0;
            Attack = 0;
            Sprite = Image.FromFile("images\\" + img + ".png");
            FileName = img;
            Type = EntityType.Grave;
        }

        public void SetActive()
        {
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
        public void RiseKillCount(){}

        public bool IsActive()
        {
            return false;
        }
    }
}