using MoreLinq;

namespace AdventOfCode.Solutions.Year2024;

internal class Day02 : ASolution
{
    private int[][] _intput;

    public Day02() : base(2, 2024, "Red-Nosed Reports", false)
    {

    }

    protected override object SolvePartOne()
    {
        _intput = InputByNewLine.Select(l => l.ToIntArray()).ToArray();

        int safe = 0;
        foreach (var row in _intput)
        {
            if (IsRowSafe(row))
            {
                safe++;
            }
        }

        return safe;
    }

    protected override object SolvePartTwo()
    {
        int safe = 0;
        foreach (var row in _intput) {
            if (IsRowSafe(row))
            {
                safe++;
            }
            else
            {
                for (int i = 0; i < row.Length; i++)
                {
                    var rowCopy = row.ToList();
                    rowCopy.RemoveAt(i);
                    if (IsRowSafe(rowCopy))
                    {
                        safe++;
                        break;
                    }
                }
            }
        }

        return safe;
    }

    private static bool IsRowSafe(IList<int> row)
    {
        var sign = Math.Sign(row[1] - row[0]);
        return row.Window(2).All(l =>
        {
            var diff = l[1] - l[0];
            var abs = Math.Abs(diff);
            return Math.Sign(diff) == sign && abs >= 1 && abs <= 3;
        });
    }

    protected override string LoadDebugInput()
    {
        return """
        7 6 4 2 1
        1 2 7 8 9
        9 7 6 2 1
        1 3 2 4 5
        8 6 4 4 1
        1 3 6 7 9
        """;
    }
}
