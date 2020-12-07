using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day01 : ASolution
    {
        private HashSet<int> _numSet;
        private List<int> _numList;

        public Day01() :
            base(1, 2020, "Report Repair")
        {
            _numSet = Input.SplitByNewline().Select(int.Parse).ToHashSet();
            _numList = _numSet.ToList();
        }

        protected override object SolvePartOne()
        {
            foreach (var num in _numSet)
            {
                if (_numSet.Contains(2020 - num))
                    return (num * (2020 - num)).ToString();
            }

            throw new KeyNotFoundException();
        }

        protected override object SolvePartTwo()
        {
            Dictionary<int, (int, int)> pairs = new();
            for (int i = 0; i < _numList.Count - 1; i++)
            {
                for (int j = i + 1; j < _numList.Count; j++)
                {
                    int one = _numList[i];
                    int two = _numList[j];
                    pairs[one + two] = (one, two);
                }
            }

            foreach (var num in _numSet)
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