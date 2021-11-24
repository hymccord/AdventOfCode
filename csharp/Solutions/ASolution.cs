using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using Windows.Win32;
using Windows.Win32.System.Console;

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
#if RELEASE
        long _perf1, _perf2;
#endif

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
#if RELEASE
                _perf1 = watch.ElapsedMilliseconds; 
#endif
                return o;
            });
            _part2 = new Lazy<object>(() =>
            {
                var watch = Stopwatch.StartNew();
                var o = SolvePartTwo();
#if RELEASE
                _perf2 = watch.ElapsedMilliseconds; 
#endif
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
            string INPUT_FILEPATH = $"Solutions/Year{Year}/Day{Day:D2}input";
            string INPUT_FILEPATH_ALTERNATE = $"Solutions/Year{Year}/Day{Day:D2}/input";
            string INPUT_URL = $"https://adventofcode.com/{Year}/day/{Day}/input";
            string input = "";

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
                try
                {
                    using var client = new WebClient();
                    client.Headers.Add(HttpRequestHeader.Cookie, Program.Config.Cookie);
                    input = client.DownloadString(INPUT_URL).Trim();
                    File.WriteAllText(INPUT_FILEPATH, input);
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
            if (DebugOutput)
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

            public static Point operator *(Point a, int value)
            {
                return new Point(a.X * value, a.Y * value);
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

            public static Point North = new(0, -1);
            public static Point East = new(1, 0);
            public static Point South = new(0, 1);
            public static Point West = new(-1, 0);
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

        protected void WriteConsole(int row, int col, short left, short top, char c, ConsoleColor color = ConsoleColor.White)
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
            private T _value;
            private HashSet<TreeNode<T>> _children = new();

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
                SortedList<double, Point> frontier = new(new DuplicateKeyComparer<double>());
                HashSet<Point> visited = new();
                Dictionary<Point, double> costSoFar = new() { { start, 0 } };
                Dictionary<Point, Point> cameFrom = new();
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
