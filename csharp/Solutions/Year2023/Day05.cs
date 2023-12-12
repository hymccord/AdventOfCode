using System.Collections.Immutable;

using MoreLinq;

namespace AdventOfCode.Solutions.Year2023;

internal class Day05 : ASolution
{
    private List<long> _seeds;
    List<List<long[]>> _blocks = new();

    public Day05() : base(05, 2023, "If You Give A Seed A Fertilizer", false)
    {
    }

    protected override object SolvePartOne()
    {
        ParseInput();

        foreach (var block in _blocks)
        {
            var @new = new List<long>();
            foreach (var seed in _seeds)
            {
                bool remapped = false;
                foreach (var range in block)
                {
                    (var a, var b, var c) = (range[0], range[1], range[2]);
                    if ((seed >= b) && (seed < b + c))
                    {
                        @new.Add(seed - b + a);
                        remapped = true;
                    }
                }

                if (!remapped)
                {
                    @new.Add(seed);
                }
            }
            _seeds = new (@new);
        }

        return _seeds.Min();
    }

    protected override object SolvePartTwo()
    {
        var inputs = InputByNewLine[0].SplitInTwo(':').Item2.ToLongArray();

        Queue<(long Start, long End)> seeds = new();
        for (int i = 0; i < inputs.Length; i += 2)
        {
            seeds.Enqueue((inputs[i], inputs[i] + inputs[i + 1]));
        }

        var blocks = Input.SplitByBlankLine()[1..];

        foreach (var block in _blocks)
        {

            var @new = new Queue<(long Start, long End)>();
            while (seeds.Count > 0)
            {
                var seed = seeds.Dequeue();
                (var s, var e) = (seed.Start, seed.End);
                bool reMapped = false;

                foreach (var range in block)
                {

                    (var a, var b, var c) = (range[0], range[1], range[2]);

                    var os = Math.Max(s, b);
                    var oe = Math.Min(e, b + c);

                    if (os < oe)
                    {
                        @new.Enqueue((os - b + a, oe - b + a));
                        if (os > s)
                        {
                            seeds.Enqueue((s, os));
                        }
                        if (e > oe)
                        {
                            seeds.Enqueue((oe, e));
                        }
                        reMapped = true;
                        break;
                    }
                }

                if (!reMapped)
                {
                    @new.Enqueue((s, e));
                }
            }

            seeds = @new;
        }

        return seeds.Min(i => i.Start);
    }

    private void ParseInput()
    {
        _seeds = InputByNewLine[0].Split(':')[1].ToLongArray().ToList();

        var stringMaps = Input.SplitByBlankLine()[1..];

        foreach (var map in stringMaps)
        {
            var block = new List<long[]>();
            map.SplitByNewline()[1..]
                .ForEach(l =>
                {
                    block.Add(l.ToLongArray());
                });
            _blocks.Add(block);
        }
    }

    protected override string LoadDebugInput()
    {
        return """
        seeds: 79 14 55 13

        seed-to-soil map:
        50 98 2
        52 50 48

        soil-to-fertilizer map:
        0 15 37
        37 52 2
        39 0 15

        fertilizer-to-water map:
        49 53 8
        0 11 42
        42 0 7
        57 7 4

        water-to-light map:
        88 18 7
        18 25 70

        light-to-temperature map:
        45 77 23
        81 45 19
        68 64 13

        temperature-to-humidity map:
        0 69 1
        1 0 69

        humidity-to-location map:
        60 56 37
        56 93 4
        """;
    }

}
