using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2015
{
    class Day02 : ASolution
    {
        public Day02() : base(2, 2015, "")
        {

        }

        protected override string SolvePartOne()
        {
            var total = 0;
            foreach (string line in Input.SplitByNewline())
            {
                int[] lwh = line.Split('x').Select(int.Parse).ToArray();
                var a = lwh[0] * lwh[1];
                var b = lwh[1] * lwh[2];
                var c = lwh[0] * lwh[2];
                total += 2 * a + 2 * b + 2 * c + new int[] { a, b, c}.Min();
            }

            return total.ToString();
        }

        protected override string SolvePartTwo()
        {
            var total = 0;
            foreach (string line in Input.SplitByNewline())
            {
                int[] lwh = line.Split('x').Select(int.Parse).ToArray();
                Array.Sort(lwh);

                total += lwh[0] * 2 + lwh[1] * 2 + lwh.Aggregate(1, (a, b) => a * b);
            }

            return total.ToString();
        }
    }
}
