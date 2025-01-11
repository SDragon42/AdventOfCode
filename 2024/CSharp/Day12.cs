namespace AdventOfCode.CSharp.Year2024;

public class Day12(ITestOutputHelper output)
{
    private const int DAY = 12;



    private (IGrid<char> map, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                               .ToList();

        var map = new ArrayGrid<char>(input, c => c);

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                  ?.ToInt32();

        return (map, expected);
    }

    private readonly Size[] _moveDirections = [
        new(-1, 0),
        new(1, 0),
        new(0, -1),
        new(0, 1)
        ];



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "example2")]
    [InlineData(1, "example3")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (map, expected) = GetTestData(part, inputName);

        var regions = BreakIntoRegions(map).ToList();

        var value = regions.Select(r => (parimeter: r.Sum(a => a.sides), 
                                         area: r.Count()))
                           .Sum(r => r.parimeter * r.area);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    //[InlineData(2, "example2")]
    //[InlineData(2, "example3")]
    //[InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (map, expected) = GetTestData(part, inputName);

        var regions = BreakIntoRegions(map).ToList();

        var value = regions.Select(r => (parimeter: r.Sum(a => a.sides),
                                         area: r.Count()))
                           .Sum(r => r.parimeter * r.area);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private IEnumerable<IList<(Point point, int sides)>> BreakIntoRegions(IGrid<char> map)
    {
        var visited = new HashSet<Point>();

        for (int i = 0; i < map.Count; i++)
        {
            var point = map.IndexToPoint(i);
            if (visited.Contains(point))
            {
                continue;
            }

            var region = GetRegionBlocks(point, map, visited).ToList();
            yield return region;
        }
    }

    private IEnumerable<(Point point, int sides)> GetRegionBlocks(Point point, IGrid<char> map, HashSet<Point> visited)
    {
        if (visited.Contains(point))
        {
            yield break;
        }
        else
        {
            visited.Add(point);

            var plantType = map[point];
            var adjacentPoints = _moveDirections.Select(offset => point + offset)
                                                .Where(map.IsInBounds)
                                                .Where(p => map[p] == plantType)
                                                .ToList();
            var sides = 4 - adjacentPoints.Count;
            yield return (point, sides);

            foreach (var sidePoint in adjacentPoints)
            {
                var results = GetRegionBlocks(sidePoint, map, visited);
                foreach (var item in results)
                {
                    yield return item;
                }
            }
        }
    }

    //private IEnumerable<int> Get
}
