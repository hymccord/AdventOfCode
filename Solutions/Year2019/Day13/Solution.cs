using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode.Solutions.Year2019 {

    class Day13 : ASolution {
        
        public Day13() : base(13, 2019, "") {
            
        }

        protected override string SolvePartOne() {
            IntCode cpu = IntCode.Create(Input.SplitByNewline().First());           
            int i = 0;
            int blocks = 0;
            cpu.Output += (s, e) =>
            {
                if (i == 2 && e == 2)
                    blocks++;

                i = ++i % 3;
            };
            cpu.Run();
            return blocks.ToString();
        }

        private void WriteScore(long e)
        {
            var arr = e.ToString().ToCharArray();
            for (int z = 0; z < arr.Length; z++)
            {
                WriteConsole(0, z, 14, 14, arr[z]);
            }
            e.ToString().ToCharArray();
        }

        protected override string SolvePartTwo() {
            IntCode cpu = IntCode.Create(Input.SplitByNewline().First());
            cpu.ByteCode[0] = 2;
            var output = new List<long>();
            long[] counts = new long[5];
            int i = 0;
            Point ball = new Point();
            Point paddle = new Point();
            long x = 0;
            long y = 0;
            cpu.GetInput = (i) =>
            {
                return ball.X.CompareTo(paddle.X);
            };
            cpu.Output += (s, e) =>
            {
                if (i == 0)
                    x = e;
                else if (i == 1)
                    y = e;
                else if (i == 2)
                {
                    if (x == -1)
                    {
                        WriteScore(e);
                    }
                    else
                    {
                        char c = e switch
                        {
                            0 => ' ',
                            1 => '#',
                            2 => '.',
                            3 => '_',
                            4 => 'o',
                        };
                        WriteConsole((int)y, (int)x, 15, 15, c);
                    }
                    if (e == 3)
                        paddle = new Point((int)x, (int)y);
                    if (e == 4)
                        ball = new Point((int)x, (int)y);
                    //Thread.Sleep(1);
                }
                i = ++i % 3;
                output.Add(e);
            };
            cpu.Run();

            return null;
        }
    }
}
