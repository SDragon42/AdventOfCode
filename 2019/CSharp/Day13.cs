namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/13
/// </summary>
public class Day13 : TestBase
{
    public Day13(ITestOutputHelper output) : base(output, 13) { }


    private (List<long>, long?) GetTestData(string name, int part)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .First()
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault()
            ?.ToInt64();

        return (input, expected);
    }


    [Fact]
    public void Part1()
    {
        var (input, expected) = GetTestData("input", 1);

        var result = RunPart1(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Part2()
    {
        var (input, expected) = GetTestData("input", 2);

        var result = RunPart2(input);

        Assert.Equal(expected, result);
    }



    readonly Dictionary<Point, TileType> TileGrid = new Dictionary<Point, TileType>();
    readonly List<long> outputCache = new List<long>();
    long Score = 0;

    int RunPart1(List<long> code)
    {
        TileGrid.Clear();
        var arcade = new IntCode(code);
        arcade.Output += ArcadeOutput;

        arcade.Run();

        var result = TileGrid.Values.Where(v => v == TileType.Brick).Count();

        return result;
    }

    long RunPart2(List<long> code)
    {
        TileGrid.Clear();
        outputCache.Clear();
        Score = 0;

        var arcade = new IntCode(code);
        arcade.Output += ArcadeOutput;

        var ballPosition = GetTilePositions(TileType.Ball);
        var PaddlePosition = GetTilePositions(TileType.Paddle);

        arcade.Poke(0, 2); // Put in 2 quarters

        while (arcade.State != IntCodeState.Finished)
        {
            arcade.Run();
            if (arcade.State == IntCodeState.NeedsInput)
            {
                var diff = ballPosition.First().X - PaddlePosition.First().X;
                var inputValue = (diff == 0) ? 0 : diff / Math.Abs(diff);
                arcade.AddInput(inputValue);
            }
        }

        return Score;
    }

    IEnumerable<Point> GetTilePositions(TileType tileKind) =>
        TileGrid.Where(kv => kv.Value == tileKind).Select(kv => kv.Key);


    void ArcadeOutput(object sender, IntCodeOutputEventArgs e)
    {
        outputCache.Add(e.OutputValue);
        if (outputCache.Count < 3)
            return;

        var x = outputCache[0];
        var y = outputCache[1];
        if (x == -1 && y == 0)
        {
            Score = outputCache[2];
        }
        else
        {
            var location = new Point((int)x, (int)y);
            var tile = (TileType)outputCache[2];
            if (TileGrid.ContainsKey(location))
                TileGrid[location] = tile;
            else
                TileGrid.Add(location, tile);
        }

        outputCache.Clear();
    }

    string DrawGridTitle(TileType value) => value switch
    {
        TileType.Blank => " ",
        TileType.Wall => "#",
        TileType.Brick => "B",
        TileType.Paddle => "-",
        TileType.Ball => "*",
        _ => " "
    };

    enum TileType
    {
        Blank = 0,
        Wall = 1,
        Brick = 2,
        Paddle = 3,
        Ball = 4
    }

}
