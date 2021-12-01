using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2021;

internal class Day01 : ASolution
{
    public Day01() : base(01, 2021)
    { }

    protected override object SolvePartOne()
    {
    var ints = Input.ToIntArray();
    int c = 0;

    for (int i = 1; i <= ints.Length - 1; i++)
    {
        if (ints[i] > ints[i - 1])
        {
            c++;
        }
    }

    return c;
    }

    protected override object SolvePartTwo()
    {
    var ints = Input.ToIntArray();

    int previousWindow = int.MaxValue;
    int c = 0;

    for (int i = 0; i <= ints.Length - 3; i++)
    {
        int currentWindow = ints[i..(i + 3)].Sum();
        if (currentWindow > previousWindow)
            c++;

        previousWindow = currentWindow;
    }

    return c;
    }

    string test = @"199
200
208
210
200
207
240
269
260
263";
}
