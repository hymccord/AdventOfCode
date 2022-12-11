using System.Linq.Expressions;
using System.Numerics;
using System.Text.RegularExpressions.Generated;

namespace AdventOfCode.Solutions.Year2022;

internal class Day11 : ASolution
{
    private List<Monkey> _monkeys;
    private long _factor;

    public Day11() : base(11, 2022, "Monkey in the Middle", false)
    { }

    protected override void Preprocess()
    {
        _monkeys = new();
        _factor = 1;
        foreach (var section in Input.SplitByBlankLine())
        {
            var lines = section.SplitByNewline();
            var monkey = new Monkey();

            monkey.Items = new Queue<long>(Regex.Matches(lines[1], @"\d+").Select(m => long.Parse(m.Value)));
            Match match = Regex.Match(lines[2], "= old (.) (.*)");
            if (long.TryParse(match.Groups[2].Value, out long op))
            {
                monkey.Operation = match.Groups[1].Value switch
                {
                    "*" => (i) => i * op,
                    "+" => (i) => i + op,
                    _ => throw new NotImplementedException()
                };
            }
            else
            {
                monkey.Operation = match.Groups[1].Value switch
                {
                    "*" => (i) => i * i,
                    "+" => (i) => i + i,
                    _ => throw new NotImplementedException()
                };
            }
            long divisor = long.Parse(lines[3].Split("by")[1]);
            monkey.Test = (i) => i % divisor == 0;
            monkey.True = int.Parse(Regex.Match(lines[4], @"\d+").Value);
            monkey.False = int.Parse(Regex.Match(lines[5], @"\d+").Value);

            _monkeys.Add(monkey);

            _factor *= divisor;
        }
    }

    protected override object SolvePartOne()
    {
        for (long i = 0; i < 20; i++)
        {
            foreach (var monkey in _monkeys)
            {
                monkey.Business(_monkeys);
            }
        }

        return _monkeys
            .OrderByDescending(m => m.Inspects)
            .Take(2)
            .Aggregate(1L, (i, m) => i * m.Inspects);
    }

    protected override object SolvePartTwo()
    {
        Preprocess();

        for (long i = 0; i < 10_000; i++)
        {
            foreach (var monkey in _monkeys)
            {
                monkey.Business(_monkeys, false);
            }

            foreach (var monkey in _monkeys)
            {
                for (int j = 0; j < monkey.Items.Count; j++)
                {
                    monkey.Items.Enqueue(monkey.Items.Dequeue() % _factor);
                }
            }
        }

        return _monkeys
            .OrderByDescending(m => m.Inspects)
            .Take(2)
            .Aggregate(1L, (i, m) => i * m.Inspects);
    }

    protected override string LoadDebugInput()
    {
        return """
        Monkey 0:
          Starting items: 79, 98
          Operation: new = old * 19
          Test: divisible by 23
            If true: throw to monkey 2
            If false: throw to monkey 3

        Monkey 1:
          Starting items: 54, 65, 75, 74
          Operation: new = old + 6
          Test: divisible by 19
            If true: throw to monkey 2
            If false: throw to monkey 0

        Monkey 2:
          Starting items: 79, 60, 97
          Operation: new = old * old
          Test: divisible by 13
            If true: throw to monkey 1
            If false: throw to monkey 3

        Monkey 3:
          Starting items: 74
          Operation: new = old + 3
          Test: divisible by 17
            If true: throw to monkey 0
            If false: throw to monkey 1
        """;
    }

    class Monkey
    {
        public long Inspects { get; private set; }

        public Queue<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public Func<long, bool> Test { get; set; }
        public int True { get; set; }
        public int False { get; set; }

        public void Business(IReadOnlyList<Monkey> monkies, bool worry = true)
        {
            while (Items.Count > 0)
            {
                Inspects++;
                var item = Items.Dequeue();
                item = Operation(item);
                if (worry)
                {
                    item = (long)(item / 3.0);
                }
                var b = Test(item);
                if (b)
                {
                    monkies[True].Items.Enqueue(item);
                }
                else
                {
                    monkies[False].Items.Enqueue(item);
                }
            }
        }
    }
}
