using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc
{    public static class Util
    {
        public static void AddOrSum<T>(this Dictionary<T, long> dict, T key, long value)
        {
            if (dict.ContainsKey(key)) dict[key] += value;
            else dict.Add(key, value);
        }

        public static int XLen(this int[,] grid)
        {
            return grid.GetLength(0);
        }

        public static int YLen(this int[,] grid)
        {
            return grid.GetLength(1);
        }

        public static int XMax(this int[,] grid)
        {
            return grid.XLen() - 1;
        }

        public static int YMax(this int[,] grid)
        {
            return grid.YLen() - 1;
        }

        public static string HexToBinary(this char val)
        {
            string hexabet = "0123456789ABCDEF";
            int lastD = hexabet.IndexOf(val);
            return Convert.ToString(lastD, 2).PadLeft(4, '0');
        }

        public static long ToDecimal(this string val)
        {
            return Convert.ToInt64(val, 2);
        }
    }
}
