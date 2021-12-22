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
    public readonly record struct cube(int x1, int x2, int y1, int y2, int z1, int z2);

    public static class util22
    {
        public static long area(this cube a) => (Math.Abs(a.x1 - a.x2) + 1) * (Math.Abs(a.y1 - a.y2) + 1) * (Math.Abs(a.z1 - a.z2) + 1);

        public static bool contains(this cube a, cube b) =>
            b.x1 > a.x2 && b.x2 < a.x2 &&
            b.y1 > a.y2 && b.y2 < a.y2 &&
            b.z1 > a.z2 && b.z2 < a.z2;

        //public static bool intersects(this cube a, cube b) =>
        //    !(a.x1 <= b.x1 || a.x1 >= b.x2 ||
        //      a.y2 <= b.y1 || a.y1 >= b.y2 ||
        //      a.z2 <= b.z1 || a.z1 >= b.z2);

        public static bool intersects(this cube a, cube b) =>
            Math.Min(a.x2, b.x2) >= Math.Max(a.x1, b.x1) &&
            Math.Min(a.y2, b.y2) >= Math.Max(a.y1, b.y1) &&
            Math.Min(a.z2, b.z2) >= Math.Max(a.z1, b.z1);

        public static bool allowed(this cube a) =>
            a.x1 >= -50 && a.x2 <= 50 &&
            a.y1 >= -50 && a.y2 <= 50 &&
            a.z1 >= -50 && a.z2 <= 50;

        public static void AddOrUpdate(this Dictionary<cube, int> dict, cube key, int value)
        {
            if (dict.ContainsKey(key)) dict[key] += value;
            else dict.Add(key, value);
        }

        public static void AddOrSet(this Dictionary<cube, int> dict, cube key, int value)
        {
            if (dict.ContainsKey(key)) dict[key] = value;
            else dict.Add(key, value);
        }
    }

    public class Day_22 : BaseDay
    {
        private readonly List<(bool on, cube cube)> _input;

        public Day_22()
        {
            _input = File.ReadAllLines(InputFilePath)
                .Select(d => Regex.Match(d, @"(on|off) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)"))
                .Select(d => (
                    d.Groups[1].Value == "on",
                    new cube (
                        int.Parse(d.Groups[2].Value), 
                        int.Parse(d.Groups[3].Value),
                        int.Parse(d.Groups[4].Value),
                        int.Parse(d.Groups[5].Value),
                        int.Parse(d.Groups[6].Value),
                        int.Parse(d.Groups[7].Value)
                        )
                    )
                ).ToList();
        }

        public cube Intersect(cube a, cube b)
        {
            var c = new cube(
                Math.Max(a.x1, b.x1),
                Math.Min(a.x2, b.x2),
                Math.Max(a.y1, b.y1),
                Math.Min(a.y2, b.y2),
                Math.Max(a.z1, b.z1),
                Math.Min(a.z2, b.z2)
                );

            return c;
        }


        public ValueTask<string> Solve(bool all)
        {
            var finalCubes = new Dictionary<cube, int>();
            var allowed = _input.Where(d => all || d.cube.allowed());
            foreach (var (on, c) in allowed)
            {
                var temp = new Dictionary<cube, int>();
                foreach (var fc in finalCubes.Keys)
                {
                    if (fc.intersects(c))
                    {
                        var newc = Intersect(fc, c);
                        //Console.WriteLine($"{c.GetHashCode(),-12} intersects with {fc.GetHashCode()}");
                        temp.AddOrUpdate(newc, -finalCubes[fc]);
                    }
                }
                if (on) temp.AddOrUpdate(c, 1);
                foreach (var tc in temp)
                {
                    finalCubes.AddOrUpdate(tc.Key, tc.Value);
                    //Console.WriteLine($"{tc.Key.GetHashCode(),-12} intersection/cube now {finalCubes[tc.Key]}");
                }
                //Console.WriteLine($"{temp.Count,-4} intersections, total on: {finalCubes.Sum(d =>  d.Key.area() * d.Value)}");
                //Console.WriteLine();
            }

            var result = 0L;
            foreach (var fc in finalCubes)
            {
                result = checked(result + fc.Value * fc.Key.area());
            }

            return new(result.ToString());
        }

        public override ValueTask<string> Solve_1()
        {
            return Solve(false);
        }

        public override ValueTask<string> Solve_2()
        {
            return Solve(true);
        }
    }


}
