namespace AdventOfCode.Solutions.Year2018
{
    internal class Day22 : ASolution
    {
        int depth = 0;
        Point p;

        Location[,] cave;

        public Day22() : base(22, 2018)
        {

        }

        protected override object SolvePartOne()
        {
            var input = Input.SplitByNewline();

            depth = int.Parse(input[0].Split(' ')[1]);
            string[] target = input[1].Split(new[] { ' ', ',' });
            p = new Point(int.Parse(target[1]), int.Parse(target[2]));

            depth = 510;
            p = new Point(10, 10);

            cave = new Location[p.Y + 6, p.X + 6];
            int height = cave.GetLength(0);
            int width = cave.GetLength(1);
            Console.SetBufferSize(Console.WindowWidth, height + 20);

            for (int i = 0; i < height; i++)
            {
                cave[i, 0] = new Location(0, i, depth);
            }
            for (int i = 0; i < width; i++)
            {
                cave[0, i] = new Location(i, 0, depth);

            }

            cave[p.Y, p.X] = new Location(p.X, p.Y, depth);
            cave[p.Y, p.X].GeologicIndex = 0;
            for (int y = 1; y < height; y++)
            {
                for (int x = 1; x < width; x++)
                {
                    if (cave[y, x] != null)
                        continue;
                    cave[y, x] = new Location(x, y, depth);
                    cave[y, x].GeologicIndex = cave[y - 1, x].ErosionLevel * cave[y, x - 1].ErosionLevel;
                }
            }

            WriteConsole(height, width, 20, 5, (y, x) =>
            {
                int i = cave[y, x].RegionType;
                char c;
                if (i == 0)
                {
                    c = '.';
                }
                else if (i == 1)
                {
                    c = '=';
                }
                else
                {
                    c = '|';
                }
                return (ConsoleColor.White, c);
            });

            return cave.Cast<Location>().Where(l => l.X <= p.X && l.Y <= p.Y).Sum(l => l.RegionType);
        }

        protected override object SolvePartTwo()
        {
            var wGrid = new LocationWeightedGrid(cave);
            AStar<Location> aStar = new AStar<Location>(wGrid);

            var l = aStar.A_Star(new Point(0, 0), p, heuristic);

            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var location in l)
            {
                int i = cave[location.Y, location.X].RegionType;
                Console.SetCursorPosition(20 + location.X, 5 + location.Y);
                char c;
                if (i == 0)
                {
                    c = '.';
                }
                else if (i == 1)
                {
                    c = '=';
                }
                else
                {
                    c = '|';
                }
                Console.Write(c);
            }
            Console.ForegroundColor = ConsoleColor.White;

            return "";
        }

        private int heuristic(Point a, Point b)
        {
            return 0;
            // return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        internal class Location
        {
            public int GeologicIndex
            {
                get => _geologicIndex;
                set
                {
                    _geologicIndex = value;
                    ErosionLevel = (value + _caveDepth) % 20183;
                    RegionType = ErosionLevel % 3;
                }
            }
            private int _geologicIndex;

            public int ErosionLevel { get; private set; }

            public int RegionType { get; private set; }
            public int X { get; }
            public int Y { get; }

            private readonly int _caveDepth;

            public Location(int x, int y, int caveDepth)
            {
                X = x;
                Y = y;
                _caveDepth = caveDepth;

                if (x == 0 && y == 0)
                {
                    GeologicIndex = 0;
                }
                else if (x == 0)
                {
                    GeologicIndex = y * 48271;
                }
                else if (y == 0)
                {
                    GeologicIndex = x * 16807;
                }
            }
        }

        internal class LocationWeightedGrid : WeightedGrid<Location>
        {
            private Dictionary<Point, Tool> _pointTools = new Dictionary<Point, Tool>() { { new Point(0, 0), Tool.Torch } };

            public LocationWeightedGrid(Location[,] grid)
                : base(grid)
            {
            }

            internal override double Cost(Point from, Point to)
            {
                Tool cameFromTool = _pointTools[from];
                Region fromRegion = (Region)_grid[from.Y, from.X].RegionType;
                Region toRegion = (Region)_grid[to.Y, to.X].RegionType;

                if (fromRegion == toRegion)
                    return 1;

                switch (fromRegion)
                {

                    case Region.Rocky when toRegion == Region.Narrow && cameFromTool == Tool.Torch:
                    case Region.Rocky when toRegion == Region.Wet && cameFromTool == Tool.ClimbingGear:
                    case Region.Wet when toRegion == Region.Narrow && cameFromTool == Tool.Neither:
                    case Region.Wet when toRegion == Region.Rocky && cameFromTool == Tool.ClimbingGear:
                    case Region.Narrow when toRegion == Region.Wet && cameFromTool == Tool.Neither:
                    case Region.Narrow when toRegion == Region.Rocky && cameFromTool == Tool.Torch:
                        return 1;
                    case Region.Rocky when toRegion == Region.Narrow && cameFromTool == Tool.ClimbingGear:
                    case Region.Rocky when toRegion == Region.Wet && cameFromTool == Tool.Torch:
                    case Region.Wet when toRegion == Region.Narrow && cameFromTool == Tool.ClimbingGear:
                    case Region.Wet when toRegion == Region.Rocky && cameFromTool == Tool.Neither:
                    case Region.Narrow when toRegion == Region.Wet && cameFromTool == Tool.Torch:
                    case Region.Narrow when toRegion == Region.Rocky && cameFromTool == Tool.Neither:
                        return 8;
                }

                return double.PositiveInfinity;
            }

            internal override void EnterLoc(Point from, Point to)
            {
                Region fromRegion = (Region)_grid[from.Y, from.X].RegionType;
                Region toRegion = (Region)_grid[to.Y, to.X].RegionType;
                switch (fromRegion)
                {
                    case Region.Rocky when toRegion == Region.Narrow:
                        _pointTools[to] = Tool.Torch;
                        return;
                    case Region.Rocky when toRegion == Region.Wet:
                        _pointTools[to] = Tool.ClimbingGear;
                        return;
                    case Region.Wet when toRegion == Region.Narrow:
                        _pointTools[to] = Tool.Neither;
                        return;
                    case Region.Wet when toRegion == Region.Rocky:
                        _pointTools[to] = Tool.ClimbingGear;
                        return;
                    case Region.Narrow when toRegion == Region.Wet:
                        _pointTools[to] = Tool.Neither;
                        return;
                    case Region.Narrow when toRegion == Region.Rocky:
                        _pointTools[to] = Tool.Torch;
                        return;
                }
                _pointTools[to] = _pointTools[from];
            }

            private static Dictionary<(int, int), double> neitherCostMatrix = new Dictionary<(int, int), double>
            {
                // (from, to) -> Cost
                { (1, 0), double.PositiveInfinity }, // from wet to rock
                { (1, 1), 1 }, // from and to wet
                { (1, 2), 1 }, // from wet to narrow
                { (2, 0), double.PositiveInfinity }, // from narrow to rock
                { (2, 1), 1 }, // from narrow to wet
                { (2, 2), 1 }, // from and to narrow
            };
            private static Dictionary<(int, int), double> TorchCostMatrix = new Dictionary<(int, int), double>
            {
                // (from, to) -> Cost
                { (0, 0), 1 }, // from and to rock
                { (0, 1), 1 }, // from rock to wet
                { (0, 2), 1 }, // from rock to narrow
                { (2, 0), 1 }, // from narrow to rock
                { (2, 1), 1 }, // from narrow to wet
                { (2, 2), 1 }, // from and to narrow
            };
            private static Dictionary<(int, int), double> ClimbingGearCostMatrix = new Dictionary<(int, int), double>
            {
                // (from, to) -> Cost
                { (0, 0), 1 }, // from and to rock
                { (0, 1), 1 }, // from rock to wet
                { (0, 2), 1 }, // from rock to narrow
                { (1, 0), 1 }, // from wet to rock
                { (1, 1), 1 }, // from and to rock
                { (1, 2), 1 }, // from rock to narrow
            };
        }

        internal enum Tool
        {
            Neither,
            Torch,
            ClimbingGear
        }

        internal enum Region
        {
            Rocky,
            Wet,
            Narrow
        }
    }
}