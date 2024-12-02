
using System.Collections.ObjectModel;

namespace AdventOfCode.Solutions.Year2023;

internal class Day17 : ASolution
{
    private int[,] _grid;

    public Day17() : base(17, 2023, "Clumsy Crucible", true)
    {
    }

    protected override object SolvePartOne()
    {
        _grid = Input.To2DIntArray();

        var wg = new ClumsyWeightedGrid(_grid);

        AStar<int> star = new AStar<int>(wg);

        //var p = star.A_Star((0, 0), (wg.Width - 1, wg.Height - 1), (goal, next) => goal.Manhatten(next)).ToHashSet();

        var p = star.A_Star((0, 12), (11, 1), (goal, next) => goal.Manhatten(next)).ToHashSet();

        WriteConsole(wg.Height, wg.Width, 15, 5, (y, x) =>
        {
            var color = ConsoleColor.White;

            if (p.Contains((x, y)))
            {
                color = ConsoleColor.Green;
            }

            return (color, _grid[y, x].ToString()[0]);
        });

        return p.Sum(p => _grid[p.Y, p.X]);
    }

    protected override object SolvePartTwo()
    {
        return null;
    }

    protected override string LoadDebugInput()
    {
        return """
            14999
            23111
            99991
            """;
        return """
            2413432311323
            3215453535623
            3255245654254
            3446585845452
            4546657867536
            1438598798454
            4457876987766
            3637877979653
            4654967986887
            4564679986453
            1224686865563
            2546548887735
            4322674655533
            """;
    }

    private class ClumsyWeightedGrid : WeightedGrid<int>
    {
        public ClumsyWeightedGrid(int[,] grid) : base(grid)
        {
        }

        internal override double Cost(Point from, Point to)
        {
            var clumsy = 0;
            //var totalPath = new List<Point> { from };
            //while (_cameFrom.ContainsKey(from) && totalPath.Count < 4)
            //{
            //    from = _cameFrom[from];
            //    totalPath.Insert(0, from);
            //}

            //var diffX = Math.Abs(to.X - totalPath[0].X);
            //var diffY = Math.Abs(to.Y - totalPath[0].Y);
            //if ((diffX, diffY) == (0, 4) || (diffX, diffY) == (4, 0))
            //{
            //    clumsy += 10000;
            //}

            return _grid[to.Y, to.X] * _grid[to.Y, to.X] +  clumsy;
        }
    }
}
