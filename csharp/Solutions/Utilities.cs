/**
 * This utility class is largely based on: 
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */ 

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions {

    public static class Utilities {

        public static int[] ToIntArray(this string str) 
            => str
                .Split(new char[] { ' ', '\n' })
                .Where(n => int.TryParse(n, out int v))
                .Select(n => Convert.ToInt32(n))
                .ToArray(); 

        public static int MinOfMany(params int[] items) {
            var result = items[0];
            for (int i = 1; i < items.Length; i++) {
                result = Math.Min(result, items[i]);
            }
            return result;
        }

        public static int MaxOfMany(params int[] items) {
            var result = items[0];
            for (int i = 1; i < items.Length; i++) {
                result = Math.Max(result, items[i]);
            }
            return result;
        }

        // https://stackoverflow.com/a/3150821/419956 by @RonWarholic
        public static IEnumerable<T> Flatten<T>(this T[,] map) {
            for (int row = 0; row < map.GetLength(0); row++) {
                for (int col = 0; col < map.GetLength(1); col++) {
                    yield return map[row, col];
                }
            }
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items) {
            return string.Join("", items);
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items, string separator)
        {
            return string.Join(separator, items);
        }

        public static string[] SplitByNewline(this string input, bool shouldTrim = false) {
            return input
                .Split(new[] {"\r", "\n", "\r\n"}, StringSplitOptions.None)
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
    }
}