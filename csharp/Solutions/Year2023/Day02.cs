using MoreLinq;

namespace AdventOfCode.Solutions.Year2023;

using System.Linq;

internal class Day02 : ASolution
{
    private IReadOnlyList<Game> _games;

    public Day02() : base(02, 2023, "Cube Conundrum", false)
    {

    }

    protected override object SolvePartOne()
    {
        _games = InputByNewLine
            .Select(l => new Game(l))
            .ToList();

        return _games.Where(g => g.Pulls.All(p =>
        {
            return p.Red <= 12 && p.Green <= 13 && p.Blue <= 14;
        }))
            .Sum(g => g.Id);

    }

    protected override object SolvePartTwo()
    {
        return _games.Sum(g =>
        {
            return g.Pulls.Max(p => p.Red)
                * g.Pulls.Max(p => p.Green)
                * g.Pulls.Max(p => p.Blue);
        });
    }

    protected override string LoadDebugInput()
    {
        return """
        Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
        Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
        Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
        Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
        """;
    }

    private class Game
    {
        public int Id { get; private set; }

        public IReadOnlyList<Pull> Pulls { get; private set; }

        public Game(string line)
        {
            Parse(line);
        }

        private void Parse(string line)
        {
            var lineSplit = line.Split(':');
            var game = lineSplit[0];

            Id = int.Parse(game[5..]);
            Pulls = lineSplit[1].Split(';')
                .Select(s => new Pull(s))
                .ToList();
        }
    }

    private class Pull
    {
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public Pull(string s)
        {
            s.Split(',')
                .Select(s => Regex.Match(s, @"(\d+) (\w+)"))
                .Select(m => (amount: int.Parse(m.Groups[1].Value), color: m.Groups[2].Value))
                .ForEach(t =>
                {
                    if (t.color == "red")
                    {
                        Red = t.amount;
                    }
                    else if (t.color == "green")
                    {
                        Green = t.amount;
                    }
                    else if (t.color == "blue")
                    {
                        Blue = t.amount;
                    }
                });
        }
    }
}
