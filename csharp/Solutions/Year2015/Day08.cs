namespace AdventOfCode.Solutions.Year2015
{
    internal class Day08 : ASolution
    {
        public Day08() : base(08, 2015, "Matchsticks", false)
        {

        }

        protected override void Preprocess()
        {
            base.Preprocess();
        }

        protected override object SolvePartOne()
        {
            var chars =
                InputByNewLine
                .Select(x => Regex.Replace(x, @"\\", @"\"))
                .Select(x => Regex.Replace(x, @"\""", @""""))
                .Select(x => Regex.Replace(x, @"\\x([0-9a-f]{2})", HexReplacer))
                .ToArray();

            return null;
        }

        private string HexReplacer(Match match)
        {
            return ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString();
        }

        protected override object SolvePartTwo()
        {
            return null;
        }

        protected override string LoadDebugInput()
        {
            return @"""""
""abc""
""aaa\""aaa""
""\x27""";
        }

        
    }
}
