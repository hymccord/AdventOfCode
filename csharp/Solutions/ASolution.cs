using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace AdventOfCode.Solutions
{

    interface ISolution
    {
        int Day { get; }
        int Year { get; }
        string Title { get; }
    }
    abstract class ASolution : ISolution
    {

        Lazy<string> _input;
        Lazy<object> _part1, _part2;
        long _perf1, _perf2;

        public int Day { get; }
        public int Year { get; }
        public string Title { get; }
        public string Input => string.IsNullOrEmpty(_input.Value) ? null : _input.Value;
        public string Part1 => $"{_part1.Value ?? ""}";
        public string Part2 => $"{_part2.Value ?? ""}";
        protected bool DebugOutput { get; set; } = false;

        private protected ASolution(int day, int year, string title = "")
        {
            Day = day;
            Year = year;
            Title = title;
            _input = new Lazy<string>(() => LoadInput());
            _part1 = new Lazy<object>(() =>
            {
                var watch = Stopwatch.StartNew();
                var o = SolvePartOne();
                _perf1 = watch.ElapsedMilliseconds;
                return o;
            });
            _part2 = new Lazy<object>(() =>
            {
                var watch = Stopwatch.StartNew();
                var o = SolvePartTwo();
                _perf2 = watch.ElapsedMilliseconds;
                return o;
            });
        }

        public void Solve(int part = 0)
        {
            if (Input == null) return;

            Console.WriteLine($"--- Day {Day}: {Title} ---");

            if (part != 2)
            {
                Console.WriteLine($"Part 1: {(!string.IsNullOrEmpty(Part1) ? Part1 : "Unsolved")}");
#if RELEASE
                Console.WriteLine($"  (in {_perf1}ms)");
#endif
            }
            if (part != 1)
            {
                Console.WriteLine($"Part 2: {(!string.IsNullOrEmpty(Part2) ? Part2 : "Unsolved")}");
#if RELEASE
                Console.WriteLine(@$"  (in {_perf2}ms)");
#endif
            }
        }

        string LoadInput()
        {
            string INPUT_FILEPATH = $"Solutions/Year{Year}/Day{Day.ToString("D2")}/input";
            string INPUT_URL = $"https://adventofcode.com/{Year}/day/{Day}/input";
            string input = "";

            if (File.Exists(INPUT_FILEPATH))
            {
                input = File.ReadAllText(INPUT_FILEPATH);
            }
            else
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, Program.Config.Cookie);
                        input = client.DownloadString(INPUT_URL).Trim();
                        File.WriteAllText(INPUT_FILEPATH, input);
                    }
                }
                catch (WebException e)
                {
                    var statusCode = ((HttpWebResponse)e.Response).StatusCode;
                    if (statusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine($"Day {Day}: Error code 400 when attempting to retrieve puzzle input through the web client. Your session cookie is probably not recognized.");
                    }
                    else if (statusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Day {Day}: Error code 404 when attempting to retrieve puzzle input through the web client. The puzzle is probably not available yet.");
                    }
                    else
                    {
                        Console.WriteLine(e.Status);
                    }
                }
            }
            return input;
        }

        protected abstract object SolvePartOne();
        protected abstract object SolvePartTwo();

        protected void WriteOutput(string output)
        {
            if (this.DebugOutput)
            {
                Console.WriteLine(output);
            }
        }

        [DebuggerStepThrough]
        [DebuggerDisplay("{X}, {Y}")]
        public struct Point
        {
            public int X;
            public int Y;

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

            public void Deconstruct(out int x, out int y) =>
                (x, y) = (X, Y);

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

            public static IEnumerable<Point> Splatt
            {
                get
                {
                    foreach (var p in Cardinal)
                    {
                        yield return p;
                    }
                    yield return new Point(1, 1);
                    yield return new Point(1, -1);
                    yield return new Point(-1, 1);
                    yield return new Point(-1, -1);
                }
            }

            public int Manhatten(Point p)
            {
                return Math.Abs(X - p.X) + Math.Abs(Y - p.Y);
            }

            public static Point North = new Point(0, -1);
            public static Point East = new Point(1, 0);
            public static Point South = new Point(0, 1);
            public static Point West = new Point(-1, 0);
        }

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }


        protected void WriteConsole(int rows, int cols, short left, short top, Func<int, int, (ConsoleColor, char)> indexingFunc)
        {
            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!h.IsInvalid)
            {
                CharInfo[] buf = new CharInfo[rows * cols];

                SmallRect rect = new SmallRect
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
                        buf[y * cols + x].Attributes = (short)color;
                        buf[y * cols + x].Char.AsciiChar = (byte)character;
                    }
                }
                bool b = WriteConsoleOutput(h, buf,
                    new Coord() { X = (short)cols, Y = (short)rows },
                    new Coord() { X = 0, Y = 0 },
                    ref rect);
            }
        }

        protected void WriteConsole(int row, int col, short left, short top, char c, ConsoleColor color = ConsoleColor.White)
        {
            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!h.IsInvalid)
            {
                CharInfo[] buf = new CharInfo[1];
                buf[0].Attributes = (short)color;
                buf[0].Char.AsciiChar = (byte)c;

                var rect = new SmallRect
                {
                    Left = (short)(left + col),
                    Top = (short)(top + row),
                    Right = (short)(col + left),
                    Bottom = (short)(row + top)
                };

                bool b = WriteConsoleOutput(h, buf,
                    new Coord() { X = 1, Y = 1 },
                    new Coord() { X = 0, Y = 0 },
                    ref rect);
            }
        }

        [DebuggerDisplay("{Value}")]
        public class TreeNode<T> : IEnumerable<TreeNode<T>>, IEnumerable
        {
            private T _value;
            private HashSet<TreeNode<T>> _children = new HashSet<TreeNode<T>>();

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

            public List<Point> A_Star(Point start, Point goal, Func<Point, Point, int> h)
            {
                SortedList<double, Point> frontier = new SortedList<double, Point>(new DuplicateKeyComparer<double>());
                HashSet<Point> visited = new HashSet<Point>();
                Dictionary<Point, double> costSoFar = new Dictionary<Point, double>() { { start, 0 } };
                Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
                frontier.Add(0, start);

                while (!(frontier.Count == 0))
                {
                    Point current = frontier.Values[0];

                    if (current == goal)
                        return ReconstructPath(cameFrom, current);

                    frontier.RemoveAt(0);
                    visited.Add(current);

                    foreach (Point next in current.Neighbors)
                    {
                        if (next.X < 0 || next.Y < 0 || next.X >= _grid.Width || next.Y >= _grid.Height)
                            continue;
                        var newCost = costSoFar[current] + _grid.Cost(current, next);
                        if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            _grid.EnterLoc(current, next);
                            costSoFar[next] = newCost;
                            double priority = newCost + h(goal, next);
                            frontier.Add(priority, next);
                            cameFrom[next] = current;
                        }
                    }
                }

                return null;
            }

            private class DuplicateKeyComparer<TKey>
                : IComparer<TKey> where TKey : IComparable
            {
                public int Compare(TKey x, TKey y)
                {
                    int result = x.CompareTo(y);
                    if (result == 0)
                        return 1;

                    return result;
                }
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
        }
    }
}
