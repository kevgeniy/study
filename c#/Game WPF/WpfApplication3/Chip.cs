using System.Collections.Generic;
namespace WpfApplication3
{
    public class chip
    {
        public chip(int beginX = 0, int beginY = 0, int life = 1, cell[,] costMap = null)
        {
            BeginX = beginX;
            BeginY = beginY;
            Life = life;
            CurrentState = new cell(BeginX, BeginY, Life);
        }

        public int BeginX { get; private set; }
        public int BeginY { get; private set; }
        public int Life { get; private set; }

        public cell CurrentState { get; set; }

        //map of path for current chip
        private path thisPath;

        //all methods down are delegated from thisPath

        public void InitPathMap(cell[,] costMap)
        {
            thisPath = new path(Life, costMap);
        }

        public void initAllPath()
        {
            thisPath.InitAllPath();
        }

        public void InitPathWithWarriors(cell begin, chip[] warriorPlayer)
        {
            int number = warriorPlayer.Length;
            cell[] player2Chips = new cell[number];
            for (int i = 0; i < number; i++)
                player2Chips[i] = warriorPlayer[i].CurrentState;
            thisPath.InitPathWithWarriors(begin, ref player2Chips);
        }

        public int GetCostPath(cell begin, cell end)
        {
            return thisPath.GetCostPath(begin, end);
        }

        public cell GetOneTimeCell(cell a, int index)
        {
            return (thisPath.GetOneCell(a, index));
        }

        public int GetOneTimeCellCount(cell a)
        {
            return thisPath.GetOneCount(a);
        }

        public List<cell> GetParent(cell a, cell b)
        {
            return thisPath.GetParent(a, b);
        }
    }
}
