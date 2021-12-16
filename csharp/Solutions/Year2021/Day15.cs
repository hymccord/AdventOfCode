namespace AdventOfCode.Solutions.Year2021
{
    internal class Day15 : ASolution
    {
        int _rows, _cols;
        int[,] _risk;
        int[,] _bigRisk;

        public Day15() : base(15, 2021, "Chiton", false)
        { }

        protected override void Preprocess()
        {
            _risk = Input.To2DIntArray();
            _rows = _risk.GetLength(0);
            _cols = _risk.GetLength(1);

            _bigRisk = new int[_rows * 5, _cols * 5];
            // copy original
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    _bigRisk[y, x] = _risk[y, x];
                }
            }
            // expand
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Math.DivRem(_risk[y, x] + i + j, 9, out int modRisk);
                            _bigRisk[_rows * j + y, _cols * i + x] = modRisk == 0 ? 9 : modRisk;
                        } 
                    }
                }
            }
        }

        protected override object SolvePartOne()
        {
            var wg = new ChitonCave(_risk);
            var aStar = new AStar<int>(wg);

            var l = aStar.A_Star(new Point(0, 0), new Point(_rows - 1, _cols - 1),
                heuristic: (goal, next) => next.Manhatten(goal));

            //DebugPrint(_risk, l);

            return l.Skip(1).Sum(p => _risk[p.Y, p.X]);
        }

        protected override object SolvePartTwo()
        {
            var wg = new ChitonCave(_bigRisk);
            var aStar = new AStar<int>(wg);

            var l = aStar.A_Star(new Point(0, 0), new Point((_rows * 5) - 1, (_cols * 5) - 1),
                heuristic: (goal, next) => next.Manhatten(goal));

            //DebugPrint(_bigRisk, l);

            return l.Skip(1).Sum(p => _bigRisk[p.Y, p.X]);
        }

        protected override string LoadDebugInput()
        {
            return @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";
        }

        private void DebugPrint(int[,] grid, ICollection<Point> path)
        {
            var s = path.ToHashSet();
            int r = grid.GetLength(0);
            int c = grid.GetLength(1);
            WriteConsole(r, c, 20, 10, (row, col) =>
            {
                return (ConsoleColor.White, (char)(grid[row, col] + '0'));
            });

            foreach (var p in path)
            {
                WriteConsole(p.Y, p.X, 20, 10, (char)(grid[p.Y, p.X] + '0'), ConsoleColor.Magenta);
            }
        }

        private class ChitonCave : WeightedGrid<int>
        {
            public ChitonCave(int[,] grid)
                : base(grid)
            {
            }

            internal override double Cost(Point from, Point to)
            {
                return _grid[to.Y, to.X];
            }
        }
    }
}
