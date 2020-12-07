using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019 {

    class Day11 : ASolution {
        
        public Day11() : base(11, 2019, "") {
            
        }


        protected override object SolvePartOne() {
            var panels = new Dictionary<Point, long>();
            (Point location, Point direction) = (new Point(), Point.North);
            IntCode cpu = IntCode.Create(Input.SplitByNewline().First());
            cpu.GetInput = (i) =>
            {
                return panels.TryGetValue(location, out var result) 
                ? result 
                : 0;
            };
            int output = 0;
            cpu.Output += (s, e) =>
            {
                if (output == 0)
                    panels[location] = e;
                else
                {
                    direction = e switch
                    {
                        0 => new Point(direction.Y, -direction.X),
                        1 => new Point(-direction.Y, direction.X),
                        _ => throw new System.Exception(),
                    };
                    location += direction;
                }
                output = (output + 1) & 1;
            };
            cpu.Run();
            return panels.Count.ToString(); 
        }

        protected override object SolvePartTwo() {
            var panels = new Dictionary<Point, long>
            {
                { new Point(), 1 },
            };
            (Point location, Point direction) = (new Point(), Point.North);
            IntCode cpu = IntCode.Create(Input.SplitByNewline().First());
            cpu.GetInput = (i) =>
            {
                return panels.TryGetValue(location, out var result)
                ? result
                : 0;
            };
            int output = 0;
            cpu.Output += (s, e) =>
            {
                if (output == 0)
                    panels[location] = e;
                else
                {
                    direction = e switch
                    {
                        0 => new Point(direction.Y, -direction.X),
                        1 => new Point(-direction.Y, direction.X),
                        _ => throw new System.Exception(),
                    };
                    location += direction;
                }
                output = (output + 1) & 1;
            };
            cpu.Run();

            int minX = panels.Keys.Min(p => p.X);
            int minY = panels.Keys.Min(p => p.Y);
            int rows = panels.Keys.Max(p => p.Y) - minY + 1;
            int cols = panels.Keys.Max(p => p.X) - minX + 1;

            WriteConsole(rows, cols, 15, 15, (row, col) =>
            {
                var p = new Point(col - minX, row - minY);
                char c = '.';
                if (panels.TryGetValue(p, out long value))
                    c = panels[p] == 0 ? '.' : '#';
                return (ConsoleColor.White, c);
            });
            return null;
        }
    }
}
