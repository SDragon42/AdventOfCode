namespace AdventOfCode.CSharp.Year2023;

using MapDictionary = Dictionary<string, Dictionary<char, string>>;

public class Day08
{
    private const int DAY = 8;

    private readonly ITestOutputHelper output;

    public Day08(ITestOutputHelper output) => this.output = output;



    private (char[] instructions, MapDictionary maps, int? expected) GetTestData(int part, string inputName)
    {
        var lines = Services.Input.ReadLines(DAY, inputName)
            .ToList();

        var instructions = lines[0].ToArray();
        var maps = new MapDictionary();

        foreach (var line in lines.Skip(2))
        {
            var lineSpan = line.AsSpan();
            var name = lineSpan[0..3];
            var left = lineSpan[7..10];
            var right = lineSpan[12..15];

            var subDict = new Dictionary<char, string>
            {
                { 'L', left.ToString() },
                { 'R', right.ToString() }
            };

            maps.Add(name.ToString(), subDict);
        }


        var expected = Services.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (instructions, maps, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "example2")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (instructions, maps, expected) = GetTestData(part, inputName);

        var value = FindNumberOfStepsToReachZZZ(instructions, maps);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }



    private int FindNumberOfStepsToReachZZZ(char[] instructions, MapDictionary map)
    {
        var stepCount = 0;

        var found = false;
        var current = "AAA";
        while (!found)
        {
            for (var i = 0; i < instructions.Length; i++)
            {
                var instruction = instructions[i];
                
                var next = map[current][instruction];
                current = next;
                stepCount++;

                if (current == "ZZZ")
                {
                    found = true;
                    break;
                }
            }
        }

        return stepCount;
    }
}