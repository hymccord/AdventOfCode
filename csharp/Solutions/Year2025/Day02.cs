namespace AdventOfCode.Solutions.Year2025;

internal class Day02 : ASolution
{
    public Day02() : base(02, 2025, "Gift Shop", true)
    { }

    protected override object SolvePartOne()
    {
        var sum = 0L;
        foreach (var range in Input.Split(','))
        {
            (var left, var right) = range.SplitInTwo('-');
            long start = long.Parse(left);
            long end = long.Parse(right);

            for (; start <= end; start++)
            {
                long digits = (long)Math.Floor(Math.Log10(start)) + 1;
                if ((digits & 0b1) == 1) continue;

                long lower = start % (long)(Math.Pow(10, digits / 2));
                long upper = (long)(start / Math.Pow(10, digits / 2));

                if (lower == upper)
                {
                    //Console.WriteLine($"invalid: {start}");
                    sum += start;
                }
            }
        }

        return sum;
    }

    protected override object SolvePartTwo()
    {
        var sum = 0L;
        var regex = new Regex(@"^(\d*)\1{1,}$");
        foreach (var range in Input.Split(','))
        {
            (var left, var right) = range.SplitInTwo('-');
            long start = long.Parse(left);
            long end = long.Parse(right);

            for (; start <= end; start++)
            {

                if (regex.IsMatch($"{start}"))
                {
                    WriteOutput($"invalid: {start}");
                    sum += start;
                }
            }
        }

        return sum;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
                """, partOne: 1227775554, partTwo: 4174379265)
            ];
    }
}
