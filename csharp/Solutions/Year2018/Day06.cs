using System.Diagnostics;

namespace AdventOfCode.Solutions.Year2018
{
    class Day06 : ASolution
    {
        //static string[] s_testInput = SplitInput(TEST_INPUT).ToArray();

        public Day06() : base(06, 2018, "")
        {

        }

        protected override object SolvePartOne()
        {

            List<Point> points = Input.SplitByNewline().Select(s => new Point(s)).ToList();

            int minCol = points.Min(p => p.Col);
            int maxCol = points.Max(p => p.Col);
            int minRow = points.Min(p => p.Row);
            int maxRow = points.Max(p => p.Row);

            int gridRows = maxRow - minRow + 1;
            int gridColumns = maxCol - minCol + 1;
            Point[,] grid = new Point[gridRows, gridColumns];

            foreach (var p in points)
            {
                grid[p.Row - minRow, p.Col - minCol] = p;
            }

            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridColumns; col++)
                {
                    //if (grid[x, y] != '\0')
                    //    continue;

                    var minPoint = IteratePoints(points, row + minRow, col + minCol);

                    if (minPoint == null)
                        continue;

                    grid[row, col] = minPoint;
                }
            }
            HashSet<char> inf = new HashSet<char>(new char[] { '.' });

            for (int col = 0; col < gridColumns; col++)
            {
                inf.Add(grid[0, col]?.ID ?? '.');
                inf.Add(grid[gridRows - 1, col]?.ID ?? '.');
            }
            for (int row = 0; row < gridRows; row++)
            {
                inf.Add(grid[row, 0]?.ID ?? '.');
                inf.Add(grid[row, gridColumns - 1]?.ID ?? '.');
            }

            var g = grid.Cast<Point>().Where(p => p != null).GroupBy(c => c.ID).Where(gr => !inf.Contains(gr.Key)).Select(gr => new { key = gr.Key, count = gr.Count() }).OrderByDescending(a => a.count);

            //PrintGrid(grid);
            //Console.WriteLine($"Max Area: {g.First().count}");
            return g.First().count;
        }

        protected override object SolvePartTwo()
        {
            List<Point> points = Input.SplitByNewline().Select(s => new Point(s)).ToList();
            int minCol = points.Min(p => p.Col);
            int maxCol = points.Max(p => p.Col);
            int minRow = points.Min(p => p.Row);
            int maxRow = points.Max(p => p.Row);

            int gridRows = maxRow - minRow + 1;
            int gridColumns = maxCol - minCol + 1;
            Point[,] grid = new Point[gridRows, gridColumns];

            int safecount = 0;
            const int safe_dist = 10_000;
            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridColumns; col++)
                {
                    if (points.Sum(p => Manhatten(p, row + minRow, col + minCol)) < safe_dist)
                        safecount++;
                }
            }

            //Console.WriteLine($"Safe Region area: {safecount}");
            return safecount;
        }

        private Point IteratePoints(List<Point> points, int row, int col)
        {
            Point p = null;
            int min = int.MaxValue;
            foreach (var point in points)
            {
                var dist = Manhatten(point, row, col);
                if (dist < min)
                {
                    p = point;
                    min = dist;
                }
                else if (dist == min)
                {
                    p = null;
                }
            }
            return p;
        }

        private int Manhatten(Point p1, Point p2)
        {
            return Math.Abs(p1.Col - p2.Col) + Math.Abs(p1.Row - p2.Row);
        }

        private int Manhatten(Point p, int row, int col)
        {
            return Math.Abs(p.Row - row) + Math.Abs(p.Col - col);
        }

        private void PrintGrid(Point[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Console.Write($"{(grid[row, col] == null ? '.' : grid[row, col].ID)} ");
                }
                Console.WriteLine();
            }
        }

        [DebuggerDisplay("{ID}: {Col}, {Row}")]
        private class Point
        {
            static int id = 0;

            public Point(string input)
            {
                int commaIndex = input.IndexOf(',');
                Col = int.Parse(input.Substring(0, commaIndex));
                Row = int.Parse(input.Substring(commaIndex + 1, input.Length - commaIndex - 1));

                ID = (char)('A' + id++);
            }

            public int Col { get; }
            public int Row { get; }

            public char ID { get; }
        }

        private const string TEST_INPUT =
@"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9
";
        private const string red =
@"45, 315
258, 261
336, 208
160, 322
347, 151
321, 243
232, 148
48, 202
78, 161
307, 230
170, 73
43, 73
74, 248
177, 296
330, 266
314, 272
175, 291
75, 142
278, 193
279, 337
228, 46
211, 164
131, 100
110, 338
336, 338
231, 353
184, 213
300, 56
99, 231
119, 159
180, 349
130, 193
308, 107
140, 40
222, 188
356, 44
73, 107
304, 313
199, 238
344, 158
49, 225
64, 117
145, 178
188, 265
270, 215
48, 181
213, 159
174, 311
114, 231
325, 162
";
    }
}
