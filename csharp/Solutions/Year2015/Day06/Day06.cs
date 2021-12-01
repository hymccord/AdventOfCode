using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode.Solutions.Year2015.Day06
{
    class Day06 : ASolution
    {
        private int[] grid = new int[1_000_000];
        private List<(Action, Rectangle)> instructions = new List<(Action, Rectangle)>();
        public Day06() : base(6, 2015, "")
        {
        }

        private string test = @"turn on 0,0 through 999,999
toggle 0,0 through 999,0
turn off 499,499 through 500,500
";

        protected override object SolvePartOne()
        {
            int[] grid = new int[1_000_000];
            instructions = BuildInstruction(Input.SplitByNewline());

            foreach ((Action action, Rectangle rect) in instructions)
            {
                for (int y = rect.Bottom; y <= rect.Top; y++)
                {
                    for (int x = rect.Left; x <= rect.Right; x++)
                    {
                        int val = grid[y * 1000 + x];

                        switch (action)
                        {
                            case Action.Toggle:
                                val = (byte)(~val & 0b1);
                                break;
                            case Action.Off:
                                val = 0;
                                break;
                            case Action.On:
                                val = 1;
                                break;
                        }
                        //val = action switch
                        //{
                        //    Action.Toggle => (byte)(~val & 0b1),
                        //    Action.Off => 0,
                        //    Action.On => 1,
                        //    _ => throw new Exception(),
                        //};

                        grid[y * 1000 + x] = val;
                    }
                }
            }

            //Console.WindowWidth = 300;
            //Console.SetBufferSize(1020, 1020);
            //WriteConsole(1000, 1000, 15, 15, (row, col) =>
            //{
            //    if (grid[row, col] == 0)
            //    {
            //        return (ConsoleColor.White, '.');
            //    }
            //    return (ConsoleColor.Yellow, '*');
            //});

            return grid.Sum(x => x).ToString();
        }

        string test2 = @"turn on 0,0 through 0,0
toggle 0,0 through 999,999
";

        protected override object SolvePartTwo()
        {
            int[] grid = new int[1_000_000];
            instructions = BuildInstruction(Input.SplitByNewline());

            foreach ((Action action, Rectangle rect) in instructions)
            {
                for (int y = rect.Bottom; y <= rect.Top; y++)
                {
                    for (int x = rect.Left; x <= rect.Right; x++)
                    {
                        int val = grid[y * 1000 + x];

                        switch (action)
                        {
                            case Action.Toggle:
                                val += 2;
                                break;
                            case Action.Off:
                                val--;
                                break;
                            case Action.On:
                                val++;
                                break;
                        }
                        //val = action switch
                        //{
                        //    Action.Toggle => (byte)(~val & 0b1),
                        //    Action.Off => 0,
                        //    Action.On => 1,
                        //    _ => throw new Exception(),
                        //};

                        grid[y * 1000 + x] = val < 0 ? 0 : val;
                    }
                }
            }

            Console.WindowWidth = 300;
            Console.SetBufferSize(1020, 1020);
            WriteConsole(1000, 1000, 15, 15, (row, col) =>
            {
                return (grid[row * 1000 + col]) switch
                {
                    0 => (ConsoleColor.Black, '.'),
                    1 => (ConsoleColor.DarkGray, '*'),
                    2 => (ConsoleColor.DarkRed, '*'),
                    3 => (ConsoleColor.DarkBlue, '*'),
                    4 => (ConsoleColor.DarkYellow, '*'),
                    5 => (ConsoleColor.Red, '*'),
                    6 => (ConsoleColor.Blue, '*'),
                    7 => (ConsoleColor.Yellow, '*'),
                    8 => (ConsoleColor.Cyan, '*'),
                    _ => (ConsoleColor.Green, '*'),
                };
            });

            return grid.Sum(x => x).ToString();
        }

        private List<(Action, Rectangle)> BuildInstruction(string[] v)
        {
            var l = new List<(Action, Rectangle)>();
            foreach (var s in v)
            {
                var a = s.Split(' ');
                Action action = a[0] == "toggle"
                    ? Action.Toggle
                    : a[1] == "on"
                        ? Action.On
                        : Action.Off;

                int[] x1y1 = a[a.Length - 3].Split(',').Select(int.Parse).ToArray();
                int[] x2y2 = a[a.Length - 1].Split(',').Select(int.Parse).ToArray();

                Debug.Assert(x1y1[0] <= x2y2[0]);
                Debug.Assert(x1y1[1] <= x2y2[1]);
                l.Add((action, Rectangle.FromLTRB(x1y1[0], x2y2[1], x2y2[0], x1y1[1])));
            }

            return l;
        }

        enum Action
        {
            Toggle = -1,
            Off,
            On,
        }
    }
}
