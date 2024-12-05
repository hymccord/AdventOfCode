namespace AdventOfCode.Solutions.Year2024;

internal class Day03 : ASolution
{
    public Day03() : base(03, 2024, "Mull It Over", false)
    { }

    protected override object SolvePartOne()
    {
        return Scan(Input);
    }

    protected override object SolvePartTwo()
    {
        var undone = Regex.Replace(Input, @"don't\(\).*?(do\(\)|$)", "", RegexOptions.Singleline);
        return Scan(undone);
    }

    private static long Scan(string input)
    {
        return Regex.Matches(input, @"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Singleline)
            .Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ExampleInput("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", partOne: 161),
            new ExampleInput("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", partTwo: 48)
            ];
    }
}
