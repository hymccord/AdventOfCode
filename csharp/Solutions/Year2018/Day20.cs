namespace AdventOfCode.Solutions.Year2018
{
    internal class Day20 : ASolution
    {
        public Day20() : base(20, 2018)
        {

        }

        private Dictionary<char, Point> directions = new Dictionary<char, Point>
        {
            { 'N', Point.North },
            { 'E', Point.East },
            { 'S', Point.South },
            { 'W', Point.West },
        };
        Dictionary<Point, int> grid = new Dictionary<Point, int>();

        protected override object SolvePartOne()
        {
            Stack<(int, Point)> stack = new Stack<(int, Point)>();
            var sr = new StringReader(Input);
            sr.Read();
            char c;
            int dist = 0;
            Point pos = new Point();
            while (sr.Peek() != -1)
            {
                switch (c = (char)sr.Read())
                {
                    case 'N':
                    case 'S':
                    case 'E':
                    case 'W':
                        pos += directions[c];
                        dist++;
                        if (!grid.ContainsKey(pos) || dist < grid[pos])
                            grid[pos] = dist;
                        break;
                    case '(':
                        stack.Push((dist, pos));
                        break;
                    case ')':
                        (dist, pos) = stack.Pop();
                        break;
                    case '|':
                        (dist, pos) = stack.Peek();
                        break;
                    case '$':

                        break;
                }
            }

            return grid.Max(kvp => kvp.Value);
        }

        protected override object SolvePartTwo()
        {
            return grid.Count(kvp => kvp.Value > 999);
        }

        protected override string LoadDebugInput()
        {
            return
            @"^WNE$";
            //@"^ENWWW(NEEE|SSE(EE|N))$";
            //@"^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";
            //@"^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$";
            //@"^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$";
        }

    }
}
