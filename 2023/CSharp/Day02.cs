namespace AdventOfCode.CSharp.Year2023;

using RGB = (int red, int green, int blue);

public class Day02
{
    private const int DAY = 2;

    private readonly ITestOutputHelper output;
    public Day02(ITestOutputHelper output) => this.output = output;



    private (List<(int id, List<RGB> plays)> input, int? expected) GetTestData(int part, string inputName)
    {
        var inputRegex = new Regex("Game (?<id>.*): (?<plays>.*)");
        var blueRegex  = new Regex("(?<num>.*) blue");
        var redRegex   = new Regex("(?<num>.*) red");
        var greenRegex = new Regex("(?<num>.*) green");

        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(ParseInput)
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);

        (int id, List<RGB>) ParseInput(string line)
        {
            var match = inputRegex.Match(line);
            if (!match.Success)
                throw new ApplicationException("Input line does not match the pattern");

            var id = match.Groups["id"].Value.ToInt32();
            var gameData = match.Groups["plays"].Value.Split(';');

            var colors = new List<RGB>();
            foreach (var play in gameData)
            {
                var blue = 0;
                var red = 0;
                var green = 0;

                foreach (var cubes in play.Split(','))
                {
                    red = GetNumCubes(redRegex, cubes) ?? red;
                    green = GetNumCubes(greenRegex, cubes) ?? green;
                    blue = GetNumCubes(blueRegex, cubes) ?? blue;
                }

                var item = (red, green, blue);
                colors.Add(item);
            }

            return (id, colors);
        }

        int? GetNumCubes(Regex regex, string data)
        {
            var match = regex.Match(data);
            var value = match.Success
                ? match.Groups["num"].Value.ToInt32() 
                : (int?)null;
            return value;
        }
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetAllValidGameIDs(input, 12, 13, 14)
            .Sum();

        output.WriteLine($"Answer: {value}");
        output.WriteLine("Too Low: 325");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetMinimumColorsForGames(input)
            .Select(a => a.redMinimum * a.blueMinimum * a.greenMinimum)
            .Sum();

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }



    private IEnumerable<int> GetAllValidGameIDs(List<(int id, List<RGB> plays)> input, int redMax, int greenMax, int blueMax)
    {
        foreach (var (id, plays) in input)
        {
            var totalRed = plays.Select(x => x.red).Max();
            var totalGreen = plays.Select(y => y.green).Max();
            var totalBlue = plays.Select(z => z.blue).Max();

            var isValid = totalRed <= redMax
                && totalGreen <= greenMax
                && totalBlue <= blueMax;

            if (isValid)
                yield return id;
        }
    }

    private IEnumerable<(int id, int redMinimum, int greenMinimum, int blueMinimum)> GetMinimumColorsForGames(List<(int id, List<RGB> plays)> input)
    {
        foreach (var (id, plays) in input)
        {
            yield return (
                id,
                plays.Select(x => x.red).Max(),
                plays.Select(y => y.green).Max(),
                plays.Select(z => z.blue).Max()
            );
        }
    }
}