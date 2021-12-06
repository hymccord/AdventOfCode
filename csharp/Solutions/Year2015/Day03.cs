namespace AdventOfCode.Solutions.Year2015
{
    class Day03 : ASolution
    {

        public Day03() : base(3, 2015, "")
        {

        }

        protected override object SolvePartOne()
        {
            HashSet<Point> _points = new HashSet<Point>();
            Point current = new Point();
            _points.Add(current);
            foreach (char c in Input)
            {
                Point p = c switch
                {
                    '>' => Point.East,
                    '<' => Point.West,
                    '^' => Point.North,
                    'v' => Point.South,
                    _ => throw new Exception(),
                };
                current = current.Offset(p);
                _points.Add(current);
            }

            return _points.Count.ToString();
        }

        protected override object SolvePartTwo()
        {
            HashSet<Point> _points = new HashSet<Point>();
            Point santa = new Point();
            Point roboSanta = new Point();
            _points.Add(santa);
            _points.Add(roboSanta);
            for (int i = 0; i < Input.Length; i++)
            {
                char c = Input[i];
                Point p = c switch
                {
                    '>' => Point.East,
                    '<' => Point.West,
                    '^' => Point.North,
                    'v' => Point.South,
                    _ => throw new Exception(),
                };
                if (i % 2 == 0)
                {
                    santa = santa.Offset(p);
                    _points.Add(santa);

                }
                else
                {
                    roboSanta = roboSanta.Offset(p);
                    _points.Add(roboSanta);
                }
            }

            return _points.Count.ToString();
        }
    }
}
