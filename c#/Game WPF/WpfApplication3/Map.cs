namespace WpfApplication3
{
    public class map
    {
        public map(int sizeX = 0, int sizeY = 0, cell[,] costMap = null)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            if (costMap == null || costMap.GetLength(0) != SizeX || costMap.GetLength(1) != SizeY)
                this.costMap = new cell[SizeX, SizeY];
            else
                this.costMap = (cell[,])costMap.Clone();
        }

        //map
        private cell[,] costMap;

        //size of the map
        public int SizeX { get; private set; }

        public int SizeY { get; private set; }

        //get methods
        public cell getCell(int x, int y)
        {
            return costMap[x, y];
        }

        public cell[,] getMap()
        {
            if (costMap != null)
                return (cell[,])costMap.Clone();
            return null;
        }

        //set methods
        public void setCell(int x, int y, int price)
        {
            costMap[x, y] = new cell(x, y, price);
        }

        public void setCell(cell a)
        {
            this.costMap[a.X, a.Y] = a;
        }
    }
}
