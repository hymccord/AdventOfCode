
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2018
{
    class Day03 : ASolution
    {
        public Day03() : base(03, 2018, "")
        { }

        protected override object SolvePartOne()
        {
            IEnumerable<Claim> claims = Input.SplitByNewline().Select(s => new Claim(s)).ToList();
            var overwritten = claims.AsParallel().Select(c => c.GenerateIndexes()).SelectMany(i => i).GroupBy(i => i).Count(g => g.Count() > 1);
            return overwritten;
        }

        protected override object SolvePartTwo()
        {
            IEnumerable<Claim> claims = Input.SplitByNewline().Select(s => new Claim(s)).ToList();
            var overwritten = claims.AsParallel().Select(c2 => c2.GenerateIndexes()).SelectMany(i => i).GroupBy(i => i).ToDictionary(g => g.Key, g => g.Count());
            var non = claims.AsParallel().Select(cl => cl.GenerateIndexes()).Where(e => e.All(i => overwritten[i] == 1)).First();
            var c = claims.AsParallel().Where(claim => claim.GenerateIndexes().SequenceEqual(non)).First();

            return c.ID;
        }

        [DebuggerDisplay("{DebuggerDisplay,nq}")]
        private class Claim
        {
            private int _id;
            private int _left;
            private int _top;
            private int _width;
            private int _height;

            public int ID => _id;

            public Claim(string claim)
            {
                Regex regex = new Regex(@"#(?'id'\d+) @ (?'left'\d+),(?'top'\d+): (?'width'\d+)x(?'height'\d+)");
                var match = regex.Match(claim);

                _id = int.Parse(match.Groups["id"].Value);
                _left = int.Parse(match.Groups["left"].Value);
                _top = int.Parse(match.Groups["top"].Value);
                _width = int.Parse(match.Groups["width"].Value);
                _height = int.Parse(match.Groups["height"].Value);
            }

            public IEnumerable<int> GenerateIndexes()
            {
                int startID = _top * 1000 + _left;
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                    {
                        yield return startID + i * 1000 + j;
                    }
                }

                yield break;
            }

            private string DebuggerDisplay => $@"ID: {_id,4} {_left,3},{_top,3}: {_width}x{_height}";
        }
    }
}
