namespace AdventOfCode.Solutions.Year2025;

internal class Day05 : ASolution
{
    private List<long[]> _fresh;
    private long[] _available;

    public Day05() : base(05, 2025, "Cafeteria", true)
    { }

    protected override void Preprocess()
    {
        var arr = Input.SplitByBlankLine();
        _fresh = arr[0].SplitByNewline().Select(s => s.ToLongArray('-')).ToList();
        _available = arr[1].ToLongArray();
        CombineRanges();
    }

    protected override object SolvePartOne()
    {
        return _available.Count(a => _fresh.Any(f => a >= f[0] && a <= f[1]));
    }

    protected override object SolvePartTwo()
    {
        return _fresh.Sum(f => f[1] - f[0] + 1);
    }

    private void CombineRanges()
    {
        for (int i = 0; i < _fresh.Count - 1; i++)
        {
            long a = _fresh[i][0];
            long b = _fresh[i][1];
            for (int j = i + 1; j < _fresh.Count; j++)
            {
                long c = _fresh[j][0];
                long d = _fresh[j][1];

                if (a <= d && b >= c)
                {
                    _fresh.RemoveAt(j);
                    _fresh[i][0] = Math.Min(a, c);
                    _fresh[i][1] = Math.Max(b, d);
                    i = -1; break;
                }
            }
        }
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                3-5
                10-14
                16-20
                12-18

                1
                5
                8
                11
                17
                32
                """,
                partOne: 3, partTwo: 14),
            ];
    }
}
