namespace AdventOfCode.CSharp.Year2022;

public class Day02
{
    private const int DAY = 2;

    private readonly ITestOutputHelper output;
    public Day02(ITestOutputHelper output) => this.output = output;



    private enum Move
    {
        Rock, Paper, Scissors
    }

    private enum PlayResult
    {
        Lose, Draw, Win
    }

    record StrategyGuideMoves
    {
        public required string Theirs { get; init; }
        public required string Yours { get; init; }
    }

    record RoundMoves
    {
        public required Move Theirs { get; init; }
        public required Move Yours { get; init; }
    }



    private (List<StrategyGuideMoves>, int?) GetTestData(int part, string inputName)
    {
        var input = InputHelper.LoadInputFile(DAY, inputName)
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(l => new StrategyGuideMoves
            {
                Theirs = l[0],
                Yours = l[1]
            })
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, inputName)
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = CalculateTotalScoreV1(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = CalculateTotalScoreV2(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int CalculateTotalScoreV1(List<StrategyGuideMoves> input)
    {
        var totalScore = input
            .Select(GetRoundMovesV1)
            .Select(PlayRound)
            .Sum();
        return totalScore;
    }

    private RoundMoves GetRoundMovesV1(StrategyGuideMoves r)
    {
        var result = new RoundMoves
        {
            Theirs = DecodeTheirMove(r.Theirs),
            Yours = CalculateYourMoveV1(r.Yours)
        };
        return result;
    }

    private Move CalculateYourMoveV1(string value) => value switch
    {
        "X" => DecodeTheirMove("A"),
        "Y" => DecodeTheirMove("B"),
        "Z" => DecodeTheirMove("C"),
        _ => throw new ApplicationException("Invalid code")
    };



    private int CalculateTotalScoreV2(List<StrategyGuideMoves> input)
    {
        var totalScore = input
            .Select(GetRoundMovesV2)
            .Select(PlayRound)
            .Sum();
        return totalScore;
    }

    private RoundMoves GetRoundMovesV2(StrategyGuideMoves r)
    {
        var theirMove = DecodeTheirMove(r.Theirs);
        var result = new RoundMoves
        {
            Theirs = theirMove,
            Yours = CalculateYourMoveV2(theirMove, r.Yours)
        };
        return result;
    }

    private Move CalculateYourMoveV2(Move theirMove, string value) => value switch
    {
        "X" => theirMove switch // Make losing move
        {
            Move.Rock => Move.Scissors,
            Move.Paper => Move.Rock,
            Move.Scissors => Move.Paper,
            _ => throw new ApplicationException("Invalid Move")
        },
        "Y" => theirMove, // Make draw move
        "Z" => theirMove switch // Make winning move
        {
            Move.Rock => Move.Paper,
            Move.Paper => Move.Scissors,
            Move.Scissors => Move.Rock,
            _ => throw new ApplicationException("Invalid Move")
        },
        _ => throw new ApplicationException("Invalid Code")
    };



    private Move DecodeTheirMove(string value) => value switch
    {
        "A" => Move.Rock,
        "B" => Move.Paper,
        "C" => Move.Scissors,
        _ => throw new ApplicationException("Invalid Code")
    };

    private int PlayRound(RoundMoves plays)
    {
        var playResult = ProcessRound(plays.Theirs, plays.Yours);
        var scoreYouPlay = GetPlayPoints(plays.Yours);
        var scoreResults = GetResultPoints(playResult);

        return scoreYouPlay + scoreResults;
    }

    private PlayResult ProcessRound(Move theirMove, Move yourMove) => theirMove switch
    {
        Move.Rock => yourMove switch
        {
            Move.Rock => PlayResult.Draw,
            Move.Paper => PlayResult.Win,
            Move.Scissors => PlayResult.Lose,
            _ => throw new ApplicationException("Invalid Move")
        },
        Move.Paper => yourMove switch
        {
            Move.Rock => PlayResult.Lose,
            Move.Paper => PlayResult.Draw,
            Move.Scissors => PlayResult.Win,
            _ => throw new ApplicationException("Invalid Move")
        },
        Move.Scissors => yourMove switch
        {
            Move.Rock => PlayResult.Win,
            Move.Paper => PlayResult.Lose,
            Move.Scissors => PlayResult.Draw,
            _ => throw new ApplicationException("Invalid Move")
        },
        _ => throw new ApplicationException("Invalid Move")
    };


    private int GetResultPoints(PlayResult playResult) => playResult switch
    {
        PlayResult.Lose => 0,
        PlayResult.Draw => 3,
        PlayResult.Win => 6,
        _ => throw new ApplicationException("Invalid Result")
    };

    private int GetPlayPoints(Move move) => move switch
    {
        Move.Rock => 1,
        Move.Paper => 2,
        Move.Scissors => 3,
        _ => throw new ApplicationException("Invalid Move")
    };

}