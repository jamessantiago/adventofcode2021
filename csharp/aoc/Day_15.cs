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
    public class Day_15 : BaseDay
    {
        private readonly int[,] _input;

        public Day_15()
        {
            var lines = File.ReadAllLines(InputFilePath);
            _input = new int[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[0].Length; x++)
                {
                    _input[x, y] = int.Parse(lines[y][x].ToString());
                }
        }

        public List<(int x, int y)> GetNeighbors((int x, int y) point)
        {
            List<(int x, int y)> neighbors = new List<(int x, int y)>();
            if (point.x != 0) neighbors.Add((point.x - 1, point.y));
            if (point.y != 0) neighbors.Add((point.x, point.y - 1));
            if (point.x != _input.XMax()) neighbors.Add((point.x + 1, point.y));
            if (point.y != _input.YMax()) neighbors.Add((point.x, point.y + 1));
            return neighbors;
        }

        public List<(int x, int y)> GetNeighbors2((int x, int y) point, int xmax, int ymax)
        {
            List<(int x, int y)> neighbors = new List<(int x, int y)>();
            if (point.x != 0) neighbors.Add((point.x - 1, point.y));
            if (point.y != 0) neighbors.Add((point.x, point.y - 1));
            if (point.x != xmax) neighbors.Add((point.x + 1, point.y));
            if (point.y != ymax) neighbors.Add((point.x, point.y + 1));
            return neighbors;
        }

        public override ValueTask<string> Solve_1()
        {
            var queue = new Queue<((int x, int y) point, long risk)>();
            var riskGrid = new int[_input.XLen(), _input.YLen()];
            for (int x = 0; x < _input.XLen(); x++)
                for (int y = 0; y < _input.YLen(); y++)
                    riskGrid[x, y] = int.MaxValue;

            riskGrid[0, 0] = 0;
            queue.Enqueue(((0, 0), 0));

            while (queue.Count > 0)
            {
                ((int, int) point, long risk) = queue.Dequeue();
                (int x, int y) = point;
                if (x == _input.XMax() && y == _input.YMax())
                    break;

                foreach (var n in GetNeighbors(point))
                {
                    if (riskGrid[n.x, n.y] > riskGrid[x, y] + _input[n.x, n.y])
                    {
                        riskGrid[n.x, n.y] = riskGrid[x, y] + _input[n.x, n.y];
                        queue.Enqueue(((n.x, n.y), riskGrid[n.x, n.y]));
                    }
                }
            }
            //552 567 not these :(
            return new(riskGrid[riskGrid.XMax(), riskGrid.YMax()].ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var queue = new Queue<((int x, int y) point, long risk)>();
            int xmax = _input.XLen() * 5 - 1;
            int ymax = _input.YLen() * 5 - 1;
            var riskGrid = new int[xmax + 1, xmax + 1];
            for (int x = 0; x < xmax + 1; x++)
                for (int y = 0; y < ymax + 1; y++)
                    riskGrid[x, y] = int.MaxValue;

            riskGrid[0, 0] = 0;
            queue.Enqueue(((0, 0), 0));

            while (queue.Count > 0)
            {
                ((int, int) point, long risk) = queue.Dequeue();
                (int x, int y) = point;
                //if (x == xmax && y == ymax)
                //    break;

                foreach (var n in GetNeighbors2(point, xmax, ymax))
                {
                    var nrisk = _input[n.x % _input.XLen(), n.y % _input.YLen()];
                    int add = n.x / _input.XLen() + n.y / _input.YLen();
                    nrisk += add;
                    if (nrisk >= 10) nrisk = (nrisk + 1) % 10;

                    if (riskGrid[n.x, n.y] > riskGrid[x, y] + nrisk)
                    {
                        riskGrid[n.x, n.y] = riskGrid[x, y] + nrisk;
                        queue.Enqueue(((n.x, n.y), riskGrid[n.x, n.y]));
                    }
                }
            }
            // 2860 too high
            //var lrisk = _input[499 % _input.XLen(), 499 % _input.YLen()];
            //int ladd = 499 / _input.XLen() + 499 / _input.YLen();
            //lrisk += ladd;
            //if (lrisk >= 10) lrisk = (lrisk + 1) % 10;
            return new(riskGrid[xmax, ymax].ToString());
        }
                
    }
}
