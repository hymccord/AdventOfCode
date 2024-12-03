namespace AdventOfCode.Solutions.Year2021;

internal class Day01 : ASolution
{
    private int[] _ints;

    public Day01() : base(01, 2021)
    { }

    protected override void Preprocess()
    {
        _ints = Input.ToIntArray();
    }

    protected override object SolvePartOne()
    {
        int c = 0;

        for (int i = 1; i <= _ints.Length - 1; i++)
        {
            if (_ints[i] > _ints[i - 1])
            {
                c++;
            }
        }

        return c;
    }

    protected override object SolvePartTwo()
    {
        int previousWindow = int.MaxValue;
        int c = 0;

        for (int i = 0; i <= _ints.Length - 3; i++)
        {
            int currentWindow = _ints[i..(i + 3)].Sum();
            if (currentWindow > previousWindow)
                c++;

            previousWindow = currentWindow;
        }

        return c;
    }

    protected override string LoadDebugInput()
    {
        return """
            199
            200
            208
            210
            200
            207
            240
            269
            260
            263
            """;
    }
}
