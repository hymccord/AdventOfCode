using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Numerics;

using AdventOfCode.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;

using Nito.AsyncEx;

using Windows.Win32;
using Windows.Win32.System.Console;

namespace AdventOfCode.Solutions
{

    interface ISolution
    {
        int Day { get; }
        int Year { get; }
        string Title { get; }

        Task RunAsync(IOptions<Session> session, CancellationToken cancellationToken);
    }
    abstract class ASolution : ISolution
    {
        AsyncLazy<string> _input;
        protected readonly bool _debugInput;
        readonly Lazy<string[]> _inputByNewLine;
        readonly Lazy<object> _part1, _part2;
#if RELEASE
        TimeSpan _perf1, _perf2;
#endif

        public int Day { get; }
        public int Year { get; }
        public string Title { get; }


        public string Input => string.IsNullOrEmpty(_input.Task.Result) ? null : _input.Task.Result;
        public string[] InputByNewLine => string.IsNullOrEmpty(_input.Task.Result) ? null : _inputByNewLine.Value;
        public string Part1 => $"{_part1.Value ?? ""}";
        public string Part2 => $"{_part2.Value ?? ""}";
        protected bool DebugOutput { get; set; } = false;

        private protected ASolution(int day, int year, string title = "", bool debug = false)
        {
            Day = day;
            Year = year;
            Title = title;
            _debugInput = debug;

            _inputByNewLine = new Lazy<string[]>(() => Input.SplitByNewline());
            _part1 = new Lazy<object>(() =>
            {
                var watch = Stopwatch.StartNew();
                var o = SolvePartOne();
#if RELEASE
                _perf1 = watch.Elapsed;
#endif
                return o;
            });
            _part2 = new Lazy<object>(() =>
            {
                var watch = Stopwatch.StartNew();
                var o = SolvePartTwo();
#if RELEASE
                _perf2 = watch.Elapsed;
#endif
                return o;
            });
        }

        public async Task RunAsync(IOptions<Session> config, CancellationToken cancellationToken)
        {
            _input = new AsyncLazy<string>(() => LoadInputAsync(config));
            await Solve(0, cancellationToken);
        }

        public Task Solve(int part = 0, CancellationToken cancellation = default)
        {
            if (Input == null)
            {
                return Task.CompletedTask;
            }

            Console.WriteLine($"--- Day {Day}: {Title} ---");

            Preprocess();

            if (part != 2)
            {
                Console.WriteLine($"Part 1: {(!string.IsNullOrEmpty(Part1) ? Part1 : "Unsolved")}");
#if RELEASE
                Console.WriteLine($"  (in {_perf1:ss\\.fffff}s)");
#endif
            }
            if (part != 1)
            {
                Console.WriteLine($"Part 2: {(!string.IsNullOrEmpty(Part2) ? Part2 : "Unsolved")}");
#if RELEASE
                Console.WriteLine($"  (in {_perf2:ss\\.fffff}s)");
#endif
            }

            return Task.CompletedTask;
        }

        async Task<string> LoadInputAsync(IOptions<Session> session)
        {
            string INPUT_URL = $"https://adventofcode.com/{Year}/day/{Day}/input";
            string input = string.Empty;

            string basePath = Environment.CurrentDirectory;
            while (!File.Exists(Path.Combine(basePath, "AdventOfCode.sln")))
            {
                basePath = Directory.GetParent(basePath).FullName;
            }

            string INPUT_FILEPATH = Path.Combine(basePath, $"Solutions/Year{Year}/Day{Day:D2}input");
            string INPUT_FILEPATH_ALTERNATE = Path.Combine(basePath, $"Solutions/Year{Year}/Day{Day:D2}/input");

            if (File.Exists(INPUT_FILEPATH))
            {
                input = File.ReadAllText(INPUT_FILEPATH);
            }
            else if (File.Exists(INPUT_FILEPATH_ALTERNATE))
            {
                input = File.ReadAllText(INPUT_FILEPATH_ALTERNATE);
            }
            else
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", $"session={session.Value.Token}");
                // https://www.reddit.com/r/adventofcode/comments/z9dhtd/please_include_your_contact_info_in_the_useragent/
                client.DefaultRequestHeaders.Add("User-Agent", ".NET/7.0 (github.com/hymccord/adventofcode + hymccord@gmail.com)");
                var response = await client.GetAsync(INPUT_URL);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        input = await response.Content.ReadAsStringAsync();
                        _ = File.WriteAllTextAsync(INPUT_FILEPATH, input.TrimEnd());
                        break;
                    case HttpStatusCode.BadRequest:
                        Console.WriteLine($"Day {Day}: Error code 400 when attempting to retrieve puzzle input through the web client. Your session cookie is probably not recognized.");
                        break;
                    case HttpStatusCode.NotFound:
                        Console.WriteLine($"Day {Day}: Error code 404 when attempting to retrieve puzzle input through the web client. The puzzle is probably not available yet.");
                        break;
                    default:
                        await Console.Out.WriteLineAsync($"Day {Day}: Error code {response.StatusCode}.");
                        break;
                }
            }

            if (_debugInput)
            {
                return LoadDebugInput();
            }

            return input;
        }

        protected virtual string LoadDebugInput() => string.Empty;

        protected virtual void Preprocess() { }

        protected abstract object SolvePartOne();
        protected abstract object SolvePartTwo();

        protected void WriteOutput(string output)
        {
            if (DebugOutput)
            {
                Console.WriteLine(output);
            }
        }

#nullable enable
        [DebuggerStepThrough]
        [DebuggerDisplay("{X}, {Y}")]
        public struct Point : IEquatable<Point>,
            IAdditionOperators<Point, Point, Point>,
            ISubtractionOperators<Point, Point, Point>,
            IUnaryNegationOperators<Point, Point>
        {
            public readonly int X;
            public readonly int Y;

            /// <summary>
            /// Get NSEW points.
            /// </summary>
            public IEnumerable<Point> Neighbors
            {
                get
                {
                    foreach (var direction in Cardinal)
                    {
                        yield return Offset(direction);
                    }
                }
            }

            /// <summary>
            /// Get all 4 diagonal points.
            /// </summary>
            public IEnumerable<Point> DiagonalNeighbors
            {
                get
                {
                    foreach (var direction in Diagonals)
                    {
                        yield return Offset(direction);
                    }
                }
            }

            /// <summary>
            /// Get all 8 surrounding points.
            /// </summary>
            public IEnumerable<Point> SplattNeighbors
            {
                get
                {
                    foreach (var direction in Splatt)
                    {
                        yield return Offset(direction);
                    }
                }
            }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Point Offset(Point p)
            {
                return this + p;
            }

            public Point Offset(int x, int y)
            {
                return new Point(X + x, Y + y);
            }

            public static bool operator ==(Point a, Point b)
            {
                return a.X == b.X && a.Y == b.Y;
            }

            public static bool operator !=(Point a, Point b)
            {
                return a.X != b.X || a.Y != b.Y;
            }

            public static Point operator +(Point a, Point b)
            {
                return new Point(a.X + b.X, a.Y + b.Y);
            }

            public static Point operator -(Point a, Point b)
            {
                return new Point(a.X - b.X, a.Y - b.Y);
            }

            public static Point operator -(Point a)
            {
                return new Point(-a.X, -a.Y);
            }

            public static Point operator *(Point a, int value)
            {
                return new Point(a.X * value, a.Y * value);
            }

            public static implicit operator Point((int x, int y) t)
            {
                return new Point(t.x, t.y);
            }

            public static implicit operator (int X, int Y)(Point p)
            {
                return (p.X, p.Y);
            }

            public void Deconstruct(out int x, out int y) =>
                (x, y) = (X, Y);

            public bool IsInBoundsOfArray(int rows, int cols) => X >= 0 && Y >= 0 && X < cols && Y < rows;

            public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Point point && Equals(point);

            public readonly bool Equals(Point other) => this == other;

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            public override string ToString()
            {
                return $"{X}, {Y}";
            }

            public static IEnumerable<Point> Cardinal
            {
                get
                {
                    yield return North;
                    yield return East;
                    yield return South;
                    yield return West;
                }
            }

            public static IEnumerable<Point> Diagonals
            {
                get
                {
                    yield return SouthEast;
                    yield return NorthEast;
                    yield return SouthWest;
                    yield return NorthWest;

                }
            }

            public static IEnumerable<Point> Splatt
            {
                get
                {
                    foreach (var p in Cardinal)
                    {
                        yield return p;
                    }

                    foreach (var p in Diagonals)
                    {
                        yield return p;
                    }
                }
            }

            public int Manhatten(Point p)
            {
                return Math.Abs(X - p.X) + Math.Abs(Y - p.Y);
            }

            public double Distance(Point p)
            {
                var xDiff = X - p.X;
                var yDiff = Y - p.Y;
                return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
            }

            public static Point North = new(0, -1);
            public static Point East = new(1, 0);
            public static Point South = new(0, 1);
            public static Point West = new(-1, 0);
            public static Point NorthEast = new(1, -1);
            public static Point SouthEast = new(1, 1);
            public static Point NorthWest = new(-1, -1);
            public static Point SouthWest = new(-1, 1);
        }
#nullable restore

        protected void WriteCharGrid(char[,] src, short left = 15, short top = 5)
        {
            WriteConsole(src.RowLength(), src.ColLength(), left, top, (row, col) =>
            {
                return (ConsoleColor.White, src[row, col]);
            });
        }

        protected void Write2DGrid<T>(T[,] src, short left = 15, short top = 5)
        {
            WriteConsole(src.RowLength(), src.ColLength(), left, top, (row, col) =>
            {
                return (ConsoleColor.White, src[row, col].ToString()[0]);
            });
        }

        protected void WriteConsole(int rows, int cols, short left, short top, Func<int, int, (ConsoleColor, char)> indexingFunc)
        {
            SafeFileHandle h = OpenConOut();

            if (!h.IsInvalid)
            {
                var screenBuffer = new CHAR_INFO[rows * cols];

                SMALL_RECT rect = new()
                {
                    Left = left,
                    Top = top,
                    Right = (short)(cols + left),
                    Bottom = (short)(rows + top)
                };

                ConsoleColor color;
                char character;
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        (color, character) = indexingFunc(y, x);
                        screenBuffer[y * cols + x].Attributes = (ushort)color;
                        screenBuffer[y * cols + x].Char.AsciiChar = new Windows.Win32.Foundation.CHAR((byte)character);
                    }
                }
                PInvoke.WriteConsoleOutput(h,
                    in screenBuffer[0],
                    new COORD { X = (short)cols, Y = (short)rows },
                    new COORD { X = 0, Y = 0 },
                    ref rect);
            }
        }

        protected void WriteConsole(int row, int col, int left, int top, char c, ConsoleColor color = ConsoleColor.White)
        {
            SafeFileHandle h = OpenConOut();

            if (!h.IsInvalid)
            {
                var character = new CHAR_INFO();
                character.Attributes = (ushort)color;
                character.Char.UnicodeChar = c;

                var rect = new SMALL_RECT
                {
                    Left = (short)(left + col),
                    Top = (short)(top + row),
                    Right = (short)(col + left),
                    Bottom = (short)(row + top)
                };

                PInvoke.WriteConsoleOutput(h,
                    in character,
                    new COORD() { X = 1, Y = 1 },
                    new COORD() { X = 0, Y = 0 },
                    ref rect);
            }
        }

        protected void ClearConsole()
        {
            var rows = (short)Console.WindowHeight;
            var cols = (short)Console.WindowWidth;
            SafeFileHandle h = OpenConOut();


            if (!h.IsInvalid)
            {
                var screenBuffer = new CHAR_INFO[rows * cols];

                var writeRegion = new SMALL_RECT
                {
                    Left = 0,
                    Top = 0,
                    Right = (short)(cols),
                    Bottom = (short)(rows)
                };
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        screenBuffer[y * cols + x].Attributes = (byte)Console.ForegroundColor;
                        screenBuffer[y * cols + x].Char.UnicodeChar = ' ';
                    }
                }
                PInvoke.WriteConsoleOutput(
                    h,
                    screenBuffer[0],
                    new COORD { X = cols, Y = rows },
                    new COORD(),
                    ref writeRegion
                    );
            }
        }

        private SafeFileHandle OpenConOut()
        {
            SafeFileHandle h = PInvoke.CreateFile("CONOUT$",
                Windows.Win32.Storage.FileSystem.FILE_ACCESS_FLAGS.FILE_GENERIC_READ | Windows.Win32.Storage.FileSystem.FILE_ACCESS_FLAGS.FILE_GENERIC_WRITE,
                Windows.Win32.Storage.FileSystem.FILE_SHARE_MODE.FILE_SHARE_READ | Windows.Win32.Storage.FileSystem.FILE_SHARE_MODE.FILE_SHARE_WRITE,
                null,
                Windows.Win32.Storage.FileSystem.FILE_CREATION_DISPOSITION.OPEN_EXISTING,
                Windows.Win32.Storage.FileSystem.FILE_FLAGS_AND_ATTRIBUTES.FILE_ATTRIBUTE_NORMAL,
                null);
            return h;
        }

        [DebuggerDisplay("{Value}")]
        public class TreeNode<T> : IEnumerable<TreeNode<T>>, IEnumerable
        {
            private readonly T _value;
            private readonly HashSet<TreeNode<T>> _children = new();

            public TreeNode(T value)
            {
                _value = value;
            }

            //public TreeNode<T> this[int i] => _children[i];

            public TreeNode<T> Parent { get; private set; }

            public T Value => _value;

            public ICollection<TreeNode<T>> Children => _children;

            public TreeNode<T> AddChild(TreeNode<T> node)
            {
                node.Parent = this;
                _children.Add(node);
                return node;
            }

            public TreeNode<T> AddChild(T value)
            {
                var node = new TreeNode<T>(value) { Parent = this };
                _children.Add(node);
                return node;
            }

            public TreeNode<T>[] AddChildren(params TreeNode<T>[] values)
            {
                return values.Select(AddChild).ToArray();
            }

            public TreeNode<T>[] AddChildren(params T[] values)
            {
                return values.Select(AddChild).ToArray();
            }

            public bool RemoveChild(TreeNode<T> node)
            {
                return _children.Remove(node);
            }

            public void Traverse(Action<T> action)
            {
                action(Value);
                foreach (var child in _children)
                {
                    child.Traverse(action);
                }
            }

            public IEnumerable<T> Flatten()
            {
                return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
            }

            public IEnumerator<TreeNode<T>> GetEnumerator()
            {
                foreach (var node in _children)
                {
                    yield return node;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        internal class AStar<T>
        {
            private readonly WeightedGrid<T> _grid;

            public AStar(WeightedGrid<T> grid)
            {
                _grid = grid;
            }

            public virtual List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
            {
                var totalPath = new List<Point> { current };
                while (cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    totalPath.Insert(0, current);
                }

                return totalPath;
            }

            public ICollection<Point> Dijkstra(Point start, Point goal)
                => A_Star(start, goal, (goal, next) => 0);

            public List<Point> A_Star(Point start, Point goal, Func<Point, Point, int> heuristic)
            {
                var frontier = new PriorityQueue<Point, double>();
                frontier.Enqueue(start, 0);

                var costSoFar = new Dictionary<Point, double>();
                costSoFar.Add(start, 0);

                var cameFrom = new Dictionary<Point, Point>();

                while (frontier.Count > 0)
                {
                    Point current = frontier.Dequeue();

                    if (current == goal)
                        return ReconstructPath(cameFrom, current);

                    foreach (Point next in _grid.GetNeighbors(current))
                    {
                        if (!next.IsInBoundsOfArray(_grid.Height, _grid.Width))
                            continue;

                        var cost = costSoFar[current] + _grid.Cost(current, next);
                        if (!costSoFar.ContainsKey(next) || cost < costSoFar[next])
                        {
                            _grid.EnterLoc(current, next);

                            cameFrom[next] = current;
                            costSoFar[next] = cost;
                            double priority = cost + heuristic(goal, next);
                            frontier.Enqueue(next, priority);
                        }
                    }
                }

                throw new InvalidOperationException("Could not find path between start and goal");
            }
        }

        internal abstract class WeightedGrid<T>
        {
            protected readonly T[,] _grid;

            public WeightedGrid(T[,] grid)
            {
                _grid = grid;
                Height = _grid.GetLength(0);
                Width = _grid.GetLength(1);
            }

            public int Width { get; }
            public int Height { get; }

            internal abstract double Cost(Point from, Point to);

            internal virtual void EnterLoc(Point from, Point to) { }

            /// <summary>
            /// Customize getting neigbors. Defaults to cardinal only.
            /// </summary>
            internal virtual IEnumerable<Point> GetNeighbors(Point p)
            {
                return p.Neighbors;
            }
        }

        internal class AStar<T, TState>
        {
            private readonly WeightedGrid<T, TState> _grid;

            public AStar(WeightedGrid<T, TState> grid)
            {
                _grid = grid;
            }

            public virtual List<T> ReconstructPath(Dictionary<T, T> cameFrom, T current)
            {
                var totalPath = new List<T> { current };
                while (cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    totalPath.Insert(0, current);
                }

                return totalPath;
            }

            public ICollection<T> Dijkstra(T start, T goal)
                => A_Star(start, goal, (goal, next) => 0);

            public List<T> A_Star(T start, T goal, Func<T, T, int> heuristic)
            {
                var frontier = new PriorityQueue<T, double>();
                frontier.Enqueue(start, 0);

                var costSoFar = new Dictionary<T, double>();
                costSoFar.Add(start, 0);

                var cameFrom = new Dictionary<T, T>();

                while (frontier.Count > 0)
                {
                    var current = frontier.Dequeue();

                    if (EqualityComparer<T>.Default.Equals(current, goal))
                        return ReconstructPath(cameFrom, current);

                    foreach (T next in _grid.GetNeighbors(current))
                    {
                        var cost = costSoFar[current] + _grid.Cost(current, next);
                        if (!costSoFar.ContainsKey(next) || cost < costSoFar[next])
                        {
                            _grid.EnterLoc(current, next);

                            cameFrom[next] = current;
                            costSoFar[next] = cost;
                            double priority = cost + heuristic(goal, next);
                            frontier.Enqueue(next, priority);
                        }
                    }
                }

                throw new InvalidOperationException("Could not find path between start and goal");
            }
        }

        internal abstract class WeightedGrid<T, TState>
        {
            protected readonly T[,] _grid;

            public WeightedGrid(T[,] grid)
            {
                _grid = grid;
                Height = _grid.GetLength(0);
                Width = _grid.GetLength(1);
            }

            public int Width { get; }
            public int Height { get; }

            internal abstract double Cost(T from, T to);

            internal virtual void EnterLoc(T from, T to) { }

            internal abstract IEnumerable<T> GetNeighbors(T p);
        }
    }
}
