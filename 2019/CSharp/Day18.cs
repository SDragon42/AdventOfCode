namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/18
/// </summary>
public class Day18 : TestBase
{
    public Day18(ITestOutputHelper output) : base(output, 18) { }


    private (List<List<char>>, int?) GetTestData(string name, int part)
    {
        var input = InputHelper.ReadLines(DAY, name)
            .Select(l => l.ToList())
            .ToList();

        var expected = InputHelper.ReadLines(DAY, $"{name}-answer{part}")
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }


    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("example3")]
    [InlineData("example4")]
    [InlineData("example5")]
    // [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var result = RunPart1(input);
        output.WriteLine($"How many steps is the shortest path that collects all of the keys? {result}");

        Assert.Equal(expected, result);
    }



    int RunPart1(List<List<char>> input)
    {
        var answer = 0;
        // return Helper.GetPuzzleResultText($"How many steps is the shortest path that collects all of the keys? {answer}", answer, puzzleData.ExpectedAnswer);
        return answer;
    }

    // string RunPart2(InputAnswer puzzleData)
    // {
    //     var answer = 0;
    //     return Helper.GetPuzzleResultText($": {answer}", answer, puzzleData.ExpectedAnswer);
    // }

}
