using System;
using System.Collections.Generic;
using System.Text;

namespace mankala_tests
{
    public class Board
    {
        public static int SIZE = 7;
        public int[,] stones = new int[7, 2];
        public bool End { private set; get; } = false;
        public Board()
        {
            stones = new int[SIZE, 2];
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    stones[i, j] = 4;
                }
            }
            stones[SIZE - 1, 0] = 0;
            stones[SIZE - 1, 1] = 0;

        }
        public Board(Board other)
        {
            stones = new int[7, 2];
            Array.Copy(other.stones, stones, other.stones.Length);
        }
        private void NextPos(int n, int r, out int newN, out int newR)
        {
            n++;
            newN = n % SIZE;
            newR = n % SIZE == 0 ? (r + 1) % 2 : r;
        }
        public int CaptureNumb(int n, int r)
        {
            if (stones[n, r] == 0 || n == 6 || End)
            {
                return 0;
            }
            int s = stones[n, r];
            int newN = n, newR = r;
            for (int i = 0; i < s; i++)
            {
                NextPos(newN, newR, out newN, out newR);
            }
            if (stones[newN, newR] == 0 && newR == r && newN != 6 && stones[SIZE - 2 - newN, (newR + 1) % 2] != 0)
                return stones[SIZE - 2 - newN, (r + 1) % 2] + 1;
            else
                return 0;
        }

        public bool MakeMove(int n, int r)
        {
            if (stones[n, r] == 0 || n == 6 || End)
            {
                return false;
            }

            int s = stones[n, r];
            int newN = n, newR = r;
            stones[n, r] = 0;
            for (int i = 0; i < s; i++)
            {
                NextPos(newN, newR, out newN, out newR);
                stones[newN, newR]++;
            }

            //dont end turn if your last stone gets in the backet
            if (newN == 6 && newR == r)
            {
                End = CheckGameEnded(r);
                if (End) GameEndProcedure(r);
                return End;
            }

            //take all stones from oposite hole if your last stone gets to YOUR empty hole
            if (stones[newN, newR] == 1 && newR == r && stones[SIZE - 2 - newN, (newR + 1) % 2] != 0)
            {

                stones[6, r] += stones[SIZE - 2 - newN, (r + 1) % 2] + 1;
                stones[SIZE - 2 - newN, (r + 1) % 2] = 0;
                stones[newN, newR] = 0;
            }

            ActualizeEnd(r);
            if (End) GameEndProcedure(r);
            return true;
        }
        private bool CheckGameEnded(int player)
        {
            for (int i = 0; i < SIZE - 1; i++)
            {
                if (stones[i, player] != 0) return false;
            }
            return true;
        }
        public void ActualizeEnd(int player)
        {
            End = CheckGameEnded(player);
            if (End) GameEndProcedure(player);
        }
        private void GameEndProcedure(int player)
        {
            player = (player + 1) % 2;
            for (int i = 0; i < SIZE - 1; i++)
            {
                stones[SIZE - 1, player] += stones[i, player];
                stones[i, player] = 0;
            }
        }
        public int StaticScore(int player)
        {
            return stones[6, player] - stones[6, (player + 1) % 2];
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            for (int i = SIZE - 1; i >= 0; i--)
            {
                b.Append(stones[i, 1].ToString() + " ");
            }
            b.Append("DOWN -> ");
            for (int i = 0; i < SIZE; i++)
            {
                b.Append(stones[i, 0].ToString() + " ");
            }
            return b.ToString();
        }
    }
}
