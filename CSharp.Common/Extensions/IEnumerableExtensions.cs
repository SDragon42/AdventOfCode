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


    /// <summary>
    /// Returns an enumerable yielding a sliding window containing elements drawn from the input enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <param name="windowSize"></param>
    /// <returns></returns>
    /// <remarks>Code from: https://stackoverflow.com/a/8877876/6136</remarks>
    public static IEnumerable<T[]> Windowed<T>(this IEnumerable<T> elements, int windowSize)
    {
        var arr = new T[windowSize];

        var i = 0;
        var r = windowSize - 1;

        using var e = elements.GetEnumerator();

        while (e.MoveNext())
        {
            arr[i] = e.Current;
            i = (i + 1) % windowSize;

            if (r == 0)
            {
                var arrR = new T[windowSize];
                for (int j = 0; j < windowSize; j++)
                    arrR[j] = arr[(i + j) % windowSize];
                yield return arrR;
            }
            else
                r--;
        }
    }

}
