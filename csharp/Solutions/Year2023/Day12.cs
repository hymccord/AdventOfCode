
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

using MoreLinq;

using Cache = System.Collections.Generic.Dictionary<(string, System.Collections.Immutable.ImmutableStack<int>), long>;

namespace AdventOfCode.Solutions.Year2023;

internal class Day12 : ASolution
{

    public Day12() : base(12, 2023, "Hot Springs", false)
    {
    }

    protected override object SolvePartOne()
    {
        var total = 0L;

        foreach (var input in InputByNewLine)
        {
            (var line, var rest) = input.SplitInTwo(' ');
            var group = ImmutableStack.CreateRange(rest.Split(',').Reverse().Select(int.Parse));

            var step = Calculate(line, group, new Cache());
            // uncomment to debug each line
            //Console.WriteLine($"{input} - {step}");

            total += step;
        }

        return total;
    }

    protected override object SolvePartTwo()
    {
        var total = 0L;

        foreach (var input in InputByNewLine)
        {
            (var line, var rest) = input.SplitInTwo(' ');

            line = string.Join("?", Enumerable.Repeat(line, 5));
            var group = ImmutableStack.CreateRange(rest.Split(',').Reverse().Select(int.Parse).Repeat(5));

            var step = Calculate(line, group, new Cache());
            // uncomment to debug each line
            //Console.WriteLine($"{input} - {step}");

            total += step;
        }

        return total;
    }

    private long Calculate(string line, ImmutableStack<int> groups, Cache cache)
    {
        if (cache.TryGetValue((line, groups), out var value))
        {
            return value;
        }

        cache[(line, groups)] = Springergize(line, groups, cache);

        return cache[(line, groups)];
    }

    private long Springergize(string line, ImmutableStack<int> groups, Cache cache)
    {
        while (true)
        {
            if (string.IsNullOrEmpty(line))
            {
                return groups.IsEmpty ? 1 : 0;
            }

            if (groups.IsEmpty)
            {
                return line.IndexOf('#') < 0 ? 1 : 0;
            }

            if (line.StartsWith('.'))
            {
                line = line.Trim('.');
                continue;
            }

            if (line.StartsWith('?'))
            {
                return Calculate($"#{line[1..]}", groups, cache)
                    + Calculate($".{line[1..]}", groups, cache);
            }

            if (line.StartsWith('#'))
            {
                var consumable = groups.Peek();

                if (consumable > line.Length)
                {
                    return 0;
                }

                if (line[..consumable].Contains('.'))
                {
                    return 0;
                }

                if (groups.Count() > 1)
                {
                    if (consumable + 1 > line.Length || line[consumable] == '#')
                    {
                        return 0;
                    }

                    line = line[(consumable + 1)..];
                    groups = groups.Pop();
                    continue;
                }

                line = line[consumable..];
                groups = groups.Pop();
            }
        }
    }

    protected override string LoadDebugInput()
    {
        //return """
        //    ??#.?????? 3,1
        //    """;
        //return """
        //    ?###???????? 3,2,1
        //    """;
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
