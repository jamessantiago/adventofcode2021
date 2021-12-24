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
    public static class util23
    {
        public static char GD(this Dictionary<(int x, int y), char> dict, int x, int y) => dict.GetValueOrDefault((x, y));
    }

    public class Day_23 : BaseDay
    {
        private readonly Dictionary<(int x, int y), char> _input = new Dictionary<(int x, int y), char>();
        private readonly Dictionary<(int x, int y), char> _complete = new Dictionary<(int x, int y), char>();

        public Day_23()
        {
            var lines = File.ReadAllLines(InputFilePath).ToList();

            for (int y = 0; y < 5; y++)
                for (int x = 0; x < lines[0].Length; x++)
                    _input.Add((x, y), lines[y][x]);

            for (int y = 5; y < lines.Count; y++)
                for (int x = 0; x < lines[0].Length; x++)
                    _complete.Add((x, y - 5), lines[y][x]);
        }

        public IEnumerable<(int x, int y, int c)> AvailMoves(Dictionary<(int x, int y), char> state, int x, int y, char a)
        {
            if (y != 1)
            {
                int cost = 0;
                y--;
                if (state.GD(x, y) == '.') {
                    cost++;                
                    var nx = x;
                    while (state.GD(nx, y) == '.')
                    {
                        nx--; cost++;
                        if (state.GD(nx, y + 1) == '#') yield return (nx, y, cost);
                    }
                    cost = 1;
                    nx = x;
                    while (state.GD(nx, y) == '.')
                    {
                        nx++; cost++;
                        if (state.GD(nx, y + 1) == '#') yield return (nx, y, cost);
                    }
                }
            } else
            {
                int cost = 1;
                var nx = x + 1;
                while (state.GD(nx, y) == '.')
                {
                    nx++; cost++;
                    if (state.GD(nx, y + 1) == '.') yield return (nx, y + 1, cost + 1);
                    if (state.GD(nx, y + 2) == '.') yield return (nx, y + 2, cost + 2);
                }
                cost = 1;
                nx = x - 1;
                while (state.GD(nx, y) == '.')
                {
                    nx--; cost++;
                    if (state.GD(nx, y + 1) == '.') yield return (nx, y + 1, cost + 1);
                    if (state.GD(nx, y + 2) == '.') yield return (nx, y + 2, cost + 2);
                }
            }
        }           

        public bool IsComplete(Dictionary<(int x, int y), char> state)
        {
            foreach (var k in state.Keys)
            {
                if (state[k] != _complete[k]) return false;
            }

            return true;
        }

        public void print(Dictionary<(int x, int y), char> state)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x= 0; x < 13; x++)
                {
                    Console.Write(state[(x, y)]);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
        }



        public override ValueTask<string> Solve_1()
        {
            var queue = new Queue<(int x, int y, char a, Dictionary<(int x, int y), char> state, long total)>();
            var amphipods = "ABCD".ToCharArray();
            var aVals = new Dictionary<char, int>();
            aVals.Add('A', 1);
            aVals.Add('B', 10);
            aVals.Add('C', 100);
            aVals.Add('D', 1000);

            // just do by hand
            return new("");
        }

        public override ValueTask<string> Solve_2()
        {
            // harder, but doable
            return new("");
        }
    }


}
