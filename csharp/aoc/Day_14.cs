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
    public class Day_14 : BaseDay
    {
        private readonly string template;
        private readonly Dictionary<string, string> insertions;

        public Day_14()
        {
            var lines = File.ReadAllLines(InputFilePath);
            template = lines[0];
            insertions = lines.Skip(2).Select(d => d.Split(" -> ")).ToDictionary(d => d[0], d => d[1]);
        }

        public override ValueTask<string> Solve_1()
        {
            var sequence = ImmutableList.Create(template.ToCharArray());
            for (int i = 0; i < 10; i++)
            {
                var matches = new List<(char c, int p)>();
                for (int j = 0; j < sequence.Count - 1; j++)
                {
                    var m = insertions.GetValueOrDefault(new string(sequence.Skip(j).Take(2).ToArray()));
                    if (m != null) matches.Add((m[0], j));
                }
                var offset = 1;
                foreach (var match in matches)
                {
                    sequence = sequence.Insert(match.p + offset, match.c);
                    offset++;
                }
            }

            var counts = sequence.GroupBy(d => d).Select(d => d.Count());
            var result = counts.Max() - counts.Min();
            return new(result.ToString());
        }

        private Dictionary<string, long> Aggregator(Dictionary<string, long> current)
        {
            var next = new Dictionary<string, long>();
            foreach (var cur in current)
            {
                var num = current[cur.Key];
                if (insertions.ContainsKey(cur.Key))
                {
                    var insert = insertions[cur.Key];
                    var a = cur.Key[0] + insert;
                    var b = insert + cur.Key[1];
                    next.AddOrUpdate(a, num);
                    next.AddOrUpdate(b, num);
                }
            }

            return next;
        }

        public override ValueTask<string> Solve_2()
        {
            var pairs = new Dictionary<string, long>();
            for (int j = 0; j < template.Length - 1; j++)
            {
                var k = new string(template.Skip(j).Take(2).ToArray());
                if (pairs.ContainsKey(k)) pairs[k]++;
                else pairs.Add(k, 1);
            }

            //for (int i = 0; i < 40; i++)
            //{
            //    var newPairs = new Dictionary<string, long>();

            //    foreach (var pair in pairs)
            //    {
            //        if (insertions.ContainsKey(pair.Key))
            //        {
            //            var newchar = insertions[pair.Key][0];
            //            var new1 = new string(new char[] { pair.Key[0], newchar });
            //            var new2 = new string(new char[] { newchar, pair.Key[1] });
            //            if (newPairs.ContainsKey(new1)) newPairs[new1] += pair.Value;
            //            else newPairs.Add(new1, pair.Value);

            //            if (newPairs.ContainsKey(new2)) newPairs[new2] += pair.Value;
            //            else newPairs.Add(new2, pair.Value);
            //        }
            //    }

            //    foreach (var newpair in newPairs)
            //    {
            //        if (pairs.ContainsKey(newpair.Key)) pairs[newpair.Key] += newpair.Value;
            //        else pairs.Add(newpair.Key, newpair.Value);
            //    }
            //}

            var generatedPairs = Enumerable.Range(0, 40).Aggregate(pairs, (d, _) => Aggregator(d));

            var counts = new Dictionary<char, long>();
            foreach (var pair in generatedPairs)
            {
                counts.AddOrUpdate(pair.Key[0], pair.Value);
                counts.AddOrUpdate(pair.Key[1], pair.Value);
            }
            foreach (var c in counts)
            {
                var pre = (counts[c.Key] % 1) > 0;
                counts[c.Key] /= 2;
                if (pre && c.Key == template.First())
                {
                    counts[c.Key] += 1;
                }
                else if (pre && c.Key == template.Last())
                {
                    counts[c.Key] += 1;
                }
            }

            var result = counts.Max(d => d.Value) - counts.Min(d => d.Value) - 1;

            return new(result.ToString());
        }
                
    }

    public static class util
    {
        public static void AddOrUpdate<T>(this Dictionary<T, long> dict, T key, long value)
        {
            if (dict.ContainsKey(key)) dict[key] += value;
            else dict.Add(key, value);
        }
    }
}
