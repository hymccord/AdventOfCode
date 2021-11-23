
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2018
{
    class Day07 : ASolution
    {
        public Day07() : base(07, 2018, "")
        {

        }
        protected override object SolvePartOne()
        {
            Dictionary<string, SortedList<string, string>> edges = new Dictionary<string, SortedList<string, string>>();
            ConcurrentDictionary<string, int> degreesIn = new ConcurrentDictionary<string, int>();
            foreach (var line in Input.SplitByNewline())
            {
                var words = line.Split();
                var from = words[1];
                var to = words[7];

                if (edges.ContainsKey(from))
                {
                    edges[from].Add(to, to);
                }
                else
                {
                    edges[from] = new SortedList<string, string> { { to, to } };
                }
                degreesIn.AddOrUpdate(to, 1, (c, i) => i + 1);
            }

            List<string> L = new List<string>();
            SortedSet<string> S = new SortedSet<string>(edges.Keys.Except(degreesIn.Keys).OrderByDescending(s => s));

            while (S.Count > 0)
            {
                string n = S.First();
                S.Remove(n);
                L.Add(n);

                if (!edges.ContainsKey(n))
                    continue;

                foreach (var kvp in edges[n])
                {
                    string m = kvp.Key;
                    degreesIn[m] -= 1;
                    if (degreesIn[m] == 0)
                    {
                        S.Add(m);
                    }
                }
            }


            var sb = new StringBuilder();
            foreach (string s in L)
            {
                sb.Append(s);
            }
            
            return sb;
        }

        const int baseRef = 'A' - 1;
        const int stepTime = 60;
        const int numWorkers = 5;

        protected override object SolvePartTwo()
        {


            List<Worker> workers = new List<Worker>();
            for (int i = 0; i < numWorkers; i++)
            {
                workers.Add(new Worker { ID = i });
            }

            Dictionary<char, SortedList<char, char>> edges = new Dictionary<char, SortedList<char, char>>();
            ConcurrentDictionary<char, int> degreesIn = new ConcurrentDictionary<char, int>();
            foreach (var line in Input.SplitByNewline())
            {
                var words = line.Split();
                var from = words[1][0];
                var to = words[7][0];

                if (edges.ContainsKey(from))
                {
                    edges[from].Add(to, to);
                }
                else
                {
                    edges[from] = new SortedList<char, char> { { to, to } };
                }
                degreesIn.AddOrUpdate(to, 1, (c, i) => i + 1);
            }

            Queue<char> L = new Queue<char>();
            Queue<char> S = new Queue<char>(edges.Keys.Except(degreesIn.Keys).OrderByDescending(s => s));

            foreach (var item in S)
            {
                degreesIn[item] = 0;
            }

            //Dictionary<char, SortedList<char, char>> edges2 = new Dictionary<char, SortedList<char, char>>(edges);
            //ConcurrentDictionary<char, int> degreesIn2 = new ConcurrentDictionary<char, int>(degreesIn);

            //while (S.Count > 0)
            //{
            //    char n = S.Dequeue();
            //    L.Enqueue(n);

            //    if (!edges.ContainsKey(n))
            //        continue;

            //    foreach (var kvp in edges[n])
            //    {
            //        char m = kvp.Key;
            //        degreesIn[m] -= 1;
            //        if (degreesIn[m] == 0)
            //        {
            //            S.Enqueue(m);
            //        }
            //    }
            //}

            // 1. Work available, workers available
            // 2. Work available, no workers
            // 3. No work, workers available
            // 4. No work, no workers

            int currentTime = 0;

            while (degreesIn.Count > 0)
            {
                if (degreesIn.Values.Contains(0) && workers.Any(w => currentTime >= w.AvailableOn))
                {
                    var available = degreesIn.Where(kvp => kvp.Value == 0).OrderBy(kvp => kvp.Key).ToList();
                    foreach (var item in available)
                    {
                        var worker = workers.Where(w => currentTime >= w.AvailableOn).FirstOrDefault();
                        if (worker != null)
                        {
                            worker.AvailableOn = currentTime + StepTime(item.Key);
                            worker.WorkingOn = item.Key;
                            degreesIn.TryRemove(item.Key, out int value);
                        }
                    }
                }
                else
                {
                    // Waiting on blocked work. Get the blocking work
                    var doneWork = workers.Where(w => w.AvailableOn > currentTime).GroupBy(w => w.AvailableOn).OrderBy(g => g.Key).First();
                    foreach (var worker in doneWork)
                    {
                        currentTime = worker.AvailableOn;
                        Console.Write(worker.WorkingOn);
                        foreach (var c in edges[worker.WorkingOn])
                        {
                            degreesIn[c.Key] -= 1;
                        }
                    }
                }
            }
            //Console.WriteLine(workers.Max().WorkingOn);
            //Console.WriteLine(workers.Max(w => w.AvailableOn));
            return workers.Max(w => w.AvailableOn);
        }


        private int StepTime(char @char)
        {
            return @char - baseRef + stepTime;
        }

        [DebuggerDisplay("{AvailableOn}")]
        private class Worker : IComparable<Worker>
        {
            public int ID { get; set; }
            public char WorkingOn { get; set; }
            public int AvailableOn { get; set; }

            public int CompareTo(Worker other)
            {
                return AvailableOn > other.AvailableOn ? 1 : -1;
            }
        }

        const string test =
@"Step C must be finished before step A can begin
Step C must be finished before step F can begin
Step A must be finished before step B can begin
Step A must be finished before step D can begin
Step B must be finished before step E can begin
Step D must be finished before step E can begin
Step F must be finished before step E can begin
";
    }
}
