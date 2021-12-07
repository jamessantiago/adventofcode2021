using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_07 : BaseDay
    {
        private readonly int[] _input;

        public Day_07()
        {
            _input = File.ReadAllText(InputFilePath).Split(',').Select(int.Parse).ToArray();
            
        }

        public override ValueTask<string> Solve_1()
        {
            var ordered = _input.OrderBy(d => d);
            var mid = ordered.Skip(ordered.Count() / 2).First();
            var fuel = ordered.Sum(d => Math.Abs(d - mid));
            return new(fuel.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var avg = (int)_input.Average();
            var fuel = _input.Select(d => Math.Abs(avg - d)).Sum(d => (d * (d + 1)) / 2);
            return new(fuel.ToString());
        }
    }
}
