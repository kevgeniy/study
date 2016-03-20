using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication3
{
    partial class game
    {
        private bool player1Flag;

        private int depth;

        public void Start()
        {
            player1.InitPathMap(gameMap.getMap());
            for (int i = 0; i < player2.Length; i++)
            {
                player2[i].InitPathMap(gameMap.getMap());
                player2[i].initAllPath();
            }
        }

        //do one step
        public int Step()
        {
            depth = 0;
            tetra a;

            player1Flag = !player1Flag;
            if (player1Flag)
            {
                if (IsGameEndOnPlayer1Step() != 0)
                    return IsGameEndOnPlayer1Step();
                a = TheBestCell(player1, player2);
                player1.CurrentState = a.Cell;
                return IsGameEndOnPlayer1Step();
            }
            else
            {
                if (IsGameEndOnPlayer2Step() != 0)
                    return IsGameEndOnPlayer2Step();
                a = TheWorstCell(player1, player2);
                player2[a.Index].CurrentState = a.Cell;
                return IsGameEndOnPlayer2Step();
            }
        }

        private int IsGameEndOnPlayer1Step()
        {
            if (player1.CurrentState == End)
                return 1;
            return IsGameEndOnPlayer2Step();
        }

        private int IsGameEndOnPlayer2Step()
        {
            for (int i = 0; i < player2.Length; i++)
                if (player2[i].CurrentState == player1.CurrentState || player2[i].CurrentState == End)
                    return 2;
            return 0;

        }
    }
}
