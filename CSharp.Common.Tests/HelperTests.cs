namespace CSharp.Common.Tests;

public class HelperTests
{

    [Theory]
    [InlineData(123456789, 1, 9)]
    [InlineData(123456789, 2, 8)]
    public void GetDigitRight_INT(int value, int offset, int expected)
    {
        var result = Helper.GetDigitRight(value, offset);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(123456789L, 1, 9)]
    [InlineData(123456789L, 2, 8)]
    public void GetDigitRight_LONG(long value, int offset, int expected)
    {
        var result = Helper.GetDigitRight(value, offset);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(123456789, 1, 1)]
    [InlineData(123456789, 2, 2)]
    [InlineData(42, 2, 2)]
    [InlineData(123456, 2, 2)]
    public void GetDigitLeft_INT(int value, int offset, int expected)
    {
        var result = Helper.GetDigitLeft(value, offset);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(18, 21, 3)]
    [InlineData(18, 24, 6)]
    public void FindGreatestCommonFactor(int a, int b, int expected)
    {
        var result = Helper.FindGreatestCommonFactor(a, b);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3, 5, 15)]
    [InlineData(5, 10, 10)]
    [InlineData(10, 15, 30)]
    public void FindLeastCommonMultiple(long a, long b, long expected)
    {
        var result = Helper.FindLeastCommonMultiple(a, b);
        Assert.Equal(expected, result);
    }

}
