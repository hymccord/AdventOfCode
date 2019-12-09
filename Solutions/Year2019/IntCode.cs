using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2019
{
    internal class IntCode
    {
        public Func<int, int> GetInput { get; set; } = delegate { return 0; };

        public event EventHandler Halt;
        public event EventHandler<int> Output;

        ReadOnlyMemory<int> program;
        readonly IntCodeInstruction[] _opCodeHandlers;

        internal State state;
        internal bool askInput = false;

        public int[] ByteCode => state.memory;

        public int InstructionPointer { get; internal set; }

        internal struct State
        {
            public int[] memory;
            public int inputRequest;
        }

        static IntCode()
        {
            _ = InstructionOpCounts.OpCount;
        }

        IntCode(string input)
        {
            _opCodeHandlers = OpCodeHandlersTable.Handlers;
            program = input.Split(',').Select(int.Parse).ToArray();
            state.memory = new int[program.Length];
            program.CopyTo(state.memory);
        }

        public static IntCode Create(string input)
        {
            return new IntCode(input);
        }

        public void Reset()
        {
            InstructionPointer = 0;
            state.inputRequest = 0;
            program.CopyTo(state.memory);
        }

        public void Run()
        {
            int opCode;
            while (true)
            {
                opCode = Read();
                if (opCode % 100 == 99)
                {
                    Halt?.Invoke(this, EventArgs.Empty);
                    break;
                }

                Decode(_opCodeHandlers[opCode % 100], opCode / 100);
            }
        }

        private void Decode(IntCodeInstruction handler, int parameterMode)
        {
            handler.Execute(this, parameterMode);
        }

        private int Read()
        {
            return state.memory[InstructionPointer++];
        }

        internal void RaiseOutput(int s) => Output?.Invoke(this, s);
    }
    abstract class IntCodeInstruction
    {
        public IntCodeInstruction(OpCode code)
        {
            Code = code;
        }
        public OpCode Code { get; internal set; }

        public int OpCount => InstructionOpCounts.OpCount[(int)Code];

        public abstract void Execute(IntCode decoder, int parameterMode);

        protected int GetParameter(int mode, int pointer, int[] byteCode)
        {
            return mode == 0 ? byteCode[byteCode[pointer]] : byteCode[pointer];
        }

        protected void SetParameter(int mode, int pointer, int[] byteCode, int value)
        {
            if (mode == 0)
            {
                byteCode[byteCode[pointer]] = value;
            }
            else
            {
                byteCode[pointer] = value;
            }
        }
    }

    sealed class halt : IntCodeInstruction
    {
        public halt() : base(OpCode.halt)
        {
        }

        public override void Execute(IntCode decoder, int parameterMode) { }
    }

    sealed class invalid : IntCodeInstruction
    {
        public static readonly invalid Instance = new invalid();
        invalid() : base(OpCode.invalid) { }
        public override void Execute(IntCode decoder, int parameterMode) => throw new Exception();
    }


    class addm : IntCodeInstruction
    {
        public addm() : base(OpCode.addr)
        {

        }
        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;
            int p1 = parameterMode % 2;
            int p2 = parameterMode / 10 % 2;
            int p3 = parameterMode / 100 % 2;
            int m1 = GetParameter(p1, decoder.InstructionPointer++, state.memory);
            int m2 = GetParameter(p2, decoder.InstructionPointer++, state.memory);

            SetParameter(p3, decoder.InstructionPointer++, state.memory, m1 + m2);
        }
    }

    class mulm : IntCodeInstruction
    {
        public mulm() : base(OpCode.mulr)
        {

        }
        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;
            int p1 = parameterMode % 2;
            int p2 = parameterMode / 10 % 2;
            int p3 = parameterMode / 100 % 2;
            int m1 = GetParameter(p1, decoder.InstructionPointer++, state.memory);
            int m2 = GetParameter(p2, decoder.InstructionPointer++, state.memory);

            SetParameter(p3, decoder.InstructionPointer++, state.memory, m1 * m2);
        }
    }

    class input : IntCodeInstruction
    {
        public input() : base(OpCode.input)
        {

        }

        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;
            int p1 = parameterMode % 2;

            if (decoder.askInput) Console.Write("Input: ");
            SetParameter(p1, decoder.InstructionPointer++, state.memory, decoder.GetInput(state.inputRequest++));

            //state.instructionsRun.Add("Store")
        }
    }

    class output : IntCodeInstruction
    {
        public output() : base(OpCode.output)
        {

        }

        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;

            decoder.RaiseOutput(GetParameter(parameterMode % 2, decoder.InstructionPointer++, state.memory));
        }
    }

    class jmptrue : IntCodeInstruction
    {
        public jmptrue() : base(OpCode.jmptrue)
        {

        }

        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;
            int p1 = parameterMode % 2;
            int p2 = parameterMode / 10 % 2;
            int m1 = GetParameter(p1, decoder.InstructionPointer++, state.memory);
            int m2 = GetParameter(p2, decoder.InstructionPointer++, state.memory);

            if (m1 != 0)
                decoder.InstructionPointer = m2;
        }
    }

    class jmpfalse : IntCodeInstruction
    {
        public jmpfalse() : base(OpCode.jmpfalse)
        {

        }

        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;
            int p1 = parameterMode % 2;
            int p2 = parameterMode / 10 % 2;
            int m1 = GetParameter(p1, decoder.InstructionPointer++, state.memory);
            int m2 = GetParameter(p2, decoder.InstructionPointer++, state.memory);

            if (m1 == 0)
                decoder.InstructionPointer = m2;
        }
    }

    class slt : IntCodeInstruction
    {
        public slt() : base(OpCode.slt)
        {

        }

        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;
            int p1 = parameterMode % 2;
            int p2 = parameterMode / 10 % 2;
            int p3 = parameterMode / 100 % 2;
            int m1 = GetParameter(p1, decoder.InstructionPointer++, state.memory);
            int m2 = GetParameter(p2, decoder.InstructionPointer++, state.memory);

            SetParameter(p3, decoder.InstructionPointer++, state.memory, m1 < m2 ? 1 : 0);
        }
    }

    class eq : IntCodeInstruction
    {
        public eq() : base(OpCode.eq)
        {

        }

        public override void Execute(IntCode decoder, int parameterMode)
        {
            ref var state = ref decoder.state;
            int p1 = parameterMode % 2;
            int p2 = parameterMode / 10 % 2;
            int p3 = parameterMode / 100 % 2;
            int m1 = GetParameter(p1, decoder.InstructionPointer++, state.memory);
            int m2 = GetParameter(p2, decoder.InstructionPointer++, state.memory);

            SetParameter(p3, decoder.InstructionPointer++, state.memory, m1 == m2 ? 1 : 0);
        }
    }

    enum OpCode
    {
        invalid,
        addr,
        mulr,
        input,
        output,
        jmptrue,
        jmpfalse,
        slt,
        eq,
        halt = 99,
    }

    static class DecoderConstants
    {
        public const int NumberOfCodeValues = 100;
    }

    static class InstructionOpCounts
    {
        internal static byte[] OpCount = new byte[DecoderConstants.NumberOfCodeValues]
        {
            0,//invalid
            3,//addr
            3,//mulr
            1,//input
            1,//output
            2,//jmptrue
            2,//jmpfalse
            3,//slt
            3,//eq
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//invalid
            0,//halt
        };
    }

    static class OpCodeHandlersTable
    {
        internal static readonly IntCodeInstruction[] Handlers;

        static OpCodeHandlersTable()
        {
            var invalid = Year2019.invalid.Instance;

            Handlers = new IntCodeInstruction[100]
            {
                invalid,
                new addm(),
                new mulm(),
                new input(),
                new output(),
                new jmptrue(),
                new jmpfalse(),
                new slt(),
                new eq(),
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                invalid,
                new halt(),
            };
        }
    }
}
