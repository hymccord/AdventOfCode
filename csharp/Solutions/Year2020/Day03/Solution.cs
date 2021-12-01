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
        { }

        protected override void Preprocess()
        {
            trees = Input.SplitByNewline().Select(s => s.ToCharArray()).ToArray();
            //trees = test.SplitByNewline().Select(s => s.ToCharArray()).ToArray();
            width = trees[0].Length;
            height = trees.Length;
        }

        protected override object SolvePartOne()
        {
            return Toboggan(new Point(3, 1)).ToString();
        }

        protected override object SolvePartTwo()
        {
            if (DebugOutput)
            {
                WriteConsole(height, width, 0, 0, (row, col) =>
                {
                    return (ConsoleColor.White, trees[row][col]);
                });
            }

            return (Toboggan(new Point(1, 1))
                * Toboggan(new Point(3, 1))
                * Toboggan(new Point(5, 1))
                * Toboggan(new Point(7, 1))
                * Toboggan(new Point(1, 2))).ToString();
        }

        private long Toboggan(Point slope)
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
                    if (DebugOutput)
                        WriteConsole(p.Y, p.X % width, 0, 0, 'X', ConsoleColor.Red);
                    count++;
                }
                else if (DebugOutput)
                {
                    WriteConsole(p.Y, p.X % width, 0, 0, 'O', ConsoleColor.Green);
                }
            }

            return count;
        }
    }
}
