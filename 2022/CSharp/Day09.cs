using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;

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
        var visited = new HashSet<Coordinate>();
        var head = new Coordinate() { X = 0, Y = 0 };
        var tail = new Coordinate() { X = 0, Y = 0 };
        visited.Add(tail);

        var instructions = input.Select(ParseInstruction);
        foreach (var (dir, steps) in instructions)
        {
            for (int i = 0; i < steps; i++)
            {
                var headWas = head;
                head = MoveHead(head, dir);
                tail = MoveTail(head, tail, headWas);
                visited.Add(tail);
            }
        }

        var result = visited.Count;
        return result;
    }


    private (Direction direction, int steps) ParseInstruction(string value)
    {
        var valueSpan = value.AsSpan();
        Enum.TryParse(valueSpan.Slice(0, 1), out Direction dir);
        var steps = int.Parse(valueSpan.Slice(2));
        return (dir, steps);
    }

    private enum Direction { U, D, L, R }

    private Coordinate MoveHead(Coordinate head, Direction direction) => direction switch
    {
        Direction.U => new Coordinate() { X = head.X, Y = head.Y + 1 },
        Direction.D => new Coordinate() { X = head.X, Y = head.Y - 1 },
        Direction.L => new Coordinate() { X = head.X - 1, Y = head.Y },
        Direction.R => new Coordinate() { X = head.X + 1, Y = head.Y },
        _ => throw new ApplicationException($"Invalid direction: {direction}")
    };

    private Coordinate MoveTail(Coordinate head, Coordinate tail, Coordinate headWas)
    {
        var xDist = Math.Abs(head.X - tail.X);
        var yDist = Math.Abs(head.Y - tail.Y);
        if (xDist <= 1 && yDist <= 1)
            return tail;
        return headWas;
        //throw new NotImplementedException();
    }

    private record Coordinate
    {
        public required int X { get; init; }
        public required int Y { get; init; }
    }
}