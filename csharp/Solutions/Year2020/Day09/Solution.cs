namespace AdventOfCode.Solutions.Year2020.Day09
{
    class Day09 : ASolution
    {
        private long[] _nums;
        private const int Preamble = 25;

        public Day09()
            : base(09, 2020, "Encoding Error") { }

        protected override object SolvePartOne()
        {
            _nums = Input.SplitByNewline().Select(long.Parse).ToArray();
            for (int i = Preamble; i < (_nums.Length - Preamble); i++)
            {
                long cur = _nums[i];
                bool found = false;
                HashSet<long> set = new HashSet<long>(_nums[(i - Preamble)..i]);
                for (int j = 0; j < Preamble; j++)
                {
                    if (set.Contains(_nums[i] - _nums[i - Preamble + j]))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return _nums[i];
            }

            return null;
        }

        protected override object SolvePartTwo()
        {
            long weakness = long.Parse(Part1);

            // Sliding window only works for positive numbers
            //             head       tail
            //             v          v
            // subsums: [0 1 3 5 7 11 22 33 44 101]
            int head = 0;
            int tail = head + 2;
            long sum = _nums[head..tail].Sum();
            while (tail < _nums.Length)
            {
                // sum too low, increment tail
                while (tail < _nums.Length && sum < weakness)
                {
                    sum += _nums[tail];
                    tail++;
                }
                // sum too great now, increment head
                while (head < tail && sum > weakness)
                {
                    sum -= _nums[head];
                    head++;
                }

                if (sum == weakness)
                    return _nums[head..tail].Min() + _nums[head..tail].Max();
            }
            /* Original bruteforce
            for (int i = 0; i < _nums.Length - 1; i++)
            {
                for (int j = i + 1; j < _nums.Length; j++)
                {
                    if (_nums[i..j].Sum() == weakness)
                        return _nums[i..j].Min() + _nums[i..j].Max();
                }
            }
            */

            return null;
        }
    }
}
