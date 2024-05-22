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
        Ally
    }
}