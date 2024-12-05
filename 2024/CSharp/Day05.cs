namespace AdventOfCode.CSharp.Year2024;

public class Day05(ITestOutputHelper output)
{
    private const int DAY = 5;



    private (IList<(int, int)> pageOrderRules, IList<IList<int>> pageUpdates, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName);

        var inputEnumerator = input.GetEnumerator();

        IList<(int, int)> pageOrderRules = [];
        while (inputEnumerator.MoveNext() && !string.IsNullOrWhiteSpace(inputEnumerator.Current))
        {
            var parts = inputEnumerator.Current.Split('|', 2, StringSplitOptions.RemoveEmptyEntries);
            pageOrderRules.Add((parts[0].ToInt32(), parts[1].ToInt32()));
        }

        IList<IList<int>> pageUpdates = [];
        while (inputEnumerator.MoveNext())
        {
            var value = inputEnumerator.Current.Split(',')
                                               .Select(int.Parse)
                                               .ToList();
            pageUpdates.Add(value);
        }

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (pageOrderRules, pageUpdates, expected);
    }


    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (pageOrderRules, pageUpdates, expected) = GetTestData(part, inputName);

        var value = pageUpdates.Where(pu => IsCorrectlyOrdered(pu, pageOrderRules))
                               .Select(GetMiddlePageNumber)
                               .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    private bool IsCorrectlyOrdered(IList<int> pageUpdates, IList<(int, int)> pageOrderRules)
    {
        return IsValidOrder(pageUpdates.First(),
                            pageUpdates.Skip(1),
                            pageOrderRules);
    }

    private bool IsValidOrder(int? first, IEnumerable<int> remainder, IList<(int left, int right)> pageOrderRules)
    {
        if (remainder.Count() == 0)
        {
            return true;
        }

        var result = remainder.Any(value => pageOrderRules.Where(rule => rule.left == value)
                                                          .Where(r => r.right == first)
                                                          .Any());
        if (result) return false;

        result = IsValidOrder(remainder.First(), remainder.Skip(1), pageOrderRules);
        return result;
    }


    private int GetMiddlePageNumber(IList<int> pageUpdates)
    {
        var idx = pageUpdates.Count / 2;
        return pageUpdates[idx];
    }
}
