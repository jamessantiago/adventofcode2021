using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_04 : BaseDay
    {
        private readonly List<string> _input;
        private readonly int[] _callout;
        private readonly List<int[][]> _boards = new List<int[][]>();

        public Day_04()
        {
            _input = File.ReadLines(InputFilePath).ToList();
            _callout = _input[0].Split(',').Select(int.Parse).ToArray();

            for (int i = 2; i < _input.Count; i += 6)
            {
                var board = new int[5][];
                int itr = 0;
                for (int j = i; j < i + 5; j++)
                {
                    board[itr++] = _input[j].Trim().Split(' ').Where(d => !string.IsNullOrWhiteSpace(d)).Select(int.Parse).ToArray();
                }
                _boards.Add(board);
            }
        }

        public bool winner(int[][] board)
        {
            for (int i = 0; i < 5; i++) {
                if (board[i].Sum() == 0)
                    return true;
                if (board.Select(d => d[i]).Sum() == 0)
                    return true;
            }

            return false;
                
        }

        public override ValueTask<string> Solve_1()
        {
            var wc = 0;
            foreach (var callout in _callout)
            {
                foreach (var board in _boards)
                {
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 5; j++)
                            if (board[i][j] == callout) board[i][j] = 0;
                }
                if (_boards.Any(d => winner(d))) { wc = callout; break; }
            }

            var wb = _boards.Single(d => winner(d));
            var sum = 0;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    sum += wb[i][j];

            return new((sum * wc).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var wc = 0;
            var lastWinners = new List<int[][]>();
            var newWinners = new List<int[][]>();
            foreach (var callout in _callout)
            {
                foreach (var board in _boards)
                {
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 5; j++)
                            if (board[i][j] == callout) board[i][j] = 0;
                }
                newWinners = _boards.Where(d => winner(d)).ToList();
                if (newWinners.Count() == _boards.Count) { wc = callout; break; }
                lastWinners = _boards.Where(d => winner(d)).ToList();
            }

            var wb = newWinners.Except(lastWinners).Single();
            var sum = 0;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    sum += wb[i][j];

            return new((sum * wc).ToString());
        }
    }
}
