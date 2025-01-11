namespace AdventOfCode.CSharp.Year2024;

public class Day01(ITestOutputHelper output)
{
    private const int DAY = 1;



    private (List<int> leftInput, List<int> rightInput, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToList();
        var left = input.Select(x => x[0].ToInt32()).ToList();
        var right = input.Select(x => x[1].ToInt32()).ToList();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (left, right, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (leftList, rightList, expected) = GetTestData(part, inputName);

        var value = CalculateTotalDistanceBetween(leftList, rightList);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (leftList, rightList, expected) = GetTestData(part, inputName);

        var value = CalculateSimilarityBetween(leftList, rightList);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int CalculateTotalDistanceBetween(IList<int> leftList, IList<int> rightList)
    {
        var leftSorted = leftList.OrderBy(x => x);
        var rightSorted = rightList.OrderBy(x => x);

        var combined = leftSorted.Zip(rightSorted);

        var total = combined.Sum(x => Math.Abs(x.First - x.Second));

        return total;
    }


    private int CalculateSimilarityBetween(IList<int> leftList, IList<int> rightList)
    {
        var occuranceDict = rightList.GroupBy(key => key)
            .Select(g => (g.Key, Count: g.Count()))
            .ToDictionary();

        var score = leftList
            .Select(value =>
            {
                if (occuranceDict.TryGetValue(value, out var result))
                    return value * result;
                return 0;
            })
            .Sum();

        return score;
    }
}
