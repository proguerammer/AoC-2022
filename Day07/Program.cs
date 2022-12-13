using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    public class ElfFile
    {
        public ElfFile(string name, int size)
        {
            Name = name;
            Size = size;
        }

        public int Size;
        public string Name;
    }

    public class ElfDirectory
    {
        public ElfDirectory(string name)
        {
            Subdirectories = new List<ElfDirectory>();
            Files = new List<ElfFile>();
            Name = name;
            Size = 0;
        }

        public ElfDirectory FindSubdirectory(string name)
        {
            return Subdirectories.Find(s => s.Name == name);
        }

        public int ComputeSize()
        {
            Size = Files.Sum(f => f.Size);
            foreach (var subdirectory in Subdirectories)
            {
                Size += subdirectory.ComputeSize();
            }

            return Size;
        }

        public void SumSizesUnderThreshold(ref int sum, int threshold)
        {
            if (Size <= threshold)
            {
                sum += Size;
            }

            foreach (var subdirectory in Subdirectories)
            {
                subdirectory.SumSizesUnderThreshold(ref sum, threshold);
            }
        }

        public void FindSmallestUnderThreshold(ref int smallest, int threshold)
        {
            if (Size >= threshold)
            {
                if (Size < smallest)
                {
                    smallest = Size;
                }

                foreach (var subdirectory in Subdirectories)
                {
                    subdirectory.FindSmallestUnderThreshold(ref smallest, threshold);
                }
            }
        }

        public ElfDirectory Root;
        public ElfDirectory Parent;
        public List<ElfDirectory> Subdirectories;
        public List<ElfFile> Files;
        public string Name;
        public int Size;
    }

    class Program
    {
        static ElfDirectory ParseCommands(string[] commands)
        {
            var root = new ElfDirectory("/");
            root.Root = root;
            root.Parent = root;

            var cwd = root;
            foreach (var command in commands)
            {
                if (command == "$ cd /")
                {
                    cwd = cwd.Root;
                }
                else if (command == "$ cd ..")
                {
                    cwd = cwd.Parent;
                }
                else if (command.StartsWith("$ cd"))
                {
                    string name = command.Split(' ')[2];
                    cwd = cwd.FindSubdirectory(name);
                }
                else if (command == "$ ls")
                {
                    // Do nothing
                }
                else if (command.StartsWith("dir"))
                {
                    string name = command.Split(' ')[1];
                    
                    var directory = new ElfDirectory(name);
                    directory.Root = cwd.Root;
                    directory.Parent = cwd;

                    cwd.Subdirectories.Add(directory);
                }
                else
                {
                    string[] split = command.Split(' ');
                    int size = int.Parse(split[0]);
                    string name = split[1];

                    var file = new ElfFile(name, size);

                    cwd.Files.Add(file);
                }
            }

            return root;
        }

        static void Part1(ElfDirectory root)
        {
            int sum = 0;
            root.SumSizesUnderThreshold(ref sum, 100000);
            Console.WriteLine("Sum of directories under 100000: {0}", sum);
        }

        static void Part2(ElfDirectory root)
        {
            int spaceAvailable = 70000000 - root.Size;
            int spaceNeeded = 30000000 - spaceAvailable;
            int smallestSize = 70000000;

            root.FindSmallestUnderThreshold(ref smallestSize, spaceNeeded);
            Console.WriteLine("The smallest folder with enough space is {0} ({1} needed)", smallestSize, spaceNeeded);
        }

        static void Main(string[] args)
        {
            var commands = File.ReadAllLines("Input.txt");
            
            var root = ParseCommands(commands);
            root.ComputeSize();

            Part1(root);
            Part2(root);
        }
    }
}
