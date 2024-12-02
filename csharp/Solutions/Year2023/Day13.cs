
using MoreLinq;

namespace AdventOfCode.Solutions.Year2023;

internal class Day13 : ASolution
{
    Dictionary<int, (int vert, int horiz)> _part1 = new();

    public Day13() : base(13, 2023, "Point of Incidence", false)
    {

    }

    protected override object SolvePartOne()
    {
        List<char[,]> grids = Input.SplitByBlankLine()
            .Select(l => l.To2DCharArray()).ToList();

        var total = 0L;
        for (var i = 0; i < grids.Count; i++)
        {
            var grid = grids[i];
            var rowLen = grid.RowLength();
            var colLen = grid.ColLength();
            HashSet<int> verticalCandidates = Enumerable.Range(1, colLen - 1).ToHashSet();
            HashSet<int> horizontalCandidates = Enumerable.Range(1, rowLen - 1).ToHashSet();

            for (int row = 0; row < rowLen; row++)
            {
                var theRow = grid.SliceRow(row).ToArray();
                for (int col = 1; col < colLen; col++)
                {
                    if (!verticalCandidates.Contains(col))
                    {
                        continue;
                    }

                    var one = theRow[..col];
                    var two = theRow[col..];

                    if (!(one.Reverse().StartsWith(two) || two.StartsWith(one.Reverse())))
                    {
                        // b/c numbers starts at 1 in examples
                        verticalCandidates.Remove(col);
                    }
                }
            }

            for (int col = 0; col < colLen; col++)
            {
                var theCol = grid.SliceColumn(col).ToArray();
                for (int row = 1; row < rowLen; row++)
                {
                    if (!horizontalCandidates.Contains(row))
                    {
                        continue;
                    }

                    var one = theCol[..row];
                    var two = theCol[row..];

                    if (!(one.Reverse().StartsWith(two) || two.StartsWith(one.Reverse())))
                    {
                        // b/c numbers starts at 1 in examples
                        horizontalCandidates.Remove(row);
                    }
                }
            }

            if ((horizontalCandidates.Count + verticalCandidates.Count) > 1)
            {
                throw new InvalidOperationException();
            }

            _part1[i] = (verticalCandidates.SingleOrDefault(), horizontalCandidates.SingleOrDefault());

            total += verticalCandidates.SingleOrDefault();
            total += horizontalCandidates.SingleOrDefault() * 100;

        }

        return total;
    }

    protected override object SolvePartTwo()
    {
        List<char[,]> grids = Input.SplitByBlankLine()
            .Select(l => l.To2DCharArray()).ToList();

        var total = 0L;
        for (var i = 0; i < grids.Count; i++)
        {
            var grid = grids[i];
            var rowLen = grid.RowLength();
            var colLen = grid.ColLength();
            total += Something(i, grid, rowLen, colLen);
        }

        return total;
    }

    private long Something(int i, char[,] grid, int rowLen, int colLen)
    {
        for (int y = 0; y < rowLen; y++)
        {
            for (int x = 0; x < colLen; x++)
            {
                grid[y, x] = grid[y, x] == '.' ? '#' : '.';

                HashSet<int> verticalCandidates = Enumerable.Range(1, colLen - 1).ToHashSet();
                HashSet<int> horizontalCandidates = Enumerable.Range(1, rowLen - 1).ToHashSet();

                verticalCandidates.Remove(_part1[i].vert);
                horizontalCandidates.Remove(_part1[i].horiz);

                for (int row = 0; row < rowLen; row++)
                {
                    var theRow = grid.SliceRow(row).ToArray();
                    for (int col = 1; col < colLen; col++)
                    {
                        if (!verticalCandidates.Contains(col))
                        {
                            continue;
                        }

                        var one = theRow[..col];
                        var two = theRow[col..];

                        if (!(one.Reverse().StartsWith(two) || two.StartsWith(one.Reverse())))
                        {
                            // b/c numbers starts at 1 in examples
                            verticalCandidates.Remove(col);
                        }
                    }
                }

                for (int col = 0; col < colLen; col++)
                {
                    var theCol = grid.SliceColumn(col).ToArray();
                    for (int row = 1; row < rowLen; row++)
                    {
                        if (!horizontalCandidates.Contains(row))
                        {
                            continue;
                        }

                        var one = theCol[..row];
                        var two = theCol[row..];

                        if (!(one.Reverse().StartsWith(two) || two.StartsWith(one.Reverse())))
                        {
                            // b/c numbers starts at 1 in examples
                            horizontalCandidates.Remove(row);
                        }
                    }
                }

                if ((horizontalCandidates.Count + verticalCandidates.Count) > 1)
                {
                    throw new InvalidOperationException();
                }
                else if ((horizontalCandidates.Count + verticalCandidates.Count) == 1)
                {
                    WriteCharGrid(grid);

                    return verticalCandidates.SingleOrDefault() + horizontalCandidates.SingleOrDefault() * 100;

                }

                grid[y, x] = grid[y, x] == '.' ? '#' : '.';
            }
        }

        throw new NotImplementedException();
    }

    protected override string LoadDebugInput()
    {
        return """
            #.##..##.
            ..#.##.#.
            ##......#
            ##......#
            ..#.##.#.
            ..##..##.
            #.#.##.#.

            #...##..#
            #....#..#
            ..##..###
            #####.##.
            #####.##.
            ..##..###
            #....#..#

            .#.##.#.#
            .##..##..
            .#.##.#..
            #......##
            #......##
            .#.##.#..
            .##..##.#

            #..#....#
            ###..##..
            .##.#####
            .##.#####
            ###..##..
            #..#....#
            #..##...#

            #.##..##.
            ..#.##.#.
            ##..#...#
            ##...#..#
            ..#.##.#.
            ..##..##.
            #.#.##.#.
            """;
        return """
        #.##..##.
        ..#.##.#.
        ##......#
        ##......#
        ..#.##.#.
        ..##..##.
        #.#.##.#.

        #...##..#
        #....#..#
        ..##..###
        #####.##.
        #####.##.
        ..##..###
        #....#..#       
        """;
    }
}
