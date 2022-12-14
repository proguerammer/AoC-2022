using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    class Program
    {
        static void CountTreesVisibleFromOutside(int[,] forest, int height, int width)
        {
            int[,] visibility = new int[height, width];

            // Outer grid is always visible
            for (int i = 0; i < height; ++i)
            {
                visibility[i, 0] = 1;
                visibility[i, width - 1] = 1;
            }

            for (int i = 0; i < width; ++i)
            {
                visibility[0, i] = 1;
                visibility[height - 1, i] = 1;
            }

            // West to east
            for (int i = 1; i < height - 1; ++i)
            {
                int tallest = forest[i, 0];
                for (int j = 1; j < width - 1; ++j)
                {
                    int tree = forest[i, j];
                    if (tree > tallest)
                    {
                        visibility[i, j] = 1;
                        tallest = tree;
                    }
                }
            }

            // East to west
            for (int i = 1; i < height - 1; ++i)
            {
                int tallest = forest[i, width - 1];
                for (int j = width - 1; j > 0; --j)
                {
                    int tree = forest[i, j];
                    if (tree > tallest)
                    {
                        visibility[i, j] = 1;
                        tallest = tree;
                    }
                }
            }

            // North to South
            for (int i = 1; i < width - 1; ++i)
            {
                int tallest = forest[0, i];
                for (int j = 1; j < height - 1; ++j)
                {
                    int tree = forest[j, i];
                    if (tree > tallest)
                    {
                        visibility[j, i] = 1;
                        tallest = tree;
                    }
                }
            }

            // South to North
            for (int i = 1; i < width - 1; ++i)
            {
                int tallest = forest[height - 1, i];
                for (int j = height - 1; j > 0; --j)
                {
                    int tree = forest[j, i];
                    if (tree > tallest)
                    {
                        visibility[j, i] = 1;
                        tallest = tree;
                    }
                }
            }

            // Count total
            int sum = 0;
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    sum += visibility[i, j];
                }
            }

            Console.WriteLine("Number of trees visible from outside: {0}", sum);
        }

        static void CalculateBestScenicScore(int[,] forest, int height, int width)
        {
            int best = 0;

            for (int i = 1; i < height - 1; ++i)
            {
                for (int j = 1; j < width - 1; ++j)
                {
                    int tree = forest[i, j];

                    // Left
                    int left = 0;
                    for (int l = j - 1; l >= 0; --l)
                    {
                        left++;
                        if (forest[i, l] >= tree)
                        {
                            break;
                        }
                    }

                    // Right
                    int right = 0;
                    for (int r = j + 1; r < width; ++r)
                    {
                        right++;
                        if (forest[i, r] >= tree)
                        {
                            break;
                        }
                    }

                    // Up
                    int up = 0;
                    for (int u = i - 1; u >= 0; --u)
                    {
                        up++;
                        if (forest[u, j] >= tree)
                        {
                            break;
                        }
                    }

                    // Down
                    int down = 0;
                    for (int d = i + 1; d < height; ++d)
                    {
                        down++;
                        if (forest[d, j] >= tree)
                        {
                            break;
                        }
                    }

                    int score = left * right * up * down;
                    best = Math.Max(best, score);
                }
            }

            Console.WriteLine("The best possible scenic score is {0}", best);
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Input.txt");
            int height = lines.Length;
            int width = lines[0].Length;

            int[,] forest = new int[height, width];
            for (int i = 0; i < lines.Length; ++i)
            {
                for (int j = 0; j < lines[i].Length; ++j)
                {
                    forest[i, j] = lines[i][j] - '0';
                }
            }

            CountTreesVisibleFromOutside(forest, height, width);
            CalculateBestScenicScore(forest, height, width);
        }
    }
}
