#nullable enable

namespace AdventOfCode.Solutions.Year2019
{

    class Day03 : ASolution
    {
        private int _minHatten = int.MaxValue;
        private int _minSteps = int.MaxValue;
        string[] _wire1;
        string[] _wire2;
        Dictionary<Point, int> _points = new Dictionary<Point, int>();
        Dictionary<(int, int), char> _dict = new Dictionary<(int, int), char>();

        //string test = "R8,U5,L5,D3\nU7,R6,D4,L4";
        string test = "L8,D5,R5,U3\nD7,L6,U4,R4";
        string test2 = "R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83";
        string test3 = "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7";

        public Day03() : base(3, 2019, "")
        {
            var wires = test.SplitByNewline(true);
            _wire1 = wires[0].Split(',');
            _wire2 = wires[1].Split(',');
            _points = new Dictionary<Point, int>();
        }

        protected override object SolvePartOne()
        {
            int steps = 0;
            var origin = new Point(0, 0);

            Point dir;
            Point cur = origin;
            foreach (var item in _wire1)
            {
                _dict[(cur.X, cur.Y)] = '+';

                dir = GetDirection(item[0]);
                int max = int.Parse(item.Substring(1));
                for (int i = 0; i < max; i++)
                {
                    cur = cur.Offset(dir);
                    _dict[(cur.X, cur.Y)] = dir.X == 0 ? '|' : '-';
                    _points.TryAdd(cur, ++steps);
                }
            }

            steps = 0;
            cur = origin;
            foreach (var item in _wire2)
            {
                _dict[(cur.X, cur.Y)] = '+';
                dir = GetDirection(item[0]);
                int max = int.Parse(item.Substring(1));
                for (int i = 0; i < max; i++)
                {
                    steps++;
                    cur = cur.Offset(dir);

                    if (_points.ContainsKey(cur))
                    {
                        _dict[(cur.X, cur.Y)] = 'X';
                        _minHatten = Math.Min(_minHatten, origin.Manhatten(cur));
                        _minSteps = Math.Min(_points[cur] + steps, _minSteps);
                    }
                    else
                    {
                        _dict[(cur.X, cur.Y)] = dir.X == 0 ? '|' : '-';
                    }
                }
            }

            int minX = _points.Keys.Min(p => p.X);
            int maxX = _points.Keys.Max(p => p.X);
            int minY = _points.Keys.Min(p => p.Y);
            int maxY = _points.Keys.Max(p => p.Y);
            int width = maxX - minX + 3;
            int height = maxY - minY + 3;

            Console.SetBufferSize(width, height);
            WriteConsole(width, height, 15, 15, (row, col) =>
            {
                col = col - width / 2 + maxX / 2;
                row = row - height / 2 + minY / 2;
                if (col == origin.X && row == origin.Y)
                {
                    return (ConsoleColor.White, 'o');
                }

                if (_dict.ContainsKey((col, row)))
                {
                    return (ConsoleColor.White, _dict[(col, row)]);
                }
                else
                {
                    return (ConsoleColor.White, '.');
                }
            }
            );


            return _minHatten.ToString();
        }

        protected override object SolvePartTwo()
        {
            return _minSteps.ToString();
        }

        Point GetDirection(char dir)
        {
            switch (dir)
            {
                case 'L':
                    return Point.West;
                case 'R':
                    return Point.East;
                case 'U':
                    return Point.North;
                case 'D':
                    return Point.South;
            }

            throw new System.NotImplementedException();
        }
    }
}
