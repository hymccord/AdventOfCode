using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020.Day16
{
    class Day16 : ASolution
    {
        private const string Expression = @"^(?<field>\w+(\s+\w+)*): (?<min1>\d+)-(?<max1>\d+) or (?<min2>\d+)-(?<max2>\d+)\r*$";
        readonly List<TicketRule> _rules = new();
        int[] _myTicket;
        int[][] _nearby;
        readonly List<int[]> _valid = new();

        public Day16()
            : base(16, 2020, "Ticket Translation")
        {

        }

        protected override object SolvePartOne()
        {
            string[] input = Input.SplitByBlankLine().ToArray();
            _myTicket = input[1].SplitByNewline().ToArray()[1]
                .Split(',').Select(int.Parse).ToArray();
            _nearby = input[2].SplitByNewline().Skip(1)
                .Select(s => s.Split(',').Select(int.Parse).ToArray())
                .ToArray();

            foreach (Match m in Regex.Matches(input[0], Expression, RegexOptions.Multiline))
            {
                var r = new TicketRule(m.Groups["field"].Value, int.Parse(m.Groups["min1"].Value), int.Parse(m.Groups["max1"].Value), int.Parse(m.Groups["min2"].Value), int.Parse(m.Groups["max2"].Value));
                _rules.Add(r);
            }

            long error = 0;
            foreach (var ticket in _nearby)
            {
                bool valid = true;
                foreach (var value in ticket)
                {
                    if (!_rules.Any(t => t.IsWithin(value)))
                    {
                        valid = false;
                        error += value;
                    }
                }

                if (valid)
                    _valid.Add(ticket);
            }

            return error;
        }

        protected override object SolvePartTwo()
        {
            Queue<TicketRule> pending = new Queue<TicketRule>(_rules);
            List<(int, TicketRule)> verified = new();

            int[,] valid = _valid.To2D();

            HashSet<TicketRule>[] columnRules = new HashSet<TicketRule>[valid.GetLength(1)];
            for (int i = 0; i < columnRules.Length; i++)
            {
                columnRules[i] = new HashSet<TicketRule>();
            }

            TicketRule current;
            while (pending.Count > 0)
            {
                current = pending.Dequeue();
                for (int i = 0; i < valid.GetLength(1); i++)
                {
                    if (valid.SliceColumn(i).All(n => current.IsWithin(n)))
                    {
                        columnRules[i].Add(current);
                    }
                }
            }

            while (verified.Count < _rules.Count)
            {
                TicketRule toRemove = null;
                int i;
                for (i = 0; i < columnRules.Length; i++)
                {
                    if (columnRules[i].Count == 1)
                    {
                        toRemove = columnRules[i].First();
                        break;
                    }
                }
                verified.Add((i, toRemove));
                foreach (var set in columnRules)
                {
                    set.Remove(toRemove);
                }
            }

            return verified.Where(t => t.Item2.Field.StartsWith("depar"))
                .Aggregate(1L, (total, next)
                            => total * _myTicket[next.Item1]);
        }

        [DebuggerDisplay("{ToString(),nq}")]
        class TicketRule
        {
            private readonly int _range1Min;
            private readonly int _range1Max;
            private readonly int _range2Min;
            private readonly int _range2Max;
            public string Field { get; set; }

            public TicketRule(string field, int range1Min, int range1Max, int range2Min, int range2Max)
            {
                Field = field;
                _range1Min = range1Min;
                _range1Max = range1Max;
                _range2Min = range2Min;
                _range2Max = range2Max;
            }

            public bool IsWithin(int value)
            {
                return (value >= _range1Min && value <= _range1Max) || (value >= _range2Min && value <= _range2Max);
            }

            public override string ToString()
                => $"{Field}: {_range1Min}-{_range1Max} or {_range2Min}-{_range2Max}";
        }
    }
}
