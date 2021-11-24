using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day01 : ASolution
    {
        public Day01() :
            base(1, 2020, "Report Repair")
        {
        }

        protected override object SolvePartOne()
        {
            var numSet = Input.ToIntArray().ToHashSet();
            foreach (var num in numSet)
            {
                if (numSet.Contains(2020 - num))
                    return (num * (2020 - num)).ToString();
            }

            throw new KeyNotFoundException();
        }

        protected override object SolvePartTwo()
        {
            var numSet = Input.ToIntArray().ToHashSet();
            var numList = numSet.ToList();

            Dictionary<int, (int, int)> pairs = new();
            for (int i = 0; i < numList.Count - 1; i++)
            {
                for (int j = i + 1; j < numList.Count; j++)
                {
                    int one = numList[i];
                    int two = numList[j];
                    pairs[one + two] = (one, two);
                }
            }

            foreach (var num in numSet)
            {
                if (pairs.ContainsKey(2020 - num))
                {
                    (int x, int y) = pairs[2020 - num];
                    return (num * x * y).ToString();
                }
            }
            throw new KeyNotFoundException();
        }
    }
}