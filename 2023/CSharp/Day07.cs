namespace AdventOfCode.CSharp.Year2023;

public class Day07
{
    private const int DAY = 7;

    private readonly ITestOutputHelper output;

    private readonly Dictionary<char, long> CardTypes;
    private readonly List<(Func<string, bool> calc, long value)> HandTypes;

    public Day07(ITestOutputHelper output)
    {
        this.output = output;

        CardTypes = new Dictionary<char, long>
        {
            { 'A', 14 },
            { 'K', 13 },
            { 'Q', 12 },
            { 'J', 11 },
            { 'T', 10 },
            { '9', 9 },
            { '8', 8 },
            { '7', 7 },
            { '6', 6 },
            { '5', 5 },
            { '4', 4 },
            { '3', 3 },
            { '2', 2 }
        };

        HandTypes = new List<(Func<string, bool>, long)>
        {
            (IsFiveOfaKind,  60000000000),
            (IsFourOfaKind,  50000000000),
            (IsFullHouse,    40000000000),
            (IsThreeOfaKind, 30000000000),
            (IsTwoPair,      20000000000),
            (IsOnePair,      10000000000),
        };
    }




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
        var (input, expected) = GetTestData(part, inputName);

        input.ForEach(hand => RankHand(hand));

        var value = input
            .OrderBy(h => h.Rank)
            .Select((h, i) => (i + 1) * h.Bid)
            .Sum();

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }



    #region Hand Type Tests

    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("AAAAA", true)]
    [InlineData("AAAJA", false)]
    public void Test_IsFiveOfaKind(string cards, bool expected) => Assert.Equal(expected, IsFiveOfaKind(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("AA8AA", true)]
    [InlineData("AA8A1", false)]
    public void Test_IsFourOfaKind(string cards, bool expected) => Assert.Equal(expected, IsFourOfaKind(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("23332", true)]
    [InlineData("23331", false)]
    [InlineData("23312", false)]
    public void Test_IsFullHouse(string cards, bool expected) => Assert.Equal(expected, IsFullHouse(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("TTT98", true)]
    [InlineData("TJT98", false)]
    public void Test_IsThreeOfaKind(string cards, bool expected) => Assert.Equal(expected, IsThreeOfaKind(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("23432", true)]
    [InlineData("2343J", false)]
    public void Test_IsTwoPair(string cards, bool expected) => Assert.Equal(expected, IsTwoPair(cards));


    [Trait("Method Tests", "")]
    [Theory]
    [InlineData("A23A4", true)]
    [InlineData("A23J4", false)]
    public void Test_IsOnePair(string cards, bool expected) => Assert.Equal(expected, IsOnePair(cards));

    #endregion



    private CamalHand RankHand(CamalHand hand)
    {
        var validTypes = HandTypes
            .Where(ht => ht.calc(hand.Cards))
            .ToArray();
        var firstType = validTypes.FirstOrDefault();
        var typeScore = firstType.value;

        var cardScoreArray = hand.Cards.Reverse()
            .Select((c, i) => CardTypes[c] * GetPositionModifier(i))
            .ToArray();
        var cardScore = cardScoreArray.Sum();

        hand.Rank = typeScore + cardScore;
        return hand;

        long GetPositionModifier(int pos) => (long)Math.Pow(100, pos);
    }


    private bool IsFiveOfaKind(string cards) => IsXOfaKind(cards, 5);

    private bool IsFourOfaKind(string cards) => IsXOfaKind(cards, 4);

    private bool IsFullHouse(string cards)
    {
        var cardCounts = GetCardCounts(cards);

        var has3 = cardCounts.Any(c => c.Value == 3);
        var has2 = cardCounts.Any(c => c.Value == 2);

        var result = has3 && has2;
        return result;
    }

    private bool IsThreeOfaKind(string cards) => IsXOfaKind(cards, 3);

    private bool IsTwoPair(string cards)
    {
        var numPairs = GetCardCounts(cards)
            .Where(kv => kv.Value == 2)
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



    private static bool IsXOfaKind(string cards, int count)
    {
        var result = GetCardCounts(cards)
            .Any(kv => kv.Value == count);
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