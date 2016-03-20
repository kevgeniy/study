using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication3
{
    public partial class game
    {
        private static map gameMap;
        private static chip player1;
        private static chip[] player2;
        private static cell end;

        private static bool[] Player2Initiated;

        public static int MapSizeX { get; private set; }
        public static int MapSizeY { get; private set; }
        public static cell End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
                IsEndInitiated = true;
            }
        }

        public static bool IsMapInitiated { get; private set;}
        public static bool IsPlayer1Initiated { get; private set; }
        public static bool IsPlayer2Initiated
        {
            get
            {
                if (player2 == null)
                    return false;
                for (int i = 0; i < player2.Length; i++)
                    if (!Player2Initiated[i])
                        return false;
                return true;
            }
            private set{}
        }
        public static bool IsEndInitiated { get; private set; }

        //initializates
        public static void InitGameMap(int sizeX, int sizeY)
        {
            IsMapInitiated = true;
            MapSizeX = sizeX;
            MapSizeY = sizeY;
            gameMap = new map(sizeX, sizeY);
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    gameMap.setCell(new cell(i, j, 1));
        }

        public static void InitPlayer1(int beginX, int beginY, int life)
        {
            IsPlayer1Initiated = true;
            player1 = new chip(beginX, beginY, life);
        }

        public static void InitPlayer2Count(int count)
        {
            Player2Initiated = new bool[count];
            player2 = new chip[count];
            for (int i = 0; i < count; i++)
                player2[i] = new chip();
        }

        public static void InitPlayer2(int number, int x, int y, int life)
        {
            Player2Initiated[number - 1] = true;
            player2[number - 1] = new chip(x, y, life);
        }

        public static void InitMapCell(int x, int y, int price)
        {
            gameMap.setCell(new cell (x, y, price));
        }

        //get
        public static cell GetMapCell(int x, int y)
        {
            return gameMap.getCell(x, y);
        }

        public static cell GetPlayer1CurrentState()
        {
            return player1.CurrentState;
        }

        public static int GetPlayer2Count ()
        {
            return player2.Length;
        }

        public static cell GetPlayer2CurrentState(int number)
        {
            return player2[number - 1].CurrentState;
        }

        public static void Clear()
        {
            IsEndInitiated = false;
            IsMapInitiated = false;
            gameMap = null;
            End = new cell(0, 0);
        }
    }
}
