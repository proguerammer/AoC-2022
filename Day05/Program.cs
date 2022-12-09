using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    class Program
    {
        static Stack<char>[] CreateStacks()
        {
            var stacks = new Stack<char>[9];
            stacks[0] = new Stack<char>(new char[] { 'Z', 'J', 'G' });
            stacks[1] = new Stack<char>(new char[] { 'Q', 'L', 'R', 'P', 'W', 'F', 'V', 'C' });
            stacks[2] = new Stack<char>(new char[] { 'F', 'P', 'M', 'C', 'L', 'G', 'R' });
            stacks[3] = new Stack<char>(new char[] { 'L', 'F', 'B', 'W', 'P', 'H', 'M' });
            stacks[4] = new Stack<char>(new char[] { 'G', 'C', 'F', 'S', 'V', 'Q' });
            stacks[5] = new Stack<char>(new char[] { 'W', 'H', 'J', 'Z', 'M', 'Q', 'T', 'L' });
            stacks[6] = new Stack<char>(new char[] { 'H', 'F', 'S', 'B', 'V' });
            stacks[7] = new Stack<char>(new char[] { 'F', 'J', 'Z', 'S' });
            stacks[8] = new Stack<char>(new char[] { 'M', 'C', 'D', 'P', 'F', 'H', 'B', 'T' });

            return stacks;
        }

        static void ProcessMoves1(string[] lines)
        {
            var stacks = CreateStacks();
            foreach (var line in lines)
            {
                // Ignore the setup lines
                if (line.StartsWith("move"))
                {
                    string command = line.Replace("move ", "").Replace(" from ", ",").Replace(" to ", ",");
                    string[] parameters = command.Split(',');

                    int count = int.Parse(parameters[0]);
                    int from = int.Parse(parameters[1]) - 1;
                    int to = int.Parse(parameters[2]) - 1;

                    for (int i = 0; i < count; ++i)
                    {
                        stacks[to].Push(stacks[from].Pop());
                    }
                }
            }

            Console.Write("Top crates 1: ");
            for (int i = 0; i < 9; ++i)
            {
                Console.Write(stacks[i].Peek());
            }
            Console.Write('\n');
        }

        static void ProcessMoves2(string[] lines)
        {
            var stacks = CreateStacks();
            var temp = new Stack<char>();
            foreach (var line in lines)
            {
                // Ignore the setup lines
                if (line.StartsWith("move"))
                {
                    string command = line.Replace("move ", "").Replace(" from ", ",").Replace(" to ", ",");
                    string[] parameters = command.Split(',');

                    int count = int.Parse(parameters[0]);
                    int from = int.Parse(parameters[1]) - 1;
                    int to = int.Parse(parameters[2]) - 1;

                    // I'll be lazy and just put to a temp stack, then pop/push that onto the new stack
                    for (int i = 0; i < count; ++i)
                    {
                        temp.Push(stacks[from].Pop());
                    }

                    while (temp.Count > 0)
                    {
                        stacks[to].Push(temp.Pop());
                    }
                }
            }

            Console.Write("Top crates 2: ");
            for (int i = 0; i < 9; ++i)
            {
                Console.Write(stacks[i].Peek());
            }
            Console.Write('\n');
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Input.txt");

            ProcessMoves1(lines);
            ProcessMoves2(lines);
        }
    }
}
