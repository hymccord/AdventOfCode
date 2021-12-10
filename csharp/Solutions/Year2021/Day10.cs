namespace AdventOfCode.Solutions.Year2021
{
    internal class Day10 : ASolution
    {
        readonly Dictionary<char, char> _openToClose = new()
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' },
        };
        readonly Dictionary<char, char > _closeToOpen = new()
        {
            { ')', '(' },
            { ']', '[' },
            { '}', '{' },
            { '>', '<' },
        };
        readonly Dictionary<char, int> _closeToSyntaxScore = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 },
        };
        readonly Dictionary<char, int> _closeToAutocompleteScore = new()
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 },
        };

        public Day10() : base(10, 2021, "Syntax Scoring", false)
        {

        }

        readonly List<Stack<char>> _incomplete = new();
        protected override object SolvePartOne()
        {
            int score = 0;
            foreach (var line in InputByNewLine)
            {
                bool valid = true;
                Stack<char> s = new();
                s.Push(line[0]);
                foreach (char c in line[1..])
                {
                    if (_openToClose.ContainsKey(c))
                    {
                        s.Push(c);
                    }
                    else
                    { // It's a closing bracket
                        if (s.Peek() != _closeToOpen[c])
                        {
                            score += _closeToSyntaxScore[c];
                            valid = false;
                            break;
                        }
                        _ = s.Pop();
                    }
                }

                if (valid)
                {
                    _incomplete.Add(s);
                }
            }

            return score;
        }

        protected override object SolvePartTwo()
        {
            return _incomplete
                .Select(incompleteOpenChars => incompleteOpenChars.Aggregate(0L, (sum, c) => sum * 5L + _closeToAutocompleteScore[_openToClose[c]]))
                .OrderBy(x => x)
                .Skip(_incomplete.Count / 2)
                .Take(1)
                .Single();
        }

        protected override string LoadDebugInput()
        {
            return @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";
        }
    }
}
