using System;
using System.Collections.Generic;
using System.Linq;
//using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2018
{
    internal class Day16 : ASolution
    {
        static Dictionary<int, Action<byte, byte, byte>> opCodeToAction = new Dictionary<int, Action<byte, byte, byte>>();

        public Day16() : base(16, 2018)
        {

        }

        protected override object SolvePartOne()
        {
            var input = Input.SplitByNewline();

            Dictionary<int, HashSet<Action<byte, byte, byte>>> opCodeMap = new Dictionary<int, HashSet<Action<byte, byte, byte>>>();

            int total = 0;
            for (int i = 0; i < 824; i++)
            {
                int[] before = Regex.Match(input[i * 3], @"\[(?<nums>.*?)\]").Groups[1].Value.Split(',').Select(int.Parse).ToArray();
                Instruction instruction = Instruction.Parse(input[i * 3 + 1]);
                int[] after = Regex.Match(input[i * 3 + 2], @"\[(?<nums>.*?)\]").Groups[1].Value.Split(',').Select(int.Parse).ToArray();

                int applies = 0;
                for (int j = 0; j < CPU.OpCodes.Count; j++)
                {
                    var op = CPU.OpCodes[j];
                    CPU.Register = (int[])before.Clone();
                    op(instruction.InputA, instruction.InputB, instruction.Output);

                    if (CPU.Register.SequenceEqual(after))
                    {
                        applies++;

                        if (!opCodeMap.ContainsKey(instruction.OpCode))
                        {
                            opCodeMap.Add(instruction.OpCode, new HashSet<Action<byte, byte, byte>>());
                        }
                        opCodeMap[instruction.OpCode].Add(op);
                    }
                }

                if (applies > 2)
                    total++;
            }

            var tmp = opCodeMap.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());

            while (true)
            {
                var uniqueKVP = tmp.First(kvp => kvp.Value.Count == 1);
                tmp.Remove(uniqueKVP.Key);
                opCodeToAction.Add(uniqueKVP.Key, uniqueKVP.Value.First());

                foreach (var listOfOps in tmp.Values)
                {
                    listOfOps.Remove(uniqueKVP.Value.First());
                }

                if (tmp.Count == 0)
                    break;
            }

            return total;
        }

        protected override object SolvePartTwo()
        {
            var input = Input.SplitByNewline();
            CPU.Register = new int[4];
            for (int i = 824 * 3; i < input.Length; i++)
            {
                Instruction instruction = Instruction.Parse(input[i]);
                opCodeToAction[instruction.OpCode](instruction.InputA, instruction.InputB, instruction.Output);
            }

            return CPU.Register[0];
        }

        class Instruction
        {
            public byte OpCode { get; }
            public byte InputA { get; }
            public byte InputB { get; }
            public byte Output { get; }

            public Instruction(byte opCode, byte inputA, byte inputB, byte output)
            {
                OpCode = opCode;
                InputA = inputA;
                InputB = inputB;
                Output = output;
            }

            public static Instruction Parse(string s)
            {
                byte[] num = s.Split(' ').Select(byte.Parse).ToArray();
                return new Instruction(num[0], num[1], num[2], num[3]);
            }
        }

        static class CPU
        {
            public static int[] Register = new int[4];

            // Addition
            public static Action<byte, byte, byte> addr => (a, b, c) => Register[c] = Register[a] + Register[b];
            public static Action<byte, byte, byte> addi => (a, b, c) => Register[c] = Register[a] + b;

            // Mult
            public static Action<byte, byte, byte> mulr => (a, b, c) => Register[c] = Register[a] * Register[b];
            public static Action<byte, byte, byte> muli => (a, b, c) => Register[c] = Register[a] * b;

            // Bitwise AND
            public static Action<byte, byte, byte> banr => (a, b, c) => Register[c] = Register[a] & Register[b];
            public static Action<byte, byte, byte> bani => (a, b, c) => Register[c] = Register[a] & b;

            // Bitwise AND
            public static Action<byte, byte, byte> borr => (a, b, c) => Register[c] = Register[a] | Register[b];
            public static Action<byte, byte, byte> bori => (a, b, c) => Register[c] = Register[a] | b;

            // Assignment
            public static Action<byte, byte, byte> setr => (a, b, c) => Register[c] = Register[a];
            public static Action<byte, byte, byte> seti => (a, b, c) => Register[c] = a;

            // Greater-than testing
            public static Action<byte, byte, byte> gtir => (a, b, c) => Register[c] = a > Register[b] ? 1 : 0;
            public static Action<byte, byte, byte> gtri => (a, b, c) => Register[c] = Register[a] > b ? 1 : 0;
            public static Action<byte, byte, byte> gtrr => (a, b, c) => Register[c] = Register[a] > Register[b] ? 1 : 0;

            // Equality testing
            public static Action<byte, byte, byte> eqir => (a, b, c) => Register[c] = a == Register[b] ? 1 : 0;
            public static Action<byte, byte, byte> eqri => (a, b, c) => Register[c] = Register[a] == b ? 1 : 0;
            public static Action<byte, byte, byte> eqrr => (a, b, c) => Register[c] = Register[a] == Register[b] ? 1 : 0;

            public static List<Action<byte, byte, byte>> OpCodes { get; }
                = new List<Action<byte, byte, byte>>{
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
                };
        }
    }
}
