using System;
using System.Collections.Generic;
using System.Text;

namespace mankala_tests
{
    public class GameMaster
    {
        private AI player0, player1;
        public Board Board { private set; get; } = new Board();
        private Random rng;
        private int turn = 0;
        public GameMaster(AI player0, AI player1, Random rng)
        {
            this.player0 = player0;
            this.player1 = player1;
            this.rng = rng;
        }

        public (int, double, int) PlayGame()
        {
            int firstRandomMove = rng.Next(0, 6);
            SingleMove(firstRandomMove);
            bool gameInProgress = true;
            while (gameInProgress)
            {
                switch (turn)
                {
                    case 0:
                        SingleMove(player0.MakeMove(Board));
                        gameInProgress = !Board.End;
                        break;
                    case 1:
                        SingleMove(player1.MakeMove(Board));
                        gameInProgress = !Board.End;
                        break;
                }
            }
            switch (Winner())
            {
                case 0:
                    return (0, player0.Stopwatch.Elapsed.TotalSeconds, player0.Moves);
                case 1:
                    return (1, player1.Stopwatch.Elapsed.TotalSeconds, player1.Moves);
                default:
                    return (-1, player0.Stopwatch.Elapsed.TotalSeconds, player0.Moves);
            }
        }
        private bool SingleMove(int move)
        {
            var finishedTurn = Board.MakeMove(move, turn);
            if (finishedTurn)
            {
                turn = (turn + 1) % 2;
                Board.ActualizeEnd(turn);
            }
            return finishedTurn;
        }

        private int Winner()
        {
            int pointDiff = Board.stones[6, 0] - Board.stones[6, 1];
            if (pointDiff == 0) return -1;
            else if (pointDiff < 0) return 1;
            else return 0;
        }


    }
}
