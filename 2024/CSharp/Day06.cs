namespace AdventOfCode.CSharp.Year2024;

public class Day06(ITestOutputHelper output)
{
    private const int DAY = 6;



    private (int mapSize, Point startPoint, IList<Point> obsticalLocations, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(x => x.ToArray())
            .ToArray();

        var mapSize = input.Length;

        var startIdx = input.SelectMany(m => m)
                            .Index()
                            .First(m => m.Item == '^')
                            .Index;
        var startPoint = GridHelper.IndexToPoint(startIdx, mapSize);

        var obsticalLocations = input.SelectMany(m => m)
                                     .Index()
                                     .Where(m => m.Item == '#')
                                     .Select(x => GridHelper.IndexToPoint(x.Index, mapSize))
                                     .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (mapSize, startPoint, obsticalLocations, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (mapSize, startPoint, obsticalLocations, expected) = GetTestData(part, inputName);

        var value = GetDistictPositionsVisited(mapSize, startPoint, obsticalLocations).Count();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private IEnumerable<Point> GetDistictPositionsVisited(int mapSize, Point startPoint, IEnumerable<Point> obsticals)
    {
        var visited = new HashSet<Point>();
        var direction = Direction.Up;
        var keepGoing = true;

        while (keepGoing)
        {
            var foundObsticals = GetAllInDirection(startPoint, obsticals, direction).ToList();

            if (foundObsticals.Count == 0)
            {
                keepGoing = false;
                foundObsticals.Add(GetOutOfBoundsPoint(startPoint, direction, mapSize));
            }

            var points = CreatePointsBetween(startPoint, foundObsticals.First());
            foreach (var p in points)
            {
                visited.Add(p);
            }

            startPoint = points.Last();
            direction = NextDirection(direction);
        }

        return visited;
    }

    private IEnumerable<Point> GetAllInDirection(Point origin, IEnumerable<Point> pointList, Direction direction)
    {
        var found = pointList.Where(SearchPredicate(origin, direction));
        return OrderPoints(found, direction);
    }

    private Func<Point, bool> SearchPredicate(Point origin, Direction direction) => direction switch
        {
            Direction.Up => (p) => p.X == origin.X && p.Y < origin.Y,
            Direction.Right => (p) => p.Y == origin.Y && p.X > origin.X,
            Direction.Down => (p) => p.X == origin.X && p.Y > origin.Y,
            Direction.Left => (p) => p.Y == origin.Y && p.X < origin.X,
            _ => throw new InvalidOperationException()
        };

    private IOrderedEnumerable<Point> OrderPoints(IEnumerable<Point> found, Direction direction) => direction switch
        {
            Direction.Up => found.OrderByDescending(p => p.Y),
            Direction.Left => found.OrderByDescending(p => p.X),
            Direction.Down => found.OrderBy(p => p.Y),
            Direction.Right => found.OrderBy(p => p.X),
            _ => throw new InvalidOperationException()
        };

    private Point GetOutOfBoundsPoint(Point origin, Direction direction, int mapSize) => direction switch
    {
        Direction.Up => new Point(origin.X, -1),
        Direction.Right => new Point(mapSize, origin.Y),
        Direction.Down => new Point(origin.X, mapSize),
        Direction.Left => new Point(-1, origin.Y),
        _ => throw new InvalidOperationException()
    };

    private Direction NextDirection(Direction direction) => direction switch
    {
        Direction.Up => Direction.Right,
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        _ => throw new InvalidOperationException()
    };

    private IEnumerable<Point> CreatePointsBetween(Point origin, Point end)
    {
        var xRange = CreateArray(origin.X,
                                 Math.Max(1, Math.Abs(end.X - origin.X)),
                                 origin.X > end.X ? -1 : 1);
        var yRange = CreateArray(origin.Y,
                                 Math.Max(1, Math.Abs(end.Y - origin.Y)),
                                 origin.Y > end.Y ? -1 : 1);

        foreach (var x in xRange)
        {
            foreach (var y in yRange)
            {
                yield return new Point(x, y);
            }
        }
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


    private enum Direction { Up, Right, Down, Left }
}
