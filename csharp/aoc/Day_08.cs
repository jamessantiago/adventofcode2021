using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_08 : BaseDay
    {
        private readonly List<(string[] signal, string[] output)> _input;

        public Day_08()
        {
            _input = File.ReadAllLines(InputFilePath).Select(d => d.Split('|')).Select(d => (d[0].Trim().Split(' '), d[1].Trim().Split(' '))).ToList();
        }

        public int EvaluateUniqueSignal(string signal)
        {
            if (signal.Length == 2) return 1;
            else if (signal.Length == 4) return 4;
            else if (signal.Length == 3) return 7;
            else if (signal.Length == 7) return 8;            
            else return -1;
        }        

        public int PatternIntersection(string signal, string? pattern)
        {
            if (pattern == null) return -1;
            var diff = signal.ToCharArray().Intersect(pattern.ToCharArray()).Count();
            return diff;
        }

        public Dictionary<string, int> DiscoverMappings(string[] signals)
        {
            Dictionary<string, int> discovered = new Dictionary<string, int>();
            Dictionary<int, string> map = new Dictionary<int, string>();
            while (discovered.Count != signals.Length)
            {
                foreach (var signal in signals)
                {
                    if (map.Count != 7) { 
                        if (EvaluateUniqueSignal(signal) == 1)
                        {
                            map[1] = signal;
                            discovered[String.Concat(signal.OrderBy(c => c))] = 1;
                            continue;
                        }
                        else if (EvaluateUniqueSignal(signal) == 4)
                        {
                            map[4] = signal;
                            discovered[String.Concat(signal.OrderBy(c => c))] = 4;
                            continue;
                        }
                        else if (EvaluateUniqueSignal(signal) == 7)
                        {
                            map[7] = signal;
                            discovered[String.Concat(signal.OrderBy(c => c))] = 7;
                            continue;
                        }
                        else if (EvaluateUniqueSignal(signal) == 8)
                        {
                            map[8] = signal;
                            discovered[String.Concat(signal.OrderBy(c => c))] = 8;
                            continue;
                        }
                    }

                    if (signal.Length == 6)
                    {
                        if (PatternIntersection(signal, map.GetValueOrDefault(4)) == 4) // 9
                        {
                            map[9] = signal;
                            discovered.TryAdd(String.Concat(signal.OrderBy(c => c)), 9);
                        } else if (PatternIntersection(signal, map.GetValueOrDefault(1)) == 1) // 6
                        {
                            map[6] = signal;
                            discovered[String.Concat(signal.OrderBy(c => c))] = 6;
                        } else if (map.ContainsKey(4) && map.ContainsKey(1))
                        {
                            map[0] = signal;
                            discovered.TryAdd(String.Concat(signal.OrderBy(c => c)), 0);
                        }
                    } else if (signal.Length == 5) // 2 3 5
                    {
                        if (PatternIntersection(signal, map.GetValueOrDefault(1)) == 2) // 3
                        {
                            map[3] = signal;
                            discovered.TryAdd(String.Concat(signal.OrderBy(c => c)), 3);
                        } else if (PatternIntersection(signal, map.GetValueOrDefault(4)) == 2) // 2
                        {
                            map[2] = signal;
                            discovered.TryAdd(String.Concat(signal.OrderBy(c => c)), 2);
                        } else if (map.ContainsKey(4) && map.ContainsKey(1))
                        {
                            map[5] = signal;
                            discovered.TryAdd(String.Concat(signal.OrderBy(c => c)), 5);
                        }
                    }
                }
            }
            return discovered;
        }

        public override ValueTask<string> Solve_1()
        {
            var c = _input.SelectMany(d => d.output).Count(d => EvaluateUniqueSignal(d) != -1);
            return new(c.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            long sum = 0;
            foreach (var (signals, output) in _input)
            {
                var discovered = DiscoverMappings(signals);
                long res = 0;
                foreach (var o in output)
                {
                    res *= 10;
                    res += discovered[String.Concat(o.OrderBy(c => c))];
                }
                sum += res;
            }
            return new (sum.ToString());
        }
    }
}
