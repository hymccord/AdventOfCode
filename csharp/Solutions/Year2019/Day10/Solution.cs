using System.Diagnostics;

namespace AdventOfCode.Solutions.Year2019
{

    class Day10 : ASolution
    {
        string test = @".#..#
.....
#####
....#
...##
";

        HashSet<Point> _asteroids = new HashSet<Point>();
        int _rows = 0, _cols = 0;
        private (Point location, int lineOfSightCount) _thePoint;

        public Day10() : base(10, 2019, "")
        {
        }

        protected override object SolvePartOne()
        {
            ParseAsteroids(Input);
            _thePoint = _asteroids.Select(FigureLineOfSight).OrderByDescending(t => t.count).First();

            return _thePoint.lineOfSightCount.ToString();
        }

        private (Point point, int count) FigureLineOfSight(Point point)
        {
            var i = _asteroids
                .Except(new Point[] { point })
                .Select(p => p - point)
                .Select(p => Math.Atan2(p.Y, p.X))
                .Distinct().Count();

            return (point, i);
        }

        protected override object SolvePartTwo()
        {
            Point station = _thePoint.location;
            var a = _asteroids.Except(new Point[] { station })
                .Select(p => new AsteroidDistance(station, p))
                .OrderBy(a => a.Angle)
                .ThenBy(a => a.Manhatten)
                .GroupBy(a => a.Angle)
                .Select(g => new Queue<AsteroidDistance>(g));

            var qqs = new Queue<Queue<AsteroidDistance>>(a);


            int i = 0;
            AsteroidDistance cur = null;
            while (qqs.Count() > 0)
            {
                var angleGroup = qqs.Dequeue();
                cur = angleGroup.Dequeue();
                if (++i == 200)
                    break;

                if (angleGroup.Count > 0)
                    qqs.Enqueue(angleGroup);
            }

            return $"{cur.Asteroid.X * 100 + cur.Asteroid.Y}";
        }

        private void ParseAsteroids(string input)
        {
            _asteroids.Clear();
            var rows = input.SplitByNewline();
            _rows = rows.Length;
            _cols = rows[0].Length;
            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    if (rows[y][x] != '.')
                        _asteroids.Add(new Point(x, y));
                }
            }
        }

        [DebuggerDisplay("({Asteroid}), {ClockWiseQuandrant}, {Angle}, {Manhatten}")]
        class AsteroidDistance
        {
            public Point Asteroid { get; }
            public int Manhatten { get; }
            public double Angle { get; }

            public AsteroidDistance(Point station, Point asteroid)
            {
                Manhatten = station.Manhatten(asteroid);
                // Convert angle from anti-cw from positive X axis to cw from positive Y axis
                // Y's a swapped because going down is positive for grid but negative for Trig
                Angle = (90 - (Math.Atan2(station.Y - asteroid.Y, asteroid.X - station.X) * 180 / Math.PI) + 720) % 360;
                Asteroid = asteroid;
            }
        }

        private string test2 = @"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####";
        private string test3 = @"#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.";
        private string test4 = @".#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..";
        private string test5 = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";
    }
}
