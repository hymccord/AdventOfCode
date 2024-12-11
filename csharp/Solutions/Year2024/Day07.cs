using System.Collections.Immutable;

namespace AdventOfCode.Solutions.Year2024;

internal class Day07 : ASolution
{
    private ImmutableDictionary<long, int[]> _operations;
    private Dictionary<string, long> _cache;

    public Day07() : base(07, 2024, "Bridge Repair", true)
    { }

    protected override void Preprocess()
    {
        _operations = InputByNewLine.Select(static l =>
        {
            (string left, string right) = l.SplitInTwo(':');
            return new KeyValuePair<long, int[]>(long.Parse(left), right.ToIntArray());
        }).ToImmutableDictionary();
    }

    protected override object SolvePartOne()
    {
        _cache = [];
        _partOneInvalid = [];
        long sum = 0L;

        foreach (var (target, arr) in _operations)
        {
            if (IsSolvable(target, [], [..arr]))
            {
                sum += target;
            }
            else
            {
                _partOneInvalid[target] = arr;
            }
        }

        return sum;
    }

    private Dictionary<long, int[]> _partOneInvalid;
    protected override object SolvePartTwo()
    {
        long sum = 0L;

        foreach (var (target, arr) in _partOneInvalid)
        {
            if (IsSolvable(target, [], [.. arr], true))
            {
                sum += target;
            }
        }

        return long.Parse(Part1) + sum;
    }

    private bool IsSolvable(long target, ImmutableList<object> left, ImmutableList<int> right, bool useConcat = false)
    {
        var existing = Evaluate(left);

        if (existing > target)
        {
            return false;
        }

        if (right.Count == 1)
        {
            return target == Evaluate(left.Add(right[0]));
        }

        int appendLeft = right[0];

        left = left.Add(appendLeft);

        return
            IsSolvable(target, left.Add("*"), right.RemoveAt(0), useConcat) ||
            IsSolvable(target, left.Add("+"), right.RemoveAt(0), useConcat) ||
            (useConcat &&
            IsSolvable(target, left.Add("||"), right.RemoveAt(0), useConcat));
    }

    private long Evaluate(ImmutableList<object> expression)
    {
        if (expression.Count == 0) return 0;
        if (expression.Count == 1) return (long)expression[0];

        if (expression.Count % 2 == 0)
        {
            expression = expression.RemoveAt(expression.Count - 1);
        }

        string key = string.Join("", expression);
        if (_cache.TryGetValue(key, out long result))
        {
            return result;
        }

        result = (int)expression[0];
        for (var i = 1; i <= expression.Count - 2; i += 2)
        {
            string operation = (string)expression[i];
            int value = (int)expression[i + 1];

            result = operation switch
            {
                "+" => result + value,
                "*" => result * value,
                "||" => (long)(result * Math.Pow(10, Math.Ceiling(Math.Log10(value))) + value),
                _ => throw new NotImplementedException()
            };
        }

        _cache.Add(key, result);

        return result;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ExampleInput("""
                190: 10 19
                3267: 81 40 27
                83: 17 5
                156: 15 6
                7290: 6 8 6 15
                161011: 16 10 13
                192: 17 8 14
                21037: 9 7 18 13
                292: 11 6 16 20
                """, 3749, 11387)
            ];
    }

    private class Base
    {

    }

    private class Value : Base {}
    private class Expression : Base { }
}
