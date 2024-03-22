namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/9
/// </summary>
public class Day09 : TestBase
{
    public Day09(ITestOutputHelper output) : base(output, 9) { }


    private (List<long>, string) GetTestData(string name, int part)
    {
        var input = InputHelper.ReadLines(DAY, name)
            .First()
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var expected = InputHelper.ReadLines(DAY, $"{name}-answer{part}")
            ?.FirstOrDefault();

        return (input, expected);
    }


    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("example3")]
    public void TestCase(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var outputBuffer = new List<long>();
        var computer = new IntCode(input);
        computer.Output += (s, e) => outputBuffer.Add(e.OutputValue);

        computer.Run();

        var answer = string.Join(',', outputBuffer);

        Assert.Equal(answer, expected);
    }


    [Theory]
    [InlineData("input", 1, 1)]
    [InlineData("input", 2, 2)]
    public void RunBoost(string inputName, int part, int initialInputValue)
    {
        var (input, expected) = GetTestData(inputName, part);

        var answer = default(string);

        var computer = new IntCode(input);
        computer.Output += (s, e) => answer = e.OutputValue.ToString();

        computer.AddInput(initialInputValue);
        computer.Run();

        Assert.Equal(answer, expected);
    }

}
