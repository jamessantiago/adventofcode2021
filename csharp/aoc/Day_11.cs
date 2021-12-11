using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_11 : BaseDay
    {
        private readonly Dictionary<(int x, int y), int> _input;
        private readonly int xmax;
        private readonly int ymax;
        private event EventHandler<((int x, int y) p, int v)> valueChanged;

        public Day_11()
        {
            var lines = File.ReadAllLines(InputFilePath);
            _input = new Dictionary<(int x, int y), int>();
            xmax = lines[0].Length - 1;
            ymax = lines.Length - 1;
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                    _input[(x,y)] = int.Parse(lines[y][x].ToString());
            }
        }

        public List<(int x, int y)> GetNeighbors((int, int) point)
        {
            var (x, y) = point;
            var neighbors = new List<(int x, int y)>();
            if (x != 0) neighbors.Add((x - 1, y));
            if (x != xmax) neighbors.Add((x + 1, y));
            if (y != 0) neighbors.Add((x, y - 1));
            if (y != ymax) neighbors.Add((x, y + 1));

            if (x != 0 && y != 0) neighbors.Add((x - 1, y - 1));
            if (x != xmax && y != 0) neighbors.Add((x + 1, y - 1));
            if (x != xmax && y != ymax) neighbors.Add((x + 1, y + 1));
            if (x != 0 && y != ymax) neighbors.Add((x - 1, y + 1));

            return neighbors;
        }

        public override ValueTask<string> Solve_1()
        {
            var grid = new Dictionary<(int x, int y), int>(_input);

            var totalFlashed = 0L;
            for (int i = 0; i < 100; i++)
            {
                foreach (var k in grid.Keys) grid[k]++;

                var flashed = new HashSet<(int x, int y)>();
                var queue = new Queue<(int x, int y)>();
                foreach (var s in grid.Where(d => d.Value > 9)) queue.Enqueue(s.Key);

                while (queue.Count > 0)
                {
                    var s = queue.Dequeue();
                    if (grid[s] > 9 && flashed.Add(s))
                    {
                        grid[s] = 0;
                        foreach(var n in GetNeighbors(s))
                        {
                            if (grid[n] == 0) continue;
                            grid[n]++;
                            queue.Enqueue(n);
                        }
                    }
                }
                totalFlashed += flashed.Count;
            }
            return new(totalFlashed.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            valueChanged += (o, input) =>
            {
                Console.SetCursorPosition(input.p.x + 2, input.p.y + 1);
                Console.Write(input.v);
            };

            var grid = new Dictionary<(int x, int y), int>(_input);

            int i = 0;
            for (; ; i++)
            {
                foreach (var k in grid.Keys)
                {
                    grid[k]++;
                    valueChanged?.Invoke(this, (k, grid[k]));
                }

                    var flashed = new HashSet<(int x, int y)>();
                var queue = new Queue<(int x, int y)>();
                foreach (var s in grid.Where(d => d.Value > 9)) queue.Enqueue(s.Key);

                while (queue.Count > 0)
                {
                    var s = queue.Dequeue();
                    if (grid[s] > 9 && flashed.Add(s))
                    {
                        grid[s] = 0;
                        foreach (var n in GetNeighbors(s))
                        {
                            if (grid[n] == 0) continue;
                            grid[n]++;
                            queue.Enqueue(n);
                            valueChanged?.Invoke(this, (n, grid[n]));
                        }
                    }
                }
                if (flashed.Count == grid.Count) break;
            }
            i++;
            Console.WriteLine("\n\n\n");
            return new(i.ToString());
        }
    }
}
