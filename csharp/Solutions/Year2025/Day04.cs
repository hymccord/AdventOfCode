using MoreLinq;

namespace AdventOfCode.Solutions.Year2025;

internal class Day04 : ASolution
{
    private char[,] _grid;

    public Day04() : base(04, 2025, "Printing Department", true)
    { }

    protected override void Preprocess()
    {
        _grid = Input.To2DCharArray().PadGrid('.');
    }

    protected override object SolvePartOne()
    {
        return _grid.GetPointHashset('@')
            .Count(p => p.SplattNeighbors.Count(n => _grid.At(n) == '@') < 4);
    }

    protected override object SolvePartTwo()
    {

        var points = _grid.GetPointHashset('@');
        var initialCount = points.Count;

        Dictionary<Point, int> numNeighbors = points
            .Select(p =>
                new KeyValuePair<Point, int>(p, p.SplattNeighbors.Count(p => _grid.TryAt(p, '.') == '@'))
            )
            .ToDictionary();

        HashSet<Point> toCheck = numNeighbors
            .Select(kvp => kvp.Key)
            .ToHashSet();

        while (toCheck.Count > 0)
        {
            HashSet<Point> modified = [];
            HashSet<Point> toRemove = [];
            foreach (var point in toCheck)
            {
                if (numNeighbors[point] < 4)
                {
                    toRemove.Add(point);
                }
            }

            foreach (var point in toRemove)
            {
                numNeighbors.Remove(point);
                point.SplattNeighbors.ForEach(n =>
                {
                    if (numNeighbors.TryGetValue(n, out var value) && !toRemove.Contains(n))
                    {
                        numNeighbors[n] = --value;
                        modified.Add(n);
                    }
                });
            }

            toCheck = modified;
        }

        return initialCount - numNeighbors.Count;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                ..@@.@@@@.
                @@@.@.@.@@
                @@@@@.@.@@
                @.@@@@..@.
                @@.@@@@.@@
                .@@@@@@@.@
                .@.@.@.@@@
                @.@@@.@@@@
                .@@@@@@@@.
                @.@.@@@.@.
                """,
                partOne: 13, partTwo: 43)
            ];
    }
}
