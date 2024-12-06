namespace AdventOfCode.Solutions.Year2024;

internal class Day05 : ASolution
{
    private Dictionary<int, HashSet<int>> _rules;
    private SortWithRulesComparer _sortComparer;
    private List<List<int>> _updates;

    public Day05() : base(05, 2024, "Print Queue", true)
    { }

    protected override void Preprocess()
    {
        var input = Input.SplitByBlankLine();
        var rules = input[0].SplitByNewline();
        var updates = input[1].SplitByNewline();

        _rules = new Dictionary<int, HashSet<int>>();
        foreach (var rule in rules)
        {
            if (rule.ToIntArray('|') is [int left, int right])
            {
                var set = _rules.Get(left, new HashSet<int>());
                set.Add(right);

                _rules[left] = set;
            }
        }

        _sortComparer = new SortWithRulesComparer(_rules);
        _updates = updates.Select(l => l.ToIntArray(',').ToList()).ToList();
    }

    protected override object SolvePartOne()
    {
        int sum = 0;
        List<List<int>> incorrect = [];
        foreach (var original in _updates)
        {
            var sorted = new List<int>(original);
            sorted.Sort(_sortComparer);

            if (original.SequenceEqual(sorted))
            {
                sum += original[original.Count / 2];
            }
            else
            {
                incorrect.Add(original);
            }
        }

        _incorrect = incorrect;
        return sum;
    }

    private List<List<int>> _incorrect;
    protected override object SolvePartTwo()
    {
        int sum = 0;
        foreach (var update in _incorrect)
        {
            update.Sort(_sortComparer);

            sum += update[update.Count / 2];
        }

        return sum;
    }

    private class SortWithRulesComparer : IComparer<int>
    {
        private readonly IDictionary<int, HashSet<int>> _allRules;

        public SortWithRulesComparer(IDictionary<int, HashSet<int>> rules)
        {
            _allRules = rules;
        }
        public int Compare(int x, int y)
        {
            var xRules = _allRules.Get(x, null);
            var yRules = _allRules.Get(y, null);

            if (xRules is null && yRules is null)
            {
                return 0;
            }

            if (xRules is not null && xRules.Contains(y))
            {
                return -1;
            }

            if (yRules is not null && yRules.Contains(x))
            {
                return 1;
            }

            return 0;
        }
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ExampleInput("""
                47|53
                97|13
                97|61
                97|47
                75|29
                61|13
                75|53
                29|13
                97|29
                53|29
                61|53
                97|53
                61|29
                47|13
                75|47
                97|75
                47|61
                75|61
                47|29
                75|13
                53|13

                75,47,61,53,29
                97,61,53,29,13
                75,29,13
                75,97,47,61,53
                61,13,29
                97,13,75,29,47
                """, 143, 123)
            ];
    }
}
