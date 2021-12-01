namespace AdventOfCode.Solutions.Year2019
{

    class Day15 : ASolution
    {

        private Dictionary<Point, char> grid = new Dictionary<Point, char>();
        private Dictionary<Point, long> stepsToPoint = new Dictionary<Point, long>();
        private HashSet<Point> processed = new HashSet<Point>();

        public Day15() : base(15, 2019, "")
        {

        }

        Queue<(Point, IntCode)> Q = new Queue<(Point, IntCode)>();
        protected override object SolvePartOne()
        {
            processed.Add(new Point());
            stepsToPoint[new Point()] = 0;
            var cpu = IntCode.Create(Input);
            Q.Enqueue((new Point(), cpu));
            while (Q.Count > 0)
            {
                (Point p, IntCode code) = Q.Dequeue();
                foreach (var neighbor in p.Neighbors)
                {
                    if (!processed.Contains(neighbor))
                    {
                        long ret = RunPoint(code.Clone(), neighbor, p);
                        processed.Add(neighbor);
                    }
                }
            }

#if ASTAR
            int minX = grid.Min(kvp => kvp.Key.X);
            int minY = grid.Min(kvp => kvp.Key.Y);
            int maxX = grid.Max(kvp => kvp.Key.X);
            int maxY = grid.Max(kvp => kvp.Key.Y);
            Point goal = grid.First(kvp => kvp.Value == 'O').Key;

            var charGrid = new char[maxY - minY + 1, maxX - minX + 1];
            foreach (var kvp in grid)
            {
                charGrid[kvp.Key.Y - minY, kvp.Key.X - minX] = kvp.Value;
            }
            var oxyGrid = new OxygenGrid(charGrid);
            var aStar = new AStar<char>(oxyGrid);
            var path = aStar.A_Star(new Point(-minX, -minY), new Point(goal.X - minX, goal.Y - minY), (p1, p2) => 0);

            WriteGrid();
            foreach (var p in path)
            {
                RobitWrite(p.Y + minY, p.X + minX, '*', ConsoleColor.Green);
            }
            return (path.Count - 1).ToString();
#endif
            return stepsToPoint[grid.First(kvp => kvp.Value == 'O').Key].ToString();
        }

        private long RunPoint(IntCode clone, Point neighbor, Point p)
        {
            long ret = 0L;
            clone.GetInput = (i) =>
            {
                return (i & 1) == 0
                ? DirToNum(neighbor, p)
                : 0;
            };
            clone.Output += (s, e) =>
            {
                ret = e;
                if (ret == 0)
                {
                    grid[neighbor] = '#';
                }
                else if (ret == 1)
                {
                    grid[neighbor] = '.';
                    Q.Enqueue((neighbor, clone.Clone()));
                }
                else if (ret == 2)
                {
                    grid[neighbor] = 'O';
                }
                stepsToPoint[neighbor] = stepsToPoint[p] + 1;
            };
            clone.Run();

            //WriteGrid();
            return ret;
        }

        protected override object SolvePartTwo()
        {
            int minX = grid.Min(kvp => kvp.Key.X);
            int minY = grid.Min(kvp => kvp.Key.Y);
            int maxX = grid.Max(kvp => kvp.Key.X);
            int maxY = grid.Max(kvp => kvp.Key.Y);
            Point goal = grid.First(kvp => kvp.Value == 'O').Key;

            var charGrid = new char[maxY - minY + 1, maxX - minX + 1];
            foreach (var kvp in grid)
            {
                charGrid[kvp.Key.Y - minY, kvp.Key.X - minX] = kvp.Value;
            }
            var oxyGrid = new OxygenGrid(charGrid);
            var aStar = new AStar<char>(oxyGrid);

            var longestPath = grid.Where(kvp => kvp.Value == '.')
                .AsParallel()
                .Max(kvp => aStar.A_Star(
                    start: new Point(goal.X - minX, goal.Y - minY),
                    goal: new Point(kvp.Key.X - minX, kvp.Key.Y - minY),
                    h: (p1, p2) => 0).Count);
#if false
            var longestPath = grid.Where(kvp => kvp.Value == '.').AsParallel()
                .Select(kvp => new
                {
                    goal = new Point(kvp.Key.X - minX, kvp.Key.Y - minY),
                    list = aStar.A_Star(
                    start: new Point(goal.X - minX, goal.Y - minY),
                    goal: new Point(kvp.Key.X - minX, kvp.Key.Y - minY),
                    h: (p1, p2) => 0)
                }).OrderByDescending(a => a.list.Count)
                .ToList();

            WriteGrid();
            foreach (var p in longestPath.First().list)
            {
                RobitWrite(p.Y + minY, p.X + minX, '*', ConsoleColor.Green);
            }
#endif

            return (longestPath - 1).ToString();
        }

        private long DirToNum(Point nextPoint, Point curPos)
        {
            return (nextPoint - curPos) switch
            {
                var (x, y) when x == -1 && y == 0 => 3,
                var (x, y) when x == 1 && y == 0 => 4,
                var (x, y) when x == 0 && y == -1 => 1,
                var (x, y) when x == 0 && y == 1 => 2,
            };
        }

        private void RobitWrite(int row, int col, char c, ConsoleColor color = ConsoleColor.White)
        {
            WriteConsole(row + 15, col + 15, 25, 25, c, color);
        }

        private void WriteGrid()
        {
            foreach (var kvp in grid)
            {
                var p = kvp.Key;
                var c = kvp.Value;
                RobitWrite(p.Y, p.X, c);
            }
        }

        private class OxygenGrid : WeightedGrid<char>
        {
            public OxygenGrid(char[,] grid)
                : base(grid)
            {

            }

            internal override double Cost(Point from, Point to)
            {
                if (_grid[to.Y, to.X] == '#')
                    return double.PositiveInfinity;

                return 1;
            }
        }
    }
}
