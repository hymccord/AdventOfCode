using System.Reflection.Metadata.Ecma335;
using System.Text;

using MoreLinq;

namespace AdventOfCode.Solutions.Year2024;

internal class Day09 : ASolution
{
    public Day09() : base(09, 2024, "Disk Fragmenter", true)
    { }

    protected override object SolvePartOne()
    {

        int[] diskMaps = ToDiskMap(Input);

        string fragment = Fragment(diskMaps);
        string reflect = Reflect(fragment);

        return Checksum(reflect);
    }

    protected override object SolvePartTwo()
    {
        return null;
    }

    private static int[] ToDiskMap(string input)
    {
        return [
            ..input.Select(c => c - '0')
            ];
    }

    private static string Fragment(int[] diskMap)
    {
        string frag = "";
        for (int i = 0; i < diskMap.Length; i++)
        {
            if (i % 2 == 0)
            {
                frag += string.Concat(Enumerable.Repeat($"{i >> 1}", diskMap[i]));
            }
            else
            {
                frag += new string('.', diskMap[i]);
            }
        }

        return frag;
    }

    private static string Reflect(string input)
    {
        char[] fragment = input.ToArray();
        int head = 0;
        int tail = fragment.Length - 1;

        while (head < tail)
        {
            while (fragment[head] != '.')
            {
                head++;
            }

            while (fragment[tail] == '.')
            {
                tail--;
            }

            if (head >= tail)
            {
                break;
            }

            fragment[head] = fragment[tail];
            fragment[tail] = '.';
        }

        return new string(fragment)[0..head];
    }

    private static long Checksum(string input)
    {
        long sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            sum += i * (input[i] - '0');
        }

        return sum;
    }

    protected override IEnumerable<ExampleInput> LoadExampleInput()
    {
        return [
            new ExampleInput("2333133121414131402", 1928)
            ];
    }
}
