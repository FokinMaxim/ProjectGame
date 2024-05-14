using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ArgumentException = System.ArgumentException;

namespace ProjectGame
{
    public class Map
    {
        public Cell[,] Matrix;
        public int Size;
        public Map(int size, string img, Control.ControlCollection Controls)
        {
            Matrix = new Cell[size, size];
            Size = size;
            var dy = 0;
            var dx = 0; 
            for (var j = 0; j < size; j++)
            {
                for (var i = 0; i < size; i++)
                {
                    //var cell = new Cell(i * 130 + dx, dy, Image.FromFile(img), new Point(i, j));
                    var cell = new Cell(i * 130 + dx, dy, Image.FromFile(img), new Point(i, j));
                    Matrix[i, j] = cell;
                    Controls.Add(cell.Box);
                }
                if (j % 2 == 0) dx += 65;
                else dx -= 65;
                dy += 100;
            }
        }

        public Cell this[int x, int y]
        {
            get => Matrix[x, y];
            
            set { Matrix[x, y] = value; }
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
                var dx = pos.X + delta.X;
                var dy = pos.Y + delta.Y;
                if (InBounds(new Point(dx, dy))) yield return Matrix[dx, dy];
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
            if (From.Entity == null) return;
            if(!IsAdjacent(From, To)) return;
            
            if(To.Entity != null) DealDamage(From, To);
            else
            {
                To.Entity = From.Entity;
                From.Entity = null;
            }
            if(From.Entity != null)Console.WriteLine("From:" + From.Entity.Name);
            if(To.Entity != null)Console.WriteLine("To:" +To.Entity.Name);
        }

        public void SpawnEntity((Entity, Point)[] entityPositions)
        {
            foreach (var entity in entityPositions)
            {
                if (!InBounds(entity.Item2)) throw new ArgumentException();

                Matrix[entity.Item2.X, entity.Item2.Y].Entity = entity.Item1;
            }
        }

        public bool IsAdjacent(Cell cell1, Cell cell2) =>  GetAdjacentToCell(cell1).Contains(cell2);
        public bool InBounds(Point point)
        {
            return ((point.Y < Size && point.Y >= 0) && (point.X < Size && point.X >= 0));
        }
    }
}