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
        private Cell[,] matrix;
        private  List<IEntity> allyArray;
        private List<Sceleton> foeArray;
        public readonly int Size;
        private List<IEntity> entityList;
        public Map(int size, string img, Control.ControlCollection Controls)
        {
            allyArray = new List<IEntity>();
            foeArray = new List<Sceleton>();
            matrix = new Cell[size, size];
            Size = size;
            var dy = 0;
            var dx = 0; 
            for (var j = 0; j < size; j++)
            {
                for (var i = 0; i < size; i++)
                {
                    //var cell = new Cell(i * 130 + dx, dy, Image.FromFile(img), new Point(i, j));
                    var cell = new Cell(i * 130 + dx, dy, Image.FromFile(img), new Point(i, j));
                    matrix[i, j] = cell;
                    Controls.Add(cell.Box);
                }
                if (j % 2 == 0) dx += 65;
                else dx -= 65;
                dy += 100;
            }
        }

        public Cell this[int x, int y]
        {
            get => matrix[x, y];
            
            set { matrix[x, y] = value; }
        }

        private readonly Point[] OddAdjacenceTemplate = 
        {
            new Point(1, 0), new Point(0, 1), new Point(1, 1),
            new Point(-1, 0), new Point(0, -1), new Point(1, -1),
        };
        
        private readonly Point[] EvenAdjacenceTemplate = 
        {
            new Point(1, 0), new Point(0, 1), new Point(-1, -1),
            new Point(-1, 0), new Point(0, -1), new Point(-1, 1),
        };
        public IEnumerable<Cell> GetAdjacentToCell(Cell cell)
        {
            var template = new Point[] { };
            var pos = cell.MapPosition;
            if (cell.MapPosition.Y % 2 == 0)
                template = EvenAdjacenceTemplate;
            else
                template = OddAdjacenceTemplate;
            foreach (var delta in template)
            {
                var dx = pos.X + delta.X;
                var dy = pos.Y + delta.Y;
                if (InBounds(new Point(dx, dy))) yield return matrix[dx, dy];
            }
        }

        public void DealDamage(Cell Dealer, Cell Reciwer)
        {
            if (Dealer.Entity == null || Reciwer.Entity == null) throw new ArgumentException();
            if (Dealer.Entity.Type == Reciwer.Entity.Type) return;
            {
                
            }
            if (Dealer.Entity is Knight)
            {
                var knight = (Knight)Dealer.Entity;
                knight.DoBigAction();
            }
            if (Reciwer.Entity.HealthPoints - Dealer.Entity.Attack <= 0)
            {
                Dealer.Entity.RiseKillCount();
                Reciwer.Entity = null;
            }
            else
            {
                Reciwer.Entity.HealthPoints -= Dealer.Entity.Attack;
                Dealer.Entity.HealthPoints -= Reciwer.Entity.Attack;
                if (Dealer.Entity.HealthPoints <= 0)
                {
                    Reciwer.Entity.RiseKillCount();
                    Dealer.Entity = null; 
                }
            }
        }

        public void Move(Cell From, Cell To)
        {
            if (From.Entity == null) return;
            if(!IsAdjacent(From, To)) return;
            if(!From.Entity.IsActive()) return;
            
            if(To.Entity != null) DealDamage(From, To);
            else
            {
                if (From.Entity is Knight)
                {
                    var knight = (Knight)From.Entity;
                    knight.DoSmallAction();
                }

                if (From.Entity.Type == EntityType.Foe)
                {
                    var skeleton = (Sceleton)From.Entity;
                    skeleton.Cell = To;
                }
                To.Entity = From.Entity;
                From.Entity = null;
            }
        }

        public void SpawnEntity((IEntity, Point)[] entityPositions)
        {
            foreach (var entity in entityPositions)
            {
                if (entity.Item1.Type == EntityType.Ally) allyArray.Add(entity.Item1);
                if (!InBounds(entity.Item2)) throw new ArgumentException();
                if (entity.Item1.Type == EntityType.Foe)
                {
                    var skelet = (Sceleton)entity.Item1;
                    skelet.Cell = matrix[entity.Item2.X, entity.Item2.Y];
                    foeArray.Add(skelet);
                }
                matrix[entity.Item2.X, entity.Item2.Y].Entity = entity.Item1;
            }
        }

        public bool IsAdjacent(Cell cell1, Cell cell2) =>  GetAdjacentToCell(cell1).Contains(cell2);
        public bool InBounds(Point point)
        {
            return ((point.Y < Size && point.Y >= 0) && (point.X < Size && point.X >= 0));
        }

        public IEnumerable<Cell> GetCells()
        {
            foreach (var cell in matrix)
            {
                yield return cell;
            }
        }

        public void EndTurn()
        {
            foreach (var ally in allyArray)
            {
                ally.UnSetChosen();
                ally.SetActive();
            }

            foreach (var foe in foeArray)
            {
                foe.SetActive();
                var closestPath = GetAdjacentToCell(foe.Cell)
                    .Select(x => FindClosestAlly(new HashSet<Cell>() { }, x, x, EntityType.Ally, 1)).ToList();
                if (closestPath.Count() != 0 && closestPath.First().Item2 != Size + 1) 
                    Move(foe.Cell, closestPath.OrderBy(x => x.Item2).First().Item1);
                foe.UnsetActive();
            }
        }

        private (Cell, int) FindClosestAlly(HashSet<Cell> visited, Cell currentCell, Cell initialCell, EntityType seekFor, int pathLength)
        {
            if (currentCell.Entity != null && currentCell.Entity.Type == seekFor) 
                return (initialCell, pathLength);
            visited.Add(currentCell);
            
            var adjasentCellsFiltered =  GetAdjacentToCell(currentCell).Where(x => !visited.Contains(x));
            if (!adjasentCellsFiltered.Any()) return (initialCell, Size + 1);
            
            var paths = adjasentCellsFiltered.Select(x => 
                    FindClosestAlly(visited, x, initialCell, seekFor, pathLength + 1)).ToList();
            
            if (paths.Count() != 0) return paths.OrderBy(x => x.Item2).First();
            return (initialCell, Size + 1);
        }
    }
}