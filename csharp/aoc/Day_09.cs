using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_09 : BaseDay
    {
        private readonly int[,] _input;

        public Day_09()
        {
            var lines = File.ReadAllLines(InputFilePath);
            _input = new int[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    _input[x,y] = int.Parse(lines[y][x].ToString());
                }
            }
        }

        public List<(int x, int y, int v)> GetNeighbors((int x, int y) point)
        {
            List<(int x, int y, int v)> neighbors = new List<(int x, int y, int v)>();
            if (point.x != 0) neighbors.Add((point.x - 1, point.y, _input[point.x - 1, point.y]));
            if (point.y != 0) neighbors.Add((point.x, point.y - 1, _input[point.x, point.y - 1]));
            if (point.x != _input.GetLength(0) - 1) neighbors.Add((point.x + 1, point.y, _input[point.x + 1, point.y]));
            if (point.y != _input.GetLength(1) - 1) neighbors.Add((point.x, point.y + 1, _input[point.x, point.y + 1]));
            return neighbors;
        }

        public Dictionary<(int x, int y), HashSet<(int x, int y, int v)>> Basins = new Dictionary<(int x, int y), HashSet<(int x, int y, int v)>>();
        public override ValueTask<string> Solve_1()
        {
            List<int> lowPointRisks = new List<int>();
            for (int x = 0; x < _input.GetLength(0); x++)
            {
                for (int y = 0; y < _input.GetLength(1); y++)
                {
                    var neighbors = GetNeighbors((x, y));
                    if (_input[x,y] < neighbors.Min(d => d.v))
                    {
                        lowPointRisks.Add(_input[x, y] + 1);
                        Basins.Add((x, y), new HashSet<(int x, int y, int v)>());
                    }
                }
            }
            return new(lowPointRisks.Sum().ToString());
        }

        public List<(int x, int y, int v)> AddNeighbors((int x, int y) point, HashSet<(int x, int y, int v)> basin) {
            List<(int x, int y, int v)> neighbors = new List<(int x, int y, int v)>();
            foreach (var n in GetNeighbors(point).Where(d => d.v != 9))
            {
                if (basin.Add(n)) neighbors.Add(n);
            }
            return neighbors;
        }

        public void FillBasin(KeyValuePair<(int x, int y), HashSet<(int x, int y, int v)>> basin)
        {
            var neighbors = AddNeighbors((basin.Key.x, basin.Key.y), basin.Value);
            foreach (var n in neighbors)
            {
                FillBasin(new KeyValuePair<(int x, int y), HashSet<(int x, int y, int v)>>((n.x, n.y), basin.Value));
            }
        }

        public override ValueTask<string> Solve_2()
        {
            foreach (var basin in Basins)
            {
                FillBasin(basin);
            }

            var top3 = Basins.OrderByDescending(d => d.Value.Count).Take(3).Aggregate(1, (a, v) => a * v.Value.Count);
            return new(top3.ToString());
        }
    }
}
