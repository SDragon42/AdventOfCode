using System.Linq;

namespace AdventOfCode.CSharp.Year2024;

public class Day04(ITestOutputHelper output)
{
    private const int DAY = 4;



    private (char[][] input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(x => x.ToArray())
            .ToArray();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }


    private readonly IReadOnlyList<(int xOffset, int yOffset)> directions = [
        (-1, -1),
        (0, -1),
        (1, -1),
        (1, 0),
        (1, 1),
        (0, 1),
        (-1, 1),
        (-1, 0)
        ];


    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        int value = CountPatternInPuzzle(input, "XMAS".ToArray());

        int value = CountPatternInPuzzle(input, pattern);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int CountPatternInPuzzle(char[][] puzzle, IReadOnlyList<char> pattern)
    {
        var bounds = new Bounds(xLower: 0,
                                xUpper: puzzle[0].Length - 1,
                                yLower: 0,
                                yUpper: puzzle.Length - 1);

        var result = 0;
        for (var y = bounds.yLower; y <= bounds.yUpper; y++)
        {
            for (int x = bounds.xLower; x <= bounds.xUpper; x++)
            {
                var paths = GetSearchPaths(x, y, pattern.Count, bounds).ToArray();
                result += paths.Where(p => MatchesPattern(puzzle, pattern, p))
                               .Count();
            }
        }

        return result;
    }

    private bool MatchesPattern(char[][] puzzle, IReadOnlyList<char> pattern, Point[] searchPath)
    {
        for (int i = 0; i < searchPath.Length; i++)
        {
            var a = puzzle[searchPath[i].Y][searchPath[i].X];
            var b = pattern[i];
            if (a != b)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerable<Point[]> GetSearchPaths(int x, int y, int length, Bounds bounds)
    {
        foreach (var offset in directions)
        {
            var xPathValues = CreateArray(x, length, offset.xOffset);
            var yPathValues = CreateArray(y, length, offset.yOffset);

            var path = xPathValues.Zip(yPathValues)
                                  .Select((pair) => new Point(pair.First, pair.Second))
                                  .ToArray();
            if (path.All(p => IsInBounds(p, bounds)))
            {
                yield return path;
            }
        }


        static IEnumerable<int> CreateArray(int start, int length, int step)
        {
            for (var i = 0; i < length; i++)
            {
                yield return start + (step * i);
            }
        }
    }

    private bool IsInBounds(Point point, Bounds bounds)
    {
        if (bounds.xLower > point.X || point.X > bounds.xUpper)
            return false;
        if (bounds.yLower > point.Y || point.Y > bounds.yUpper)
            return false;
        return true;
    }

    private record Bounds(int xLower,
                          int xUpper,
                          int yLower,
                          int yUpper);
}
