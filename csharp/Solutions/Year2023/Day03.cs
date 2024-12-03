using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace AdventOfCode.Solutions.Year2023;

internal class Day03 : ASolution
{
    private char[,] _grid;
    private int _rows;
    private int _cols;

    public Day03() : base(03, 2023, "Gear Ratios", false)
    {

    }

    protected override object SolvePartOne()
    {
        _grid = InputByNewLine.Select(l => l.ToCharArray()).ToArray().To2D();
        _rows = _grid.RowLength();
        _cols = _grid.ColLength();

        long sum = 0;

        for (int y = 0; y < _rows; y++)
        {
            bool hasSymbolNeighbor = false;
            bool parsingNumber = false;
            var sb = new StringBuilder();
            for (int x =  0; x < _cols; x++)
            {
                Point p = new Point(x, y);
                char c = _grid[y, x];

                bool isDigit = char.IsAsciiDigit(_grid[y, x]);
                if (!isDigit)
                {
                    Check();
                    continue;
                }

                parsingNumber = true;
                sb.Append(c);

                if (!hasSymbolNeighbor)
                {
                    hasSymbolNeighbor = p.SplattNeighbors.Any(p => p.IsInBoundsOfArray(_rows, _cols) && !char.IsAsciiDigit(_grid[p.Y, p.X]) && _grid[p.Y, p.X] != '.');
                }
            }
            Check();

            void Check()
            {
                if (parsingNumber && hasSymbolNeighbor)
                {
                    sum += int.Parse(sb.ToString());
                }

                sb.Clear();
                parsingNumber = false;
                hasSymbolNeighbor = false;
            }
        }

        return sum;
    }

    protected override object SolvePartTwo()
    {
        Dictionary<Point, List<long>> _gears = new();

        for (int y = 0; y < _rows; y++)
        {
            bool hasSymbolNeighbor = false;
            bool parsingNumber = false;
            Point gearPoint = new();
            var sb = new StringBuilder();
            for (int x = 0; x < _cols; x++)
            {
                Point p = new Point(x, y);
                char c = _grid[y, x];

                bool isDigit = char.IsAsciiDigit(_grid[y, x]);
                if (!isDigit)
                {
                    Check();
                    continue;
                }

                parsingNumber = true;
                sb.Append(c);

                if (!hasSymbolNeighbor)
                {
                    hasSymbolNeighbor = p.SplattNeighbors.Any(p =>
                    {
                        if (!p.IsInBoundsOfArray(_rows, _cols))
                        {
                            return false;
                        }
                        char pc = _grid[p.Y, p.X];
                        if (pc == '*')
                        {
                            gearPoint = p;
                            return true;
                        }

                        return false;
                    });
                }
            }
            Check();

            void Check()
            {
                if (parsingNumber && hasSymbolNeighbor)
                {
                    if (!_gears.ContainsKey(gearPoint))
                    {
                        _gears[gearPoint] = new List<long>();
                    }

                    _gears[gearPoint].Add(long.Parse(sb.ToString()));
                }

                sb.Clear();
                parsingNumber = false;
                hasSymbolNeighbor = false;
            }
        }

        return _gears.Where(kvp => kvp.Value.Count == 2).Sum(kvp => kvp.Value[0] * kvp.Value[1]);
    }

    protected override string LoadDebugInput()
    {
        return """
        467..114..
        ...*......
        ..35..633.
        ......#...
        617*......
        .....+.58.
        ..592.....
        ......755.
        ...$.*....
        .664.598..
        """;
    }
}
