using System.Collections.Generic;
using System.Drawing;

namespace ProjectGame
{
    public class Map
    {
        public Cell[,] map;
        public Map(int size, string img)
        {
            map = new Cell[size, size];
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    map[x, y] = new Cell(x, y, img);
                }
            }
        }

        public Cell this[int x, int y]
        {
            get => map[x, y];
            
            set { map[x, y] = value; }
        }
    }
}