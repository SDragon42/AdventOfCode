namespace AdventOfCode.CSharp.Year2024;

public class Day07(ITestOutputHelper output)
{
    private const int DAY = 7;



    private (IList<(long testValue, long[] numbers)> input, long? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(line =>
            {
                var parts = line.Split(':', 2);
                var testValue = parts[0].ToInt64();

                var numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(v => v.ToInt64())
                                      .ToArray();

                return (testValue, numbers);
            })
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = input.Where(eq => IsValidTestValue(eq.testValue,
                                                       [Operator.Add, Operator.Multiply], 
                                                       eq.numbers[0], 
                                                       eq.numbers[1..]))
                         .Sum(line => line.testValue);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = input.Where(eq => IsValidTestValue(eq.testValue,
                                                       [Operator.Add, Operator.Multiply, Operator.Concat],
                                                       eq.numbers[0],
                                                       eq.numbers[1..]))
                         .Sum(line => line.testValue);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private bool IsValidTestValue(long testValue, Operator[] operators, long total, Span<long> numbers)
    {
        if (total > testValue)
        {
            return false;
        }

        if (numbers.Length == 0)
        {
            return testValue == total;
        }

        var result = false;
        foreach (var op in operators)
        {
            result |= op switch
            {
                Operator.Add => IsValidTestValue(testValue, operators, 
                                                 total + numbers[0], 
                                                 numbers[1..]),

                Operator.Multiply => IsValidTestValue(testValue, operators, 
                                                      total * numbers[0], 
                                                      numbers[1..]),
                
                Operator.Concat => IsValidTestValue(testValue, operators,
                                                    $"{total}{numbers[0]}".ToInt64(), 
                                                    numbers[1..]),
                
                _ => throw new InvalidOperationException()
            };
        }
        return result;
    }

    private enum Operator { Add, Multiply, Concat }
}
