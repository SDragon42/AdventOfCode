namespace AdventOfCode.CSharp.Year2023;

public class Day06
{
    private const int DAY = 6;

    private readonly ITestOutputHelper output;
    public Day06(ITestOutputHelper output) => this.output = output;




    private record RaceData(string Time, string DistanceRecord);

    private (List<RaceData> input, int? expected) GetTestData(int part, string inputName)
    {
        var lines = Services.Input.ReadLines(DAY, inputName).ToArray();
        var times = SplitNumbers(lines[0], 1);
        var dists = SplitNumbers(lines[1], 1);

        List<RaceData> input = new();
        for (var i = 0; i < times.Length; i++)
            input.Add(new RaceData(times[i], dists[i]));

        var expected = Services.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }
    private static string[] SplitNumbers(string numbersText, int skipCount)
    {
        var result = numbersText
            .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Skip(skipCount)
            .ToArray();
        return result;
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = 1;
        input.Select(GetTheNumberOfWaysToBeatTheRecord)
            .ForEach(x => value *= x);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var raceData = MergeRaceData(input);
        var value = GetTheNumberOfWaysToBeatTheRecord(raceData);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }



    private int GetTheNumberOfWaysToBeatTheRecord(RaceData raceData)
    {
        var time = ulong.Parse(raceData.Time);
        var distanceRecord = ulong.Parse(raceData.DistanceRecord);

        var result = 0;
        for (ulong t = 0; t <= time; t++)
        {
            var distanceCovered = t * (time - t);
            if (distanceCovered > distanceRecord)
                result++;
        }

        return result;
    }

    private RaceData MergeRaceData(List<RaceData> input)
    {
        var result = new RaceData(
            string.Join(string.Empty, input.Select(x => x.Time)),
            string.Join(string.Empty, input.Select(x => x.DistanceRecord)));
        return result;
    }

}