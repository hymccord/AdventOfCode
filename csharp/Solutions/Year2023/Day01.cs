using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2023;

internal class Day01 : ASolution
{
    public Day01() : base(01, 2023, "Trebuchet?!", false)
    { }

    protected override object SolvePartOne()
    {
        return InputByNewLine.Select(x => x.Where(char.IsAsciiDigit))
            .Sum(ic => int.Parse($"{ic.First()}{ic.Last()}"));
    }

    protected override object SolvePartTwo()
    {
        var replacement = new Dictionary<string, int>
        {
            { "one", 1},
            { "two", 2},
            { "three", 3},
            { "four", 4},
            { "five", 5},
            { "six", 6},
            { "seven", 7},
            { "eight", 8},
            { "nine", 9},
            { "1", 1},
            { "2", 2},
            { "3", 3},
            { "4", 4},
            { "5", 5},
            { "6", 6},
            { "7", 7},
            { "8", 8},
            { "9", 9},
        };

        return InputByNewLine
            .Select(line =>
            {
                // Overlap match groups by using lookahead (?=).
                return Regex.Matches(line, "(?=(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9))");
            })
            .Sum(mc =>
            {
                // Use first and last matches.
                const int LookaheadGroup = 1;
                Index FirstMatch = 0;
                Index LastMatch = ^1;
                return replacement[mc[FirstMatch].Groups[LookaheadGroup].Value] * 10
                    + replacement[mc[LastMatch].Groups[LookaheadGroup].Value];
            });
    }

    protected override string LoadDebugInput()
    {
        return """
        two1nine
        eightwothree
        abcone2threexyz
        xtwone3four
        4nineeightseven2
        zoneight234
        7pqrstsixteen
        """;
    }
}
