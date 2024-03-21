namespace CSharp.Common.Tests.Extensions;

public class IEnumerableExtensionsTests
{

    private static IList<int> GetTestData()
    {
        var items = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        return items;
    }


    #region ForEach Tests

    [Fact]
    public void ForEach_ValidSource_HitsEveryElement()
    {
        var items = GetTestData();
        var expected = items.Sum();

        int sumTotal = 0;
        var action = (int value) => { sumTotal += value; };

        IEnumerableExtensions.ForEach(items, action);

        Assert.Equal(expected, sumTotal);
    }

    [Fact]
    public void ForEach_NullSource_ThrowsException()
    {
        List<int>? items = default;

        var action = (int value) => { var x = value; };

        Assert.Throws<ArgumentNullException>(
            () => IEnumerableExtensions.ForEach(items, action));
    }

    [Fact]
    public void ForEach_NullAction_ThrowsException()
    {
        var items = GetTestData();

        Action<int>? action = null;

        Assert.Throws<ArgumentNullException>(
            () => IEnumerableExtensions.ForEach(items, action));
    }

    #endregion



    #region Windowed Tests
    public static IEnumerable<object[]> Windowed_SmallerSize_ReturnsExpectedElements_Params()
    {
        var count = GetTestData().Count;
        for (int windowSize = 1; windowSize <= count + 1; windowSize++)
        {
            var expectedNumWindows = count - windowSize + 1;
            yield return new object[] { windowSize, expectedNumWindows };
        }
    }
    [Theory]
    [MemberData(nameof(Windowed_SmallerSize_ReturnsExpectedElements_Params))]
    public void Windowed_ValidSize_ReturnsWindows(int windowSize, int expected)
    {
        var items = GetTestData();

        var results = items.Windowed(windowSize);

        Assert.Equal<int>(expected, results.Count());
    }



    public static IEnumerable<object[]> Windowed_LargerSize_ReturnsNothing_Params()
    {
        var count = GetTestData().Count;
        var windowSize = count + 1;
        yield return new object[] { windowSize, 0 };
    }
    [Theory]
    [MemberData(nameof(Windowed_LargerSize_ReturnsNothing_Params))]
    public void Windowed_LargerSize_ReturnsNothing(int windowSize, int expected)
    {
        var items = GetTestData();

        var results = items.Windowed(windowSize);

        Assert.Equal<int>(expected, results.Count());
    }



    [Theory]
    [InlineData(3, 0, 0, 1, 2)]
    [InlineData(3, 1, 1, 2, 3)]
    [InlineData(2, 4, 4, 5)]
    public void Windowed_Valid_ReturnsExpectedElements(int windowSize, int offset, params int[] expected)
    {
        var items = GetTestData();

        var parts = items.Windowed(windowSize);
        var results = parts.Skip(offset).First();

        Assert.Equal<int>(expected, results);
    }



    [Fact]
    public void Windowed_NullSource_ThrowsException()
    {
        List<int>? items = default;
        var windowSize = 2;

        void action()
        {
            var windows = IEnumerableExtensions
                .Windowed(items, windowSize)
                .ToArray();
        }

        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Windowed_InvalidSize_ThrowsException()
    {
        var items = GetTestData();
        var windowSize = 0;

        void action()
        {
            var windows = IEnumerableExtensions
                .Windowed(items, windowSize)
                .ToArray();
        }

        Assert.Throws<ArgumentOutOfRangeException>(action); ;
    }

    #endregion
}
