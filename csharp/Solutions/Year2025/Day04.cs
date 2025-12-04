using MoreLinq;

namespace AdventOfCode.Solutions.Year2025;

internal class Day04 : ASolution
{
    private char[,] _grid;

    public Day04() : base(04, 2025, "Printing Department", true)
    { }

    protected override void Preprocess()
    {
        _grid = Input.To2DCharArray();
    }

    protected override object SolvePartOne()
    {
        var points = _grid.GetPointHashset('@');
        var initialCount = points.Count;

        var accessible = points.Where(p => p.SplattNeighbors.Intersect(points).Count() < 4);

        points = points.Except(accessible).ToHashSet();

        return initialCount - points.Count;
    }

    protected override object SolvePartTwo()
    {

        var points = _grid.GetPointHashset('@');
        var initialCount = points.Count;

        while (true)
        {
            var accessible = points.Where(p => p.SplattNeighbors.Intersect(points).Count() < 4);
            if (!accessible.Any())
            {
                break;
            }

            points = points.Except(accessible).ToHashSet();
        }

        return initialCount - points.Count;
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
