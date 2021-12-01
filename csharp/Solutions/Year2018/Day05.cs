using System.Text;

namespace AdventOfCode.Solutions.Year2018
{
    class Day05 : ASolution
    {
        const string test = "dabAcCaCBAcCcaDA";

        public Day05() : base(05, 2018, "")
        {

        }

        protected override object SolvePartOne()
        {
            string input = Input.SplitByNewline().First();
            //string input = SplitInput(test).First();

            input = React(input);

            return input.Length;
        }

        protected override object SolvePartTwo()
        {
            string originalInput = Input.SplitByNewline().First();

            int minL = int.MaxValue;
            for (int i = 0; i < 26; i++)
            {
                StringBuilder sb = new StringBuilder();

                foreach (char c in originalInput)
                {
                    if (!(c == 'a' + i || c == 'A' + i))
                    {
                        sb.Append(c);
                    }
                }
                string s = React(sb.ToString());

                if (s.Length < minL)
                    minL = s.Length;

            }

            return minL;
        }

        private string React(string s)
        {
            for (int i = 0; i < s.Length - 1;)
            {
                if ((s[i] ^ s[i + 1]) == 32)
                {
                    s = s.Remove(i, 2);
                    i--;

                    if (i < 0)
                        i = 0;
                }
                else
                {
                    i++;
                }
            }

            return s;
        }
    }
}
