namespace AdventOfCode.Solutions.Year2022;

internal class Day03 : ASolution
{
    public Day03() : base(03, 2022, "Rucksack Reorganization", false)
    { }

    protected override object SolvePartOne()
    {
        return InputByNewLine.Sum(line =>
        {
            char c = line[..(line.Length / 2)]
                .Intersect(line[(line.Length / 2)..])
                .Single();

            if (char.IsLower(c))
            {
                return c - 'a' + 1;
            }

            return c - 'A' + 27;
        });
    }

    protected override object SolvePartTwo()
    {
        return InputByNewLine.Chunk(3).Sum(chunk =>
        {
            char c = chunk[0].Intersect(chunk[1]).Intersect(chunk[2]).Single();
            if (char.IsLower(c))
            {
                return c - 'a' + 1;
            }
            
            return c - 'A' + 27;
        });
    }

    protected override string LoadDebugInput()
    {
        return """
        vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw
        """;
    }
}
