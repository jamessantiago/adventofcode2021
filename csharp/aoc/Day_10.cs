using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_10 : BaseDay
    {
        private readonly List<string> _input;

        public Day_10()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var openings = new[] { '(', '[', '{', '<' };
            var clostings = new Dictionary<char, (char o, int s)>() { { ')', ('(', 3) }, { ']', ('[', 57) }, { '}', ('{', 1197) }, { '>', ('<', 25137) } };
            var errors = new List<char>();
            foreach (var input in _input)
            {
                var left = new Stack<char>();
                for (int i = 0; i < input.Length; i++)
                {
                    if (openings.Contains(input[i]))
                    {
                        left.Push(input[i]);
                    } else if (left.Pop() != clostings[input[i]].o)
                    {
                        errors.Add(input[i]);
                    }
                }
            }
            var score = errors.Sum(d => clostings[d].s);
            return new(score.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var openings = new[] { '(', '[', '{', '<' };
            var clostings = new Dictionary<char, (char o, int s)>() { { ')', ('(', 1) }, { ']', ('[', 2) }, { '}', ('{', 3) }, { '>', ('<', 4) } };
            var scores = new List<long>();
            foreach (var input in _input)
            {
                var left = new Stack<char>();
                var valid = true;
                for (int i = 0; i < input.Length; i++)
                {
                    if (openings.Contains(input[i]))
                    {
                        left.Push(input[i]);
                    }
                    else if (left.Pop() != clostings[input[i]].o)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    long score = 0;
                    while (left.Count > 0)
                    {
                        score *= 5;
                        var l = left.Pop();
                        score += clostings.First(d => d.Value.o == l).Value.s;
                    }
                    scores.Add(score);
                }
            }
            var middleScore = scores.OrderBy(d => d).Skip(scores.Count / 2).First();
            return new(middleScore.ToString());
        }
    }
}
