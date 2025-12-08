using Microsoft.Extensions.DependencyInjection;

using MoreLinq;

namespace AdventOfCode.Solutions.Year2025;

internal class Day06 : ASolution
{
    private string[] _operations;

    public Day06() : base(06, 2025, "Trash Compactor", true)
    { }

    protected override void Preprocess()
    {
        _operations = InputByNewLine[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
    }

    protected override object SolvePartOne()
    {
        var worksheet = InputByNewLine[..^1].Select(a => a.ToLongArray()).ToArray().To2D();
        var numProblems = worksheet.ColCount;

        var sum = 0L;
        for (int i = 0; i < numProblems; i++)
        {
            Func<long, long, long> f = _operations[i] switch
            {
                "*" => (long x, long y) => x * y,
                "+" => (long x, long y) => x + y,
                _ => throw new NotImplementedException()
            };

            sum += worksheet.SliceColumn(i).Aggregate(f);
        }

        return sum;
    }

    protected override object SolvePartTwo()
    {
        // The last line of Input doesnt contain the trailing spaces after the operator
        // append enough spaces so extension method wont throw
        var grid = (Input + "   ").To2DCharArray();
        var sum = 0L;

        List<long> currentProblem = [];
        for (int y = grid.ColCount - 1; y >= 0; y--)
        {
            bool accumulating = true;
            long currentNumber = 0L;
            for (int x = 0; x < grid.RowCount; x++)
            {
                char c = grid[x, y];
                if ( c == ' ' && !accumulating)
                {
                    break;
                }

                if (c == '*' || c == '+')
                {
                    accumulating = false;
                }

                if ( c != ' ' && accumulating)
                {
                    currentNumber = currentNumber * 10 + (c - '0');                
                }
            }

            if (currentNumber > 0)
            {
                currentProblem.Add(currentNumber);
            }

            if (!accumulating)
            {
                Func<long, long, long> f = grid[grid.RowCount - 1, y] switch
                {
                    '*' => (long x, long y) => x * y,
                    '+' => (long x, long y) => x + y,
                    _ => throw new NotImplementedException()
                };

                sum += currentProblem.Aggregate(f);
                currentProblem.Clear();
            }
        }

        return sum;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                123 328  51 64 
                 45 64  387 23 
                  6 98  215 314
                *   +   *   +
                """,
                partOne: 4277556, partTwo: 3263827)
            ];
    }
}
