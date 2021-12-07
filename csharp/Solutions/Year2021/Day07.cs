namespace AdventOfCode.Solutions.Year2021
{
    internal class Day07 : ASolution
    {
        int[] _crabs;
        int _min;
        int _max;

        public Day07() : base(07, 2021, "The Treachery of Whales", false)
        {

        }

        protected override void Preprocess()
        {
            _crabs = Input.ToIntArray(',');
            _min = _crabs.Min();
            _max = _crabs.Max();
        }

        protected override object SolvePartOne()
        {
            // For inclusive range 1 to 2. Need count: 2 - 1 + 1
            return Enumerable.Range(_min, _max - _min + 1)
                .Min(i => _crabs.Select(c => Math.Abs(c - i)).Sum());
        }

        protected override object SolvePartTwo()
        {
            // Steps vs Energy Sum
            // 1 2 3 4 5
            // 1 3 6 10 15
            // Which is just (x * (x + 1)) / 2
            return Enumerable.Range(_min, _max - _min + 1)
                .Min(i => _crabs.Select(c => Math.Abs(c - i)).Select(c => (c * (c + 1)) >> 1).Sum());
        }

        protected override string LoadDebugInput()
        {
            return @"16,1,2,0,4,2,7,1,2,14";
        }
    }
}
