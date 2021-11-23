
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode.Solutions.Year2018
{
    class Day10 : ASolution
    {
        public Day10() : base(10, 2018, "")
        {

        }
        protected override object SolvePartOne()
        {
            var points = Input.SplitByNewline().Select(s => new Point(s)).ToList();
            var hmmm = points.Select(p => new { divX = p.ColumnX / p.DeltX, divY = p.RowY / p.DeltY });
            var avgX = hmmm.Average(a => a.divX);
            var avgY = hmmm.Average(a => a.divY);
            int realAvg = Math.Abs((int)(avgX + avgY) / 2) - 11;
            Console.BufferHeight = 500;
            Console.BufferWidth = 500;
            Console.WriteLine("Press n to step");
            while (Console.ReadKey().KeyChar == 'n')
            {
                realAvg += 1;
                points.ForEach(p => { p.GraphX = p.ColumnX + p.DeltX * realAvg; p.GraphY = p.RowY + p.DeltY * realAvg; });
                int minCol = points.Min(p => p.GraphX);
                int minRow = points.Min(p => p.GraphY);
                int maxCol = points.Max(p => p.GraphX);
                int maxRow = points.Max(p => p.GraphY);

                int gridCol = maxCol - minCol + 1;
                int gridRow = maxRow - minRow + 1;

                char[,] grid = new char[gridRow, gridCol];

                foreach (var p in points)
                {
                    int row = p.GraphY - minRow;
                    int col = p.GraphX - minCol;
                    grid[row, col] = '#';
                }

                ClearConsole();
                WriteConsole(gridRow, gridCol, 5, 5, (r, c) => (Console.ForegroundColor, grid[r, c]));
            }

            return 0;
        }

        protected override object SolvePartTwo()
        {
            return "";
        }

        [DebuggerDisplay("{ColumnX},{RowY} {DeltX},{DeltY}")]
        private class Point
        {
            public Point(string line)
            {
                var match = Regex.Match(line, @"<(.*),(.*)>.*<(.*),(.*)>");
                ColumnX = int.Parse(match.Groups[1].Value);
                RowY = int.Parse(match.Groups[2].Value);
                DeltX = int.Parse(match.Groups[3].Value);
                DeltY = int.Parse(match.Groups[4].Value);
            }
            public int ColumnX { get; set; }
            public int RowY { get; set; }
            public int DeltX { get; set; }
            public int DeltY { get; set; }
            public int GraphX { get; set; }
            public int GraphY { get; set; }
        }
    }
}
