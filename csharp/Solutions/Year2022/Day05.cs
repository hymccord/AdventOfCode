namespace AdventOfCode.Solutions.Year2022;

internal class Day05 : ASolution
{
    private Dictionary<string, Stack<char>> _stacks = new();
    private List<(string num, string from, string to)> _moves = new();

    public Day05() : base(05, 2022, "Supply Stacks", false)
    {

    }
    protected override void Preprocess()
    {
        _stacks = new();
        _moves = new();

        string[] boxesAndMoves = Input.SplitByBlankLine();
        string[] boxes = boxesAndMoves[0].SplitByNewline();
        string[] moves = boxesAndMoves[1].SplitByNewline();

        var m = Regex.Matches(boxes[^1], @"\d");
        for (int i = boxes.Length - 2; i >= 0; i--)
        {
            foreach (Match match in m)
            {
                var currentLetter = boxes[i][match.Index];
                if (currentLetter != ' ')
                {
                    _stacks.AddOrUpdate(match.Value, new Stack<char>(new[] { currentLetter }), (k, s) =>
                    {
                        s.Push(currentLetter);
                        return s;
                    });
                }
            }
        }

        foreach (var line in moves)
        {
            var mv = Regex.Match(line, @"^move (\d+) from (\d+) to (\d+)$");
            _moves.Add((mv.Groups[1].Value, mv.Groups[2].Value, mv.Groups[3].Value));
        }
    }

    protected override object SolvePartOne()
    {
        foreach (var move in _moves)
        {
            int numMoves = int.Parse(move.num);
            var fromStack = _stacks[move.from];
            var toStack = _stacks[move.to];

            for (int i = 0; i < numMoves; i++)
            {
                toStack.Push(fromStack.Pop());
            }
        }

        return new string(_stacks.Values.Select(s => s.Pop()).ToArray());
    }

    protected override object SolvePartTwo()
    {
        Preprocess();
        foreach (var move in _moves)
        {
            int numMoves = int.Parse(move.num);
            var fromStack = _stacks[move.from];
            var toStack = _stacks[move.to];

            Stack<char> xfer = new();
            for (int i = 0; i < numMoves; i++)
            {
                xfer.Push(fromStack.Pop());
            }

            for (int j = 0; j < numMoves; j++)
            {
                toStack.Push(xfer.Pop());
            }
        }

        return new string(_stacks.Values.Select(s => s.Pop()).ToArray());
    }

    protected override string LoadDebugInput()
    {
        return """
            [D]    
        [N] [C]    
        [Z] [M] [P]
         1   2   3 

        move 1 from 2 to 1
        move 3 from 1 to 3
        move 2 from 2 to 1
        move 1 from 1 to 2
        """;
    }
}
