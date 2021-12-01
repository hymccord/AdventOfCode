namespace AdventOfCode.Solutions.Year2019
{

    class Day09 : ASolution
    {

        public Day09() : base(9, 2019, "")
        {

        }
        string test = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";
        string test2 = "1102,34915192,34915192,7,4,7,99,0";
        string test3 = "104,1125899906842624,99";

        protected override object SolvePartOne()
        {
            string output = null; ;
            var cpu = IntCode.Create(Input);
            cpu.GetInput = (i) => 1;
            cpu.Output += (s, l) =>
            {
                //System.Console.WriteLine(l);
                output = $"{l}";
            };
            cpu.Run();
            return output;
        }

        protected override object SolvePartTwo()
        {
            string output = null; ;
            var cpu = IntCode.Create(Input);
            cpu.GetInput = (i) => 2;
            cpu.Output += (s, l) =>
            {
                //System.Console.WriteLine(l);
                output = $"{l}";
            };
            cpu.Run();
            return output;
        }
    }
}
