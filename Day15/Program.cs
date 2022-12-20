using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }

    public class SensorBeaconPair
    {
        public SensorBeaconPair(string line)
        {
            var split = line.Split(':');

            var sensorSplit = split[0].Replace("Sensor at ", "").Replace(" ", "").Split(',');
            Sensor.X = int.Parse(sensorSplit[0].Split('=')[1]);
            Sensor.Y = int.Parse(sensorSplit[1].Split('=')[1]);

            var beaconSplit = split[1].Replace(" closest beacon is at ", "").Replace(" ", "").Split(',');
            Beacon.X = int.Parse(beaconSplit[0].Split('=')[1]);
            Beacon.Y = int.Parse(beaconSplit[1].Split('=')[1]);
        }

        public int Distance()
        {
            return Math.Abs(Beacon.X - Sensor.X) + Math.Abs(Beacon.Y - Sensor.Y);
        }

        public Position Sensor;
        public Position Beacon;
    }

    class Program
    {
        static void Part1(List<SensorBeaconPair> pairs)
        {
            var invalidPositions = new Dictionary<int, int>();
            foreach (var pair in pairs)
            {
                int distance = pair.Distance();
                for (int y = pair.Sensor.Y - distance; y <= pair.Sensor.Y + distance; ++y)
                {
                    if (y == 2000000)
                    {
                        int remainingDistance = distance - Math.Abs(pair.Sensor.Y - y);
                        for (int x = pair.Sensor.X - remainingDistance; x <= pair.Sensor.X + remainingDistance; ++x)
                        {
                            if (pairs.FindIndex(p => p.Beacon.X == x && p.Beacon.Y == y) == -1)
                            {
                                invalidPositions[x] = 1;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Part 1: {0}", invalidPositions.Count);
        }

        static long FindTuningFrequency(List<SensorBeaconPair> pairs)
        {
            for (int i = 0; i < pairs.Count; ++i)
            {
                // Check the perimeter against every other sensor
                int distance = pairs[i].Distance() + 1;

                int startY = pairs[i].Sensor.Y - distance;
                int endY = pairs[i].Sensor.Y + distance;

                for (int y = startY; y <= endY; ++y)
                {
                    int remainingDistance = distance - Math.Abs(pairs[i].Sensor.Y - y);

                    int[] values = { pairs[i].Sensor.X - remainingDistance, pairs[i].Sensor.X + remainingDistance };
                    foreach (int x in values)
                    {
                        bool found = false;
                        for (int j = 0; j < pairs.Count; ++j)
                        {
                            int pointDistance = Math.Abs(x - pairs[j].Sensor.X) + Math.Abs(y - pairs[j].Sensor.Y);
                            if (pointDistance <= pairs[j].Distance())
                            {
                                found = true;
                            }
                        }

                        if (!found && x >= 0 && x <= 4000000 && y >= 0 && y <= 4000000)
                        {
                            // I'm feeling too lazy to change all the ints to longs, so just cast here
                            long tuningFrequency = ((long)x * 4000000) + (long)y;
                            return tuningFrequency;
                        }
                    }
                }
            }

            return 0;
        }

        static void Part2(List<SensorBeaconPair> pairs)
        {
            long tuningFrequency = FindTuningFrequency(pairs);
            Console.WriteLine("Part 2: {0}", tuningFrequency);
        }

        static void Main(string[] args)
        {
            var pairs = File.ReadAllLines("Input.txt").Select(x => new SensorBeaconPair(x)).ToList();

            Part1(pairs);
            Part2(pairs);
        }
    }
}
