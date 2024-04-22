namespace ProjectGame
{// Класс для клетки, из которой будет состоять карта.
    public class Cell
    {
        public string ImageName;
        public Entity Entity;
        public int X;
        public int Y;

        public Cell(int x, int y, string img)
        {
            X = x;
            Y = y;
            ImageName = img;
        }
    }
}