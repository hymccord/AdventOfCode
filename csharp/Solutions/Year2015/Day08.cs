namespace AdventOfCode.Solutions.Year2015
{
    internal class Day08 : ASolution
    {
        private int _totalChars;

        public Day08() : base(08, 2015, "Matchsticks", false)
        {

        }

        protected override void Preprocess()
        {
            _totalChars = InputByNewLine.Sum(x => x.Length);
        }

        protected override object SolvePartOne()
        {
            var decodedLen = InputByNewLine
                .Select(x => x.Replace("\\\"", @""""))
                .Select(x => x.Replace(@"\\", @"\"))
                .Select(x => Regex.Replace(x, @"\\x([0-9a-f]{2})", HexReplacer))
                .Select(x => x.Length - 2)
                .Sum();

            return _totalChars - decodedLen;
        }

        private string HexReplacer(Match match)
        {
            return ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString();
        }

        protected override object SolvePartTwo()
        {
            var uglyEncodeLen = InputByNewLine.Sum(line =>
            {
                int numCharsToEncode = line.Count(c => c == '\"' || c == '\\');
                // special chars expand by 2 + (numnonspecial chars) + 2 for surrounding quotes
                return numCharsToEncode * 2 + (line.Length - numCharsToEncode) + 2;
            });

            return uglyEncodeLen - _totalChars;
        }

        protected override string LoadDebugInput()
        {
            return """
                ""
                "abc"
                "aaa\"aaa"
                "\x27"
                """;
        }

        
    }
}
