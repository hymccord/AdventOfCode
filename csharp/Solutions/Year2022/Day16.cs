namespace AdventOfCode.Solutions.Year2022;

using System.Collections.Immutable;
using System.Diagnostics;

using MoreLinq;

internal class Day16 : ASolution
{
    private List<string> _valves = new();
    private HashSet<string> _valveSet = new();
    private HashSet<string> _openableValves = new();
    private SortedDictionary<string, int> _valveToFlowRate = new();
    private Dictionary<string, HashSet<string>> _valveToFlowTo = new();
    private Dictionary<string, int> _valveToIndex = new();
    private HashSet<string> _openValves = new ();
    private Dictionary<string, List<(string, string)>> _xToY = new();

    private int[,] _adjacencyMatrix;
    public Day16() : base(16, 2022, "", true)
    {
        // 1192, too low
        // 1471, too low
        // 1651, too low
    }

    protected override void Preprocess()
    {
        _adjacencyMatrix = new int[InputByNewLine.Length, InputByNewLine.Length];
        int[,] graph = Enumerable.Repeat(Enumerable.Repeat<int>(9999, InputByNewLine.Length).ToArray(), InputByNewLine.Length).ToArray().To2D();
        foreach (var line in InputByNewLine)
        {
            var m = Regex.Matches(line, @"[A-Z]{2}");
            var m2 = Regex.Match(line, @"\d+");
            int flow = int.Parse(m2.Value);

            string from = m[0].Value;
            _valves.Add(from);
            if (flow > 0)
            {
                _openableValves.Add(from);
            }

            string[] to = m.Skip(1).Select(m => m.Value).ToArray();

            _valveToFlowRate[from] = flow;
            _valveToFlowTo[from] = new(to);
        }

        for (int i = 0; i < _valves.Count; i++)
        {
            graph[i, i] = 0;
            string from = _valves[i];
            _valveToIndex[from] = i;
            foreach (var to in _valveToFlowTo[from])
            {
                var idx = _valves.IndexOf(to);
                graph[i, idx] = 1;
            }
        }

        for (int i = 0; i < _valves.Count; i++)
        {
            for (int j = 0; j < _valves.Count; j++)
            {
                _adjacencyMatrix[i, j] = graph[i, j];
            }
        }

        for (int k = 0; k < _valves.Count; k++)
        {
            for (int i = 0; i < _valves.Count; i++)
            {
                for (int j = 0; j < _valves.Count; j++)
                {
                    if (_adjacencyMatrix[i, k] + _adjacencyMatrix[k, j] < _adjacencyMatrix[i, j])
                        _adjacencyMatrix[i, j] = _adjacencyMatrix[i, k] + _adjacencyMatrix[k, j];
                }
            }
        }

        _valveSet = new(_valves);
    }

    protected override object SolvePartOne()
    {
        return Score(0, 0, 30);
    }

    protected override object SolvePartTwo()
    {
        return null;
    }

    private Dictionary<(int, int, int), int> _memo = new();
    private int Score(int current, int idx, int minutesLeft)
    {
        if (minutesLeft == 0)
        {
            return 0;
        }

        if (_memo.TryGetValue((current, idx, minutesLeft), out var value))
        {
            return value;
        }

        int max = 0;
        for (int i = 0; i < _valveSet.Count; i++)
        {
            int temp = 0;
            if (IsValveTurnedOn(_valves[i], idx))
            {
                temp += _valveToFlowRate[_valves[i]];
            }

            if (current == i)
            {
                temp = _valveToFlowRate[_valves[i]] * (minutesLeft - 1);
                Debug.Assert((minutesLeft - 1) >= 0);
                temp += Score(i, TurnOnValve(i, idx), minutesLeft - 1);
            }
            else
            {
                int d = _adjacencyMatrix[current, i];
                temp = Score(i, idx, minutesLeft - d);
            }

            _memo[(i, idx, minutesLeft)] = temp;

            if (temp > max)
            {
                max = temp;
            }
        }

        return max;
    }

    private int TurnOnValve(int valve, int idx)
    {
        return idx |= (1 << valve);
    }

    private bool IsValveTurnedOn(string valve, int idx)
    {
        return ((idx >> _valveToIndex[valve]) & 1) == 1;
    }

    protected override string LoadDebugInput()
    {
        return """
        Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
        Valve BB has flow rate=13; tunnels lead to valves CC, AA
        Valve CC has flow rate=2; tunnels lead to valves DD, BB
        Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
        Valve EE has flow rate=3; tunnels lead to valves FF, DD
        Valve FF has flow rate=0; tunnels lead to valves EE, GG
        Valve GG has flow rate=0; tunnels lead to valves FF, HH
        Valve HH has flow rate=22; tunnel leads to valve GG
        Valve II has flow rate=0; tunnels lead to valves AA, JJ
        Valve JJ has flow rate=21; tunnel leads to valve II
        """;
    }
}
