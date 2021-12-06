using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_06 : BaseDay
    {
        private readonly int[] _input;

        public Day_06()
        {
            _input = File.ReadAllText(InputFilePath).Split(',').Select(int.Parse).ToArray();
            
        }

        public override ValueTask<string> Solve_1()
        {
            var fish = _input.ToList();
            for (int i = 0; i < 80; i++)
            {
                var newFish = new List<int>();
                for (int j = 0; j < fish.Count; j++)
                {
                    if (fish[j] == 0)
                    {
                        fish[j] = 6;
                        newFish.Add(8);
                    } else
                    {
                        fish[j]--;
                    }
                }
                fish.AddRange(newFish);
            }
            return new(fish.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var fish = new Dictionary<long, long>();
            for (int i = -1; i < 9; i++)
            {
                fish[i] = _input.Count(d => d == i);
            }

            for (int i = 0; i < 256; i++)
            {
                fish[6] += fish[-1];
                fish[8] = fish[-1];
                for (int j = 0; j < 9; j++)
                {
                    fish[j - 1] = fish[j];
                }
            }
            fish[8] = fish[-1]; // hello darkness, my old friend
            return new(fish.Values.Sum().ToString());
        }
    }
}
