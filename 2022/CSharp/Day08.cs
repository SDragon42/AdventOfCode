namespace AdventOfCode.CSharp.Year2022;

using IntGrid = List<List<int>>;
public class Day08_Treetop_Tree_House
{
    private const int DAY = 8;

    private readonly ITestOutputHelper output;
    public Day08_Treetop_Tree_House(ITestOutputHelper output) => this.output = output;



    private (IntGrid input, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
            .Select(l => l.AsEnumerable().Select(c => c - 48).ToList())
            .ToList();

        var expected = TestServices.Input.ReadLines(DAY, $"{inputName}-answer{part}")
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetHowManyTreesAreVisible(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetHighestSenicScore(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetHowManyTreesAreVisible(IntGrid input)
    {
        var result = CountPerimeterTrees(input) +
            CountInteriorTrees(input);
        return result;
    }

    private int CountPerimeterTrees(IntGrid grid)
    {
        var result = (grid.Count * 2) + (grid[0].Count * 2) - 4;
        return result;
    }

    private int CountInteriorTrees(IntGrid grid)
    {
        var (left, right, top, bottom) = GetOutterCheckingBounds(grid);
        var visibleTreeCoordinates = new HashSet<(int, int)>();
        
        MarkVisibleTrees(      // From the Top
            tallestTrees:      grid.First().ToList(),
            treeLineIndexes:   GetIndexes(left, right),
            treeDepthIndexes:  GetIndexes(top, bottom),
            getTreeCoordinate: (d, l) => (d, l));

        MarkVisibleTrees(      // From the Bottom
            tallestTrees:      grid.Last().ToList(),
            treeLineIndexes:   GetIndexes(left, right),
            treeDepthIndexes:  GetIndexes(bottom, top),
            getTreeCoordinate: (d, l) => (d, l));

        MarkVisibleTrees(      // From the Left
            tallestTrees:      grid.Select(g => g.First()).ToList(),
            treeLineIndexes:   GetIndexes(top, bottom),
            treeDepthIndexes:  GetIndexes(left, right),
            getTreeCoordinate: (d, l) => (l, d));

        MarkVisibleTrees(      // From the Right
            tallestTrees:      grid.Select(g => g.Last()).ToList(),
            treeLineIndexes:   GetIndexes(top, bottom),
            treeDepthIndexes:  GetIndexes(right, left),
            getTreeCoordinate: (d, l) => (l, d));

        return visibleTreeCoordinates.Count();

        void MarkVisibleTrees(List<int> tallestTrees, IEnumerable<int> treeLineIndexes, IEnumerable<int> treeDepthIndexes, Func<int, int, (int, int)> getTreeCoordinate)
        {
            foreach (var depthIdx in treeDepthIndexes)
            {
                foreach (var lineIdx in treeLineIndexes)
                {
                    var (row, col) = getTreeCoordinate(depthIdx, lineIdx);
                    var treeHeight = grid[row][col];
                    if (treeHeight > tallestTrees[lineIdx])
                    {
                        visibleTreeCoordinates.Add((row, col));
                        tallestTrees[lineIdx] = treeHeight;
                    }
                }
            }
        }
    }

    private (int left, int right, int top, int bottom) GetOutterCheckingBounds(IntGrid grid)
    {
        var left   = 1;
        var top    = 1;
        var right  = grid[0].Count - 2;
        var bottom = grid.Count - 2;
        return (left, right, top, bottom);
    }


    private int GetHighestSenicScore(IntGrid input)
    {
        var result = 0;
        for (int row = 0; row < input.Count; row++)
        {
            for (int col = 0; col < input[row].Count; col++)
            {
                var score = GetSenicScore(row, col, input);
                if (score > result)
                    result = score;
            }
        }
        return result;
    }


    private int GetSenicScore(int row, int col, IntGrid grid)
    {
        var height = grid[row][col];

        var left  = GetViewDistance(row, col, (r, c) => (r, c - 1));
        var right = GetViewDistance(row, col, (r, c) => (r, c + 1));
        var up    = GetViewDistance(row, col, (r, c) => (r - 1, c));
        var down  = GetViewDistance(row, col, (r, c) => (r + 1, c));

        return left * right * up * down;

        int GetViewDistance(int row, int col, Func<int, int, (int, int)> shiftFunc)
        {
            (row, col) = shiftFunc(row, col);
            if (!IsInBounds(row, col, grid))
                return 0;
            if (grid[row][col] < height)
                return 1 + GetViewDistance(row, col, shiftFunc);
            return 1;
        }
    }

    private bool IsInBounds(int row, int col, IntGrid grid)
    {
        if (row < 0 || (grid.Count - 1) < row)
            return false;
        if (col < 0 || (grid[row].Count - 1) < col)
            return false;
        return true;
    }

    private IEnumerable<int> GetIndexes(int start, int end)
    {
        var offset = (start <= end) ? 1 : -1;
        for (int i = start; i != end + offset; i = i + offset)
            yield return i;
    }
}