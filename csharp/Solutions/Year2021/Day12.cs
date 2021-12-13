using System.Diagnostics;
using System.IO;

namespace AdventOfCode.Solutions.Year2021
{
    internal class Day12 : ASolution
    {
        Dictionary<string, HashSet<string>> _neighbors = new();

        public Day12() : base(12, 2021, "Passage Pathing", false)
        {

        }

        protected override void Preprocess()
        {
            foreach (var line in InputByNewLine)
            {
                var lr = line.Split('-');
                var left = lr[0];
                var right = lr[1];

                var leftNode = _neighbors.GetValueOrDefault(left, new HashSet<string>());
                var rightnode = _neighbors.GetValueOrDefault(right, new HashSet<string>());
                leftNode.Add(right);
                rightnode.Add(left);

                _neighbors[left] = leftNode;
                _neighbors[right] = rightnode;
            }
        }

        protected override object SolvePartOne()
        {
            return CountPaths("start", new HashSet<string>(), false);            
        }

        protected override object SolvePartTwo()
        {
            return CountPaths("start", new HashSet<string>(), true);
        }

        private int CountPaths(string cave, HashSet<string> visited, bool reuseOneCave)
        {
            visited.Add(cave);

            if (cave == "end")
            {
                return 1;
            }


            var sum = 0;
            foreach (var child in _neighbors[cave])
            {
                if (child == "start")
                {
                    continue;
                }

                if (visited.Contains(child) && child.ToUpper() != child)
                {
                    if (reuseOneCave && child.ToLower() == child)
                    {
                        sum += CountPaths(child, new HashSet<string>(visited), false);
                    }
                    continue;
                }

                sum += CountPaths(child, new HashSet<string>(visited), reuseOneCave);
            }

            return sum;
        }

        protected override string LoadDebugInput()
        {
            return @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW";
            return @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";
            return @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";
        }

        [DebuggerDisplay("{Value}")]
        class Cell
        {
            public Cell(string value)
            {
                Value = value;

                if (value == "start")
                {
                    IsStart = true;
                }
                else if (value == "end")
                {
                    IsEnd = true;
                }
                else
                {
                    IsSmallCave = value.Any(char.IsLower);
                }
            }
            public string Value { get; }
            public bool IsSmallCave { get; }
            public bool IsStart { get; }
            public bool IsEnd { get; }
        }
    }
}
