namespace AdventOfCode.CSharp.Year2025;

public class Day04(ITestOutputHelper output)
{
    private const int DAY = 4;



    private (char[][] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .Select(l => l.ToCharArray())
                                      .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (grid, expected) = GetTestData(part, inputName);

        var value = GetAccessibleRolePositions(grid).Count();

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (grid, expected) = GetTestData(part, inputName);

        var value = 0;
        
        do
        {
            var positions = GetAccessibleRolePositions(grid).ToArray();
            if (positions.Length == 0)
                break;

            foreach (var pos in positions)
            {
                grid[pos.y][pos.x] = EMPTY; // Mark as inaccessible
                value++;
            }
        } while (true);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private const char PAPER_ROLL = '@';
    private const char EMPTY = '.';
    private readonly (int dx, int dy)[] directions =
        [
            (0, 1),  // down
            (1, 1),  // down-right
            (1, 0),  // right
            (1, -1), // up-right
            (0, -1), // up
            (-1, -1),// up-left
            (-1, 0), // left
            (-1, 1)  // down-left
        ];

    private IEnumerable<(int x, int y)> GetAccessibleRolePositions(char[][] grid)
    {
        var results = grid
            .SelectMany((row, y) => row
                .Select((cell, x) => (x, y))
                .Where(pos => grid[pos.y][pos.x] == PAPER_ROLL))
            .Where(pos => HasFewerThanNObstacles(grid, pos, 4));
        return results;
    }

    private bool HasFewerThanNObstacles(char[][] grid, (int x, int y) position, int n)
    {
        var results = GetAdjacentPositions(grid, position)
            .Count(pos => grid[pos.y][pos.x] == PAPER_ROLL);
        return results < n;
    }

    private IEnumerable<(int x, int y)> GetAdjacentPositions(char[][] grid, (int x, int y) position)
    {
        var adjacentPositions = directions
            .Select(d => (x: position.x + d.dx, y: position.y + d.dy))
            .Where(p => IsWithinBounds(grid, p));
        return adjacentPositions;
    }

    private bool IsWithinBounds(char[][] grid, (int x, int y) position)
    {
        return position.x >= 0 &&
               position.y >= 0 &&
               position.y < grid.Length &&
               position.x < grid[position.y].Length;
    }
}
