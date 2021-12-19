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
    public class Day_19 : BaseDay
    {
        private readonly List<List<(int x, int y, int z)>> _input = new List<List<(int x, int y, int z)>>();

        public Day_19()
        {
            List<(int x, int y, int z)> cur = null;
            foreach (var line in File.ReadLines(InputFilePath))
            {
                var lm = Regex.Match(line, @"scanner (\d)");
                if (lm.Success)
                {
                    if (cur != null) _input.Add(cur);
                    cur = new List<(int x, int y, int z)>();
                }
                else if (!string.IsNullOrEmpty(line))
                {
                    var p = line.Split(',');
                    (int x, int y, int z) v = (int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                    cur.Add(v);
                }
            }
            _input.Add(cur);
        }
        
        public int distance((int x, int y, int z) a, (int x, int y, int z) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);

        public IEnumerable<List<(int x, int y, int z)>> orientations(List<(int x, int y, int z)> points)
        {
            yield return points.Select(p => (p.x, p.y, p.z)).ToList();
            yield return points.Select(p => (p.y, p.z, p.x)).ToList();
            yield return points.Select(p => (p.z, p.x, p.y)).ToList();
            yield return points.Select(p => (p.z, p.y, -p.x)).ToList();
            yield return points.Select(p => (p.y, p.x, -p.z)).ToList();
            yield return points.Select(p => (p.x, p.z, -p.y)).ToList();
            yield return points.Select(p => (p.x, -p.y, -p.z)).ToList();
            yield return points.Select(p => (p.y, -p.z, -p.x)).ToList();
            yield return points.Select(p => (p.z, -p.x, -p.y)).ToList();
            yield return points.Select(p => (p.z, -p.y, p.x)).ToList();
            yield return points.Select(p => (p.y, -p.x, p.z)).ToList();
            yield return points.Select(p => (p.x, -p.z, p.y)).ToList();
            yield return points.Select(p => (-p.x, p.y, -p.z)).ToList();
            yield return points.Select(p => (-p.y, p.z, -p.x)).ToList();
            yield return points.Select(p => (-p.z, p.x, -p.y)).ToList();
            yield return points.Select(p => (-p.z, p.y, p.x)).ToList();
            yield return points.Select(p => (-p.y, p.x, p.z)).ToList();
            yield return points.Select(p => (-p.x, p.z, p.y)).ToList();
            yield return points.Select(p => (-p.x, -p.y, p.z)).ToList();
            yield return points.Select(p => (-p.y, -p.z, p.x)).ToList();
            yield return points.Select(p => (-p.z, -p.x, p.y)).ToList();
            yield return points.Select(p => (-p.z, -p.y, -p.x)).ToList();
            yield return points.Select(p => (-p.y, -p.x, -p.z)).ToList();
            yield return points.Select(p => (-p.x, -p.z, -p.y)).ToList();
        }

        public List<(int x, int z, int y)> differs = new List<(int x, int z, int y)>();

        public override ValueTask<string> Solve_1()
        {
            var left = new List<List<(int x, int y, int z)>>(_input);
            var oriented = new List<List<(int x, int y, int z)>>() { left[0] };
            left.RemoveAt(0);
            while (left.Count > 0)
            {
                for (int i = 0; i < left.Count; i++)
                    foreach (var o in oriented)
                        foreach (var b in orientations(left[i]))
                            for (int j = 0; j < o.Count; j++)
                                for (int k = 0; k < j && k < b.Count; k++)
                                {
                                    (int x, int y, int z) differ = (o[j].x - b[k].x, o[j].y - b[k].y, o[j].z - b[k].z);
                                    var nb = b.Select(d => (d.x + differ.x, d.y + differ.y, d.z + differ.z)).ToList();
                                    var distinct = o.Union(nb).Distinct().ToList();
                                    if (o.Count + b.Count - distinct.Count >= 12)
                                    {
                                        differs.Add(differ);
                                        left.RemoveAt(i);
                                        oriented.Add(nb);
                                        goto End;
                                    }
                                }
                            End: continue;
            }
            var res = oriented.SelectMany(d => d).Distinct().OrderBy(d => d.x).ToList();
            return new(res.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            differs.Add((0, 0, 0));
            int max = 0;
            for (int i = 0; i < differs.Count; i++)
                for (int j = 0; j < differs.Count; j++)
                    max = Math.Max(max, distance(differs[i], differs[j]));

            return new(max.ToString());
        }
    }
}
