namespace AdventOfCode.Solutions.Year2020.Day11
{
    class Day11 : ASolution
    {
        private const string Test = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

        private Space[][] _seats;

        public Day11()
            : base(11, 2020, "Seating System")
        {
        }

        protected override object SolvePartOne()
        {
            _seats = Parse(Input);
            int rows = _seats.Length;
            int cols = _seats[0].Length;

            DebugPrint(0, 30);

            bool changed = false;
            do
            {
                changed = false;
                List<Point> swap = new();
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        Point p = new Point(col, row);
                        Space current = _seats[p.Y][p.X];

                        int occupiedNeighbors = 0;
                        foreach (Point offset in p.SplattNeighbors)
                        {
                            if (offset.X < 0 || offset.X >= cols || offset.Y < 0 || offset.Y >= rows)
                                continue;

                            if (_seats[offset.Y][offset.X] == Space.Occupied)
                                occupiedNeighbors++;
                        }

                        switch (current)
                        {
                            case Space.Empty when occupiedNeighbors == 0:
                            case Space.Occupied when occupiedNeighbors >= 4:
                                swap.Add(p);
                                break;
                        }
                    }
                }
                if (swap.Count > 0)
                    changed = true;
                foreach (var p in swap)
                {
                    Space seat = _seats[p.Y][p.X];
                    _seats[p.Y][p.X] = seat switch
                    {
                        Space.Empty => Space.Occupied,
                        Space.Occupied => Space.Empty,
                        _ => seat
                    };
                }

                DebugPrint(0, 30);

            } while (changed);

            return _seats.SelectMany(s => s).Count(s => s == Space.Occupied);
        }

        protected override object SolvePartTwo()
        {
            _seats = Parse(Input);
            int rows = _seats.Length;
            int cols = _seats[0].Length;

            DebugPrint(0, 30);

            bool changed = false;
            do
            {
                changed = false;
                List<Point> swap = new();
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        Point p = new Point(col, row);
                        Space current = _seats[p.Y][p.X];

                        int occupiedNeighbors = 0;
                        foreach (Point slope in Point.Splatt)
                        {
                            Point offset = p + slope;

                            while (offset.X >= 0
                                && offset.X < cols
                                && offset.Y >= 0
                                && offset.Y < rows
                                && _seats[offset.Y][offset.X] == Space.Floor)
                            {
                                offset += slope;
                            }

                            if (offset.X < 0 || offset.X >= cols || offset.Y < 0 || offset.Y >= rows)
                                continue;

                            if (_seats[offset.Y][offset.X] == Space.Occupied)
                                occupiedNeighbors++;
                        }

                        switch (current)
                        {
                            case Space.Empty when occupiedNeighbors == 0:
                            case Space.Occupied when occupiedNeighbors >= 5:
                                swap.Add(p);
                                break;
                        }
                    }
                }
                if (swap.Count > 0)
                    changed = true;
                foreach (var p in swap)
                {
                    Space seat = _seats[p.Y][p.X];
                    _seats[p.Y][p.X] = seat switch
                    {
                        Space.Empty => Space.Occupied,
                        Space.Occupied => Space.Empty,
                        _ => seat
                    };
                }

                DebugPrint(0, 30);

            } while (changed);

            return _seats.SelectMany(s => s).Count(s => s == Space.Occupied);
        }

        private void DebugPrint(short left = 0, short top = 0)
        {
            if (DebugOutput)
            {
                int rows = _seats.Length;
                int cols = _seats[0].Length;

                WriteConsole(rows, cols, left, top, (row, col) =>
                {
                    char c = _seats[row][col] switch
                    {
                        Space.Empty => 'L',
                        Space.Floor => '.',
                        Space.Occupied => '#',
                        _ => throw new NotSupportedException(),
                    };
                    return (ConsoleColor.White, c);
                });
            }
        }

        private static Space[][] Parse(string input)
        {
            return input.SplitByNewline()
                .Select(s => s.ToCharArray().Select(c =>
                {
                    return c switch
                    {
                        '.' => Space.Floor,
                        'L' => Space.Empty,
                        '#' => Space.Occupied,
                        _ => throw new NotSupportedException(),
                    };
                }).ToArray()).ToArray();
        }

        enum Space
        {
            Floor,
            Empty,
            Occupied
        }
    }
}
