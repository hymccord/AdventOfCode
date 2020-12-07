using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020.Day05
{
    class Day05 : ASolution
    {
        private List<(int, int)> _passes;
        private HashSet<int> _ids;
        private int _max;

        public Day05()
            : base(5, 2020, "Binary Boarding")
        {
            var encoding = Regex.Replace(Input, @"[FL]", "0");
            encoding = Regex.Replace(encoding, @"[BR]", "1");
            _passes = encoding.SplitByNewline()
                .Select(s => (Convert.ToInt32(s.Substring(0, 7), 2), Convert.ToInt32(s.Substring(7, 3), 2)))
                .ToList();
        }

        protected override string SolvePartOne()
        {
            _ids = _passes.Select(p => p.Item1 * 8 + p.Item2).ToHashSet();
            _max = _ids.Max();
            return _max.ToString();
        }

        protected override string SolvePartTwo()
        {
            return Enumerable.Range(0, _max + 1).ToHashSet()
                .First(i => _ids.Contains(i - 1) && !_ids.Contains(i) && _ids.Contains(i + 1)).ToString();
        }
    }
}
