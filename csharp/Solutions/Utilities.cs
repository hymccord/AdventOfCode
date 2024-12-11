

using System.Runtime.CompilerServices;
using System.Text;

using static AdventOfCode.Solutions.ASolution;

/**
 * This utility class is largely based on: 
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */
namespace AdventOfCode.Solutions
{

    public static class Utilities
    {

        public static int[] ToIntArray(this string str)
            => str
                .Split(new char[] { ' ', '\n' })
                .Where(n => int.TryParse(n, out int v))
                .Select(n => Convert.ToInt32(n))
                .ToArray();

        public static long[] ToLongArray(this string str)
            => str
                .Split(new char[] { ' ', '\n' })
                .Where(n => long.TryParse(n, out long v))
                .Select(n => Convert.ToInt64(n))
                .ToArray();

        public static int[] ToIntArray(this string str, params char[] separator)
            => str
                .Split(separator)
                .Where(n => int.TryParse(n, out int v))
                .Select(n => Convert.ToInt32(n))
                .ToArray();

        public static int[,] To2DIntArray(this string str)
            => str
            .SplitByNewline()
            .Select(s => s.ToCharArray().Select(c => c - '0').ToArray())
            .ToArray()
            .To2D();

        public static int MinOfMany(params int[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Min(result, items[i]);
            }
            return result;
        }

        public static int MaxOfMany(params int[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Max(result, items[i]);
            }
            return result;
        }

        // https://stackoverflow.com/a/3150821/419956 by @RonWarholic
        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> nested)
        {
            foreach (var nestedItem in nested)
            {
                foreach (var item in nestedItem)
                {
                    yield return item;
                }
            }
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items, string separator = "")
        {
            return string.Join(separator, items);
        }

        public static string[] SplitByNewline(this string input, bool shouldTrim = false)
        {
            return input
                .Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.None)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => shouldTrim ? s.Trim() : s)
                .ToArray();
        }

        public static string[] SplitByBlankLine(this string input, bool shouldTrim = false)
        {
            return input
                .Split(new[] { "\r\r", "\n\n", "\r\n\r\n" }, StringSplitOptions.None)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => shouldTrim ? s.Trim() : s)
                .ToArray();
        }

        public static IEnumerable<IEnumerable<T>> Permutate<T>(this IEnumerable<T> source, int length)
        {
            if (length == 1)
                return source.Select(t => new T[] { t });

            return source.Permutate(length - 1)
                .SelectMany(t => source.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private static long GCD(long a, long b)
        {
            while (b > 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static long GCD(this long[] input)
        {
            long result = input[0];
            for (int i = 1; i < input.Length; i++)
            {
                result = LCM(result, input[i]);
            }

            return result;
        }

        public static long LCM(long a, long b)
        {
            return a * b / GCD(a, b);
        }

        public static long LCM(this IEnumerable<long> input)
        {
            return input.Aggregate(1L, LCM);
        }

        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        public static char[,] To2DCharArray(this string src)
        {
            string[] lines = src.SplitByNewline();
            char[,] dest = new char[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    dest[i, j] = lines[i][j];
                }
            }

            return dest;
        }

        public static T[,] To2D<T>(this T[][] src)
        {
            try
            {
                int FirstDim = src.Length;
                int SecondDim = src.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = src[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

        public static T[,] To2D<T>(this IList<T[]> src)
        {
            try
            {
                int FirstDim = src.Count;
                int SecondDim = src.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = src[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

        public static T[,] PadGrid<T>(this T[,] src, T padding, int count = 1)
        {
            int rows = src.RowLength();
            int cols = src.ColLength();

            T[,] paddedArray = new T[
                src.GetLength(0) + 2 * count,
                src.GetLength(1) + 2 * count
            ];

            for (var row = 0; row < rows + 2 * count; row++)
            {
                for (var col = 0; col < cols + 2 * count; col++)
                {
                    if (row < count || row >= (rows + count))
                    {
                        paddedArray[row, col] = padding;
                    }
                    else
                    {
                        if (col < count || col >= (cols + count))
                        {
                            paddedArray[row, col] = padding;
                        }
                        else
                        {
                            paddedArray[row, col] = src[row - count, col - count];
                        }
                    }
                }
            }

            return paddedArray;
        }

        public static T[,] Transpose<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] transposed = new T[columnCount, rowCount];
            if (rowCount == columnCount)
            {
                transposed = (T[,])arr.Clone();
                for (int i = 1; i < rowCount; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        T temp = transposed[i, j];
                        transposed[i, j] = transposed[j, i];
                        transposed[j, i] = temp;
                    }
                }
            }
            else
            {
                for (int column = 0; column < columnCount; column++)
                {
                    for (int row = 0; row < rowCount; row++)
                    {
                        transposed[column, row] = arr[row, column];
                    }
                }
            }
            return transposed;
        }

        public static IEnumerable<T> SliceRow<T>(this T[,] arr, int row)
        {
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                yield return arr[row, i];
            }
        }

        public static IEnumerable<T> SliceColumn<T>(this T[,] arr, int column)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                yield return arr[i, column];
            }
        }

        public static int RowLength<T>(this T[,] arr)
        {
            return arr.GetLength(0);
        }

        public static int ColLength<T>(this T[,] arr)
        {
            return arr.GetLength(1);
        }

        internal static HashSet<Point> GetPointHashset<T>(this T[,] arr, T comparison)
        {
            var set = new HashSet<Point>();
            for (int row = 0; row < arr.RowLength(); row++)
            {
                for (int col = 0; col < arr.ColLength(); col++)
                {
                    if (EqualityComparer<T>.Default.Equals(arr[row, col], comparison))
                    {
                        set.Add((col, row));
                    }
                }
            }

            return set;
        }

        internal static HashSet<Point> GetPointHashset<T>(this T[,] arr, Func<T, bool> predicate)
        {
            var set = new HashSet<Point>();
            for (int row = 0; row < arr.RowLength(); row++)
            {
                for (int col = 0; col < arr.ColLength(); col++)
                {
                    if (predicate(arr[row, col]))
                    {
                        set.Add((col, row));
                    }
                }
            }

            return set;
        }

        internal static T At<T>(this T[,] arr, Point p)
        {
            return arr[p.Y, p.X];
        }

        internal static T TryAt<T>(this T[,] arr, Point p, T defaultValue)
        {
            if (!p.IsInBoundsOfGrid(arr))
            {
                return defaultValue;
            }

            return arr[p.Y, p.X];
        }

        public static string GetSimpleString(this char[,] arr)
        {
            var sb = new StringBuilder();
            var rows = arr.RowLength();
            var cols = arr.ColLength();

            for (var row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    sb.Append(arr[row, col]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value, Func<TKey, TValue, TValue> updateExisting)
        {
            if (dict.TryGetValue(key, out TValue existing))
            {
                dict[key] = updateExisting(key, existing);
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items,
            Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }

        public static IEnumerable<IList<TSource>> Window<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            return _(); 
            
            IEnumerable<IList<TSource>> _()
            {
                using var iter = source.GetEnumerator();

                // generate the first window of items
                var window = new TSource[size];
                int i;
                for (i = 0; i < size && iter.MoveNext(); i++)
                    window[i] = iter.Current;

                if (i < size)
                    yield break;

                while (iter.MoveNext())
                {
                    // generate the next window by shifting forward by one item
                    // and do that before exposing the data
                    var newWindow = new TSource[size];
                    Array.Copy(window, 1, newWindow, 0, size - 1);
                    newWindow[size - 1] = iter.Current;

                    yield return window;
                    window = newWindow;
                }

                // return the last window.
                yield return window;
            }
        }

        public static (string, string) SplitInTwo(this string @string, char separator)
        {
            return @string.Split(separator, StringSplitOptions.RemoveEmptyEntries) switch
            {
                [var v, var i] => (v, i),
                _ => default
            };
        }
    }
}
