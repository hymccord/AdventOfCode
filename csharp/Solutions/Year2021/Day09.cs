namespace AdventOfCode.Solutions.Year2021
{
    internal class Day09 : ASolution
    {
        int[,] _ints;
        private int _rows;
        private int _cols;
        private List<Point> _points;

        public Day09() : base(09, 2021, "", false)
        {

        }    

        protected override void Preprocess()
        {
            _ints = InputByNewLine.Select(s => s.ToCharArray().Select(c => c - '0').ToArray()).ToArray().To2D();

            _rows = _ints.GetLength(0);
            _cols = _ints.GetLength(1);

            _points = new();
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    _points.Add(new Point(x, y));
                }
            }
        }

        HashSet<Point> _lowPoints = new();
        protected override object SolvePartOne()
        {
            int riskSum = 0;
            foreach (var p in _points)
            {
                int currentValue = _ints[p.Y, p.X]; // 2D arrays, you select row then columns... Y then X.

                if (p.Neighbors.Where(n => n.IsInBoundsOfArray(_rows, _cols)).All(n => currentValue < _ints[n.Y, n.X]))
                {
                    riskSum += 1 + currentValue;
                    _lowPoints.Add(p);
                }
            }

            // DebugWrite(_points.ToHashSet(), ConsoleColor.White);
            // DebugWrite(_lowPoints, ConsoleColor.Magenta);

            return riskSum;
        }

        protected override object SolvePartTwo()
        {
            var allBasins = new List<HashSet<Point>>();
            foreach (var lowPoint in _lowPoints)
            {
                Queue<Point> toProcess = new();
                toProcess.Enqueue(lowPoint);
                HashSet<Point> currentBasin = new();
                
                while (toProcess.Count > 0)
                {
                    Point p = toProcess.Dequeue();
                    currentBasin.Add(p);

                    foreach (var neighbor in p.Neighbors.Where(p => !currentBasin.Contains(p) && p.IsInBoundsOfArray(_rows, _cols) && _ints[p.Y, p.X] != 9))
                    {
                        toProcess.Enqueue(neighbor);
                    }
                }

                allBasins.Add(currentBasin);
            }

            var largestThree = allBasins.OrderByDescending(s => s.Count).Take(3);
            
            // DebugWrite(allBasins.Aggregate((h, h2) => h.Union(h2).ToHashSet()).Except(_lowPoints).ToHashSet(), ConsoleColor.Green);
            // DebugWrite(largestThree.Aggregate((h, h2) => h.Union(h2).ToHashSet()).Except(_lowPoints).ToHashSet(), ConsoleColor.Red);

            return largestThree.Aggregate(1, (m, s) => m * s.Count);
        }

        protected override string LoadDebugInput()
        {
            return @"2199943210
3987894921
9856789892
8767896789
9899965678";
        }

        private void DebugWrite(HashSet<Point> points, ConsoleColor foreground)
        {
            foreach (var p in points)
            {
                WriteConsole(p.Y, p.X, 10, 10, (char)(_ints[p.Y, p.X] + '0'), foreground);
            }
        }
    }
}
