namespace AdventOfCode.CSharp.Year2025;

public class Day12(ITestOutputHelper output)
{
    private const int DAY = 12;

    private (string[] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt64();

        return (input, expected);
    }



    // [Theory]
    // [InlineData(1, "example1")]
    // // [InlineData(1, "input")]
    // public void Part1(int part, string inputName)
    // {
    //     var (input, expected) = GetTestData(part, inputName);

    //     var value = -1L;

    //     output.WriteLine($"Answer: {value}");

    //     Assert.Equal(expected, value);
    // }

    // [Theory]
    // [InlineData(2, "example1")]
    // [InlineData(2, "input")]
    // public void Part2(int part, string inputName)
    // {
    //     var (input, expected) = GetTestData(part, inputName);

    //     var value = -1L;

    //     output.WriteLine($"Answer: {value}");

    //     Assert.Equal(expected, value);
    // }
    
}
