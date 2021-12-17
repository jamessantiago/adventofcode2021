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
    public class Day_17 : BaseDay
    {
        private readonly long[] area;
        private long xmin => area[0];
        private long xmax => area[1];
        private long ymin => area[2];
        private long ymax => area[3];

        public Day_17()
        {
            area = Regex.Match(File.ReadAllText(InputFilePath), @"x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)")
                .Groups.Values.Skip(1).Select(d => Int64.Parse(d.Value)).ToArray();
            var (xmin, xmax, ymin, ymax) = (0, 1, 2, 3);
        }

        public bool isIn(long x, long y)
        {
            return x >= area[xmin] && x <= area[xmax] &&
                   y >= area[ymin] && y <= area[ymax];
        }

        public bool overshot(long x, long y)
        {
            return (y >= area[ymin] && x > area[xmax]) || y < area[ymax];
        }

        public bool undershot(long x, long y, long nvx)
        {
            return y > area[ymax] && x < area[xmin] && nvx == 0;
        }

        public override ValueTask<string> Solve_1()
        {
            var maxy = area[ymin] * (area[ymin] + 1) / 2;
            return new(maxy.ToString());
        }

public override ValueTask<string> Solve_2()
        {
            long total = 0;
            for (long vx = (long)Math.Sqrt(area[xmin]) / 2; vx <= area[xmax]; vx++)
            {
                for (long vy = area[ymin]; vy < -area[ymin]; vy++)
                {
                    long sx = 0, sy = 0, nvx = vx, nvy = vy;
                    while (sx <= area[xmax] && sy >= area[ymin] && (sx < area[xmin] || sy > area[ymax]))
                    {
                        sx += nvx;
                        sy += nvy;

                        nvx = nvx > 0 ? nvx - 1 : 0;
                        nvy--;
                    }
                    if (isIn(sx, sy)) total++;
                }
            }
            return new(total.ToString());
               
    }
}
