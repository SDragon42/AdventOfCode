namespace AdventOfCode.CSharp.Year2019;

public class Day02 : TestBase
{
    public Day02(ITestOutputHelper output) : base(output, 2) { }



    private (List<long>, long?) GetTestData(string name, int part)
    {
        var input = InputHelper.ReadLines(DAY, name)
            .First()
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var expected = InputHelper.ReadLines(DAY, $"{name}-answer{part}")
            ?.FirstOrDefault()
            ?.ToInt64();

        return (input, expected);
    }

    [Theory]
    [InlineData("example1", -1, -1)]
    [InlineData("input", 12, 2)]
    public void Part1(string inputName, int valueAt1, int valueAt2)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var value = RunCode(input, valueAt1, valueAt2);

        output.WriteLine($"Value as position 0 : {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData("input", 19690720)]
    public void Part2(string inputName, long valueAt0)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var value = FindNounVerb(input, valueAt0);

        output.WriteLine($"Noun-Verb pair is : {value}");

        Assert.Equal(expected, value);
    }




    long RunCode(List<long> code, long valueAt1, long valueAt2)
    {
        var computer = new IntCode(code);
        if (valueAt1 >= 0) computer.Poke(1, valueAt1);
        if (valueAt2 >= 0) computer.Poke(2, valueAt2);
        computer.Run();

        var valAt0 = computer.Peek(0);
        return valAt0;
    }


    int FindNounVerb(List<long> input, long desiredValueAt0)
    {
        for (var noun = 0; noun <= 99; noun++)
        {
            for (var verb = 0; verb <= 99; verb++)
            {
                var result = RunCode(input, noun, verb);
                if (result == desiredValueAt0)
                    return (100 * noun) + verb;
            }
        }

        return -1;
    }

}
