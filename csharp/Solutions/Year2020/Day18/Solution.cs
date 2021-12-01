namespace AdventOfCode.Solutions.Year2020.Day18
{
    class Day18 : ASolution
    {
        private bool _plusPref = false;
        public Day18()
            : base(18, 2020, "Operation Order")
        { }

        protected override object SolvePartOne()
        {
            return Solve();
        }

        protected override object SolvePartTwo()
        {
            _plusPref = true;
            return Solve();
        }

        private long Solve()
        {
            long sum = 0;
            foreach (string line in Input.SplitByNewline())
            {
                sum += long.Parse(EvaluateParens(line));
            }
            return sum;
        }

        private string EvaluateParens(string line)
        {
            // Balancing groups to match parens
            Regex r = new Regex(@"\((?>[^()]+|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))\)");
            // Process from end b/c removing from beginning shifts indexing
            foreach (Match m in r.Matches(line).Reverse())
            {
                string s = EvaluateParens(m.Groups[0].Value[1..^1]);
                line = line.Remove(m.Groups[0].Index, m.Groups[0].Length);
                line = line.Insert(m.Groups[0].Index, s);
            }
            return !_plusPref ? EvalExpression(line) : PlusCalc(line);
        }

        private string EvalExpression(string line)
        {
            string[] expr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            long num = long.Parse(expr[0]);

            for (int i = 1; i < expr.Length - 1; i++)
            {
                switch (expr[i])
                {
                    case "*":
                        num *= long.Parse(expr[i + 1]);
                        break;
                    case "+":
                        num += long.Parse(expr[i + 1]);
                        break;
                }
            }

            return num.ToString();
        }

        private string PlusCalc(string line)
        {
            if (!line.Contains('*'))
                return EvalExpression(line);

            string[] expr = line.Split('*');

            for (int i = 0; i < expr.Length; i++)
            {
                expr[i] = PlusCalc(expr[i]);
            }

            line = string.Join(" * ", expr);

            string num = EvalExpression(line);

            return num.ToString();
        }
    }
}
