using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode.CSharp.Year2022;

public class Day04_Camp_Cleanup
{
    private const int DAY = 4;

    private readonly ITestOutputHelper output;
    public Day04_Camp_Cleanup(ITestOutputHelper output) => this.output = output;


    private (List<(Range, Range)>, int?) GetTestData(int part, string inputName)
    {
        var input = InputHelper.LoadInputFile(DAY, inputName)
            .Select(LineToRanges)
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, inputName)
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }

    private (Range, Range) LineToRanges(string line)
    {
        var split = line.IndexOf(',');

        var pair1 = line.AsSpan(0, split);
        var pair2 = line.AsSpan(split + 1);

        return (MakeRange(pair1), MakeRange(pair2));

        Range MakeRange(ReadOnlySpan<char> data)
        {
            var split = data.IndexOf('-');
            var a = int.Parse(data.Slice(0, split));
            var b = int.Parse(data.Slice(split + 1));
            return new Range(a, b);
        };
    }




    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetNumberOfFullyContainedPairs(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetNumberOfOverlappedPairs(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetNumberOfFullyContainedPairs(List<(Range, Range)> input)
    {
        var result = input
            .Where(IsFullyContained)
            .Count();
        return result;
    }

    private int GetNumberOfOverlappedPairs(List<(Range, Range)> input)
    {
        var result = input
            .Where(IsOverlapped)
            .Count();
        return result;
    }

    

    private bool IsFullyContained((Range, Range) pair)
    {
        var (a, b) = pair;
        if (a.Start.Value <= b.Start.Value && a.End.Value >= b.End.Value)
            return true;
        if (b.Start.Value <= a.Start.Value && b.End.Value >= a.End.Value)
            return true;
        return false;
    }

    private bool IsOverlapped((Range, Range) pair)
    {
        var (a, b) = pair;
        if (a.Start.Value <= b.Start.Value && b.Start.Value <= a.End.Value)
            return true;
        if (a.Start.Value <= b.End.Value && b.End.Value <= a.End.Value)
            return true;
        if (b.Start.Value <= a.Start.Value && a.Start.Value <= b.End.Value)
            return true;
        if (b.Start.Value <= a.End.Value && a.End.Value <= b.End.Value)
            return true;
        return false;
    }

}

