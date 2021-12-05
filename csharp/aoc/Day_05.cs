using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_05 : BaseDay
    {
        private readonly List<int[]> _input = new List<int[]>();

        public Day_05()
        {
            foreach (var input in File.ReadLines(InputFilePath))
                _input.Add(Regex.Match(input, @"(\d+),(\d+) -> (\d+),(\d+)").Groups.Values.Skip(1).Select(d => int.Parse(d.Value)).ToArray());
            
        }

        public void printGrid(int[,] grid, int x, int y)
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Console.Write(grid[j, i] == 0 ? "." : grid[j, i].ToString());
                }
                Console.WriteLine();
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var vertLines = _input.Where(d => d[0] == d[2]);
            var horizLines = _input.Where(d => d[1] == d[3]);            
            int xmax = Math.Max(_input.Max(d => d[0]), _input.Max(d => d[2])) + 1;
            int ymax = Math.Max(_input.Max(d => d[1]), _input.Max(d => d[3])) + 1;
            var grid = new int[xmax, ymax];

            foreach (var hl in vertLines)
            {
                for (int i = hl[1] < hl[3] ? hl[1] : hl[3]; i <= (hl[1] < hl[3] ? hl[3] : hl[1]); i++)
                {
                    grid[hl[0], i]++;
                }
            }

            foreach (var vl in horizLines)
            {
                for (int i = vl[0] < vl[2] ? vl[0] : vl[2]; i <= (vl[0] < vl[2] ? vl[2] : vl[0]); i++)
                {
                    grid[i, vl[1]]++;
                }
            }

            int count = 0;
            for (int i = 0; i < xmax; i++)
            for (int j = 0; j < ymax; j++)
                {
                    if (grid[i, j] >= 2) count++;
                }

            return new(count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var vertLines = _input.Where(d => d[0] == d[2]);
            var horizLines = _input.Where(d => d[1] == d[3]);
            var otherLines = _input.Except(vertLines).Except(horizLines);

            int xmax = Math.Max(_input.Max(d => d[0]), _input.Max(d => d[2])) + 1;
            int ymax = Math.Max(_input.Max(d => d[1]), _input.Max(d => d[3])) + 1;
            var grid = new int[xmax, ymax];

            foreach (var hl in vertLines)
            {
                for (int i = hl[1] < hl[3] ? hl[1] : hl[3]; i <= (hl[1] < hl[3] ? hl[3] : hl[1]); i++)
                {
                    grid[hl[0], i]++;
                }
            }

            foreach (var vl in horizLines)
            {
                for (int i = vl[0] < vl[2] ? vl[0] : vl[2]; i <= (vl[0] < vl[2] ? vl[2] : vl[0]); i++)
                {
                    grid[i, vl[1]]++;
                }
            }

            foreach (var hl in otherLines)
            {
                bool gor = hl[0] < hl[2];
                bool gou = hl[1] < hl[3];
                int curx = hl[0], cury = hl[1];
                do
                {
                    grid[curx, cury]++;
                    if (gor) curx++; else curx--;
                    if (gou) cury++; else cury--;
                } while (curx != hl[2] && cury != hl[3]);
                grid[curx, cury]++; // (╯‵□′)╯︵┻━┻
                //Console.WriteLine($"Handling: {string.Join(',', hl)}");
                //printGrid(grid, xmax, ymax);
                //Console.WriteLine();
            }

            int count = 0;
            for (int i = 0; i < xmax; i++)
                for (int j = 0; j < ymax; j++)
                {
                    if (grid[i, j] >= 2) count++;
                }

            //printGrid(grid, xmax, ymax);

            return new(count.ToString());
        }
    }
}
