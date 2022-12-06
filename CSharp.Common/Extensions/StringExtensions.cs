namespace AdventOfCode.CSharp.Common;

public static class StringExtensions
{
    /// <summary>
    /// Converts a string to an Int32 using a fluent method chaining style
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static int ToInt32(this string text)
    {
        return int.Parse(text);
    }

    /// <summary>
    /// Converts a string to an Int64 using a fluent method chaining style
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static long ToInt64(this string text)
    {
        return long.Parse(text);
    }

}
