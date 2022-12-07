using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    class Program
    {
        static int CalculatePriority(char itemType)
        {
            if (itemType >= 'a' && itemType <= 'z')
            {
                return (itemType - 'a') + 1;
            }
            else if (itemType >= 'A' && itemType <= 'Z')
            {
                return (itemType - 'A') + 27;
            }

            return 0;
        }

        static char FindCommonItemType(string rucksack)
        {
            string compartment1 = rucksack.Substring(0, (rucksack.Length / 2));
            string compartment2 = rucksack.Substring(rucksack.Length / 2);

            foreach (char c in compartment1)
            {
                if (compartment2.Contains(c))
                {
                    return c;
                }
            }

            return '\0';
        }

        static int Part1(string[] rucksacks)
        {
            int sum = 0;
            foreach (var rucksack in rucksacks)
            {
                char itemType = FindCommonItemType(rucksack);
                sum += CalculatePriority(itemType);
            }

            return sum;
        }

        static char FindCommonItemType(string r1, string r2, string r3)
        {
            foreach (char c in r1)
            {
                if (r2.Contains(c) && r3.Contains(c))
                {
                    return c;
                }
            }

            return '\0';
        }

        static int Part2(string[] rucksacks)
        {
            int sum = 0;
            for (int i = 0; i < rucksacks.Length; i += 3)
            {
                char itemType = FindCommonItemType(rucksacks[i], rucksacks[i + 1], rucksacks[i + 2]);
                sum += CalculatePriority(itemType);
            }

            return sum;
        }

        static void Main(string[] args)
        {
            var rucksacks = File.ReadAllLines("Input.txt");

            int sum1 = Part1(rucksacks);
            int sum2 = Part2(rucksacks);

            Console.WriteLine("The answer for part 1 is {0}", sum1);
            Console.WriteLine("The answer for part 2 is {0}", sum2);
        }
    }
}
