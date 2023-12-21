
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace AdventOfCode.Solutions.Year2023;

internal class Day15 : ASolution
{
    public Day15() : base(15, 2023, "Lens Library", false)
    {
    }

    protected override object SolvePartOne()
    {
        return Input.Split(',').Sum(Hash);
    }

    internal static readonly char[] separator = new[] { '-', '=' };


    private OrderedDictionary[] _boxes2 = 
        Enumerable.Range(0, 256)
            .Select(i => new OrderedDictionary())
            .ToArray();

    protected override object SolvePartTwo()
    {
        foreach (var item in Input.Split(','))
        {
            var arr = item.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var label = arr[0];
            var box = _boxes2[Hash(label)];
            
            // -
            if (arr.Length == 1) 
            {
                box.Remove(label);
            }
            // =
            else
            {
                box[label] = arr[1];
            }
        }

        return _boxes2
            .Select((box, index) =>
            {
                if (box.Count == 0) return 0;

                var total = 0;
                for (int i = 0; i < box.Count; i++)
                {
                    total += (index + 1) * (i + 1) * int.Parse((string)box[i]);
                }

                return total;
            })
            .Sum();
    }

    protected override string LoadDebugInput()
    {
        return """
            rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
            """;
    }

    private static int Hash(string s)
    {
        var currentValue = 0;
        for (var i = 0; i < s.Length; i++)
        {
            currentValue += (int)s[i];
            currentValue *= 17;
            (_, currentValue) = Math.DivRem(currentValue, 256);
        }

        return currentValue;
    }
}
