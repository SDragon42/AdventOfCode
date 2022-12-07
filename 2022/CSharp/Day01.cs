namespace AdventOfCode.CSharp.Year2022;

public class Day01_Calorie_Counting
{
    private const int DAY = 1;

    private readonly ITestOutputHelper output;
    public Day01_Calorie_Counting(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            .ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetMostCalories(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetMostCalories(input, 3);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetMostCalories(List<string> input)
    {
        var calorieTotals = GetTotalCaloriesPerElf(input);
        var most = calorieTotals.Max();

        return most;
    }

    private int GetMostCalories(List<string> input, int sumCount)
    {
        var calorieTotals = GetTotalCaloriesPerElf(input);
        var top3 = calorieTotals.OrderByDescending(c => c).Take(sumCount).Sum();

        return top3;
    }

    private IEnumerable<int> GetTotalCaloriesPerElf(List<string> input)
    {
        var position = 0;
        var sumTotal = 0;
        while (position < input.Count)
        {
            var calorie = input[position];
            position++;

            if (!string.IsNullOrWhiteSpace(calorie))
            {
                sumTotal += calorie.ToInt32();
                continue;
            }

            yield return sumTotal;
            sumTotal = 0;
        }

        yield return sumTotal;
    }

}