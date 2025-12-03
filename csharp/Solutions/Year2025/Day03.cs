namespace AdventOfCode.Solutions.Year2025;

internal class Day03 : ASolution
{
    public Day03() : base(03, 2025, "Lobby", true)
    { }

    protected override object SolvePartOne()
    {
        return Input
            .To2DIntArray()
            .Rows()
            .Sum(r => Joltage(r, 2));
    }

    protected override object SolvePartTwo()
    {
        return Input
            .To2DIntArray()
            .Rows()
            .Sum(r => Joltage(r, 12));
    }

    private static long Joltage(int[] arr, int numBatteries)
    {
        var sum = 0L;
        var needle = 0;
        for (; numBatteries > 0 ; numBatteries--)
        {
            long pow = (long)Math.Max(Math.Pow(10, numBatteries - 1), 1);

            var max = 0;
            for (int i = needle; i < arr.Length - numBatteries + 1; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                    needle = i + 1;
                }
            }

            sum += pow * max;
        }

        return sum;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                987654321111111
                811111111111119
                234234234234278
                818181911112111
                """,
                partOne: 357, partTwo: 3121910778619
            )
            ];
    }
}
