using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2022;

internal class Day06 : ASolution
{
    public Day06() : base(06, 2022, "", false)
    { }

    protected override object SolvePartOne()
    {
        return Tune(4);
    }

    protected override object SolvePartTwo()
    {
        return Tune(14);
    }

    private int Tune(int numCharacters)
    {
        for (int i = 0; i < Input.Length; i++)
        {
            if (new HashSet<char>(Input.Substring(i, numCharacters).ToCharArray()).Count == numCharacters)
            {
                return i + numCharacters;
            }
        }

        return -1;
    }

    protected override string LoadDebugInput()
    {
        return """
        mjqjpqmgbljsphdztnvjfqwrcgsmlb
        """;
    }
}
