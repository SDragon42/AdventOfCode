using Xunit;

namespace AdventOfCode.CSharp.Year2024;

public class Day02(ITestOutputHelper output)
{
    private const int DAY = 2;



    private (List<List<int>> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(v => v.ToInt32())
                                .ToList())
            .ToList();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        int value = input.Where(IsReportSafe)
                         .Count();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        int value = input.Where(IsReportSafeWithDampening)
                         .Count();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private bool IsReportSafe(IList<int> reportValues)
    {
        var valuePairs = reportValues.Windowed(2)
                                     .Select(x => new ValuePair(x[0], x[1]))
                                     .ToList();

        var result = valuePairs.All(IsIncreasing)
                  || valuePairs.All(IsDecreasing);
        if (!result)
        {
            return false;
        }

        result = valuePairs.All(pair => IsInRange(pair, min: 1, max: 3));

        return result;
    }
    private bool IsReportSafeWithDampening(IList<int> reportValues)
    {
        var isSafe = IsReportSafe(reportValues);
        if (isSafe)
            return isSafe;

        isSafe = GenerateDampenedReports(reportValues)
             .Any(IsReportSafe);

        return isSafe;
    }

    public IEnumerable<IList<int>> GenerateDampenedReports(IList<int> report)
    {
        for (int i = 0; i < report.Count; i++)
        {
            var dampenedReport = report.Where((v, idx) => idx != i)
                                       .ToList();
            yield return dampenedReport;
        }
    }

    private static bool IsIncreasing(ValuePair pair) => pair.A < pair.B;
    private static bool IsDecreasing(ValuePair pair) => pair.A > pair.B;
    private static bool IsInRange(ValuePair pair, int min, int max)
    {
        var dist = Math.Abs(pair.A - pair.B);
        var inRange = min <= dist && dist <= max;
        return inRange;
    }



    private record ValuePair(int A, int B);
}
