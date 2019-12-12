using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2019
{

    class Day07 : ASolution
    {

        int[] _phases = Enumerable.Range(0, 5).ToArray();

        string test = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0";
        string test2 = "3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0";
        string test3 = "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0";

        public Day07() : base(7, 2019, "")
        {

        }

        protected override string SolvePartOne()
        {

            var perm = _phases.Permutate(_phases.Length);
            int[] curPhase = new int[5];
            //var output = new List<string>();
            string s = Input.Trim('\n');
            IntCode cpu = IntCode.Create(s);

            int runningAmp = 0;
            long input = 0;
            long output = 0;

            long maxOutput = long.MinValue;
            cpu.GetInput += (i) =>
            {
                return i switch
                {
                    0 => curPhase[runningAmp],
                    _ => input,
                };
                //System.Console.WriteLine($"Getting input {curPhase[runningAmp]}.");
                //return curPhase[runningAmp];
            };

            cpu.Output += (s, e) =>
            {
                output = e;
                //System.Console.WriteLine($"Amp {(char)('A' + runningAmp)}: {input}->{output}");
                input = output;
            };

            foreach (var item in perm)
            {
                curPhase = item.ToArray();
                runningAmp = 0;
                input = 0;
                output = 0;
                for (int i = 0; i < curPhase.Length; i++)
                {
                    cpu.Reset();
                    cpu.Run();
                    runningAmp++;
                }

                maxOutput = output > maxOutput ? output : maxOutput;
            }

            return maxOutput.ToString();
        }

        private string test4 = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
        private string test5 = "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1005,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10";

        protected override string SolvePartTwo()
        {
            var perm = Enumerable.Range(5, 5).Permutate(5);
            int[] curPhase = new int[5];
            string s = Input.Trim('\n');

            IntCode[] amplifiers = new IntCode[5];
            AutoResetEvent[] events = new AutoResetEvent[5]
            {
                new AutoResetEvent(false),
                new AutoResetEvent(false),
                new AutoResetEvent(false),
                new AutoResetEvent(false),
                new AutoResetEvent(false),
            };

            long input = 0;
            long output = 0;

            long maxOutput = int.MinValue;

            for (int i = 0; i < 5; i++)
            {
                int local = i;
                amplifiers[i] = IntCode.Create(s);
                amplifiers[i].GetInput += (j) =>
                {
                    // ask for input from previous Amp
                    if (j > 0)
                    {
                        //System.Console.WriteLine($"Amp {(char)('A' + local)} waiting for input");
                        events[GetPreviousIndex(local)].WaitOne();
                        //System.Console.WriteLine($"Amp {(char)('A' + local)} got {input}");
                    }

                    return j switch
                    {
                        0 => curPhase[local],
                        _ => input,
                    };
                    //System.Console.WriteLine($"Getting input {curPhase[runningAmp]}.");
                    //return curPhase[runningAmp];
                };

                amplifiers[i].Output += (s, e) =>
                {
                    output = e;
                    //System.Console.WriteLine($"Amp {(char)('A' + local)}: {input}->{output}");
                    input = output;

                    // Input is ready for next amp, so signal our output
                    // The other task will be latched on to ours
                    events[local].Set();
                };
            }

            Task[] tasks;
            foreach (var item in perm)
            {
                curPhase = item.ToArray();
                input = 0;
                output = 0;

                tasks = new Task[5];
                events.Select(e => e.Reset()).ToArray();
                for (int i = 0; i < curPhase.Length; i++)
                {
                    int local = i;
                    amplifiers[i].Reset();
                    tasks[i] = Task.Run(() => amplifiers[local].Run());
                }
                // Output on E is 0 and ready at the start
                events.Last().Set();
                Task.WhenAll(tasks).Wait();

                maxOutput = output > maxOutput ? output : maxOutput;
            }

            return maxOutput.ToString();
        }

        private int GetNextIndex(int i)
        {
            return Wrap(i + 1, 5);
        }

        private int GetPreviousIndex(int i)
        {
            return Wrap(i - 1, 5);
        }

        private static int Wrap(int index, int n)
        {
            return ((index % n) + n) % n;
        }
    }
}
