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
    public class Day_20 : BaseDay
    {
        private readonly string _algo;
        private readonly Dictionary<(int x, int y), bool> _input = new Dictionary<(int x, int y), bool>();

        public Day_20()
        {
            var lines = File.ReadAllLines(InputFilePath);
            _algo = lines[0];
            var image = lines.Skip(2).ToList();
            for (int y = 0; y < image.Count; y++)
                for (int x = 0; x < image[0].Length; x++)
                    _input.Add((x, y), image[y][x] == '#');
        }

        public int getIndex((int x, int y) point, Dictionary<(int x, int y), bool> image, int step)
        {
            var sb = new StringBuilder();
            var (x, y) = point;
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                    sb.Append(image.GetValueOrDefault((x + j, y + i), step % 2 == 0) ? '1' : '0');                       

            return Convert.ToInt32(sb.ToString(), 2);
        }

        public void print(Dictionary<(int x, int y), bool> cur)
        {
            var (minX, minY) = (cur.Keys.Min(p => p.x), cur.Keys.Min(p => p.y));
            var (maxX, maxY) = (cur.Keys.Max(p => p.x), cur.Keys.Max(p => p.y));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Console.Write(cur.GetValueOrDefault((x, y)) ? '#' : '.');
                }
                Console.WriteLine();
            }

        }

        public Dictionary<(int x, int y), bool> process(Dictionary<(int x, int y), bool> input, int step)
        {
            var output = new Dictionary<(int x, int y), bool>();
            var m = _input.Keys.Max(p => p.x);
            
            for (int x = -step; x <= m + step; x++)
            {
                for (int y = -step; y <= m + step; y++)
                {
                    var i = getIndex((x, y), input, step);
                    output.Add((x, y), _algo[i] == '#');
                }
            }
            return output;
        }

        public override ValueTask<string> Solve_1()
        {
            var o1 = process(_input, 1);
            var o2 = process(o1, 2);

            return new(o2.Count(d => d.Value).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var cur = process(_input, 1);
            for (int i = 2; i <= 50; i++)
            { 
                cur = process(cur, i);
            }
            return new(cur.Count(d => d.Value).ToString());
        }
    }
}
