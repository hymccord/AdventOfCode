using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2021
{
    class Day03 : ASolution
    {
        int[] _diagnosticCodes;
        int _codeLength;
        int _mask;

        public Day03() : base(03, 2021, "Binary Diagnostic", false)
        {

        }

        protected override string LoadDebugInput()
        {
            return test;
        }

        protected override void Preprocess()
        {
            _diagnosticCodes = Input.SplitByNewline().Select(x => Convert.ToInt32(x, 2)).ToArray();
            _codeLength = Input.SplitByNewline()[0].Length;
            _mask = (1 << _codeLength) - 1;
        }

        protected override object SolvePartOne()
        {
            int[,] count = GetCommonBits(_diagnosticCodes);

            string gammaString = "";
            for (int i = 0; i < _codeLength; i++)
            {
                gammaString += count[i, 0] > count[i, 1] ? "0" : "1";
            }
            int gamma = Convert.ToInt32(gammaString, 2) & _mask;
            int epsilon = ~gamma & _mask;

            return gamma * epsilon;
        }

        protected override object SolvePartTwo()
        {
            List<int> oxygenCodes = new List<int>(_diagnosticCodes);
            for (int j = 0; j < _codeLength && oxygenCodes.Count > 1; j++)
            {
                int[,] count = GetCommonBits(oxygenCodes);
                List<int> newOxygen = new();

                int keepBit = count[j, 1] >= count[j, 0] ? 1 : 0;
                for (int i = 0; i < oxygenCodes.Count; i++)
                {
                    bool keep = (oxygenCodes[i] >> (_codeLength - j - 1) & 1) == keepBit;

                    if (keep)
                    {
                        newOxygen.Add(oxygenCodes[i]);
                    }
                }
                oxygenCodes = newOxygen;
            }

            List<int> co2Codes = new List<int>(_diagnosticCodes);
            for (int j = 0; j < _codeLength && co2Codes.Count > 1; j++)
            {
                int[,] count = GetCommonBits(co2Codes);
                List<int> newCO2 = new();

                int keepBit = count[j, 1] >= count[j, 0] ? 0 : 1;
                for (int i = 0; i < co2Codes.Count; i++)
                {
                    bool keep = (co2Codes[i] >> (_codeLength - j - 1) & 1) == keepBit;

                    if (keep)
                    {
                        newCO2.Add(co2Codes[i]);
                    }
                }
                co2Codes = newCO2;
            }

            return oxygenCodes[0] * co2Codes[0];
        }

        private int[,] GetCommonBits(IList<int> array)
        {
            int[,] count = new int[_codeLength, 2];
            int length = array.Count;

            for (int i = 0; i < length; i++)
            {
                int code = array[i];
                for (int j = 0; j < _codeLength; j++)
                {
                    int index = (code >> (_codeLength - j - 1)) & 1;
                    count[j, index] += 1;
                }
            }

            return count;
        }

        const string test = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010";
    }
}
