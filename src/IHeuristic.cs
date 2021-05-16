using System;
using System.Collections.Generic;
using System.Text;

namespace mankala_tests
{
    public interface IHeuristic
    {
        int Value(Board b, int player);
    }
}
