using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2018
{
    internal class Day01 : ASolution
    {
        public Day01() : base(01, 2018, "")
        {
        }

        protected override object SolvePartOne()
        {
            return Input.ToIntArray().Sum();
        }

        protected override object SolvePartTwo()
        {
            var nums = Input.ToIntArray();
            var set = new HashSet<int>();

            int freq = 0;
            int i = 0;
            while (true)
            {
                freq += nums[i];
                if (set.Contains(freq))
                {
                    break;
                }
                set.Add(freq);
                i++;

                if (i == nums.Length)
                {
                    i = 0;
                }
            }

            return freq;
        }
    }
}
