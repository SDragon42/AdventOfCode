namespace AdventOfCode.CSharp.Year2023;

using Number = long;

file static class Extentions
{
    public static Number ToNumber(this string text) => Number.Parse(text);
}



public class Day09
{
    private const int DAY = 9;

    private readonly ITestOutputHelper output;

    public Day09(ITestOutputHelper output) => this.output = output;



    private (List<Number[]> input, Number? expected) GetTestData(int part, string inputName)
    {
        var input = Services.Input.ReadLines(DAY, inputName)
            .Select(x => x.Split(' ').Select(c => c.ToNumber()).ToArray())
            .ToList();

        var expected = Services.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToNumber();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = input
            .Select(s => FindValueInSequence(s, GetNextValue))
            .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = input
            .Select(s => FindValueInSequence(s, GetPreviousValue))
            .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private Number FindValueInSequence(Number[] sequence, Func<Number[], Number, Number> GetValue)
    {
        var allZeros = sequence.All(x => x == 0);
        if (allZeros)
            return 0;

        var nextSequence = GetSequenceDiffs(sequence);
        var result = FindValueInSequence(nextSequence, GetValue);
        
        return GetValue(sequence, result);
    }

    Number GetNextValue(Number[] seq, Number v)
        => seq.Last() + v;

    Number GetPreviousValue(Number[] seq, Number v)
        => seq.First() - v;

    Number[] GetSequenceDiffs(Number[] sequence)
        => sequence
            .Windowed(2)
            .Select(x => x[1] - x[0])
            .ToArray();

}
