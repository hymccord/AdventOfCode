namespace AdventOfCode.Solutions.Year2021
{
    internal class Day08 : ASolution
    {
        string[] _signals;
        string[] _output;

        public Day08() : base(08, 2021, "Seven Segment Search", false)
        {

        }

        protected override void Preprocess()
        {
            var signals = Input.SplitByNewline().Select(
                l =>
                {
                    var lr = l.Split(" | ");
                    return (lr[0], lr[1]);
                }).ToArray();
            _signals = signals.Select(t => t.Item1).ToArray();
            _output = signals.Select(t => t.Item2).ToArray();
        }

        protected override object SolvePartOne()
        {
            return _output
                .Sum(s =>
                    s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                     .Count(c => c.Length == 2 || c.Length == 3 || c.Length == 4 || c.Length == 7)
                );
        }

        Dictionary<int, int[]> _segmentIndicies = new Dictionary<int, int[]>()
        {
            // What segments light up on each letter
            {0, new int[] { 0, 1, 2, 4, 5, 6} },
            {1, new int[] { 2, 5} },
            {2, new int[] { 0, 2, 3, 4, 6} },
            {3, new int[] { 0, 2, 3, 5, 6} },
            {4, new int[] { 1, 2, 3, 5} },
            {5, new int[] { 0, 1, 3, 5, 6} },
            {6, new int[] { 0, 1, 3, 4, 5, 6} },
            {7, new int[] { 0, 2, 5} },
            {8, new int[] { 0, 1, 2, 3, 4, 5, 6} },
            {9, new int[] { 0, 1, 2, 3, 5, 6} },
        };

        protected override object SolvePartTwo()
        {
            /* Sum of all segment usages 0-9. i.e the very top segmement (index 0) is used 8 times from 0-9.
             * Index top to bottom, left to right
             *   8
             * 6   8
             *   7
             * 4   9
             *   7
             */
            int sum = 0;
            for (int i = 0; i < _signals.Length; i++)
            {
                char[] locations = new char[7];

                var line = _signals[i];
                var smoosh = line.Replace(" ", "");
                var lettersWithCounts = smoosh.GroupBy(c => c).Select(g => (g.Key, g.Count()));
                var lettersByCounts = lettersWithCounts.ToLookup(t => t.Item2,  t => t.Key);
                var words = line.Split();

                char[] oneLetters = words.Single(x => x.Length == 2).ToCharArray();
                char[] fourLetters = words.Single(x => x.Length == 4).ToCharArray();

                locations[1] = lettersByCounts[6].First(); // unique segment counts
                locations[4] = lettersByCounts[4].First(); // unique segment counts
                locations[5] = lettersByCounts[9].First(); // unique segment counts
                locations[0] = lettersByCounts[8].Except(oneLetters).Single();     // top segment doesnt share letter with "one" letters
                locations[2] = lettersByCounts[8].Intersect(oneLetters).Single();  // the other 8 count segment does
                locations[6] = lettersByCounts[7].Except(fourLetters).Single();    // bottom most segment doesnt share letter with "four" 
                locations[3] = lettersByCounts[7].Intersect(fourLetters).Single(); // does with other 7

                // Recreate the words using my indices and my deduced wiring
                string[] theNums = _segmentIndicies.Select(kvp => new string(kvp.Value.Select(i => locations[i]).OrderBy(c => c).ToArray())).ToArray();

                // Words match when the sequences are equal (they are both sorted)
                var outputWords = _output[i].Split();
                for (int j = 0; j < 4; j++)
                {
                    var theOutput = outputWords[j].OrderBy(c => c);
                    var pow = Math.Pow(10, 3 - j);

                    for (int k = 0; k < 10; k++)
                    {
                        if (theNums[k].SequenceEqual(theOutput))
                        {
                            sum += (int)pow * k;
                            break;
                        }
                    }
                }
            }

            return sum;
        }

        protected override string LoadDebugInput()
        {
            return @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";
        }
    }
}
