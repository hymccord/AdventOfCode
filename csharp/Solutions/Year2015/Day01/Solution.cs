using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions.Year2015
{
    class Day01 : ASolution
    {
        public Day01() : base(1, 2015, "")
        {
        }

        protected override string SolvePartOne()
        {
            var floor = 0;
            foreach (char c in Input)
            {
                if (c == '(')
                    floor++;
                else
                    floor--;

            }
            return floor.ToString();
        }

        protected override string SolvePartTwo()
        {
            var floor = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                char c = Input[i];
                if (c == '(')
                    floor++;
                else
                    floor--;

                if (floor == -1)
                {
                    return (i + 1).ToString();
                }
            }
            return null;
        }
    }
}
