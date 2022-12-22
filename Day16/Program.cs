using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public class Valve
    {
        public Valve(string id, int flowRate)
        {
            Id = id;
            FlowRate = flowRate;
            Neighbors = new List<string>();
            DistanceCache = new Dictionary<string, int>();
        }

        public string Id { get; set; }

        public int FlowRate { get; set; }

        public List<string> Neighbors;

        public Dictionary<string, int> DistanceCache;
    }

    class Program
    {
        static void FindPath(Dictionary<string, Valve> valves, Valve start, Valve end, Stack<Valve> path, ref int shortest)
        {
            foreach (var neighbor in start.Neighbors)
            {
                Valve valve = valves[neighbor];
                if (!path.Contains(valve))
                {
                    path.Push(valve);

                    if (valve != end)
                    {
                        FindPath(valves, valve, end, path, ref shortest);
                    }
                    else
                    {
                        shortest = Math.Min(shortest, path.Count);
                    }

                    path.Pop();
                }
            }
        }

        static void FindShortestPath(Dictionary<string, Valve> valves, Valve start, Valve end)
        {
            if (start != end)
            {
                int shortest = int.MaxValue;
                var path = new Stack<Valve>();

                FindPath(valves, start, end, path, ref shortest);

                start.DistanceCache[end.Id] = shortest;
            }
            else
            {
                start.DistanceCache[end.Id] = 0;
            }
        }

        static void CalculatePressure(Valve current, List<Valve> openValves, int time, int currentPressure, ref int mostPressure)
        {
            foreach (var valve in openValves)
            {
                // Add one to account for opening the valve
                int distance = current.DistanceCache[valve.Id] + 1;
                if (distance <= time)
                {
                    time -= distance;
                    currentPressure += time * valve.FlowRate;

                    var remainingValves = new List<Valve>(openValves);
                    remainingValves.Remove(valve);

                    // Need to see if there's a valve we can reach with the time remaining
                    int closestDistance = int.MaxValue;
                    int bestPossibleRemainingPressure = 0;

                    foreach (var r in remainingValves)
                    {
                        var dist = valve.DistanceCache[r.Id] + 1;

                        bestPossibleRemainingPressure += r.FlowRate * Math.Max(time - dist, 0);
                        closestDistance = Math.Min(closestDistance, dist);
                    }

                    // Don't bother if there's no way it could beat the best score
                    bool canExceedMax = (currentPressure + bestPossibleRemainingPressure) > mostPressure;

                    if (remainingValves.Count > 0 && closestDistance <= time && canExceedMax)
                    {
                        CalculatePressure(valve, remainingValves, time, currentPressure, ref mostPressure);
                    }
                    else
                    {
                        mostPressure = Math.Max(currentPressure, mostPressure);
                    }
                    
                    currentPressure -= time * valve.FlowRate;
                    time += distance;
                }
            }
        }

        static void Part1(Dictionary<string, Valve> valves, Valve start, int time)
        {
            int mostPressure = 0;

            var openValves = valves.Values.Where(v => v.FlowRate > 0).ToList();
            CalculatePressure(start, openValves, time, 0, ref mostPressure);

            Console.WriteLine("Part 1: {0}", mostPressure);
        }

        static int CalculateTotalPressure(Stack<Valve> path, int time)
        {
            int pressure = 0;
            var pathArray = path.ToArray().Reverse().ToArray();

            for (int p = 1; p < pathArray.Length; ++p)
            {
                var prev = pathArray[p - 1];
                var curr = pathArray[p];

                int dist = prev.DistanceCache[curr.Id] + 1;
                time -= dist;
                pressure += time * curr.FlowRate;
            }

            return pressure;
        }

        static int CalculateTimeRemaining(Stack<Valve> path, int time)
        {
            var pathArray = path.ToArray().Reverse().ToArray();

            for (int p = 1; p < pathArray.Length; ++p)
            {
                var prev = pathArray[p - 1];
                var curr = pathArray[p];

                time -= prev.DistanceCache[curr.Id] + 1;
            }

            return time;
        }

        static bool AttemptToAdvance(List<Valve> openValves, Valve current, Valve next, Stack<Valve> path, ref int time)
        {
            bool success = false;

            // Add one to account for opening the valve
            int distance = current.DistanceCache[next.Id] + 1;
            if (distance < time)
            {
                time -= distance;

                openValves.Remove(next);
                path.Push(next);

                success = true;
            }

            return success;
        }

        static void PrintPath(string prefix, Stack<Valve> path)
        {
            var pathArray = path.ToArray().Reverse().ToArray();
            Console.Write("{0}: ", prefix);
            for (int p = 0; p < pathArray.Length; ++p)
            {
                Console.Write(pathArray[p].Id);
                if (p < pathArray.Length - 1)
                {
                    Console.Write(", ");
                }
                else
                {
                    Console.Write("\n");
                }
            }
        }

        static void CalculatePressure2(List<Valve> openValves, int initialTime, Valve me, Valve elephant, Stack<Valve> mePath, Stack<Valve> elephantPath, ref int mostPressure)
        {
            for (int i = 0; i < openValves.Count; ++i)
            {
                for (int j = 0; j < openValves.Count; ++j)
                {
                    if (i != j)
                    {
                        var remainingValves = new List<Valve>(openValves);

                        var meNext = openValves[i];
                        int meTime = CalculateTimeRemaining(mePath, initialTime);
                        var meAdvanced = AttemptToAdvance(remainingValves, me, meNext, mePath, ref meTime);

                        var elephantNext = openValves[j];
                        int elephantTime = CalculateTimeRemaining(elephantPath, initialTime);
                        var elephantAdvanced = AttemptToAdvance(remainingValves, elephant, elephantNext, elephantPath, ref elephantTime);

                        // Need to see if there's a valve we can reach with the time remaining
                        int meClosestDistance = int.MaxValue;
                        foreach (var r in remainingValves)
                        {
                            meClosestDistance = Math.Min(meClosestDistance, meNext.DistanceCache[r.Id] + 1);
                        }

                        int elephantClosestDistance = int.MaxValue;
                        foreach (var r in remainingValves)
                        {
                            elephantClosestDistance = Math.Min(elephantClosestDistance, elephantNext.DistanceCache[r.Id] + 1);
                        }

                        // Overly optimistic estimate of the possible pressure remaining
                        int time = (meTime > elephantTime) ? meTime : elephantTime;
                        Valve valve = (meTime > elephantTime) ? meNext : elephantNext;
                        int optimisticPressure = 0;
                        foreach (var r in remainingValves)
                        {
                            var dist = valve.DistanceCache[r.Id] + 1;
                            if (dist < time)
                            {
                                optimisticPressure += r.FlowRate * (time - dist);
                            }
                        }

                        int meCalculatedPressure = CalculateTotalPressure(mePath, initialTime);
                        int elephantCalculatedPressure = CalculateTotalPressure(elephantPath, initialTime);

                        bool shouldBail = (meCalculatedPressure + elephantCalculatedPressure + optimisticPressure) < mostPressure;

                        // Need to check that at least one move is possible and at least one move happened
                        if (remainingValves.Count > 0 && (meClosestDistance <= meTime || elephantClosestDistance <= elephantTime) && remainingValves.Count != openValves.Count && !shouldBail)
                        {
                            CalculatePressure2(remainingValves, initialTime, meNext, elephantNext, mePath, elephantPath, ref mostPressure);
                        }

                        mostPressure = Math.Max(meCalculatedPressure + elephantCalculatedPressure, mostPressure);

                        if (meAdvanced)
                        {
                            mePath.Pop();
                        }

                        if (elephantAdvanced)
                        {
                            elephantPath.Pop();
                        }
                    }
                }
            }
        }

        static void Part2(Dictionary<string, Valve> valves, Valve me, Valve elephant, int time)
        {
            int mostPressure = 0;

            var openValves = valves.Values.Where(v => v.FlowRate > 0).OrderBy(v => valves["AA"].DistanceCache[v.Id]).ToList();
            
            var mePath = new Stack<Valve>();
            mePath.Push(valves["AA"]);

            var elephantPath = new Stack<Valve>();
            elephantPath.Push(valves["AA"]);

            CalculatePressure2(openValves, time, me, elephant, mePath, elephantPath, ref mostPressure);

            Console.WriteLine("Part 2: {0}", mostPressure);
        }

        static void Main(string[] args)
        {
            var valves = new Dictionary<string, Valve>();
            
            var lines = File.ReadAllLines("Input.txt");
            foreach (var line in lines)
            {
                var split = line.Split(';');
                var left = split[0].Replace("Valve ", "").Replace(" has flow rate", "").Split('=');
                
                var valve = new Valve(left[0], int.Parse(left[1]));

                if (split[1].StartsWith(" tunnels"))
                {
                    valve.Neighbors.AddRange(split[1].Replace(" tunnels lead to valves ", "").Replace(" ", "").Split(','));
                }
                else
                {
                    valve.Neighbors.Add(split[1].Replace(" tunnel leads to valve ", ""));
                }

                valves[valve.Id] = valve;
            }

            Valve start = valves["AA"];

            // Cache the shortest distance from each valve to each other valve
            foreach (var valve in valves.Values)
            {
                if (valve == start || valve.FlowRate > 0)
                {
                    foreach (var other in valves.Values)
                    {
                        if (other.FlowRate > 0)
                        {
                            FindShortestPath(valves, valve, other);
                        }
                    }
                }
            }

            Part1(valves, start, 30);
            Part2(valves, start, start, 26);
        }
    }
}
