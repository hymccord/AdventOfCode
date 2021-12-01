namespace AdventOfCode.Solutions.Year2018
{
    internal class Day17 : ASolution
    {
        char[,] grid;
        int maxY;
        Queue<(int col, int row)> pendingLocations = new Queue<(int col, int row)>();
        Stack<(int col, int row)> flowLocations = new Stack<(int col, int row)>();

        Stack<Point> _points = new Stack<Point>();

        public Day17() : base(17, 2018)
        {

        }

        protected override object SolvePartOne()
        {
            Range[] ranges = Input.SplitByNewline().Select(Range.Parse).ToArray();

            var b = ranges.All(r => r.StartX <= r.EndX && r.StartY <= r.EndY);

            int minX = ranges.Min(r => r.StartX) - 1;
            int maxX = ranges.Max(r => r.EndX);
            int minY = 0;
            maxY = ranges.Max(r => r.EndY);

            int width = maxX - minX + 4;
            int height = maxY - minY + 4;

            Console.CursorVisible = false;
            //Console.SetBufferSize(width + 6, height + 5);
            Console.SetWindowSize(width + 10, 40);

            grid = new char[height, width];
            // Set sand
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = '.';
                }
            }
            // Set clay
            foreach (Range range in ranges)
            {
                for (int y = range.StartY; y <= range.EndY; y++)
                {
                    for (int x = range.StartX; x <= range.EndX; x++)
                    {
                        grid[y, x - minX] = '#';
                    }
                }
            }
            // Set spring
            grid[0, 500 - minX] = '|';
            _points.Push(new Point(500 - minX, 0));
            WriteDay17ToConsole();
            ProcessLocations();
            WriteDay17ToConsole();

            int waterCount = 0;
            minY = ranges.Min(r => r.StartY);
            for (int y = minY; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] == '|' || grid[y, x] == '~')
                        waterCount++;
                }
            }

            return waterCount;
        }


        protected override object SolvePartTwo()
        {
            return grid.Cast<char>().ToList().Where(c => c == '~').Count();
        }

        private void ProcessLocations()
        {
            while (_points.Count != 0)
            {
                Point p = _points.Pop();

                if (p.Y > maxY)
                    continue;

                if (grid[p.Y, p.X] == '.' || grid[p.Y, p.X] == '#')
                    continue;

                if (grid[p.Y, p.X] == '|' && grid[p.Y + 1, p.X] == '.')
                {
                    PushPoint(p.Offset(0, 1), '|');
                }
                if (grid[p.Y, p.X] == '|')
                {
                    bool supported = true;
                    for (int col = p.X; true; col--)
                    {
                        Point check = new Point(col, p.Y);
                        if (grid[check.Y, check.X] != '.')
                        {
                            if (grid[check.Y, check.X] == '#')
                                break;
                        }
                        if (grid[check.Y + 1, check.X] == '.')
                        {
                            supported = false;
                            break;
                        }
                        if (grid[check.Y + 1, check.X] != '#' && grid[check.Y + 1, check.X] != '~')
                        {
                            supported = false;
                            break;
                        }
                    }
                    for (int col = p.X; true; col++)
                    {
                        Point check = new Point(col, p.Y);
                        if (grid[check.Y, check.X] != '.')
                        {
                            if (grid[check.Y, check.X] == '#')
                                break;
                        }
                        if (grid[check.Y + 1, check.X] == '.')
                        {
                            supported = false;
                            break;
                        }
                        if (grid[check.Y + 1, check.X] != '#' && grid[check.Y + 1, check.X] != '~')
                        {
                            supported = false;
                            break;
                        }
                    }
                    if (supported)
                    {
                        PushPoint(p, '~');
                    }
                }

                if (grid[p.Y, p.X] == '|' && grid[p.Y + 1, p.X] != '.' && (grid[p.Y + 1, p.X] == '~' || grid[p.Y + 1, p.X] == '#'))
                {
                    PushPoint(p.Offset(-1, 0), '|');
                    PushPoint(p.Offset(1, 0), '|');
                }

                if (grid[p.Y, p.X] == '~')
                {
                    PushPoint(p.Offset(-1, 0), '~');
                    PushPoint(p.Offset(1, 0), '~');
                }

            }
        }

        private void PushPoint(Point p, char c)
        {
            if (p.Y > maxY)
                return;

            if (grid[p.Y, p.X] == '#')
                return;

            if (grid[p.Y, p.X] == '.' || grid[p.Y, p.X] != c)
            {
                _points.Push(p.Offset(1, 0)); // RIGHT
                _points.Push(p.Offset(0, 1)); // UP
                _points.Push(p.Offset(-1, 0)); // LEFT
                _points.Push(p.Offset(0, -1)); // DOWN
                _points.Push(p); // MIDDLE

            }


            grid[p.Y, p.X] = c;
            //Console.SetCursorPosition(p.X + 4, p.Y + 5);
            //Console.Write(c);

        }

        private void WriteDay17ToConsole()
        {
            var consoleRowLabels = Enumerable.Range(0, grid.GetLength(0) + 1).Select(i => $"{i,4}").ToArray();

            WriteConsole((short)consoleRowLabels.Length, 4, 0, 5, (row, col)
                => (ConsoleColor.White, consoleRowLabels[row][col]));
            WriteConsole((short)grid.GetLength(0), (short)grid.GetLength(1), 4, 5,
                (row, col) =>
                {
                    ConsoleColor color;
                    switch (grid[row, col])
                    {
                        case '.':
                            color = ConsoleColor.DarkYellow;
                            break;
                        case '|':
                            color = ConsoleColor.Yellow;
                            break;
                        case '~':
                            color = ConsoleColor.Blue;
                            break;
                        case '#':
                            color = ConsoleColor.DarkRed;
                            break;
                        default:
                            color = ConsoleColor.White;
                            break;
                    }

                    return (color, grid[row, col]);
                });
        }


        private static string[] tinput =
@"x=495, y=2..7
y=7, x=495..501
x=501, y=3..7
x=498, y=2..4
x=506, y=1..2
x=498, y=10..13
x=504, y=10..13
y=13, x=498..504
".Split().ToArray();

        private enum FlowType
        {
            Stagnant,
            Unsettled,
            Unknown
        }
    }

    public class Range
    {
        public Range(int startX, int endX, int startY, int endY)
        {
            if (endX == 0)
                endX = startX;
            if (endY == 0)
                endY = startY;

            StartX = startX;
            EndX = endX;
            StartY = startY;
            EndY = endY;
        }

        public int StartX { get; }
        public int EndX { get; }
        public int StartY { get; }
        public int EndY { get; }

        public static Range Parse(string s)
        {
            var x = Regex.Match(s, @"x=(?<startx>\d+)(\.\.(?<endx>\d+))?");
            var y = Regex.Match(s, @"y=(?<starty>\d+)(\.\.(?<endy>\d+))?");
            int.TryParse(x.Groups["startx"].Value, out int startX);
            int.TryParse(x.Groups["endx"].Value, out int endX);
            int.TryParse(y.Groups["starty"].Value, out int startY);
            int.TryParse(y.Groups["endy"].Value, out int endY);

            return new Range(startX, endX, startY, endY);
        }
    }
}
