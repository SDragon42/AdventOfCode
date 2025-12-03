namespace AdventOfCode.CSharp.Year2025;

public class Day02(ITestOutputHelper output)
{
    private const int DAY = 2;



    private (string[] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .SelectMany(l => l.Split(','))
                                      .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (idRanges, expected) = GetTestData(part, inputName);

        var value = idRanges.SelectMany(r => GetInvalidIds(r, IsSequenceRepeatedTwice))
                            .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (idRanges, expected) = GetTestData(part, inputName);

        var value = idRanges.SelectMany(r => GetInvalidIds(r, IsSequenceRepeatedAtLeastTwice))
                            .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    private IEnumerable<long> GetInvalidIds(string range, Func<long, bool> IsInvalidId)
    {
        var parts = range.Split('-');

        var start = parts[0].ToInt64();
        var end = parts[1].ToInt64();

        for (var id = start; id <= end; id++)
        {
            if (IsInvalidId(id))
                yield return id;
        }
    }

    private static bool IsSequenceRepeatedTwice(long id)
    {
        var idString = id.ToString();
        var mid = idString.Length / 2;
        var firstHalf = idString[..mid];
        var secondHalf = idString[mid..];
        return firstHalf == secondHalf;
    }

    private static bool IsSequenceRepeatedAtLeastTwice(long id)
    {
        var idString = id.ToString();
        var parts = 2;
        while (parts <= idString.Length)
        {
            if (idString.Length % parts == 0)
            {
                var segmentLength = idString.Length / parts;
                var segment = idString[..segmentLength];
                var repeated = string.Concat(Enumerable.Repeat(segment, parts));
                if (repeated == idString)
                    return true;
            }
            parts++;
        }

        return false;
    }
}
