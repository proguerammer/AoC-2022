using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day06
{
    class Program
    {
        static int FindStartOfMarker(string datastream, int numUnique)
        {
            for (int i = 0; i < datastream.Length - numUnique; ++i)
            {
                bool allUnique = true;
                for (int j = i; j < i + numUnique; ++j)
                {
                    for (int k = j + 1; k < i + numUnique; ++k)
                    {
                        allUnique &= datastream[j] != datastream[k];
                    }
                }

                if (allUnique)
                {
                    return i + numUnique;
                }
            }

            return 0;
        }

        static void Main(string[] args)
        {
            string datastream = File.ReadAllText("Input.txt");

            int packetStart = FindStartOfMarker(datastream, 4);
            int messageStart = FindStartOfMarker(datastream, 14);

            Console.WriteLine("Start of first packet: {0}", packetStart);
            Console.WriteLine("Start of message packet: {0}", messageStart);
        }
    }
}
