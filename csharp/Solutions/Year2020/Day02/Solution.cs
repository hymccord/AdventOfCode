using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day02 : ASolution
    {
        private List<(int, int, char, string)> _lines;

        public Day02() :
            base(2, 2020, "Password Philosophy")
        {
            _lines = Input.SplitByNewline().Select(s =>
            {
                var line = s.Split(new char[] { '-', ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return (int.Parse(line[0]), int.Parse(line[1]), line[2][0], line[3]);
            }).ToList();
        }

        protected override string SolvePartOne()
        {
            return _lines.Count(l =>
            {
                int occurs = l.Item4.Count(c => c == l.Item3);
                return occurs >= l.Item1 && occurs <= l.Item2;
            }).ToString();
        }

        protected override string SolvePartTwo()
        {
            return _lines.Count(l =>
            {
                return l.Item4[l.Item1 - 1] == l.Item3 ^ l.Item4[l.Item2 - 1] == l.Item3;
            }).ToString();
        }
    }
}