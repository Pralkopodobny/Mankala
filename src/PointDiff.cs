using System;
using System.Collections.Generic;
using System.Text;

namespace mankala_tests
{
    public class PointDiff : IHeuristic
    {
        public int Value(Board b, int player)
        {
            int diff = b.stones[6, player] - b.stones[6, (player + 1) % 2];
            if (b.End)
            {
                if (diff < 0) return int.MinValue + 1;
                if (diff == 0) return 0;
                else return int.MaxValue - 1;
            }
            return b.stones[6, player] - b.stones[6, (player + 1) % 2];
        }
    }
}
