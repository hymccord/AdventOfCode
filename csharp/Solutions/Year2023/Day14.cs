

namespace AdventOfCode.Solutions.Year2023;

internal class Day14 : ASolution
{
    private char[,] _grid;
    private HashSet<Point> _rocks;

    public Day14() : base(14, 2023, "Parabolic Reflector Dish", false)
    {
    }

    protected override object SolvePartOne()
    {
        _grid = Input.To2DCharArray().PadGrid('#');
        _rocks = _grid.GetPointHashset('O');

        Tilt(Point.North);
        var grade = Grade();

        return grade;
    }

    protected override object SolvePartTwo()
    {
        Dictionary<int, int> hash = new();

        bool skipping = false;
        for (var i = 0; i < 1_000_000_000; i++)
        {
            Tilt(Point.North);
            Tilt(Point.West);
            Tilt(Point.South);
            Tilt(Point.East);

            var code = _grid.GetSimpleString().GetHashCode();

            if (skipping)
            {
                continue;
            }

            if (hash.ContainsKey(code))
            {
                var cycle = i - hash[code];

                i += ((1_000_000_000 - i) / cycle) * cycle;
                skipping = true;
            }
            else
            {
                hash.Add(code, i);
            }
        }

        return Grade();
    }

    private void Tilt(Point direction)
    {
        IOrderedEnumerable<Point> order = direction switch
        {
            (0, -1) => _rocks.OrderBy(p => p.Y).ThenBy(p => p.X),
            (0, 1) => _rocks.OrderByDescending(p => p.Y).ThenBy(p => p.X),
            (1, 0) => _rocks.OrderByDescending(p => p.X).ThenBy(p => p.Y),
            (-1, 0) => _rocks.OrderBy(p => p.X).ThenBy(p => p.Y),
            _=> throw new NotImplementedException(),
        };
        var queue = new Queue<Point>(order);
        _rocks.Clear();

        foreach (Point current in queue)
        {
            var p = current;
            while (_grid[p.Y + direction.Y, p.X + direction.X] == '.')
            {
                _grid[p.Y, p.X] = '.';
                _grid[p.Y + direction.Y, p.X + direction.X] = 'O';
                p += direction;

                //WriteCharGrid(_grid);
            }

            _rocks.Add(p);
        }
    }

    private int Grade()
    {
        var cols = _grid.RowLength();
        return _rocks.Sum(p => cols - p.Y - 1);
    }

    protected override string LoadDebugInput()
    {
        return """
            O....#....
            O.OO#....#
            .....##...
            OO.#O....O
            .O.....O#.
            O.#..O.#.#
            ..O..#O..O
            .......O..
            #....###..
            #OO..#....
            """;
    }
}
