using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019 {

    class Day05 : ASolution {
        
        public Day05() : base(5, 2019, "") {
            
        }

        protected override object SolvePartOne() {
            var output = new List<long>();
            string s = Input.Trim('\n');
            IntCode cpu = IntCode.Create(s);
            cpu.GetInput += (i) => 1;
            cpu.Output += (s, e) =>
            {
                output.Add(e);
                System.Console.WriteLine(e);
            };
            cpu.Run();
            return output.Last().ToString(); 
        }

        protected override object SolvePartTwo() {
            var output = new List<long>();
            string s = Input.Trim('\n');
            IntCode cpu = IntCode.Create(s);
            cpu.GetInput += (i) => 5;
            cpu.Output += (s, e) => output.Add(e);
            cpu.Run();
            return output.Last().ToString();
        }
    }
}
