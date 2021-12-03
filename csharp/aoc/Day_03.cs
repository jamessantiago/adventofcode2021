using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_03 : BaseDay
    {
        private readonly List<char[]> _input;

        public Day_03()
        {
            _input = File.ReadLines(InputFilePath).Select(d => d.ToCharArray()).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var len = _input.First().Length;
            int hlen = _input.Count / 2;
            StringBuilder gamma = new StringBuilder();
            StringBuilder eps = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                var mc = _input.Count(d => d[i] == '0');
                if (mc >= hlen)
                {
                    gamma.Append('0');
                    eps.Append('1');
                }
                else
                {
                    gamma.Append('1');
                    eps.Append('0');
                }
            }
            var consumption = Convert.ToInt32(gamma.ToString(), 2) * Convert.ToInt32(eps.ToString(), 2);
            return new(consumption.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var len = _input.First().Length;
            int hlen = _input.Count / 2;
            var oxyvals = _input;
            var c02vals = _input;
            for (int i = 0; i < len; i++)
            {
                var c0 = oxyvals.Count(d => d[i] == '0');
                var c1 = oxyvals.Count(d => d[i] == '1');
                var bit = c1 == c0 ? '1' : c1 > c0 ? '1' : '0';
                oxyvals = oxyvals.Where(d => d[i] == bit).ToList();
                if (oxyvals.Count() == 1) break;
            }

            for (int i = 0; i < len; i++)
            {
                var c0 = c02vals.Count(d => d[i] == '0');
                var c1 = c02vals.Count(d => d[i] == '1');
                var bit = c1 == c0 ? '0' : c1 < c0 ? '1' : '0';
                c02vals = c02vals.Where(d => d[i] == bit).ToList();
                if (c02vals.Count() == 1) break;
            }

            var consumption = Convert.ToInt32(new string(c02vals.First()), 2) * Convert.ToInt32(new string(oxyvals.First()), 2);

            return new(consumption.ToString());
        }
    }
}
