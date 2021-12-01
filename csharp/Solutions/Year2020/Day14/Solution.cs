namespace AdventOfCode.Solutions.Year2020.Day14
{
    class Day14 : ASolution
    {
        private const string Test = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";
        private const string Test2 = @"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1";

        private string mask = "";
        private Dictionary<ulong, ulong> mem = new();
        private Dictionary<ulong, string> mem2 = new();

        public Day14()
            : base(14, 2020, "Docking Data")
        {

        }
        protected override object SolvePartOne()
        {
            return Solve(relative: false);
        }

        protected override object SolvePartTwo()
        {
            return Solve(relative: true);
        }

        private long Solve(bool relative)
        {
            mem.Clear();

            foreach (var line in Input.SplitByNewline())
            {
                Match m;
                switch (line[0..3])
                {
                    case "mem":
                        m = Regex.Match(line, @"mem\[(?<address>\d+)\] = (?<value>\d+)");
                        if (relative)
                        {
                            RelativeWrite(ulong.Parse(m.Groups["address"].Value), ulong.Parse(m.Groups["value"].Value));
                        }
                        else
                        {
                            Write(ulong.Parse(m.Groups["address"].Value), ulong.Parse(m.Groups["value"].Value));
                        }
                        break;
                    case "mas":
                        m = Regex.Match(line, @"mask = (?<mask>\w+)");
                        mask = m.Groups["mask"].Value;
                        break;
                    default:
                        break;
                }
            }

            return mem.Sum(kvp => (long)kvp.Value);
        }

        private void Write(ulong address, ulong value)
        {
            ulong newVal = 0;
            for (int i = 0; i < 36; i++)
            {
                newVal += (mask[35 - i] switch
                {
                    '0' => 0,
                    '1' => 1,
                    'X' => (value >> i) & 1,
                    _ => throw new Exception()
                } << i);
            }

            mem[address] = newVal;
        }

        private void RelativeWrite(ulong address, ulong value)
        {
            int numX = mask.Split('X').Length - 1;

            var addy = Convert.ToString((long)address, 2).PadLeft(36, '0');

            string floatAddress = new string(mask.Select((c, i) =>
            {
                return c switch
                {
                    '0' => addy[i],
                    '1' => '1',
                    'X' => 'X',
                };
            }).ToArray());

            void WriteFloatAddress(string floating, ulong value)
            {
                int xi = floating.IndexOf('X');
                if (xi == -1)
                {
                    mem[Convert.ToUInt64(floating, 2)] = value;
                    return;
                }

                WriteFloatAddress(floating.Remove(xi, 1).Insert(xi, "0"), value);
                WriteFloatAddress(floating.Remove(xi, 1).Insert(xi, "1"), value);
            }

            WriteFloatAddress(floatAddress, value);
        }
    }
}
