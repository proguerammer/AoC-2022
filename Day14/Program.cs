using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }

    public class Path
    {
        public List<Point> Points = new List<Point>();
    }

    class Program
    {
        static int[,] FillOutTiles(List<Path> paths, int widest, int deepest)
        {
            int[,] tiles = new int[deepest + 1, widest + 1];

            foreach (var path in paths)
            {
                for (int i = 0; i < path.Points.Count - 1; ++i)
                {
                    var start = path.Points[i];
                    var end = path.Points[i + 1];

                    // Assumes that either X or Y must be equal between points
                    if (start.X == end.X)
                    {
                        int x = start.X;
                        if (start.Y < end.Y)
                        {
                            for (int y = start.Y; y <= end.Y; ++y)
                            {
                                tiles[y, x] = 1;
                            }
                        }
                        else
                        {
                            for (int y = end.Y; y <= start.Y; ++y)
                            {
                                tiles[y, x] = 1;
                            }
                        }
                    }
                    else
                    {
                        int y = start.Y;
                        if (start.X < end.X)
                        {
                            for (int x = start.X; x <= end.X; ++x)
                            {
                                tiles[y, x] = 1;
                            }
                        }
                        else
                        {
                            for (int x = end.X; x <= start.X; ++x)
                            {
                                tiles[y, x] = 1;
                            }
                        }
                    }
                }
            }

            return tiles;
        }

        static void SimulateSand1(List<Path> paths, int widest, int deepest)
        {
            int count = 0;
            var tiles = FillOutTiles(paths, widest, deepest);

            bool fallingIntoAbyss = false;
            while (!fallingIntoAbyss)
            {
                var sand = new Point(500, 0);

                bool simulating = true;
                while (simulating)
                {
                    if (sand.Y >= deepest)
                    {
                        simulating = false;
                        fallingIntoAbyss = true;
                        break;
                    }

                    if (tiles[sand.Y + 1, sand.X] == 0)
                    {
                        sand.Y++;
                    }
                    else if (tiles[sand.Y + 1, sand.X - 1] == 0)
                    {
                        sand.Y++;
                        sand.X--;
                    }
                    else if (tiles[sand.Y + 1, sand.X + 1] == 0)
                    {
                        sand.Y++;
                        sand.X++;
                    }
                    else
                    {
                        tiles[sand.Y, sand.X] = 1;
                        count++;
                        simulating = false;
                    }
                }
            }

            Console.WriteLine("Units of sand before abyss: {0}", count);
        }

        static void SimulateSand2(List<Path> paths, int widest, int deepest)
        {
            int count = 0;
            var tiles = FillOutTiles(paths, widest, deepest + 2);

            // Fill out floor
            for (int i = 0; i < widest; ++i)
            {
                tiles[deepest + 2, i] = 1;
            }

            bool blocked = false;
            while (!blocked)
            {
                var sand = new Point(500, 0);

                bool simulating = true;
                while (simulating)
                {
                    if (tiles[sand.Y + 1, sand.X] == 0)
                    {
                        sand.Y++;
                    }
                    else if (tiles[sand.Y + 1, sand.X - 1] == 0)
                    {
                        sand.Y++;
                        sand.X--;
                    }
                    else if (tiles[sand.Y + 1, sand.X + 1] == 0)
                    {
                        sand.Y++;
                        sand.X++;
                    }
                    else
                    {
                        tiles[sand.Y, sand.X] = 1;
                        count++;
                        simulating = false;

                        if (sand.X == 500 && sand.Y == 0)
                        {
                            blocked = true;
                        }
                    }
                }
            }

            Console.WriteLine("Units of sand before blocked: {0}", count);
        }

        static void Main(string[] args)
        {
            int widest = 0;
            int deepest = 0;
            var paths = new List<Path>();

            string[] separators = new string[1] { " -> " };
            var lines = File.ReadAllLines("Input.txt");
            foreach (var line in lines)
            {
                var path = new Path();
                
                var stringPoints = line.Split(separators, StringSplitOptions.None);
                foreach (var stringPoint in stringPoints)
                {
                    var values = stringPoint.Split(',');
                    
                    var point = new Point(int.Parse(values[0]), int.Parse(values[1]));
                    path.Points.Add(point);

                    // Keep track of the widest and deepest points
                    widest = Math.Max(widest, point.X);
                    deepest = Math.Max(deepest, point.Y);
                }

                paths.Add(path);
            }

            SimulateSand1(paths, widest, deepest);
            SimulateSand2(paths, widest + 200, deepest);
        }
    }
}
