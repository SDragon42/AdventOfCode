namespace CSharp.Common.Tests;

public class ExtensionsTests
{
    [Fact]
    public void ForEach()
    {
        var elements = new[] { 1, 2, 3 };

        var expectedSum = elements.Sum(x => x);
        var expectedCount = elements.Length;

        var sum = 0;
        var count = 0;

        void localAction(int value)
        {
            sum += value;
            count++;
        }

        elements.ForEach(localAction);

        Assert.Equal(expectedCount, count);
        Assert.Equal(expectedSum, sum);
    }


    [Fact]
    public void ToInt32()
    {
        var value = "42";
        var expectedResult = 42;

        var result = value.ToInt32();

        Assert.Equal(expectedResult, result);
    }


    [Fact]
    public void ToInt64()
    {
        var value = "42";
        var expectedResult = 42L;

        var result = value.ToInt64();

        Assert.Equal(expectedResult, result);
    }
}