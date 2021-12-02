using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_02 : BaseDay
    {
        private readonly List<(string, int)> _input;

        public Day_02()
        {
            _input = File.ReadLines(InputFilePath).Select(d => (d.Split(' ')[0], int.Parse(d.Split(' ')[1]))).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            int horiz = 0, vert = 0;
            foreach (var dir in _input)
            {
                if (dir.Item1 == "forward")
                {
                    horiz += dir.Item2;
                } else if (dir.Item1 == "down")
                {
                    vert += dir.Item2;
                } else if (dir.Item1 == "up")
                {
                    vert -= dir.Item2;
                }
            }
            return new((horiz * vert).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            long horiz = 0, vert = 0, aim = 0;
            foreach (var dir in _input)
            {
                if (dir.Item1 == "forward")
                {
                    horiz += dir.Item2;
                    vert += dir.Item2 * aim;
                }
                else if (dir.Item1 == "down")
                {
                    aim += dir.Item2;
                }
                else if (dir.Item1 == "up")
                {
                    aim -= dir.Item2;
                }
            }
            return new((horiz * vert).ToString());
        }
    }
}
