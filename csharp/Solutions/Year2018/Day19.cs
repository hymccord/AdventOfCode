
using AdventofCode.ELF_Lang;

namespace AdventOfCode.Solutions.Year2018
{
    internal class Day19 : ASolution
    {
        public Day19() : base(19, 2018)
        {

        }
        protected override object SolvePartOne()
        {
            var input = Input.SplitByNewline();

            CPU.InstructionRegister = int.Parse(input[0].Substring(4, 1));
            CPU.Program = input.Skip(1).Select(Instruction.Parse).ToArray();

            CPU.Execute();

            return CPU.Register[0];
        }

        protected override object SolvePartTwo()
        {
            var input = Input.SplitByNewline();

            CPU.Flush();
            CPU.InstructionRegister = int.Parse(input[0].Substring(4, 1));
            CPU.Program = input.Skip(1).Select(Instruction.Parse).ToArray();
            CPU.Register[0] = 1;

            // 2021 me doesn't remember 2018 AoC me.
            // You gotta decode the program to understand the flow
            CPU.RegRead += (s, e) =>
            {
                if (CPU.Register[CPU.InstructionRegister] == 1)
                {
                    int ret = 0;
                    for (int i = 1; i <= CPU.Register[2]; i++)
                    {
                        if (CPU.Register[2] % i == 0)
                            ret += i;
                    }
                    CPU.Register[0] = ret;

                    // Just throw to break out of program
                    throw new ArithmeticException();
                }
            };

            try
            {
                CPU.Execute();
            }
            catch
            { }

            return CPU.Register[0];
        }

        protected override string LoadDebugInput() => """
            #ip 0
            seti 5 0 1
            seti 6 0 2
            addi 0 1 0
            addr 1 2 3
            setr 1 0 0
            seti 8 0 4
            seti 9 0 5
            """;
    }
}
