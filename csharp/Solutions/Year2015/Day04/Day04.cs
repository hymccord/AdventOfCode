using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2015
{
    class Day04 : ASolution
    {
        private string s;
        public Day04() : base(4, 2015, "")
        {
            s = Input.SplitByNewline().First();
        }

        protected override object SolvePartOne()
        {
            long seed = 0;
            var s = Stopwatch.StartNew();
            //Compute(new ParallelOptions(),
            //s => s.StartsWith("00000"),
            //AdventCoinString,
            //x => seed = x);
            //Console.WriteLine(seed);
            //Console.WriteLine(s.ElapsedMilliseconds);
            //seed = 0;
            //s.Restart();
            Compute(new ParallelOptions(),
            b => b[0] == 0 && b[1] == 0 && b[2] >> 4 == 0,
            AdventCoin,
            x => seed = x);
            //Console.WriteLine(seed);
            Console.WriteLine(s.ElapsedMilliseconds);
            return seed.ToString();
        }

        protected override object SolvePartTwo()
        {
            long seed = 0;
            var s = Stopwatch.StartNew();
            //Compute(new ParallelOptions(),
            //s => s.StartsWith("000000"),
            //AdventCoinString,
            //x => seed = x);
            //Console.WriteLine(seed);
            //Console.WriteLine(s.ElapsedMilliseconds);
            //seed = 0;
            //s.Restart();
            Compute(new ParallelOptions(),
            b => b[0] == 0 && b[1] == 0 && b[2] == 0,
            AdventCoin,
            x => seed = x);
            //Console.WriteLine(seed);
            Console.WriteLine(s.ElapsedMilliseconds);
            return seed.ToString();
        }

        private byte[] AdventCoin(long counter, ParallelLoopState loopState)
        {
            var sb = new StringBuilder();
            using var md5 = MD5.Create();
            return md5.ComputeHash(Encoding.ASCII.GetBytes($"{s}{counter}"));
        }

        private string AdventCoinString(long counter, ParallelLoopState loopState)
        {
            var sb = new StringBuilder();
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes($"{s}{counter}"));

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static void Compute<T>(
            ParallelOptions parallelOptions, Func<T, bool> condition,
            Func<long, ParallelLoopState, T> body, Action<long> complete)
        {
            Parallel.ForEach(new InfinitePartioner(), parallelOptions,
                (counter, loopState) =>
            {

                T t = body(counter, loopState);
                if (condition(t))
                {
                    complete(counter);
                    loopState.Stop();
                }
            });
        }

        public class InfinitePartioner : Partitioner<long>
        {
            private static volatile int _count = -1;

            public override bool SupportsDynamicPartitions => true;

            public override IList<IEnumerator<long>> GetPartitions(int partitionCount)
            {
                if (partitionCount < 1)
                    throw new ArgumentOutOfRangeException(nameof(partitionCount));
                else
                    return (from i in Enumerable.Range(0, partitionCount)
                            select InfiniteEnumerator()).ToArray();
            }

            public override IEnumerable<long> GetDynamicPartitions()
            {
                return new InfiniteEnumerators();
            }

            private static IEnumerator<long> InfiniteEnumerator()
            {
                while (true)
                {
                    yield return Interlocked.Increment(ref _count);
                }
            }

            private class InfiniteEnumerators : IEnumerable<long>
            {
                public IEnumerator<long> GetEnumerator()
                {
                    return InfiniteEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }
        }
    }
}
