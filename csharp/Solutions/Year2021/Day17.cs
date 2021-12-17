using System.Numerics;

namespace AdventOfCode.Solutions.Year2021
{
    internal class Day17 : ASolution
    {
        long _x1, _x2;
        long _y1, _y2;

        long maxyVel;

        public Day17() : base(17, 2021, "Trick Shot", false)
        {

        }

        protected override void Preprocess()
        {
            // target area: x=269..292, y=-68..-44";
            var m = Regex.Match(Input, @"x=(-?\d+)\.\.(-?\d+), y=(-?\d+)\.\.(-?\d+)");
            _x1 = long.Parse(m.Groups[1].Value);
            _x2 = long.Parse(m.Groups[2].Value);
            _y1 = long.Parse(m.Groups[3].Value);
            _y2 = long.Parse(m.Groups[4].Value);
        }

        protected override object SolvePartOne()
        {
            // A projectile thrown up at speed X will be at speed -X when on the step it returns to y=0
            // After the velocity increase (-X - 1), it will reach the bottom of the y grid.
            maxyVel = Math.Abs(_y1) - 1;

            return (maxyVel * (maxyVel + 1)) / 2;
        }

        protected override object SolvePartTwo()
        {
            List<long> validYs = new List<long>();
            long maxV = long.Parse(Part1);
            for (long v = _y1; v <= maxyVel; v++)
            {
                long max = 0;
                long vNow = -Math.Abs(v) - 1; // Y velocity is -V at 0, increase by -1 for first calc
                while (true)
                {
                    max += vNow--;
                    if (max < _y1)
                    {
                        break;
                    }
                    if (max <= _y2)
                    {
                        validYs.Add(v);
                        break;
                    }
                }
            }

            long maxXVel = _x2;
            HashSet<Point> points = new HashSet<Point>();

            for (int y = (int)_y1; y < Math.Abs(_y1); y++)
            {
                for (int x = 0; x <= _x2; x++)
                {
                    if (WillTrajectoryHitGoal(new Vector2(x, y)))
                    {
                        points.Add(new Point(x, (int)y));
                    }
                }
            }

            points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToHashSet();

            return points.Count;
        }

        bool WillTrajectoryHitGoal(Vector2 velocity)
        {
            Vector2 position = new Vector2(0, 0);
            while (true)
            {
                position += velocity;
                if (velocity.X > 0)
                {
                    velocity.X -= 1;
                }
                velocity.Y -= 1;

                if (position.X > _x2 || position.Y < _y1)
                {
                    return false;
                }

                if (position.X >= _x1 && position.Y <= _y2)
                {
                    return true;
                }
            }
        }

        protected override string LoadDebugInput()
        {
            return @"target area: x=20..30, y=-10..-5";
        }
    }
}
