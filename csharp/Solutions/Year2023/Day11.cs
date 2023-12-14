

using MoreLinq;

namespace AdventOfCode.Solutions.Year2023;

internal class Day11 : ASolution
{
    private HashSet<Point> _galaxies;
    private HashSet<int> _nonGalaxyCols;
    private HashSet<int> _nonGalaxyRows;

    public Day11() : base(11, 2023, "Cosmic Explanation", false)
    {
    }

    protected override object SolvePartOne()
    {
        Initialize();

        return PairwiseDistance(2);
    }

    protected override object SolvePartTwo()
    {
        return PairwiseDistance(1_000_000);
    }

    private object PairwiseDistance(int multiplier)
    {
        HashSet<Point> moved = new();
        foreach (var p in _galaxies)
        {
            var colExpand = Enumerable.Range(0, p.X).Intersect(_nonGalaxyCols).Count();
            var rowExpand = Enumerable.Range(0, p.Y).Intersect(_nonGalaxyRows).Count();

            moved.Add((p.X + (colExpand * (multiplier - 1)), p.Y + (rowExpand * (multiplier - 1))));
        }

        var x = moved.Subsets(2).ToList();

        var total = 0L;
        foreach (var pair in x)
        {
            var g1 = pair[0];
            var g2 = pair[1];

            total += g1.Manhatten(g2);
        }

        return total;
    }

    private void Initialize()
    {
        _galaxies = Input.To2DCharArray().GetPointHashset('#');

        _nonGalaxyCols = Enumerable.Range(0, _galaxies.Max(p => p.X))
            .Except(_galaxies.Select(p => p.X)).ToHashSet();

        _nonGalaxyRows = Enumerable.Range(0, _galaxies.Max(p => p.Y))
            .Except(_galaxies.Select(p => p.Y)).ToHashSet();
    }


    protected override string LoadDebugInput()
    {
        return """
            ...#......
            .......#..
            #.........
            ..........
            ......#...
            .#........
            .........#
            ..........
            .......#..
            #...#.....
            """;
    }
}
