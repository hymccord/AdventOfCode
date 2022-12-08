namespace AdventOfCode.Solutions.Year2022;

internal class Day08 : ASolution
{
    private int[,] _grid;
    private Dictionary<(int, int), bool> _visited = new();

    public Day08() : base(08, 2022, "", false)
    {

    }

    protected override void Preprocess()
    {
        _grid = Input.To2DIntArray();
    }

    protected override object SolvePartOne()
    {
        // [ROW, COL]

        int sum = 0;

        for (int col = 0; col < _grid.ColLength(); col++)
        {
            int[] colNums = _grid.SliceColumn(col).ToArray();
            for (int row = 0; row < _grid.RowLength(); row++)
            {
                int tree = _grid[row, col];
                int[] rowNums = _grid.SliceRow(row).ToArray();

                bool top = colNums[0..row].Reverse().All(i => i < tree);
                bool bot = colNums[(row + 1)..].All(i => i < tree);
                bool left = rowNums[0..col].Reverse().All(i => i < tree);
                bool right = rowNums[(col + 1)..].All(i => i < tree);

                if (top || bot || left || right)
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    protected override object SolvePartTwo()
    {
        int max = -1;

        for (int row = 0; row < _grid.RowLength(); row++)
        {
            int[] rowNums = _grid.SliceRow(row).ToArray();
            for (int col = 0; col < _grid.ColLength(); col++)
            {
                int tree = _grid[row, col];
                int[] colNums = _grid.SliceColumn(col).ToArray();

                int top = CountTreeUntil(tree, colNums[0..row].Reverse());
                int bot = CountTreeUntil(tree, colNums[(row + 1)..]);
                int left = CountTreeUntil(tree, rowNums[0..col].Reverse());
                int right = CountTreeUntil(tree, rowNums[(col + 1)..]);

                max = Math.Max(max, top * bot * left * right);
            }
        }

        int CountTreeUntil(int tree, IEnumerable<int> ints)
        {
            int count = 0;
            foreach (var item in ints)
            {
                count++;
                if (item >= tree)
                    break;
            }

            return count;
        }

        return max;
    }

    protected override string LoadDebugInput()
    {
        return """
        30373
        25512
        65332
        33549
        35390
        """;
    }
}
