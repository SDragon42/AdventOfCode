namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/16
/// </summary>
public class Day16 : TestBase
{
    public Day16(ITestOutputHelper output) : base(output, 16) { }

    private readonly IReadOnlyList<int> basePattern = new List<int>() { 0, 1, 0, -1 };


    private (string, string) GetTestData(string name, int part)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .First();

        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault();

        return (input, expected);
    }


    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("example3")]
    [InlineData("example4")]
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var numPhases = 100;
        var result = RunPart1(input, numPhases);
        output.WriteLine($"first 8 digits after {numPhases} phases : {result}");

        Assert.Equal(expected, result);
    }


    string ListToString(IList<int> l) => string.Join("", l.Select(x => x.ToString()));

    Action<string> Debug = (t) => Console.WriteLine(t);

    string RunPart1(string input, int numPhases)
    {
        var phase = 0;

        var signal = input.Select(c => c.ToString().ToInt32()).ToList();
        var maxDigits = signal.Count;

        Debug("Input Signal: " + ListToString(signal));
        Debug("");

        var line = new StringBuilder();
        while (phase < numPhases)
        {
            phase++;

            var newSignal = new List<int>();
            for (var i = 0; i < maxDigits; i++)
            {
                line.Clear();
                var newSignalLine = signal.ToList();
                var dupCount = i + 1;
                var total = 0;
                for (var j = 0; j < maxDigits; j++)
                {
                    var val = GetPatternDigit(dupCount, j);

                    var tmp = $"{newSignalLine[j]}*{val}";
                    line.Append($"{tmp,-5}");
                    if (j + 1 < maxDigits)
                        line.Append(" + ");
                    newSignalLine[j] *= val;
                    total += newSignalLine[j];
                }
                total = Helper.GetDigitRight(Math.Abs(total), 1);
                line.Append($" = {total}");

                newSignal.Add(total);

                Debug(line.ToString());
            }

            signal = newSignal;

            Debug("");
            Debug($"After {phase} Phase: {ListToString(signal)}");
            Debug("");
        }

        var result = ListToString(signal.Take(8).ToList());
        return result;
    }

    int GetPatternDigit(int repeatCount, int idx)
    {
        idx++;
        var a = basePattern.Count * repeatCount;
        while (idx >= a)
            idx -= a;
        var i = idx / repeatCount;
        return basePattern[i];
    }

}