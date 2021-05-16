using System;
using System.Linq;

namespace mankala_tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rng = new Random();
            int seed = rng.Next();
            //FirstTests(seed);
            RunWinrateTests(seed);
        }
        static (double, double) AlfaBetaTests(int n, Random rng, int depth = 5)
        {
            int[] winners = new int[n];
            double[] times = new double[n];
            int[] moves = new int[n];
            for (int i = 0; i < n; i++)
            {
                AI player0 = new AlfaBetaAI(new PointDiff(), 0, depth);
                AI player1 = new AlfaBetaAI(new PointDiff(), 1, depth);
                GameMaster gm = new GameMaster(player0, player1, rng);
                (var winner, var time, var count) = gm.PlayGame();
                winners[i] = winner;
                times[i] = time;
                moves[i] = count;
            }
            var timeAvg = Enumerable.Average(times);
            var movesAvg = Enumerable.Average(moves);
            return (timeAvg, movesAvg);
        }
        static (double, double) MinMaxTests(int n, Random rng, int depth = 5)
        {
            int[] winners = new int[n];
            double[] times = new double[n];
            int[] moves = new int[n];
            for (int i = 0; i < n; i++)
            {
                AI player0 = new MinMaxAI(new PointDiff(), 0, depth);
                AI player1 = new MinMaxAI(new PointDiff(), 1, depth);
                GameMaster gm = new GameMaster(player0, player1, rng);
                (var winner, var time, var count) = gm.PlayGame();
                winners[i] = winner;
                times[i] = time;
                moves[i] = count;
            }
            var timeAvg = Enumerable.Average(times);
            var movesAvg = Enumerable.Average(moves);
            return (timeAvg, movesAvg);
        }
        static void FirstTests(int seed)
        {
            Random rng1 = new Random(seed), rng2 = new Random(seed);
            for(int i = 3; i < 7; i++)
            {
                FirstTestsSub(rng1, rng2, 7, i);
                FirstTestsSub(rng1, rng2, 20, i);
                FirstTestsSub(rng1, rng2, 50, i);
            }
        }
        private static void FirstTestsSub(Random rng1, Random rng2, int testCount, int depth)
        {
            Console.WriteLine($"Testy dla powt {testCount} depth {depth}:");
            Console.WriteLine(AlfaBetaTests(testCount, rng1, depth));
            Console.WriteLine(MinMaxTests(testCount, rng2, depth));
        }

        static (double, double, double) WinrateTests(IHeuristic h1, IHeuristic h2, int depth1, int depth2, Random rng, int n)
        {
            int[] winners = new int[n];
            double[] times = new double[n];
            int[] moves = new int[n];
            for (int i = 0; i < n; i++)
            {
                AI player0 = new AlfaBetaAI(h1, 0, depth1);
                AI player1 = new AlfaBetaAI(h2, 1, depth2);
                GameMaster gm = new GameMaster(player0, player1, rng);
                (var winner, var time, var count) = gm.PlayGame();
                winners[i] = winner;
                times[i] = time;
                moves[i] = count;
            }
            var timeAvg = Enumerable.Average(times);
            var movesAvg = Enumerable.Average(moves);
            var winAvg = Enumerable.Average(winners);
            return (winAvg, timeAvg, movesAvg);
        }

        static void RunWinrateTests(int seed, int testCount = 100)
        {
            Random rng = new Random(seed);

            Console.WriteLine("Diff5 vs Diff4" + WinrateTests(new PointDiff(), new PointDiff(), 5, 4, rng, testCount));
            Console.WriteLine("Diff4 vs Diff5" + WinrateTests(new PointDiff(), new PointDiff(), 4, 5, rng, testCount));
            Console.WriteLine("Diff5 vs Diff3" + WinrateTests(new PointDiff(), new PointDiff(), 5, 4, rng, testCount));
            Console.WriteLine("Diff3 vs Diff5" + WinrateTests(new PointDiff(), new PointDiff(), 3, 5, rng, testCount));
            Console.WriteLine("Diff4 vs Diff3)" + WinrateTests(new PointDiff(), new PointDiff(), 4, 3, rng, testCount));
            Console.WriteLine("Diff3 vs Diff4)" + WinrateTests(new PointDiff(), new PointDiff(), 3, 4, rng, testCount));

            Console.WriteLine("Points5 vs Points4" + WinrateTests(new PointsH(), new PointsH(), 5, 4, rng, testCount));
            Console.WriteLine("Points4 vs Points5" + WinrateTests(new PointsH(), new PointsH(), 4, 5, rng, testCount));
            Console.WriteLine("Points5 vs Points3" + WinrateTests(new PointsH(), new PointsH(), 5, 4, rng, testCount));
            Console.WriteLine("Points3 vs Points5" + WinrateTests(new PointsH(), new PointsH(), 3, 5, rng, testCount));
            Console.WriteLine("Points4 vs Points3)" + WinrateTests(new PointsH(), new PointsH(), 4, 3, rng, testCount));
            Console.WriteLine("Points3 vs Points4)" + WinrateTests(new PointsH(), new PointsH(), 3, 4, rng, testCount));

            Console.WriteLine("Smart5 vs Smart4" + WinrateTests(new SmartH(), new SmartH(), 5, 4, rng, 30));
            Console.WriteLine("Smart4 vs Smart5" + WinrateTests(new SmartH(), new SmartH(), 4, 5, rng, 30));
            Console.WriteLine("Smart5 vs Smart3" + WinrateTests(new SmartH(), new SmartH(), 5, 4, rng, 30));
            Console.WriteLine("Points3 vs Smart5" + WinrateTests(new SmartH(), new SmartH(), 3, 5, rng, 30));
            Console.WriteLine("Smart4 vs Smart3" + WinrateTests(new SmartH(), new SmartH(), 4, 3, rng, 30));
            Console.WriteLine("Smart3 vs Smart4" + WinrateTests(new SmartH(), new SmartH(), 3, 4, rng, 30));

            Console.WriteLine("Diff5 vs Point5" + WinrateTests(new PointDiff(), new PointsH(), 5, 5, rng, testCount));
            Console.WriteLine("Point5 vs Diff5" + WinrateTests(new PointDiff(), new PointsH(), 5, 5, rng, testCount));
            Console.WriteLine("Smart3 vs Diff5" + WinrateTests(new SmartH(), new PointDiff(), 3, 5, rng, testCount));
            Console.WriteLine("Smart3 vs Point5" + WinrateTests(new SmartH(), new PointsH(), 3, 5, rng, testCount));
            Console.WriteLine("Diff5 vs Smart3" + WinrateTests(new PointDiff(), new SmartH(), 5, 3, rng, testCount));
            Console.WriteLine("Point5 vs Smart3" + WinrateTests(new PointsH(), new SmartH(), 5, 3, rng, testCount));
        }

    }
}
