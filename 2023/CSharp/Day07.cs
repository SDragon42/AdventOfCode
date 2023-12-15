namespace AdventOfCode.CSharp.Year2023;

public class Day07
{
    private const int DAY = 7;

    private readonly ITestOutputHelper output;

    private readonly Dictionary<char, long> CardTypes = new();
    private readonly List<(Func<string, bool> calc, long value)> HandTypes = new();

    public Day07(ITestOutputHelper output) => this.output = output;


    private record CamalHand(string Cards, int Bid)
    {
        public long Rank { get; set; } = 0;
    }


    private (List<CamalHand> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(line => line.Split(' '))
            .Select(parts => new CamalHand(parts[0], parts[1].ToInt32()))
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }



    [Trait("Puzzle", "")]
    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        SetCardAndHandTypes();

        var (input, expected) = GetTestData(part, inputName);

        input.ForEach(hand => RankHand(hand));

        var value = input
            .OrderBy(h => h.Rank)
            .Select((h, i) => (i + 1) * h.Bid)
            .Sum();

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }

    [Trait("Puzzle", "")]
    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        SetCardAndHandTypes(replaceJacksWithJokers: true);

        var (input, expected) = GetTestData(part, inputName);

        input.ForEach(hand => RankHand(hand));

        var value = input
            .OrderBy(h => h.Rank)
            .Select((h, i) => (i + 1) * h.Bid)
            .Sum();

        output.WriteLine("   Low: 253148077");
        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }


    #region Hand Type Tests

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("AAAAA", true)]
    [InlineData("AAAJA", false)]
    [InlineData("JJJJJ", true)]
    public void Test_IsFiveOfaKind(string cards, bool expected) => Assert.Equal(expected, IsFiveOfaKind(cards));

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("AAAAA", true)]
    [InlineData("AJAAA", true)]
    [InlineData("AJAAK", false)]
    [InlineData("JJJJJ", true)]
    public void Test_IsFiveOfaKind_WithJokers(string cards, bool expected) => Assert.Equal(expected, IsFiveOfaKindWithJokers(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("AA8AA", true)]
    [InlineData("AA8A1", false)]
    public void Test_IsFourOfaKind(string cards, bool expected) => Assert.Equal(expected, IsFourOfaKind(cards));

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("AA8AA", true)]
    [InlineData("AA8AJ", true)]
    [InlineData("QJJQ2", true)]
    [InlineData("QJ1Q2", false)]
    public void Test_IsFourOfaKind_WithJokers(string cards, bool expected) => Assert.Equal(expected, IsFourOfaKindWithJokers(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("23332", true)]
    [InlineData("23331", false)]
    [InlineData("23312", false)]
    public void Test_IsFullHouse(string cards, bool expected) => Assert.Equal(expected, IsFullHouse(cards));

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("J3332", true)]
    [InlineData("23331", false)]
    [InlineData("23312", false)]
    [InlineData("JJ332", true)]
    [InlineData("JJ3J2", true)]
    public void Test_IsFullHouse_WithJokers(string cards, bool expected) => Assert.Equal(expected, IsFullHouseWithJokers(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("TTT98", true)]
    [InlineData("TJT98", false)]
    public void Test_IsThreeOfaKind(string cards, bool expected) => Assert.Equal(expected, IsThreeOfaKind(cards));

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("TTT98", true)]
    [InlineData("TJT98", true)]
    [InlineData("KJT98", false)]
    public void Test_IsThreeOfaKind_WithJokers(string cards, bool expected) => Assert.Equal(expected, IsThreeOfaKindWithJokers(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("23432", true)]
    [InlineData("2343J", false)]
    public void Test_IsTwoPair(string cards, bool expected) => Assert.Equal(expected, IsTwoPair(cards));

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("23432", true)]
    [InlineData("2343J", true)]
    public void Test_IsTwoPair_WithJokers(string cards, bool expected) => Assert.Equal(expected, IsTwoPairWithJokers(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("A23A4", true)]
    [InlineData("A23J4", false)]
    public void Test_IsOnePair(string cards, bool expected) => Assert.Equal(expected, IsOnePair(cards));

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("A23A4", true)]
    [InlineData("A23J4", true)]
    public void Test_IsOnePair_WithJokers(string cards, bool expected) => Assert.Equal(expected, IsOnePairWithJokers(cards));

    #endregion



    private void SetCardAndHandTypes(bool replaceJacksWithJokers = false)
    {
        CardTypes.Clear();
        CardTypes.Add('A', 14);
        CardTypes.Add('K', 13);
        CardTypes.Add('Q', 12);
        CardTypes.Add('J', 11);
        CardTypes.Add('T', 10);
        CardTypes.Add('9', 9);
        CardTypes.Add('8', 8);
        CardTypes.Add('7', 7);
        CardTypes.Add('6', 6);
        CardTypes.Add('5', 5);
        CardTypes.Add('4', 4);
        CardTypes.Add('3', 3);
        CardTypes.Add('2', 2);

        if (replaceJacksWithJokers)
            CardTypes['J'] = 1;

        HandTypes.Clear();
        HandTypes.Add((
            replaceJacksWithJokers ? IsFiveOfaKindWithJokers : IsFiveOfaKind,
            60000000000));
        HandTypes.Add((
            replaceJacksWithJokers ? IsFourOfaKindWithJokers : IsFourOfaKind,
            50000000000));
        HandTypes.Add((
            replaceJacksWithJokers ? IsFullHouseWithJokers : IsFullHouse,
            40000000000));
        HandTypes.Add((
            replaceJacksWithJokers ? IsThreeOfaKindWithJokers : IsThreeOfaKind,
            30000000000));
        HandTypes.Add((
            replaceJacksWithJokers ? IsTwoPairWithJokers : IsTwoPair,
            20000000000));
        HandTypes.Add((
            replaceJacksWithJokers ? IsOnePairWithJokers : IsOnePair,
            10000000000));
    }



    private CamalHand RankHand(CamalHand hand)
    {
        var typeScore = HandTypes
            .FirstOrDefault(ht => ht.calc(hand.Cards))
            .value;

        var cardScore = hand.Cards.Reverse()
            .Select((c, i) => CardTypes[c] * GetPositionModifier(i))
            .ToArray()
            .Sum();

        hand.Rank = typeScore + cardScore;
        return hand;

        long GetPositionModifier(int pos) => (long)Math.Pow(100, pos);
    }


    private bool IsFiveOfaKind(string cards) => IsXOfaKind(cards, 5);
    private bool IsFiveOfaKindWithJokers(string cards) => IsXOfaKindWithJokers(cards, 5);

    private bool IsFourOfaKind(string cards) => IsXOfaKind(cards, 4);
    private bool IsFourOfaKindWithJokers(string cards) => IsXOfaKindWithJokers(cards, 4);

    private bool IsFullHouse(string cards)
    {
        var cardCounts = GetCardCounts(cards);

        var has3 = cardCounts.Any(c => c.Value == 3);
        var has2 = cardCounts.Any(c => c.Value == 2);

        var result = has3 && has2;
        return result;
    }
    private bool IsFullHouseWithJokers(string cards)
    {
        var cardCounts = GetCardCounts(cards);
        cardCounts.Remove('J', out int jokerCount);

        var keys = cardCounts.Keys.ToArray();

        var orderedCounts = cardCounts.AsEnumerable()
            .OrderByDescending(kv => kv.Value)
            .ToArray()
            ;

        var has3 = false;
        var has2 = false;

        for (var i = 0; i < orderedCounts.Length; i++)
        {
            var kv = orderedCounts[i];
            if (!has3)
            {
                var needed = 3 - kv.Value;
                if (needed == 0)
                {
                    has3 = true;
                    continue;
                }
                if (needed > 0 && needed <= jokerCount)
                {
                    jokerCount -= needed;
                    has3 = true;
                    continue;
                }
            }
            if (!has2)
            {
                var needed = 2 - kv.Value;
                if (needed == 0)
                {
                    has2 = true;
                    continue;
                }
                if (needed > 0 && needed <= jokerCount)
                {
                    jokerCount -= needed;
                    has2 = true;
                    continue;
                }
            }
        }

        var result = has3 && has2;
        return result;
    }

    private bool IsThreeOfaKind(string cards) => IsXOfaKind(cards, 3);
    private bool IsThreeOfaKindWithJokers(string cards) => IsXOfaKindWithJokers(cards, 3);

    private bool IsTwoPair(string cards)
    {
        var numPairs = GetCardCounts(cards)
            .Where(kv => kv.Value == 2)
            .Count();

        var result = (numPairs == 2);
        return result;
    }
    private bool IsTwoPairWithJokers(string cards)
    {
        var cardCounts = GetCardCounts(cards);
        cardCounts.Remove('J', out int jokerCount);

        var keys = cardCounts.Keys.ToArray();

        if (jokerCount > 0)
        {
            foreach (var key in keys)
            {
                if (cardCounts[key] == 1)
                {
                    cardCounts[key] += 1;
                    jokerCount--;

                    if (jokerCount == 0)
                        break;
                }
            }
        }

        var numPairs = cardCounts
            .Where(kv => kv.Value + jokerCount >= 2)
            .Count();

        var result = (numPairs == 2);
        return result;
    }

    private bool IsOnePair(string cards)
    {
        var numPairs = GetCardCounts(cards)
            .Where(kv => kv.Value == 2)
            .Count();

        var result = (numPairs == 1);
        return result;
    }
    private bool IsOnePairWithJokers(string cards)
    {
        var cardCounts = GetCardCounts(cards);
        cardCounts.Remove('J', out int jokerCount);

        var numPairs = cardCounts
            .Where(kv => kv.Value + jokerCount >= 2)
            .Count();

        var result = (numPairs >= 1);
        return result;
    }



    private static bool IsXOfaKind(string cards, int count)
    {
        var result = GetCardCounts(cards)
            .Any(kv => kv.Value == count);
        return result;
    }
    private static bool IsXOfaKindWithJokers(string cards, int count)
    {
        var cardCounts = GetCardCounts(cards);
        cardCounts.Remove('J', out int jokerCount);

        if (jokerCount >= count) 
            return true;

        var result = cardCounts
            .Any(kv => kv.Value + jokerCount >= count);
        return result;
    }

    private static IDictionary<char, int> GetCardCounts(string cards)
    {
        var result = cards.Distinct()
            .Select(c => (card: c, count: cards.Count(z => z == c)))
            .ToDictionary(
                c => c.card,
                c => c.count
                );
        return result;
    }
}