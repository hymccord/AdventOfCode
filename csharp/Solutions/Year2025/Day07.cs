namespace AdventOfCode.Solutions.Year2025;

internal class Day07 : ASolution
{
    private char[,] _grid;
    private Point _manifold;
    private HashSet<Point> _splitters;

    public Day07() : base(07, 2025, "Laboratories", true)
    { }

    protected override void Preprocess()
    {
        _grid = Input.To2DCharArray();
        _manifold = _grid.GetPointHashset('S').Single();
        _splitters = _grid.GetPointHashset('^');
    }

    protected override object SolvePartOne()
    {
        Queue<Point> sources = [];
        sources.Enqueue(_manifold);

        while (sources.Count > 0)
        {
            var s = sources.Dequeue();
            var down = s + Point.South;

            if (_grid.TryAt(down, out char c))
            {
                if (c == '.')
                {
                    sources.Enqueue(down);
                    _grid[down.Y, down.X] = '|';
                }

                if (c == '^')
                {
                    var sw = s + Point.SouthWest;
                    var se = s + Point.SouthEast;
                    sources.Enqueue(sw);
                    sources.Enqueue(se);
                    _grid[sw.Y, sw.X] = '|';
                    _grid[se.Y, se.X] = '|';
                }
            }

            //WriteCharGrid(_grid);
        }

        return _splitters.Count(p =>
        {
            return _grid.At(p + Point.North) == '|';
        });
    }

    protected override object SolvePartTwo()
    {
        _grid = Input.To2DCharArray();
        Dictionary<Point, long> cache = [];

        return Sim(_manifold + Point.South);

        long Sim(Point p)
        {
            if (cache.TryGetValue(p, out var s))
            {
                return s; 
            }

            if (!_grid.TryAt(p, out char c))
            {
                return 1;
            }

            if (c == '.')
            {
                return Sim(p + Point.South);
            }

            var sw = p + Point.SouthWest;
            var se = p + Point.SouthEast;

            cache[sw] = Sim(sw);
            cache[se] = Sim(se);

            return cache[sw] + cache[se];
        }
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                .......S.......
                ...............
                .......^.......
                ...............
                ......^.^......
                ...............
                .....^.^.^.....
                ...............
                ....^.^...^....
                ...............
                ...^.^...^.^...
                ...............
                ..^...^.....^..
                ...............
                .^.^.^.^.^...^.
                ...............
                """,
                partOne: 21, partTwo: 40),
                        new ("""
                ..S..
                .....
                ..^..
                .....
                .^.^.
                .....
                """,
                partOne: 3, partTwo: 4)
            ];
    }
}
