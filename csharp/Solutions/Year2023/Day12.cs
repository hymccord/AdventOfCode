
using System.Collections.Immutable;

namespace AdventOfCode.Solutions.Year2023;

internal class Day12 : ASolution
{
    public Day12() : base(12, 2023, "Hot Springs", true)
    {
    }

    protected override object SolvePartOne()
    {
        var total = 0;

        foreach (var input in InputByNewLine)
        {
            (var line, var rest) = input.SplitInTwo(' ');
            var group = ImmutableStack.CreateRange(rest.Split(',').Reverse().Select(int.Parse));

            var step = Step(line, group);
            Console.WriteLine($"{input} - {step}");

            total += step;
        }
        return null;
    }

    protected override object SolvePartTwo()
    {
        return null;
    }

    private int Step(string line, ImmutableStack<int> groups)
    {
        if (string.IsNullOrEmpty(line))
        {
            return 1;
        }

        if (groups.IsEmpty)
        {
            return 0;
        }
        
        if (line.StartsWith("."))
        {
            if (groups.PeekRef() == 0)
            {
                return Step(line.Substring(1), groups.Pop());
            }

            return Step(line.Substring(1), groups);
        }

        if (line.StartsWith('#'))
        {
            var i = groups.Peek();
            groups = groups.Pop();

            if (i == 0)
            {
                return 0;
            }

            i--;

            return Step(line.Substring(1), groups.Push(i));

        }

        if (line.StartsWith('?'))
        {
            var one = Step($".{line.Substring(1)}", groups);
            var two = Step($"#{line.Substring(1)}", groups);

            return one + two;
        }

        //return Step(line, groups);
        throw new NotImplementedException();
    }

    protected override string LoadDebugInput()
    {
        return """
            ???.### 1,1,3
            .??..??...?##. 1,1,3
            ?#?#?#?#?#?#?#? 1,3,1,6
            ????.#...#... 4,1,1
            ????.######..#####. 1,6,5
            ?###???????? 3,2,1
            """;
    }
}
