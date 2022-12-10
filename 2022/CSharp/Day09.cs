namespace AdventOfCode.CSharp.Year2022;

public class Day09_Rope_Bridge
{
    private const int DAY = 9;

    private readonly ITestOutputHelper output;
    public Day09_Rope_Bridge(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .ToList();

        var expected = InputHelper.ReadLines(DAY, $"{inputName}-answer{part}")
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

        var value = GetNumOfPositionsTheTailVisits(input, 2);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "example2")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetNumOfPositionsTheTailVisits(input, 10);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetNumOfPositionsTheTailVisits(List<string> input, int numKnots)
    {
        var visited = new HashSet<Point>();
        var knots = MakeKnots(numKnots);
        visited.Add(knots.First());

        var instructions = input.Select(ParseInstruction);
        foreach (var (dir, steps) in instructions)
        {
            for (int i = 0; i < steps; i++)
            {
                // Move head
                knots[0] = MoveKnot(knots[0], dir);

                // Move trailing knots
                for (int k = 1; k < knots.Count; k++)
                    knots[k] = MoveKnot(knots[k], knots[k - 1]);

                visited.Add(knots.Last());
            }
        }

        var result = visited.Count;
        return result;
    }

    private List<Point> MakeKnots(int numKnots)
    {
        var knots = new List<Point>();
        for (int i = 0; i < numKnots; i++)
            knots.Add(new Point());
        return knots;
    }


    private (Direction direction, int steps) ParseInstruction(string value)
    {
        var valueSpan = value.AsSpan();
        Enum.TryParse(valueSpan.Slice(0, 1), out Direction dir);
        var steps = int.Parse(valueSpan.Slice(2));
        return (dir, steps);
    }

    private enum Direction { U, D, L, R }

    private Point MoveKnot(Point knot, Direction direction) => direction switch
    {
        Direction.U => new Point(knot.X, knot.Y + 1),
        Direction.D => new Point(knot.X, knot.Y - 1),
        Direction.L => new Point(knot.X - 1, knot.Y),
        Direction.R => new Point(knot.X + 1, knot.Y),
        _ => throw new ApplicationException($"Invalid direction: {direction}")
    };

    private Point MoveKnot(Point knot, Point prevKnot)
    {
        var xOffset = prevKnot.X - knot.X;
        var yOffset = prevKnot.Y - knot.Y;

        var xDist = Math.Abs(xOffset);
        var yDist = Math.Abs(yOffset);

        xOffset = (xDist <= 1) ? xOffset : xOffset / 2;
        yOffset = (yDist <= 1) ? yOffset : yOffset / 2;

        if (xDist <= 1 && yDist <= 1)
            return knot;
        if (xDist > 1 && yDist == 0)
            return new Point(knot.X + xOffset, knot.Y);
        if (xDist == 0 && yDist > 1)
            return new Point(knot.X, knot.Y + yOffset);
        return new Point(knot.X + xOffset, knot.Y + yOffset);
    }

    //private string OutputPoints(List<Point> knots)
    //{
    //    var start = new Point(0, 0);
    //    var knotDict = new Dictionary<Point, string>();
    //    for (var i = 0; i < knots.Count; i++)
    //    {
    //        if (!knotDict.ContainsKey(knots[i]))
    //        {
    //            var value = (i > 0) ? i.ToString() : "H";
    //            knotDict.Add(knots[i], value);
    //        }
    //    }
    //    if (!knotDict.ContainsKey(start))
    //        knotDict.Add(start, "S");

    //    var result = Helper.DrawPointGrid2D(
    //        knotDict,
    //        (v) => v ?? "."
    //        , new Size(5, 5)
    //        );

    //    return result;
    //}

}
