using System.Collections.Generic;

namespace WpfApplication3
{
    partial class path
    {
        private static List<pair>[] constPath;

        private int size;
        private pair[,] costPath;
        private bool[] used;
        private int[,] parent;
        private List<int>[] oneTime;
        private cell[,] Map;


        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public int Life { get; private set; }

        //declare variables and initializates some of them
        private void PrimaryInitialization()
        {
            used = new bool[size];
            oneTime = new List<int>[size];
            constPath = new List<pair>[size];
            costPath = new pair[size, size];
            parent = new int[size, size];

            for (int i = 0; i < size; i++)
                constPath[i] = new List<pair>();
        }

        //get number of cells where you can go for one time from thisCell
        public int GetOneCount(cell thisCell)
        {
            return oneTime[Number(thisCell.X, thisCell.Y)].Count;
        }

        //get the cell number index where you can go for one time from thisCell
        public cell GetOneCell(cell thisCell, int index)
        {
            cell b = new cell();
            b.X = oneTime[Number(thisCell.X, thisCell.Y)][index] % SizeX;
            b.Y = (oneTime[Number(thisCell.X, thisCell.Y)][index] - b.X) / SizeX;
            b.Price = costPath[Number(thisCell.X, thisCell.Y), oneTime[Number(thisCell.X, thisCell.Y)][index]].Key;
            return b;
        }

        //get the cost of the path from begin to end
        public int GetCostPath(cell begin, cell end)
        {
            return (costPath[Number(begin.X, begin.Y), Number(end.X, end.Y)].Key);
        }

        //get the List od cells - the path from begin to end
        public List<cell> GetParent(cell begin, cell end)
        {
            List<cell> parentMap = new List<cell>();
            parentMap.Add(end);
            while (end != begin)
            {
                int number = parent[Number(begin), Number(end)];
                end = Map[number % SizeX, (number - number % SizeX) / SizeX];
                parentMap.Add(end);
            }
            parentMap.Reverse();
            return (parentMap);
        }  
    }
}
