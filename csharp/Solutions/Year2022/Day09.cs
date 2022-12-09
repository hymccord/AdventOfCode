using System.Linq.Expressions;
using System.Numerics;

namespace AdventOfCode.Solutions.Year2022;

internal class Day09 : ASolution
{
    private List<(Point, int)> _moves = new();
    public Day09() : base(09, 2022, "Rope Bridge", false)
    { }

    protected override void Preprocess()
    {
        _moves.AddRange(InputByNewLine.Select(s =>
        {
            var ss = s.Split(' ');
            return (DirToCardinal(ss[0]), int.Parse(ss[1]));
        }));
    }

    protected override object SolvePartOne()
    {
        List<Point> rope = new List<Point>(Enumerable.Repeat(new Point(), 2));

        return SimulateRope(rope);
    }

    protected override object SolvePartTwo()
    {
        List<Point> rope = new List<Point>(Enumerable.Repeat(new Point(), 10));

        return SimulateRope(rope);
    }

    private int SimulateRope(List<Point> rope)
    {
        HashSet<Point> visited = new();
        visited.Add(new Point());
        foreach (var move in _moves)
        {
            var direction = move.Item1;
            for (int i = 0; i < move.Item2; i++)
            {
                rope[0] += direction;
                for (int j = 1; j < rope.Count; j++)
                {
                    if (rope[j - 1].Distance(rope[j]) >= 2)
                    {
                        rope[j] = MoveTailCloserToHead(rope[j], rope[j - 1]);
                    }
                }
                visited.Add(rope.Last());
            }
        }

        return visited.Count;
    }

    private static Point MoveTailCloserToHead(Point tail, Point head)
    {
        // If the head is ever two steps directly up, down, left, or right from the tail, the tail must also move one step in that direction
        // Otherwise, if the head and tail aren't touching and aren't in the same row or column, the tail always moves one step diagonally to keep up:

        // if the row or col are the same, then sign will be 0
        // for diagonal moved where the head has moved 2, 1 away. sign will move at most 1, 1
        int xSign = Math.Sign(head.X - tail.X);
        int ySign = Math.Sign(head.Y - tail.Y);

        return tail + new Point(xSign, ySign);
    }

    private static Point DirToCardinal(string d)
    {
        return d switch
        {
            "U" => Point.North,
            "R" => Point.East,
            "D" => Point.South,
            "L" => Point.West,
            _ => throw new NotImplementedException(),
        }; 
    }

    protected override string LoadDebugInput()
    {
        return """
        R 4
        U 4
        L 3
        D 1
        R 4
        D 1
        L 5
        R 2
        """;
        return """
            R 5
            U 8
            L 8
            D 3
            R 17
            D 10
            L 25
            U 20
            """;
    }
}
