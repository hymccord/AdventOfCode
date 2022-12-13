using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdventOfCode.Solutions.Year2022;

internal class Day13 : ASolution
{
    private JsonElement[] _pairs;

    public Day13() : base(13, 2022, "Distress Signal", false)
    {
    }

    protected override void Preprocess()
    {
        _pairs = Input
            .SplitByBlankLine()
            .SelectMany(l => l.SplitByNewline().Select(s => JsonDocument.Parse(s).RootElement))
            .ToArray();
    }

    protected override object SolvePartOne()
    {
        return _pairs
            .Chunk(2)
            .Select((e, i) =>
            {
                if (Compare(e[0], e[1]) < 0)
                {
                    return i + 1;
                }
                return 0;
            })
            .Sum();
    }

    protected override object SolvePartTwo()
    {
        var elements = _pairs.ToList();
        var two = JsonDocument.Parse("[[2]]").RootElement;
        var six = JsonDocument.Parse("[[6]]").RootElement;

        elements.Add(two);
        elements.Add(six);
        elements.Sort(Compare);

        return (elements.IndexOf(two) + 1) * (elements.IndexOf(six) + 1);
    }

    public static int Compare(JsonElement left, JsonElement right)
    {
        if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
        {
            int i = left.GetInt32();
            int j = right.GetInt32();

            return i.CompareTo(j);
        }

        if (left.ValueKind == JsonValueKind.Array && right.ValueKind == JsonValueKind.Array)
        {
            var leftLen = left.GetArrayLength();
            var rightLen = right.GetArrayLength();

            for (int i = 0; i < leftLen; i++)
            {
                if (i == rightLen)
                { // right ran out
                    return 1;
                }

                var result = Compare(left[i], right[i]);
                if (result == 0)
                {
                    continue;
                }

                return result;
            }

            if (leftLen == rightLen)
            {
                return 0;
            }

            // left ran out
            return -1;
        }

        if (left.ValueKind == JsonValueKind.Number)
        {
            return Compare(JsonSerializer.SerializeToElement(new[] { left.GetInt32() }), right);
        }
        else
        {
            return Compare(left, JsonSerializer.SerializeToElement(new[] { right.GetInt32() }));
        }

    }

    protected override string LoadDebugInput()
    {
        return """
        [1,1,3,1,1]
        [1,1,5,1,1]

        [[1],[2,3,4]]
        [[1],4]

        [9]
        [[8,7,6]]

        [[4,4],4,4]
        [[4,4],4,4,4]

        [7,7,7,7]
        [7,7,7]

        []
        [3]

        [[[]]]
        [[]]

        [1,[2,[3,[4,[5,6,7]]]],8,9]
        [1,[2,[3,[4,[5,6,0]]]],8,9]
        """;
    }
}
