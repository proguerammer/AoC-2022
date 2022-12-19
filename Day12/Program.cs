using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(int x, int y, int f, int g, char value)
        {
            X = x;
            Y = y;
            F = f;
            G = g;
            Value = value;
        }

        public int GetHeight()
        {
            if (Value == 'S')
            {
                return 0;
            }
            else if (Value == 'E')
            {
                return 'z' - 'a';
            }
            else
            {
                return Value - 'a';
            }
        }

        public int CalculateDistance(Point other)
        {
            return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
        }

        public int X;
        public int Y;
        public int F;
        public int G;
        public char Value;
    }

    class Program
    {
        static List<Point> GetNeighbors(Point[,] map, Point node, int width, int height)
        {
            var neighbors = new List<Point>();

            // Left
            if ((node.X - 1 >= 0) && (map[node.X - 1, node.Y].GetHeight() - node.GetHeight() <= 1))
            {
                neighbors.Add(map[node.X - 1, node.Y]);
            }

            // Right
            if ((node.X + 1 < width) && (map[node.X + 1, node.Y].GetHeight() - node.GetHeight() <= 1))
            {
                neighbors.Add(map[node.X + 1, node.Y]);
            }

            // Down
            if ((node.Y - 1 >= 0) && (map[node.X, node.Y - 1].GetHeight() - node.GetHeight() <= 1))
            {
                neighbors.Add(map[node.X, node.Y - 1]);
            }

            // Up
            if ((node.Y + 1 < height) && (map[node.X, node.Y + 1].GetHeight() - node.GetHeight() <= 1))
            {
                neighbors.Add(map[node.X, node.Y + 1]);
            }

            return neighbors;
        }

        static List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point goal)
        {
            var path = new List<Point>();
            var current = goal;
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            return path;
        }

        static int FindShortestPath(Point[,] map, int width, int height, Point startPos, Point goal)
        {
            // Grab from the map because this might just have been used to pass position information
            Point start = map[startPos.X, startPos.Y];

            var cameFrom = new Dictionary<Point, Point>();

            start.G = 0;
            start.F = goal.CalculateDistance(start);

            var openSet = new List<Point>();
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                Point current = openSet.OrderBy(p => p.F).First();
                openSet.Remove(current);

                foreach (var neighbor in GetNeighbors(map, current, width, height))
                {
                    int g = current.G + 1;
                    if (g < neighbor.G)
                    {
                        cameFrom[neighbor] = current;

                        neighbor.G = g;
                        neighbor.F = g + goal.CalculateDistance(neighbor);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            var path = ReconstructPath(cameFrom, goal);
            if (path.Count > 0)
            {
                return path.Count;
            }
            else
            {
                // Failure
                return int.MaxValue;
            }
        }

        static Point[,] CreateMap(string[] lines, int width, int height)
        {
            Point[,] map = new Point[width, height];
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    map[x, y] = new Point(x, y, int.MaxValue, int.MaxValue, lines[y][x]);
                }
            }

            return map;
        }

        static void Part1(string[] lines)
        {
            int width = lines[0].Length;
            int height = lines.Length;

            Point[,] map = CreateMap(lines, width, height);
            var start = map.Cast<Point>().Where(p => p.Value == 'S').First();
            var goal = map.Cast<Point>().Where(p => p.Value == 'E').First();

            int steps = FindShortestPath(map, width, height, start, goal);
            Console.WriteLine("Part 1: {0}", steps);
        }

        static void Part2(string[] lines)
        {
            int width = lines[0].Length;
            int height = lines.Length;

            int shortest = int.MaxValue;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    char c = lines[y][x];
                    if (c == 'a' || c == 'S')
                    {
                        Point[,] map = CreateMap(lines, width, height);

                        var start = new Point(x, y);
                        var goal = map.Cast<Point>().Where(p => p.Value == 'E').First();

                        int steps = FindShortestPath(map, width, height, start, goal);
                        shortest = Math.Min(shortest, steps);
                    }
                }
            }

            Console.WriteLine("Part 2: {0}", shortest);
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Input.txt");

            Part1(lines);
            Part2(lines);
        }
    }
}
