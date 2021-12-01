using System.Collections.Concurrent;

namespace AdventOfCode.Solutions.Year2018
{
    class Day04 : ASolution
    {
        public Day04() : base(04, 2018, "")
        {

        }

        protected override object SolvePartOne()
        {
            string[] input = Input.SplitByNewline().ToArray();
            Array.Sort(input);

            Dictionary<int, ConcurrentDictionary<int, int>> sleepyBois = new Dictionary<int, ConcurrentDictionary<int, int>>();

            int id = 0;
            int sleepStart = 0;
            int sleepEnd = 0;
            foreach (var line in input)
            {
                var date = line.Substring(1, line.IndexOf(']') - 1);
                var time = DateTime.Parse(date);

                var guardMatch = Regex.Match(line, @"#(\d+)");
                var sleepy = line.IndexOf("falls") > 0;
                var awake = line.IndexOf("wakes") > 0;

                if (guardMatch.Success)
                {
                    id = int.Parse(guardMatch.Groups[1].Value);
                }
                else if (sleepy)
                {
                    sleepStart = time.Minute;
                }
                else if (awake)
                {
                    sleepEnd = time.Minute;
                    if (!sleepyBois.ContainsKey(id))
                    {
                        sleepyBois[id] = new ConcurrentDictionary<int, int>();
                    }

                    Enumerable.Range(sleepStart, sleepEnd - sleepStart).AsParallel().ForAll(i => sleepyBois[id].AddOrUpdate(i, 1, (key, value) => value + 1));
                }
            }

            int sleepyBoiID = sleepyBois.Select(kvp => new { id = kvp.Key, sum = kvp.Value.Sum(kvp2 => kvp2.Value) }).Aggregate((i1, i2) => i1.sum > i2.sum ? i1 : i2).id;
            Console.WriteLine($"Sleepy Boi: #{sleepyBoiID}");

            int mostSleepMin = sleepyBois[sleepyBoiID].Select(kvp => new { min = kvp.Key, sum = kvp.Value }).Aggregate((i1, i2) => i1.sum > i2.sum ? i1 : i2).min;
            Console.WriteLine($"Sleepy Min: {mostSleepMin}");

            return sleepyBoiID * mostSleepMin;
        }

        protected override object SolvePartTwo()
        {
            string[] input = Input.SplitByNewline().ToArray();
            Array.Sort(input);

            Dictionary<int, ConcurrentDictionary<int, int>> sleepyBois = new Dictionary<int, ConcurrentDictionary<int, int>>();

            int id = 0;
            int sleepStart = 0;
            int sleepEnd = 0;
            foreach (var line in input)
            {
                var date = line.Substring(1, line.IndexOf(']') - 1);
                var time = DateTime.Parse(date);

                var guardMatch = Regex.Match(line, @"#(\d+)");
                var sleepy = line.IndexOf("falls") > 0;
                var awake = line.IndexOf("wakes") > 0;

                if (guardMatch.Success)
                {
                    id = int.Parse(guardMatch.Groups[1].Value);
                }
                else if (sleepy)
                {
                    sleepStart = time.Minute;
                }
                else if (awake)
                {
                    sleepEnd = time.Minute;
                    if (!sleepyBois.ContainsKey(id))
                    {
                        sleepyBois[id] = new ConcurrentDictionary<int, int>();
                    }

                    Enumerable.Range(sleepStart, sleepEnd - sleepStart).AsParallel().ForAll(i => sleepyBois[id].AddOrUpdate(i, 1, (key, value) => value + 1));
                }
            }

            var sleepyBoi = sleepyBois.Select(kvp => new { id = kvp.Key, minIdValue = kvp.Value.Aggregate((i1, i2) => i1.Value > i2.Value ? i1 : i2) }).Aggregate((i1, i2) => i1.minIdValue.Value > i2.minIdValue.Value ? i1 : i2);
            Console.WriteLine($"Sleepy Boi: #{sleepyBoi.id}->{sleepyBoi.minIdValue.Key}");

            return sleepyBoi.id * sleepyBoi.minIdValue.Key;
        }

        const string test =
@"
[1518-11-01 00:00] Guard #10 begins shift
[1518-11-01 00:05] falls asleep
[1518-11-01 00:25] wakes up
[1518-11-01 00:30] falls asleep
[1518-11-01 00:55] wakes up
[1518-11-01 23:58] Guard #99 begins shift
[1518-11-02 00:40] falls asleep
[1518-11-02 00:50] wakes up
[1518-11-03 00:05] Guard #10 begins shift
[1518-11-03 00:24] falls asleep
[1518-11-03 00:29] wakes up
[1518-11-04 00:02] Guard #99 begins shift
[1518-11-04 00:36] falls asleep
[1518-11-04 00:46] wakes up
[1518-11-05 00:03] Guard #99 begins shift
[1518-11-05 00:45] falls asleep
[1518-11-05 00:55] wakes up
";
    }
}
