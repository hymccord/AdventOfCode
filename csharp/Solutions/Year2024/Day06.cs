namespace AdventOfCode.Solutions.Year2024;

internal class Day06 : ASolution
{
    Point _guard;
    HashSet<Point> _obstacles;
    int _rows;
    int _cols;

    public Day06() : base(06, 2024, "Guard Gallivant", true)
    { }

    protected override void Preprocess()
    {
        var grid = Input.To2DCharArray();
        _rows = grid.RowLength();
        _cols = grid.ColLength();

        _guard = grid.GetPointHashset('^').Single();
        _obstacles = grid.GetPointHashset('#');
    }

    protected override object SolvePartOne()
    {
        HashSet<Point> visited = new();
        Point g = _guard;

        Point dir = Point.North;
        while (g.X >= 0 && g.Y >= 0 && g.X < _cols && g.Y < _rows)
        {
            if (_obstacles.Contains(g + dir))
            {
                // turn right
                dir = dir with { X = -dir.Y, Y = dir.X };
                continue;
            }

            if (_useExamples)
            {
                Print(visited, _obstacles, guard: g, direction: dir);
                Thread.Sleep(100);
            }

            visited.Add(g);
            g += dir;
        }

        _partOne = visited;
        return visited.Count;
    }

    private HashSet<Point> _partOne;
    protected override object SolvePartTwo()
    {

        Queue<Point> obstacleCandidates = new Queue<Point>(_partOne);

        int success = 0;
        while (obstacleCandidates.TryDequeue(out Point newObstacle))
        {
            HashSet<Point> visited = new();
            HashSet<Point> obstacles = new([.._obstacles, newObstacle]);
            HashSet<(Point, Point)> visitedWithDir = new();

            //Print(visited, obstacles, newObstacle);
            Point g = _guard;
            Point dir = Point.North;
            while (g.X >= 0 && g.Y >= 0 && g.X < _cols && g.Y < _rows && !visitedWithDir.Contains((g, dir)))
            {
                if (obstacles.Contains(g + dir))
                {
                    // turn right
                    dir = dir with { X = -dir.Y, Y = dir.X };
                    continue;
                }

                visited.Add(g);
                visitedWithDir.Add((g, dir));

                g += dir;
                //Print(visited, obstacles, newObstacle, g);
            }

            if (visitedWithDir.Contains((g, dir)))
            {
                success++;
            }
        }

        // 1959 too low
        return success;
    }

    private void Print(HashSet<Point> visited, HashSet<Point> obstacles, Point? newObstacle = null, Point? guard = null, Point? direction = null)
    {
        WriteConsole(_rows, _cols, 15, 15, (y, x) =>
        {
            Point p = (x, y);

            if (newObstacle == p)
            {
                return (ConsoleColor.Red, 'O');
            }
            else if (guard == p)
            {
                if (direction is null)
                {
                    return (ConsoleColor.Yellow, '^');
                }
                else if (direction == Point.North)
                {
                    return (ConsoleColor.Yellow, '^');
                }
                else if (direction == Point.South)
                {
                    return (ConsoleColor.Yellow, 'v');
                }
                else if (direction == Point.West)
                {
                    return (ConsoleColor.Yellow, '<');
                }
                else if (direction == Point.East)
                {
                    return (ConsoleColor.Yellow, '>');
                }
            }
            else if (visited.Contains(p))
            {
                return (ConsoleColor.Green, 'X');
            }
            else if (obstacles.Contains(p))
            {
                return (ConsoleColor.White, '#');
            }
            return (ConsoleColor.White, '.');
        });
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ExampleInput("""
                ....#.....
                .........#
                ..........
                ..#.......
                .......#..
                ..........
                .#..^.....
                ........#.
                #.........
                ......#...
                """, 41, 6)
            ];
    }
}
