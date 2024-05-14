using System.Drawing;
using System.Net.Mime;

namespace ProjectGame
{
    public class Entity
    {
        public int HealthPoints;
        public readonly int Attack;
        public Image Sprite;
        public int KillCount;
        public string Name;

        public Entity(int health, int attack, Image img, string name)
        {
            HealthPoints = health;
            Attack = attack;
            Sprite = img;
            KillCount = 0;
            Name = name;
        }

    }
}