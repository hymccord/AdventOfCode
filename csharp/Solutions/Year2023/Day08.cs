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
        int[] cycles = new int[current.Length];

        for (int i = 0; i < current.Length; i++)
        {
            int step = 0;
            int total = 0;
            do
            {
                (step, current[i]) = Stepper((step, current[i]));
                total++;
            } while (!current[i].EndsWith("Z"));
            cycles[i] = total;
        }

        long lcm = cycles.Select(i => (long)i).LCM();

        return lcm;
    }

    private (int, string) Stepper((int step, string current) x0)
    {
        (int step, string current) = x0;
        current = _steps[step] == 'L' ? _map[current].left : _map[current].right;
        step = (step + 1) % _steps.Length;

        return (step, current);
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
