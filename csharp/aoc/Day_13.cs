using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_13 : BaseDay
    {
        private readonly List<(int x, int y)> points;
        private readonly List<(string axis, int p)> folds;

        public Day_13()
        {
            var pr = new Regex(@"\d+,\d+");
            var lines = File.ReadAllLines(InputFilePath);
            points = lines.Where(d => pr.IsMatch(d)).Select(d => d.Split(',')).Select(d => (int.Parse(d[0]), int.Parse(d[1]))).ToList();
            folds = lines.Where(d => !pr.IsMatch(d) && !string.IsNullOrEmpty(d)).Select(d => Regex.Match(d, @"(x|y)=(\d+)"))
                .Select(d => (d.Groups[1].Value, int.Parse(d.Groups[2].Value))).ToList();
        }

        public void print(IList<(int x, int y)> points)
        {
            var xmax = points.Select(d => d.x).Max();
            var ymax = points.Select(d => d.y).Max();
            for (int y = 0; y <= ymax; y++)
            {
                for (int x = 0; x <= xmax; x++)
                {
                    Console.Write(points.Contains((x, y)) ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var curPoints = new List<(int x, int y)>(points);
            var xmax = curPoints.Select(d => d.x).Max();
            var ymax = curPoints.Select(d => d.y).Max();
            var (axis, p) = folds.First();
            var newPoints = new HashSet<(int x, int y)>();
            foreach (var (x, y) in curPoints)
            {
                if (axis == "x")
                    newPoints.Add((x > p ? xmax - x : x, y));
                else
                    newPoints.Add((x, y > p ? ymax - y : y));
            }
            //print(newPoints);
            
            return new(newPoints.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {            
            var curPoints = new List<(int x, int y)>(points);
            foreach (var (axis, p) in folds)
            {
                var xmax = curPoints.Select(d => d.x).Max();
                var ymax = curPoints.Select(d => d.y).Max();
                var newPoints = new HashSet<(int x, int y)>();
                foreach (var (x, y) in curPoints)
                {
                    if (axis == "x")
                        newPoints.Add((x > p ? xmax - x : x, y));
                    else
                        newPoints.Add((x, y > p ? ymax - y : y));
                }
                curPoints = new List<(int x, int y)>(newPoints);
            }
            print(curPoints);
            return new("");
        }
    }
}
