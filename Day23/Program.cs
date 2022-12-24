using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
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

    [DebuggerDisplay("X = {Location.X}, Y = {Location.Y}")]
    public class Elf
    {
        public Elf(Position location)
        {
            Location = location;
        }

        public Position Location { get; set; }
    }

    class Program
    {
        static int North = 0;
        static int South = 1;
        static int West = 2;
        static int East = 3;

        static Position[] Directions = { new Position(0 , -1), new Position(0, 1), new Position(-1, 0), new Position(1, 0) };

        static string[] lines;

        static List<Elf> InitializeElves(string[] lines)
        {
            var elves = new List<Elf>();
            for (int y = 0; y < lines.Length; ++y)
            {
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    char c = lines[y][x];
                    if (c == '#')
                    {
                        elves.Add(new Elf(new Position(x, y)));
                    }
                }
            }

            return elves;
        }

        static bool IsDirectionClear(Elf elf, List<Elf> neighbors, int direction)
        {
            if (direction == North)
            {
                return neighbors.Where(e => e.Location.Y == elf.Location.Y - 1).Count() == 0;
            }
            else if (direction == South)
            {
                return neighbors.Where(e => e.Location.Y == elf.Location.Y + 1).Count() == 0;
            }
            else if (direction == West)
            {
                return neighbors.Where(e => e.Location.X == elf.Location.X - 1).Count() == 0;
            }
            else
            {
                return neighbors.Where(e => e.Location.X == elf.Location.X + 1).Count() == 0;
            }
        }

        static List<Elf> FindNeighbors(Dictionary<Position, Elf> elvesByLocation, Elf elf)
        {
            var neighbors = new List<Elf>();

            for (int x = elf.Location.X - 1; x <= elf.Location.X + 1; ++x)
            {
                for (int y = elf.Location.Y - 1; y <= elf.Location.Y + 1; ++y)
                {
                    if (x != elf.Location.X || y != elf.Location.Y)
                    {
                        Position p = new Position(x, y);
                        if (elvesByLocation.ContainsKey(p))
                        {
                            neighbors.Add(elvesByLocation[p]);
                        }
                    }
                }
            }

            return neighbors;
        }

        static void Simulate()
        {
            var elves = InitializeElves(lines);
            var directions = new Queue<int>(new int[] { North, South, West, East });

            var proposedMoves = new Dictionary<Position, List<Elf>>();
            var elvesByLocation = new Dictionary<Position, Elf>();

            int round = 1;
            while (true)
            {
                elvesByLocation.Clear();
                foreach (var elf in elves)
                {
                    elvesByLocation[elf.Location] = elf;
                }

                foreach (var elf in elves)
                {
                    var neighbors = FindNeighbors(elvesByLocation, elf);
                    if (neighbors.Count > 0)
                    {
                        // Only move if there are neighbors
                        foreach (var direction in directions)
                        {
                            if (IsDirectionClear(elf, neighbors, direction))
                            {
                                Position updated = new Position(elf.Location.X + Directions[direction].X, elf.Location.Y + Directions[direction].Y);
                                if (!proposedMoves.ContainsKey(updated))
                                {
                                    proposedMoves[updated] = new List<Elf>();
                                }

                                proposedMoves[updated].Add(elf);

                                break;
                            }
                        }
                    }
                }

                foreach (var move in proposedMoves)
                {
                    if (move.Value.Count == 1)
                    {
                        move.Value[0].Location = move.Key;
                    }
                }

                if (round == 10)
                {
                    int north = int.MaxValue;
                    int south = int.MinValue;
                    int west = int.MaxValue;
                    int east = int.MinValue;

                    foreach (var elf in elves)
                    {
                        north = Math.Min(north, elf.Location.Y);
                        south = Math.Max(south, elf.Location.Y);
                        west = Math.Min(west, elf.Location.X);
                        east = Math.Max(east, elf.Location.X);
                    }

                    int tiles = (south - north + 1) * (east - west + 1);

                    Console.WriteLine("Part 1: {0}", tiles - elves.Count);
                }

                if (proposedMoves.Count == 0)
                {
                    Console.WriteLine("Part 2: {0}", round);
                    break;
                }

                proposedMoves.Clear();

                // Move the first direction checked to the back
                directions.Enqueue(directions.Dequeue());

                round++;
            }
        }

        static void Main(string[] args)
        {
            lines = File.ReadAllLines("Input.txt");

            Simulate();
        }
    }
}
