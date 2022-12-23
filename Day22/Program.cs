using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
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

    class Program
    {
        static string[] lines;
        static Dictionary<Position, char> map;
        static List<string> commands;
        static Position[] direction;

        static int Right = 0;
        static int Down = 1;
        static int Left = 2;
        static int Up = 3;

        static void ParseMap()
        {
            map = new Dictionary<Position, char>();
            for (int y = 0; y < lines.Length - 2; ++y)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; ++x)
                {
                    char c = line[x];
                    if (c == '.' || c == '#')
                    {
                        map[new Position(x, y)] = c;
                    }
                }
            }

            direction = new Position[4];
            direction[Right] = new Position(1, 0);
            direction[Down] = new Position(0, 1);
            direction[Left] = new Position(-1, 0);
            direction[Up] = new Position(0, -1);
        }

        static void ParseCommands()
        {
            commands = new List<string>();

            var sb = new StringBuilder();
            foreach (var c in lines[lines.Length - 1])
            {
                if (c >= '0' && c <= '9')
                {
                    sb.Append(c);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        commands.Add(sb.ToString());
                        sb.Clear();
                    }

                    commands.Add(string.Format("{0}", c));
                }
            }

            // Handle trailing number
            if (sb.Length > 0)
            {
                commands.Add(sb.ToString());
                sb.Clear();
            }
        }

        static Position Wrap(Position next, int facing)
        {
            if (facing == Right)
            {
                return map.Keys.Where(p => p.Y == next.Y).OrderBy(p => p.X).First();
            }
            else if (facing == Down)
            {
                return map.Keys.Where(p => p.X == next.X).OrderBy(p => p.Y).First();
            }
            else if (facing == Left)
            {
                return map.Keys.Where(p => p.Y == next.Y).OrderByDescending(p => p.X).First();
            }
            else
            {
                return map.Keys.Where(p => p.X == next.X).OrderByDescending(p => p.Y).First();
            }
        }

        static void Move(ref Position current, int facing, int distance)
        {
            while (distance > 0)
            {
                Position next = current;
                next.X += direction[facing].X;
                next.Y += direction[facing].Y;

                if (!map.ContainsKey(next))
                {
                    next = Wrap(next, facing);
                }

                if (map[next] == '.')
                {
                    current = next;
                    distance--;
                }
                else if (map[next] == '#')
                {
                    break;
                }
            }
        }

        static Position WrapCube(Position next, ref int facing)
        {
            // Layout: 12
            //         3
            //        45
            //        6

            Position wrapped = new Position();

            // Hard code given the layout
            if (next.Y < 0 && next.X >= 50 && next.X < 100)
            {
                // Top edge of 1, going to the left edge of 6 and facing right
                wrapped.X = 0;
                wrapped.Y = 100 + next.X;
                facing = Right;
            }
            else if (next.X < 50 && next.Y >= 0 && next.Y < 50)
            {
                // Left edge of 1, going to the left edge of 4 and facing right
                wrapped.X = 0;
                wrapped.Y = 149 - next.Y;
                facing = Right;
            }
            else if (next.Y < 0 && next.X >= 100 && next.X < 150)
            {
                // Top edge of 2, going to the bottom edge of 6 and facing up
                wrapped.X = next.X - 100;
                wrapped.Y = 199;
                facing = Up;
            }
            else if (next.X >= 150 && next.Y >= 0 && next.Y < 50)
            {
                // Right edge of 2, going to the right edge of 5 and facing left
                wrapped.X = 99;
                wrapped.Y = 149 - next.Y;
                facing = Left;
            }
            else if (next.Y >= 50 && next.X >= 100 && next.X < 150 && facing == Down)
            {
                // Bottom edge of 2, going to the right edge of 3 and facing left
                wrapped.X = 99;
                wrapped.Y = next.X - 50;
                facing = Left;
            }
            else if (next.X >= 100 && next.Y >= 50 && next.Y < 100 && facing == Right)
            {
                // Right edge of 3, going to the bottom edge of 2 and facing up
                wrapped.X = next.Y + 50;
                wrapped.Y = 49;
                facing = Up;
            }
            else if (next.X < 50 && next.Y >= 50 && next.Y < 100 && facing == Left)
            {
                // Left edge of 3, going to the top edge of 4 and facing down
                wrapped.X = next.Y - 50;
                wrapped.Y = 100;
                facing = Down;
            }
            else if (next.X < 0 && next.Y >= 100 && next.Y < 150)
            {
                // Left edge of 4, going to the left edge of 1 and facing right
                wrapped.X = 50;
                wrapped.Y = 149 - next.Y;
                facing = Right;
            }
            else if (next.Y < 100 && next.X >= 0 && next.X < 50 && facing == Up)
            {
                // Top edge of 4, going to the left edge of 3 and facing right
                wrapped.X = 50;
                wrapped.Y = 50 + next.X;
                facing = Right;
            }
            else if (next.X >= 100 && next.Y >= 100 && next.Y < 150)
            {
                // Right edge of 5, going to the right edge of 2 and facing left
                wrapped.X = 149;
                wrapped.Y = 149 - next.Y;
                facing = Left;
            }
            else if (next.Y >= 150 && next.X >= 50 && next.X < 100 && facing == Down)
            {
                // Bottom edge of 5, going to the right edge of 6 and facing left
                wrapped.X = 49;
                wrapped.Y = 100 + next.X;
                facing = Left;
            }
            else if (next.X >= 50 && next.Y >= 150 && next.Y < 200 && facing == Right)
            {
                // Right edge of 6, going to the bottom edge of 5 and facing up
                wrapped.X = next.Y - 100;
                wrapped.Y = 149;
                facing = Up;
            }
            else if (next.Y >= 200 && next.X >= 0 && next.X < 50)
            {
                // Bottom edge of 6, going to the top edge of 2 and facing down
                wrapped.X = 100 + next.X;
                wrapped.Y = 0;
                facing = Down;
            }
            else if (next.X < 0 && next.Y >= 150 && next.Y < 200)
            {
                // Left edge of 6, going to the top edge of 1 and facing down
                wrapped.X = next.Y - 100;
                wrapped.Y = 0;
                facing = Down;
            }

            return wrapped;
        }

        static void MoveCube(ref Position current, ref int facing, int distance)
        {
            while (distance > 0)
            {
                Position next = current;
                next.X += direction[facing].X;
                next.Y += direction[facing].Y;

                int updatedFacing = facing;

                if (!map.ContainsKey(next))
                {
                    next = WrapCube(next, ref updatedFacing);
                }

                if (map[next] == '.')
                {
                    current = next;
                    facing = updatedFacing;
                    distance--;
                }
                else if (map[next] == '#')
                {
                    break;
                }
            }
        }

        static int CalculatePassword(int x, int y, int facing)
        {
            return 1000 * (y + 1) + 4 * (x + 1) + facing;
        }

        static void Part1()
        {
            var current = map.Keys.First();
            int facing = Right;

            foreach (var command in commands)
            {
                int move = 0;
                if (int.TryParse(command, out move))
                {
                    Move(ref current, facing, move);
                }
                else if (command == "R")
                {
                    facing = (facing + 1) % 4;
                }
                else if (command == "L")
                {
                    facing = (facing + 3) % 4;
                }
            }

            int password = CalculatePassword(current.X, current.Y, facing);
            Console.WriteLine("Part 1: {0}", password);
        }

        static void Part2()
        {
            var current = map.Keys.First();
            int facing = Right;

            foreach (var command in commands)
            {
                int move = 0;
                if (int.TryParse(command, out move))
                {
                    MoveCube(ref current, ref facing, move);
                }
                else if (command == "R")
                {
                    facing = (facing + 1) % 4;
                }
                else if (command == "L")
                {
                    facing = (facing + 3) % 4;
                }
            }

            int password = CalculatePassword(current.X, current.Y, facing);
            Console.WriteLine("Part 2: {0}", password);
        }

        static void Main(string[] args)
        {
            lines = File.ReadAllLines("Input.txt");

            ParseMap();
            ParseCommands();

            Part1();
            Part2();
        }
    }
}
