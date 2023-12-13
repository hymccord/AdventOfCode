
using MoreLinq;

namespace AdventOfCode.Solutions.Year2023;

internal class Day09 : ASolution
{
    private List<int[]> _dataset = new();

    public Day09() : base(09, 2023, "Mirage Maintenance", false)
    {
    }

    protected override object SolvePartOne()
    {
        _dataset = InputByNewLine.Select(l => l.ToIntArray()).ToList();

        var total = 0;
        foreach (var set in _dataset)
        {
            List<int[]> history = [set];

            for (int i = 0; i < history.Count; i++)
            {
                if (history[i].All(c => c == 0))
                {
                    break;
                }

                history.Add(
                    history[i].Window(2).Select(il => il[1] - il[0]).ToArray()
                );
            }

            total += history.Sum(a => a[^1]);
        }

        return total;
    }

    protected override object SolvePartTwo()
    {
        var total = 0;
        foreach (var set in _dataset)
        {
            List<List<int>> history = [[..set]];

            for (int i = 0; i < history.Count; i++)
            {
                if (history[i].All(c => c == 0))
                {
                    break;
                }

                history.Add(
                    history[i].Window(2).Select(il => il[1] - il[0]).ToList()
                );
            }

            history[^1].Insert(0,0);

            for (int i = history.Count - 1; i > 0; i--)
            {
                history[i - 1].Insert(0, history[i - 1][0] - history[i][0]);
            }

            total += history[0][0];
        }

        return total;
    }

    protected override string LoadDebugInput()
    {
        return """
            0 3 6 9 12 15
            1 3 6 10 15 21
            10 13 16 21 30 45
            """;
    }
}
