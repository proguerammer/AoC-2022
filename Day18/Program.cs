using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    [DebuggerDisplay("X = {X}, Y = {Y}, Z={Z}")]
    public struct Vec3
    {
        public Vec3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vec3 operator +(Vec3 a) => a;

        public static Vec3 operator -(Vec3 a) => new Vec3(-a.X, -a.Y, -a.Z);

        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vec3 operator -(Vec3 a, Vec3 b) => a + (-b);

        public static bool operator ==(Vec3 lhs, Vec3 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
        }

        public static bool operator !=(Vec3 lhs, Vec3 rhs) => !(lhs == rhs);

        public int Distance(Vec3 other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
        }

        public int X;
        public int Y;
        public int Z;
    }

    public class PathNode
    {
        public PathNode()
        {
            F = int.MaxValue;
            G = int.MaxValue;
        }

        public Vec3 Location;

        public int F;
        public int G;
    }

    class Program
    {
        static Vec3 UnitX = new Vec3(1, 0, 0);
        static Vec3 UnitY = new Vec3(0, 1, 0);
        static Vec3 UnitZ = new Vec3(0, 0, 1);

        static Vec3[] GetNeighbors(Vec3 cube)
        {
            return new Vec3[6] { cube - UnitX, cube + UnitX, cube - UnitY, cube + UnitY, cube - UnitZ, cube + UnitZ };
        }

        static void Visualize(Dictionary<Vec3, int> cubes)
        {
            int xMin = cubes.OrderBy(c => c.Key.X).First().Key.X;
            int xMax = cubes.OrderByDescending(c => c.Key.X).First().Key.X;

            int yMin = cubes.OrderBy(c => c.Key.Y).First().Key.Y;
            int yMax = cubes.OrderByDescending(c => c.Key.Y).First().Key.Y;

            int zMin = cubes.OrderBy(c => c.Key.Z).First().Key.Z;
            int zMax = cubes.OrderByDescending(c => c.Key.Z).First().Key.Z;

            for (int z = 0; z <= zMax; ++z)
            {
                Console.WriteLine("Z = {0}", z);
                Console.WriteLine();
                int[,] slice = new int[xMax + 1, yMax + 1];
                foreach (var cube in cubes.Where(c => c.Key.Z == z))
                {
                    slice[cube.Key.X, cube.Key.Y] = 1;
                }

                for (int x = 0; x <= xMax; ++x)
                {
                    for (int y = 0; y <= yMax; ++y)
                    {
                        char c = slice[x, y] == 1 ? '#' : '.';
                        Console.Write(c);
                    }

                    Console.Write("\n");
                }

                Console.WriteLine();
            }
        }

        static void Part1(Dictionary<Vec3, int> cubes)
        {
            int sides = 0;
            foreach (var cube in cubes)
            {
                var neighbors = GetNeighbors(cube.Key);
                sides += 6 - cubes.Keys.Intersect(neighbors).Count();
            }

            Console.WriteLine("Part 1: {0}", sides);
        }

        static List<Vec3> ReconstructPath(Dictionary<Vec3, Vec3> cameFrom, Vec3 goal)
        {
            var path = new List<Vec3>();

            var current = goal;
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            return path;
        }

        static bool WithinBounds(Vec3 p)
        {
            return p.X >= -2 && p.X <= 22 && p.Y >= -2 && p.Y <= 22 && p.Z >= -2 && p.Z <= 22;
        }

        static Dictionary<Vec3, Vec3> FindPath(Dictionary<Vec3, int> cubes, Vec3 vStart, Vec3 goal, Dictionary<Vec3, bool> pathCache)
        {
            var cameFrom = new Dictionary<Vec3, Vec3>();
            if (!pathCache.ContainsKey(vStart))
            {
                var map = new Dictionary<Vec3, PathNode>();
                for (int x = -2; x <= 22; ++x)
                {
                    for (int y = -2; y <= 22; ++y)
                    {
                        for (int z = -2; z <= 22; ++z)
                        {
                            var v = new Vec3(x, y, z);
                            var p = new PathNode();
                            p.Location = v;

                            map.Add(v, p);
                        }
                    }
                }

                var start = map[vStart];
                start.G = 0;
                start.F = goal.Distance(vStart);

                var openSet = new List<PathNode>();
                openSet.Add(start);

                while (openSet.Count > 0)
                {
                    PathNode current = openSet.OrderBy(p => p.F).First();
                    openSet.Remove(current);

                    foreach (var vNeighbor in GetNeighbors(current.Location))
                    {
                        if (pathCache.ContainsKey(vNeighbor))
                        {
                            pathCache.Add(vStart, pathCache[vNeighbor]);
                            openSet.Clear();
                            break;
                        }
                        else if (!cubes.ContainsKey(vNeighbor) && WithinBounds(vNeighbor))
                        {
                            var neighbor = map[vNeighbor];

                            int g = current.G + 1;
                            if (g < neighbor.G)
                            {
                                cameFrom[vNeighbor] = current.Location;

                                neighbor.G = g;
                                neighbor.F = g + goal.Distance(vNeighbor);

                                if (!openSet.Contains(neighbor))
                                {
                                    openSet.Add(neighbor);
                                }
                            }
                        }
                    }
                }
            }

            return cameFrom;
        }

        static void Part2(Dictionary<Vec3, int> cubes)
        {
            int sides = 0;

            // Pick a point guaranteed to be outside as the goal
            var goal = new Vec3(22, 22, 22);

            // Cache whether a point has a path to the goal (which means its exterior facing)
            var pathCache = new Dictionary<Vec3, bool>();

            // For each cube, check each neighbor
            foreach (var cube in cubes)
            {
                foreach (var neighbor in GetNeighbors(cube.Key))
                {
                    if (!cubes.ContainsKey(neighbor))
                    {
                        var cameFrom = FindPath(cubes, neighbor, goal, pathCache);
                        if (pathCache.ContainsKey(neighbor))
                        {
                            if (pathCache[neighbor])
                            {
                                sides++;
                            }
                        }
                        else
                        {
                            var path = ReconstructPath(cameFrom, goal);
                            if (path.Count > 0)
                            {
                                // There is a path to the outside, count this one!
                                sides++;

                                // Also mark all the other locations in the path as exterior facing
                                foreach (var p in path)
                                {
                                    if (p != goal)
                                    {
                                        pathCache[p] = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Part 2: {0}", sides);
        }

        static void Main(string[] args)
        {
            var cubes = new Dictionary<Vec3, int>();

            var lines = File.ReadAllLines("Input.txt");
            foreach (var line in lines)
            {
                var values = line.Split(',').Select(v => int.Parse(v)).ToArray();
                var coord = new Vec3(values[0], values[1], values[2]);

                cubes.Add(coord, 1);
            }

            Part1(cubes);
            Part2(cubes);
        }
    }
}
