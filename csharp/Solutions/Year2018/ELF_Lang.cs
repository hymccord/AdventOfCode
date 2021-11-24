using System;
using System.Diagnostics;
using System.Linq;

namespace AdventofCode.ELF_Lang
{

    enum OpCode
    {
        addr,
        addi,
        mulr,
        muli,
        banr,
        bani,
        borr,
        bori,
        setr,
        seti,
        gtir,
        gtri,
        gtrr,
        eqir,
        eqri,
        eqrr,
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal abstract class Instruction
    {
        public OpCode OpCode { get; }
        public int InputA { get; }
        public int InputB { get; }
        public int Output { get; }

        public Instruction(OpCode opCode, int inputA, int inputB, int output)
        {
            OpCode = opCode;
            InputA = inputA;
            InputB = inputB;
            Output = output;
        }

        public static Instruction Parse(string s)
        {
            var arr = s.Split(' ');
            Enum.TryParse(arr[0], out OpCode opCode);
            int[] num = s.Split(' ').Skip(1).Select(int.Parse).ToArray();

            return opCode switch
            {
                OpCode.addr => new addr(num[0], num[1], num[2]),
                OpCode.addi => new addi(num[0], num[1], num[2]),
                OpCode.mulr => new mulr(num[0], num[1], num[2]),
                OpCode.muli => new muli(num[0], num[1], num[2]),
                OpCode.banr => new banr(num[0], num[1], num[2]),
                OpCode.bani => new bani(num[0], num[1], num[2]),
                OpCode.borr => new borr(num[0], num[1], num[2]),
                OpCode.bori => new bori(num[0], num[1], num[2]),
                OpCode.setr => new setr(num[0], num[1], num[2]),
                OpCode.seti => new seti(num[0], num[1], num[2]),
                OpCode.gtir => new gtir(num[0], num[1], num[2]),
                OpCode.gtri => new gtri(num[0], num[1], num[2]),
                OpCode.gtrr => new gtrr(num[0], num[1], num[2]),
                OpCode.eqir => new eqir(num[0], num[1], num[2]),
                OpCode.eqri => new eqri(num[0], num[1], num[2]),
                OpCode.eqrr => new eqrr(num[0], num[1], num[2]),
                _ => throw new NotImplementedException(),
            };
        }

        public abstract int Execute();

        private string DebuggerDisplay => $"{OpCode} {InputA} {InputB} {Output}";
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal static class CPU
    {
        public static event EventHandler RegRead;
        public static int InstructionRegister = 0;
        public static int InstructionPointer = 0;
        public static int[] Register = new int[6];
        public static Instruction[] Program;

        public static void Execute()
        {
            int programLength = Program.Length;
            while (InstructionPointer >= 0
                && InstructionPointer < programLength)
            {
                RegRead?.Invoke(null, EventArgs.Empty);

                Register[InstructionRegister] = InstructionPointer;
                Instruction current = Program[InstructionPointer];
                Register[current.Output] = current.Execute();
                InstructionPointer = Register[InstructionRegister];
                InstructionPointer++;
            }
        }

        public static void Flush()
        {
            InstructionRegister = 0;
            InstructionPointer = 0;
            Register = new int[6];
            Program = null;
        }

        private static string DebuggerDisplay
        {
            get
            {
                return string.Format("ip={0} [{1}, {2}, {3}, {4}, {5}, {6}]", InstructionPointer, Register[0], Register[1], Register[2], Register[3], Register[4], Register[5]);
            }
        }
    }

    #region Instructions
    class addr : Instruction
    {
        public addr(int inputA, int inputB, int output) : base(OpCode.addr, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] + CPU.Register[InputB];
    }
    class addi : Instruction
    {
        public addi(int inputA, int inputB, int output) : base(OpCode.addi, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] + InputB;
    }
    class mulr : Instruction
    {
        public mulr(int inputA, int inputB, int output) : base(OpCode.mulr, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] * CPU.Register[InputB];
    }
    class muli : Instruction
    {
        public muli(int inputA, int inputB, int output) : base(OpCode.muli, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] * InputB;
    }
    class banr : Instruction
    {
        public banr(int inputA, int inputB, int output) : base(OpCode.banr, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] & CPU.Register[InputB];
    }
    class bani : Instruction
    {
        public bani(int inputA, int inputB, int output) : base(OpCode.bani, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] & InputB;
    }
    class borr : Instruction
    {
        public borr(int inputA, int inputB, int output) : base(OpCode.borr, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] | CPU.Register[InputB];
    }
    class bori : Instruction
    {
        public bori(int inputA, int inputB, int output) : base(OpCode.bori, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] | InputB;
    }
    class setr : Instruction
    {
        public setr(int inputA, int inputB, int output) : base(OpCode.setr, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA];
    }
    class seti : Instruction
    {
        public seti(int inputA, int inputB, int output) : base(OpCode.seti, inputA, inputB, output) { }
        public override int Execute() => InputA;
    }
    class gtir : Instruction
    {
        public gtir(int inputA, int inputB, int output) : base(OpCode.gtir, inputA, inputB, output) { }
        public override int Execute() => InputA > CPU.Register[InputB] ? 1 : 0;
    }
    class gtri : Instruction
    {
        public gtri(int inputA, int inputB, int output) : base(OpCode.gtri, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] > InputB ? 1 : 0;
    }
    class gtrr : Instruction
    {
        public gtrr(int inputA, int inputB, int output) : base(OpCode.gtrr, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] > CPU.Register[InputB] ? 1 : 0;
    }
    class eqir : Instruction
    {
        public eqir(int inputA, int inputB, int output) : base(OpCode.eqir, inputA, inputB, output) { }
        public override int Execute() => InputA == CPU.Register[InputB] ? 1 : 0;
    }
    class eqri : Instruction
    {
        public eqri(int inputA, int inputB, int output) : base(OpCode.eqri, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] == InputB ? 1 : 0;
    }
    class eqrr : Instruction
    {
        public eqrr(int inputA, int inputB, int output) : base(OpCode.eqrr, inputA, inputB, output) { }
        public override int Execute() => CPU.Register[InputA] == CPU.Register[InputB] ? 1 : 0;
    }
    #endregion
}
