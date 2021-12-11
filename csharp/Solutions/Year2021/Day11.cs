namespace AdventOfCode.Solutions.Year2021
{
    internal class Day11 : ASolution
    {
        int[,] _input;
        int _rows;
        int _cols;

        public Day11() : base(11, 2021, "Dumbo Octopus", false)
        {

        }

        protected override void Preprocess()
        {
            _input = Input.SplitByNewline().Select(s => s.ToCharArray().Select(c => c - '0').ToArray()).ToArray().To2D();
            _rows = _input.GetLength(0);
            _cols = _input.GetLength(1);
        }

        protected override object SolvePartOne()
        {
            int steps = 0;
            int flashes = 0;
            while (steps < 100)
            {
                steps++;
                IncreaseAll();
                flashes += ProcessGrid();
            }

            return flashes;
        }

        protected override object SolvePartTwo()
        {
            Preprocess();

            int steps = 0;
            while (true)
            {
                steps++;
                IncreaseAll();
                ProcessGrid();

                if (_input.Flatten().All(x => x == 0))
                {
                    break;
                }
            }

            return steps;
        }

        private int ProcessGrid()
        {
            int flashes = 0;
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    Point c = new(x, y);
                    if (_input[c.Y, c.X] > 9)
                    {
                        flashes += Flash(c);
                    }
                }
            }

            return flashes;
        }

        private void IncreaseAll()
        {
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    _input[y, x] += 1;
                }
            }
        }

        private int Flash(Point toFlash)
        {
            if (!toFlash.IsInBoundsOfArray(_rows, _cols))
            {
                return 0;
            }

            if (_input[toFlash.Y, toFlash.X] == 0)
            {
                return 0;
            }

            _input[toFlash.Y, toFlash.X]++;

            if (_input[toFlash.Y, toFlash.X] > 9)
            {
                _input[toFlash.Y, toFlash.X] = 0;
                return 1 + toFlash.SplattNeighbors.Select(Flash).Sum();
            }
            
            return 0;
        }

        private void DebugPrint()
        {
            WriteConsole(_rows, _cols, 10, 10, (row, col) =>
            {
                int x = _input[row, col];
                if (x == 0)
                {
                    return (ConsoleColor.Red, '0');
                }
                else if (x < 9)
                {
                    return (ConsoleColor.White, (char)(x + '0'));
                }
                return (ConsoleColor.White, '9');
            });
        }

        protected override string LoadDebugInput()
        {
            return @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";
        }
    }
}
