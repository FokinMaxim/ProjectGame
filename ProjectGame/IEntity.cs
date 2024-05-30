using System;
using System.Drawing;
using System.Net.Mime;
using System.Windows.Forms;

namespace ProjectGame
{
    public interface IEntity
    {
        int HealthPoints { get; set; }
        int Attack { get; set; }
        Image Sprite { get; set; }
        EntityType Type { get; }

        
        void SetActive();
        bool IsActive();
        EntityInfo GetInfo();
    }

    public interface IPlayable : IEntity
    {
         void SetChosen();
         void UnSetChosen();
         void TryHeal();
         void RiseKillCount();
    }
    
    public enum EntityType
    {
        Foe,
        Ally,
        Castle,
        Grave
    }

    public class EntityInfo
    {
        [PropertyName("Тип: ")]
        public string Name { get; set; }
        [PropertyName("Очки Здоровья: ")]
        public string HealthPoints { get; set; }
        [PropertyName("Атака: ")]
        public string Attack { get; set; }
        [PropertyName("Очки активности: ")]
        public string ActivityPoints { get; set; }
        
        [PropertyName("Ходов до подкрепления: ")]
        public string TurnsToReinforcement { get; set; }
        [PropertyName("Убийства: ")]
        public string Kills { get; set; }
        [PropertyName("Информация и способности: ")]
        public string Information { get; set; }
    }
    
    public class PropertyNameAttribute : Attribute
    {
        public PropertyNameAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}