using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ArgumentException = System.ArgumentException;

namespace ProjectGame
{
    public class Map
    {
        private Cell[,] matrix;
        private  List<IPlayable> allyArray;
        private List<Skeleton> foeArray;
        public readonly int Size;
        private List<IEntity> entityList;
        private List<Cell> GravePositions;
        private Cell castlePosition;
        public bool isEndGame;
        public int TurnsToWin;
        
        public Map(int size, string img, Control.ControlCollection Controls)
        {
            isEndGame = false;
            TurnsToWin = Constants.TurnsToWin;
            allyArray = new List<IPlayable>();
            foeArray = new List<Skeleton>();
            matrix = new Cell[size, size];
            Size = size;
            var dy = 0;
            var dx = 0; 
            for (var j = 0; j < size; j++)
            {
                for (var i = 0; i < size; i++)
                {
                    var cell = new Cell(i * 130 + dx, dy, Image.FromFile(img), new Point(i, j));
                    matrix[i, j] = cell;
                    Controls.Add(cell.Box);
                }
                if (j % 2 == 0) dx += 65;
                else dx -= 65;
                dy += 100;
            }

            var buildings = new List<(IEntity, Point)>
            {
                (new Castle(), new Point(Size / 2, Size / 2))
            };
            castlePosition = matrix[Size / 2, Size / 2];
            GravePositions = new List<Cell>();

            var helpingInt = 0;
            foreach (var gravePoint in new Point[]
                         {new Point(0, 0), new Point(0, Size-1), new Point(Size-1, 0), new Point(Size-1, Size-1)})
            {
                buildings.Add((new Grave(helpingInt * Constants.TurnsToWin/4),gravePoint));
                helpingInt += 1;
                GravePositions.Add(matrix[gravePoint.X, gravePoint.Y]);
            }
            SpawnEntity(buildings.ToArray());
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
        private IEnumerable<Cell> GetAdjacentToCell(Cell cell)
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

        private void DealDamage(Cell Dealer, Cell Reciwer)
        {
            if (Dealer.Entity == null || Reciwer.Entity == null) throw new ArgumentException();
            if (Dealer.Entity.Type == Reciwer.Entity.Type) return;
            if (Dealer.Entity.Type == EntityType.Ally && Dealer.Entity.Type == EntityType.Castle) return;
            if (Reciwer.Entity.Type == EntityType.Grave) return;
            if (Dealer.Entity is IPlayable && Reciwer.Entity is IPlayable) return;
            
            var dealerAttack = Dealer.Entity.Attack + GetAdditionDealerAttack(Dealer);
            if (Dealer.Entity is IPlayable)
            {
                var pleable = (IPlayable)Dealer.Entity;
                pleable.DoBigAction();
            }
            if (Reciwer.Entity.HealthPoints - dealerAttack <= 0)
            {
                if (Dealer.Entity is IPlayable)
                {
                    var pleable = (IPlayable)Dealer.Entity;
                    pleable.RiseKillCount();
                }
                Reciwer.Entity = null;
            }
            else
            {
                Reciwer.Entity.HealthPoints -= dealerAttack;
                Dealer.Entity.HealthPoints -= Reciwer.Entity.Attack;
                if (Dealer.Entity.HealthPoints <= 0)
                {
                    if (Reciwer.Entity is IPlayable)
                    {
                        var pleable = (IPlayable)Reciwer.Entity;
                        pleable.RiseKillCount();
                    }
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
                if (From.Entity is IPlayable)
                {
                    var pleable = (IPlayable)From.Entity;
                    pleable.DoSmallAction();
                }

                if (From.Entity.Type == EntityType.Foe)
                {
                    var skeleton = (Skeleton)From.Entity;
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
                if (entity.Item1.Type == EntityType.Ally) allyArray.Add((IPlayable)entity.Item1);
                if (!InBounds(entity.Item2)) throw new ArgumentException();
                if (entity.Item1.Type == EntityType.Foe)
                {
                    var skelet = (Skeleton)entity.Item1;
                    skelet.Cell = matrix[entity.Item2.X, entity.Item2.Y];
                    foeArray.Add(skelet);
                }
                matrix[entity.Item2.X, entity.Item2.Y].Entity = entity.Item1;
            }
        }

        private bool IsAdjacent(Cell cell1, Cell cell2) =>  GetAdjacentToCell(cell1).Contains(cell2);
        private bool InBounds(Point point)
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
            if(isEndGame) return;
            foreach (var ally in allyArray)
            {
                ally.UnSetChosen();
                ally.TryHeal();
                ally.SetActive();
            }

            foreach (var foe in foeArray)
            {
                foe.SetActive();
                var closestPathToAlly = GetAdjacentToCell(foe.Cell)
                    .Select(x => FindClosestAlly(x, EntityType.Ally))
                    .Where(x => x.Item2 != -1)
                    .ToList();
                var closestPathToCastle = GetAdjacentToCell(foe.Cell)
                    .Select(x => FindClosestAlly(x, EntityType.Castle))
                    .Where(x => x.Item2 != -1)
                    .ToList();

                if (closestPathToAlly.Count() != 0)
                {
                    closestPathToAlly = closestPathToAlly.OrderBy(x => x.Item2).ToList();
                    if(closestPathToAlly.First().Item2 < 3) Move(foe.Cell, closestPathToAlly.First().Item1);
                    else if(closestPathToCastle.Count() != 0) Move(foe.Cell, closestPathToCastle.OrderBy(x => x.Item2).First().Item1);
                }
                foe.UnsetActive();

                if (castlePosition.Entity == null)
                {
                    EndGame();
                    return;
                }
            }
            
            if (((Castle)castlePosition.Entity).TrySpawn())SpawnReinforcement(
                GetRandomAlly(), castlePosition);
            SpawnFoe();

            TurnsToWin -= 1;
            if (TurnsToWin == 0)
            {
                EndGame();
            }
        }

        private (Cell, int) FindClosestAlly(Cell initialCell, EntityType seekingFor)
        {
            var visited = new HashSet<Cell>();
            var queue = new Queue<(Cell, int)>();
            queue.Enqueue((initialCell, 1));
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                if (visited.Contains(node.Item1)) continue;
                if (node.Item1.Entity != null && node.Item1.Entity.Type == seekingFor) return (initialCell, node.Item2);
                visited.Add(node.Item1);
                foreach (var incidentCell in GetAdjacentToCell(node.Item1))
                    queue.Enqueue((incidentCell, node.Item2 + 1));
            }
            return(initialCell, -1);
        }

        private void SpawnReinforcement(IEntity entity, Cell spawnFrom)
        {
            var random = new Random();
            var cellList = GetAdjacentToCell(spawnFrom).OrderBy(e => random.NextDouble());
            foreach (var spawnPositions in cellList)
            {
                if(spawnPositions.Entity == null) 
                {
                    SpawnEntity(new []{(entity,
                    new Point(spawnPositions.MapPosition.X, spawnPositions.MapPosition.Y))});
                    return;
                }
            }
        }

        private void SpawnFoe()
        {
            foreach (var gravePosition in GravePositions)
            {
                var grave = (Grave)gravePosition.Entity;
                if(grave.CanSpawn()) SpawnReinforcement(new Skeleton(), gravePosition);
            }
        }

        private void EndGame()
        {
            isEndGame = true;
            foreach (var cell in matrix)
            {
                cell.Box.Visible = false;
            }
        }

        private int GetAdditionDealerAttack(Cell cell)
        {
            if (cell.Entity is Knight) return GetAdditionalKnightAttack(cell);
            if (cell.Entity is Rider) return GetAdditionalRiderAttack(cell);
            return 0;
        }

        private int GetAdditionalKnightAttack(Cell cell)
        {
            var knight = (Knight)cell.Entity;
            return GetAdjacentToCell(cell)
                .Where(x => (x.Entity != null && x.Entity.Type == EntityType.Ally))
                .Count() * knight.AddAttack;
        }
        
        private int GetAdditionalRiderAttack(Cell cell)
        {
            var rider = (Rider)cell.Entity;
            return rider.GetAdditionalAttack;
        }

        public bool IsWinning() => (castlePosition.Entity != null);

        private IPlayable GetRandomAlly()
        {
            var rnd = new Random();
            var allyList = new IPlayable[] {new Knight(), new Rider() };
            return allyList.OrderBy(x => rnd.Next()).First();
        }
    }
}