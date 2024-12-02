using MoreLinq;

namespace AdventOfCode.Solutions.Year2024;

internal class Day01 : ASolution
{
    private List<int> _left = [];
    private List<int> _right = [];

    public Day01() : base(1, 2024, "Historian Hysteria", false)
    {

    }

    protected override object SolvePartOne()
    {
        foreach (var line in InputByNewLine)
        {
            if (line.ToIntArray() is [var l, var r])
            {
                _left.Add(l);
                _right.Add(r);
            }
        }

        _left.Sort();
        _right.Sort();
        
        return _left.Zip(_right, (l, r) => Math.Abs(r - l)).Sum();
    }

    protected override object SolvePartTwo()
    {
        var counts = _right.CountBy(x => x).ToDictionary();

        return _left.Sum(l =>
        {
            var r = counts.TryGetValue(l, out var c) ? c : 0;
            return l * r;
        });
    }

    protected override string LoadDebugInput()
    {
        return """
        3   4
        4   3
        2   5
        1   3
        3   9
        3   3
        """;
    }
}
