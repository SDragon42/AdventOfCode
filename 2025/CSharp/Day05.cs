namespace AdventOfCode.CSharp.Year2025;

public class Day05(ITestOutputHelper output)
{
    private const int DAY = 5;



    private (IList<(long Start, long End)> freshIdRanges, IList<long> availableIds, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName);
        var inputEnumerator = input.GetEnumerator();

        var freshIdRanges = ReadSectionLines(inputEnumerator)
            .Select(line => line.Split('-')
                                .Select(long.Parse)
                                .ToArray())
            .Select(arr => (arr[0], arr[1]))
            .ToArray();

        var availableIds = ReadSectionLines(inputEnumerator)
            .Select(long.Parse)
            .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt64();

        return (freshIdRanges, availableIds, expected);

        IEnumerable<string> ReadSectionLines(IEnumerator<string> inputEnumerator)
        {
            while (inputEnumerator.MoveNext() && !string.IsNullOrWhiteSpace(inputEnumerator.Current))
            {
                yield return inputEnumerator.Current;
            }
        }
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (freshIdRanges, availableIds, expected) = GetTestData(part, inputName);

        var value = GetFreshIds(freshIdRanges, availableIds).Count();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (freshIdRanges, _, expected) = GetTestData(part, inputName);

        var value = GetCountOfPossibleFreshIDs(freshIdRanges);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private IEnumerable<long> GetFreshIds(IList<(long Start, long End)> freshIdRanges, IList<long> availableIds)
    {
        foreach (var id in availableIds)
        {
            var IsFresh = freshIdRanges.Any(range => range.Start <= id && id <= range.End);
            if (IsFresh)
            {
                yield return id;
            }
        }
    }

    private long GetCountOfPossibleFreshIDs(IList<(long Start, long End)> freshIdRanges)
    {
        long count = 0;

        var sortedRanges = freshIdRanges
            .OrderBy(range => range.Start);

        long? currentStart = null;
        long? currentEnd = null;
        foreach (var (Start, End) in sortedRanges)
        {
            if (currentStart == null)
            {
                currentStart = Start;
                currentEnd = End;
                continue;
            }
            
            if (Start > currentEnd)
            {
                count += (currentEnd.Value - currentStart.Value + 1);

                currentStart = Start;
                currentEnd = End;
                continue;
            }

            if (End > currentEnd)
            {
                currentEnd = End;
            }
        }

        count += (currentEnd.Value - currentStart.Value + 1);

        return count;
    }
}
