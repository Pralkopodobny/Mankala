using System;
using System.Collections.Generic;
using System.Text;

namespace mankala_tests
{
    public class PointsH : IHeuristic
    {
        public int Value(Board b, int player)
        {
            return b.stones[6, player];
        }
    }
}
