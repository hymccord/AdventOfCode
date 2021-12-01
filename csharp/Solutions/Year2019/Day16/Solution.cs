namespace AdventOfCode.Solutions.Year2019
{

    class Day16 : ASolution
    {

        public Day16() : base(16, 2019, "")
        {

        }

        string test = "12345678";
        string test2 = "80871224585914546619083218645595";
        string test3 = "19617804207202209144916044189917";
        string test4 = "69317163492948606335995924319873";

        int[] patten = new int[] { 0, 1, 0, -1 };
        int pattenLength = 4;
        protected override object SolvePartOne()
        {
            var arr = Input.SplitByNewline().First().ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();
            int[] nextRound = new int[arr.Length];

            for (int phase = 0; phase < 100; phase++)
            {
                Parallel.For(0, arr.Length, (i) =>
                {
                    int sum = 0;
                    var enu = GetPhaseEnumerable(i + 1).GetEnumerator();
                    for (int j = 0; j < arr.Length; j++)
                    {
                        enu.MoveNext();
                        sum += arr[j] * patten[enu.Current];
                    }

                    sum = Math.Abs(sum % 10);
                    nextRound[i] = sum;
                });
                nextRound.CopyTo(arr, 0);
            }
            return arr.Take(8).JoinAsStrings();
        }

        protected override object SolvePartTwo()
        {
            var arr2 = Input.SplitByNewline().First().ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();
            var bigArr = Enumerable.Repeat(arr2, 10000).SelectMany(c => c).ToArray();
            int offset = int.Parse(arr2.Take(7).JoinAsStrings());
            int[] nextRound = new int[bigArr.Length];

            for (int phase = 0; phase < 100; phase++)
            {
                Parallel.For(0, bigArr.Length, (i) =>
                {
                    int sum = 0;
                    var enu = GetPhaseEnumerable(i + 1).GetEnumerator();
                    for (int j = 0; j < bigArr.Length; j++)
                    {
                        enu.MoveNext();
                        sum += bigArr[j] * patten[enu.Current];
                    }

                    sum = Math.Abs(sum % 10);
                    nextRound[i] = sum;
                });
                nextRound.CopyTo(bigArr, 0);
            }
            return bigArr.Skip(offset).Take(8).JoinAsStrings();
        }

        private IEnumerable<int> GetPhaseEnumerable(int phaseNum)
        {
            foreach (var num in Enumerable.Repeat(patten[0], phaseNum - 1))
                yield return num;

            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < phaseNum; j++)
                {
                    yield return i;
                }
            }
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < phaseNum; j++)
                    {
                        yield return i;
                    }
                }
            }
        }
    }
}
