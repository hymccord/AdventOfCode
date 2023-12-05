using System.Collections.Immutable;

using MoreLinq;

namespace AdventOfCode.Solutions.Year2023;

internal class Day04 : ASolution
{
    private List<ImmutableHashSet<int>> _winningNumbers = new();
    private List<ImmutableHashSet<int>> _lottoNumbers = new();

    public Day04() : base(04, 2023, "Scratchcards", false)
    {
    }

    protected override object SolvePartOne()
    {
        ParseInput();

        var total = 0;
        for (int i = 0; i < _winningNumbers.Count; i++)
        {
            var count = _lottoNumbers[i].Intersect(_winningNumbers[i]).Count;

            total += count == 0 ? 0 : (int)Math.Pow(2, count - 1);
        }

        return total;

        // or
        // return _lottoNumbers
        //    .Select((w, i) => w.Intersect(_winningNumbers[i]).Count)
        //    .Where(i => i > 0)
        //    .Sum(i => (int)Math.Pow(2, i - 1));
    }

    protected override object SolvePartTwo()
    {
        long[] longs = Enumerable.Repeat<long>(1L, InputByNewLine.Length).ToArray();

        for (int i = 0; i < _winningNumbers.Count; i++)
        {
            var count = _lottoNumbers[i].Intersect(_winningNumbers[i]).Count;

            for (int j = 1; j <= count; j++)
            {
                longs[i + j] += longs[i];
            }
        }

        return longs.Sum();
    }

    void ParseInput()
    {
        InputByNewLine
            .ForEach(l =>
            {
                (var win, var lotto) = l.SplitInTwo('|');
                _winningNumbers.Add(win.ToIntArray().ToImmutableHashSet());
                _lottoNumbers.Add(lotto.ToIntArray().ToImmutableHashSet());
            });
    }

    protected override string LoadDebugInput()
    {
        return """
        Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
        Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
        Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
        Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
        Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
        """;
    }

}
