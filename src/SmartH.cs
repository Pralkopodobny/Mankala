using System;
using System.Collections.Generic;
using System.Text;

namespace mankala_tests
{
    public class SmartH : IHeuristic
    {
        private static readonly int[] w = new int[] {80, 100, 6, 0, 87, 60, 0, 20, 73, 93, 0, 80 };
        public int Value(Board b, int player)
        {
            int enemy = (player + 1) % 2;
            int[] a = new int[12];
            a[0] = a1234(b, enemy, 2);
            a[1] = a1234(b, enemy, 3);
            a[2] = a1234(b, player, 2);
            a[3] = a1234(b, player, 3);
            a[4] = a56(b, enemy);
            a[5] = a56(b, player);
            a[6] = a78(b, enemy);
            a[7] = a78(b, player);
            a[8] = a910(b, enemy);
            a[9] = a910(b, player);
            a[10] = a1112(b, enemy);
            a[11] = a1112(b, player);

            int result = 0;
            for(int i = 0; i < 12; i++)
            {
                result += a[i] * w[i];
            }
            return result;

        }

        private int a1234(Board b, int who, int thresh)
        {
            int result = 0;
            for(int i = 0; i < 6; i++)
            {
                if (b.stones[i,who] > 0 && b.CaptureNumb(i, who) >= thresh) result++;
            }
            return result;
        }

        private int a56(Board b, int who)
        {
            int result = 0;
            for (int i = 0; i < 6; i++)
            {
                if (b.stones[i,who] > 6 - i) result++;
            }
            return result;
        }

        private int a78(Board b, int who)
        {
            int result = 0;
            for (int i = 0; i < 6; i++)
            {
                if (b.stones[i, who] > 12) result++;
            }
            return result;
        }

        private int a910(Board b, int who)
        {
            return b.stones[6, who];
        }
        private int a1112(Board b, int who)
        {
            int result = 0;
            for (int i = 0; i < 6; i++)
            {
                if (b.stones[i, who] == 0) result++;
            }
            return result;
        }
    }
}
