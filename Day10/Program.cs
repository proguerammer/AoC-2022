using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        static void Part1(string[] lines)
        {
            int cycle = 1;
            int x = 1;
            int sum = 0;
            int[] targetCycles = { 20, 60, 100, 140, 180, 220 };

            foreach (var line in lines)
            {
                int executionTime = line == "noop" ? 1 : 2;
                while (executionTime > 0)
                {
                    if (targetCycles.Contains(cycle))
                    {
                        sum += cycle * x;
                    }

                    cycle++;
                    executionTime--;
                }

                if (line.StartsWith("addx"))
                {
                    x += int.Parse(line.Split(' ')[1]);
                }
            }

            Console.WriteLine("The sum of the signal strenghts is {0}", sum);
        }

        static void Part2(string[] lines)
        {
            int cycle = 1;
            int x = 1;
            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                int executionTime = line == "noop" ? 1 : 2;
                while (executionTime > 0)
                {
                    int pixel = (cycle - 1) % 40;
                    bool pixelIsLit = (pixel >= (x - 1) && pixel <= (x + 1));
                    sb.Append(pixelIsLit ? '#' : '.');

                    if (cycle % 40 == 0)
                    {
                        sb.Append('\n');
                    }

                    cycle++;
                    executionTime--;
                }

                if (line.StartsWith("addx"))
                {
                    x += int.Parse(line.Split(' ')[1]);
                }
            }

            Console.WriteLine();
            Console.Write(sb.ToString());
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Input.txt");

            Part1(lines);
            Part2(lines);
        }
    }
}
