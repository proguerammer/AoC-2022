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

        public int X;
        public int Y;
        public int Z;
    }

    class Program
    {
        static Vec3 UnitX = new Vec3(1, 0, 0);
        static Vec3 UnitY = new Vec3(0, 1, 0);
        static Vec3 UnitZ = new Vec3(0, 0, 1);

        static void Main(string[] args)
        {
            var cubes = new Dictionary<Vec3, int>();

            var lines = File.ReadAllLines("Input.txt");
            foreach(var line in lines)
            {
                var values = line.Split(',').Select(v => int.Parse(v)).ToArray();
                var coord = new Vec3(values[0], values[1], values[2]);

                cubes.Add(coord, 1);
            }

            int sides = 0;
            foreach(var cube in cubes)
            {
                var neighbors = new Vec3[6] { cube.Key - UnitX, cube.Key + UnitX, cube.Key - UnitY, cube.Key + UnitY, cube.Key - UnitZ, cube.Key + UnitZ };
                sides += 6 - cubes.Keys.Intersect(neighbors).Count();
            }

            Console.WriteLine("Part 1: {0}", sides);
        }
    }
}
