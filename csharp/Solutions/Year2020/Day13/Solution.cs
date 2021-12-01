namespace AdventOfCode.Solutions.Year2020.Day13
{
    class Day13 : ASolution
    {

        public Day13()
            : base(13, 2020, "Shuttle Search")
        {

        }
        protected override object SolvePartOne()
        {
            string[] input = Input.SplitByNewline();
            int _time = int.Parse(input[0]);
            List<int> _busIds = input[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Where(x => x != "x").Select(int.Parse).ToList();

            var bus = _busIds.Select(i =>
            {
                int j = _time / i + 1;
                return new { id = i, wait = j * i - _time };
            }).OrderBy(a => a.wait).First();
            return bus.id * bus.wait;
        }

        protected override object SolvePartTwo()
        {
            string buses = Input.SplitByNewline()[^1];
            var _pairs = buses.Split(',')
                .Select((s, i) => new { i, s })
                .Where(a => a.s != "x")
                .Select(a => new { index = a.i, id = int.Parse(a.s) })
                .ToArray();


            long num = 0;
            long jump = _pairs[0].id;
            for (int i = 1; i < _pairs.Length; i++)
            {
                int index = _pairs[i].index;
                int id = _pairs[i].id;
                while ((num + index) % id != 0)
                {
                    num += jump;
                }
                jump *= id;
            }

            return num;
        }
    }
}
