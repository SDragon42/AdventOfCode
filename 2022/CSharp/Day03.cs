using System;

namespace AdventOfCode.CSharp.Year2022;

public class Day03_Rucksack_Reorganization
{
    private const int DAY = 3;

    private readonly ITestOutputHelper output;
    public Day03_Rucksack_Reorganization(ITestOutputHelper output) => this.output = output;



    private (List<string>, int?) GetTestData(int part, string inputName)
    {
        var input = InputHelper.LoadInputFile(DAY, inputName)
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

        var value = FindSumOfPriorityTypes(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = FindSumOfGroupIdentityBadges(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int FindSumOfPriorityTypes(List<string> input)
    {
        var result = input
            .Select(FindDulicateType)
            .Select(CharToPriority)
            .Sum();
        return result;
    }

    private int FindSumOfGroupIdentityBadges(List<string> input)
    {
        var result = SplitIntoGroups(input)
            .Select(FindGroupBadge)
            .Select(CharToPriority)
            .Sum();
        return result;
    }

    private IEnumerable<List<string>> SplitIntoGroups(List<string> input)
    {
        var i = 0;
        while (i < input.Count)
        {
            var group = new List<string>
            {
                input[i++],
                input[i++],
                input[i++]
            };
            yield return group;
        }
    }

    private char FindGroupBadge(List<string> groupInput)
    {
        var elf1 = groupInput[0].AsSpan();
        var elf2 = groupInput[1].AsSpan();
        var elf3 = groupInput[2].AsSpan();

        // Find duplicate
        foreach (var c in elf1)
        {
            if (elf2.Contains(c) && elf3.Contains(c))
                return c;
        }

        throw new ApplicationException("No Badge found.");
    }

    private char FindDulicateType(string contents)
    {
        // Split contents
        var len = contents.Length / 2;
        var contentsSpan = contents.AsSpan();

        var compartment1 = contentsSpan.Slice(0, len);
        var compartment2 = contentsSpan.Slice(len);

        // Find duplicate
        foreach (var c in compartment1)
        {
            if (compartment2.Contains(c))
                return c;
        }

        throw new ApplicationException("dup not found.");
    }

    /// <summary>
    /// Lowercase item types a through z have priorities 1 through 26.
    /// Uppercase item types A through Z have priorities 27 through 52.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private int CharToPriority(char value)
    {
        return (value >= 'a')
            ? value - 96
            : value - 64 + 26;
    }

}