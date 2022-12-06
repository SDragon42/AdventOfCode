namespace AdventOfCode.CSharp.Year2022;

public class Day06_Tuning_Trouble
{
    private const int DAY = 6;

    private readonly ITestOutputHelper output;
    public Day06_Tuning_Trouble(ITestOutputHelper output) => this.output = output;



    private (string, int?) GetTestData(int part, string inputName)
    {
        var input = InputHelper.LoadInputFile(DAY, inputName)
            .First();

        var expected = InputHelper.LoadAnswerFile(DAY, part, inputName)
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "example2")]
    [InlineData(1, "example3")]
    [InlineData(1, "example4")]
    [InlineData(1, "example5")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetCharsBeforeStartOfPacket(input, 4);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "example2")]
    [InlineData(2, "example3")]
    [InlineData(2, "example4")]
    [InlineData(2, "example5")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetCharsBeforeStartOfPacket(input, 14);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetCharsBeforeStartOfPacket(string input, int markerSize)
    {
        var inputSpan = input.AsSpan();
        for (int i = 0; i < inputSpan.Length - markerSize; i++)
        {
            if (AllUnique(inputSpan.Slice(i, markerSize)))
                return i + markerSize;
        }

        return -1;

        bool AllUnique(ReadOnlySpan<char> data)
        {
            for (int i = 0; i < data.Length - 1; i++)
                for (int j = i+1; j < data.Length; j++)
                    if (data[i] == data[j]) return false;
            return true;
        }
    }
}