using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020.Day15
{
    class Day15 : ASolution
    {
        private string Test = @"0,3,6";

        public Day15()
            : base(15, 2020, "Rambunctious Recitation")
        {

        }
        protected override object SolvePartOne()
        {
            return Elvish(2020);
        }

        protected override object SolvePartTwo()
        {
            return Elvish(30_000_000);
        }

        private long Elvish(long gameLength)
        {
            Dictionary<long, long[]> _set = new();
            long[] arr = Input.Split(',').Select(long.Parse).ToArray();
            _ = arr.Select((l, i) => _set[l] = new long[] { i + 1, -1 }).ToArray();

            int turn = _set.Count + 1;
            long lastNum = arr[^1];

            while (turn <= gameLength)
            {
                // First time number was spoken
                if (_set[lastNum][1] == -1)
                {
                    lastNum = 0;
                    _set[lastNum][1] = _set[lastNum][0];
                    _set[lastNum][0] = turn;
                }
                else
                {
                    lastNum = _set[lastNum][0] - _set[lastNum][1];
                    if (!_set.ContainsKey(lastNum))
                        _set[lastNum] = new long[] { turn, -1 };
                    else
                    {
                        _set[lastNum][1] = _set[lastNum][0];
                        _set[lastNum][0] = turn;
                    }
                }

                turn++;
            }

            return lastNum;
        }
    }
}
