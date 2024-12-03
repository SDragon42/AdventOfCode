namespace AdventOfCode.CSharp.Year2024;

public class Day03(ITestOutputHelper output)
{
    private const int DAY = 3;



    private (string input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadText(DAY, inputName);

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var instructions = GetInstructions(input);

        int value = instructions.Select(i => i.Value1 * i.Value2)
                                .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example2")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var instructions = GetInstructions(input, enabledOnly: true);

        int value = instructions.Select(i => i.Value1 * i.Value2)
                                .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private const string DO_INSTRUCTION = "do()";
    private const string DONT_INSTRUCTION = "don't()";
    private readonly Regex mulPattern = new Regex(@"mul\((?<x>\d+),(?<y>\d+)\)");

    private IList<InstructionData> GetInstructions(string memory, bool enabledOnly = false)
    {
        if (enabledOnly)
        {
            memory = string.Join(string.Empty, GetEnabledSegmentsOnly(memory));
        }

        var instructions = mulPattern
                .Matches(memory)
                .Select(m => new InstructionData(m.Groups["x"].Value.ToInt32(),
                                                 m.Groups["y"].Value.ToInt32()))
                .ToList();

        return instructions;
    }
    private static IEnumerable<string> GetEnabledSegmentsOnly(string memory)
    {
        var enabledMemoryParts = memory.Split(DO_INSTRUCTION);

        foreach (var line in enabledMemoryParts)
        {
            var enabledMemory = line.Split(DONT_INSTRUCTION, 2)[0];
            yield return enabledMemory;
        }
    }


    private record InstructionData(int Value1, int Value2);
}
