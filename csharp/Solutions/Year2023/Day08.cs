


using MoreLinq;

namespace AdventOfCode.Solutions.Year2023;

internal class Day08 : ASolution
{
    private string _steps;
    private Dictionary<string, (string left, string right)> _map;

    public Day08() : base(08, 2023, "Haunted Wasteland", false)
    {

    }

    protected override object SolvePartOne()
    {
        ParseInput();

        string current = "AAA";
        int step = 0;
        int total = 0;
        do
        {
            (step, current) = Stepper((step, current));
            total++;
        } while (current != "ZZZ");

        return total;
    }

    protected override object SolvePartTwo()
    {
        string[] current = _map.Keys.Where(s => s.EndsWith("A")).ToArray();
        (int lam, int mu)[] res = new (int lam, int mu)[current.Length];

        for (int i = 0; i < res.Length; i++)
        {
            res[i] = Floyd(Stepper, (0, current[i]));
        }

        long lcm = res.Select(t => (long)t.lam).LCM();

        return lcm;
    }

    private (int, string) Stepper((int step, string current) x0)
    {
        (int step, string current) = x0;
        current = _steps[step] == 'L' ? _map[current].left : _map[current].right;
        step = (step + 1) % _steps.Length;

        return (step, current);
    }

    // Floyd cycle detection
    private (int, int) Floyd(Func<(int, string), (int, string)> f, (int, string) x0)
    {
        var tortoise = f(x0);
        var hare = f(f(x0));
        while (tortoise != hare)
        {
            tortoise = f(tortoise);
            hare = f(f(hare));
        }

        var mu = 0;
        tortoise = x0;
        while (tortoise != hare)
        {
            tortoise = f(tortoise);
            hare = f(hare);
            mu += 1;
        }

        var lam = 1;
        hare = f(tortoise);
        while (tortoise != hare)
        {
            hare = f(hare);
            lam += 1;
        }

        return (lam, mu);
    }

    private void ParseInput()
    {
        _steps = InputByNewLine[0];

        _map = InputByNewLine[1..].Select(l =>
        {
            return Regex.Match(l, @"(\w+) = \((\w+), (\w+)\)");
        }).ToDictionary(
            m => m.Groups[1].Value,
            m => (m.Groups[2].Value, m.Groups[3].Value)
        );
    }

    protected override string LoadDebugInput()
    {
        // 2

        //return """
        //RL

        //AAA = (BBB, CCC)
        //BBB = (DDD, EEE)
        //CCC = (ZZZ, GGG)
        //DDD = (DDD, DDD)
        //EEE = (EEE, EEE)
        //GGG = (GGG, GGG)
        //ZZZ = (ZZZ, ZZZ)
        //""";

        // 6

        //return """
        //LLR

        //AAA = (BBB, BBB)
        //BBB = (AAA, ZZZ)
        //ZZZ = (ZZZ, ZZZ)
        //""";

        return """
        LR

        11A = (11B, XXX)
        11B = (XXX, 11Z)
        11Z = (11B, XXX)
        22A = (22B, XXX)
        22B = (22C, 22C)
        22C = (22Z, 22Z)
        22Z = (22B, 22B)
        XXX = (XXX, XXX)
        """;
    }
}
