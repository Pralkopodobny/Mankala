using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace mankala_tests
{
    public abstract class AI
    {
        public int Time { get; protected set; } = 0;
        public int Moves { get; protected set; } = 0;
        public int Id { get; protected set; }
        public int Depth { protected set; get; }
        protected MinMax algorithm;
        public Stopwatch Stopwatch { protected set; get; }

        public AI(IHeuristic heuristic, int player, int depth = 5)
        {
            algorithm = new MinMax(heuristic);
            Id = player;
            Stopwatch = new Stopwatch();
            Depth = depth;
        }
        public abstract int MakeMove(Board b);

    }

    public class AlfaBetaAI : AI
    {
        public AlfaBetaAI(IHeuristic heuristic, int player, int depth = 5) : base(heuristic, player, depth) { }
        public override int MakeMove(Board b)
        {
            Stopwatch.Start();
            var move = algorithm.AlfaBetaSolve(b, Id, Depth);
            Stopwatch.Stop();
            Moves++;
            return move;
        }
    }

    public class MinMaxAI : AI
    {
        public MinMaxAI(IHeuristic heuristic, int player, int depth = 5) : base(heuristic, player, depth) { }
        public override int MakeMove(Board b)
        {
            Stopwatch.Start();
            var move = algorithm.MinMaxSolve(b, Id, Depth);
            Stopwatch.Stop();
            Moves++;
            return move;
        }
    }
}
