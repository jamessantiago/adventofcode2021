using AoCHelper;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{
    public class Day_21 : BaseDay
    {
        private readonly int[] _input;
        private readonly int[] _input2;

        public Day_21()
        {
            _input = File.ReadAllLines(InputFilePath).Select(d => Regex.Match(d, @"Player (\d+) starting position: (\d+)"))
                .Select(d => int.Parse(d.Groups[2].Value) - 1).ToArray();
            _input2 = (int[])_input.Clone();
        }

        public override ValueTask<string> Solve_1()
        {
            long[] scores = { 0, 0 };
            long rolls = 0;
            for (int i = 0; ;  i += 2 * 3)
            {
                _input[0] = (_input[0] + (i + 1 % 100) + (i + 2 % 100) + (i + 3 % 100)) % 10;
                scores[0] += _input[0] + 1;
                rolls = i + 3;
                if (scores[0] >= 1000) break;
                _input[1] = (_input[1] + (i + 4 % 100) + (i + 5 % 100) + (i + 6 % 100)) % 10;
                scores[1] += _input[1] + 1;
                rolls = i + 6;
                if (scores[1] >= 1000) break;
            }

            return new((Math.Min(scores[0], scores[1]) * rolls).ToString());
        }

        public long[] wins = { 0, 0 };
        public int[] probability = { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };
        public void getWins((int p1, int p2, int s1, int s2, long r, bool turn) state)
        {
            var (p1, p2, s1, s2, r, turn) = state;
            if (turn)
                for (int i = 3; i <= 9; i++)
                {
                    var np1 = (p1 + i) % 10;
                    var ns1 = s1 + np1 + 1;
                    if (ns1 >= 21) wins[0] += r * probability[i];
                    else getWins((np1, p2, ns1, s2, r * probability[i], !turn));
                }
            else
                for (int i = 3; i <= 9; i++)
                {
                    var np2 = (p2 + i) % 10;
                    var ns2 = s2 + np2 + 1;
                    if (ns2 >= 21) wins[1] += r * probability[i];
                    else getWins((p1, np2, s1, ns2, r * probability[i], !turn));
                }                
        }

        public override ValueTask<string> Solve_2()
        {
            getWins((_input2[0], _input2[1], 0, 0, 1, true));

            return new(wins.Max().ToString());
        }
    }
}
