namespace AdventOfCode.CSharp.Year2022;

public class Day14_Regolith_Reservoir
{
    private const int DAY = 14;

    private readonly ITestOutputHelper output;
    public Day14_Regolith_Reservoir(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

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

        var value = GetHowManySandUnits(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetHowManySandUnits(input, 2);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetHowManySandUnits(List<string> rockPath, int? floorOffset = null)
    {
        var source = new Point(500, 0);
        var grid = BuildScanMap(rockPath);

        var highestY = grid.Keys.Select(k => k.Y).Max();
        var floor = floorOffset.HasValue ? floorOffset.Value + highestY : floorOffset;
        if (floor.HasValue)
            highestY = floor.Value;

        var sandUnits = 0;
        var canRest = true;

        while (canRest)
        {
            if (grid.ContainsKey(source))
                break;

            var sand = source;
            var falling = true;
            while (falling && canRest)
            {
                var next = FallPoints(sand, floor)
                    .Where(p => !grid.ContainsKey(p))
                    .FirstOrDefault();

                falling = (next != Point.Empty);
                if (!falling)
                {
                    grid.Add(sand, Legend.Sand);
                    sandUnits++;
                    canRest = true;
                }
                else if (next.Y > highestY)
                {
                    canRest = false;
                }

                sand = next;
            }
        }

        return sandUnits;
    }

    void RenderGrid(IDictionary<Point, char> grid)
    {
        var text = GridHelper.DrawScreenGrid2D(grid,
            v => (v > 0 ? v : Legend.Air).ToString());
        output.WriteLine(text);
    }

    private IEnumerable<Point> FallPoints(Point sandPoint, int? floor)
    {
        var nextY = sandPoint.Y + 1;
        if (floor.HasValue && nextY >= floor.Value)
            yield break;

        yield return new Point(sandPoint.X, nextY);
        yield return new Point(sandPoint.X - 1, nextY);
        yield return new Point(sandPoint.X + 1, nextY);
    }

    private IDictionary<Point, char> BuildScanMap(List<string> rockPaths)
    {
        var map = new Dictionary<Point, char>();
        
        var rockPathPoints = rockPaths.Select(s => ConvertToPoints(s).ToArray());
        foreach (var rockPath in rockPathPoints)
        {
            foreach (var pairs in rockPath.Windowed(2))
            {
                foreach (var position in GetAllPointsBetween(pairs[0], pairs[1]))
                {
                    if (!map.ContainsKey(position))
                        map.Add(position, Legend.Rock);
                }
            }
        }

        return map;
    }
    private IEnumerable<Point> ConvertToPoints(string rockPath)
    {
        var pointPairs = rockPath.Split("->");
        foreach (var pair in pointPairs)
        {
            var values = pair.Split(',').Select(int.Parse).ToArray();
            yield return new Point(values[0], values[1]);
        }
    }

    public IEnumerable<Point> GetAllPointsBetween(Point start, Point end)
    {
        var xStep = GetStep(start.X, end.X);
        var yStep = GetStep(start.Y, end.Y);

        yield return start;
        while (start != end)
        {
            start.Offset(xStep, yStep);
            yield return start;
        }

        int GetStep(int a, int b)
        {
            var diff = b - a;
            return (diff == 0)
                ? 0
                : diff / Math.Abs(diff);
        }
    }

    static class Legend
    {
        public const char Rock = '#';
        public const char Air = '.';
        public const char Sand = 'o';
    }
}