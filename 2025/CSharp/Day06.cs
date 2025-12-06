namespace AdventOfCode.CSharp.Year2025;

public class Day06(ITestOutputHelper output)
{
    private const int DAY = 6;

    private (string[][] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .ToArray();

        var problemGroups = GetVerticalWidth(input)
            .Select(r => BuildProblemGroup(r, input))
            .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt64();

        return (problemGroups, expected);
    }

    private static IEnumerable<(int start, int end)> GetVerticalWidth(string[] lines)
    {
        var maxChars = lines[0].Length;

        var start = -1;
        var end = -1;

        for (var i = 0; i < maxChars; i++)
        {
            if (start < 0)
            {
                start = i;
                end = i;
            }

            var isSeparator = lines.Select(l => l[i])
                                   .All(c => c == ' ');

            if (!isSeparator)
            {
                end = i;
                continue;
            }

            if (start < end)
            {
                yield return (start, end);
            }
            start = -1;
            end = -1;
        }

        yield return (start, end);
    }

    private static string[] BuildProblemGroup((int start, int end) r, string[] input)
    {
        var entries = new string[input.Length];
        for (var i = 0; i < entries.Length; i++)
        {
            entries[i] = input[i][r.start..(r.end + 1)];
        }
        return entries;
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (problemGroups, expected) = GetTestData(part, inputName);

        var value = problemGroups.Select(SolveProblem)
                                 .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (problemGroups, expected) = GetTestData(part, inputName);

        var value = problemGroups.Select(SolveProblem2)
                                 .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private static Func<IEnumerable<long>, long> GetOperation(string op)
    {
        return op switch
        {
            "+" => (numbers) => numbers.Aggregate((a, b) => a + b),
            "*" => (numbers) => numbers.Aggregate((a, b) => a * b),
            _ => throw new InvalidOperationException("Unknown operation")
        };
    }

    private long SolveProblem(string[] problemGroup)
    {
        var numbers = problemGroup[..^1].Select(long.Parse);
        var operation = GetOperation(problemGroup.Last().Trim());
        var result = operation(numbers);
        return result;
    }

    private long SolveProblem2(string[] problemGroup)
    {
        var numbers = BuildVerticalNumbers(problemGroup[..^1]);
        var operation = GetOperation(problemGroup.Last().Trim());
        var result = operation(numbers);
        return result;
    }

    private IEnumerable<long> BuildVerticalNumbers(string[] numberGroup)
    {
        var i = numberGroup[0].Length - 1;
        while (i >= 0)
        {
            var x = numberGroup.Select(line => line[i]);
            var result = string.Concat(x);
            yield return long.Parse(result);
            i--;
        }
    }
}
