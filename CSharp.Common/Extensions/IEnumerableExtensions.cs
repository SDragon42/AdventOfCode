namespace AdventOfCode.CSharp.Common;

public static class IEnumerableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <param name="actionMethod"></param>
    public static void ForEach<T>(this IEnumerable<T> elements, Action<T> actionMethod)
    {
        foreach (var item in elements)
            actionMethod(item);
    }

}
