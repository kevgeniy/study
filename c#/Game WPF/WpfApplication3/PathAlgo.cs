using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    partial class path
    {
        public path(int life, cell[,] costMap) {
            SizeX = costMap.GetLength(0);
            SizeY = costMap.GetLength(1);
            Life = life;
            size = SizeX * SizeY;
            this.Map = (cell[,])costMap.Clone();
            InitCostPath();
        }

        private const int max = 1000000;

        //initializate all path for current map and life
        public void InitAllPath()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    used[j] = false;
                    costPath[i, j] = new pair(max, 0);
                }
                SetPath(i);
            }
        }

        //initializate path from cell begin for current map and life considering warriors
        public void InitPathWithWarriors(cell begin, ref cell[] warriors)
        {
            for (int i = 0; i < size; i++)
            {
                used[i] = false;
                costPath[Number(begin), i] = new pair(max, 0);
            }

            for (int j = 0; j < warriors.Length; j++)
                used[Number(warriors[j])] = true;

            SetPath(Number(begin));
        }

        //initializate cost of paths from every cell to its neighbors
        private void InitCostPath()
        {
            PrimaryInitialization();
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    if (i != 0)
                    {
                        constPath[Number(i, j)].Add(new pair(Number(i - 1, j), Map[i - 1, j].Price));

                        if (j != 0)
                            constPath[Number(i, j)].Add(new pair(Number(i - 1, j - 1), Map[i - 1, j - 1].Price));

                        if (j != SizeY - 1)
                            constPath[Number(i, j)].Add(new pair(Number(i - 1, j + 1), Map[i - 1, j + 1].Price));
                    }

                    if (i != SizeX - 1)
                    {
                        constPath[Number(i, j)].Add(new pair(Number(i + 1, j), Map[i + 1, j].Price));

                        if (j != 0)
                            constPath[Number(i, j)].Add(new pair(Number(i + 1, j - 1), Map[i + 1, j - 1].Price));

                        if (j != SizeY - 1)
                            constPath[Number(i, j)].Add(new pair(Number(i + 1, j + 1), Map[i + 1, j + 1].Price));
                    }

                    if (j != 0)
                        constPath[Number(i, j)].Add(new pair(Number(i, j - 1), Map[i, j - 1].Price));

                    if (j != SizeY - 1)
                        constPath[Number(i, j)].Add(new pair(Number(i, j + 1), Map[i, j + 1].Price));
                }
        }

        //set cost of paths from cell with specific number to each other
        private void SetPath(int number)
        {
            costPath[number, number] = new pair(0, 0);
            deikstra(number);
            oneTime[number] = new List<int>();

            for (int i = 0; i < size; i++)
                if (costPath[number, i].Value != 0)
                    costPath[number, i] = new pair(costPath[number, i].Key + 1, 0);

            for (int i = 0; i < size; i++)
                if (costPath[number, i].Key <= 1)
                    oneTime[number].Add(i);
        }

        //algorithm deikstra for SetPath method
        private void deikstra(int number)
        {
            int min;
            for (int i = 0; i < size; i++)
            {
                min = -1;
                for (int j = 0; j < size; j++)
                    if (!used[j] && (min == -1 || costPath[number, j] < costPath[number, min]))
                        min = j;
                if (min == -1 || costPath[number, min].Key == max)
                    break;

                used[min] = true;

                pair a;
                for (int j = 0; j < constPath[min].Count; j++)
                    if (constPath[min][j].Value <= Life)
                    {
                        a = pair.plus(costPath[number, min], constPath[min][j].Value, Life);
                        if (a < costPath[number, constPath[min][j].Key])
                        {
                            costPath[number, constPath[min][j].Key] = a;
                            parent[number, constPath[min][j].Key] = min;
                        }
                    }
            }
        }

        //convert x-y coordinates to unique number
        private int Number(int x, int y)
        {
            return (x + y * SizeX);
        }

        private int Number(cell a)
        {
            return (a.X + a.Y * SizeX);
        }

    }
}
