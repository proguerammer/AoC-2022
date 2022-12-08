using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day04
{
    class Program
    {
        static int CountFullyContainedRanges(string[] lines)
        {
            int count = 0;

            foreach (var line in lines)
            {
                string[] ranges = line.Split(',');
                string[] r1 = ranges[0].Split('-');
                string[] r2 = ranges[1].Split('-');

                int s1 = int.Parse(r1[0]);
                int e1 = int.Parse(r1[1]);

                int s2 = int.Parse(r2[0]);
                int e2 = int.Parse(r2[1]);

                if(((s1 <= s2 && s2 <= e1) && (s1 <= e2 && e2 <= e1)) || ((s2 <= s1 && s1 <= e2) && (s2 <= e1 && e1 <= e2)))
                {
                    count++;
                }
            }

            return count;
        }

        static int CountOverlappingRanges(string[] lines)
        {
            int count = 0;

            foreach (var line in lines)
            {
                string[] ranges = line.Split(',');
                string[] r1 = ranges[0].Split('-');
                string[] r2 = ranges[1].Split('-');

                int s1 = int.Parse(r1[0]);
                int e1 = int.Parse(r1[1]);

                int s2 = int.Parse(r2[0]);
                int e2 = int.Parse(r2[1]);

                if ((e1 >= s2 && s1 <= s2) || (e2 >= s1 && s2 <= s1))
                {
                    count++;
                }
            }

            return count;
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Input.txt");

            int fullyContainedCount = CountFullyContainedRanges(lines);
            int overlappingCount = CountOverlappingRanges(lines);

            Console.WriteLine("Fully contained ranges: {0}", fullyContainedCount);
            Console.WriteLine("Overlapping ranges: {0}", overlappingCount);
        }
    }
}
