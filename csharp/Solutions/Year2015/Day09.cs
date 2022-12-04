namespace AdventOfCode.Solutions.Year2015;

internal class Day09 : ASolution
{
    private Dictionary<(string, string), int> _distances = new();
    private HashSet<string> _cities = new();

    public Day09() : base(09, 2015, "All in a Single Night", false)
    {

    }

    protected override void Preprocess()
    {
        foreach (var line in InputByNewLine)
        {
            var r = Regex.Match(line, @"^(\w+) to (\w+) = (\d+)$");
            var c1 = r.Groups[1].Value;
            var c2 = r.Groups[2].Value;
            var dist = int.Parse(r.Groups[3].Value);
            _distances.Add((c1,c2), dist);
            _distances.Add((c2, c1), dist);
            _cities.Add(c1);
            _cities.Add(c2);
        }
    }

    protected override object SolvePartOne()
    {
        var x = _cities.Permutate(_cities.Count);

        return x.Min(perm =>
        {
            var route = perm.ToArray();
            var len = 0;
            for (var i = 1; i < route.Length; i++)
            {
                len += _distances[(route[i - 1], route[i])];
            }

            return len;
        });
    }

    protected override object SolvePartTwo()
    {
        var x = _cities.Permutate(_cities.Count);
        return x.Max(perm =>
        {
            var route = perm.ToArray();
            var len = 0;
            for (var i = 1; i < route.Length; i++)
            {
                len += _distances[(route[i - 1], route[i])];
            }

            return len;
        });
    }

    protected override string LoadDebugInput()
    {
        return """
        London to Dublin = 464
        London to Belfast = 518
        Dublin to Belfast = 141
        """;
    }
}
