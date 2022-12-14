using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is Position other && this.Equals(other);

        public bool Equals(Position p) => X == p.X && Y == p.Y;

        public override int GetHashCode() => (X, Y).GetHashCode();

        public static bool operator ==(Position lhs, Position rhs) => lhs.Equals(rhs);

        public static bool operator !=(Position lhs, Position rhs) => !(lhs == rhs);

        public void MoveLeft()
        {
            X--;
        }

        public void MoveRight()
        {
            X++;
        }

        public void MoveUp()
        {
            Y++;
        }

        public void MoveDown()
        {
            Y--;
        }

        public void Follow(Position other)
        {
            int distX = other.X - X;
            int distY = other.Y - Y;

            if (Math.Abs(distX) <= 1 && Math.Abs(distY) <= 1)
            {
                // Stay put
            }
            else if (distX == 0)
            {
                if (distY > 1)
                {
                    Y++;
                }
                else if (distY < -1)
                {
                    Y--;
                }
            }
            else if (distY == 0)
            {
                if (distX > 1)
                {
                    X++;
                }
                else if (distX < -1)
                {
                    X--;
                }
            }
            else if (distX > 1)
            {
                if (distY > 0)
                {
                    X++;
                    Y++;
                }
                else if (distY < 0)
                {
                    X++;
                    Y--;
                }
                else
                {
                    X++;
                }
            }
            else if (distX < -1)
            {
                if (distY > 0)
                {
                    X--;
                    Y++;
                }
                else if (distY < 0)
                {
                    X--;
                    Y--;
                }
                else
                {
                    X--;
                }
            }
            else if (distY > 1)
            {
                if (distX > 0)
                {
                    X++;
                    Y++;
                }
                else if (distX < 0)
                {
                    X--;
                    Y++;
                }
                else
                {
                    Y++;
                }
            }
            else if (distY < -1)
            {
                if (distX > 0)
                {
                    X++;
                    Y--;
                }
                else if (distX < 0)
                {
                    X--;
                    Y--;
                }
                else
                {
                    Y--;
                }
            }
        }

        public int X { get; set; }

        public int Y { get; set; }
    }

    class Program
    {
        static void CountTailPositions(string[] commands)
        {
            var head = new Position(0, 0);
            var tail = new Position(0, 0);

            var uniqueTailPositions = new HashSet<Position>();
            uniqueTailPositions.Add(tail);

            foreach(var command in commands)
            {
                var split = command.Split(' ');
                string direction = split[0];
                int distance = int.Parse(split[1]);

                for (int i = 0; i < distance; ++i)
                {
                    if (direction == "L")
                    {
                        head.MoveLeft();
                    }
                    else if (direction == "R")
                    {
                        head.MoveRight();
                    }
                    else if (direction == "U")
                    {
                        head.MoveUp();
                    }
                    else if (direction == "D")
                    {
                        head.MoveDown();
                    }

                    tail.Follow(head);
                    uniqueTailPositions.Add(tail);
                }
            }

            Console.WriteLine("Unique tail positions with 2 knot rope: {0}", uniqueTailPositions.Count);
        }

        static void CountTailPositions10(string[] commands)
        {
            var head = new Position(0, 0);
            var k1 = new Position(0, 0);
            var k2 = new Position(0, 0);
            var k3 = new Position(0, 0);
            var k4 = new Position(0, 0);
            var k5 = new Position(0, 0);
            var k6 = new Position(0, 0);
            var k7 = new Position(0, 0);
            var k8 = new Position(0, 0);
            var tail = new Position(0, 0);

            var uniqueTailPositions = new HashSet<Position>();
            uniqueTailPositions.Add(tail);

            foreach (var command in commands)
            {
                var split = command.Split(' ');
                string direction = split[0];
                int distance = int.Parse(split[1]);

                for (int i = 0; i < distance; ++i)
                {
                    if (direction == "L")
                    {
                        head.MoveLeft();
                    }
                    else if (direction == "R")
                    {
                        head.MoveRight();
                    }
                    else if (direction == "U")
                    {
                        head.MoveUp();
                    }
                    else if (direction == "D")
                    {
                        head.MoveDown();
                    }

                    k1.Follow(head);
                    k2.Follow(k1);
                    k3.Follow(k2);
                    k4.Follow(k3);
                    k5.Follow(k4);
                    k6.Follow(k5);
                    k7.Follow(k6);
                    k8.Follow(k7);
                    tail.Follow(k8);

                    uniqueTailPositions.Add(tail);
                }
            }

            Console.WriteLine("Unique tail positions with 10 knot rope: {0}", uniqueTailPositions.Count);
        }

        static void Main(string[] args)
        {
            var commands = File.ReadAllLines("Input.txt");
            CountTailPositions(commands);
            CountTailPositions10(commands);
        }
    }
}
