using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_12 : BaseDay
    {
        private readonly List<(string a, string b)> _input;

        public Day_12()
        {
            _input = File.ReadAllLines(InputFilePath).Select(d => d.Split('-')).Select(d => (d[0], d[1])).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var queue = new Queue<(string next, List<string> visited)>();
            foreach (var input in _input.Where(d => d.a == "start")) queue.Enqueue((input.b, new List<string>() { "start", input.b }));
            foreach (var input in _input.Where(d => d.b == "start")) queue.Enqueue((input.a, new List<string>() { "start", input.a }));

            var paths = 0L;
            while (queue.Count > 0) {
                var path = queue.Dequeue();
                var neighbors = _input.Where(d => d.a == path.next).Union(
                        _input.Where(d => d.b == path.next && d.b != "end").Select(d => (d.b, d.a))
                    );                
                foreach ((string a, string b) neighbor in neighbors)
                {
                    if (neighbor.b == "end")
                    {
                        paths++;                        
                        //Console.WriteLine(string.Join('-', path.visited));
                    } 
                    else if ((neighbor.b == neighbor.b.ToLower() && !path.visited.Contains(neighbor.b)) ||
                        (neighbor.b == neighbor.b.ToUpper()))
                    {
                        var visted = new List<string>(path.visited);
                        visted.Add(neighbor.b);
                        queue.Enqueue((neighbor.b, visted));
                    }
                }
            }

            return new(paths.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var queue = new Queue<(string next, List<string> visited)>();
            foreach (var input in _input.Where(d => d.a == "start")) queue.Enqueue((input.b, new List<string>() { "start", input.b }));
            foreach (var input in _input.Where(d => d.b == "start")) queue.Enqueue((input.a, new List<string>() { "start", input.a }));

            var paths = 0L;
            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var neighbors = _input.Where(d => d.a == path.next).Union(
                        _input.Where(d => d.b == path.next && d.b != "end").Select(d => (d.b, d.a))
                    );
                var has2 = path.visited.Where(d => d == d.ToLower()).GroupBy(d => d).Any(g => g.Count() == 2);
                foreach ((string a, string b) neighbor in neighbors)
                {
                    if (neighbor.b == "start") 
                    {
                        continue;
                    }
                    else if (neighbor.b == "end")
                    {
                        paths++;
                        //Console.WriteLine(string.Join('-', path.visited));
                    }
                    else if ((neighbor.b == neighbor.b.ToLower() && !path.visited.Contains(neighbor.b)) ||
                        (neighbor.b == neighbor.b.ToLower() && path.visited.Count(d => d == neighbor.b) == 1 && !has2) ||
                        (neighbor.b == neighbor.b.ToUpper()))
                    {
                        var visted = new List<string>(path.visited) { neighbor.b };
                        queue.Enqueue((neighbor.b, visted));
                    }
                }
            }

            return new(paths.ToString());
        }
    }
}
