using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2021
{
    internal class Day05 : ASolution
    {
        (Point, Point)[] _ventCoords;
#if PARALLEL
        ConcurrentDictionary<Point, int> _ventPoints;
#else
        Dictionary<Point, int> _ventPoints;
#endif

        public Day05() : base(05, 2021, "Hydrothermal Venture", false)
        {

        }

        protected override void Preprocess()
        {
            _ventCoords = InputByNewLine
                .Select(l => Regex.Match(l, @"(\d+),(\d+) -> (\d+),(\d+)"))
                .Select(m => (
                    new Point(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)),
                    new Point(int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value))
                ))
                .ToArray();
            _ventPoints = new();
        }

        protected override object SolvePartOne()
        {
#if PARALLEL
            Parallel.ForEach(_ventCoords, line =>
#else
            for (int i = 0; i < _ventCoords.Length; i++)
#endif
            {
                Point p, p1, p2;
                p1 = _ventCoords[i].Item1;
                p2 = _ventCoords[i].Item2;

                int changeX = Math.Sign(p2.X - p1.X);
                int changeY = Math.Sign(p2.Y - p1.Y);

                // diag line
                if (changeX != 0 && changeY != 0)
                {
                    continue;
                }
                else if (changeY == 0)
                {
                    for (int x = p1.X; x != (p2.X + changeX); x += changeX)
                    {
                        AddIt(new(x, p1.Y));
                    }
                }
                else if (changeX == 0)
                {
                    for (int y = p1.Y; y != (p2.Y + changeY); y += changeY)
                    {
                        AddIt(new(p1.X, y));
                    }
                }
#if PARALLEL
            });
#else
            }
#endif

            return _ventPoints.Count(kvp => kvp.Value > 1);
        }

        private void AddIt(Point p)
        {
#if PARALLEL
            _ventPoints.AddOrUpdate(p, 1, (p, i) => i + 1);
#else
            _ventPoints[p] = _ventPoints.GetValueOrDefault(p) + 1;
#endif
        }

        protected override object SolvePartTwo()
        {
#if PARALLEL
            Parallel.ForEach(_ventCoords, line =>
#else
            for (int i = 0; i < _ventCoords.Length; i++)
#endif
            {
                Point p1, p2;
                p1 = _ventCoords[i].Item1;
                p2 = _ventCoords[i].Item2;

                int changeX = Math.Sign(p2.X - p1.X);
                int changeY = Math.Sign(p2.Y - p1.Y);

                // diag line
                if (changeX != 0 && changeY != 0)
                {
                    int x, y;
                    for (x = p1.X, y = p1.Y; x != p2.X + changeX && y != p2.Y + changeY; x += changeX, y += changeY)
                    {

                        Point p = new(x, y);
                        AddIt(p);
                    }
                }
#if PARALLEL
            });
#else
            }
#endif

            return _ventPoints.Count(kvp => kvp.Value > 1);
        }

        protected override string LoadDebugInput()
        {
            return @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";
        }
    }
}
