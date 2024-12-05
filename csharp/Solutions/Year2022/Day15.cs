namespace AdventOfCode.Solutions.Year2022;

internal class Day15 : ASolution
{
    private List<Point> _sensors = new();
    private List<Point> _beacons = new();
    private List<int> _distance = new();

    private HashSet<Point> _senSet = new HashSet<Point>();
    private HashSet<Point> _beaSet = new HashSet<Point>();

    private int _maxX;
    private int _maxY;
    private int _minX;
    private int _minY;
    private int _maxD;
    public Day15() : base(15, 2022, "", false)
    {

    }

    protected override void Preprocess()
    {
        foreach (var line in InputByNewLine)
        {
            var a = line.Split(':');
            var m = Regex.Matches(a[0], @"(-?\d+)");
            _sensors.Add(new Point(int.Parse(m[0].Value), int.Parse(m[1].Value)));
            m = Regex.Matches(a[1], @"(-?\d+)");
            _beacons.Add(new Point(int.Parse(m[0].Value), int.Parse(m[1].Value)));
        }
        _distance.AddRange((_sensors.Zip(_beacons)).Select((p) => p.First.Manhatten(p.Second)));

        _minX = _sensors.Concat(_beacons).Min(p => p.X);
        _minY = _sensors.Concat(_beacons).Min(p => p.Y);
        _maxX = _sensors.Concat(_beacons).Max(p => p.X);
        _maxY = _sensors.Concat(_beacons).Max(p => p.Y);
        _maxD = _distance.Max();
        _senSet = new(_sensors);
        _beaSet = new (_beacons);
    }

    protected override object SolvePartOne()
    {
        int y = _useExamples ? 20 : 2_000_000;
        HashSet<Point> points = new HashSet<Point>();
        for (int x = _minX - _maxD; x < _maxX + _maxD; x++)
        {
            var p = new Point(x, y);
            for (int i = 0; i < _sensors.Count; i++)
            {
                Point b = _sensors[i];
                int m = p.Manhatten(b);
                if (m <= _distance[i])
                {
                    points.Add(p);
                    break;
                }
            }
        }

        return points.Except(_beaSet).Count();
    }

    protected override object SolvePartTwo()
    {
        int space = _useExamples ? 20 : 4_000_000;
        HashSet<Point> points = new HashSet<Point>();
        for (int y = 0; y < space; y++)
        {
            for (int x = 0; x < space; x++)
            {
                var p = new Point(x, y);
                bool seen = _sensors.Zip(_distance).Any(pair =>
                {
                    return p.Manhatten(pair.First) <= pair.Second;
                });

                if (!seen)
                {
                    return x * 4000000 + y;
                }
            }
        }

       // DebugPrint(points);

        return null;
    }

    private void DebugPrint(HashSet<Point> points = default)
    {
        const short LabelWidth = 4;
        const short LeftOffset = 0;
        const short TopOffset = 10;
        var consoleRowLabels = Enumerable.Range(0, _maxY + 1).Select(i => $"{i,LabelWidth}").ToArray();

        WriteConsole((short)consoleRowLabels.Length, LabelWidth, LeftOffset, TopOffset, (row, col)
            => (ConsoleColor.White, consoleRowLabels[row][col]));
        WriteConsole(_maxY - _minY, _maxX - _minX, LeftOffset + LabelWidth, TopOffset,
            (row, col) =>
            {
                Point p = new(col + _minX, row + _minY);

                char c = '.';
                if (points?.Contains(p) ?? false)
                {
                    c = '#';
                }

                if (_beacons.Contains(p))
                {
                    c = 'B';
                }
               
                if (_senSet.Contains(p))
                {
                    c = 'S';
                }


                return (ConsoleColor.White, c);
            });
    }

    protected override string LoadDebugInput()
    {
        return """
        Sensor at x=2, y=18: closest beacon is at x=-2, y=15
        Sensor at x=9, y=16: closest beacon is at x=10, y=16
        Sensor at x=13, y=2: closest beacon is at x=15, y=3
        Sensor at x=12, y=14: closest beacon is at x=10, y=16
        Sensor at x=10, y=20: closest beacon is at x=10, y=16
        Sensor at x=14, y=17: closest beacon is at x=10, y=16
        Sensor at x=8, y=7: closest beacon is at x=2, y=10
        Sensor at x=2, y=0: closest beacon is at x=2, y=10
        Sensor at x=0, y=11: closest beacon is at x=2, y=10
        Sensor at x=20, y=14: closest beacon is at x=25, y=17
        Sensor at x=17, y=20: closest beacon is at x=21, y=22
        Sensor at x=16, y=7: closest beacon is at x=15, y=3
        Sensor at x=14, y=3: closest beacon is at x=15, y=3
        Sensor at x=20, y=1: closest beacon is at x=15, y=3
        """;
    }
}
