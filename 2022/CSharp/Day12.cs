using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode.CSharp.Year2022;

public class Day12_Hill_Climbing_Algorithm
{
    private const int DAY = 12;

    private readonly ITestOutputHelper output;
    public Day12_Hill_Climbing_Algorithm(ITestOutputHelper output) => this.output = output;



    private (char[][] input, Point start, Point end, int? expected) GetTestData(int part, string inputName)
    {
        var grid = InputHelper.ReadLines(DAY, inputName)
            .Select(x => x.ToArray())
            .ToArray();

        var start = default(Point);
        var end = default(Point);

        for (var row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == 'S')
                {
                    start = new Point(col, row);
                    grid[row][col] = 'a';
                }
                if (grid[row][col] == 'E')
                {
                    end = new Point(col, row);
                    grid[row][col] = 'z';
                }
            }
        }

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (grid, start, end, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (grid, start, end, expected) = GetTestData(part, inputName);

        var value = GetFewestNumberOfSteps(grid, start, end);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (grid, _, end, expected) = GetTestData(part, inputName);

        var value = GetFewestNumberOfStepsFromAnyLowPoint(grid, end);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetFewestNumberOfSteps(char[][] grid, Point start, Point end)
    {
        var visited = new HashSet<Point>();
        visited.Add(start);

        var checkingQueue = new Queue<(int numSteps, Point position)>();
        checkingQueue.Enqueue((0, start));

        while (checkingQueue.Count > 0)
        {
            var (steps, current) = checkingQueue.Dequeue();

            // Is this the end?
            if (current == end)
                return steps;

            foreach (var next in CalcEdgePoints(current))
            {
                // Is the next point out of bounds?
                if (next.Y < 0 || next.X < 0 || next.Y >= grid.Length || next.X >= grid[0].Length)
                    continue;

                // Is next step too high?
                if ((grid[next.Y][next.X] - grid[current.Y][current.X]) > 1)
                    continue;

                // Have we been here before?
                if (visited.Contains(next))
                    continue;

                // Add to the check queue
                checkingQueue.Enqueue((steps + 1, next));
                visited.Add(next);
            }

        }

        return -1;
    }

    private int GetFewestNumberOfStepsFromAnyLowPoint(char[][] grid, Point end)
    {
        var startingPoints = GetLowestPoints(grid);

        var result = startingPoints
            .Select(p => GetFewestNumberOfSteps(grid, p, end))
            .Where(s => s > 0)
            .Min();
        return result;
    }

    private IEnumerable<Point> CalcEdgePoints(Point point)
    {
        yield return new Point(point.X + 1, point.Y);
        yield return new Point(point.X - 1, point.Y);
        yield return new Point(point.X, point.Y + 1);
        yield return new Point(point.X, point.Y - 1);
    }

    private IEnumerable<Point> GetLowestPoints(char[][] grid)
    {
        for (var row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == 'a')
                    yield return new Point(col, row);
            }
        }
    }

}