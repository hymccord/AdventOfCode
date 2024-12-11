using System.Reflection.Metadata;

namespace AdventOfCode.Solutions.Year2024;

internal class Day08 : ASolution
{
    private char[,] _grid;
    private Dictionary<char, List<Point>> _antennas;

    public Day08() : base(08, 2024, "Resonant Collinearity", true)
    { }

    protected override void Preprocess()
    {
        _antennas = [];
        _grid = Input.To2DCharArray();

        for (int y = 0; y < _grid.RowLength(); y++)
        {
            for (int x = 0; x < _grid.ColLength(); x++)
            {
                char c = _grid[y, x];
                if (c == '.')
                {
                    continue;
                }

                if (_antennas.TryGetValue(c, out var value))
                {
                    value.Add((x, y));
                }
                else
                {
                    _antennas.Add(c, new List<Point>() { (x, y) });
                }
            }
        }
    }

    protected override object SolvePartOne()
    {
        HashSet<Point> antinodes = [];
        foreach ((char c, List<Point> set) in _antennas)
        {
            for (int x = 0; x < set.Count - 1; x++)
            {
                var p1 = set[x];
                for (int y = x + 1; y < set.Count; y++)
                {
                    var p2 = set[y];
                    var diff = p2 - p1;

                    if ((p1 - diff).IsInBoundsOfGrid(_grid))
                    {
                        antinodes.Add(p1 - diff);
                    }

                    if ((p2 + diff).IsInBoundsOfGrid(_grid))
                    {
                        antinodes.Add(p2 + diff);
                    }
                }
            }
        }

        return antinodes.Count;
    }

    protected override object SolvePartTwo()
    {
        var antennas = _grid.GetPointHashset(c => c != '.').ToList();
        HashSet<Point> antinodes = [];
        foreach ((char c, List<Point> set) in _antennas)
        {
            for (int x = 0; x < set.Count - 1; x++)
            {
                var p1 = set[x];
                for (int y = x + 1; y < set.Count; y++)
                {
                    var p2 = set[y];
                    var diff = p2 - p1;

                    var tmp = p1;
                    while (tmp.IsInBoundsOfGrid(_grid))
                    {
                        antinodes.Add(tmp);
                        tmp = tmp - diff;

                    }

                    tmp = p2;
                    while (tmp.IsInBoundsOfGrid(_grid))
                    {
                        antinodes.Add(tmp);
                        tmp = tmp + diff;
                    }
                }
            }
        }

        return antinodes.Count;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                ............
                ........0...
                .....0......
                .......0....
                ....0.......
                ......A.....
                ............
                ............
                ........A...
                .........A..
                ............
                ............
                """, 14, 34)
            ];
    }
}
