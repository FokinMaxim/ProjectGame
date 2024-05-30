using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Windows.Forms.VisualStyles;

namespace ProjectGame
{
    public class Knight: IPlayable
    {
        public int HealthPoints { get; set; }
        private int MaxHealth;
        public  int Attack { get; set; }
        public Image Sprite{ get; set; }
        public int AddAttack{ get; set; }
        private int killCount;
        private string FileName;
        private int CurrentActivityPoints;
        private int MaxAtivityPoints;
        public EntityType Type { get; }


        public Knight()
        {
            Type = EntityType.Ally;
            MaxAtivityPoints = Constants.knightActivityPoints;
            CurrentActivityPoints = MaxAtivityPoints;
            HealthPoints = MaxHealth = Constants.knightHealth;
            Attack = Constants.knightAttack;
            AddAttack = Constants.knightAdditionalAttack;
            Sprite = Image.FromFile("images\\knight.png");
            killCount = 0;
            FileName = "knight";
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
                AddAttack += 1;
                Attack += 2;
                MaxAtivityPoints += 1;
                HealthPoints = MaxHealth + 4;
                MaxHealth += 4;
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

        public void TryHeal()
        {
            if (CurrentActivityPoints == MaxAtivityPoints) HealthPoints = Math.Min(HealthPoints + 3, MaxHealth);
        }

        public bool IsActive() => CurrentActivityPoints > 0;

        public EntityInfo GetInfo()
        {
            var newInfo = new EntityInfo();
            
            newInfo.Name = "Рыцарь";
            newInfo.Kills = (3 - killCount).ToString();
            newInfo.Attack = Attack.ToString();
            newInfo.HealthPoints = HealthPoints.ToString();
            newInfo.ActivityPoints = CurrentActivityPoints.ToString();
            newInfo.Attack = Attack.ToString();
            newInfo.Information = @"Получает плюс " + AddAttack +
                                  " к атаке за каждого союзника по соседству, когда нападает на врага." +
                                  "Если ничего не делает за ход, восстанавливает 3 очка здоровья." +
                                  "После первых 3 убийств полностью излечивается и увеличивает урон.";
            return newInfo;
        }
    }



    public class Rider :IPlayable // Буду честен, на разработку каваллериста я потратил примерно 20 минут и скорее всего где-то накосячил, но не смог удержаться от его добавления
    {
        public int HealthPoints { get; set; }
        private int MaxHealth;
        public  int Attack { get; set; }
        public Image Sprite{ get; set; }
        private int killCount;
        private string FileName;
        private int CurrentActivityPoints;
        private int MaxAtivityPoints;
        public EntityType Type { get; }


        public Rider()
        {
            Type = EntityType.Ally;
            MaxAtivityPoints = Constants.riderActivityPoints;
            CurrentActivityPoints = MaxAtivityPoints;
            HealthPoints = MaxHealth = Constants.riderHealth;
            Attack = Constants.riderAttack;
            Sprite = Image.FromFile("images\\rider.png");
            killCount = 0;
            FileName = "rider";
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
                Attack += 2;
                MaxAtivityPoints += 2;
                HealthPoints = MaxHealth + 4;
                MaxHealth += 4;
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

        public void TryHeal()
        {
            if (CurrentActivityPoints == MaxAtivityPoints) HealthPoints = Math.Min(HealthPoints + 3, MaxHealth);
        }

        public bool IsActive() => CurrentActivityPoints > 0;

        public int GetAdditionalAttack => MaxHealth - CurrentActivityPoints;

        public EntityInfo GetInfo()
        {
            var newInfo = new EntityInfo();
            
            newInfo.Name = "Каваллерист";
            newInfo.Kills = killCount.ToString();
            newInfo.Attack = Attack.ToString();
            newInfo.HealthPoints = HealthPoints.ToString();
            newInfo.ActivityPoints = CurrentActivityPoints.ToString();
            newInfo.Attack = Attack.ToString();
            newInfo.Information = @"Очень быстрый атакующий юнит. Получает плюс 1 к атаке за каждую пройденную на ходу клетку" + 
                                  "Если ничего не делает за ход, восстанавливает 3 очка здоровья." +
                                  "После первых 3 убийств полностью излечивается и увеличивает урон.";
            return newInfo;
        }
    }
    
    public class Skeleton : IEntity
    {
        public int HealthPoints { get; set; }
        public  int Attack { get; set; }
        public Image Sprite{ get; set; }
        public EntityType Type { get; }
        public Cell Cell { set; get; }
        private bool ActivityFlag;

        public Skeleton()
        {
            Type = EntityType.Foe;
            HealthPoints = Constants.sceletonHealth;
            Attack = Constants.sceletonAttack;
            Sprite = Image.FromFile("images\\skeleton.png");
        }

        public void SetActive()
        {
            ActivityFlag = true;}

        public void UnsetActive()
        {
            ActivityFlag = false;
        }
        
        public bool IsActive()
        {
            return ActivityFlag;
        }

        public EntityInfo GetInfo()
        {
            var newInfo = new EntityInfo();
            
            newInfo.Attack = Attack.ToString();
            newInfo.Name = "Скелет";
            newInfo.Information = @"Этот враг пытается унечтожить ваш замок";
            return newInfo;
        }
    }

    public class Castle: IPlayable
    {
        public int HealthPoints { get; set; }
        public int Attack { get; set; }
        public Image Sprite { get; set; }
        public EntityType Type { get; }
        private int TurnsToReinforcement{ get; set; }
        private string FileName;

        public Castle()
        {
            HealthPoints = Constants.castleHealth;
            Attack = 0;
            Sprite = Image.FromFile("images\\castle.png");
            FileName = "castle";
            Type = EntityType.Castle;
            TurnsToReinforcement = 3;
        }

        public void SetActive()
        {
        }
        public void DoSmallAction(){}
        public void DoBigAction(){}
        public void RiseKillCount(){}
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
        
        public void TryHeal(){}

        public void UnSetChosen()
        {
            Sprite = Image.FromFile("images\\" + FileName + ".png");
        }

        public bool IsActive()
        {
            return false;
        }
        public EntityInfo GetInfo()
        {
            var newInfo = new EntityInfo();
            
            newInfo.Name = "Замок";
            newInfo.TurnsToReinforcement = TurnsToReinforcement.ToString();
            newInfo.Information = @"Каждые 3 хода призывает нового союзника на соседнюю с ним клетку. Если Замок разрушат, вы проиграете.";
            return newInfo;
        }
    }
    
    public class Grave: IEntity
    {
        public int HealthPoints { get; set; }
        public int Attack { get; set; }
        public Image Sprite { get; set; }
        public EntityType Type { get; }
        private int TurnsToStartSpawning{ get; set; }
        
        private string FileName;

        public Grave(int turnsToStartSpawning)
        {
            TurnsToStartSpawning = turnsToStartSpawning;
            HealthPoints = 0;
            Attack = 0;
            Sprite = Image.FromFile("images\\grave.png");
            FileName = "grave";
            Type = EntityType.Grave;
        }

        public bool CanSpawn()
        {
            if (TurnsToStartSpawning == 0) return true;
            
            TurnsToStartSpawning -= 1;
            return false;
        }

        public void SetActive()
        {
        }
       

        public bool IsActive()
        {
            return false;
        }
        public EntityInfo GetInfo()
        {
            var newInfo = new EntityInfo();
            
            newInfo.Name = "Кладбище";
            if (TurnsToStartSpawning != 0) newInfo.Information = "Начнёт призывать скелетов через " + TurnsToStartSpawning + " ходов";
            else newInfo.Information = "Каждый ход призывает нового скелета";
            
            return newInfo;
        }
    }
}