namespace AdventOfCode.Solutions.Year2022;

internal class Day12 : ASolution
{
    private List<Point> _startingPoints = new();
    private Point _end;
    private char[,] _grid;
    private AStar<char> _astar;

    public Day12() : base(12, 2022, "Hill Climbing Algorithm", false)
    { }

    protected override void Preprocess()
    {
        _startingPoints = new();
        for (int y = 0; y < InputByNewLine.Length; y++)
        {
            for (int x = 0; x < InputByNewLine[y].Length; x++)
            {
                char c = InputByNewLine[y][x];
                if (c == 'a' || c == 'S')
                {
                    _startingPoints.Add(new Point(x, y));
                }
                else if (c == 'E')
                {
                    _end = new Point(x, y);
                }
            }
        }

        _grid = InputByNewLine.Select(s => s.ToCharArray()).ToArray().To2D();
        _grid[_startingPoints[0].Y, _startingPoints[0].X] = 'a';
        _grid[_end.Y, _end.X] = 'z';
        _astar = new(new CharGrid(_grid));
    }

    protected override object SolvePartOne()
    {
        return _astar.Dijkstra(_startingPoints[0], _end).Count - 1;
    }

    protected override object SolvePartTwo()
    {
        return _startingPoints.Min(p =>
        {
            try
            {
                return _astar.Dijkstra(p, _end).Count - 1;
            }
            catch (Exception)
            {
                return int.MaxValue;
            }
        });
    }

    protected override string LoadDebugInput()
    {
        return """
        Sabqponm
        abcryxxl
        accszExk
        acctuvwj
        abdefghi
        """;
    }

    private void DebugPrint(List<Point> result)
    {
        WriteConsole(_grid.RowLength(), _grid.ColLength(), 10, 10, (row, col) =>
        {
            return (ConsoleColor.White, _grid[row, col]);
        });

        WriteConsole(_grid.RowLength(), _grid.ColLength(), 20, 10, (row, col) =>
        {
            var index = result.IndexOf(new Point(col, row));
            if (index > -1 && index < result.Count - 1)
            {
                var next = result[index + 1];
                int x = Math.Sign(next.X - col);
                int y = Math.Sign(next.Y - row);

                return (x, y) switch
                {
                    (1, 0) => (ConsoleColor.White, '>'),
                    (0, 1) => (ConsoleColor.White, 'v'),
                    (-1, 0) => (ConsoleColor.White, '<'),
                    (0, -1) => (ConsoleColor.White, '^'),
                    _ => throw new NotImplementedException(),
                };
            }

            return (ConsoleColor.White, '.');
        });
    }

    private class CharGrid : WeightedGrid<char>
    {
        public CharGrid(char[,] grid)
            : base(grid)
        { }

        internal override double Cost(Point from, Point to)
        {
            if (_grid[to.Y, to.X] - _grid[from.Y, from.X] > 1)
            {
                return double.PositiveInfinity;
            }

            return 1;
        }
    }
}
