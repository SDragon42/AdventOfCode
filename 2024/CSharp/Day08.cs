﻿namespace AdventOfCode.CSharp.Year2024;

public class Day08(ITestOutputHelper output)
{
    private const int DAY = 8;



    private (IDictionary<char, List<(int x, int y)>> antennaMap, int mapSize, int? expected) GetTestData(int part, string inputName)
    {
        var inputLines = InputHelper.ReadLines(DAY, inputName)
            .ToList();

        var mapSize = inputLines.Count;

        var antennaMap = inputLines.Select((line, y) => line.Select((frequency, x) => (location: (x, y), frequency)))
            .SelectMany(p => p)
            .Where(p => p.frequency != '.')
            .GroupBy(p => p.frequency, p => p.location)
            .Select(p => (p.Key, p.Select(x => x).ToList()))
            //;
            .ToDictionary();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (antennaMap, mapSize, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (frequencyMap, mapSize, expected) = GetTestData(part, inputName);

        var value = FindAntinodes(frequencyMap, mapSize)
            .Distinct()
            .Count();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    private IEnumerable<(int x, int y)> FindAntinodes(IDictionary<char, List<(int x, int y)>> frequencyMap, int mapSize)
    {
        //var foundAntinodes = new HashSet<(int x, int y)>();
        //foreach (var values in frequencyMap.Values)
        //    foreach (var loc1 in values)
        //        foreach (var loc2 in values.Where(l => l != loc1))
        //            foreach (var ap in FindAntinodes(loc1, loc2, mapSize))
        //                foundAntinodes.Add(ap);

        var foundAntinodes = frequencyMap
            .SelectMany(kvp => kvp.Value,
                        (kvp, loc1) => new { kvp.Value, loc1 })
            .SelectMany(item => item.Value
                .Where(loc2 => loc2 != item.loc1)
                .SelectMany(loc2 => FindAntinodes(item.loc1, loc2, mapSize)))
            .ToHashSet();
        return foundAntinodes;
    }

    private IEnumerable<(int x, int y)> FindAntinodes((int x, int y) loc1, (int x, int y) loc2, int mapSize)
    {
        var xDiff = loc1.x - loc2.x;
        var yDiff = loc1.y - loc2.y;

        var loc1Candidates = GetCandidates(loc1, (xDiff, yDiff));
        var loc2Candidates = GetCandidates(loc2, (xDiff, yDiff));

        var antinodes = loc1Candidates.Union(loc2Candidates)
                                      .Where(pt => pt != loc1)
                                      .Where(pt => pt != loc2)
                                      .Where(pt => IsInBounds(pt, mapSize));
        return antinodes;
    }


    private IEnumerable<(int x, int y)> GetCandidates((int x, int y) loc, (int x, int y) offset)
    {
        yield return (loc.x + offset.x, loc.y + offset.y);
        yield return (loc.x + (-1 * offset.x), loc.y + (-1 * offset.y));
    }

    private bool IsInBounds((int x, int y) loc, int mapSize)
    {
        return (0 <= loc.x && loc.x < mapSize)
            && (0 <= loc.y && loc.y < mapSize);
    }

}