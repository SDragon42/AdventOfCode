using System.Runtime.Intrinsics.Arm;
using System.Text;
using AdventOfCode.CSharp.Common;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2023;

public class Day01
{
    private const int DAY = 1;

    private readonly ITestOutputHelper output;
    public Day01(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

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

        var value = input
            .Select(RecoverCalibrationNumber)
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

        var value = input
            .Select(ReplaceWordsWithNumbers)
            .Select(RecoverCalibrationNumber)
            .Sum();

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }



    private int RecoverCalibrationNumber(string line)
    {
        var charArrary = line
            .Where(char.IsNumber)
            .ToArray();

        var result = string.Concat(charArrary.First(), charArrary.Last());
        return result.ToInt32();
    }


    private readonly Dictionary<string, string> NumberMap = new Dictionary<string, string>() {
        { "one",   "1" },
        { "two",   "2" },
        { "three", "3" },
        { "four",  "4" },
        { "five",  "5" },
        { "six",   "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine",  "9" }
    };
    private string ReplaceWordsWithNumbers(string line)
    {
        var lineSpan = line.AsSpan();
        var output = new StringBuilder();
        var start = 0;
        var end = start + 1;

        while (start < lineSpan.Length && end <= lineSpan.Length)
        {
            var sample = lineSpan[start..end].ToString();
            var matchedKeys = NumberMap.Keys.Where(k => k.StartsWith(sample));
            var numMatches = matchedKeys.Count();

            if (numMatches == 0)
            {
                output.Append(sample[0]);
                start++;
                end = start + 1;
                continue;
            }

            if (numMatches == 1)
            {
                var key = matchedKeys.First();
                var checkEnd = start + key.Length;
                if (checkEnd <= lineSpan.Length)
                {
                    var check = lineSpan[start..checkEnd].ToString();
                    if (key == check)
                    {
                        output.Append(NumberMap[key]);
                        start++;
                        end = start + 1;
                        continue;
                    }
                }

                output.Append(sample);
                start++;
                end = start + 1;
                continue;
            }

            end++;
        }

        var result = output.ToString();
        return result;
    }
}