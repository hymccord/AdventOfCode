
namespace AdventOfCode.Solutions.Year2024;

internal class Day04 : ASolution
{
    private char[,] _grid;
    private HashSet<Point> _xLocations;
    private HashSet<Point> _mLocs;
    private HashSet<Point> _aLocs;
    private HashSet<Point> _sLocs;

    public Day04() : base(04, 2024, "Ceres Search", true)
    { }

    protected override void Preprocess()
    {
        _grid = Input.To2DCharArray().PadGrid('.', 1);
        _xLocations = _grid.GetPointHashset('X');
        _mLocs = _grid.GetPointHashset('M');
        _aLocs = _grid.GetPointHashset('A');
        _sLocs = _grid.GetPointHashset('S');
    }

    protected override object SolvePartOne()
    {
        int count = 0;
        foreach (Point p in _xLocations)
        {
            foreach (Point dir in Point.Splatt)
            {
                if (_mLocs.Contains(p + dir)
                    && _aLocs.Contains(p + (2 * dir))
                    && _sLocs.Contains(p + (3 * dir))
                    )
                {
                    count++;
                }
            }
        }

        return count;
    }

    protected override object SolvePartTwo()
    {
        // M . M
        // . A .
        // S . S
        
        // valid X-MAX should have S and M at diagonals
        const int diagSum = 'S' + 'M';
        int count = 0;
        foreach (Point p in _aLocs)
        {
            char nw = _grid.At(p + Point.NorthWest);
            char ne = _grid.At(p + Point.NorthEast);
            char sw = _grid.At(p + Point.SouthWest);
            char se = _grid.At(p + Point.SouthEast);

            count += ne + sw == diagSum && nw + se == diagSum ? 1 : 0;
        }

        return count;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ExampleInput("""
                MMMSXXMASM
                MSAMXMSMSA
                AMXSXMAAMM
                MSAMASMSMX
                XMASAMXAMM
                XXAMMXXAMA
                SMSMSASXSS
                SAXAMASAAA
                MAMMMXMMMM
                MXMXAXMASX
                """, partOne: 18, partTwo: 9),
            ];
    }
}
