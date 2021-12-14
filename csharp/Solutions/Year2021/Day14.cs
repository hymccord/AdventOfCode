using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2021
{
    internal class Day14 : ASolution
    {
        Dictionary<string, char> _rules = new();
        Dictionary<string, long> _polymers = new();
        Dictionary<char, long> _charCounts = new();
        int _steps = 0;

        public Day14() : base(14, 2021, "Extended Polymerization", false)
        {

        }

        protected override void Preprocess()
        {
            var input = Input.SplitByBlankLine();

            var template = input[0];
            for (int i = 0; i < template.Length - 1; i++)
            {
                AddPair(template[i..(i + 2)], 1);
            }

            _charCounts = template
                .GroupBy(g => g)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            _rules = input[1].SplitByNewline()
                .Select(s => s.Split(" -> "))
                .ToDictionary(s => s[0], s => s[1][0]);
        }

        protected override object SolvePartOne()
        {
            while (_steps < 10)
            {
                _steps++;
                Step();
            }

            return CountPolymers();
        }

        protected override object SolvePartTwo()
        {
            while (_steps < 40)
            {
                _steps++;
                Step();
            }

            return CountPolymers();
        }

        private void Step()
        {
            var clone = _polymers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            _polymers.Clear();
            foreach ((string key, long value) in clone)
            {
                var c = _rules[key];
                var newish = key.Insert(1, c.ToString());

                _charCounts.AddOrUpdate(c, value, (exiting, count) => count + value);
                AddPair(newish[0..2], value);
                AddPair(newish[1..3], value);
            }
        }

        private void AddPair(string pair, long toAdd)
        {
            _polymers.AddOrUpdate(pair, toAdd, (key, value) => value + toAdd);
        }

        private long CountPolymers()
        {
            return _charCounts.Max(kvp => kvp.Value) - _charCounts.Min(kvp => kvp.Value);
        }

        protected override string LoadDebugInput()
        {
            return @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";
        }
    }
}
