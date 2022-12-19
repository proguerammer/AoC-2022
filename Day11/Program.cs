using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        static void Part1(string[] input)
        {
            int linesPerMonkey = 6;
            int numMonkeys = input.Length / linesPerMonkey;

            var monkeyItems = new List<int>[numMonkeys];
            var monkeyOperations = new string[numMonkeys];
            var monkeyTests = new int[numMonkeys];
            var monkeyTrueTargets = new int[numMonkeys];
            var monkeyFalseTargets = new int[numMonkeys];
            var monkeyInpections = new int[numMonkeys];

            for (int i = 0; i < numMonkeys; ++i)
            {
                monkeyItems[i] = input[i * linesPerMonkey + 1].Replace("  Starting items: ", "").Replace(" ", "").Split(',').Select(s => int.Parse(s)).ToList();
                monkeyOperations[i] = input[i * linesPerMonkey + 2].Replace("  Operation: new = ", "");
                monkeyTests[i] = int.Parse(input[i * linesPerMonkey + 3].Replace("  Test: divisible by ", ""));
                monkeyTrueTargets[i] = int.Parse(input[i * linesPerMonkey + 4].Replace("    If true: throw to monkey ", ""));
                monkeyFalseTargets[i] = int.Parse(input[i * linesPerMonkey + 5].Replace("    If false: throw to monkey ", ""));
            }

            for (int i = 0; i < 20; ++i)
            {
                for (int m = 0; m < numMonkeys; ++m)
                {
                    foreach (var item in monkeyItems[m])
                    {
                        string operation = monkeyOperations[m].Replace("old", item.ToString());
                        string[] split = operation.Split(' ');

                        int updated = 0;
                        if (split[1] == "+")
                        {
                            updated = int.Parse(split[0]) + int.Parse(split[2]);
                        }
                        else if (split[1] == "*")
                        {
                            updated = int.Parse(split[0]) * int.Parse(split[2]);
                        }

                        updated /= 3;

                        if (updated % monkeyTests[m] == 0)
                        {
                            monkeyItems[monkeyTrueTargets[m]].Add(updated);
                        }
                        else
                        {
                            monkeyItems[monkeyFalseTargets[m]].Add(updated);
                        }

                        monkeyInpections[m]++;
                    }

                    monkeyItems[m].Clear();
                }
            }

            var sortedInspections = monkeyInpections.OrderByDescending(i => i).ToArray();
            long monkeyBusiness = sortedInspections[0] * sortedInspections[1];

            Console.WriteLine("Part 1: {0}", monkeyBusiness);
        }

        public struct Equation
        {
            public Equation(object left)
            {
                Left = left;
                Operation = "+";
                Right = 0;

                Cache = new Dictionary<int, int>();
            }

            public Equation(object left, string operation, object right)
            {
                Left = left;
                Operation = operation;
                Right = right;

                Cache = new Dictionary<int, int>();
            }

            public int CalculateModulo(int modulo)
            {
                if (Cache.ContainsKey(modulo))
                {
                    return Cache[modulo];
                }

                int left = (Left is int) ? (int)Left % modulo : ((Equation)Left).CalculateModulo(modulo);
                int right = (Right is int) ? (int)Right % modulo : ((Equation)Right).CalculateModulo(modulo);

                if (Operation == "+")
                {
                    // Modular addition
                    // (A + B) mod C = (A mod C + B mod C) mod C
                    int result = (left + right) % modulo;
                    Cache.Add(modulo, result);

                    return result;
                }
                else
                {
                    // Modular multiplication
                    // (A * B) mod C = (A mod C * B mod C) mod C
                    int result = (left * right) % modulo;
                    Cache.Add(modulo, result);

                    return result;
                }
            }

            public object Left;
            public string Operation;
            public object Right;

            public Dictionary<int, int> Cache;
        }

        static void Part2(string[] input)
        {
            int linesPerMonkey = 6;
            int numMonkeys = input.Length / linesPerMonkey;

            var monkeyItems = new List<Equation>[numMonkeys];
            var monkeyOperations = new string[numMonkeys];
            var monkeyTests = new int[numMonkeys];
            var monkeyTrueTargets = new int[numMonkeys];
            var monkeyFalseTargets = new int[numMonkeys];
            var monkeyInpections = new long[numMonkeys];

            for (int i = 0; i < numMonkeys; ++i)
            {
                monkeyItems[i] = input[i * linesPerMonkey + 1].Replace("  Starting items: ", "").Replace(" ", "").Split(',').Select(s => new Equation(int.Parse(s))).ToList();
                monkeyOperations[i] = input[i * linesPerMonkey + 2].Replace("  Operation: new = ", "");
                monkeyTests[i] = int.Parse(input[i * linesPerMonkey + 3].Replace("  Test: divisible by ", ""));
                monkeyTrueTargets[i] = int.Parse(input[i * linesPerMonkey + 4].Replace("    If true: throw to monkey ", ""));
                monkeyFalseTargets[i] = int.Parse(input[i * linesPerMonkey + 5].Replace("    If false: throw to monkey ", ""));
            }

            int[] debugRounds = { 1, 20, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 };

            for (int i = 0; i < 10000; ++i)
            {
                for (int m = 0; m < numMonkeys; ++m)
                {
                    foreach (var item in monkeyItems[m])
                    {
                        string[] split = monkeyOperations[m].Split(' ');
                        if (split[2] == "old")
                        {
                            var newItem = new Equation(item, split[1], item);
                            if (newItem.CalculateModulo(monkeyTests[m]) == 0)
                            {
                                monkeyItems[monkeyTrueTargets[m]].Add((newItem));
                            }
                            else
                            {
                                monkeyItems[monkeyFalseTargets[m]].Add(newItem);
                            }
                        }
                        else
                        {
                            var newItem = new Equation(item, split[1], int.Parse(split[2]));
                            if (newItem.CalculateModulo(monkeyTests[m]) == 0)
                            {
                                monkeyItems[monkeyTrueTargets[m]].Add((newItem));
                            }
                            else
                            {
                                monkeyItems[monkeyFalseTargets[m]].Add(newItem);
                            }
                        }

                        monkeyInpections[m]++;
                    }

                    monkeyItems[m].Clear();
                }
            }

            var sortedInspections = monkeyInpections.OrderByDescending(i => i).ToArray();
            long monkeyBusiness = sortedInspections[0] * sortedInspections[1];

            Console.WriteLine("Part 2: {0}", monkeyBusiness);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input.txt").Where(l => l != "").ToArray();

            Part1(input);
            Part2(input);
        }
    }
}
