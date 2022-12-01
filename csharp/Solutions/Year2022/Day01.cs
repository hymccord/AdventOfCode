namespace AdventOfCode.Solutions.Year2022;

internal class Day01 : ASolution
{
    private readonly Lazy<int[]> _nums;

    public Day01() : base(1, 2022, "Calorie Counting", false)
    {
        _nums = new(Input.SplitByBlankLine().Select(s => s.ToIntArray().Sum()).ToArray());
    }

    protected override object SolvePartOne()
    {
        return _nums.Value.Max();
    }

    protected override object SolvePartTwo()
    {
        return _nums.Value.OrderDescending().Take(3).Sum();
    }

    protected override string LoadDebugInput()
    {
        return """
            1000
            2000
            3000

            4000

            5000
            6000

            7000
            8000
            9000

            10000
            """;
    }
}
