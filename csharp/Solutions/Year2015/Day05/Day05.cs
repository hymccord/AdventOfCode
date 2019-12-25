using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2015.Day05
{
    class Day05 : ASolution
    {
        public Day05() : base(5, 2015, "")
        {
        }

        protected override string SolvePartOne()
        {
            int c = 0;
            foreach (var s in Input.SplitByNewline())
            {
                var v = Regex.Matches(s, @"[aeiou]").Count >= 3;
                var d = Regex.IsMatch(s, @"(\w)\1");
                var n = !Regex.IsMatch(s, @"(ab|cd|pq|xy)");

                if (v && d && n)
                    c++;
            }

            return c.ToString();
        }
        string test = @"qjhvhtzxzqqjkmpb
xxyxx
uurcxstgmygtbstg
ieodomkazucvgmuy
";

        protected override string SolvePartTwo()
        {
            int c = 0;
            foreach (var s in Input.SplitByNewline())
            {
                var d = Regex.IsMatch(s, @"(\w{2}).*\1");
                var n = Regex.IsMatch(s, @"(\w)\w\1");

                if (d && n)
                    c++;
            }

            return c.ToString();
        }
    }
}
