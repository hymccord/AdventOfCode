namespace AdventOfCode.Solutions.Year2021
{
    internal class Day06 : ASolution
    {
        public Day06() : base(06, 2021, "Lanternfish", false)
        { }

        protected override object SolvePartOne()
        {
            return GetDaFish(80);
        }

        protected override object SolvePartTwo()
        {
            return GetDaFish(256);
        }

        private long GetDaFish(int days)
        {
            long[] fish = new long[9];
            foreach (var item in Input.ToIntArray(','))
            {
                fish[item] += 1;
            }

            int day = 0;
            while (day < days)
            {
                long spawn = fish[0];
                for (int i = 1; i <= 8; i++)
                {
                    fish[i - 1] = fish[i];
                }
                fish[6] += spawn;
                fish[8] = spawn;
                day++;
            }

            return fish.Sum();
        }

        protected override string LoadDebugInput()
        {
            return @"3,4,3,1,2";
        }
    }
}
