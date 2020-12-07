using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020
{
    class Day03 : ASolution
    {
        private char[][] trees;
        int width;
        int height;

#pragma warning disable IDE0051 // Remove unused private members
        private readonly string test = @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#"
#pragma warning restore IDE0051 // Remove unused private members
;

        public Day03() : base(3, 2020, "Toboggan Trajectory")
        {
            trees = Input.SplitByNewline().Select(s => s.ToCharArray()).ToArray();
            //trees = test.SplitByNewline().Select(s => s.ToCharArray()).ToArray();
            width = trees[0].Length;
            height = trees.Length;
        }

        protected override string SolvePartOne()
        {
            return Toboggan(new Point(3, 1)).ToString();
        }

        protected override string SolvePartTwo()
        {
            WriteConsole(height, width, 0, 0, (row, col) =>
            {
                return (ConsoleColor.White, trees[row][col]);
            });

            return (Toboggan(new Point(1, 1))
                * Toboggan(new Point(3, 1))
                * Toboggan(new Point(5, 1))
                * Toboggan(new Point(7, 1))
                * Toboggan(new Point(1, 2), true)).ToString();
        }

        private long Toboggan(Point slope, bool print = false)
        {
            Point p = new Point(0, 0);
            long count = 0;
            while (true)
            {
                p += slope;

                if (p.Y >= height)
                    break;

                if (trees[p.Y][p.X % width] == '#')
                {
                    if (print)
                        WriteConsole(p.Y, p.X % width, 0, 0, 'X', ConsoleColor.Red);
                    count++;
                }
                else if (print)
                {
                    WriteConsole(p.Y, p.X % width, 0, 0, 'O', ConsoleColor.Green);
                }
            }

            return count;
        }
    }
}
