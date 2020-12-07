using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020.Day07
{
    class Day07 : ASolution
    {
        Dictionary<string, Dictionary<string, int>> _bags = new();

        private string test = @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";

        public Day07()
            : base(07, 2020, "Handy Haversacks")
        {
        }

        protected override object SolvePartOne()
        {

            foreach (var line in Input.SplitByNewline())
            {
                var outer = line.Split(' ').Take(2).JoinAsStrings(" ");
                var inner = Regex.Matches(line, @"((\d+) (\w+ \w+))").ToDictionary(m => m.Groups[3].Value, m => int.Parse(m.Groups[2].Value));
                _bags.Add(outer, inner);
            }

            HashSet<string> processed = new();
            Queue<string> queue = new();
            queue.Enqueue("shiny gold");

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                processed.Add(current);

                foreach (var bag in _bags)
                {
                    if (bag.Value.ContainsKey(current))
                        queue.Enqueue(bag.Key);
                }
            }
            return (processed.Count - 1).ToString();
        }

        protected override object SolvePartTwo()
        {
            long Process(long i, string color)
            {
                long j = 0;
                foreach (var item in _bags[color])
                {
                    j += item.Value + Process(item.Value, item.Key);
                }
                return j * i;
            }

            return Process(1, "shiny gold").ToString();
        }

        [DebuggerDisplay("")]
        class Bag
        {
            public Bag(string color)
            {

            }
        }
    }
}
