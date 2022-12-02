using System;

namespace AdventOfCode.Solutions.Year2022;

internal class Day02 : ASolution
{
    public Day02() : base(02, 2022, "", false)
    { }

    protected override object SolvePartOne()
    {
        return InputByNewLine.Select(s =>
        {
            var opponent = s[0];
            var me = s[2];

            return (opponent, me) switch
            {
                ('A', 'X') => 3 + 1,
                ('A', 'Y') => 6 + 2,
                ('A', 'Z') => 0 + 3,
                ('B', 'X') => 0 + 1,
                ('B', 'Y') => 3 + 2,
                ('B', 'Z') => 6 + 3,
                ('C', 'X') => 6 + 1,
                ('C', 'Y') => 0 + 2,
                ('C', 'Z') => 3 + 3,
                _ => throw new NotImplementedException(),
            };
        }).Sum();
    }

    protected override object SolvePartTwo()
    {
        return InputByNewLine.Select(s =>
        {
            var opponent = s[0];
            var me = s[2];

            return (opponent, me) switch
            {
                ('A', 'X') => 0 + 3,
                ('B', 'X') => 0 + 1,
                ('C', 'X') => 0 + 2,
                ('A', 'Y') => 3 + 1,
                ('B', 'Y') => 3 + 2,
                ('C', 'Y') => 3 + 3,
                ('A', 'Z') => 6 + 2,
                ('B', 'Z') => 6 + 3,
                ('C', 'Z') => 6 + 1,
                _ => throw new NotImplementedException(),
            };
        }).Sum();
    }

    protected override string LoadDebugInput()
    {
        return """
        A Y
        B X
        C Z
        """;
    }
}
