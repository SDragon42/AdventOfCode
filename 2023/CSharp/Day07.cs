namespace AdventOfCode.CSharp.Year2023;

public class Day07
{
    private const int DAY = 7;

    private readonly ITestOutputHelper output;

    private readonly Dictionary<char, long> cardTypes = new();
    private readonly List<(Func<string, bool> calc, long value)> handTypes = new();
    private bool useJokers = false;

    private const char JOKER = 'J';

    public Day07(ITestOutputHelper output) => this.output = output;



    private record CamalHand(string Cards, int Bid)
    {
        public long Rank { get; set; } = 0;
    }

    private (List<CamalHand> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = Services.Input.ReadLines(DAY, inputName)
            .Select(line => line.Split(' '))
            .Select(parts => new CamalHand(parts[0], parts[1].ToInt32()))
            .ToList();

        var expected = Services.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }



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

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        useJokers = true;
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


    #region Hand Type Tests
    [Theory]
    [InlineData("AAAAA", 5, false, true)]
    [InlineData("AAAJA", 5, false, false)]
    [InlineData("JJJJJ", 5, false, true)]
    [InlineData("AAAAA", 5, true, true)]
    [InlineData("AJAAA", 5, true, true)]
    [InlineData("AJAAK", 5, true, false)]
    [InlineData("JJJJJ", 5, true, true)]
    [InlineData("AA8AA", 4, false, true)]
    [InlineData("AA8A1", 4, false, false)]
    [InlineData("AA8AA", 4, true, true)]
    [InlineData("AA8AJ", 4, true, true)]
    [InlineData("QJJQ2", 4, true, true)]
    [InlineData("QJ1Q2", 4, true, false)]
    [InlineData("TTT98", 3, false, true)]
    [InlineData("TJT98", 3, false, false)]
    [InlineData("TTT98", 3, true, true)]
    [InlineData("TJT98", 3, true, true)]
    [InlineData("KJT98", 3, true, false)]
    public void Test_IsXOfaKind(string cards, int count, bool useJokers, bool expected)
    {
        this.useJokers = useJokers;
        var result = IsXOfaKind(cards, count);
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("23332", false, true)]
    [InlineData("23331", false, false)]
    [InlineData("23312", false, false)]
    [InlineData("J3332", true, true)]
    [InlineData("23331", true, false)]
    [InlineData("23312", true, false)]
    [InlineData("JJ332", true, true)]
    [InlineData("JJ3J2", true, true)]
    public void Test_IsFullHouse(string cards, bool useJokers, bool expected)
    {
        this.useJokers = useJokers;
        var result = IsFullHouse(cards);
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("23432", false, true)]
    [InlineData("2343J", false, false)]
    [InlineData("23432", true, true)]
    [InlineData("2343J", true, true)]
    public void Test_IsTwoPair(string cards, bool useJokers, bool expected)
    {
        this.useJokers = useJokers;
        var result = IsTwoPair(cards);
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("A23A4", false, true)]
    [InlineData("A23J4", false, false)]
    [InlineData("A23A4", true, true)]
    [InlineData("A23J4", true, true)]
    public void Test_IsOnePair(string cards, bool useJokers, bool expected)
    {
        this.useJokers = useJokers;
        var result = IsOnePair(cards);
        Assert.Equal(expected, result);
    }

    #endregion



    private void SetCardAndHandTypes()
    {
        cardTypes.Clear();
        cardTypes.Add('A', 14);
        cardTypes.Add('K', 13);
        cardTypes.Add('Q', 12);
        cardTypes.Add('J', 11);
        cardTypes.Add('T', 10);
        cardTypes.Add('9', 9);
        cardTypes.Add('8', 8);
        cardTypes.Add('7', 7);
        cardTypes.Add('6', 6);
        cardTypes.Add('5', 5);
        cardTypes.Add('4', 4);
        cardTypes.Add('3', 3);
        cardTypes.Add('2', 2);

        if (useJokers)
            cardTypes[JOKER] = 1;

        handTypes.Clear();
        handTypes.Add((IsFiveOfaKind, 60000000000));
        handTypes.Add((IsFourOfaKind, 50000000000));
        handTypes.Add((IsFullHouse, 40000000000));
        handTypes.Add((IsThreeOfaKind, 30000000000));
        handTypes.Add((IsTwoPair, 20000000000));
        handTypes.Add((IsOnePair, 10000000000));
    }



    private CamalHand RankHand(CamalHand hand)
    {
        var typeScore = handTypes
            .FirstOrDefault(ht => ht.calc(hand.Cards))
            .value;

        var cardScore = hand.Cards.Reverse()
            .Select((c, i) => cardTypes[c] * GetPositionModifier(i))
            .ToArray()
            .Sum();

        hand.Rank = typeScore + cardScore;
        return hand;

        long GetPositionModifier(int pos) => (long)Math.Pow(100, pos);
    }


    private bool IsFiveOfaKind(string cards) => IsXOfaKind(cards, 5);

    private bool IsFourOfaKind(string cards) => IsXOfaKind(cards, 4);

    private bool IsFullHouse(string cards)
    {
        var cardCounts = GetCardCounts(cards);
        var jokerCount = 0;
        if (useJokers)
            cardCounts.Remove(JOKER, out jokerCount);

        var has3 = false;
        var has2 = false;

        var orderedCounts = cardCounts.AsEnumerable()
            .OrderByDescending(kv => kv.Value)
            .ToArray();

        for (var i = 0; i < orderedCounts.Length; i++)
        {
            var kv = orderedCounts[i];
            if (HadFoundneeded(3, ref has3, kv))
                continue;
            if (HadFoundneeded(2, ref has2, kv))
                continue;
        }

        var result = has3 && has2;
        return result;

        bool HadFoundneeded(int minNeeded, ref bool hasNeeded, KeyValuePair<char, int> kv)
        {
            if (hasNeeded)
                return false;

            var needed = minNeeded - kv.Value;
            if (needed == 0)
            {
                hasNeeded = true;
                return true;
            }
            if (needed > 0 && needed <= jokerCount)
            {
                jokerCount -= needed;
                hasNeeded = true;
                return true;
            }
            return false;
        }
    }

    private bool IsThreeOfaKind(string cards) => IsXOfaKind(cards, 3);

    private bool IsTwoPair(string cards)
    {
        var cardCounts = GetCardCounts(cards);
        var jokerCount = 0;
        if (useJokers)
            cardCounts.Remove(JOKER, out jokerCount);

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
        var cardCounts = GetCardCounts(cards);
        var jokerCount = 0;
        if (useJokers)
            cardCounts.Remove(JOKER, out jokerCount);

        var numPairs = cardCounts
            .Where(kv => kv.Value + jokerCount >= 2)
            .Count();

        var result = (numPairs >= 1);
        return result;
    }



    private bool IsXOfaKind(string cards, int count)
    {
        var cardCounts = GetCardCounts(cards);
        if (useJokers)
        {
            cardCounts.TryGetValue(JOKER, out int jokerCount);

            foreach (var key in cardCounts.Keys.Where(k => k != JOKER))
                cardCounts[key] += jokerCount;
        }

        var result = cardCounts
            .Any(kv => kv.Value >= count);
        return result;
    }

    private static IDictionary<char, int> GetCardCounts(string cards)
    {
        var result = cards.Distinct()
            .Select(c => (card: c, count: cards.Count(z => z == c)))
            .ToDictionary(k => k.card, v => v.count);
        return result;
    }
}