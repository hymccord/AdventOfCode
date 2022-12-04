namespace AdventOfCode.Solutions.Year2022;

internal class Day04 : ASolution
{
    private int[][] _lines;

    public Day04() : base(04, 2022, "", false)
    { }

    protected override void Preprocess()
    {
        _lines = InputByNewLine.Select(x => x.ToIntArray(new[] { ',', '-' })).ToArray();
    }

    protected override object SolvePartOne()
    {
        return _lines.Count(nums =>
        {
            var e1L = nums[0];
            var e1R = nums[1];
            var e2L = nums[2];
            var e2R = nums[3];

            return (e1L >= e2L && e1L <= e2R && e1R >= e2L && e1R <= e2R)
                || (e2L >= e1L && e2L <= e1R && e2R >= e1L && e2R <= e1R);

            // Slower, but more understandable
            //var s = new HashSet<int>(Enumerable.Range(e1L, e1R - e1L + 1));
            //var s2 = new HashSet<int>(Enumerable.Range(e2L, e2R - e2L + 1));
            //return s.IsSubsetOf(s2) || s2.IsSubsetOf(s);
        });
    }

    protected override object SolvePartTwo()
    {
        return _lines.Count(nums =>
        {
            var e1L = nums[0];
            var e1R = nums[1];
            var e2L = nums[2];
            var e2R = nums[3];

            return (e1L >= e2L && e1L <= e2R || e1R >= e2L && e1R <= e2R)
                || (e2L >= e1L && e2L <= e1R || e2R >= e1L && e2R <= e1R);

            //var s = new HashSet<int>(Enumerable.Range(e1L, e1R - e1L + 1));
            //var s2 = new HashSet<int>(Enumerable.Range(e2L, e2R - e2L + 1));
            //return s.Overlaps(s2);
        });
    }

    protected override string LoadDebugInput()
    {
        return """
        2-4,6-8
        2-3,4-5
        5-7,7-9
        2-8,3-7
        6-6,4-6
        2-6,4-8
        """;
    }
}
