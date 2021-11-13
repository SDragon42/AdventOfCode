namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/13
/// </summary>
class Day13 : PuzzleBase
{
    const int DAY = 13;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 13: Care Package";

        yield return string.Empty;
        yield return Run(Part1);

        yield return string.Empty;
        yield return Run(Part2);
    }

    string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));
    string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));



    class InputAnswer : IntCodeInputAnswer<long?> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer()
        {
            Input = InputHelper.LoadInputFile(DAY, name).ToList(),
            ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt64()
        };
        return result;
    }



    readonly Dictionary<Point, TileType> TileGrid = new Dictionary<Point, TileType>();
    readonly List<long> outputCache = new List<long>();
    long Score = 0;

    string RunPart1(InputAnswer puzzleData)
    {
        TileGrid.Clear();
        var arcade = new IntCode(puzzleData.Code);
        arcade.Output += ArcadeOutput;

        arcade.Run();

        // var grid = DisplayHelper.DrawPointGrid2D(TileGrid, DrawGridTitle);
        // Console.WriteLine(grid);

        var result = TileGrid.Values.Where(v => v == TileType.Brick).Count();

        return Helper.GetPuzzleResultText($"Number of block tiles are on the screen: {result}", result, puzzleData.ExpectedAnswer);
    }

    string RunPart2(InputAnswer puzzleData)
    {
        TileGrid.Clear();
        outputCache.Clear();
        Score = 0;

        var arcade = new IntCode(puzzleData.Code);
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

        return Helper.GetPuzzleResultText($"Final Score: {Score}", Score, puzzleData.ExpectedAnswer);
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
