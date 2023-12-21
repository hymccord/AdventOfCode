
namespace AdventOfCode.Solutions.Year2023;

internal class Day16 : ASolution
{
    private char[,] _grid;

    public Day16() : base(16, 2023, "The Floor Will Be Lava", false)
    {
    }

    protected override object SolvePartOne()
    {
        _grid = Input.To2DCharArray().PadGrid('@');

        return Energize((0, 1), Point.East);
    }

    protected override object SolvePartTwo()
    {
        var rows = _grid.RowLength();
        var cols = _grid.ColLength();

        // Start from left
        return Enumerable.Max(
            Enumerable.Range(0, rows).Select(i => (new Point(0, i), Point.East))
                .Concat(
            // Start from top
            Enumerable.Range(0, rows).Select(i => (new Point(i, 0), Point.South))
                ).Concat(
            // Start from right
            Enumerable.Range(0, rows).Select(i => (new Point(cols - 1, i), Point.West))
                ).Concat(
            // Start from bottom
            Enumerable.Range(0, rows).Select(i => (new Point(i, rows - 1), Point.North))
            )
        .Select(t => Energize(t.Item1, t.Item2)));
    }

    private int Energize(Point beamStart, Point dirStart)
    {
        var beams = new Queue<(Point, Point)>();
        beams.Enqueue((beamStart, dirStart));

        var visited = new HashSet<Point>();
        var processed = new HashSet<(Point, Point)>();

        if (DebugOutput)
        {
            WriteCharGrid(_grid);
        }

        while (beams.TryDequeue(out var t))
        {
            var (beam, dir) = t;
            var curr = beam + dir;

            if (processed.Contains((beam, dir)))
            {
                continue;
            }

            visited.Add(curr);
            char c = _grid[curr.Y, curr.X];
            switch (c)
            {
                case '/':
                    beams.Enqueue((curr, (-dir.Y, -dir.X)));
                    break;
                case '\\':
                    beams.Enqueue((curr, (dir.Y, dir.X)));
                    break;
                case '|' when dir.Y == 0:
                    beams.Enqueue((curr, Point.North));
                    beams.Enqueue((curr, Point.South));
                    break;
                case '-' when dir.X == 0:
                    beams.Enqueue((curr, Point.East));
                    beams.Enqueue((curr, Point.West));
                    break;
                case '@':
                    visited.Remove(curr);
                    break;
                default:
                    beams.Enqueue((curr, dir));
                    break;
            }

            processed.Add((beam, dir));

            if (c != '.')
            {
                continue;
            }

            if (DebugOutput)
            {
                WriteConsole(curr.Y, curr.X, 15, 5,
                dir switch
                {
                    (0, 1) => 'v',
                    (0, -1) => '^',
                    (1, 0) => '>',
                    (-1, 0) => '<',
                    _ => throw new NotImplementedException(),
                }
                );
            }
        }

        return visited.Count;
    }

    protected override string LoadDebugInput()
    {
        return """
            .|...\....
            |.-.\.....
            .....|-...
            ........|.
            ..........
            .........\
            ..../.\\..
            .-.-/..|..
            .|....-|.\
            ..//.|....
            """;
    }
}
