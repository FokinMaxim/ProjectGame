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
        void SetChosen();
        void UnSetChosen();
        void RiseKillCount();
        bool IsActive();
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
        public int HealthPoints { get; set; }
        [PropertyName("Атака: ")]
        public int Attack { get; set; }
        [PropertyName("Ходов до подкрепления: ")]
        public int TurnsToReinforcement { get; set; }
        [PropertyName("Убийства: ")]
        public int Kills { get; set; }
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