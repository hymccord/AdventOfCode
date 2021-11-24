
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2018
{
    internal class Day18 : ASolution
    {
        //string[] input = SplitInput(tinput).ToArray();
        char[,] yard;

        public Day18() : base(18, 2018)
        {

        }

        protected override object SolvePartOne()
        {
            InitBoard(Input.SplitByNewline());

            //WriteConsole(input.Length, input.Length, 5, 5, (y, x) => (ConsoleColor.White, yard[y, x]));

            for (int i = 0; i < 10; i++)
            {
                yard = Advance(yard);
                //WriteConsole(input.Length, input.Length, 5, 5, (y, x) => (ConsoleColor.White, yard[y, x]));
            }

            int wood = yard.Cast<char>().Count(c => c == '|');
            int lumber = yard.Cast<char>().Count(c => c == '#');

            return wood * lumber;
        }

        protected override object SolvePartTwo()
        {
            InitBoard(Input.SplitByNewline());

            //WriteConsole(input.Length, input.Length, 5, 5, (y, x) => (ConsoleColor.White, yard[y, x]));

            StringBuilder sb = new StringBuilder();
            Dictionary<string, int> gens = new Dictionary<string, int>();
            bool skipped = false;
            for (int i = 0; i < 1_000_000_000; i++)
            {
                yard = Advance(yard);
                //WriteConsole(input.Length, input.Length, 5, 5, (y, x) => (ConsoleColor.White, yard[y, x]));

                for (int y = 0; y < 50; y++)
                {
                    for (int x = 0; x < 50; x++)
                    {
                        sb.Append(yard[y, x]);
                    }
                }
                string sYard = sb.ToString();
                if (!skipped && gens.ContainsKey(sYard))
                {
                    int id = gens[sYard];
                    int diff = i - id;

                    i += (int)(1e9 - i) / diff * diff;
                    skipped = true;
                }

                if (!skipped)
                {
                    gens.Add(sYard, i);
                }

                sb.Clear();
            }



            int wood = yard.Cast<char>().Count(c => c == '|');
            int lumber = yard.Cast<char>().Count(c => c == '#');

            return wood * lumber;
        }

        private void InitBoard(string[] input)
        {
            yard = new char[input.GetLength(0), input.GetLength(0)];

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input.Length; x++)
                {
                    yard[y, x] = input[y][x];
                }
            }
        }

        private char[,] Advance(char[,] yard)
        {
            char[,] newYard = new char[yard.GetLength(0), yard.GetLength(1)];

            for (int y = 0; y < yard.GetLength(1); y++)
            {
                for (int x = 0; x < yard.GetLength(0); x++)
                {
                    (int open, int tree, int lumber) = GetSurroundings(yard, x, y);

                    switch (yard[y, x])
                    {

                        case '.':
                            newYard[y, x] = tree >= 3 ? '|' : '.';
                            break;
                        case '#':
                            newYard[y, x] = lumber >= 1 && tree >= 1 ? '#' : '.';
                            break;
                        case '|':
                            newYard[y, x] = lumber >= 3 ? '#' : '|';
                            break;
                    }
                }
            }

            return newYard;
        }

        private (int open, int tree, int lumber) GetSurroundings(char[,] yard, int x, int y)
        {
            int open = 0;
            int tree = 0;
            int lumber = 0;

            foreach (Point point in Point.Splatt)
            {
                Point offset = new Point(x, y) + point;
                if (offset.X < 0 || offset.X >= yard.GetLength(0) || offset.Y < 0 || offset.Y >= yard.GetLength(1))
                    continue;

                switch (yard[offset.Y, offset.X])
                {
                    case '.':
                        open++;
                        break;
                    case '#':
                        lumber++;
                        break;
                    case '|':
                        tree++;
                        break;
                }
            }

            return (open, tree, lumber);
        }

        private int Score()
        {
            int wood = yard.Cast<char>().Count(c => c == '|');
            int lumber = yard.Cast<char>().Count(c => c == '#');

            return wood * lumber;
        }

        static string tinput =
    @"
.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.
";
    }

}

