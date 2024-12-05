﻿using System.Linq;

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


    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var textArrary = "XMAS".ToArray();

        IList<OffsetPoint[]> offsetPatterns = [
            CreatePatternArray(new(0,0), textArrary.Length, new(1,-1)),
            CreatePatternArray(new(0,0), textArrary.Length, new(1,1)),
            CreatePatternArray(new(0,0), textArrary.Length, new(1,0)),
            CreatePatternArray(new(0,0), textArrary.Length, new(0,1))
        ];
        int value = CountPatternInPuzzle(input, textArrary, offsetPatterns, MatchType.Any);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var textArrary = "MAS".ToArray();
        IList<OffsetPoint[]> patterns = [
            [new(-1,-1), new(0,0), new(1,1)],
            [new(-1,1),  new(0,0), new(1,-1)]
        ];

        int value = CountPatternInPuzzle(input, textArrary, patterns, MatchType.All);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    private int CountPatternInPuzzle(char[][] puzzle, IEnumerable<char> characters, IList<OffsetPoint[]> offsetPatterns, MatchType matchType)
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
                var validPaths = GetValidPaths(new Point(x, y), offsetPatterns, bounds);

                switch (matchType)
                {
                    case MatchType.Any:
                        result += validPaths.Where(p => MatchesPattern(puzzle, characters, p)
                                                     || MatchesPattern(puzzle, characters.Reverse(), p))
                                    .Count();
                        break;
                    case MatchType.All:
                        if (validPaths.Count != offsetPatterns.Count)
                            break;
                        var matches = validPaths.All(p => MatchesPattern(puzzle, characters, p)
                                                     || MatchesPattern(puzzle, characters.Reverse(), p));
                        result += matches ? 1 : 0;
                        break;
                }
            }
        }

        return result;
    }

    private bool MatchesPattern(char[][] puzzle, IEnumerable<char> characters, Point[] searchPath)
    {
        var characterEnumerator = characters.GetEnumerator();
        for (int i = 0; i < searchPath.Length; i++)
        {
            var gridCharacter = puzzle[searchPath[i].Y][searchPath[i].X];
            if (!characterEnumerator.MoveNext())
            {
                return false;
            }
            var character = characterEnumerator.Current;
            if (gridCharacter != character)
            {
                return false;
            }
        }
        return true;
    }

    private IList<Point[]> GetValidPaths(Point origin, IList<OffsetPoint[]> offsetPatterns, Bounds bounds)
    {
        var x = offsetPatterns.Select(offsets => OffsetsToPath(origin, offsets))
                              .Where(path => path.All(p => IsInBounds(p, bounds)));
        return x?.ToList() ?? [];
    }

    private Point[] OffsetsToPath(Point origin, OffsetPoint[] offsets)
    {
        var path = new Point[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            path[i] = new Point(origin.X + offsets[i].xOffset,
                                origin.Y + offsets[i].yOffset);
        }
        return path;
    }

    private bool IsInBounds(Point point, Bounds bounds)
    {
        if (bounds.xLower > point.X || point.X > bounds.xUpper)
            return false;
        if (bounds.yLower > point.Y || point.Y > bounds.yUpper)
            return false;
        return true;
    }

    private static OffsetPoint[] CreatePatternArray(Point origin, int length, OffsetPoint step)
    {
        var xPathValues = CreateArray(origin.X, length, step.xOffset);
        var yPathValues = CreateArray(origin.Y, length, step.yOffset);

        var path = xPathValues.Zip(yPathValues)
                              .Select((pair) => new OffsetPoint(pair.First, pair.Second))
                              .ToArray();
        return path;
    }
    private static int[] CreateArray(int start, int length, int step)
    {
        int[] array = new int[length];
        for (var i = 0; i < length; i++)
        {
            array[i] = start + (step * i);
        }
        return array;
    }

    private enum MatchType { Any, All }
    private record Bounds(int xLower,
                          int xUpper,
                          int yLower,
                          int yUpper);

    public record OffsetPoint(int xOffset, int yOffset);
}
