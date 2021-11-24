using System;
using System.Collections.Generic;
using System.Linq;

using AdventofCode.ELF_Lang;

namespace AdventOfCode.Solutions.Year2018
{
    internal class Day21 : ASolution
    {
        public Day21() : base(21, 2018)
        {

        }

        protected override object SolvePartOne()
        {
            var input = Input.SplitByNewline();
            CPU.InstructionRegister = int.Parse(input[0].Substring(4, 1));
            CPU.Program = input.Skip(1).Select(Instruction.Parse).ToArray();

            CPU.RegRead += (s, e) =>
            {
                //PART 1;
                if (CPU.Register[CPU.InstructionRegister] == 28)
                {
                    //Console.WriteLine(CPU.Register[3]);
                    CPU.Register[0] = CPU.Register[3];
                    throw new Exception();
                }
            };

            try
            {
                CPU.Execute();
            }
            catch (Exception)
            {}

            return CPU.Register[0];
        }

        protected override object SolvePartTwo()
        {
            HashSet<int> repeats = new HashSet<int>();
            int a = 0;
            int b = 0;

            while (true)
            {
                a = b | 65536;
                b = 0x16F8CB;
                while (true)
                {
                    b += a & 0xff;
                    b &= 0xFF_FFFF;
                    b *= 0x1016B;
                    b &= 0xFF_FFFF;

                    if (a >= 256)
                    {
                        a /= 256;
                        continue;
                    }
                    else
                    {
                        if (!repeats.Contains(b))
                        {
                            repeats.Add(b);
                        }
                        else
                        {
                            //Console.WriteLine(repeats.Last());
                            return repeats.Last();
                        }
                        break;
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}