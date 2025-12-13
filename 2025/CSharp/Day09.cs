using Point2D = (int x, int y);

namespace AdventOfCode.CSharp.Year2025;

public class Day09(ITestOutputHelper output)
{
    private const int DAY = 9;

    private (Point2D[] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .Select(l => l.Split(',').Select(int.Parse).ToArray())
                                      .Select(arr => new Point2D(arr[0], arr[1]))
                                      .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (redTilePoints, expected) = GetTestData(part, inputName);

        var value = FindLargestPossibleRectangleArea(redTilePoints);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    // [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (redTilePoints, expected) = GetTestData(part, inputName);

        var value = FindLargestPossibleRectangleAreaInPerimeter(redTilePoints);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }
    


    private long FindLargestPossibleRectangleArea(Point2D[] redTilePoints)
    {
        var result = GetAllRectangles(redTilePoints)
            .DistinctBy(IsDistinctBounds)
            .Max(CalculateArea);
        return result;
    }

    private long FindLargestPossibleRectangleAreaInPerimeter(Point2D[] redTilePoints)
    {
        var edges = redTilePoints.Windowed(2)
                                 .Select(window => (From: window[0], To: window[1]))
                                 .ToArray();
        var pairs = GetAllRectangles(redTilePoints)
            .DistinctBy(IsDistinctBounds)
            // .Select(bounds => (Bounds: bounds, Edges:
            //     new[]
            //     {
            //         (From: new Point2D(bounds.p1.x - 1, bounds.p1.y), To: new Point2D(bounds.p2.x + 1, bounds.p1.y)), // Top
            //         (From: new Point2D(bounds.p2.x, bounds.p1.y - 1), To: new Point2D(bounds.p2.x, bounds.p2.y + 1)), // Right
            //         (From: new Point2D(bounds.p1.x - 1, bounds.p2.y), To: new Point2D(bounds.p2.x + 1, bounds.p2.y)), // Bottom
            //         (From: new Point2D(bounds.p1.x, bounds.p1.y - 1), To: new Point2D(bounds.p1.x, bounds.p2.y + 1)), // Left
            //     }
            // ))
            .Where(bounds => bounds.p1 == (7, 3) && bounds.p2 == (11, 1) || bounds.p2 == (7, 3) && bounds.p1 == (11, 1)) // Debug filter
            .ToArray()
            // .Select(bounds => (Area: CalculateArea(bounds), Bounds: bounds))
            ;

        var result = 0L;
        return result;
    }

    private IEnumerable<(Point2D p1, Point2D p2)> GetAllRectangles(Point2D[] points)
        => points.SelectMany(p1 => points.Select(p2 => (p1, p2)));
    
    private (int x1, int y1, int x2, int y2) IsDistinctBounds((Point2D p1, Point2D p2) bounds)
        => ( x1: Math.Min(bounds.p1.x, bounds.p2.x),
             y1: Math.Min(bounds.p1.y, bounds.p2.y),
             x2: Math.Max(bounds.p1.x, bounds.p2.x),
             y2: Math.Max(bounds.p1.y, bounds.p2.y));
    private long CalculateArea((Point2D p1, Point2D p2) bounds)
        => (Math.Abs(bounds.p2.x - bounds.p1.x) + 1L)
         * (Math.Abs(bounds.p2.y - bounds.p1.y) + 1L);
    
}
