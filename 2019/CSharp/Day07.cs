namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/7
/// </summary>
public class Day07 : TestBase
{
    public Day07(ITestOutputHelper output) : base(output, 7) { }

    private (List<long>, List<long>, long?) GetTestData(string name, int part)
    {
        var inputData = InputHelper.ReadLines(DAY, name)
            .ToList();

        var input = inputData[0]
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        List<long> phase = null;
        if (inputData.Count > 1)
        {
            phase = inputData[1]
                .Split(',')
                .Select(v => v.ToInt64())
                .ToList();
        }

        var expected = InputHelper.ReadLines(DAY, $"{name}-answer{part}")
            ?.FirstOrDefault()
            ?.ToInt64();

        return (input, phase, expected);
    }


    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("example3")]
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, phase, expected) = GetTestData(inputName, 1);

        var answer = RunPart1(input, phase);

        output.WriteLine($"Highest signal that can be sent to the thrusters: {answer}");

        Assert.Equal(expected, answer);
    }


    [Theory]
    [InlineData("example4")]
    [InlineData("example5")]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, phase, expected) = GetTestData(inputName, 2);

        var answer = RunPart2(input, phase);

        output.WriteLine($"Highest signal that can be sent to the thrusters: {answer}");

        Assert.Equal(expected, answer);
    }




    long RunPart1(List<long> code, List<long> fixedPhase)
    {
        var phaseValues = new long[] { 0, 1, 2, 3, 4 };
        var answer = 0L;

        foreach (var phase in GetPhases(phaseValues, fixedPhase))
        {
            var ampA = new IntCode(code);
            var ampB = new IntCode(code);
            var ampC = new IntCode(code);
            var ampD = new IntCode(code);
            var ampE = new IntCode(code);

            var outputValue = 0L;

            ampA.Output += (s, e) => outputValue = e.OutputValue;
            ampB.Output += (s, e) => outputValue = e.OutputValue;
            ampC.Output += (s, e) => outputValue = e.OutputValue;
            ampD.Output += (s, e) => outputValue = e.OutputValue;
            ampE.Output += (s, e) => outputValue = e.OutputValue;

            ampA.AddInput(phase[0], outputValue);
            ampA.Run();

            ampB.AddInput(phase[1], outputValue);
            ampB.Run();

            ampC.AddInput(phase[2], outputValue);
            ampC.Run();

            ampD.AddInput(phase[3], outputValue);
            ampD.Run();

            ampE.AddInput(phase[4], outputValue);
            ampE.Run();

            if (outputValue > answer)
                answer = outputValue;
        }

        return answer;
    }

    long RunPart2(List<long> code, List<long> fixedPhase)
    {
        var phaseValues = new long[] { 5, 6, 7, 8, 9 };
        var answer = 0L;

        foreach (var phase in GetPhases(phaseValues, fixedPhase))
        {
            var ampA = new IntCode(code);
            var ampB = new IntCode(code);
            var ampC = new IntCode(code);
            var ampD = new IntCode(code);
            var ampE = new IntCode(code);

            var outputValue = 0L;

            ampA.Output += (s, e) => outputValue = e.OutputValue;
            ampB.Output += (s, e) => outputValue = e.OutputValue;
            ampC.Output += (s, e) => outputValue = e.OutputValue;
            ampD.Output += (s, e) => outputValue = e.OutputValue;
            ampE.Output += (s, e) => outputValue = e.OutputValue;

            ampA.AddInput(phase[0]);
            ampB.AddInput(phase[1]);
            ampC.AddInput(phase[2]);
            ampD.AddInput(phase[3]);
            ampE.AddInput(phase[4]);

            while (true)
            {
                ampA.AddInput(outputValue);
                ampA.Run();

                ampB.AddInput(outputValue);
                ampB.Run();

                ampC.AddInput(outputValue);
                ampC.Run();

                ampD.AddInput(outputValue);
                ampD.Run();

                ampE.AddInput(outputValue);
                ampE.Run();
                if (ampE.State == IntCodeState.Finished) break;
            }

            if (outputValue > answer)
                answer = outputValue;
        }

        return answer;
    }

    IEnumerable<IList<long>> GetPhases(IList<long> sourceValues, IList<long> fixedPhase = null)
    {
        if (fixedPhase != null)
        {
            yield return fixedPhase;
            yield break;
        }

        var result = sourceValues.GetPermutations();
        foreach (var item in result)
            yield return item.ToList();
    }

}
