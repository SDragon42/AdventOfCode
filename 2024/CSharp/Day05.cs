namespace AdventOfCode.CSharp.Year2024;

public class Day05(ITestOutputHelper output)
{
    private const int DAY = 5;



    private (IList<(int, int)> pageOrderRules, IList<int[]> pageUpdates, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName);

        var inputEnumerator = input.GetEnumerator();

        IList<(int, int)> pageOrderRules = [];
        while (inputEnumerator.MoveNext() && !string.IsNullOrWhiteSpace(inputEnumerator.Current))
        {
            var parts = inputEnumerator.Current.Split('|', 2, StringSplitOptions.RemoveEmptyEntries);
            pageOrderRules.Add((parts[0].ToInt32(), parts[1].ToInt32()));
        }

        IList<int[]> pageUpdates = [];
        while (inputEnumerator.MoveNext())
        {
            var value = inputEnumerator.Current.Split(',')
                                               .Select(int.Parse)
                                               .ToArray();
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

        var rulesComparer = new RulesComparer(pageOrderRules);

        var value = pageUpdates.Where(pu => IsValidOrder(pu[0], pu[1..], rulesComparer))
                               .Select(GetMiddlePageNumber)
                               .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (pageOrderRules, pageUpdates, expected) = GetTestData(part, inputName);

        var rulesComparer = new RulesComparer(pageOrderRules);

        var value = pageUpdates.Where(pu => !IsValidOrder(pu[0], pu[1..], rulesComparer))
                               .Select(pu => pu.OrderBy(i => i, rulesComparer).ToArray())
                               .Select(GetMiddlePageNumber)
                               .Sum();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    private bool IsValidOrder(int left, int[] remainder, RulesComparer rulesComparer)
    {
        if (remainder.Length == 0)
        {
            return true;
        }

        var result = remainder.Any(right => rulesComparer.Compare(left, right) > 0);

        if (result)
        {
            return false;
        }

        result = IsValidOrder(remainder[0], remainder[1..], rulesComparer);
        return result;
    }

    private int GetMiddlePageNumber(IList<int> pageUpdates)
    {
        var idx = pageUpdates.Count / 2;
        return pageUpdates[idx];
    }

    private class RulesComparer(IList<(int Left, int Right)> _pageOrderRules) : IComparer<int>
    {
        public int Compare(int left, int right)
        {
            var result = _pageOrderRules.Where(rule => rule.Left == right)
                                        .Where(rule => rule.Right == left)
                                        .Any();

            return result ? 1 : -1;
        }
    }
}
