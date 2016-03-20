using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    partial class  game
    {
        private struct tetra
        {
            public tetra(cell thisCell, int rating, int depth, bool good, bool bad, int index = 0)
                : this()
            {
                Cell = thisCell;
                Rating = rating;
                Good = good;
                Bad = bad;
                Index = index;
                Depth = depth;
            }

            public cell Cell { get; set; }
            public int Rating { get; set; }
            public int Depth { get; set; }
            public bool Good { get; set; }
            public bool Bad { get; set; }
            public int Index { get; set; }
        }

        private const int maxDepth = 1;

        private bool bad = false, good = false;

        private tetra TheBestCell(chip player1, chip[] player2)
        {
            player1.InitPathWithWarriors(player1.CurrentState, player2);
            if (player1.GetCostPath(player1.CurrentState, End) == 1)
            {
                tetra ans = new tetra(End, Rating(player1.CurrentState, player1, player2), depth + 1, true, false);
                bad = false;
                good = false;
                return ans;
            }
            if (depth < maxDepth)
            {
                depth++;
                tetra Max = new tetra();
                for (int i = 0; i < player1.GetOneTimeCellCount(player1.CurrentState); i++)
                {
                    cell a = player1.CurrentState;
                    player1.CurrentState = player1.GetOneTimeCell(a, i);
                    tetra b = TheWorstCell(player1, player2);
                    player1.CurrentState = a;
                    player1.InitPathWithWarriors(player1.CurrentState, player2);
                    if (b.Good)
                    {
                        if (!Max.Good || Max.Depth > b.Depth || Max.Depth == b.Depth && Max.Rating < b.Rating)
                        {
                            Max = b;
                            Max.Cell = player1.GetOneTimeCell(a, i);
                        }
                    }
                    else
                        if (b.Bad)
                        {
                            if (i == 0 || Max.Bad && (Max.Depth < b.Depth || Max.Depth == b.Depth && Max.Rating < b.Rating))
                            {
                                Max = b;
                                Max.Cell = player1.GetOneTimeCell(a, i);
                            }
                        }
                        else
                            if (i == 0 || !Max.Good && (Max.Bad || b.Rating > Max.Rating))
                            {
                                Max = b;
                                Max.Cell = player1.GetOneTimeCell(a, i);
                            }
                }
                depth--;
                return Max;
            }
            else
            {
                tetra Max = new tetra();
                for (int i = 0; i < player1.GetOneTimeCellCount(player1.CurrentState); i++)
                {
                    cell thisCell = player1.GetOneTimeCell(player1.CurrentState, i);
                    player1.InitPathWithWarriors(thisCell, player2);
                    int thisRating = Rating(thisCell, player1, player2);
                    if (good)
                    {
                        if (!Max.Good || Max.Rating < thisRating)
                        {
                            good = false;
                            Max.Good = true;
                            Max.Bad = false;
                            Max.Cell = thisCell;
                            Max.Rating = thisRating;
                            Max.Depth = depth + 2;
                        }
                    }
                    else
                        if (bad)
                        {
                            if (i == 0 || Max.Bad && Max.Rating < thisRating)
                            {
                                bad = false;
                                Max.Good = false;
                                Max.Bad = true;
                                Max.Cell = thisCell;
                                Max.Rating = thisRating;
                                Max.Depth = depth + 1;
                            }
                        }
                        else
                            if (i == 0 || !Max.Good && (Max.Bad || Max.Rating < thisRating))
                            {
                                Max.Bad = false;
                                Max.Rating = thisRating;
                                Max.Cell = thisCell;
                                Max.Depth = depth + 1;
                            }
                }
                return Max;
            }
        }

        private tetra TheWorstCell(chip player1, chip[] player2)
        {
            tetra Min = new tetra();
            for (int i = 0; i < player2.Length; i++)
            {
                if (player2[i].GetCostPath(player2[i].CurrentState, player1.CurrentState) <= 1)
                {
                    tetra ans = new tetra(player1.CurrentState, Rating(player1.CurrentState, player1, player2), depth, false, true, i);
                    bad = false;
                    good = false;
                    return ans;
                }
                if (player2[i].GetCostPath(player2[i].CurrentState, End) == 1)
                {
                    tetra ans = new tetra(End, Rating(player1.CurrentState, player1, player2), depth, true, false, i);
                    bad = false;
                    good = false;
                    return ans;
                }
                for (int j = 0; j < player2[i].GetOneTimeCellCount(player2[i].CurrentState); j++)
                {
                    cell a = player2[i].CurrentState;
                    player2[i].CurrentState = player2[i].GetOneTimeCell(a, j);
                    tetra b = TheBestCell(player1, player2);
                    player2[i].CurrentState = a;
                    if (b.Bad)
                    {
                        if (!Min.Bad || Min.Depth > b.Rating || Min.Depth == b.Depth && Min.Rating > b.Rating)
                        {
                            Min = b;
                            Min.Index = i;
                            Min.Cell = player2[i].GetOneTimeCell(a, j);
                        }
                    }
                    else
                        if (b.Good)
                        {
                            if (j == 0 || Min.Good && (Min.Depth < b.Depth || Min.Depth == b.Depth && Min.Rating > b.Rating))
                            {
                                Min = b;
                                Min.Index = i;
                                Min.Cell = player2[i].GetOneTimeCell(a, j);
                            }
                        }
                        else
                            if (j == 0 || !Min.Bad && (Min.Good || b.Rating < Min.Rating))
                            {
                                Min = b;
                                Min.Index = i;
                                Min.Cell = player2[i].GetOneTimeCell(a, j);
                            }
                }
            }
            return Min;
        }

        private int Rating(cell thisCell, chip player1, chip[] player2)
        {
            int ans = -player1.GetCostPath(thisCell, End);
            for (int i = 0; i < player2.Length; i++)
            {
                if (player2[i].GetCostPath(player2[i].CurrentState, thisCell) == 1 ||
                    player2[i].GetCostPath(player2[i].CurrentState, End) == 1)
                    bad = true;
                ans += player2[i].GetCostPath(player2[i].CurrentState, thisCell);
                ans += player2[i].GetCostPath(player2[i].CurrentState, End);
            }
            if (!bad && -ans <= 1)
                good = true;
            return ans;
        }
    }
}
