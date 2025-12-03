namespace AdventOfCode.CSharp.Year2025;

public class Day03(ITestOutputHelper output)
{
    private const int DAY = 3;



    private (int[][] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .Select(l => l.Select(c => c - 48).ToArray())
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
        var (batteryBanks, expected) = GetTestData(part, inputName);

        var value = batteryBanks.Select(b => GetMaximumJolt(b, 2))
                                .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (batteryBanks, expected) = GetTestData(part, inputName);

        var value = batteryBanks.Select(b => GetMaximumJolt(b, 12))
                                .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private static long GetMaximumJolt(int[] batteryBank, int numToTurnOn, int offset = 0)
    {
        var index = offset + GetHighestDigitIndex(batteryBank[offset..^(numToTurnOn - 1)]);

        var result = batteryBank[index] * (long)(Math.Pow(10, numToTurnOn) / 10);

        if (numToTurnOn > 1)
        {
            result += GetMaximumJolt(batteryBank, numToTurnOn - 1, index + 1);
        }
        return result;
    }

    private static int GetHighestDigitIndex(IEnumerable<int> digits)
    {
        var index = digits.Select((val, idx) => (val, idx))
                          .OrderByDescending(t => t.val)
                          .First()
                          .idx;
        return index;
    }
}
