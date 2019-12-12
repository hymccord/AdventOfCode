using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2019
{
    internal class IntCode
    {
        public Func<long, long> GetInput { get; set; } = delegate { return 0; };

        public event EventHandler Halt;
        public event EventHandler<long> Output;

        long[] program;
        readonly IntCodeInstruction[] _opCodeHandlers;

        internal State state;
        internal bool askInput = false;

        public Dictionary<long, long> ByteCode => state.memory;

        public long InstructionPointer { get; internal set; }

        internal struct State
        {
            public Dictionary<long, long> memory;
            public long inputRequest;
            public long relativeBase;
        }

        static IntCode()
        {
            _ = InstructionOpCounts.OpCount;
        }

        IntCode(string input)
        {
            _opCodeHandlers = OpCodeHandlersTable.Handlers;
            program = input.Split(',').Select(long.Parse).ToArray();
            state.memory = program.Select((v, i) => new { v, i }).ToDictionary(a => (long)a.i, a => a.v);
        }

        public static IntCode Create(string input)
        {
            return new IntCode(input);
        }

        public void Reset()
        {
            InstructionPointer = 0;
            state = new State();
            state.memory = program.Select((v, i) => new { v, i }).ToDictionary(a => (long)a.i, a => a.v);
        }

        public void Run()
        {
            long opCode;
            while (true)
            {
                opCode = Read();
                if (opCode % DecoderConstants.NumberOfCodeValues == 99)
                {
                    Halt?.Invoke(this, EventArgs.Empty);
                    break;
                }

                Decode(_opCodeHandlers[opCode % 100], opCode);
            }
        }

        private void Decode(IntCodeInstruction handler, long opCode)
        {
            handler.Execute(this, opCode);
        }

        private long Read()
        {
            return state.memory[InstructionPointer++];
        }

        internal void RaiseOutput(long s) => Output?.Invoke(this, s);

        internal long GetParameter(long parameterIndex, long opCode)
        {
            int index = (int)Math.Pow(10, 2 + parameterIndex);
            var mode = (ParameterMode)(opCode / index % 10);
            long value;
            switch (mode)
            {
                case ParameterMode.Position:
                    value = GetMemory(GetMemory(InstructionPointer));
                    break;
                case ParameterMode.Immediate:
                    value = GetMemory(InstructionPointer);
                    break;
                case ParameterMode.Relative:
                    value = GetMemory(state.relativeBase + GetMemory(InstructionPointer));
                    break;
                default:
                    throw new NotImplementedException();
            }
            InstructionPointer++;
            return value;
        }

        internal void SetParameter(long parameterIndex, long parameterMode, long value)
        {
            int index = (int)Math.Pow(10, 2 + parameterIndex);
            var mode = (ParameterMode)(parameterMode / index % 10);
            switch (mode)
            {
                case ParameterMode.Position:
                    SetMemory(GetMemory(InstructionPointer), value);
                    break;
                case ParameterMode.Immediate:
                    SetMemory(InstructionPointer, value);
                    break;
                case ParameterMode.Relative:
                    SetMemory(state.relativeBase + GetMemory(InstructionPointer), value);
                    break;
                default:
                    throw new NotImplementedException();
            }
            InstructionPointer++;
        }

        private long GetMemory(long index)
        {
            if (!state.memory.ContainsKey(index))
                state.memory[index] = 0;

            return state.memory[index];
        }

        private void SetMemory(long index, long value)
        {
            state.memory[index] = value;
        }
    }
    abstract class IntCodeInstruction
    {
        public IntCodeInstruction(OpCode code)
        {
            Code = code;
        }
        public OpCode Code { get; internal set; }

        public long OpCount => InstructionOpCounts.OpCount[(long)Code];

        public abstract void Execute(IntCode decoder, long parameterMode);

        protected long GetParameter(long mode, long pointer, long[] byteCode)
        {
            return mode == 0 ? byteCode[byteCode[pointer]] : byteCode[pointer];
        }

        protected void SetParameter(long mode, long pointer, long[] byteCode, long value)
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

        public override void Execute(IntCode decoder, long parameterMode) { }
    }

    sealed class invalid : IntCodeInstruction
    {
        public static readonly invalid Instance = new invalid();
        invalid() : base(OpCode.invalid) { }
        public override void Execute(IntCode decoder, long parameterMode) => throw new Exception();
    }


    class addm : IntCodeInstruction
    {
        public addm() : base(OpCode.addr)
        {

        }
        public override void Execute(IntCode decoder, long parameterMode)
        {
            ref var state = ref decoder.state;
            long m1 = decoder.GetParameter(0, parameterMode);
            long m2 = decoder.GetParameter(1, parameterMode);

            decoder.SetParameter(2, parameterMode, m1 + m2);
        }
    }

    class mulm : IntCodeInstruction
    {
        public mulm() : base(OpCode.mulr)
        {

        }
        public override void Execute(IntCode decoder, long parameterMode)
        {
            long m1 = decoder.GetParameter(0, parameterMode);
            long m2 = decoder.GetParameter(1, parameterMode);

            decoder.SetParameter(2, parameterMode, m1 * m2);
        }
    }

    class input : IntCodeInstruction
    {
        public input() : base(OpCode.input)
        {

        }

        public override void Execute(IntCode decoder, long parameterMode)
        {
            ref var state = ref decoder.state;
            long p1 = parameterMode % 2;

            if (decoder.askInput) Console.Write("Input: ");
            decoder.SetParameter(0, parameterMode, decoder.GetInput(state.inputRequest++));

            //state.instructionsRun.Add("Store")
        }
    }

    class output : IntCodeInstruction
    {
        public output() : base(OpCode.output)
        {

        }

        public override void Execute(IntCode decoder, long parameterMode)
        {
            ref var state = ref decoder.state;

            decoder.RaiseOutput(decoder.GetParameter(0, parameterMode));
        }
    }

    class jmptrue : IntCodeInstruction
    {
        public jmptrue() : base(OpCode.jmptrue)
        {

        }

        public override void Execute(IntCode decoder, long parameterMode)
        {
            long m1 = decoder.GetParameter(0, parameterMode);
            long m2 = decoder.GetParameter(1, parameterMode);

            if (m1 != 0)
                decoder.InstructionPointer = m2;
        }
    }

    class jmpfalse : IntCodeInstruction
    {
        public jmpfalse() : base(OpCode.jmpfalse)
        {

        }

        public override void Execute(IntCode decoder, long parameterMode)
        {
            long m1 = decoder.GetParameter(0, parameterMode);
            long m2 = decoder.GetParameter(1, parameterMode);

            if (m1 == 0)
                decoder.InstructionPointer = m2;
        }
    }

    class slt : IntCodeInstruction
    {
        public slt() : base(OpCode.slt)
        {

        }

        public override void Execute(IntCode decoder, long parameterMode)
        {
            long m1 = decoder.GetParameter(0, parameterMode);
            long m2 = decoder.GetParameter(1, parameterMode);

            decoder.SetParameter(2, parameterMode, m1 < m2 ? 1 : 0);
        }
    }

    class eq : IntCodeInstruction
    {
        public eq() : base(OpCode.eq)
        {

        }

        public override void Execute(IntCode decoder, long parameterMode)
        {
            long m1 = decoder.GetParameter(0, parameterMode);
            long m2 = decoder.GetParameter(1, parameterMode);

            decoder.SetParameter(2, parameterMode, m1 == m2 ? 1 : 0);
        }
    }

    class rel : IntCodeInstruction
    {
        public rel() : base(OpCode.rel)
        {

        }

        public override void Execute(IntCode decoder, long parameterMode)
        {
            ref var state = ref decoder.state;

            long m1 = decoder.GetParameter(0, parameterMode);

            state.relativeBase += m1;
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
        rel,
        halt = 99,
    }

    enum ParameterMode
    {
        Position,
        Immediate,
        Relative,
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
            1,//rel
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
                new rel(),
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
