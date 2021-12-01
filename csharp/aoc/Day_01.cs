using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_01 : BaseDay
    {
        private readonly List<int> _input;

        public Day_01()
        {
            _input = File.ReadLines(InputFilePath).Select(d => int.Parse(d)).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            int increasing = 0;
            for (int i = 1; i < _input.Count; i++)
            {
                if (_input[i] > _input[i - 1]) increasing++;
            }
            return new(increasing.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int increasing = 0;
            for (int i = 1; i < _input.Count - 2; i++)
            {
                int sum1 = _input[i - 1] + _input[i] + _input[i + 1];
                int sum2 = _input[i] + _input[i + 1] + _input[i + 2];
                if (sum2 > sum1) increasing++;
            }
            return new(increasing.ToString());
        }
    }
}
