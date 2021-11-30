using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc
{    public class Day_01 : BaseDay
    {
        private readonly List<string> _input;

        public Day_01()
        {
            _input = File.ReadLines(InputFilePath).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            return new("");
        }

        public override ValueTask<string> Solve_2()
        {
            return new("");
        }
    }
}
