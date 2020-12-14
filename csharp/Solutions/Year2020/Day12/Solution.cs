using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020.Day12
{
    class Day12 : ASolution
    {
        private const string Test = @"F10
N3
F7
R90
F11";
        private List<(char, int)> _moves;

        public Day12()
            : base(12, 2020, "Rain Risk")
        {

        }

        protected override object SolvePartOne()
        {
            _moves = Input.SplitByNewline()
                .Select(s => (s[0], int.Parse(s[1..])))
                .ToList();

            Point heading = new Point(1, 0);
            Point location = new Point();

            foreach ((char dir, int value) in _moves)
            {
                double angle;
                int x, y;
                switch (dir)
                {
                    case 'N':
                        location += Point.North * value;
                        break;
                    case 'S':
                        location += Point.South * value;
                        break;
                    case 'E':
                        location += Point.East * value;
                        break;
                    case 'W':
                        location += Point.West * value;
                        break;
                    case 'L':
                    case 'R':
                        angle = value * Math.PI / 180;
                        // CCW? 
                        angle = dir == 'L' ? -angle : angle;
                        // Flip y, b/c up is negative for Point
                        heading.Y = -heading.Y;
                        // Rotate
                        x = (int)(heading.X * Math.Cos(angle) + heading.Y * Math.Sin(angle));
                        y = (int)(heading.X * -Math.Sin(angle) + heading.Y * Math.Cos(angle));
                        // Flip y back
                        heading = new Point(x, -y);
                        break;
                    case 'F':
                        location += heading * value;
                        break;
                    default:
                        break;
                }
            }


            return Math.Abs(location.X) + Math.Abs(location.Y);
        }

        protected override object SolvePartTwo()
        {
            Point location = new Point();
            Point wayPoint = Point.East * 10 + Point.North * 1;

            foreach ((char dir, int value) in _moves)
            {
                switch (dir)
                {
                    case 'N':
                        wayPoint += Point.North * value;
                        break;
                    case 'S':
                        wayPoint += Point.South * value;
                        break;
                    case 'E':
                        wayPoint += Point.East * value;
                        break;
                    case 'W':
                        wayPoint += Point.West * value;
                        break;
                    case 'L':
                        for (int i = 0; i < value / 90; i++)
                        {
                            wayPoint = new Point(wayPoint.Y, -wayPoint.X);
                        }
                        break;
                    case 'R':
                        for (int i = 0; i < value / 90; i++)
                        {
                            wayPoint = new Point(-wayPoint.Y, wayPoint.X);
                        }
                        break;
                    case 'F':
                        location += new Point(wayPoint.X * value, wayPoint.Y * value);
                        break;
                    default:
                        break;
                }
            }

            return Math.Abs(location.X) + Math.Abs(location.Y);
        }
    }
}
