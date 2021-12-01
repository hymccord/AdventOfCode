namespace AdventOfCode.Solutions.Year2020.Day19
{
    class Day19 : ASolution
    {
        const string Test = @"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb";
        string[] _messages;
        Dictionary<int, TreeNode<RuleNode>> _nodes = new();
        //Dictionary<int, Rule> _rule = new();
        Dictionary<int, string> _rules = new();
        HashSet<int> _processed = new();

        public Day19()
            : base(19, 2020, "Monster Messages")
        {

        }

        protected override object SolvePartOne()
        {
            string[] split = Input.SplitByBlankLine().ToArray();
            string r = split[0];
            string m = split[1];

            _messages = m.SplitByNewline().ToArray();
            _rules = r.SplitByNewline()
                .Select(s => s.Split(':'))
                .ToDictionary(s => int.Parse(s[0]), s => s[1]);

            //string rule = _rules[0];
            //while (_processed.Count < _rules.Count)
            //{
            string pattern = $"^{Process(0, _rules[0])}$";
            int i = 0;
            foreach (var msg in _messages)
            {
                if (Regex.IsMatch(msg, pattern))
                    i++;
            }
            //    _processed.ad
            //}

            string Process(int rule, string s)
            {
                if (_processed.Contains(rule))
                    return _rules[rule];

                string x = "";
                string[] arr;
                if (s.Contains("|"))
                {
                    arr = s.Split(" | ", StringSplitOptions.RemoveEmptyEntries);
                    arr[0] = Process(-1, arr[0].Trim());
                    arr[1] = Process(-1, arr[1].Trim());

                    x = $"({string.Join("|", arr)})";
                    // LEFT | RIGHT
                    //string x = "(" + string.Join(" | ", s.Split(" | ", StringSplitOptions.RemoveEmptyEntries)
                    //    .Select(y =>
                    //    {
                    //        string x = string.Join("", y.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    //        .Select(yy =>
                    //        {
                    //            int ruleNum = int.Parse(yy);
                    //            return Process(ruleNum, _rules[ruleNum]);
                    //        }));
                    //        return x;
                    //    }));
                    _processed.Add(rule);
                    _rules[rule] = x;

                }
                else if (Regex.IsMatch(s, @"""([ab])"""))
                {
                    _processed.Add(rule);
                    _rules[rule] = Regex.Match(s, @"""([ab])""").Groups[1].Value;
                    x = _rules[rule];
                }
                else if (Regex.IsMatch(s, @"\d+"))
                {
                    //int ruleNum = int.Parse()
                    //if (_processed.Contains)
                    x = string.Join("", s.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(y =>
                        {
                            int ruleNum = int.Parse(y);
                            return Process(ruleNum, _rules[ruleNum]);
                        }));
                    if (rule > 0)
                    {
                        _processed.Add(rule);
                        _rules[rule] = x;
                    }
                    return x;
                }

                return x;
            }

            return i;
        }

        protected override object SolvePartTwo()
        {
            return null;
        }

        class Rule
        {
            public bool Final { get; set; }
            public bool Or { get; set; }
            public Rule Left { get; set; }
            public Rule Right { get; set; }

        }

        class RuleNode
        {

        }

        class OrNode : RuleNode
        {

        }

        //class 
    }
}
