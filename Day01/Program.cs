using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> caloriesPerElf = new List<int>();

            int runningTotal = 0;
            var lines = File.ReadAllLines("Input.txt");
            foreach (var line in lines)
            {
                if (line == "")
                {
                    caloriesPerElf.Add(runningTotal);
                    runningTotal = 0;
                }
                else 
                {
                    runningTotal += int.Parse(line);
                }
            }

            Console.WriteLine("Max for one elf: {0}", caloriesPerElf.Max());
            Console.WriteLine("Max for three elves: {0}", caloriesPerElf.OrderByDescending(c => c).Take(3).Sum());
        }
    }
}
