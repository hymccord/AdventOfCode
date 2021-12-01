namespace AdventOfCode.Solutions.Year2020.Day06
{
    class Day06 : ASolution
    {

        public Day06()
            : base(06, 2020, "")
        {

        }

        protected override object SolvePartOne()
        {
            // double newline = new group
            return Input.SplitByBlankLine()
                .Aggregate(0, (i, s) =>
                {
                    // Join group into one long string
                    s = s.SplitByNewline().JoinAsStrings();
                    // count unique chars
                    return i + s.ToHashSet().Count;
                }).ToString();
        }

        protected override object SolvePartTwo()
        {
            return Input.SplitByBlankLine()
                .Aggregate(0, (i, s) =>
                {
                    var a = s.SplitByNewline();
                    // if group is just one person, count all letters
                    if (a.Length == 1)
                        return i + a[0].Length;

                    // make one one string of all answers
                    return i + a
                    .JoinAsStrings()
                    .GroupBy(c => c)
                    // count where the letter group is the same length as persons in the group
                    .Count(g => g.Count() == a.Length);

                }).ToString();
        }
    }
}
