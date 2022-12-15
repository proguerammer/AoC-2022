using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Program
    {
        static List<object> ParseList(string line)
        {
            var stack = new Stack<List<object>>();
            
            List<object> topLevelList = null;
            var currentString = new StringBuilder();

            foreach (var c in line)
            {
                if (c == '[')
                {
                    var list = new List<object>();

                    // Need to add to the list on the stack before adding it on there
                    if (stack.Count > 0)
                    {
                        stack.Peek().Add(list);
                    }

                    stack.Push(list);

                    // Track this for returning later
                    if (topLevelList == null)
                    {
                        topLevelList = list;
                    }
                }
                else if (c == ']' || c == ',')
                {
                    if (currentString.Length > 0)
                    {
                        stack.Peek().Add(int.Parse(currentString.ToString()));
                        currentString.Clear();
                    }

                    if (c == ']')
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    currentString.Append(c);
                }
            }

            return topLevelList;
        }

        static int CompareLists(List<object> left, List<object> right)
        {
            for (int i = 0; i < left.Count; ++i)
            {
                if (i < right.Count)
                {
                    int compare = CompareObjects(left[i], right[i]);
                    if (compare != 0)
                    {
                        return compare;
                    }
                }
            }

            if (left.Count < right.Count)
            {
                return -1;
            }
            else if (left.Count > right.Count)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        static int CompareInts(int left, int right)
        {
            if (left < right)
            {
                return -1;
            }
            else if (left == right)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        static int CompareObjects(object left, object right)
        {
            if (left is List<object> && right is List<object>)
            {
                return CompareLists(left as List<object>, right as List<object>);
            }
            else if (left is int && right is int)
            {
                return CompareInts((int)left, (int)right);
            }
            else if (left is List<object> && right is int)
            {
                return CompareLists(left as List<object>, new List<object>() { right });
            }
            else
            {
                return CompareLists(new List<object>() { left }, right as List<object>);
            }
        }

        static void Part1(List<Tuple<List<Object>, List<Object>>> pairs)
        {
            int sum = 0;
            for (int i = 0; i < pairs.Count; ++i)
            {
                if (CompareObjects(pairs[i].Item1, pairs[i].Item2) == -1)
                {
                    sum += (i + 1);
                }
            }

            Console.WriteLine("The sum of the indices of correctly ordered pairs is {0}", sum);
        }

        static void Part2(List<string> lines)
        {
            // Remove blank lines
            lines = lines.Where(l => l != "").ToList();

            // Add divider packets
            lines.Add("[[2]]");
            lines.Add("[[6]]");

            lines.Sort((x, y) => CompareObjects(ParseList(x), ParseList(y)));

            int start = lines.FindIndex(s => s == "[[2]]") + 1;
            int end = lines.FindIndex(s => s == "[[6]]") + 1;

            Console.WriteLine("The product of the indices is {0}", start * end);
        }

        static void Main(string[] args)
        {
            var pairs = new List<Tuple<List<Object>, List<Object>>>();

            var lines = File.ReadAllLines("Input.txt").Where(s => s != "").ToArray();
            for (int i = 0; i < lines.Length; i += 2)
            {
                var first = ParseList(lines[i]);
                var second = ParseList(lines[i + 1]);

                var pair = new Tuple<List<object>, List<object>>(first, second);

                pairs.Add(pair);
            }

            Part1(pairs);
            Part2(lines.ToList());
        }
    }
}
