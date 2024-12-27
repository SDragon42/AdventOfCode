namespace AdventOfCode.CSharp.Year2024;

public class Day10(ITestOutputHelper output)
{
    private const int DAY = 10;



    private (IGrid<int> map, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
                               .ToList();

        var grid = new ArrayGrid<int>(input, c => Convert.ToInt32(c) - 48);

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
                                  ?.ToInt32();

        return (grid, expected);
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
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (map, expected) = GetTestData(part, inputName);

        var trailHeads = map.Grid.Select((value, index) => (value, index))
                                 .Where(a => a.value == 0);

        var value = trailHeads.Sum(t => FindTrailheadEnds(t, map).Distinct().Count());

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example2")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (map, expected) = GetTestData(part, inputName);

        var trailHeads = map.Grid.Select((value, index) => (value, index))
                                 .Where(a => a.value == 0);

        var value = trailHeads.Sum(t => FindTrailheadEnds(t, map).Count());

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private IEnumerable<int> FindTrailheadEnds((int value, int index) position, IGrid<int> grid)
    {
        if (position.value == 9)
            yield return position.index;
        else
        {
            var possible = _moveDirections.Select(d => grid.IndexToPoint(position.index) + d)
                                          .Where(grid.IsInBounds)
                                          .Select(p => (value: grid[p], index: grid.PointToIndex(p)))
                                          .Where(pair => pair.value == position.value + 1);

            foreach (var item in possible)
            {
                var results = FindTrailheadEnds(item, grid);
                foreach (var value in results)
                    yield return value;
            }
        }
    }
}
