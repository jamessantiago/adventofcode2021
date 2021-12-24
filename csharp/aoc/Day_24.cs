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
    public class Day_24 : BaseDay
    {
        public record struct op(string o, char v, string? v2);

        private readonly List<List<op>> _input = new List<List<op>>();

        public Day_24()
        {
            var lines = File.ReadAllLines(InputFilePath);
            var cur = new List<op>();
            foreach (var line in lines)
            {
                if (line.StartsWith("inp")) {
                    if (cur.Any()) _input.Add(cur);
                    cur = new List<op>();
                }
                var p = line.Split(' ');
                cur.Add(new op(p[0], p[1][0], p.Length > 2 ? p[2] : null));
            }
            _input.Add(cur);
        }

        public long varOrVal(Dictionary<char, long> vars, string var) => vars.ContainsKey(var[0]) ? vars[var[0]] : int.Parse(var);



        public override ValueTask<string> Solve_1()
        {
            var sb = new StringBuilder();
            //_input.Reverse();

            // n      - requires 2-10 - (9)
            // n - 1  - inc
            // n - 2  - inc
            // n - 3  - inc
            // n - 4  - inc
            // n - 5  - dec
            // n - 6  - dec
            // n - 7  - inc
            // n - 8  - inc
            // n - 9  - inc
            // n - 10 - inc
            // n - 11 - dec
            // n - 12 - prod < 6510
            // n - 13 - prod > 30 and < 246
            // n - 14 - prod 1-9

            for (int i = 0; i < 14; i++)
            {
                Console.WriteLine($"OpGroup {i + 1}");
                
                Console.WriteLine($"Div  {_input[i][4].v2}");
                Console.WriteLine($"AddX {_input[i][5].v2}");
                Console.WriteLine($"AddY {_input[i][15].v2}");
                Console.WriteLine();
            }

            for (int i = 9; i > 0; i--)
            {
                for (int testZ = 0; testZ <= 1; testZ++)
                {
                    var vars = new Dictionary<char, long>();
                    vars.Add('w', 0);
                    vars.Add('x', 0);
                    vars.Add('y', 0);
                    vars.Add('z', testZ);
                    foreach (var op in _input[3])
                    {
                        if (op.o == "inp")
                        {
                            vars['w'] = i;
                        }
                        else if (op.o == "add")
                        {
                            vars[op.v] += varOrVal(vars, op.v2);
                        }
                        else if (op.o == "mul")
                        {
                            vars[op.v] *= varOrVal(vars, op.v2);
                        }
                        else if (op.o == "div")
                        {
                            if (varOrVal(vars, op.v2) == 0) { vars['z'] = -1; break; }
                            vars[op.v] /= varOrVal(vars, op.v2);
                        }
                        else if (op.o == "mod")
                        {
                            if (vars[op.v] < 0 || (varOrVal(vars, op.v2) <= 0)) { vars['z'] = -1; break; }
                            vars[op.v] %= varOrVal(vars, op.v2);
                        }
                        else if (op.o == "eql")
                        {
                            vars[op.v] = vars[op.v] == varOrVal(vars, op.v2) ? 1 : 0;
                        }
                    }
                    if (vars['z'] >= 0 && vars['z'] <= 300)
                        Console.WriteLine($"w{i} with z{testZ,-3} gives z{vars['z']}");
                }
            }

            
            return new(sb.ToString());
        }

        public override ValueTask<string> Solve_2()
        {

            return new("");
        }
    }


}

//OpGroup 1
//Div  1
//AddX 13
//AddY 0

// { 0 } ?? do nothing?
// 9
// 2

//OpGroup 2
//Div  1
//AddX 11
//AddY 3

// { 3, 0 }
// 9
//22 7

//OpGroup 3
//Div  1
//AddX 14
//AddY 8

// { 8, 3, 0 }
// 6
// 1

//OpGroup 4
//Div  26
//AddX -5
//AddY 5

// { 3, 0 }
// this == last + 8 - 5
// 9
// 4

//OpGroup 5
//Div  1
//AddX 14
//AddY 13

// { 13, 3, 0 }
// 1
// 1

//OpGroup 6
//Div  1
//AddX 10
//AddY 9

// { 9, 13, 3, 0 }
// 8
// 1

//OpGroup 7
//Div  1
//AddX 12
//AddY 6

// { 6, 9, 13, 3, 0 }
// 9
// 9

//OpGroup 8
//Div  26
//AddX -14
//AddY 1

// { 9, 13, 3, 0 }
// this == last + 6 - 14
// 1
// 1

//OpGroup 9
//Div  26
//AddX -8
//AddY 1

// { 13, 3, 0 }
// this == last + 9 - 8
// 9
// 2

//OpGroup 10
//Div  1
//AddX 13
//AddY 2

// { 2, 13, 3, 0 }
// 7
// 1

//OpGroup 11
//Div  26
//AddX 0
//AddY 7

// { 2, 13, 3, 0 }
// this == last + 2
// 9
// 3

//OpGroup 12
//Div  26
//AddX -5
//AddY 5

// { 13, 3, 0 }
// this == last + 2 - 5
// 9
// 9

//OpGroup 13
//Div  26
//AddX -9
//AddY 8

// { 3, 0 }
// this == last + 13 - 9
// 3
// 1

//OpGroup 14
//Div  26
//AddX -1
//AddY 15

// { 0 }
// this == last + 3 - 1
// 8
// 1