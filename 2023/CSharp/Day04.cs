namespace AdventOfCode.CSharp.Year2023;

public class Day04
{
    private const int DAY = 4;

    private readonly ITestOutputHelper output;
    public Day04(ITestOutputHelper output) => this.output = output;



    private record InputData(int CardNum, int[] WinningNumbers, int[] MyNumbers)
    {
        public int NumberCopies { get; set; } = 1;
    }

    private (List<InputData> input, int? expected) GetTestData(int part, string inputName)
    {
        var inputRegex = new Regex("Card (?<cardNum>.*): (?<winningNums>.*) [|] (?<cardNums>.*)"); // (?<id>.*): (?<plays>.*)");
        var splitter = new char[] { ' ' };

        var input = Services.Input.ReadLines(DAY, inputName)
            .Select(ParseInput)
            .ToList();

        var expected = Services.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);

        InputData ParseInput(string line)
        {
            var match = inputRegex.Match(line);
            if (!match.Success)
                throw new ApplicationException("Input line does not match the pattern");

            var cardNum = match.Groups["cardNum"].Value.ToInt32();
            var winningNums = SplitNumbers(match.Groups["winningNums"].Value);
            var cardNums = SplitNumbers(match.Groups["cardNums"].Value);

            return new InputData(cardNum, winningNums, cardNums);
        }

        int[] SplitNumbers(string numbersText)
        {
            return numbersText
                .Split(splitter, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToInt32())
                .ToArray();
        }
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetPointsForAllCards(input);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetNumberScoreCards(input);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }



    private int GetPointsForAllCards(List<InputData> cardList)
    {
        var result = cardList
            .Select(ScoreCard)
            .Sum();
        return result;
    }

    private int GetNumberScoreCards(List<InputData> cardList)
    {
        for (var i = 0; i < cardList.Count; i++)
        {
            var numMatches = GetCountOfMatchingNumbers(cardList[i]);

            var repeat = cardList[i].NumberCopies;
            while (repeat > 0)
            {
                repeat--;

                for (var j = i + 1; j < cardList.Count && j <= i + numMatches; j++)
                {
                    cardList[j].NumberCopies++;
                }
            }
        }

        var result = cardList.Select(c => c.NumberCopies).Sum();
        return result;
    }

    private int ScoreCard(InputData card)
    {
        var result = 0;

        var numMatches = GetCountOfMatchingNumbers(card);
        while (numMatches > 0)
        {
            if (result == 0)
                result = 1;
            else
                result *= 2;
            numMatches--;
        }

        return result;
    }

    private int GetCountOfMatchingNumbers(InputData card)
    {
        var numMatches = card.MyNumbers.Where(c => card.WinningNumbers.Contains(c)).Count();
        return numMatches;
    }

}