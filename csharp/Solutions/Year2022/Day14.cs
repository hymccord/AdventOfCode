using System.Diagnostics;

namespace AdventOfCode.Solutions.Year2022;

internal class Day14 : ASolution
{
    HashSet<Point> _rocks = new();
    Point _start = new(500, 0);
    int _minX;
    int _maxX;
    int _maxY;
    char[,] _cave;

    const char Air = '.';
    const char Rock = '#';
    const char Sand = 'o';

    public Day14() : base(14, 2022, "Regolith Reservoir", false)
    {

    }

    protected override void Preprocess()
    {
        var rockPaths = InputByNewLine
            // get coord pairs
            .Select(l => l.Split(" -> ")
                // coord pair to point
                .SelectMany(p => p.ToIntArray(',').Chunk(2).Select(a => new Point(a[0], a[1])))
            );

        var flatRocks = rockPaths.Flatten();
        _minX = flatRocks.Min(p => p.X);
        _maxX = flatRocks.Max(p => p.X);
        _maxY = flatRocks.Max(p => p.Y);

        foreach (var path in rockPaths)
        {
            foreach (var pair in path.Window(2))
            {
                var (left, right) = (pair[0], pair[1]);
                var (xDiff, yDiff) = (Math.Sign(right.X - left.X), Math.Sign(right.Y - left.Y));
                int numPoints = Math.Max(Math.Abs(right.X - left.X), Math.Abs(right.Y - left.Y)) + 1;

                var xs = Enumerable.Repeat(left.X, numPoints).Select((x, i) => x + i * xDiff);
                var ys = Enumerable.Repeat(left.Y, numPoints).Select((y, i) => y + i * yDiff);
                foreach (var (x, y) in xs.Zip(ys))
                {
                    _rocks.Add(new(x, y));
                }
            }
        }
        InitializeCave();
    }

    protected override object SolvePartOne()
    {
        while (DropSand())
        {
            //DebugPrint();
        }
        DebugPrint();

        return _cave.Flatten().Count(c => c == Sand);
    }

    protected override object SolvePartTwo()
    {
        _maxY += 2;
        _minX = 500 - _maxY;
        _maxX = 500 + _maxY;

        InitializeCave();

        // Change floor to rock
        for (int x = _minX; x <= _maxX; x++)
        {
            SetCaveChar(new Point(x, _maxY), '#');
        }
        
        while (DropSand())
        {
            //DebugPrint();
        }
        DebugPrint();

        return _cave.Flatten().Count(c => c == Sand);
    }

    private bool DropSand()
    {
        if (GetCaveChar(_start) == Sand)
        {
            return false;
        }

        try
        {
            Point p = _start;
            while (true)
            {
                Point s = p.Offset(Point.South);
                char c = GetCaveChar(s);
                if (c == Air)
                {
                    p = s;
                    continue;
                }

                if (c == Sand || c == Rock)
                {
                    Point sw = p.Offset(Point.SouthWest);
                    Point se = p.Offset(Point.SouthEast);

                    c = GetCaveChar(sw);
                    if (c == Air)
                    {
                        p = sw;
                        continue;
                    }

                    c = GetCaveChar(se);
                    if (c == Air)
                    {
                        p = se;
                        continue;
                    }

                    SetCaveChar(p, Sand);
                    break;
                }
            }
        }
        catch
        // easier to catch than check array bounds
        {
            return false;
        }

        return true;
    }

    protected override string LoadDebugInput()
    {
        return """
        498,4 -> 498,6 -> 496,6
        503,4 -> 502,4 -> 502,9 -> 494,9
        """;
    }

    private void DebugPrint()
    {
        const short LabelWidth = 4;
        const short LeftOffset = 0;
        const short TopOffset = 10;
        var consoleRowLabels = Enumerable.Range(0, _maxY + 1).Select(i => $"{i,LabelWidth}").ToArray();

        WriteConsole((short)consoleRowLabels.Length, LabelWidth, LeftOffset, TopOffset, (row, col)
            => (ConsoleColor.White, consoleRowLabels[row][col]));
        WriteConsole(_cave.RowLength(), _cave.ColLength(), LeftOffset + LabelWidth, TopOffset,
            (row, col) =>
            {
                ConsoleColor color;
                switch (_cave[row, col])
                {
                    case Air:
                        color = ConsoleColor.Cyan;
                        break;
                    case '+':
                    case Sand:
                        color = ConsoleColor.Yellow;
                        break;
                    case '~':
                        color = ConsoleColor.Blue;
                        break;
                    case Rock:
                        color = ConsoleColor.DarkRed;
                        break;
                    default:
                        color = ConsoleColor.White;
                        break;
                }

                return (color, _cave[row, col]);
            });
    }

    private void InitializeCave()
    {
        _cave = Enumerable.Repeat<char[]>(
            Enumerable.Repeat('.', _maxX - _minX + 1).ToArray(), _maxY + 1)
            .ToArray()
            .To2D();
        foreach (var rock in _rocks)
        {
            SetCaveChar(rock, '#');
        }
        SetCaveChar(_start, '+');
    }

    private void SetCaveChar(Point p, char c)
    {
        // Need to adjust x
        _cave[p.Y, p.X - _minX] = c;
    }

    private char GetCaveChar(Point p)
    {
        return _cave[p.Y, p.X - _minX];
    }
}
