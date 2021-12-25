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
    public class Day_25 : BaseDay
    {
        private readonly char[,] _input;

        public Day_25()
        {
            var lines = File.ReadAllLines(InputFilePath);
            _input = new char[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
                for (int x = 0; x < lines[0].Length; x++)
                    _input[x, y] = lines[y][x] == '.' ? '\0' : lines[y][x];
        }

        public void print(char[,] grid)
        {

            for (int y = 0; y < grid.YLen(); y++) {
                for (int x = 0; x < grid.XLen(); x++)
                {
                    Console.Write(grid[x, y] == '\0' ? '.' : grid[x, y]);
                }
                Console.WriteLine();
            }
        }

        public void clear(char[,] grid)
        {
            for (int y = 0; y < grid.YLen(); y++)
            {
                for (int x = 0; x < grid.XLen(); x++)
                {
                    if (grid[x, y] == 'x') grid[x, y] = '\0';
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var steps = 0;
            var grid = (char[,])_input.Clone();
            while (true)
            {
                var moved = false;
                steps++;
                var newGrid = new char[grid.XLen(), grid.YLen()];
                for (int y = 0; y < grid.YLen(); y++)
                    for (int x= 0; x < grid.XLen(); x++)
                    {
                        if (grid[x, y] == '>' && grid[(x + 1) % grid.XLen(), y] == '\0') {
                            grid[x, y] = 'x';
                            grid[(x + 1) % grid.XLen(), y] = '>';
                            x++; moved = true;
                        }
                    }
                clear(grid);

                for (int x = 0; x < grid.XLen(); x++)
                    for (int y = 0; y < grid.YLen(); y++)
                    {
                        if (grid[x, y] == 'v' && grid[x, (y + 1) % grid.YLen()] == '\0')
                        {
                            grid[x, y] = 'x';
                            grid[x, (y + 1) % grid.YLen()] = 'v';
                            y++; moved = true;
                        }
                    }
                clear(grid);

                //print(grid);
                //Console.WriteLine();
                if (!moved) break;
            }
            
            return new(steps.ToString());
        }

        public override ValueTask<string> Solve_2()
        {

            return new("");
        }
    }


}