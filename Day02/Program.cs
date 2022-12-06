using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
{
    class Program
    {
        static int CalculateScoreV1(string line)
        {
            switch (line)
            {
                case "A X":
                    return 4;
                case "A Y":
                    return 8;
                case "A Z":
                    return 3;
                case "B X":
                    return 1;
                case "B Y":
                    return 5;
                case "B Z":
                    return 9;
                case "C X":
                    return 7;
                case "C Y":
                    return 2;
                case "C Z":
                    return 6;
                default:
                    return 0;
            }
        }

        static int CalculateScoreV2(string line)
        {
            switch (line)
            {
                case "A X":
                    return 3;
                case "A Y":
                    return 4;
                case "A Z":
                    return 8;
                case "B X":
                    return 1;
                case "B Y":
                    return 5;
                case "B Z":
                    return 9;
                case "C X":
                    return 2;
                case "C Y":
                    return 6;
                case "C Z":
                    return 7;
                default:
                    return 0;
            }
        }

        static void Main(string[] args)
        {
            int totalV1 = 0;
            int totalV2 = 0;

            var lines = File.ReadAllLines("Input.txt");
            foreach (var line in lines)
            {
                totalV1 += CalculateScoreV1(line);
                totalV2 += CalculateScoreV2(line);
            }

            Console.WriteLine("The total score for V1 is {0}", totalV1);
            Console.WriteLine("The total score for V2 is {0}", totalV2);
        }
    }
}
