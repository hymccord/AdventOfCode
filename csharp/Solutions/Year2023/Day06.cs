using System.Collections.Immutable;

using MoreLinq;

using static AdventOfCode.Solutions.Year2018.Day22;

namespace AdventOfCode.Solutions.Year2023;

internal class Day06 : ASolution
{
    private int[] _time;
    private int[] _distance;

    public Day06() : base(06, 2023, "Wait For It", false)
    {
    }

    protected override object SolvePartOne()
    {
        ParseInput();

        long total = 1;
        for (int i = 0; i < _time.Length; i++)
        {
            int ways = 0;
            for (var time = 0; time < _time[i]; time++)
            {
                if ((time * (_time[i] - time)) > _distance[i])
                {
                    ways++;
                }
            }

            total *= ways;
        }

        return total;
    }

    protected override object SolvePartTwo()
    {
        var raceTime = long.Parse(InputByNewLine[0].SplitInTwo(':').Item2.Replace(" ", ""));
        var raceDistance = long.Parse(InputByNewLine[1].SplitInTwo(':').Item2.Replace(" ", ""));

        long startTime;
        for (startTime = 0; startTime < raceTime; startTime++)
        {
            if ((startTime * (raceTime - startTime)) > raceDistance)
            {
                break;
            }
        }

        long endTime;
        for (endTime = raceTime; endTime > startTime; endTime--)
        {
            if ((endTime * (raceTime - endTime)) > raceDistance)
            {
                break;
            }
        }

        return endTime - startTime + 1;
    }

    private void ParseInput()
    {
        _time = InputByNewLine[0].SplitInTwo(':').Item2.ToIntArray();
        _distance = InputByNewLine[1].SplitInTwo(':').Item2.ToIntArray();
    }

    protected override string LoadDebugInput()
    {
        return """
        Time:      7  15   30
        Distance:  9  40  200
        """;
    }

}
