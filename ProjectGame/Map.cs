using System;
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
            var dy = 0;
            var dx = 0; 
            for (var j = 0; j < size; j++)
            {
                for (var i = 0; i < size; i++)
                {
                    map[i, j] = new Cell(i * 130 + dx, dy, Image.FromFile(img), new Point(i, j));
                }
                if (j % 2 == 0) dx += 65;
                else dx -= 65;
                dy += 100;
            }
        }

        public Cell this[int x, int y]
        {
            get => map[x, y];
            
            set { map[x, y] = value; }
        }

        private readonly Point[] AdjacenceTemplate = 
        {
            new Point(1, 0), new Point(0, 1), new Point(1, 1),
            new Point(-1, 0), new Point(0, -1), new Point(1, -1),
        };
        public IEnumerable<Cell> GetAdjacentToCell(Cell cell)
        {
            var pos = cell.MapPosition;
            foreach (var delta in AdjacenceTemplate)
            {
                yield return map[pos.X + delta.X, pos.Y + delta.Y];
            }
        }

        public void DealDamage(Cell Dealer, Cell Reciwer)
        {
            if (Dealer.Entity == null || Reciwer.Entity == null) throw new ArgumentException();

            if (Reciwer.Entity.HealthPoints - Dealer.Entity.Attack <= 0)
            {
                Dealer.Entity.KillCount++;
                Reciwer.Entity = null;
            }
        }

        public void Move(Cell From, Cell To)
        {
            if (From.Entity == null) throw new ArgumentException();
            
            if(To.Entity != null) DealDamage(From, To);
            else
            {
                To.Entity = From.Entity;
                From.Entity = null;
            }
        }
    }
}