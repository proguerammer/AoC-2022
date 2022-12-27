using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    [DebuggerDisplay("Offset = {Offset}")]
    public class Shape
    {
        public Shape()
        {
            Tiles = new List<Position>();
        }

        // Represents the bottom left part of the shape
        public Position Offset;

        public List<Position> Tiles;
    }

    class Program
    {
        static string Moves;
        static List<Shape> Shapes;

        static int Floor = 0;
        static int Left = 0;
        static int Right = 8;

        static void InitializeShapes()
        {
            Shapes = new List<Shape>();

            var s1 = new Shape();
            s1.Tiles.AddRange(new Position[] { new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(3, 0) });
            Shapes.Add(s1);

            var s2 = new Shape();
            s2.Tiles.AddRange(new Position[] { new Position(1, 0), new Position(0, 1), new Position(1, 1), new Position(2, 1), new Position(1, 2) });
            Shapes.Add(s2);

            var s3 = new Shape();
            s3.Tiles.AddRange(new Position[] { new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(2, 1), new Position(2, 2) });
            Shapes.Add(s3);

            var s4 = new Shape();
            s4.Tiles.AddRange(new Position[] { new Position(0, 0), new Position(0, 1), new Position(0, 2), new Position(0, 3) });
            Shapes.Add(s4);

            var s5 = new Shape();
            s5.Tiles.AddRange(new Position[] { new Position(0, 0), new Position(1, 0), new Position(0, 1), new Position(1, 1) });
            Shapes.Add(s5);
        }

        static int Simulate(int numRocks)
        {
            var collisionTiles = new List<int[]>();

            int moveIndex = 0;
            for (int i = 0; i < numRocks; ++i)
            {
                var rock = Shapes[i % 5];
                rock.Offset.X = 3;
                rock.Offset.Y = collisionTiles.Count + 4;

                bool stopped = false;
                while (!stopped)
                {
                    // First handle left/right movement
                    int x = Moves[moveIndex % Moves.Length] == '<' ? -1 : 1;

                    int desiredX = rock.Offset.X + x;
                    int calculatedX = desiredX;

                    foreach (var tile in rock.Tiles)
                    {
                        var updated = new Position(desiredX + tile.X, rock.Offset.Y + tile.Y);
                        if (updated.X == Left || updated.X == Right)
                        {
                            // Ran into a wall
                            calculatedX = rock.Offset.X;
                            break;
                        }

                        if (updated.Y <= collisionTiles.Count)
                        {
                            if (collisionTiles[updated.Y - 1][updated.X - 1] == 1)
                            {
                                // Ran into a stopped rock
                                calculatedX = rock.Offset.X;
                                break;
                            }
                        }
                    }

                    // Then handle downward movement
                    int desiredY = rock.Offset.Y - 1;
                    int calculatedY = desiredY;

                    foreach (var tile in rock.Tiles)
                    {
                        var updated = new Position(calculatedX + tile.X, desiredY + tile.Y);
                        if (updated.Y == Floor)
                        {
                            calculatedY = rock.Offset.Y;
                            stopped = true;
                            break;
                        }

                        if (updated.Y <= collisionTiles.Count)
                        {
                            if (collisionTiles[updated.Y - 1][updated.X - 1] == 1)
                            {
                                // Ran into a stopped rock
                                calculatedY = rock.Offset.Y;
                                stopped = true;
                                break;
                            }
                        }
                    }

                    if (stopped)
                    {
                        // If the rock was stopped, then add it to the list of collision tiles
                        var offset = new Position(calculatedX, calculatedY);
                        foreach (var tile in rock.Tiles.OrderBy(t => t.Y))
                        {
                            var worldSpace = new Position(offset.X + tile.X, offset.Y + tile.Y);
                            if (worldSpace.Y > collisionTiles.Count)
                            {
                                collisionTiles.Add(new int[Right - Left - 1]);
                            }

                            collisionTiles[worldSpace.Y - 1][worldSpace.X - 1] = 1;
                        }
                    }
                    else
                    {
                        // Otherwise update the offset and keep simulating
                        rock.Offset.X = calculatedX;
                        rock.Offset.Y = calculatedY;
                    }

                    moveIndex++;
                }
            }

            return collisionTiles.Count;
        }

        static void Part1()
        {
            int height = Simulate(2022);
            Console.WriteLine("Part 1: {0}", height);
        }

        static void Part2()
        {
            // Period repeats every 1750 rocks, starting at 491, increasing height by 2796
            long h1 = Simulate((int)(1000000000000 % 1750));
            long h2 = (1000000000000 / 1750) * 2796;

            Console.WriteLine("Part 2: {0}", h1 + h2);
        }

        static void Main(string[] args)
        {
            Moves = File.ReadAllText("Input.txt");
            InitializeShapes();

            Part1();
            Part2();
        }
    }
}
