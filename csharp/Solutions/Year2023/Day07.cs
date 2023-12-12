using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2023;
internal class Day07 : ASolution
{
    private List<Hand> _hands = new();

    public Day07() : base(07, 2023, "Camel Cards", false)
    {
    }

    protected override object SolvePartOne()
    {
        _hands = InputByNewLine.Select(s => new Hand(s)).ToList();

        _hands.Sort();

        return _hands.Select((h, i) => h.Bid * (i + 1)).Sum();
    }

    protected override object SolvePartTwo()
    {
        _hands = InputByNewLine.Select(s => new Hand(s, hasJokers: true)).ToList();

        _hands.Sort();

        return _hands.Select((h, i) => h.Bid * (i + 1)).Sum();
    }

    protected override string LoadDebugInput()
    {
        return """
            32T3K 765
            T55J5 684
            KK677 28
            KTJJT 220
            QQQJA 483
            """;
    }

    [DebuggerDisplay("{Cards}, {Bid}, {Type}")]
    private class Hand : IComparable<Hand>
    {
        private bool _hasJokers;
        private string _jokerlessHand;

        public string Cards { get; private set; }
        public int Bid { get; private set; }
        public CamelType Type { get; private set; }
        public int? Jokers { get; private set; }

        public Hand(string input, bool hasJokers = false)
        {
            _hasJokers = hasJokers;

            ParseHand(input);
            DetermineHand();
        }

        private void DetermineHand()
        {
            if (Jokers > 0)
            {
                var g = _jokerlessHand.GroupBy(c => c)
                    .ToDictionary(g => g.Key, g => g.Count());
                Type = g.Keys.Count switch
                {
                    0 => CamelType.FiveOfAKind,
                    1 => CamelType.FiveOfAKind,
                    2 => Jokers == 1 && g.Values.All(v => v == 2) ? CamelType.FullHouse : CamelType.FourOfAKind,
                    3 => CamelType.ThreeOfAKind,
                    4 => CamelType.OnePair,
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                var g = Cards.GroupBy(c => c)
                    .ToDictionary(g => g.Key, g => g.Count());
                Type = g.Keys.Count switch
                {
                    0 => throw new NotImplementedException(),
                    1 => CamelType.FiveOfAKind,
                    2 => g.Values.Any(v => v == 4) ? CamelType.FourOfAKind : CamelType.FullHouse,
                    3 => g.Values.Any(v => v == 3) ? CamelType.ThreeOfAKind : CamelType.TwoPair,
                    4 => CamelType.OnePair,
                    5 => CamelType.HighCard,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        

        private static Dictionary<char, int> s_CardStrength = new()
        {
            { '2', 0 },
            { '3', 1 },
            { '4', 2 },
            { '5', 3 },
            { '6', 4 },
            { '7', 5 },
            { '8', 6 },
            { '9', 7 },
            { 'T', 8 },
            { 'J', 9 },
            { 'Q', 10 },
            { 'K', 11 },
            { 'A', 12 },
        };

        private static Dictionary<char, int> s_CardStrengthJokers = new()
        {
            { 'J', -1 },
            { '2', 0 },
            { '3', 1 },
            { '4', 2 },
            { '5', 3 },
            { '6', 4 },
            { '7', 5 },
            { '8', 6 },
            { '9', 7 },
            { 'T', 8 },
            { 'Q', 10 },
            { 'K', 11 },
            { 'A', 12 },
        };

        private int CompareCards(string other)
        {

            ReadOnlySpan<char> theseCards = Cards;
            ReadOnlySpan<char> otherCards = other;

            for (int i = 0; i < 5; i++)
            {
                char thisChar = theseCards[i];
                char otherChar = otherCards[i];

                if (thisChar == otherChar)
                {
                    continue;
                }

                if (_hasJokers)
                {
                    return s_CardStrengthJokers[thisChar].CompareTo(s_CardStrengthJokers[otherChar]);
                }

                return s_CardStrength[thisChar].CompareTo(s_CardStrength[otherChar]);
                
            }

            throw new NotImplementedException();
        }

        private void ParseHand(string input)
        {
            var a = input.SplitInTwo(' ');

            if (_hasJokers)
            {
                Jokers = a.Item1.Count(c => c == 'J');
                _jokerlessHand = a.Item1.Replace("J", "");
            }

            Cards = a.Item1;
            Bid = int.Parse(a.Item2);
        }

        public enum CamelType
        {
            Unprocessed = 0,
            HighCard = 1,
            OnePair = 2,
            TwoPair = 3,
            ThreeOfAKind = 4,
            FullHouse = 5,
            FourOfAKind = 6,
            FiveOfAKind = 7
        }

        public int CompareTo(Hand other)
        {
            if (this.Equals(other))
            {
                return 0;
            }

            var result = Type.CompareTo(other.Type);
            if (result == 0)
            {
                return CompareCards(other.Cards);
            }

            return result;
        }
    }
}
