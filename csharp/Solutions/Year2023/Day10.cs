
namespace AdventOfCode.Solutions.Year2023;

internal class Day10 : ASolution
{
    char[,] _grid;
    Point _start;
    int _rows;
    int _cols;

    List<Point> _path = new();

    public Day10() : base(10, 2023, "Pipe Maze", false)
    {
    }

    protected override object SolvePartOne()
    {
        Initialize();

        HashSet<Point> visited = new();

        int steps = 0;
        Point current = _start;
        Point direction = Point.South; // uhh, just hardcode the start direction
        do
        {
            visited.Add(current);
            _path.Add(current);

            current += direction;
            direction = _grid[current.Y, current.X] switch
            {
                '.' => throw new InvalidOperationException(),
                '|' => direction,
                '-' => direction,
                'L' when direction == Point.South => Point.East,
                'L' when direction == Point.West => Point.North,
                'J' when direction == Point.South => Point.West,
                'J' when direction == Point.East => Point.North,
                '7' when direction == Point.North => Point.West,
                '7' when direction == Point.East => Point.South,
                'F' when direction == Point.North => Point.East,
                'F' when direction == Point.West => Point.South,
                'S' => default,
                _ => throw new NotImplementedException(),
            };

            steps++;
        } while (current != _start);

        // ⚠ Doesn't work with Windows Terminal...

        //Console.SetBufferSize(Console.WindowWidth + 100,300);
        //WriteConsole(_rows, _cols, 20, 20, (row, col) =>
        //{
        //    ConsoleColor color = ConsoleColor.White;
        //    char c = _grid[row, col];
        //    //c = c switch
        //    //{
        //    //    'L' => '└',
        //    //    'J' => '┘',
        //    //    '7' => '┐',
        //    //    'F' => '┌',
        //    //    _ => c
        //    //};
        //    if (visited.Contains((col, row)))
        //    {
        //        color = ConsoleColor.Red;
        //    }

        //    return (color, c);
        //});

        return steps / 2;
    }

    protected override object SolvePartTwo()
    {
        // Pick's Theorem
        // A = i + b/2 - 1
        // where A = area, i = num interior points, b = boundary points
        
        var A = Algorithms.PolygonArea(_path);

        // Solve for i
        // i = A - b/2 + 1;
        return (int)A - (_path.Count / 2) + 1;
    }

    private void Initialize()
    {
        _grid = InputByNewLine
            .Select(l => l.ToCharArray())
            .ToArray()
            .To2D()
            .PadGrid('.');

        _rows = _grid.RowLength();
        _cols = _grid.ColLength();

        for (var row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                if (_grid[row, col] == 'S')
                {
                    _start = new Point(col, row);
                    break;
                }
            }

            if (_start != default)
            {
                break;
            }
        }
    }

    protected override string LoadDebugInput()
    {
        //return """
        //    ...........
        //    .S-------7.
        //    .|F-----7|.
        //    .||.....||.
        //    .||.....||.
        //    .|L-7.F-J|.
        //    .|..|.|..|.
        //    .L--J.L--J.
        //    ...........
        //    """;
        //return """
        //    .F----7F7F7F7F-7....
        //    .|F--7||||||||FJ....
        //    .||.FJ||||||||L7....
        //    FJL7L7LJLJ||LJ.L-7..
        //    L--J.L7...LJS7F-7L7.
        //    ....F-J..F7FJ|L7L7L7
        //    ....L7.F7||L7|.L7L7|
        //    .....|FJLJ|FJ|F7|.LJ
        //    ....FJL-7.||.||||...
        //    ....L---J.LJ.LJLJ...
        //    """;
        return """
            FF7FSF7F7F7F7F7F---7
            L|LJ||||||||||||F--J
            FL-7LJLJ||||||LJL-77
            F--JF--7||LJLJ7F7FJ-
            L---JF-JLJ.||-FJLJJ7
            |F|F-JF---7F7-L7L|7|
            |FFJF7L7F-JF7|JL---7
            7-L-JL7||F7|L7F-7F7|
            L.L7LFJ|||||FJL7||LJ
            L7JLJL-JLJLJL--JLJ.L
            """;
    }
}
