using System.Text;

namespace AdventOfCode.Solutions.Year2018
{
    internal class Day12 : ASolution
    {
        public Day12() : base(12, 2018, "")
        {

        }

        Dictionary<string, string> mutations = new Dictionary<string, string>();
        protected override object SolvePartOne()
        {
            var input = Input.SplitByNewline();
            string initialState = input[0].Split()[2];
            for (int i = 1; i < input.Length; i++)
            {
                string[] line = input[i].Split();
                mutations[line[0]] = line[2];
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder nextGen = new StringBuilder();

            int gen = 0;
            string current = initialState;
            int zeroOff = 0;
            while (gen++ < 20)
            {
                current = "...." + current + "....";
                zeroOff += 2;
                for (int i = 4; i < current.Length; i++)
                {
                    string window = current.Substring(i - 4, 5);

                    if (mutations.ContainsKey(window))
                    {
                        sb.Append(mutations[window]);
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }
                current = sb.ToString();
                Console.WriteLine($"{gen}: {current} {Score(current, zeroOff)}");
                sb.Clear();
            }


            return Score(current, zeroOff);
        }

        protected override object SolvePartTwo()
        {
            var input = Input.SplitByNewline();
            string initialState = input[0].Split()[2];
            for (int i = 1; i < input.Length; i++)
            {
                string[] line = input[i].Split();
                mutations[line[0]] = line[2];
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder nextGen = new StringBuilder();

            long gen = 0;
            string current = initialState;
            int zeroOff = 0;
            while (gen++ < 50_000_000_000)
            {
                current = "...." + current + "....";
                for (int i = 4; i < current.Length; i++)
                {
                    string window = current.Substring(i - 4, 5);

                    if (mutations.ContainsKey(window))
                    {
                        sb.Append(mutations[window]);
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }
                current = sb.ToString();
                int first = current.IndexOf('#');
                int last = current.LastIndexOf('#');
                current = current.Substring(first, last - first + 1);
                zeroOff += 2 - first;

                long score = Score(current, zeroOff);
                Console.WriteLine($"{gen}: {current} {zeroOff} {score}");
                sb.Clear();
            }

            return null;
        }

        private long Score(string current, int zeroOffset) => current.Select((c, i) => c == '#' ? (long)i - zeroOffset : 0).Sum();

        private class Cell
        {
            public int Index { get; set; }
            public char Value { get; set; }
        }
    }
}