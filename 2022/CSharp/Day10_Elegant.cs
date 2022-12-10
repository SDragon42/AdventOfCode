using System.Collections.ObjectModel;
using System.Text;

namespace AdventOfCode.CSharp.Year2022;

/// <summary>
/// This more elegant implementation was found here:
/// https://github.com/encse/adventofcode/blob/master/2022/Day10/Solution.cs
/// I just re-implemented it here for my own practice.
/// </summary>
public class Day10_Elegant
{
    private const int DAY = 10;

    private readonly ITestOutputHelper output;
    public Day10_Elegant(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData_Part1(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }

    private (List<string> input, string expected) GetTestData_Part2(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}");

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData_Part1(part, inputName);

        var value = SumTheSixSignalStrengths(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData_Part2(part, inputName);

        var value = BuildOuputMessage(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int SumTheSixSignalStrengths(List<string> input)
    {
        var readCycles = new int[] { 20, 60, 100, 140, 180, 220 };

        var result = RunSignal(input)
            .Where(s => readCycles.Contains(s.cycle))
            .Select(CalcSignalStrength)
            .Sum();
        return result;
    }

    private string BuildOuputMessage(List<string> input)
    {
        const int NumColumns = 40;
        const int NumRows = 6;
        var pixelArray = new char[NumColumns * NumRows];

        foreach (var (cycle, x) in RunSignal(input))
        {
            var idx = cycle - 1;
            var col = idx % NumColumns;

            var pixel = (Math.Abs(x - col) < 2)
                ? '#'
                : '.';
            pixelArray[idx] = pixel;
        }
        var lines = Enumerable.Range(0, NumRows)
            .Select(MakeLine);

        var result = string.Join(Environment.NewLine, lines);
        return result;

        string MakeLine(int r)
        {
            var start = r * NumColumns;
            var end = start + NumColumns;
            var result = new string(pixelArray[start..end]);
            return result;
        }
    }

    private IEnumerable<(int cycle, int x)> RunSignal(List<string> input)
    {
        var cycle = 1;
        var x = 1;

        foreach (var line in input)
        {
            var parts = line.Split(' ');
            switch (parts[0])
            {
                case "noop":
                    yield return (cycle++, x);
                    break;

                case "addx":
                    yield return (cycle++, x);
                    yield return (cycle++, x);
                    x += int.Parse(parts[1]);
                    break;

                default:
                    throw new ApplicationException($"Unknown Instruction: {parts[0]}");
            }
        }
    }

    private int CalcSignalStrength((int cycle, int x) pair)
    {
        return pair.cycle * pair.x;
    }
}