using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2025;

internal class Day01 : ASolution
{

    public Day01() : base(1, 2025, "Secret Entrance", true)
    {

    }

    protected override object SolvePartOne()
    {
        int dial = 50;
        int crossings = 0;

        foreach (var line in InputByNewLine)
        {
            char c = line[0];
            int value = int.Parse(line[1..]);

            dial += c == 'L' ? -value : value;
            dial = ((dial % 100) + 100) % 100;

            if (dial == 0) crossings++;
        }

        return crossings;
    }

    protected override object SolvePartTwo()
    {
        int dial = 50;
        int crossings = 0;

        foreach (var line in InputByNewLine)
        {
            char c = line[0];
            int value = int.Parse(line[1..]);

            int diff = c == 'L' ? -1 : 1;

            for (int i = 0; i < value; i++)
            {
                dial += diff;
                dial = ((dial % 100) + 100) % 100;
                if (dial == 0) crossings++;
            }

        }

        return crossings;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ("""
                L68
                L30
                R48
                L5
                R60
                L55
                L1
                L99
                R14
                L82
                """, partOne: 3, partTwo: 6)
            ];
    }
}
