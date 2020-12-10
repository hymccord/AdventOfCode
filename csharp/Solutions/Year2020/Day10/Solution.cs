using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020.Day10
{
    class Day10 : ASolution
    {
        List<int> _adapters;
        private string Test1 = @"16
10
15
5
1
11
7
19
6
12
4";
        private string Test2 = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";
        public Day10()
            : base(10, 2020, "Adapter Array")
        {

        }

        protected override object SolvePartOne()
        {
            _adapters = Input.SplitByNewline().Select(int.Parse).ToList();
            _adapters.Add(0);
            _adapters.Sort();
            _adapters.Add(_adapters[^1] + 3);

            int[] jolts = new int[4];
            for (int i = 1; i < _adapters.Count; i++)
            {
                int diff = _adapters[i] - _adapters[i - 1];
                jolts[diff] += 1;
            }

            return jolts[1] * jolts[3];
        }

        protected override object SolvePartTwo()
        {
            var paths = _adapters.ToDictionary(i => i, i => 0L);
            paths[0] = 1;

            foreach (var adapter in _adapters.Skip(1))
            {
                for (int i = 1; i < 4; i++)
                {
                    if (paths.ContainsKey(adapter - i))
                    {
                        paths[adapter] += paths[adapter - i];
                    }
                }
            }

            return paths[_adapters[^1]];
        }
    }
}
