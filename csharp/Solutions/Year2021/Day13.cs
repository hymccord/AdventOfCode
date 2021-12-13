namespace AdventOfCode.Solutions.Year2021
{
    internal class Day13 : ASolution
    {
        List<(char, int)> _folds = new();
        List<Point> _points = new();

        public Day13() : base(13, 2021, "Transparent Origami", false)
        {

        }

        protected override void Preprocess()
        {
            var a = Input.SplitByBlankLine();

            foreach (var coord in a[0].SplitByNewline())
            {
                var p = coord.Split(',');
                _points.Add(new Point(int.Parse(p[0]), int.Parse(p[1])));
            }

            foreach (var fold in a[1].SplitByNewline())
            {
                var f = fold.Split('=');
                _folds.Add((f[0][^1], int.Parse(f[1])));
            }
        }

        protected override object SolvePartOne()
        {
            (char direction, int line) = _folds[0];

            var points = Fold(_points, direction, line);

            return points.Count;
        }

        protected override object SolvePartTwo()
        {
            var set = new HashSet<Point>(_points);
            foreach ((char direction, int line) in _folds)
            {
                set = Fold(set, direction, line);
            }
            
            var sheet = CreateSheetFromPoints(set);
            DebugPrint(sheet);

            return "RPCKFBLR";
        }

        private static HashSet<Point> Fold(ICollection<Point> points, char direction, int line)
        {
            HashSet<Point> newPoints = new();
            foreach (var point in points)
            {
                if (direction == 'x')
                {
                    if (point.X >= line)
                    {
                        newPoints.Add(new Point(line - (point.X - line), point.Y));
                    }
                    else
                    {
                        newPoints.Add(point);
                    }
                }
                else
                {
                    if (point.Y >= line)
                    {
                        newPoints.Add(new Point(point.X, line - (point.Y - line)));
                    }
                    else
                    {
                        newPoints.Add(point);
                    }
                }
            }

            return newPoints;
        }

        private char[,] CreateSheetFromPoints(ICollection<Point> points)
        {
            var sheet = new char[points.Max(p => p.Y) + 1, points.Max(p => p.X) + 1];

            for (int y = 0; y < sheet.GetLength(0); y++)
            {
                for (int x = 0; x < sheet.GetLength(1); x++)
                {
                    sheet[y, x] = '.';
                }
            }

            foreach (var p in points)
            {
                sheet[p.Y, p.X] = '#';
            }

            return sheet;
        }

        private void DebugPrint(char[,] sheet)
        {
            WriteConsole(sheet.GetLength(0), sheet.GetLength(1), 10, 10, (r, c) =>
            {
                return (ConsoleColor.White, sheet[r, c]);
            });
        }

        private void DebugClear(char[,] sheet)
        {
            WriteConsole(sheet.GetLength(0), sheet.GetLength(1), 10, 10, (r, c) =>
            {
                return (ConsoleColor.White, ' ');
            });
        }

        protected override string LoadDebugInput()
        {
            return @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";
        }
    }
}
