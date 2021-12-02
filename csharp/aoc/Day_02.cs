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
        private readonly List<(string dir, int amount)> _input;

        public Day_02()
        {
            _input = File.ReadLines(InputFilePath).Select(d => d.Split(' ')).Select(d => (d[0], int.Parse(d[1]))).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            int horiz = 0, vert = 0;
            foreach (var (dir, amount) in _input)
            {
                if (dir == "forward")
                {
                    horiz += amount;
                } else if (dir == "down")
                {
                    vert += amount;
                } else if (dir == "up")
                {
                    vert -= amount;
                }
            }
            return new((horiz * vert).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            long horiz = 0, vert = 0, aim = 0;
            foreach (var (dir, amount) in _input)
            {
                if (dir == "forward")
                {
                    horiz += amount;
                    vert += amount * aim;
                }
                else if (dir == "down")
                {
                    aim += amount;
                }
                else if (dir == "up")
                {
                    aim -= amount;
                }
            }
            return new((horiz * vert).ToString());
        }
    }
}
