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
    public class Day_18 : BaseDay
    {
        private readonly List<string> _input;

        public Day_18()
        {
            _input = File.ReadLines(InputFilePath).ToList();
        }

        public bool canExplode((int, int) fish) => fish != (-1, -1);

        public (int, int) getBomb(string fish)
        {
            var stack = new Stack<(char c, int i)>();
            for (int i = 0; i < fish.Length; i++)
            {
                if (fish[i] == '[')
                    stack.Push((fish[i], i));

                if (fish[i] == ']' && stack.Count > 4)
                {
                    return (stack.Pop().i + 1, i);
                } else if (fish[i] == ']')
                {
                    stack.Pop();
                }
            }
            return (-1, -1);
        }

        public string explode(string fish, (int a, int b) bomb)
        {
            var bomb_vals = Regex.Match(fish[bomb.a..bomb.b], @"(\d+),(\d+)").Groups.Values.Skip(1).Select(d => int.Parse(d.Value)).ToArray();

            var leftfish = fish[..(bomb.a - 1)];
            var rightfish = fish[(bomb.b + 1)..];

            var lm = Regex.Match(leftfish, @"(\d+)", RegexOptions.RightToLeft);
            if (lm.Success)
            {
                var i = lm.Groups[1].Index;
                var ni = i + lm.Groups[1].Value.Length;
                var v = int.Parse(lm.Groups[1].Value) + bomb_vals[0];
                leftfish = leftfish[..i] + v.ToString() + leftfish[ni..];
            }

            var rm = Regex.Match(rightfish, @"(\d+)");
            if (rm.Success)
            {
                var i = rm.Groups[1].Index;
                var ni = i + rm.Groups[1].Value.Length;
                var v = int.Parse(rm.Groups[1].Value) + bomb_vals[1];
                rightfish = rightfish[..i] + v.ToString() + rightfish[ni..];
            }

            return leftfish + "0" + rightfish;
        }

        public bool canSplit(string fish) => Regex.IsMatch(fish, @"\d{2,}");

        public string split(string fish)
        {
            var sm = Regex.Match(fish, @"\d{2,}");
            if (sm.Success)
            {
                var i = sm.Index;
                var ni = i + sm.Value.Length;
                var v = int.Parse(sm.Value);
                int l = v / 2;
                int r = (int)Math.Ceiling((double)v / 2);
                fish = fish[..i] + $"[{l},{r}]" + fish[ni..];
            }
            return fish;
        }

        public string sum(string f1, string f2) => $"[{f1},{f2}]";

        public string reduce(string fish)
        {
            bool reduced = false;
            do
            {
                var bomb = getBomb(fish);
                if (canExplode(bomb))
                {
                    fish = explode(fish, bomb);
                    reduced = true;
                } else if (canSplit(fish))
                {
                    fish = split(fish);
                    reduced = true;
                } else
                {
                    reduced = false;
                }
            } while (reduced);
            return fish;
        }

        public string magnitude(string fish)
        {
            var stack = new Stack<char>();
            for (int i = 0; i < fish.Length; i++)
            {
                stack.Push(fish[i]);

                if (fish[i] == ']')
                {
                    var f = "";
                    char c;
                    do
                    {
                        c = stack.Pop();
                        f = c + f;
                    } while (c != '[');
                    var sub = Regex.Match(f, @"(\d+),(\d+)").Groups;
                    var subval = (Int64.Parse(sub[1].Value) * 3 + Int64.Parse(sub[2].Value) * 2).ToString();
                    for (int j = 0; j < subval.Length; j++) stack.Push(subval[j]);
                }
            }

            var res = "";
            while (stack.Count > 0)
            {
                res = stack.Pop() + res;
            }
            return res;
        }

        public override ValueTask<string> Solve_1()
        {
            var curfish = _input[0];
            foreach (var nextfish in _input.Skip(1))
            {
                curfish = reduce(sum(curfish, nextfish));
            }
            var mag = magnitude(curfish);

            return new(mag);
        }

        public override ValueTask<string> Solve_2()
        {
            var max = 0L;
            for (int i = 0; i < _input.Count; i++)
                for (int j = 0; j < _input.Count; j++)
                {
                    var mag1 = magnitude(reduce(sum(_input[i], _input[j])));
                    max = Math.Max(max, Int64.Parse(mag1));

                    var mag2 = magnitude(reduce(sum(_input[i], _input[j])));
                    max = Math.Max(max, Int64.Parse(mag2));
                }

            return new(max.ToString());
        }
    }
}
