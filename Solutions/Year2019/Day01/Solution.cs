using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019 {

    class Day01 : ASolution {
        
        public Day01() : base(1, 2019, "") {
            
        }

        protected override string SolvePartOne() {
            return Input.SplitByNewline().Select(int.Parse).Select(i => i / 3 - 2).Sum().ToString();
        }

        protected override string SolvePartTwo() {
            int sum = 0;
            Stack<int> q = new Stack<int>(Input.SplitByNewline().Select(int.Parse));

            while (q.Count > 0)
            {
                int num = q.Pop();
                int fuel = num / 3 - 2;
                if (fuel > 8)
                    q.Push(fuel);

                sum += fuel;
            }

            return sum.ToString();
        }
    }
}
