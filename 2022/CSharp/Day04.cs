namespace AdventOfCode.CSharp.Year2022;

public class Day04_Camp_Cleanup
{
    private const int DAY = 4;

    private readonly ITestOutputHelper output;
    public Day04_Camp_Cleanup(ITestOutputHelper output) => this.output = output;


    private (List<RPair> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
            .Select(LineToRanges)
            .ToList();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }

    private RPair LineToRanges(string line)
    {
        var split = line.IndexOf(',');

        var r1 = line.AsSpan(0, split);
        var r2 = line.AsSpan(split + 1);

        var pair = new RPair()
        {
            A = MakeRange(r1),
            B = MakeRange(r2)
        };
        return pair;

        Range MakeRange(ReadOnlySpan<char> data)
        {
            var split = data.IndexOf('-');
            var a = int.Parse(data.Slice(0, split));
            var b = int.Parse(data.Slice(split + 1));
            return new Range(a, b);
        };
    }




    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetNumberOfFullyContainedPairs(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetNumberOfOverlappedPairs(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetNumberOfFullyContainedPairs(List<RPair> input)
    {
        var result = input
            .Where(IsFullyContained)
            .Count();
        return result;
    }

    private int GetNumberOfOverlappedPairs(List<RPair> input)
    {
        var result = input
            .Where(IsOverlapped)
            .Count();
        return result;
    }

    

    private bool IsFullyContained(RPair pair)
    {
        if (pair.A.Start.Value <= pair.B.Start.Value && pair.A.End.Value >= pair.B.End.Value)
            return true;
        if (pair.B.Start.Value <= pair.A.Start.Value && pair.B.End.Value >= pair.A.End.Value)
            return true;
        return false;
    }

    private bool IsOverlapped(RPair pair)
    {
        if (pair.A.Start.Value <= pair.B.Start.Value && pair.B.Start.Value <= pair.A.End.Value)
            return true;
        if (pair.A.Start.Value <= pair.B.End.Value && pair.B.End.Value <= pair.A.End.Value)
            return true;
        if (pair.B.Start.Value <= pair.A.Start.Value && pair.A.Start.Value <= pair.B.End.Value)
            return true;
        if (pair.B.Start.Value <= pair.A.End.Value && pair.A.End.Value <= pair.B.End.Value)
            return true;
        return false;
    }


    private record RPair
    {
        public required Range A { get; init; }
        public required Range B { get; init; }
    }
}

